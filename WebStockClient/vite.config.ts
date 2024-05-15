import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      "/cat-api": {
        target: "https://catfact.ninja",
        changeOrigin: true,
        secure: false,
        rewrite: (path) => path.replace(/^\/cat-api/, ""),
      },
      "/mail-web-api": {
        target: "http://localhost:5286",
        changeOrigin: true,
        secure: false,
        rewrite: (path) => path.replace(/^\/mail-web-api/, "/api"),
      },
      "/stock-web-api": {
        target: "http://localhost:5177",
        changeOrigin: true,
        secure: false,
        rewrite: (path) => path.replace(/^\/stock-web-api/, "/api"),
      },
    },
  },
});
