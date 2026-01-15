using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WifiProxySwitcher.pojo
{
    public static class AppConfig
    {
        private static readonly string configFilePath = "config.json";

        public static List<ProxySetting> LoadSettings()
        {
            try
            {
                if (File.Exists(configFilePath))
                {
                    string json = File.ReadAllText(configFilePath);
                    return JsonSerializer.Deserialize<List<ProxySetting>>(json);
                }
            }
            catch (Exception ex)
            {
                StdOutput.debugOutput($"加载配置文件失败: {ex.Message}");
            }

            return new List<ProxySetting>(); // 返回空列表
        }

        public static void SaveSettings(List<ProxySetting> settings)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true // 格式化JSON，便于阅读
                };

                string json = JsonSerializer.Serialize(settings, options);
                File.WriteAllText(configFilePath, json);

                StdOutput.debugOutput($"配置已保存到: {Path.GetFullPath(configFilePath)}");
            }
            catch (Exception ex)
            {
                StdOutput.debugOutput($"保存配置文件失败: {ex.Message}");
            }
        }

    }
}
