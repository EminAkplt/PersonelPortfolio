// İletişim formu: sayfa yenilenmeden gönderir; mesajlar dile göre data-* ile gelir.

export function initContact() {
    const form = document.getElementById("iletisimFormu");
    if (!form) return;

    const status = form.querySelector(".form-status");
    const button = form.querySelector('button[type="submit"]');
    const sendingText = form.dataset.sending || "Gönderiliyor…";
    const successText = form.dataset.success || "Mesajın ulaştı, 24 saat içinde dönerim.";

    form.addEventListener("submit", async event => {
        event.preventDefault();

        if (!form.reportValidity()) return;

        button.disabled = true;
        status.className = "form-status";
        status.textContent = sendingText;

        try {
            const data = Object.fromEntries(new FormData(form));
            const response = await fetch(form.action, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(data)
            });

            const body = await response.json().catch(() => null);

            if (response.ok) {
                status.classList.add("is-ok");
                status.textContent = body?.message ?? successText;
                form.reset();
            } else {
                status.classList.add("is-error");
                status.textContent = body?.detail ?? body?.message ?? "Bir şeyler ters gitti.";
            }
        } catch {
            status.classList.add("is-error");
            status.textContent = "Sunucuya ulaşılamadı.";
        } finally {
            button.disabled = false;
        }
    });
}
