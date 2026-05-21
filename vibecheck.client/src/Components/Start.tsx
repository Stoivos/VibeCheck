import { useCrowdHub } from "../Hooks/useCrowdHub";
import { useLocation } from "../Hooks/useLocation";
import BottomNav from "./BottomNav";
import "./Start.css";

function Start() {

    const { crowd, myPlace, isReady } = useCrowdHub();
    const { position, error } = useLocation();


    const hasLocation = !!position;
    const blocked = !!error;

    return (
        <>
            {/* if connecting, show loading screen */}
            {!isReady && (
            <div className="loading-screen">
                <img src="/images/logo.png" alt="Vibecheck" className="loading-logo" />
                <p className="loading-text">Vibecheckar...</p>
            </div>
            )}
            

            {isReady && !hasLocation && !blocked && (
                <div className="popup">
                    <h2>För att börja vibechecka</h2>
                    <p>Behöver du tillåta platsinfo</p>
                </div>
            )}

            {isReady && blocked && (
                <div className="popup error">
                    <h2>Plats åtkomst nekad</h2>
                    <p>Slå på location i din browser för att använda Vibecheck</p>
                </div>
            )}

            {isReady && hasLocation && (
                <div className="app">
                    <div className="header">
                        <div className="logo">
                            <img src="/images/logo.png" alt="Vibecheck" />
                        </div>
                        <h2 className="page-title">Live crowd</h2>
                    </div>

                    

                    <div className="content">

                        {crowd.length === 0 && (
                            <p>Loading places...</p>
                        )}

                        {myPlace && (
                            <div className="my-place">        
                                <span className="my-place-label">Du är på</span>
                                <span className="my-place-name">{myPlace.placeName}</span>
                            </div>
                        )}

                        <div className="places">
                            {crowd.map((p) => (
                                <div key={p.placeId} className="place-card">
                                    <div className="place-image" style={{ backgroundImage: `url(${p.imageUrl})` }}>
                                        <div className="place-image-title">{p.placeName}</div>
                                    </div>
                                    <div className="place-content">
                                        <div className="place-footer">
                                            <div className="user-count">
                                                <div className="user-icon" />
                                                <span>{p.count} här nu</span>
                                            </div>
                                            <div className="live-status">LIVE</div>
                                        </div>
                                    </div>
                                </div>
                            ))}
                        </div>

                        <div className="location">        {/* Coordinates */}
                            <p>{position?.latitude.toFixed(5)}, {position?.longitude.toFixed(5)}</p>
                        </div>
                    </div>
                    <BottomNav />
                </div>
            )}
        </>
    );
}

export default Start;