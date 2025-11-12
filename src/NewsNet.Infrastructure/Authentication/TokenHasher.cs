using System.Security.Cryptography;
using System.Text;

namespace NewsNet.Infrastructure.Authentication;

internal static class TokenHasher
{
    public static string Hash(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);

        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(bytes);
    }
}
