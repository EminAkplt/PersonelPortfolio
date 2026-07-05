using System.Security.Cryptography;
using System.Text;

namespace Portfolio.Web.Common.Security;

/// <summary>
/// İstemci IP'sini SHA256 ile tek yönlü hash'ler; ham IP hiçbir yerde saklanmaz (KVKK).
/// Salt, hash'lerin gökkuşağı tablolarıyla geri çözülmesini zorlaştırır.
/// </summary>
public sealed class IpHasher(IConfiguration configuration)
{
    private readonly string _salt = configuration["Security:IpHashSalt"] ?? "portfolio-default-salt";

    public string Hash(string? ipAddress)
    {
        var input = string.IsNullOrWhiteSpace(ipAddress) ? "unknown" : ipAddress;
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(_salt + input));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
