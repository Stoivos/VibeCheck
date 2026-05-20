import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import 'leaflet/dist/leaflet.css'
import Start from './Components/Start';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Start />
  </StrictMode>,
)
