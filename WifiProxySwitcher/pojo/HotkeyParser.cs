using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WifiProxySwitcher.pojo
{
    public static class HotkeyParser
    {
        public static bool TryParse(string hotkeyString, out Keys key, out Keys modifiers)
        {
            key = Keys.None;
            modifiers = Keys.None;

            if (string.IsNullOrEmpty(hotkeyString))
                return false;

            try
            {
                string[] parts = hotkeyString.Split('+');

                foreach (string part in parts)
                {
                    string partTrimmed = part.Trim();

                    if (string.Equals(partTrimmed, "Ctrl", StringComparison.OrdinalIgnoreCase))
                    {
                        modifiers |= Keys.Control;
                    }
                    else if (string.Equals(partTrimmed, "Shift", StringComparison.OrdinalIgnoreCase))
                    {
                        modifiers |= Keys.Shift;
                    }
                    else if (string.Equals(partTrimmed, "Alt", StringComparison.OrdinalIgnoreCase))
                    {
                        modifiers |= Keys.Alt;
                    }
                    else
                    {
                        // 尝试解析为主键
                        if (Enum.TryParse(partTrimmed, true, out Keys parsedKey))
                        {
                            key = parsedKey;
                        }
                        else
                        {
                            return false; // 无法解析的键
                        }
                    }
                }

                return key != Keys.None; // 必须有一个非修饰键
            }
            catch
            {
                return false;
            }
        }

        public static string FormatHotkey(Keys key, Keys modifiers)
        {
            List<string> parts = new List<string>();

            if ((modifiers & Keys.Control) == Keys.Control)
                parts.Add("Ctrl");
            if ((modifiers & Keys.Shift) == Keys.Shift)
                parts.Add("Shift");
            if ((modifiers & Keys.Alt) == Keys.Alt)
                parts.Add("Alt");

            if (key != Keys.None)
                parts.Add(key.ToString());

            return string.Join("+", parts);
        }
    }
}
