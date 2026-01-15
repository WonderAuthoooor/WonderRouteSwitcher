using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WifiProxySwitcher.pojo
{
    public class ProxySetting
    {
        // 添加 [Serializable] 特性使对象可序列化
        public string ShortcutKey { get; set; } = "";
        public string WifiName { get; set; } = "";
        public bool EnableProxy { get; set; } = false;
        public string ProxyAddress { get; set; } = "";

        [Browsable(false)]
        public int HotkeyId { get; set; }


        // 默认构造函数（JSON序列化需要）
        public ProxySetting() { }


        // 如果需要将热键字符串转换为Keys枚举
        public Keys ParseHotkey()
        {
            Keys result = Keys.None;

            if (!string.IsNullOrEmpty(ShortcutKey))
            {
                string[] parts = ShortcutKey.Split('+');

                foreach (string part in parts)
                {
                    if (part.Equals("Ctrl", StringComparison.OrdinalIgnoreCase))
                        result |= Keys.Control;
                    else if (part.Equals("Shift", StringComparison.OrdinalIgnoreCase))
                        result |= Keys.Shift;
                    else if (part.Equals("Alt", StringComparison.OrdinalIgnoreCase))
                        result |= Keys.Alt;
                    else
                    {
                        try
                        {
                            Keys key = (Keys)Enum.Parse(typeof(Keys), part);
                            result |= key;
                        }
                        catch
                        {
                            // 无法解析的键
                        }
                    }
                }
            }

            return result;
        }


        // 解析热键字符串为Keys枚举
        public bool TryParseHotkey(out Keys key, out Keys modifiers)
        {
            return HotkeyParser.TryParse(ShortcutKey, out key, out modifiers);
        }


        // 执行WiFi切换和代理设置
        public void Execute()
        {
            WiFiManager.SwitchToNetwork(WifiName, EnableProxy, ProxyAddress);
        }



        public override string ToString()
        {
            return $"{ShortcutKey} -> {WifiName}";
        }
    }
}
