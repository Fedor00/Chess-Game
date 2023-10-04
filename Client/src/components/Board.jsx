/* eslint-disable react/prop-types */
import { useAuth } from "../contexts/AuthContext";
import styles from "./Board.module.css";
import Cell from "./Cell";
import Player from "./Player";

function Board({ game, makeMove }) {
   const { user } = useAuth();

   const shouldRevertBoard = user.email === game.topPlayerEmail;

   const revertedBoard = () => {
      const newBoard = [...game.board].reverse();
      return newBoard.map((row) => [...row].reverse());
   };

   const boardToRender = shouldRevertBoard ? revertedBoard() : game.board;

   return (
      <div className={styles.chessboard}>
         <Player name={game?.topPlayerName || "Waiting"}></Player>

         {boardToRender.map((row, rowIndex) => (
            <div className={styles.row} key={rowIndex}>
               {row.map((cell, cellIndex) => (
                  <Cell
                     key={cellIndex}
                     cell={cell}
                     rowIndex={rowIndex}
                     cellIndex={cellIndex}
                     makeMove={makeMove}
                  />
               ))}
            </div>
         ))}
         <Player name={game?.bottomPlayerName || "Waiting"}></Player>
      </div>
   );
}

export default Board;
