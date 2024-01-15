namespace 卓汇数据追溯系统
{
    partial class MainFrm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btn_home = new System.Windows.Forms.ToolStripButton();
            this.btn_Manual = new System.Windows.Forms.ToolStripButton();
            this.btn_Setting = new System.Windows.Forms.ToolStripButton();
            this.btn_UserLogin = new System.Windows.Forms.ToolStripButton();
            this.btn_Help = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.label_MachineName = new System.Windows.Forms.ToolStripLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslbl_UserLogin = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_PLCStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_TraceStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_HansStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslbl_MachineStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslbl_ErrorMeg = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslbl_time = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(50, 50);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_home,
            this.btn_Manual,
            this.btn_Setting,
            this.btn_UserLogin,
            this.btn_Help,
            this.toolStripButton9,
            this.toolStripButton10,
            this.toolStripButton8,
            this.label_MachineName});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(1184, 57);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btn_home
            // 
            this.btn_home.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btn_home.Image = ((System.Drawing.Image)(resources.GetObject("btn_home.Image")));
            this.btn_home.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_home.Name = "btn_home";
            this.btn_home.Size = new System.Drawing.Size(54, 54);
            this.btn_home.Text = "主界面";
            this.btn_home.Click += new System.EventHandler(this.btn_home_Click);
            // 
            // btn_Manual
            // 
            this.btn_Manual.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btn_Manual.Image = ((System.Drawing.Image)(resources.GetObject("btn_Manual.Image")));
            this.btn_Manual.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_Manual.Name = "btn_Manual";
            this.btn_Manual.Size = new System.Drawing.Size(54, 54);
            this.btn_Manual.Text = "toolStripButton1";
            this.btn_Manual.Click += new System.EventHandler(this.btn_Manual_Click);
            // 
            // btn_Setting
            // 
            this.btn_Setting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btn_Setting.Image = ((System.Drawing.Image)(resources.GetObject("btn_Setting.Image")));
            this.btn_Setting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_Setting.Name = "btn_Setting";
            this.btn_Setting.Size = new System.Drawing.Size(54, 54);
            this.btn_Setting.Text = "参数设置";
            this.btn_Setting.Click += new System.EventHandler(this.btn_Setting_Click);
            // 
            // btn_UserLogin
            // 
            this.btn_UserLogin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btn_UserLogin.Image = ((System.Drawing.Image)(resources.GetObject("btn_UserLogin.Image")));
            this.btn_UserLogin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_UserLogin.Name = "btn_UserLogin";
            this.btn_UserLogin.Size = new System.Drawing.Size(54, 54);
            this.btn_UserLogin.Text = "用户登录";
            this.btn_UserLogin.Click += new System.EventHandler(this.btn_UserLogin_Click);
            // 
            // btn_Help
            // 
            this.btn_Help.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btn_Help.Image = ((System.Drawing.Image)(resources.GetObject("btn_Help.Image")));
            this.btn_Help.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_Help.Name = "btn_Help";
            this.btn_Help.Size = new System.Drawing.Size(54, 54);
            this.btn_Help.Text = "帮助信息";
            this.btn_Help.Click += new System.EventHandler(this.btn_Help_Click);
            // 
            // toolStripButton9
            // 
            this.toolStripButton9.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton9.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripButton9.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton9.Image")));
            this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripButton9.Size = new System.Drawing.Size(54, 54);
            this.toolStripButton9.Text = "停止";
            this.toolStripButton9.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // toolStripButton10
            // 
            this.toolStripButton10.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton10.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripButton10.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton10.Image")));
            this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton10.Name = "toolStripButton10";
            this.toolStripButton10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripButton10.Size = new System.Drawing.Size(54, 54);
            this.toolStripButton10.Text = "暂停";
            this.toolStripButton10.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton8.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.toolStripButton8.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton8.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton8.Image")));
            this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripButton8.Size = new System.Drawing.Size(54, 54);
            this.toolStripButton8.Text = "启动";
            this.toolStripButton8.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // label_MachineName
            // 
            this.label_MachineName.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.label_MachineName.BackColor = System.Drawing.SystemColors.Control;
            this.label_MachineName.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.label_MachineName.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_MachineName.Name = "label_MachineName";
            this.label_MachineName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label_MachineName.Size = new System.Drawing.Size(132, 54);
            this.label_MachineName.Text = "D83LC-EVT";
            this.label_MachineName.Click += new System.EventHandler(this.label_MachineName_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslbl_UserLogin,
            this.tssl_PLCStatus,
            this.tssl_TraceStatus,
            this.tssl_HansStatus,
            this.tsslbl_MachineStatus,
            this.tsslbl_ErrorMeg,
            this.tsslbl_time});
            this.statusStrip1.Location = new System.Drawing.Point(0, 536);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1184, 25);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Tag = "";
            this.statusStrip1.Text = "当前用户：all user";
            // 
            // tsslbl_UserLogin
            // 
            this.tsslbl_UserLogin.BackColor = System.Drawing.Color.Yellow;
            this.tsslbl_UserLogin.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsslbl_UserLogin.Name = "tsslbl_UserLogin";
            this.tsslbl_UserLogin.Size = new System.Drawing.Size(121, 20);
            this.tsslbl_UserLogin.Text = "当前用户：技术员";
            // 
            // tssl_PLCStatus
            // 
            this.tssl_PLCStatus.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.tssl_PLCStatus.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tssl_PLCStatus.Name = "tssl_PLCStatus";
            this.tssl_PLCStatus.Size = new System.Drawing.Size(76, 20);
            this.tssl_PLCStatus.Text = "已连接PLC";
            // 
            // tssl_TraceStatus
            // 
            this.tssl_TraceStatus.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.tssl_TraceStatus.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tssl_TraceStatus.Name = "tssl_TraceStatus";
            this.tssl_TraceStatus.Size = new System.Drawing.Size(107, 20);
            this.tssl_TraceStatus.Text = "已连接校验接口";
            // 
            // tssl_HansStatus
            // 
            this.tssl_HansStatus.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.tssl_HansStatus.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.tssl_HansStatus.Name = "tssl_HansStatus";
            this.tssl_HansStatus.Size = new System.Drawing.Size(79, 20);
            this.tssl_HansStatus.Text = "焊机已连接";
            // 
            // tsslbl_MachineStatus
            // 
            this.tsslbl_MachineStatus.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsslbl_MachineStatus.Name = "tsslbl_MachineStatus";
            this.tsslbl_MachineStatus.RightToLeftAutoMirrorImage = true;
            this.tsslbl_MachineStatus.Size = new System.Drawing.Size(121, 20);
            this.tsslbl_MachineStatus.Text = "状态：设备自动中";
            this.tsslbl_MachineStatus.Visible = false;
            // 
            // tsslbl_ErrorMeg
            // 
            this.tsslbl_ErrorMeg.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsslbl_ErrorMeg.Name = "tsslbl_ErrorMeg";
            this.tsslbl_ErrorMeg.Size = new System.Drawing.Size(641, 20);
            this.tsslbl_ErrorMeg.Spring = true;
            this.tsslbl_ErrorMeg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsslbl_time
            // 
            this.tsslbl_time.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsslbl_time.Name = "tsslbl_time";
            this.tsslbl_time.Size = new System.Drawing.Size(143, 20);
            this.tsslbl_time.Text = "2023-01-01 12:11:11";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 561);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainFrm";
            this.RightToLeftLayout = true;
            this.Text = "数据追溯系统";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFrm_FormClosing);
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btn_home;
        private System.Windows.Forms.ToolStripButton btn_Setting;
        private System.Windows.Forms.ToolStripButton btn_UserLogin;
        private System.Windows.Forms.ToolStripButton btn_Help;
        private System.Windows.Forms.ToolStripButton toolStripButton8;
        private System.Windows.Forms.ToolStripButton toolStripButton10;
        private System.Windows.Forms.ToolStripButton toolStripButton9;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslbl_UserLogin;
        private System.Windows.Forms.ToolStripStatusLabel tsslbl_ErrorMeg;
        private System.Windows.Forms.ToolStripStatusLabel tsslbl_MachineStatus;
        private System.Windows.Forms.ToolStripStatusLabel tsslbl_time;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripLabel label_MachineName;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ToolStripStatusLabel tssl_PLCStatus;
        private System.Windows.Forms.ToolStripStatusLabel tssl_TraceStatus;
        private System.Windows.Forms.ToolStripButton btn_Manual;
        private System.Windows.Forms.ToolStripStatusLabel tssl_HansStatus;
    }
}

