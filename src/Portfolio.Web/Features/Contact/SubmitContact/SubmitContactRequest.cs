namespace Portfolio.Web.Features.Contact.SubmitContact;

/// <param name="Website">Honeypot alanı — insanlar görmez, botlar doldurur. Doluysa mesaj sessizce yok sayılır.</param>
public sealed record SubmitContactRequest(
    string? Name,
    string? Email,
    string? Message,
    string? Website);
