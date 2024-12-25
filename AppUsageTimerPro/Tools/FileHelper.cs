using System.IO;

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
}