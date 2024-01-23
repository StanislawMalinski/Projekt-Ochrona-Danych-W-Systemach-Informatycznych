import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
//import basicSsl from '@vitejs/plugin-basic-ssl'
// basicSsl() 
// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5000, 
    strictPort: true,
    origin: "https://0.0.0.0",
    watch: {
       usePolling: true
     }
  }
})
