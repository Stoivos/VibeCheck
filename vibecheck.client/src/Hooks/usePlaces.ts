import { useEffect, useState } from "react";
export function usePlaces() {
    const [places, setPlaces] = useState([]);

    useEffect(() => {
        const fetchPlaces = async () => {
            const res = await fetch("http://localhost:5292/api/places");
            const data = await res.json();
            setPlaces(data);
        };

        fetchPlaces();
    }, []);

    return { places };
}