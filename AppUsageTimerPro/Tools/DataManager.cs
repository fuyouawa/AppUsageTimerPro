using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace AppUsageTimerPro
{
    public class DataManager : Singleton<DataManager>
    {
        public readonly string LocalAppData;
        public readonly string SaveDir;

        public readonly string LogSaveDir;

        DataManager()
        {
            LocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            SaveDir = Path.Combine(LocalAppData, "AppUsageTimerPro");
            LogSaveDir = Path.Combine(SaveDir, "Logs");
        }

        private FileStream EnsureOpenFile(string path, FileAccess access)
        {
            var dir = Path.GetDirectoryName(path);
            DebugHelper.Assert(dir != null);

            if (!Directory.Exists(dir))
            {
                var info = Directory.CreateDirectory(dir);
                DebugHelper.Assert(info.Exists, $"创建文件夹失败：{path}");
            }

            var file = File.Open(path, FileMode.OpenOrCreate, access);
            DebugHelper.Assert(file != null, $"打开（或创建）文件失败：{path}");
            return file;
        }

        public void Save(string filename, byte[] data)
        {
            var path = Path.Combine(SaveDir, filename);
            var file = EnsureOpenFile(path, FileAccess.Write);
            file.Write(data);
        }

        public ValueTask SaveAsync(string filename, byte[] data)
        {
            var path = Path.Combine(SaveDir, filename);
            var file = EnsureOpenFile(path, FileAccess.Write);
            return file.WriteAsync(data);
        }
    }
}