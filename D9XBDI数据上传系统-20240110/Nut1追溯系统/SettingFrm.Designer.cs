namespace 卓汇数据追溯系统
{
    partial class SettingFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txt_RemotePort = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txt_RemoteIP = new System.Windows.Forms.TextBox();
            this.txt_LocalPort = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txt_LocalIP = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txt_PlcPort = new System.Windows.Forms.TextBox();
            this.label38 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.txt_PlcIP = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_CommunicationSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txt_Operation = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txt_Resource = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txt_Site = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_MESURL = new System.Windows.Forms.TextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.cmb_Trace_Mac = new System.Windows.Forms.ComboBox();
            this.cmb_Trace_IP = new System.Windows.Forms.ComboBox();
            this.lbTrace_station_id = new System.Windows.Forms.Label();
            this.lbTrace_software_name = new System.Windows.Forms.Label();
            this.lbTrace_line_id = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.btn_savelaser = new System.Windows.Forms.Button();
            this.txt_power = new System.Windows.Forms.TextBox();
            this.txt_frequency = new System.Windows.Forms.TextBox();
            this.txt_waveform = new System.Windows.Forms.TextBox();
            this.txt_laser_speed = new System.Windows.Forms.TextBox();
            this.txt_jump_speed = new System.Windows.Forms.TextBox();
            this.txt_jump_delay = new System.Windows.Forms.TextBox();
            this.txt_position_delay = new System.Windows.Forms.TextBox();
            this.txt_pulse_profile = new System.Windows.Forms.TextBox();
            this.txt_laser_height = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl1.ItemSize = new System.Drawing.Size(30, 120);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1040, 461);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 3;
            this.tabControl1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl1_DrawItem);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tableLayoutPanel10);
            this.tabPage3.Location = new System.Drawing.Point(124, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(912, 453);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "通讯设置";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 2;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel10.Controls.Add(this.groupBox2, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.groupBox3, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel11, 1, 2);
            this.tableLayoutPanel10.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 3;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.25F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.5F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.25F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(912, 453);
            this.tableLayoutPanel10.TabIndex = 6;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.txt_RemotePort);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.txt_RemoteIP);
            this.groupBox2.Controls.Add(this.txt_LocalPort);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.txt_LocalIP);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(459, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(450, 135);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "TCP/IP 地址";
            // 
            // txt_RemotePort
            // 
            this.txt_RemotePort.Location = new System.Drawing.Point(309, 59);
            this.txt_RemotePort.Name = "txt_RemotePort";
            this.txt_RemotePort.Size = new System.Drawing.Size(77, 25);
            this.txt_RemotePort.TabIndex = 7;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(231, 27);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(72, 17);
            this.label15.TabIndex = 4;
            this.label15.Text = "Remote-IP:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(218, 63);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(85, 17);
            this.label16.TabIndex = 6;
            this.label16.Text = "Remote-Port:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_RemoteIP
            // 
            this.txt_RemoteIP.Location = new System.Drawing.Point(309, 24);
            this.txt_RemoteIP.Name = "txt_RemoteIP";
            this.txt_RemoteIP.Size = new System.Drawing.Size(121, 25);
            this.txt_RemoteIP.TabIndex = 5;
            // 
            // txt_LocalPort
            // 
            this.txt_LocalPort.Location = new System.Drawing.Point(81, 60);
            this.txt_LocalPort.Name = "txt_LocalPort";
            this.txt_LocalPort.Size = new System.Drawing.Size(77, 25);
            this.txt_LocalPort.TabIndex = 3;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(19, 28);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 17);
            this.label13.TabIndex = 0;
            this.label13.Text = "Local-IP:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 63);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(69, 17);
            this.label14.TabIndex = 2;
            this.label14.Text = "Local-Port:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_LocalIP
            // 
            this.txt_LocalIP.Location = new System.Drawing.Point(81, 25);
            this.txt_LocalIP.Name = "txt_LocalIP";
            this.txt_LocalIP.Size = new System.Drawing.Size(121, 25);
            this.txt_LocalIP.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.txt_PlcPort);
            this.groupBox3.Controls.Add(this.label38);
            this.groupBox3.Controls.Add(this.label37);
            this.groupBox3.Controls.Add(this.txt_PlcIP);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(450, 135);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "PLC 地址";
            // 
            // txt_PlcPort
            // 
            this.txt_PlcPort.Location = new System.Drawing.Point(84, 60);
            this.txt_PlcPort.Name = "txt_PlcPort";
            this.txt_PlcPort.Size = new System.Drawing.Size(77, 25);
            this.txt_PlcPort.TabIndex = 3;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(30, 28);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(47, 17);
            this.label38.TabIndex = 0;
            this.label38.Text = "PLC-IP:";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(17, 63);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(60, 17);
            this.label37.TabIndex = 2;
            this.label37.Text = "PLC-Port:";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_PlcIP
            // 
            this.txt_PlcIP.Location = new System.Drawing.Point(84, 25);
            this.txt_PlcIP.Name = "txt_PlcIP";
            this.txt_PlcIP.Size = new System.Drawing.Size(121, 25);
            this.txt_PlcIP.TabIndex = 1;
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.ColumnCount = 1;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel11.Controls.Add(this.btn_CommunicationSave, 0, 0);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(459, 313);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(450, 137);
            this.tableLayoutPanel11.TabIndex = 29;
            // 
            // btn_CommunicationSave
            // 
            this.btn_CommunicationSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_CommunicationSave.BackColor = System.Drawing.Color.Gold;
            this.btn_CommunicationSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_CommunicationSave.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_CommunicationSave.ForeColor = System.Drawing.SystemColors.Highlight;
            this.btn_CommunicationSave.Location = new System.Drawing.Point(176, 99);
            this.btn_CommunicationSave.Name = "btn_CommunicationSave";
            this.btn_CommunicationSave.Size = new System.Drawing.Size(98, 35);
            this.btn_CommunicationSave.TabIndex = 5;
            this.btn_CommunicationSave.Text = "保存";
            this.btn_CommunicationSave.UseVisualStyleBackColor = false;
            this.btn_CommunicationSave.Click += new System.EventHandler(this.btn_CommunicationSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txt_Operation);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txt_Resource);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txt_Site);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txt_MESURL);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Calibri", 10.5F);
            this.groupBox1.Location = new System.Drawing.Point(3, 144);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(450, 163);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MES参数";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 120);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(68, 17);
            this.label12.TabIndex = 8;
            this.label12.Text = "工序编码:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_Operation
            // 
            this.txt_Operation.Location = new System.Drawing.Point(84, 117);
            this.txt_Operation.Name = "txt_Operation";
            this.txt_Operation.Size = new System.Drawing.Size(121, 25);
            this.txt_Operation.TabIndex = 9;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 89);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(68, 17);
            this.label10.TabIndex = 6;
            this.label10.Text = "资源代码:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_Resource
            // 
            this.txt_Resource.Location = new System.Drawing.Point(84, 86);
            this.txt_Resource.Name = "txt_Resource";
            this.txt_Resource.Size = new System.Drawing.Size(121, 25);
            this.txt_Resource.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 58);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 17);
            this.label9.TabIndex = 4;
            this.label9.Text = "工厂代码:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_Site
            // 
            this.txt_Site.Location = new System.Drawing.Point(84, 55);
            this.txt_Site.Name = "txt_Site";
            this.txt_Site.Size = new System.Drawing.Size(121, 25);
            this.txt_Site.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 27);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 17);
            this.label8.TabIndex = 2;
            this.label8.Text = "MES-URL:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_MESURL
            // 
            this.txt_MESURL.Location = new System.Drawing.Point(84, 24);
            this.txt_MESURL.Name = "txt_MESURL";
            this.txt_MESURL.Size = new System.Drawing.Size(283, 25);
            this.txt_MESURL.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(124, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(912, 453);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "焊接参数";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.51276F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.48724F));
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(907, 41);
            this.tableLayoutPanel4.TabIndex = 4;
            // 
            // comboBox3
            // 
            this.comboBox3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(352, 10);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(162, 20);
            this.comboBox3.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label11.Location = new System.Drawing.Point(3, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(343, 41);
            this.label11.TabIndex = 0;
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 50);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 7;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(907, 303);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // cmb_Trace_Mac
            // 
            this.cmb_Trace_Mac.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmb_Trace_Mac.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmb_Trace_Mac.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmb_Trace_Mac.FormattingEnabled = true;
            this.cmb_Trace_Mac.Location = new System.Drawing.Point(277, 265);
            this.cmb_Trace_Mac.Name = "cmb_Trace_Mac";
            this.cmb_Trace_Mac.Size = new System.Drawing.Size(625, 29);
            this.cmb_Trace_Mac.TabIndex = 22;
            // 
            // cmb_Trace_IP
            // 
            this.cmb_Trace_IP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmb_Trace_IP.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmb_Trace_IP.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmb_Trace_IP.FormattingEnabled = true;
            this.cmb_Trace_IP.Location = new System.Drawing.Point(277, 222);
            this.cmb_Trace_IP.Name = "cmb_Trace_IP";
            this.cmb_Trace_IP.Size = new System.Drawing.Size(625, 29);
            this.cmb_Trace_IP.TabIndex = 21;
            // 
            // lbTrace_station_id
            // 
            this.lbTrace_station_id.AutoSize = true;
            this.lbTrace_station_id.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTrace_station_id.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lbTrace_station_id.Location = new System.Drawing.Point(277, 175);
            this.lbTrace_station_id.Name = "lbTrace_station_id";
            this.lbTrace_station_id.Size = new System.Drawing.Size(625, 40);
            this.lbTrace_station_id.TabIndex = 20;
            this.lbTrace_station_id.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbTrace_software_name
            // 
            this.lbTrace_software_name.AutoSize = true;
            this.lbTrace_software_name.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTrace_software_name.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lbTrace_software_name.Location = new System.Drawing.Point(277, 133);
            this.lbTrace_software_name.Name = "lbTrace_software_name";
            this.lbTrace_software_name.Size = new System.Drawing.Size(625, 40);
            this.lbTrace_software_name.TabIndex = 19;
            this.lbTrace_software_name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbTrace_line_id
            // 
            this.lbTrace_line_id.AutoSize = true;
            this.lbTrace_line_id.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTrace_line_id.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lbTrace_line_id.Location = new System.Drawing.Point(277, 91);
            this.lbTrace_line_id.Name = "lbTrace_line_id";
            this.lbTrace_line_id.Size = new System.Drawing.Size(625, 40);
            this.lbTrace_line_id.TabIndex = 18;
            this.lbTrace_line_id.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label7.Location = new System.Drawing.Point(277, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(625, 40);
            this.label7.TabIndex = 17;
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label6.Location = new System.Drawing.Point(5, 259);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(264, 42);
            this.label6.TabIndex = 16;
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label5.Location = new System.Drawing.Point(5, 217);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(264, 40);
            this.label5.TabIndex = 15;
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label4.Location = new System.Drawing.Point(5, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(264, 40);
            this.label4.TabIndex = 14;
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label3.Location = new System.Drawing.Point(5, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(264, 40);
            this.label3.TabIndex = 13;
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label2.Location = new System.Drawing.Point(5, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(264, 40);
            this.label2.TabIndex = 12;
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label1.Location = new System.Drawing.Point(5, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 40);
            this.label1.TabIndex = 11;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.LimeGreen;
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.button2.Location = new System.Drawing.Point(2, 2);
            this.button2.Margin = new System.Windows.Forms.Padding(0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(270, 45);
            this.button2.TabIndex = 0;
            this.button2.Text = "配置";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.button1.BackColor = System.Drawing.Color.Gold;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.button1.Location = new System.Drawing.Point(407, 359);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 35);
            this.button1.TabIndex = 3;
            this.button1.Text = "修改";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(3, 12);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(450, 21);
            this.label17.TabIndex = 0;
            this.label17.Text = "power";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.txt_laser_height, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.txt_pulse_profile, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.txt_position_delay, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.txt_jump_delay, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.txt_jump_speed, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.txt_laser_speed, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.txt_waveform, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txt_frequency, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label17, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label18, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label19, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label20, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label21, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label22, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label23, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label24, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label25, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.btn_savelaser, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.txt_power, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0001F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0001F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0001F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0001F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0001F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0001F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0001F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0001F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0001F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.9991F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(912, 453);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label18.Location = new System.Drawing.Point(3, 57);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(450, 21);
            this.label18.TabIndex = 1;
            this.label18.Text = "frequency";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label19
            // 
            this.label19.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label19.Location = new System.Drawing.Point(3, 102);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(450, 21);
            this.label19.TabIndex = 1;
            this.label19.Text = "waveform";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label20.Location = new System.Drawing.Point(3, 147);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(450, 21);
            this.label20.TabIndex = 1;
            this.label20.Text = "laser_speed";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label21
            // 
            this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label21.Location = new System.Drawing.Point(3, 192);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(450, 21);
            this.label21.TabIndex = 1;
            this.label21.Text = "jump_speed";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label22
            // 
            this.label22.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label22.Location = new System.Drawing.Point(3, 237);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(450, 21);
            this.label22.TabIndex = 1;
            this.label22.Text = "jump_delay";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label23.Location = new System.Drawing.Point(3, 282);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(450, 21);
            this.label23.TabIndex = 1;
            this.label23.Text = "position_delay";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label24
            // 
            this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label24.Location = new System.Drawing.Point(3, 327);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(450, 21);
            this.label24.TabIndex = 1;
            this.label24.Text = "pulse_profile";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label25.Location = new System.Drawing.Point(3, 372);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(450, 21);
            this.label25.TabIndex = 1;
            this.label25.Text = "laser_height";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_savelaser
            // 
            this.btn_savelaser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btn_savelaser.BackColor = System.Drawing.Color.Gold;
            this.btn_savelaser.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_savelaser.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_savelaser.ForeColor = System.Drawing.SystemColors.Highlight;
            this.btn_savelaser.Location = new System.Drawing.Point(179, 408);
            this.btn_savelaser.Name = "btn_savelaser";
            this.btn_savelaser.Size = new System.Drawing.Size(98, 42);
            this.btn_savelaser.TabIndex = 6;
            this.btn_savelaser.Text = "保存";
            this.btn_savelaser.UseVisualStyleBackColor = false;
            this.btn_savelaser.Click += new System.EventHandler(this.btn_savelaser_Click);
            // 
            // txt_power
            // 
            this.txt_power.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_power.Location = new System.Drawing.Point(459, 12);
            this.txt_power.Name = "txt_power";
            this.txt_power.Size = new System.Drawing.Size(151, 21);
            this.txt_power.TabIndex = 7;
            // 
            // txt_frequency
            // 
            this.txt_frequency.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_frequency.Location = new System.Drawing.Point(459, 57);
            this.txt_frequency.Name = "txt_frequency";
            this.txt_frequency.Size = new System.Drawing.Size(151, 21);
            this.txt_frequency.TabIndex = 8;
            // 
            // txt_waveform
            // 
            this.txt_waveform.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_waveform.Location = new System.Drawing.Point(459, 102);
            this.txt_waveform.Name = "txt_waveform";
            this.txt_waveform.Size = new System.Drawing.Size(151, 21);
            this.txt_waveform.TabIndex = 9;
            // 
            // txt_laser_speed
            // 
            this.txt_laser_speed.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_laser_speed.Location = new System.Drawing.Point(459, 147);
            this.txt_laser_speed.Name = "txt_laser_speed";
            this.txt_laser_speed.Size = new System.Drawing.Size(151, 21);
            this.txt_laser_speed.TabIndex = 10;
            // 
            // txt_jump_speed
            // 
            this.txt_jump_speed.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_jump_speed.Location = new System.Drawing.Point(459, 192);
            this.txt_jump_speed.Name = "txt_jump_speed";
            this.txt_jump_speed.Size = new System.Drawing.Size(151, 21);
            this.txt_jump_speed.TabIndex = 11;
            // 
            // txt_jump_delay
            // 
            this.txt_jump_delay.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_jump_delay.Location = new System.Drawing.Point(459, 237);
            this.txt_jump_delay.Name = "txt_jump_delay";
            this.txt_jump_delay.Size = new System.Drawing.Size(151, 21);
            this.txt_jump_delay.TabIndex = 12;
            // 
            // txt_position_delay
            // 
            this.txt_position_delay.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_position_delay.Location = new System.Drawing.Point(459, 282);
            this.txt_position_delay.Name = "txt_position_delay";
            this.txt_position_delay.Size = new System.Drawing.Size(151, 21);
            this.txt_position_delay.TabIndex = 13;
            // 
            // txt_pulse_profile
            // 
            this.txt_pulse_profile.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_pulse_profile.Location = new System.Drawing.Point(459, 327);
            this.txt_pulse_profile.Name = "txt_pulse_profile";
            this.txt_pulse_profile.Size = new System.Drawing.Size(151, 21);
            this.txt_pulse_profile.TabIndex = 14;
            // 
            // txt_laser_height
            // 
            this.txt_laser_height.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_laser_height.Location = new System.Drawing.Point(459, 372);
            this.txt_laser_height.Name = "txt_laser_height";
            this.txt_laser_height.Size = new System.Drawing.Size(151, 21);
            this.txt_laser_height.TabIndex = 15;
            // 
            // SettingFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 461);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SettingFrm";
            this.Text = "SettingFrm";
            this.Load += new System.EventHandler(this.SettingFrm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tableLayoutPanel11.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.TextBox txt_PlcPort;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.TextBox txt_PlcIP;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ComboBox cmb_Trace_Mac;
        private System.Windows.Forms.ComboBox cmb_Trace_IP;
        private System.Windows.Forms.Label lbTrace_station_id;
        private System.Windows.Forms.Label lbTrace_software_name;
        private System.Windows.Forms.Label lbTrace_line_id;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private System.Windows.Forms.Button btn_CommunicationSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txt_Operation;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txt_Resource;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txt_Site;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_MESURL;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txt_RemotePort;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txt_RemoteIP;
        private System.Windows.Forms.TextBox txt_LocalPort;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txt_LocalIP;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Button btn_savelaser;
        private System.Windows.Forms.TextBox txt_laser_height;
        private System.Windows.Forms.TextBox txt_pulse_profile;
        private System.Windows.Forms.TextBox txt_position_delay;
        private System.Windows.Forms.TextBox txt_jump_delay;
        private System.Windows.Forms.TextBox txt_jump_speed;
        private System.Windows.Forms.TextBox txt_laser_speed;
        private System.Windows.Forms.TextBox txt_waveform;
        private System.Windows.Forms.TextBox txt_frequency;
        private System.Windows.Forms.TextBox txt_power;
    }
}