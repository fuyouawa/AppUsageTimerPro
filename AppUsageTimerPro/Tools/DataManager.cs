using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace AppUsageTimerPro.Tools;

public class DataManager : Singleton<DataManager>
{
    public readonly string LocalAppData;
    public readonly string SaveDir;

    DataManager()
    {
        LocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        SaveDir = Path.Combine(LocalAppData, "AppUsageTimerPro");
    }

    public void Save(string filename, byte[] data)
    {
        var path = Path.Combine(SaveDir, filename);
        var dir = Path.GetDirectoryName(path);
        DebugHelper.Assert(dir != null);

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
            DebugHelper.Assert(Directory.Exists(dir), $"创建文件夹失败:{path}）");
        }


        if (File.Exists(path))
        {
            
        }
        MessageBox.Show($"保存数据失败！\n原因：创建文件失败（{path}）");
    }
}