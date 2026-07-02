namespace Portfolio.Web.Domain;

/// <summary>Basit, kişisel veri saklamayan sayfa görüntüleme kaydı.</summary>
public class PageView
{
    public long Id { get; set; }
    public required string Path { get; set; }
    public DateTimeOffset VisitedAt { get; set; }

    /// <summary>SHA256 ile hash'lenmiş istemci IP'si — ham IP saklanmaz (KVKK).</summary>
    public required string ClientIpHash { get; set; }
}
