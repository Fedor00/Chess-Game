import { useState } from "react";
import {
   makeMoveApi,
   startRandomGameApi,
   getCurrentGameApi,
} from "../services/apiChessGame";
import useSignalRConnection from "./useSignalRConnection";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
function useChessGame(user) {
   const [game, setGame] = useState(null);
   const [loading, setLoading] = useState(false);
   const [error, setError] = useState(null);
   const { connection } = useSignalRConnection(
      "http://localhost:5240/chessGameHub"
   );

   const handleApiError = (err) => {
      console.log(err.message);
      console.error("API Error:", err);
      setError(
         () => `Error: ${err.message || "An unexpected error occurred."}`
      );
      toast.error(`Error: ${err.message || "An unexpected error occurred."}`, {
         autoClose: 2000,
      });
   };

   const fetchCurrentGame = async () => {
      setLoading(true);
      setError(null);
      try {
         const currentGame = await getCurrentGameApi(user.token);
         setGame(currentGame);
         if (connection && currentGame) {
            connection
               .invoke("JoinGameGroup", currentGame.id.toString())
               .catch((error) => handleApiError(error));
         }
      } catch (error) {
         handleApiError(error);
      } finally {
         setLoading(false);
      }
   };

   const randomGame = async () => {
      setError(null);
      setLoading(true);
      try {
         const gameData = await startRandomGameApi(user.token);
         setGame(gameData);
         if (connection) {
            connection
               .invoke("JoinGameGroup", gameData.id.toString())
               .catch((error) => handleApiError(error));
         }
      } catch (error) {
         handleApiError(error);
      } finally {
         setLoading(false);
      }
   };

   const makeMove = async (from, to) => {
      const move = { from, to, gameId: game.id };
      setError(null);
      setLoading(true);
      try {
         const updatedGame = await makeMoveApi(move, user.token);
         if (game !== updatedGame) setGame(updatedGame);
      } catch (error) {
         handleApiError(error);
      } finally {
         setLoading(false);
      }
   };

   return {
      game,
      setGame,
      loading,
      setLoading,
      connection,
      fetchCurrentGame,
      randomGame,
      makeMove,
      error, 
      setError, 
   };
}

export default useChessGame;
