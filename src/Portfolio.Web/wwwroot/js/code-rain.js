// İmza doku: arka planda akan kod yağmuru (Matrix esintili ama ölçülü).
// Okunabilirliği bozmaması için düşük opaklık; prefers-reduced-motion'da hiç çalışmaz.
// Renk, aktif temanın aksan/metin değişkenlerinden okunur, tema değişince güncellenir.

const GLYPHS = "01{}[]()<>/=;:+-*&|!?$#01アイウカサ01エオ01ΔλμπΣ".split("");
const FONT_SIZE = 16;
const HUB_OPACITY = 0.16;   // ana sayfada biraz daha belirgin
const INNER_OPACITY = 0.07; // iç sayfalarda çok hafif doku

export function initCodeRain() {
    const canvas = document.getElementById("kodYagmuru");
    if (!canvas) return;

    if (matchMedia("(prefers-reduced-motion: reduce)").matches) {
        canvas.remove();
        return;
    }

    const ctx = canvas.getContext("2d", { alpha: true });
    if (!ctx) { canvas.remove(); return; }

    const isHub = document.body.classList.contains("is-hub");
    canvas.style.opacity = String(isHub ? HUB_OPACITY : INNER_OPACITY);

    let width = 0, height = 0, columns = 0, drops = [], glyphColor = "#3ecf8e";
    let lastTime = 0;
    const STEP_MS = 70; // düşük kare hızı: hem enerji tasarrufu hem "terminal" hissi

    function readColor() {
        const styles = getComputedStyle(document.documentElement);
        const accent = styles.getPropertyValue("--ok").trim() || styles.getPropertyValue("--accent").trim();
        glyphColor = accent || "#3ecf8e";
    }

    function resize() {
        const dpr = Math.min(devicePixelRatio || 1, 2);
        width = canvas.clientWidth = innerWidth;
        height = canvas.clientHeight = innerHeight;
        canvas.width = Math.floor(width * dpr);
        canvas.height = Math.floor(height * dpr);
        ctx.setTransform(dpr, 0, 0, dpr, 0, 0);
        columns = Math.ceil(width / FONT_SIZE);
        drops = new Array(columns).fill(0).map(() => Math.floor(Math.random() * -40));
        ctx.font = `${FONT_SIZE}px "JetBrains Mono", monospace`;
        ctx.textBaseline = "top";
    }

    function draw(time) {
        if (time - lastTime >= STEP_MS) {
            lastTime = time;
            // Hafif iz bırakmak için yarı saydam silme
            ctx.clearRect(0, 0, width, height);
            ctx.fillStyle = glyphColor;

            for (let i = 0; i < columns; i++) {
                const y = drops[i] * FONT_SIZE;
                if (y > 0) {
                    const glyph = GLYPHS[(Math.floor(time / STEP_MS) + i) % GLYPHS.length];
                    // baştaki damla daha parlak, kuyruğu soluk
                    ctx.globalAlpha = 1;
                    ctx.fillText(glyph, i * FONT_SIZE, y);
                    ctx.globalAlpha = 0.5;
                    const prev = GLYPHS[(Math.floor(time / STEP_MS) + i + 3) % GLYPHS.length];
                    ctx.fillText(prev, i * FONT_SIZE, y - FONT_SIZE);
                }

                if (y > height && Math.random() > 0.975) drops[i] = Math.floor(Math.random() * -20);
                else drops[i]++;
            }
            ctx.globalAlpha = 1;
        }
        raf = requestAnimationFrame(draw);
    }

    let raf = 0;
    readColor();
    resize();

    let resizeTimer = 0;
    addEventListener("resize", () => {
        clearTimeout(resizeTimer);
        resizeTimer = setTimeout(resize, 200);
    });

    // Tema değiştiğinde rengi tazele
    new MutationObserver(readColor).observe(document.documentElement, {
        attributes: true, attributeFilter: ["data-theme"]
    });
    matchMedia("(prefers-color-scheme: dark)").addEventListener?.("change", readColor);

    // Sekme gizliyken animasyonu durdur (pil dostu)
    document.addEventListener("visibilitychange", () => {
        if (document.hidden) cancelAnimationFrame(raf);
        else raf = requestAnimationFrame(draw);
    });

    raf = requestAnimationFrame(draw);
}
