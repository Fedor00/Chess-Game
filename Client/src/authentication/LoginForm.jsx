import { useEffect, useState } from "react";
import { useAuth } from "../contexts/AuthContext";
import { useNavigate } from "react-router-dom";
import styles from "./LoginForm.module.css";
function LoginForm() {
   const [email, setEmail] = useState("");
   const [password, setPassword] = useState("");
   const { login, isAuthenticated } = useAuth();
   const navigate = useNavigate();
   const handleSubmit = (e) => {
      e.preventDefault();
      if (email || password) login(email, password);
   };
   useEffect(
      function () {
         if (isAuthenticated) navigate("/play");
      },
      [isAuthenticated, navigate]
   );
   return (
      <form onSubmit={handleSubmit} className={styles.loginForm}>
         <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
         ></input>
         <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
         ></input>
         <button>Login</button>
      </form>
   );
}

export default LoginForm;
