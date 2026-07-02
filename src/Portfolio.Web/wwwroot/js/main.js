// Giriş noktası — küçük, bağımsız modüller; framework yok.

import { initTheme } from "./theme.js";
import { initBoot } from "./boot.js";
import { initStatus } from "./status.js";
import { initReveal } from "./reveal.js";
import { initContact } from "./contact.js";
import { initTrack } from "./track.js";

// CSS'in reveal gizlemesi yalnızca JS varken devreye girsin
document.documentElement.classList.add("js");

initTheme();
initBoot();
initStatus();
initReveal();
initContact();
initTrack();
