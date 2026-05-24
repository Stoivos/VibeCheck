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

    const [myPlace, setMyPlace] = useState<{ placeId: number; placeName: string } | null>(null);

    const [isConnecting, setIsConnecting] = useState(true);

    const { position } = useLocation();

    const sessionId = useRef(
        localStorage.getItem("sessionId") ?? (() => {
            const id = crypto.randomUUID();
            localStorage.setItem("sessionId", id);
            return id;
        })()
    ).current;

    const positionRef = useRef(position);

    const alreadySentRef = useRef(false);

    // Loading trigger: Connected + has position + has crowd data.
    const isReady = !isConnecting && !!position && crowd.length > 0;

    // Keep ref updated
    useEffect(() => {
        positionRef.current = position;
    }, [position]);

    useEffect(() => {
        const start = async () => {
            try {
                if (connection.state === "Disconnected") {
                    await connection.start(); // connection done here.               
                }

                // for loading screen.
                setIsConnecting(false);

                // Listen for updates on user's place.
                connection.off("YourPlace");
                connection.on("YourPlace", (data) => {
                    console.log("YourPlace mottagen:", data);
                    setMyPlace(data);
                });

                // Listen for crowd updates.
                connection.off("ReceiveCrowdUpdate");
                connection.on("ReceiveCrowdUpdate", (data: CrowdUpdate) => {
                    setCrowd(prev => {
                        const existing = prev.find(x => x.placeId === data.placeId);
                        if (existing && existing.count === data.count) return prev; // No rerender if count hasn't changed.
                        const filtered = prev.filter(x => x.placeId !== data.placeId);
                        return [...filtered, data];
                    });
                });

                // If position is found, send it immediately.
                if (positionRef.current) {
                    await connection.invoke("SendPosition", sessionId, positionRef.current.latitude, positionRef.current.longitude);
                    alreadySentRef.current = true;
                }

            } catch (err) {
                console.error(err);
            }
        };

        start();
        return () => {
            connection.off("ReceiveCrowdUpdate");
            connection.off("YourPlace");
        };
    }, []);

    // Invokes backend if position changes, every 5 seconds.
    useEffect(() => {
        if (!position) return;

        // If start missed the initial position send, send it immediately.
        const init = async () => {

            if (alreadySentRef.current) return;

            if (connection.state === "Connected") {
                try {
                    await connection.invoke("SendPosition", sessionId, position.latitude, position.longitude);
                } catch (err) {
                    console.error("Invoke-fel:", err);
                }
            }
        };

        init();

        // Update position every 5 seconds.

        const interval = setInterval(async () => {
            if (connection.state !== "Connected") return;
            try {
                await connection.invoke("SendPosition", sessionId, position.latitude, position.longitude);
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

    return { crowd: sortedCrowd, myPlace, isReady };
}