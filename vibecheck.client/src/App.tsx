import { BrowserRouter, Routes, Route } from "react-router-dom";
import Start from "./Components/Start";
import Map from "./Components/Map";

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Start />} />
                <Route path="/map" element={<Map />} />
            </Routes>
        </BrowserRouter>
    );
}

export default App;