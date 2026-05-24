import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";
import { useState } from "react";
import "leaflet/dist/leaflet.css";
import L from "leaflet";

import BottomNav from "../Components/BottomNav";
import { usePlaces } from "../Hooks/usePlaces";
import { useCrowdHub } from "../Hooks/useCrowdHub";
import { useMapData } from "../Hooks/useMapData";
import { useLocation } from "../Hooks/useLocation";

import "./Map.css";

function Map() {
    const { places } = usePlaces();
    const { crowd } = useCrowdHub();

    const mapData = useMapData(places, crowd);

    const barIcon = new L.Icon({
        iconUrl: "/images/bar-icon.png",
        iconSize: [45, 45],
        iconAnchor: [15, 30],
    }); 

    const { position } = useLocation();

    const userIcon = new L.Icon({
        iconUrl: "/images/user-icon.png",
        iconSize: [40, 40], 
        iconAnchor: [20, 20],
    });

    const [search, setSearch] = useState("");

    const filteredMapData = mapData.filter(p =>
        p.name.toLowerCase().includes(search.toLowerCase())
    );

    return (
        <div className="map-page">
            <div className="map-header">
                <div className="map-search">
                    <img src="/images/search-icon-white.png" alt="" className="search-icon" />
                    <input
                        type="text"
                        placeholder="Sök krog..."
                        value={search}
                        onChange={e => setSearch(e.target.value)}
                    />
                </div>
                <h2>Karta</h2>
            </div>

            <div className="map-container">
                <MapContainer
                    center={[63.8258, 20.2630]}
                    zoom={14}
                    scrollWheelZoom={true}
                    style={{ height: "100%", width: "100%" }}
                >
                    <TileLayer
                        attribution='&copy; <a href="https://stadiamaps.com/">Stadia Maps</a>'
                        url="https://tiles.stadiamaps.com/tiles/alidade_smooth/{z}/{x}/{y}{r}.png"
                    />

                    {filteredMapData.map(place => (
                        <Marker
                            key={place.id}
                            position={[place.latitude, place.longitude]}
                            icon={barIcon}
                        >
                            <Popup>
                                <b>{place.name}</b>
                                <br />
                                {place.count} personer här
                            </Popup>

                            {position && (
                                <Marker
                                    position={[position.latitude, position.longitude]}
                                    icon={userIcon}
                                >
                                    <Popup>Du är här</Popup>
                                </Marker>
                            )}
                        </Marker>

                    ))}
                </MapContainer>
            </div>

            <BottomNav />
        </div>
    );
}

export default Map;