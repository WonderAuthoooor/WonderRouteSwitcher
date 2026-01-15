using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WifiProxySwitcher.pojo
{
    public static class WiFiManager
    {
        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);

        public static void SwitchToNetwork(string ssid, bool enableProxy, string proxyServer)
        {
            try
            {
                // 1. 切换 Wi-Fi
                var psi = new ProcessStartInfo
                {
                    FileName = "netsh",
                    Arguments = $"wlan connect name=\"{ssid}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (var process = Process.Start(psi))
                {
                    if (process != null)
                    {
                        process.WaitForExit(5000); // 最多等待5秒
                        if (process.ExitCode != 0)
                        {
                            var error = process.StandardError.ReadToEnd();
                            throw new Exception($"Wi-Fi 切换失败: {error}");
                        }
                    }
                }

                // 2. 设置代理
                using (var key = Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true))
                {
                    if (key == null) throw new Exception("无法访问注册表");

                    key.SetValue("ProxyEnable", enableProxy ? 1 : 0, RegistryValueKind.DWord);
                    if (enableProxy)
                    {
                        // 👇 请修改为你自己的代理地址！
                        key.SetValue("ProxyServer", proxyServer, RegistryValueKind.String);
                    }
                    else
                    {
                        key.DeleteValue("ProxyServer", throwOnMissingValue: false);
                    }

                    StdOutput.debugOutput($"切换成功:{ssid}");
                }

                // 3. 通知系统刷新网络设置
                RefreshSystemProxy();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"操作失败：Proxy错误",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // 定义常量
        private const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        private const int INTERNET_OPTION_REFRESH = 37;

        // 修改代理后刷新系统设置
        public static void RefreshSystemProxy()
        {
            // 通知系统代理设置已更改
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }
    }
}
