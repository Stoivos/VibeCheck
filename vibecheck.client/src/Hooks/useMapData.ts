import { useMemo } from "react";

export function useMapData(places, crowd) {
    return useMemo(() => {
        return places.map(p => {
            const live = crowd.find(c => c.placeId === p.id);

            return {
                ...p,
                count: live?.count ?? 0
            };
        });
    }, [places, crowd]);
}