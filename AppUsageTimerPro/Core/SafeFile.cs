using ControlzEx.Standard;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serilog;

namespace AppUsageTimerPro;

public class DataBreakException : Exception
{
    public string FilePath { get; }

    public DataBreakException(string filePath)
        : base($"数据文件损坏（{filePath}）")
    {
        FilePath = filePath;
    }

    public DataBreakException(string filePath, Exception? innerException)
        : base("数据文件损坏（{filePath}）", innerException)
    {
        FilePath = filePath;
    }
}

public static class SafeFile
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
                throw new DataBreakException(path);
            }
        }

        WriteHash(path, contents);
    }

    private static void WriteHash(string path, string contents)
    {
        var hashPath = path + ".hash";

        File.WriteAllBytes(hashPath, contents.GetSHA256());
    }


    public static async Task<string> ReadAllTextAsync(string path)
    {
        var text = await File.ReadAllTextAsync(path);
        CheckHash(path, text);
        return text;
    }

    private static T? DeserializeJsonWithCheck<T>(string path, string json)
    {
        T? res = default;
        try
        {
            CheckHash(path, json);      // 检查hash
        }
        catch (DataBreakException e)    // 如果hash没有匹配上，尝试解析json
        {
            try
            {
                res = JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonException je)    // 反序列化也失败了，重新抛出异常
            {
                throw new DataBreakException(path, je);
            }
            WriteHash(path, json);
            Log.Warning($"{path}疑似数据损坏（或人为修改），但json解析成功");
        }

        if (res == null)
        {
            res = JsonConvert.DeserializeObject<T>(json);
        }
        return res;
    }

    /// <summary>
    /// 读取文件并反序列化json
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static async Task<T?> ReadAndDeserializeJsonAsync<T>(string path)
    {
        var json = await File.ReadAllTextAsync(path);
        return DeserializeJsonWithCheck<T>(path, json);
    }

    public static async Task WriteAllTextAsync(string path, string contents)
    {
        await File.WriteAllTextAsync(path, contents);
        WriteHash(path, contents);
    }
    /// <summary>
    /// 读取文件并反序列化json
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T? ReadAndDeserializeJson<T>(string path)
    {
        var json = File.ReadAllText(path);
        return DeserializeJsonWithCheck<T>(path, json);
    }

    public static string ReadAllText(string path)
    {
        var text = File.ReadAllText(path);
        CheckHash(path, text);
        return text;
    }

    public static void WriteAllText(string path, string contents)
    {
        File.WriteAllText(path, contents);
        WriteHash(path, contents);
    }
}