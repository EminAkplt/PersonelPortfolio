// Hero'daki durum rozeti gerçekten sunucuya sorar — süs değil.

const CHECK_INTERVAL = 60_000;

export function initStatus() {
    const badge = document.getElementById("durumRozeti");
    if (!badge) return;

    const endpoint = badge.dataset.endpoint || "/api/health";
    const text = badge.querySelector(".status-text");

    async function check() {
        try {
            const response = await fetch(endpoint, { cache: "no-store" });
            const ok = response.ok;
            badge.classList.toggle("is-down", !ok);
            text.textContent = ok ? "Sistemler çalışıyor" : "Sistemlerde aksama var";
        } catch {
            badge.classList.add("is-down");
            text.textContent = "Bağlantı kurulamadı";
        }
    }

    check();
    setInterval(check, CHECK_INTERVAL);
}
