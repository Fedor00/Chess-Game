import { useState } from "react";
import Header from "../components/Header";
import { useAuth } from "../contexts/AuthContext";
import { NavLink } from "react-router-dom";
import ChessNav from "../components/ChessNav";

function Chess() {
   const { logout } = useAuth();
   return <ChessNav />;
}

export default Chess;
