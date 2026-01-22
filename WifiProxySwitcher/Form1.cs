using System.ComponentModel;
using System.Reflection.Metadata;
using System.Text;
using System.Windows.Forms;
using WifiProxySwitcher.pojo;

namespace WifiProxySwitcher
{
    public partial class MainForm : Form
    {

        // 定义绑定列表，用于存储代理设置数据
        private BindingList<ProxySetting> proxySettings;

        private GlobalHotkey globalHotkey;

        // Windows消息常量
        private const int WM_HOTKEY = 0x0312;


        public MainForm()
        {
            InitializeComponent();
            InitDatagridView();
            InitializeConfig();
            SetupHotkeyHandling();

            // 确保句柄已创建（Handle属性首次访问会创建句柄）
            if (this.Handle == IntPtr.Zero)
            {
                var handle = this.Handle; // 强制创建句柄
            }
        }

        private void InitDatagridView()
        {
            /*            // 1. 设置DataGridView基本属性
                        configTable.AllowUserToAddRows = false; // 禁止用户直接添加行
                        configTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        configTable.BackgroundColor = Color.LightGray; // 设置浅灰色背景*/

            // 手动设置列的DataPropertyName
            shortcut.DataPropertyName = "ShortcutKey";
            WIFIName.DataPropertyName = "WifiName";
            isProxy.DataPropertyName = "EnableProxy";
            proxyServer.DataPropertyName = "ProxyAddress";

            StdOutput.Init(txtDebugOutput);
        }


        private HotkeyCapture hotkeyCapture = new HotkeyCapture();
        private int capturingRowIndex = -1; // 正在捕获热键的行索引


        private void SetupHotkeyHandling()
        {
            // 监听键盘事件
            this.KeyPreview = true;
            this.KeyDown += MainForm_KeyDown;
            this.KeyUp += MainForm_KeyUp;

        }

        private void ConfigTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            // 显示提示信息
            StdOutput.debugOutput($"单元格被点击！行:{e.RowIndex}, 列:{e.ColumnIndex}");

            // 判断是否是快捷键列的编辑按钮被点击
            if (e.ColumnIndex == configTable.Columns["shortcut"].Index && e.RowIndex >= 0)
            {
                // 开始捕获热键
                hotkeyCapture.StartCapture();
                capturingRowIndex = e.RowIndex;

                // 提示用户
                configTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "按组合键...";
                configTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Yellow;

                // 显示提示信息
                StdOutput.debugOutput("正在捕获热键，请按下组合键（如Ctrl+Shift+Alt+F4）");
            }

            // 判断是否是生效列的编辑按钮被点击
            if (e.ColumnIndex == configTable.Columns["btnApplyWifi"].Index && e.RowIndex >= 0)
            {
                //执行当前生效
                proxySettings[e.RowIndex].Execute();
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (hotkeyCapture.IsCapturing)
            {
                // 处理热键捕获
                if (hotkeyCapture.ProcessKey(e))
                {
                    CaptureEnd(e);
                }
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            // ESC键取消热键捕获
            if (hotkeyCapture.IsCapturing)
            {
                CaptureEnd(e);
            }
        }

        /// <summary>
        /// 捕获热键完成
        /// </summary>
        private void CaptureEnd(KeyEventArgs e)
        {
            // 捕获完成，更新表格
            string hotkeyString = hotkeyCapture.GetHotkeyString();

            // 更新当前行的快捷键单元格
            if (capturingRowIndex >= 0)
            {
                configTable.Rows[capturingRowIndex].Cells["shortcut"].Value = hotkeyString;
                configTable.Rows[capturingRowIndex].Cells["shortcut"].Style.BackColor = Color.White;

                // 更新数据源
                if (proxySettings != null && capturingRowIndex < proxySettings.Count)
                {
                    proxySettings[capturingRowIndex].ShortcutKey = hotkeyString;
                    /*SaveConfig(); // 保存配置*/
                }

                StdOutput.debugOutput($"已设置热键: {hotkeyString}");
            }

            // 停止捕获
            hotkeyCapture.StopCapture();
            capturingRowIndex = -1;

            // 阻止按键事件进一步传播
            e.Handled = true;
            e.SuppressKeyPress = true;
        }


        private void InitializeConfig()
        {

            // 初始化全局热键管理器
            globalHotkey = new GlobalHotkey(this.Handle);

            // 加载配置
            LoadConfig();

        }

        private void LoadConfig()
        {
            try
            {
                // 从配置文件加载设置
                var settingsList = AppConfig.LoadSettings();

                // 转换为BindingList
                proxySettings = new BindingList<ProxySetting>(settingsList);

                // 绑定到DataGridView
                configTable.DataSource = proxySettings;


                StdOutput.debugOutput($"已加载 {proxySettings.Count} 条配置");


                RegisterAllHotKey();
            }
            catch (Exception ex)
            {
                StdOutput.debugOutput($"加载配置失败: {ex.Message}");
                // 创建空列表
                proxySettings = new BindingList<ProxySetting>();
                configTable.DataSource = proxySettings;
            }
        }

        private void SaveConfig()
        {
            try
            {
                // 将BindingList转换为List
                var settingsList = proxySettings.ToList();

                // 保存到配置文件
                AppConfig.SaveSettings(settingsList);

                StdOutput.debugOutput("配置已自动保存");
            }
            catch (Exception ex)
            {
                StdOutput.debugOutput($"保存配置失败: {ex.Message}");
            }
        }



        // 处理Windows消息（热键消息）
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                StdOutput.debugOutput($"收到热键消息，ID：{m.WParam.ToInt32()}");
                // 热键被触发，让GlobalHotkey处理
                globalHotkey.ProcessHotkey(m.WParam.ToInt32());
            }

            base.WndProc(ref m);
        }


        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        /// <summary>
        /// btnApply 生效按钮 click 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_1(object sender, EventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in proxySettings)
            {
                stringBuilder.Append(item.ToString());
                string txt = "已生效:" + stringBuilder.ToString();
                StdOutput.debugOutput(txt);
            }

            //保存配置到本地
            SaveConfig();

            RegisterAllHotKey();


        }

        /// <summary>
        /// 重新注册所有热键
        /// </summary>
        private void RegisterAllHotKey()
        {
            globalHotkey.Dispose();
            foreach (var item in proxySettings)
            {

                globalHotkey.RegisterHotkey(item);
            }
        }





        private void configTable_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            globalHotkey.Dispose();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            globalHotkey.Dispose();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {

        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = false; // 仅隐藏视觉显示，保留消息循环
                                      // this.ShowInTaskbar = false; // 注释/删除这行，或保持为true
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000, "提示", "程序已最小化到系统托盘", ToolTipIcon.Info);
            }
            else
            {
                this.Visible = true;
                this.ShowInTaskbar = true;
                notifyIcon1.Visible = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            RestoreWindow();
        }

        private void RestoreWindow()
        {
            this.Visible = true; // 恢复窗体可见性
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;
            this.Focus();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RestoreWindow();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
