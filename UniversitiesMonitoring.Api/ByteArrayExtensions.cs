namespace UniversitiesMonitoring.Api;

public static class ByteArrayExtensions
{
    public static bool IsSequenceEquals(this byte[] a, byte[] b)
    {
        for (var i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i]) return false;
        }

        return true;
    }
}