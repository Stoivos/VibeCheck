import { useEffect, useState } from "react";
import { connection } from "../Services/crowdHub";
import { useLocation } from "./useLocation";

// type for usestate of crowd updates.
export type CrowdUpdate = {
    placeId: number;
    placeName: string;
    count: number;
};

export function useCrowdHub() {
    const [crowd, setCrowd] = useState<CrowdUpdate[]>([]);
    const { position } = useLocation();

    const sessionId = crypto.randomUUID();

    useEffect(() => {

        const start = async () => {
            try {
                if (connection.state === "Disconnected") {
                    await connection.start();
                }
                console.log("SignalR connected");

                // Listeners to hub events.
                connection.on("ReceiveCrowdUpdate", (data: CrowdUpdate) => {
                    console.log("RAW DATA:", data);
                    setCrowd(prev => {
                        const filtered = prev.filter(x => x.placeId !== data.placeId);
                        return [...filtered, data];
                    });
                });

            } catch (err) {
                console.error(err);
            }
        };

        start();

        return () => {
            connection.off("ReceiveCrowdUpdate");
        };
    }, []);

    // Invokes backend.
    useEffect(() => {
        if (!position) return;

        const interval = setInterval(() => {
            connection.invoke(
                "SendPosition",
                sessionId,
                position.latitude,
                position.longitude
            );
        }, 5000);

        return () => clearInterval(interval);

    }, [position]);

    return { crowd };
}