using System;
using System.Security.Cryptography;
using System.Text;

namespace AppUsageTimerPro;

public static class SHA256Helper
{
    public static byte[] GetSHA256(this string contents)
    {
        using SHA256 sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(contents));
    }
    public static string GetSHA256String(this string contents)
    {
        return BitConverter.ToString(contents.GetSHA256());
    }
    public static bool Compare(byte[] hash1, byte[] hash2)
    {
        if (hash1.Length != hash2.Length)
            return false;

        for (int i = 0; i < hash1.Length; i++)
        {
            if (hash1[i] != hash2[i])
                return false;
        }

        return true;
    }
}