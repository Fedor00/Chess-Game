import styles from "./Player.module.css";
function Player({ name }) {
   return <p className={styles.player}>{name} </p>;
}

export default Player;
