import { BrowserRouter, Routes, Route } from "react-router-dom";
import Login from "./pages/Login";
import PageNotFound from "./pages/PageNotFound";
import { AuthProvider } from "./contexts/AuthContext";
import { ToastContainer } from "react-toastify";
import Homepage from "./pages/Homepage";
import Play from "./pages/Play";
import "./App.css";
import ProtectedRoute from "./components/ProtectedRoute";
import "bootstrap/dist/css/bootstrap.min.css";

function App() {
   return (
      <div className="appBackground">
         <BrowserRouter>
            <ToastContainer />
            <AuthProvider>
               <Routes>
                  <Route index element={<Homepage />}></Route>
                  <Route path="login" element={<Login />} />
                  <Route
                     path="play"
                     element={
                        <ProtectedRoute>
                           <Play />
                        </ProtectedRoute>
                     }
                  />
                  <Route path="*" element={<PageNotFound />} />
               </Routes>
            </AuthProvider>
         </BrowserRouter>
      </div>
   );
}

export default App;
