using ControlzEx.Standard;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AppUsageTimerPro;

public static class FileHelper
{
    public static void CreateIfNotExist(string path)
    {
        if (!File.Exists(path))
        {
            using (File.Create(path))
            {
            }
        }
    }


    private static void CheckHash(string path, string contents)
    {
        var hashPath = path + ".hash";

        if (File.Exists(hashPath))
        {
            var hash = File.ReadAllBytes(hashPath);
            if (!SHA256Helper.Compare(contents.GetSHA256(), hash))
            {
                ErrorHelper.DataBreak(path);
            }
        }
        WriteHash(path, contents);
    }

    private static void WriteHash(string path, string contents)
    {
        var hashPath = path + ".hash";
        
        File.WriteAllBytes(hashPath, contents.GetSHA256());
    }

    public static async Task<string> ReadAllTextWithHashAsync(string path)
    {
        var text = await File.ReadAllTextAsync(path);
        CheckHash(path, text);
        return text;
    }

    public static async Task WriteAllTextWithHashAsync(string path, string contents)
    {
        await File.WriteAllTextAsync(path, contents);
        WriteHash(path, contents);
    }

    public static string ReadAllTextWithHash(string path)
    {
        var text = File.ReadAllText(path);
        CheckHash(path, text);
        return text;
    }

    public static void WriteAllTextWithHash(string path, string contents)
    {
        File.WriteAllText(path, contents);
        WriteHash(path, contents);
    }
}