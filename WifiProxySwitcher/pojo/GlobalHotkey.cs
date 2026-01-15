using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WifiProxySwitcher.pojo
{
    public class GlobalHotkey : IDisposable
    {
        private readonly IntPtr _hWnd;
        private readonly Dictionary<int, Action> _hotkeys = new Dictionary<int, Action>();

        private int _currentId = 0x0000;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public GlobalHotkey(IntPtr windowHandle)
        {
            _hWnd = windowHandle;
        }

        public bool RegisterHotkey(Keys key, Keys modifiers, Action callback)
        {
            // 1. 转换修饰键
            uint fsModifiers = 0;

            if ((modifiers & Keys.Control) != 0)
                fsModifiers |= 0x0002;
            if ((modifiers & Keys.Shift) != 0)
                fsModifiers |= 0x0004;
            if ((modifiers & Keys.Alt) != 0)
                fsModifiers |= 0x0001;
            if ((modifiers & Keys.LWin) != 0 || (modifiers & Keys.RWin) != 0)
                fsModifiers |= 0x0008;

            // 2. 转换主键（清除高位标志）
            uint vk = (uint)key & 0xFFFF;

            // 3. 注册
            int id = Interlocked.Increment(ref _currentId);

            if (RegisterHotKey(_hWnd, id, fsModifiers, vk))
            {
                _hotkeys[id] = callback;
                return true;
            }
            else
            {
                int error = Marshal.GetLastWin32Error();
                return false;
            }

        }

        public bool RegisterHotkey(ProxySetting proxySetting)
        {
            if (string.IsNullOrEmpty(proxySetting.ShortcutKey))
                return false;

            // 先尝试注销已存在的热键
            if (proxySetting.HotkeyId != 0)
            {
                UnregisterHotkey(proxySetting.HotkeyId);
            }

            if (proxySetting.TryParseHotkey(out Keys key, out Keys modifiers))
            {
                // 生成唯一ID
                int hotkeyId = Math.Abs(proxySetting.GetHashCode()) % 0xBFFF;

                // 重试机制
                for (int i = 0; i < 3; i++)
                {
                    bool success = RegisterHotkey(key, modifiers, () =>
                    {
                        proxySetting.Execute();
                    });

                    if (success)
                    {
                        proxySetting.HotkeyId = hotkeyId;
                        StdOutput.debugOutput($"已注册热键: {proxySetting.ShortcutKey} -> {proxySetting.WifiName}");
                        return true;
                    }

                    // 短暂延迟后重试
                    System.Threading.Thread.Sleep(100);
                }

                StdOutput.debugOutput($"注册热键失败: {proxySetting.ShortcutKey}");
                return false;
            }

            StdOutput.debugOutput($"热键格式错误: {proxySetting.ShortcutKey}");
            return false;
        }

        public bool UnregisterHotkey(int id)
        {
            if (_hotkeys.ContainsKey(id))
            {
                bool result = UnregisterHotKey(_hWnd, id);
                _hotkeys.Remove(id);
                return result;
            }
            return false;
        }

        public void ProcessHotkey(int id)
        {
            if (_hotkeys.ContainsKey(id))
            {
                _hotkeys[id]?.Invoke();
            }
        }

        public void Dispose()
        {
            // 注销所有热键
            foreach (int id in _hotkeys.Keys)
            {
                UnregisterHotKey(_hWnd, id);
            }
            _hotkeys.Clear();
        }
    }
}
