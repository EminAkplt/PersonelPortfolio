using Portfolio.Web.Data;

namespace Portfolio.Web.Features.Admin.Messages.MarkMessageRead;

public sealed class MarkMessageReadHandler(AppDbContext db)
{
    public async Task<Result> HandleAsync(int id, CancellationToken ct = default)
    {
        var message = await db.ContactMessages.FindAsync([id], ct);
        if (message is null)
            return Result.Fail(Error.NotFound("message_not_found", "Mesaj bulunamadı."));

        message.IsRead = !message.IsRead;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
