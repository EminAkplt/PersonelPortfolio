// Tema geçişi: manuel seçim localStorage'da tutulur; seçim yoksa sistem tercihi geçerlidir.

const root = document.documentElement;

function currentTheme() {
    const manual = root.getAttribute("data-theme");
    if (manual) return manual;
    return matchMedia("(prefers-color-scheme: dark)").matches ? "dark" : "light";
}

export function initTheme() {
    const button = document.getElementById("temaDugmesi");
    if (!button) return;

    button.addEventListener("click", () => {
        const next = currentTheme() === "dark" ? "light" : "dark";
        root.setAttribute("data-theme", next);
        try { localStorage.setItem("theme", next); } catch { /* gizli mod vb. */ }
    });
}
