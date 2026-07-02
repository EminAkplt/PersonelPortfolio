// İmza öğe: "sistem uyanıyor" açılış sekansı.
// Kurallar: en fazla ~2.5 sn, her tıklama/tuşla atlanabilir,
// prefers-reduced-motion'da ve aynı oturumdaki tekrar ziyarette hiç oynatılmaz.

const LINES = [
    "Merhaba",
    "Projeler yükleniyor",
    "Deneyim derleniyor",
    "Kahve seviyesi: yeterli",
    "Sistemler hazır"
];

const LINE_INTERVAL = 380; // ms — 5 satır ≈ 1.9 sn + 0.5 sn kapanış

export function initBoot() {
    const overlay = document.getElementById("acilisSekansi");
    if (!overlay) return;

    const reducedMotion = matchMedia("(prefers-reduced-motion: reduce)").matches;
    let alreadyPlayed = false;
    try { alreadyPlayed = sessionStorage.getItem("bootPlayed") === "1"; } catch { /* yoksay */ }

    if (reducedMotion || alreadyPlayed) {
        overlay.remove();
        return;
    }

    try { sessionStorage.setItem("bootPlayed", "1"); } catch { /* yoksay */ }

    const terminal = overlay.querySelector(".boot-terminal");
    const timers = [];
    let finished = false;

    overlay.hidden = false;
    document.body.style.overflow = "hidden";

    function finish() {
        if (finished) return;
        finished = true;
        timers.forEach(clearTimeout);
        overlay.classList.add("is-done");
        document.body.style.overflow = "";
        removeEventListener("keydown", finish);
        setTimeout(() => overlay.remove(), 550);
    }

    LINES.forEach((text, i) => {
        timers.push(setTimeout(() => {
            const line = document.createElement("div");
            line.className = "boot-line";
            line.innerHTML = `<span></span><span class="ok">✓</span>`;
            line.firstChild.textContent = text;
            terminal.appendChild(line);
        }, i * LINE_INTERVAL));
    });

    timers.push(setTimeout(finish, LINES.length * LINE_INTERVAL + 500));

    overlay.addEventListener("click", finish);
    document.getElementById("acilisGec")?.addEventListener("click", finish);
    addEventListener("keydown", finish);
}
