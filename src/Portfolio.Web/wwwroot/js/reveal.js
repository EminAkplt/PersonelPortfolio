// Scroll'da bölümler bir kez, yumuşakça belirir.
// CSS yalnızca html.js altında gizlediği için JS yoksa içerik hep görünür.

export function initReveal() {
    const targets = document.querySelectorAll(".reveal");
    if (targets.length === 0) return;

    if (!("IntersectionObserver" in window)) {
        targets.forEach(el => el.classList.add("is-visible"));
        return;
    }

    const observer = new IntersectionObserver(entries => {
        for (const entry of entries) {
            if (entry.isIntersecting) {
                entry.target.classList.add("is-visible");
                observer.unobserve(entry.target);
            }
        }
    }, { threshold: 0.15, rootMargin: "0px 0px -5% 0px" });

    targets.forEach(el => observer.observe(el));
}
