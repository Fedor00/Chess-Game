import { NavLink } from "react-router-dom";
import styles from "./Homepage.module.css";
function Homepage() {
   return (
      <nav className={styles.nav}>
         <ul>
            <li>
               <NavLink to="/login" className={styles.ctaLink}>
                  Login
               </NavLink>
               <NavLink to="/register" className={styles.ctaLink}>
                  Register
               </NavLink>
            </li>
         </ul>
      </nav>
   );
}

export default Homepage;
