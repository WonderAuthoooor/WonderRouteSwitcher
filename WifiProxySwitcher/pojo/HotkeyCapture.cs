using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WifiProxySwitcher.pojo
{
    public class HotkeyCapture
    {
        private Keys modifierKeys = Keys.None;
        private Keys keyCode = Keys.None;
        private bool isCapturing = false;

        public bool IsCapturing => isCapturing;

        // 开始捕获热键
        public void StartCapture()
        {
            isCapturing = true;
            modifierKeys = Keys.None;
            keyCode = Keys.None;
        }

        // 停止捕获热键
        public void StopCapture()
        {
            isCapturing = false;
        }

        // 处理按键事件
        public bool ProcessKey(KeyEventArgs e)
        {
            if (!isCapturing) return false;

            // 处理修饰键（Ctrl、Shift、Alt）
            if (e.Control) modifierKeys |= Keys.Control;
            if (e.Shift) modifierKeys |= Keys.Shift;
            if (e.Alt) modifierKeys |= Keys.Alt;

            // 过滤掉单独的修饰键
            if (!IsModifierKey(e.KeyCode))
            {
                keyCode = e.KeyCode;
                return true; // 表示捕获完成
            }

            return false;
        }

        // 判断是否为修饰键
        private bool IsModifierKey(Keys key)
        {
            return key == Keys.ControlKey || key == Keys.ShiftKey || key == Keys.Menu ||
                   key == Keys.LControlKey || key == Keys.RControlKey ||
                   key == Keys.LShiftKey || key == Keys.RShiftKey ||
                   key == Keys.LMenu || key == Keys.RMenu;
        }

        // 获取格式化后的热键字符串
        public string GetHotkeyString()
        {
            List<string> parts = new List<string>();

            if ((modifierKeys & Keys.Control) == Keys.Control)
                parts.Add("Ctrl");
            if ((modifierKeys & Keys.Shift) == Keys.Shift)
                parts.Add("Shift");
            if ((modifierKeys & Keys.Alt) == Keys.Alt)
                parts.Add("Alt");

            if (keyCode != Keys.None && !IsModifierKey(keyCode))
                parts.Add(keyCode.ToString());

            return string.Join("+", parts);
        }

        // 清空热键
        public void Clear()
        {
            modifierKeys = Keys.None;
            keyCode = Keys.None;
            isCapturing = false;
        }
    }
}
