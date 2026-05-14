import { useEffect } from "react";
import * as signalR from "@microsoft/signalr";
import "./Start.css";

function Start() {
    
    useEffect(() => {

        const conn = new signalR.HubConnectionBuilder()
            .withUrl("/crowdhub")
            .withAutomaticReconnect()
            .build();

        const startConnection = async () => {
            try {
                await conn.start();
                console.log("SignalR connected!");

                conn.on("Connected", (msg) => {
                    console.log("From server:", msg);
                });

            } catch (err) {
                console.error("Connection failed:", err);
            }
        };

        startConnection();

        return () => {
            conn.stop();
        };
    }, []);


    // Html and UI.
    return (

        <p>Hello world!</p>

    );
}

export default Start;