/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect } from "react";
import Board from "../components/Board";
import { useAuth } from "../contexts/AuthContext";
import useChessGame from "../hooks/useGame";
import styles from "./Play.module.css";
import Checkmate from "../components/Checkmate";
import { Button } from "reactstrap";

function Play() {
   const { user, logout } = useAuth();
   const { game, setGame, connection, fetchCurrentGame, makeMove, randomGame } =
      useChessGame(user);
   useEffect(() => {
      if (connection) fetchCurrentGame();
   }, [connection]);
   useEffect(() => {
      if (connection) {
         const refreshGameHandler = (gameDto) => {
            setGame(gameDto);
            connection
               .invoke("JoinGameGroup", gameDto.id.toString())
               .catch((error) =>
                  console.error("Failed to join game group:", error)
               );
         };

         const moveMadeHandler = (gameDto) => {
            setGame(gameDto);
         };

         const reconnectedHandler = () => {
            if (game) {
               connection
                  .invoke("JoinGameGroup", game.id.toString())
                  .catch((error) =>
                     console.error("Failed to rejoin game group:", error)
                  );
            }
         };

         connection.on("RefreshGame", refreshGameHandler);
         connection.on("MoveMade", moveMadeHandler);
         connection.onreconnected(reconnectedHandler);

         // Cleanup
         return () => {
            connection.off("RefreshGame", refreshGameHandler);
            connection.off("MoveMade", moveMadeHandler);
            connection.off("reconnected", reconnectedHandler);
         };
      }
   }, [connection, game, setGame]);

   return (
      <>
         <div className={styles.container}>
            <div>
               <div>
                  <Button onClick={logout}> Logout</Button>
                  <Button onClick={randomGame}> Random Game</Button>
               </div>
               {game && <Board game={game} makeMove={makeMove} />}
               {game && <p>STATUUUUUS:{game.status}</p>}

               <Checkmate game={game} />
            </div>
         </div>
      </>
   );
}

export default Play;
