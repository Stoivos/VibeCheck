import { useCrowdHub } from "../Hooks/useCrowdHub";
import { useLocation } from "../Hooks/useLocation";
import "./Start.css";

function Start() {

    const { crowd } = useCrowdHub();
    const { position, error } = useLocation();

    const hasLocation = !!position;
    const blocked = !!error;

return (
    <div>
        <h1>Vibecheck</h1>

        {/*Show popup if location is not available and not blocked*/}

        {!hasLocation && !blocked && (
            <div className="popup">
                <h2>För att börja vibechecka</h2>
                <p>Behöver du tillåta platsinfo</p>
            </div>
        )}

        {/*Show popup if location access is blocked*/}

        {blocked && (
            <div className="popup error">
                <h2>Plats åtkomst nekad</h2>
                <p>Slå på location i din browser för att använda Vibecheck</p>
            </div>
        )}

        {/*sucess*/}
        {hasLocation && (
            <div>

                <div className="logo">
                    LOGGA
                </div>

                <h2 style={{ textAlign: "center" }}>Live crowd</h2>

                {/* List of places */}
                <div className="places">
                    {crowd.map((p) => (
                        <div key={p.placeId} className="place-card">

                            {/* Image section */}
                            <div className="place-image" style={{ backgroundImage: `url(${p.imageUrl})` }}>
                                {p.placeName.toUpperCase()}
                            </div>

                            {/* Content */}
                            <div className="place-content">
                                <h3>{p.placeName}</h3>

                                <div className="place-footer">

                                    <div className="user-count">
                                        <div className="user-icon" />
                                        <span>{p.count}</span>
                                    </div>

                                    <span>people here</span>
                                </div>
                            </div>

                        </div>
                    ))}
                </div>

                {/* Location */}
                <div className="location">
                    <p>Din position:</p>
                    <p>
                        {position.latitude.toFixed(5)},{" "}
                        {position.longitude.toFixed(5)}
                    </p>
                </div>

            </div>
        )}

    </div>
);
}

export default Start;