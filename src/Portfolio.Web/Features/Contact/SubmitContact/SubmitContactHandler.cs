using FluentValidation;
using Portfolio.Web.Common.Security;
using Portfolio.Web.Data;
using Portfolio.Web.Domain;

namespace Portfolio.Web.Features.Contact.SubmitContact;

public sealed class SubmitContactHandler(
    AppDbContext db,
    IValidator<SubmitContactRequest> validator,
    IpHasher ipHasher)
{
    public async Task<Result> HandleAsync(SubmitContactRequest request, string? clientIp, CancellationToken ct = default)
    {
        // Honeypot doluysa bot varsayılır; kayıt atlanır ama bota "başarılı" görüntüsü verilir.
        if (!string.IsNullOrWhiteSpace(request.Website))
            return Result.Success();

        var validation = await validator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            var first = validation.Errors[0];
            return Result.Fail(Error.Validation("contact_invalid", first.ErrorMessage));
        }

        db.ContactMessages.Add(new ContactMessage
        {
            Name = request.Name!.Trim(),
            Email = request.Email!.Trim(),
            Message = request.Message!.Trim(),
            ClientIpHash = ipHasher.Hash(clientIp),
            CreatedAt = DateTimeOffset.UtcNow,
            IsRead = false
        });

        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
