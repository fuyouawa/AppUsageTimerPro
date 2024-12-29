using System;
using System.Collections.Generic;
using System.IO;
using AppUsageTimerPro.Logic;
using EasyFramework;
using Newtonsoft.Json;

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

            var settings = new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter>()
                {
                    new TimerItemConverter(),
                    new UsageRecordListConverter(),
                    new SettingsConverter(),
                    new ListenAppConverter()
                },
                Formatting = Formatting.Indented
            };
            JsonConvert.DefaultSettings += () => settings;
        }
    }
}