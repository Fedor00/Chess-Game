/* eslint-disable react-refresh/only-export-components */
import { createContext, useContext, useEffect, useReducer } from "react";
import { useNavigate } from "react-router-dom";

const AuthContext = createContext();
const savedUser = JSON.parse(localStorage.getItem("user"));
import { toast } from "react-toastify";
const initialState = {
   user: savedUser,
   isAuthenticated: Boolean(savedUser),
};
console.log(initialState.isAuthenticated);
function reducer(state, action) {
   const savedUserString = localStorage.getItem("user");
   const savedUser = savedUserString ? JSON.parse(savedUserString) : null;

   switch (action.type) {
      case "login":
         localStorage.setItem("user", JSON.stringify(action.payload));

         return { ...state, user: action.payload, isAuthenticated: true };
      case "logout":
         localStorage.removeItem("user");

         return { ...state, user: null, isAuthenticated: false };
      case "initialize":
         return {
            ...state,
            user: savedUser,
            isAuthenticated: Boolean(savedUser),
         };
      default:
         throw new Error("Action unknown");
   }
}

function AuthProvider({ children }) {
   const [{ user, isAuthenticated }, dispatch] = useReducer(
      reducer,
      initialState
   );
   const navigate = useNavigate();
   useEffect(() => {
      dispatch({ type: "initialize" });
   }, []);
   async function login(email, password) {
      try {
         const response = await fetch(
            "http://localhost:5240/api/account/login",
            {
               method: "POST",
               headers: {
                  "Content-Type": "application/json",
               },
               body: JSON.stringify({ email, password }),
            }
         );

         const data = await response.json();
         if (!response.ok) throw new Error(data.message || "Failed to login");
         console.log(data);
         dispatch({ type: "login", payload: data }); // Assuming the API returns a user object
      } catch (error) {
         toast.error(error.message);
      }
   }

   function logout() {
      dispatch({ type: "logout" });
      navigate("/login");
   }
   return (
      <AuthContext.Provider value={{ user, isAuthenticated, login, logout }}>
         {children}
      </AuthContext.Provider>
   );
}

function useAuth() {
   const context = useContext(AuthContext);
   if (context === undefined)
      throw new Error("AuthContext was used outside AuthProvider");
   return context;
}
export { AuthProvider, useAuth };
