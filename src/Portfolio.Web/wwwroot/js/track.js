// Basit, çerezsiz sayfa görüntüleme kaydı — fire and forget.

export function initTrack() {
    const payload = JSON.stringify({ path: location.pathname });

    try {
        const blob = new Blob([payload], { type: "application/json" });
        if (navigator.sendBeacon?.("/api/track", blob)) return;
    } catch { /* sendBeacon yoksa fetch'e düş */ }

    fetch("/api/track", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: payload,
        keepalive: true
    }).catch(() => { /* analitik asla deneyimi bozmaz */ });
}
