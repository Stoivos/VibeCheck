import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";
import "leaflet/dist/leaflet.css";
import L from "leaflet";

import BottomNav from "../Components/BottomNav";
import { usePlaces } from "../Hooks/usePlaces";
import { useCrowdHub } from "../Hooks/useCrowdHub";
import { useMapData } from "../Hooks/useMapData";

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

    return (
        <div className="map-page">
            <div className="map-header">
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
                        attribution='&copy; OpenStreetMap contributors'
                        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                    />

                    {mapData.map(place => (
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
                        </Marker>
                    ))}
                </MapContainer>
            </div>

            <BottomNav />
        </div>
    );
}

export default Map;