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
        public readonly string UsageSaveDir;
        public readonly string TimerSaveDir;
        public readonly string ConfigSaveDir;

        DataManager()
        {
            LocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            SaveDir = Path.Combine(LocalAppData, "AppUsageTimerPro");
            LogSaveDir = Path.Combine(SaveDir, "Logs");
            UsageSaveDir = Path.Combine(SaveDir, "Usage");
            TimerSaveDir = Path.Combine(SaveDir, "Timers");
            ConfigSaveDir = Path.Combine(SaveDir, "Config");

            Directory.CreateDirectory(UsageSaveDir);
            Directory.CreateDirectory(TimerSaveDir);
            Directory.CreateDirectory(ConfigSaveDir);
        }

        public void Save(string filename, byte[] data)
        {
            var path = Path.Combine(SaveDir, filename);
            var file = File.OpenWrite(path);
            file.Write(data);
        }

        public ValueTask SaveAsync(string filename, byte[] data)
        {
            var path = Path.Combine(SaveDir, filename);
            var file = File.OpenWrite(path);
            return file.WriteAsync(data);
        }
    }
}