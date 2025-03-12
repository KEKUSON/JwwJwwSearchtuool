using System;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;

namespace JWWCADFileSearchTool
{
    public class AppSettings
    {
        private static string configPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "JWWCADFileSearchTool", "settings.json");

        public string JWWCADPath { get; set; } = "";
        public string JacConvertPath { get; set; } = "";
        public bool EnableColorOutput { get; set; } = true;

        // 設定をファイルから読み込む
        public static AppSettings Load()
        {
            try
            {
                string dir = Path.GetDirectoryName(configPath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                if (File.Exists(configPath))
                {
                    string json = File.ReadAllText(configPath);
                    return JsonConvert.DeserializeObject<AppSettings>(json) ?? new AppSettings();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"設定読み込みエラー: {ex.Message}");
            }
            return new AppSettings();
        }

        // 設定をファイルに保存
        public void Save()
        {
            try
            {
                string dir = Path.GetDirectoryName(configPath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(configPath, json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"設定保存エラー: {ex.Message}");
            }
        }
    }
}