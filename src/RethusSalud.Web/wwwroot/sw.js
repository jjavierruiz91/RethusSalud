// Service worker mínimo: solo existe para cumplir el criterio de instalación
// (Agregar a pantalla de inicio) en Android/Chrome. No cachea nada a propósito
// mientras la app sigue en desarrollo activo, para evitar servir contenido
// desactualizado. El paso a caché offline real es una decisión consciente futura.
self.addEventListener("install", () => {
  self.skipWaiting();
});

self.addEventListener("activate", (event) => {
  event.waitUntil(self.clients.claim());
});

self.addEventListener("fetch", () => {
  // Pass-through intencional: sin caché.
});
