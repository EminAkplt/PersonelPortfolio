// Giriş noktası — küçük, bağımsız modüller; framework yok.

import { initTheme } from "./theme.js";
import { initStatus } from "./status.js";
import { initReveal } from "./reveal.js";
import { initContact } from "./contact.js";
import { initTrack } from "./track.js";
import { initCodeRain } from "./code-rain.js";

// CSS'in reveal gizlemesi yalnızca JS varken devreye girsin
document.documentElement.classList.add("js");

initTheme();
initStatus();
initReveal();
initContact();
initTrack();
initCodeRain();
