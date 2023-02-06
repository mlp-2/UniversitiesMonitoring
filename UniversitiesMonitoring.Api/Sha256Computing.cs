using System.Security.Cryptography;
using System.Text;

namespace UniversitiesMonitoring.Api;

internal static class Sha256Computing
{
    public static byte[] ComputeSha256(string s)
    {
        using var sha256 = SHA256.Create();
        var hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));
        return hashValue;
    }
}