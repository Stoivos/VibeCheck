import { useEffect, useState } from "react";
import { connection } from "../Services/crowdHub";

// type for usestate of crowd updates.
export type CrowdUpdate = {
    placeId: number;
    placeName: string;
    count: number;
};

export function useCrowdHub() {
    const [crowd, setCrowd] = useState<CrowdUpdate[]>([]);

    useEffect(() => {

        const start = async () => {
            try {
                await connection.start();
                console.log("SignalR connected");

                connection.on("CrowdUpdate", (data) => {
                    console.log("update:", data);

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
            connection.off("CrowdUpdate");
        };
    }, []);

    return { crowd, connection };
}