namespace Portfolio.Web.Domain;

public class ContactMessage
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Message { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public bool IsRead { get; set; }

    /// <summary>SHA256 ile hash'lenmiş istemci IP'si — ham IP saklanmaz (KVKK).</summary>
    public required string ClientIpHash { get; set; }
}
