import styles from "./Cell.module.css";

function Cell({ cell, rowIndex, cellIndex, makeMove }) {
   function onDragStart(event) {
      const position = {
         rank: rowIndex,
         file: cellIndex,
      };
      event.dataTransfer.setData("text/plain", JSON.stringify(position));
   }

   function onDrop(event) {
      const fromPosition = JSON.parse(event.dataTransfer.getData("text/plain"));
      const toPosition = {
         rank: rowIndex,
         file: cellIndex,
      };
      makeMove(fromPosition, toPosition);
   }

   function allowDrop(event) {
      event.preventDefault();
   }

   function getImagePathForPiece(piece) {
      const color = piece.pieceColor === 1 ? "white" : "black";
      const pieceSymbol =
         color === "white" ? piece.symbol : piece.symbol.toLowerCase();
      return `/images/${color}_${pieceSymbol}.png`;
   }

   return (
      <div
         className={`${styles.cell} ${
            (rowIndex + cellIndex) % 2 === 0 ? styles.white : styles.black
         }`}
         onDrop={onDrop}
         onDragOver={allowDrop}
      >
         {cell && (
            <img
               src={getImagePathForPiece(cell)}
               draggable="true"
               onDragStart={onDragStart}
               alt={cell.symbol}
               className={styles.img}
            />
         )}
      </div>
   );
}

export default Cell;
