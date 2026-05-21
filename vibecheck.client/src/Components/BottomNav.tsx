import { useNavigate, useLocation } from "react-router-dom";
import "./BottomNav.css";

function BottomNav() {
    const navigate = useNavigate();
    const location = useLocation();

    const isMap = location.pathname === "/map";

    return (
        <div className="bottom-nav">
            <button
                className={`nav-btn ${!isMap ? "active" : ""}`}
                onClick={() => navigate("/")}
            >
                <div className="nav-content">
                <img src="/images/home-icon.png" alt="List"/>
                </div>
            </button>

            <button
                className={`nav-btn ${isMap ? "active" : ""}`}
                onClick={() => navigate("/map")}
            >
                <div className="nav-content">
                    <img src="/images/map-icon.png" alt="Map" />
                </div>
            </button>
        </div>
    );
}

export default BottomNav;