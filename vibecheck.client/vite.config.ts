import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import { fileURLToPath, URL } from 'node:url';

const target = 'http://localhost:5292'; // backend

export default defineConfig({
    plugins: [react()],

    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },

    server: {
        port: 5173,

        proxy: {
            '/api': {
                target,
                changeOrigin: true
            },

            '/crowdhub': {
                target,
                ws: true,
                changeOrigin: true
            }
        }
    }
});