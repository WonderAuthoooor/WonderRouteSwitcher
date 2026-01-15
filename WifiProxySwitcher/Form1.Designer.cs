namespace WifiProxySwitcher
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            txtDebugOutput = new TextBox();
            label1 = new Label();
            configTable = new DataGridView();
            shortcut = new DataGridViewButtonColumn();
            WIFIName = new DataGridViewTextBoxColumn();
            isProxy = new DataGridViewCheckBoxColumn();
            proxyServer = new DataGridViewTextBoxColumn();
            btnApplyWifi = new DataGridViewButtonColumn();
            btnApply = new Button();
            notifyIcon1 = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            exitToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)configTable).BeginInit();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // txtDebugOutput
            // 
            txtDebugOutput.Location = new Point(38, 291);
            txtDebugOutput.Multiline = true;
            txtDebugOutput.Name = "txtDebugOutput";
            txtDebugOutput.Size = new Size(706, 133);
            txtDebugOutput.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(38, 259);
            label1.Name = "label1";
            label1.Size = new Size(80, 17);
            label1.TabIndex = 4;
            label1.Text = "调试输出窗口";
            label1.Click += label1_Click;
            // 
            // configTable
            // 
            configTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            configTable.Columns.AddRange(new DataGridViewColumn[] { shortcut, WIFIName, isProxy, proxyServer, btnApplyWifi });
            configTable.Location = new Point(38, 31);
            configTable.Name = "configTable";
            configTable.Size = new Size(670, 125);
            configTable.TabIndex = 7;
            configTable.CellContentClick += ConfigTable_CellContentClick;
            // 
            // shortcut
            // 
            shortcut.HeaderText = "快捷键";
            shortcut.Name = "shortcut";
            shortcut.Resizable = DataGridViewTriState.True;
            shortcut.SortMode = DataGridViewColumnSortMode.Automatic;
            shortcut.Width = 150;
            // 
            // WIFIName
            // 
            WIFIName.HeaderText = "WiFi名称";
            WIFIName.Name = "WIFIName";
            // 
            // isProxy
            // 
            isProxy.HeaderText = "是否开启代理";
            isProxy.Name = "isProxy";
            isProxy.Width = 75;
            // 
            // proxyServer
            // 
            proxyServer.HeaderText = "代理服务器地址";
            proxyServer.Name = "proxyServer";
            proxyServer.Width = 200;
            // 
            // btnApplyWifi
            // 
            btnApplyWifi.HeaderText = "应用";
            btnApplyWifi.Name = "btnApplyWifi";
            btnApplyWifi.Text = "生效";
            // 
            // btnApply
            // 
            btnApply.Location = new Point(38, 178);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(78, 36);
            btnApply.TabIndex = 9;
            btnApply.Text = "生效";
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += button1_Click_1;
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "notifyIcon1";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(97, 26);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(96, 22);
            exitToolStripMenuItem.Text = "exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnApply);
            Controls.Add(configTable);
            Controls.Add(label1);
            Controls.Add(txtDebugOutput);
            Icon = (Icon)resources.GetObject("$this.Icon");
            IsMdiContainer = true;
            Name = "MainForm";
            Text = "WonderRoute";
            FormClosing += MainForm_FormClosing;
            FormClosed += MainForm_FormClosed;
            SizeChanged += MainForm_SizeChanged;
            Resize += MainForm_Resize;
            ((System.ComponentModel.ISupportInitialize)configTable).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtDebugOutput;
        private Label label1;
        private DataGridView configTable;
        private Button btnApply;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private DataGridViewButtonColumn shortcut;
        private DataGridViewTextBoxColumn WIFIName;
        private DataGridViewCheckBoxColumn isProxy;
        private DataGridViewTextBoxColumn proxyServer;
        private DataGridViewButtonColumn btnApplyWifi;
    }
}
