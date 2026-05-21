// import { useCrowdHub } from "../Hooks/useCrowdHub";
import BottomNav from "../Components/BottomNav";
import "./Map.css";

function Map() {
    // const { crowd } = useCrowdHub();

    return (
        <div className="map-page">
            <div className="map-header">
                <h2>Karta</h2>
            </div>
            <div className="map-container">
                {/* Leaflet kommer här */}
            </div>
            <BottomNav />
        </div>
    );
}

export default Map;