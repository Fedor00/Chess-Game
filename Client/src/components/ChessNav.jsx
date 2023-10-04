import { NavLink } from "react-router-dom";

function ChessNav() {
   return (
      <ul>
         <li>
            <NavLink to="/play">Play</NavLink>
         </li>
      </ul>
   );
}

export default ChessNav;
