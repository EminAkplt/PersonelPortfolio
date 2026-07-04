// İmza doku: arka planda akan kod yağmuru (Matrix esintili ama ölçülü).
// Okunabilirliği bozmaması için düşük opaklık; prefers-reduced-motion'da hiç çalışmaz.
// Renk, aktif temanın aksan/metin değişkenlerinden okunur, tema değişince güncellenir.

const GLYPHS = "01{}[]()<>/=;:+-*&|!?$#01アイウカサエオΔλμπΣ</>const=>{}".split("");
const FONT_SIZE = 16;
const TRAIL = 8;            // iz uzunluğu (glyph sayısı)
const HUB_OPACITY = 0.5;    // ana sayfada belirgin
const INNER_OPACITY = 0.22; // iç sayfalarda hafif doku
const STEP_MS = 90;         // düşük kare hızı: enerji tasarrufu + "terminal" hissi

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
    let lastTime = 0, raf = 0;

    function readColor() {
        const styles = getComputedStyle(document.documentElement);
        const c = styles.getPropertyValue("--ok").trim() || styles.getPropertyValue("--accent").trim();
        glyphColor = c || "#3ecf8e";
    }

    function resize() {
        const dpr = Math.min(devicePixelRatio || 1, 2);
        width = innerWidth;
        height = innerHeight;
        canvas.width = Math.floor(width * dpr);
        canvas.height = Math.floor(height * dpr);
        ctx.setTransform(dpr, 0, 0, dpr, 0, 0);
        columns = Math.ceil(width / FONT_SIZE);
        drops = Array.from({ length: columns }, () => Math.floor(Math.random() * -40));
        ctx.font = `${FONT_SIZE}px "JetBrains Mono", monospace`;
        ctx.textBaseline = "top";
    }

    function draw(time) {
        raf = requestAnimationFrame(draw);
        if (time - lastTime < STEP_MS) return;
        lastTime = time;

        ctx.clearRect(0, 0, width, height);
        const tick = Math.floor(time / STEP_MS);

        for (let i = 0; i < columns; i++) {
            const headRow = drops[i];
            const x = i * FONT_SIZE;

            // baştan kuyruğa doğru sönen iz
            for (let t = 0; t < TRAIL; t++) {
                const row = headRow - t;
                if (row < 0) continue;
                const y = row * FONT_SIZE;
                if (y > height) continue;
                ctx.globalAlpha = t === 0 ? 1 : Math.max(0, 1 - t / TRAIL);
                ctx.fillStyle = glyphColor;
                const glyph = GLYPHS[(tick + i * 7 + row) % GLYPHS.length];
                ctx.fillText(glyph, x, y);
            }

            if (headRow * FONT_SIZE > height && Math.random() > 0.975) {
                drops[i] = Math.floor(Math.random() * -20);
            } else {
                drops[i]++;
            }
        }
        ctx.globalAlpha = 1;
    }

    readColor();
    resize();

    let resizeTimer = 0;
    addEventListener("resize", () => {
        clearTimeout(resizeTimer);
        resizeTimer = setTimeout(resize, 200);
    });

    new MutationObserver(readColor).observe(document.documentElement, {
        attributes: true, attributeFilter: ["data-theme"]
    });
    matchMedia("(prefers-color-scheme: dark)").addEventListener?.("change", readColor);

    // Sekme gizliyken animasyonu durdur (pil dostu)
    document.addEventListener("visibilitychange", () => {
        cancelAnimationFrame(raf);
        if (!document.hidden) raf = requestAnimationFrame(draw);
    });

    raf = requestAnimationFrame(draw);
}
