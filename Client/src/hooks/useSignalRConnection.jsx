import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

function useSignalRConnection(url) {
   const [connection, setConnection] = useState(null);

   useEffect(() => {
      const connect = new signalR.HubConnectionBuilder()
         .withUrl(url, {
            withCredentials: true,
         })
         .configureLogging(signalR.LogLevel.Information)
         .withAutomaticReconnect()
         .build();

      const startConnection = async () => {
         try {
            await connect.start();
            console.log("connection started");
            setConnection(connect);
         } catch (error) {
            console.error("Failed to connect:", error);
         }
      };

      startConnection();

      return () => {
         if (connection) {
            connection.stop();
         }
      };
   }, [url]);

   return {
      connection: connection,
   };
}

export default useSignalRConnection;
