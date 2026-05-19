import { useEffect, useState } from "react";

type Position = {
    latitude: number;
    longitude: number;
    accuracy: number;
};

export function useLocation() {
    const [position, setPosition] = useState<Position | null>(null);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {

        if (!navigator.geolocation) {
            console.log("Geolocation not supported");
            return;
        }

        // get current position from client.
        const watchId = navigator.geolocation.watchPosition(
            (pos) => {
                setPosition({
                    latitude: pos.coords.latitude,
                    longitude: pos.coords.longitude,
                    accuracy: pos.coords.accuracy
                });

                setError(null);
            },
            (err) => {
                setError(err.message);
            },
            {
                enableHighAccuracy: true,
                maximumAge: 5000,
                timeout: 10000
            }
        );

        return () => {
            navigator.geolocation.clearWatch(watchId);
        };

    }, []);

    return { position, error };
}