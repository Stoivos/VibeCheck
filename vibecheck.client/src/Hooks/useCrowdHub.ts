import { useEffect, useState, useRef, useMemo } from "react";
import { connection } from "../Services/crowdHub";
import { useLocation } from "./useLocation";

// type for usestate of crowd updates.
export type CrowdUpdate = {
    placeId: number;
    placeName: string;
    count: number;
    imageUrl: string;
};

export function useCrowdHub() {
    const [crowd, setCrowd] = useState<CrowdUpdate[]>([]);
    const { position } = useLocation();

    const sessionId = useRef(crypto.randomUUID()).current;

    useEffect(() => {

        const start = async () => {
            try {
                if (connection.state === "Disconnected") {
                    await connection.start();
                }
                console.log("SignalR connected");

                //cleanup before
                connection.off("ReceiveCrowdUpdate");
                // Listeners to hub events.
                connection.on("ReceiveCrowdUpdate", (data: CrowdUpdate) => {
                    console.log("Received crowd update:", data);
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

        const interval = setInterval(async () => {
            if (connection.state !== "Connected") {
                console.warn("SignalR inte connected, state:", connection.state);
                return;
            }

            try {
                await connection.invoke(
                    "SendPosition",
                    sessionId,
                    position.latitude,
                    position.longitude
                );
            } catch (err) {
                console.error("Invoke-fel:", err);
            }
        }, 5000);

        return () => clearInterval(interval);

    }, [position]);

    // Sort crowd by count (most to least)
    const sortedCrowd = useMemo(() => {
        return [...crowd].sort((a, b) => b.count - a.count);
    }, [crowd]);

    return { crowd: sortedCrowd };
}