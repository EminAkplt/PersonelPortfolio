// Hero'daki durum rozeti gerçekten sunucuya sorar — süs değil.
// Metinler sunucudan data-* ile gelir (dil desteği için).

const CHECK_INTERVAL = 60_000;

export function initStatus() {
    const badge = document.getElementById("durumRozeti");
    if (!badge) return;

    const endpoint = badge.dataset.endpoint || "/api/health";
    const text = badge.querySelector(".status-text");
    const okText = badge.dataset.ok || "Sistemler çalışıyor";
    const downText = badge.dataset.down || "Sistemlerde aksama var";
    const offlineText = badge.dataset.offline || "Bağlantı kurulamadı";

    async function check() {
        try {
            const response = await fetch(endpoint, { cache: "no-store" });
            const ok = response.ok;
            badge.classList.toggle("is-down", !ok);
            text.textContent = ok ? okText : downText;
        } catch {
            badge.classList.add("is-down");
            text.textContent = offlineText;
        }
    }

    check();
    setInterval(check, CHECK_INTERVAL);
}
