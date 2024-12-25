using System;
using System.IO;
using Newtonsoft.Json;

namespace AppUsageTimerPro;

public class Settings
{
    public TimeSpan AutoSaveInterval = TimeSpan.FromSeconds(10);
}

public class SettingsManager : Singleton<SettingsManager>
{
    public Settings Settings { get; private set; }
    public JsonSerializerSettings JsonSerializerSettings { get; private set; } = new JsonSerializerSettings();

    public readonly string SavePath;

    SettingsManager()
    {
        JsonConvert.DefaultSettings += () => JsonSerializerSettings;

        SavePath = Path.Combine(DataManager.Instance.ConfigSaveDir, "Settings.json");

        FileHelper.CreateIfNotExist(SavePath);
        Load();
    }

    public void Load()
    {
        var json = File.ReadAllText(SavePath);
        Settings = JsonConvert.DeserializeObject<Settings>(json) ?? new Settings();
    }

    public void Save()
    {
        File.WriteAllText(SavePath, JsonConvert.SerializeObject(Settings));
    }
}