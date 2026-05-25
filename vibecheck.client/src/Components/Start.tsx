import { useState } from "react";
import { useCrowdHub } from "../Hooks/useCrowdHub";
import { useLocation } from "../Hooks/useLocation";
import BottomNav from "./BottomNav";
import "./Start.css";

type Filter = "alla" | "folkigt" | "lugnt";

function Start() {
    const { crowd, myPlace, isReady } = useCrowdHub();
    const { position, error } = useLocation();
    const [search, setSearch] = useState("");
    const [filter, setFilter] = useState<Filter>("alla");

    const hasLocation = !!position;
    const blocked = !!error;

    // Simulation buttons for testing. 
    const startSimulation = async () => {
        try {
            await fetch("http://localhost:5292/api/simulate/start", {
                method: "POST"
            });
        } catch (err) {
            console.error("Kunde inte starta simulering", err);
        }
    };

    const stopSimulation = async () => {
        try {
            await fetch("http://localhost:5292/api/simulate/stop", {
                method: "POST"
            });
        } catch (err) {
            console.error("Kunde inte stoppa simulering", err);
        }
    };


    const myPlaceData = crowd.find(p => p.placeId === myPlace?.placeId);

    const filteredCrowd = crowd
        .filter(p => p.placeName.toLowerCase().includes(search.toLowerCase()))
        .filter(p => {
            if (filter === "folkigt") return p.count >= 5;
            if (filter === "lugnt") return p.count < 5;
            return true;
        });

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

                        {myPlaceData ? (
                            <div
                                className="my-place-card"
                                style={{ backgroundImage: `url(${myPlaceData.imageUrl})` }}
                            >
                                <div className="my-place-overlay" />
                                <div className="my-place-info">
                                    <span className="my-place-label">Du är på</span>
                                    <span className="my-place-name">{myPlaceData.placeName}</span>
                                    <span className="my-place-count">{myPlaceData.count} här nu</span>
                                </div>
                            </div>
                        ) : (
                            <div className="no-place-card">
                                <img className="no-place-icon" src="/images/noplace.png"></img>
                                <span className="no-place-text">Hemma? Äh nu tar vi helg!</span>
                            </div>
                        )}
                    </div>

                    <div className="content">

                        {/* Search and filter together */}
                        <div className="search-filter">
                            <div className="search-bar">
                                <img src="/images/search-icon.png" alt="" className="search-icon" />
                                <input
                                    type="text"
                                    placeholder="Sök krog..."
                                    value={search}
                                    onChange={e => setSearch(e.target.value)}
                                />
                            </div>
                            <div className="filter-bar">
                                {(["alla", "folkigt", "lugnt"] as Filter[]).map(f => (
                                    <button
                                        key={f}
                                        className={`filter-btn ${filter === f ? "active" : ""}`}
                                        onClick={() => setFilter(f)}
                                    >
                                        {f.charAt(0).toUpperCase() + f.slice(1)}
                                    </button>
                                ))}
                            </div>
                        </div>

                        {/* List all places with crowd data. */}
                        <div className="places">
                            {filteredCrowd.map((p) => (
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
            <div className="dev-controls">
                <button className="dev-btn start" onClick={startSimulation}>
                    Start simulation
                </button>

                <button className="dev-btn stop" onClick={stopSimulation}>
                    Stop simulation
                </button>
            </div>

        </>
    );
}

export default Start;