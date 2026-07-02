namespace Portfolio.Web.Common.Security;

/// <summary>
/// Tek admin kullanıcı doğrulaması. Şifre hash'i konfigürasyondan (env) gelir;
/// hash üretmek için: dotnet run --project src/Portfolio.Web -- hash-password "şifreniz"
/// </summary>
public sealed class AdminAuthService(IConfiguration configuration, IHostEnvironment environment)
{
    private const string DevFallbackPassword = "admin123";

    public bool Validate(string? username, string? password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return false;

        var expectedUsername = configuration["Admin:Username"] ?? "admin";
        if (!string.Equals(username, expectedUsername, StringComparison.OrdinalIgnoreCase))
            return false;

        var passwordHash = configuration["Admin:PasswordHash"];
        if (!string.IsNullOrWhiteSpace(passwordHash))
            return PasswordHasher.Verify(password, passwordHash);

        // Hash tanımlı değilse: yalnızca geliştirme ortamında varsayılan şifre kabul edilir.
        // Üretimde hash zorunludur; yoksa giriş her zaman reddedilir.
        return environment.IsDevelopment() && password == DevFallbackPassword;
    }
}
