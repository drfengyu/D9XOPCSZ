namespace 卓汇数据追溯系统
{
    partial class ManualFrm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button5 = new System.Windows.Forms.Button();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.dtp_Start = new System.Windows.Forms.DateTimePicker();
            this.btn_Output = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.dtp_End = new System.Windows.Forms.DateTimePicker();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chart_TotalDT = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lb_errorMsg = new System.Windows.Forms.Label();
            this.lv_OEEData = new System.Windows.Forms.ListView();
            this.panel5 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.button10 = new System.Windows.Forms.Button();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.Btn_UpLoad_errortime = new System.Windows.Forms.Button();
            this.Btn_Start_errortime = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.Btn_Start_break = new System.Windows.Forms.Button();
            this.Btn_UpLoad_break = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.CB_errorinfo = new System.Windows.Forms.ComboBox();
            this.LB_ErrorCode = new System.Windows.Forms.Label();
            this.btnManualOEEStatus = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.LB_ManualSelect = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_TotalDT)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl1.ItemSize = new System.Drawing.Size(30, 120);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(924, 500);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 3;
            this.tabControl1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl1_DrawItem);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.tableLayoutPanel4);
            this.tabPage5.Location = new System.Drawing.Point(124, 4);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(796, 492);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "OEE模组";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.16777F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.83223F));
            this.tableLayoutPanel4.Controls.Add(this.splitContainer1, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.panel5, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45.3252F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 54.6748F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(796, 492);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(463, 222);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button5);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel8);
            this.splitContainer1.Size = new System.Drawing.Size(333, 270);
            this.splitContainer1.SplitterDistance = 62;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 11;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Coral;
            this.button5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button5.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.button5.Location = new System.Drawing.Point(0, 0);
            this.button5.Margin = new System.Windows.Forms.Padding(0);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(333, 62);
            this.button5.TabIndex = 0;
            this.button5.Text = "手动导出数据";
            this.button5.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.Controls.Add(this.dtp_Start, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.btn_Output, 1, 2);
            this.tableLayoutPanel8.Controls.Add(this.label20, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.label21, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.dtp_End, 1, 1);
            this.tableLayoutPanel8.Controls.Add(this.btn_Refresh, 0, 2);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 3;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(333, 207);
            this.tableLayoutPanel8.TabIndex = 0;
            // 
            // dtp_Start
            // 
            this.dtp_Start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtp_Start.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.dtp_Start.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.dtp_Start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Start.Location = new System.Drawing.Point(170, 20);
            this.dtp_Start.Name = "dtp_Start";
            this.dtp_Start.Size = new System.Drawing.Size(158, 29);
            this.dtp_Start.TabIndex = 6;
            // 
            // btn_Output
            // 
            this.btn_Output.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Output.BackColor = System.Drawing.Color.LimeGreen;
            this.btn_Output.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Output.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btn_Output.Location = new System.Drawing.Point(197, 155);
            this.btn_Output.Margin = new System.Windows.Forms.Padding(30, 0, 30, 0);
            this.btn_Output.Name = "btn_Output";
            this.btn_Output.Size = new System.Drawing.Size(104, 32);
            this.btn_Output.TabIndex = 3;
            this.btn_Output.Text = "输出";
            this.btn_Output.UseVisualStyleBackColor = false;
            this.btn_Output.Click += new System.EventHandler(this.btn_Output_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label20.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label20.Location = new System.Drawing.Point(5, 2);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(157, 66);
            this.label20.TabIndex = 4;
            this.label20.Text = "开始时间：";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label21.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label21.Location = new System.Drawing.Point(5, 70);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(157, 66);
            this.label21.TabIndex = 5;
            this.label21.Text = "结束时间：";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtp_End
            // 
            this.dtp_End.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtp_End.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.dtp_End.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.dtp_End.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_End.Location = new System.Drawing.Point(170, 88);
            this.dtp_End.Name = "dtp_End";
            this.dtp_End.Size = new System.Drawing.Size(158, 29);
            this.dtp_End.TabIndex = 7;
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Refresh.BackColor = System.Drawing.Color.Aquamarine;
            this.btn_Refresh.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Refresh.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btn_Refresh.Location = new System.Drawing.Point(32, 155);
            this.btn_Refresh.Margin = new System.Windows.Forms.Padding(30, 0, 30, 0);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(103, 32);
            this.btn_Refresh.TabIndex = 2;
            this.btn_Refresh.Text = "刷新";
            this.btn_Refresh.UseVisualStyleBackColor = false;
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chart_TotalDT);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(466, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(327, 216);
            this.panel1.TabIndex = 13;
            // 
            // chart_TotalDT
            // 
            chartArea1.Name = "ChartArea1";
            this.chart_TotalDT.ChartAreas.Add(chartArea1);
            this.chart_TotalDT.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart_TotalDT.Legends.Add(legend1);
            this.chart_TotalDT.Location = new System.Drawing.Point(0, 0);
            this.chart_TotalDT.Name = "chart_TotalDT";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.CustomProperties = "MinimumRelativePieSize=50, PieLabelStyle=Outside";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart_TotalDT.Series.Add(series1);
            this.chart_TotalDT.Size = new System.Drawing.Size(327, 216);
            this.chart_TotalDT.TabIndex = 1;
            this.chart_TotalDT.Text = "chart1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lb_errorMsg);
            this.panel2.Controls.Add(this.lv_OEEData);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 225);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(457, 264);
            this.panel2.TabIndex = 14;
            // 
            // lb_errorMsg
            // 
            this.lb_errorMsg.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lb_errorMsg.AutoSize = true;
            this.lb_errorMsg.BackColor = System.Drawing.Color.Red;
            this.lb_errorMsg.Font = new System.Drawing.Font("宋体", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_errorMsg.Location = new System.Drawing.Point(49, 103);
            this.lb_errorMsg.Name = "lb_errorMsg";
            this.lb_errorMsg.Size = new System.Drawing.Size(220, 56);
            this.lb_errorMsg.TabIndex = 21;
            this.lb_errorMsg.Text = "首件中!";
            this.lb_errorMsg.Visible = false;
            // 
            // lv_OEEData
            // 
            this.lv_OEEData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_OEEData.HideSelection = false;
            this.lv_OEEData.Location = new System.Drawing.Point(0, 0);
            this.lv_OEEData.Name = "lv_OEEData";
            this.lv_OEEData.Size = new System.Drawing.Size(457, 264);
            this.lv_OEEData.TabIndex = 20;
            this.lv_OEEData.UseCompatibleStateImageBehavior = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.tableLayoutPanel7);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(457, 216);
            this.panel5.TabIndex = 17;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Controls.Add(this.button10, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel11, 0, 1);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(457, 216);
            this.tableLayoutPanel7.TabIndex = 16;
            // 
            // button10
            // 
            this.button10.BackColor = System.Drawing.Color.Coral;
            this.button10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button10.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.button10.Location = new System.Drawing.Point(1, 1);
            this.button10.Margin = new System.Windows.Forms.Padding(0);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(455, 53);
            this.button10.TabIndex = 1;
            this.button10.Text = "停机计时";
            this.button10.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel11.ColumnCount = 4;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel11.Controls.Add(this.Btn_UpLoad_errortime, 1, 2);
            this.tableLayoutPanel11.Controls.Add(this.Btn_Start_errortime, 0, 2);
            this.tableLayoutPanel11.Controls.Add(this.button2, 2, 1);
            this.tableLayoutPanel11.Controls.Add(this.Btn_Start_break, 0, 1);
            this.tableLayoutPanel11.Controls.Add(this.Btn_UpLoad_break, 1, 1);
            this.tableLayoutPanel11.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel11.Controls.Add(this.CB_errorinfo, 1, 0);
            this.tableLayoutPanel11.Controls.Add(this.LB_ErrorCode, 2, 0);
            this.tableLayoutPanel11.Controls.Add(this.btnManualOEEStatus, 3, 0);
            this.tableLayoutPanel11.Controls.Add(this.label7, 2, 2);
            this.tableLayoutPanel11.Controls.Add(this.LB_ManualSelect, 3, 2);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(4, 58);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 3;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(449, 154);
            this.tableLayoutPanel11.TabIndex = 0;
            // 
            // Btn_UpLoad_errortime
            // 
            this.Btn_UpLoad_errortime.BackColor = System.Drawing.Color.Transparent;
            this.Btn_UpLoad_errortime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Btn_UpLoad_errortime.Enabled = false;
            this.Btn_UpLoad_errortime.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.Btn_UpLoad_errortime.Location = new System.Drawing.Point(116, 106);
            this.Btn_UpLoad_errortime.Name = "Btn_UpLoad_errortime";
            this.Btn_UpLoad_errortime.Size = new System.Drawing.Size(105, 44);
            this.Btn_UpLoad_errortime.TabIndex = 36;
            this.Btn_UpLoad_errortime.Text = "首件结束上传";
            this.Btn_UpLoad_errortime.UseVisualStyleBackColor = false;
            this.Btn_UpLoad_errortime.Click += new System.EventHandler(this.Btn_UpLoad_errortime_Click);
            // 
            // Btn_Start_errortime
            // 
            this.Btn_Start_errortime.BackColor = System.Drawing.Color.Transparent;
            this.Btn_Start_errortime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Btn_Start_errortime.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_Start_errortime.Location = new System.Drawing.Point(4, 106);
            this.Btn_Start_errortime.Name = "Btn_Start_errortime";
            this.Btn_Start_errortime.Size = new System.Drawing.Size(105, 44);
            this.Btn_Start_errortime.TabIndex = 34;
            this.Btn_Start_errortime.Text = "首件开始计时";
            this.Btn_Start_errortime.UseVisualStyleBackColor = false;
            this.Btn_Start_errortime.Click += new System.EventHandler(this.Btn_Start_errortime_Click);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.button2.Location = new System.Drawing.Point(228, 55);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(105, 44);
            this.button2.TabIndex = 32;
            this.button2.Text = "调试模式(不上传Trace)";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            // 
            // Btn_Start_break
            // 
            this.Btn_Start_break.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Btn_Start_break.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.Btn_Start_break.Location = new System.Drawing.Point(4, 55);
            this.Btn_Start_break.Name = "Btn_Start_break";
            this.Btn_Start_break.Size = new System.Drawing.Size(105, 44);
            this.Btn_Start_break.TabIndex = 31;
            this.Btn_Start_break.Text = "吃饭休息开始";
            this.Btn_Start_break.UseVisualStyleBackColor = true;
            this.Btn_Start_break.Click += new System.EventHandler(this.Btn_Start_break_Click);
            // 
            // Btn_UpLoad_break
            // 
            this.Btn_UpLoad_break.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Btn_UpLoad_break.Enabled = false;
            this.Btn_UpLoad_break.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.Btn_UpLoad_break.Location = new System.Drawing.Point(116, 55);
            this.Btn_UpLoad_break.Name = "Btn_UpLoad_break";
            this.Btn_UpLoad_break.Size = new System.Drawing.Size(105, 44);
            this.Btn_UpLoad_break.TabIndex = 30;
            this.Btn_UpLoad_break.Text = "吃饭休息结束";
            this.Btn_UpLoad_break.UseVisualStyleBackColor = true;
            this.Btn_UpLoad_break.Click += new System.EventHandler(this.Btn_UpLoad_break_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label5.Location = new System.Drawing.Point(4, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 50);
            this.label5.TabIndex = 12;
            this.label5.Text = "停机信息：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CB_errorinfo
            // 
            this.CB_errorinfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_errorinfo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.CB_errorinfo.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CB_errorinfo.FormattingEnabled = true;
            this.CB_errorinfo.Items.AddRange(new object[] {
            "无",
            "机台做验证做首件(M)",
            "设备保养(M)",
            "治具保养(M)",
            "更換零配件(M)",
            "镭焊机参数调整(M)",
            "其他原因工艺参数调整(M)",
            "其他原因设备调试(M)",
            "点位调试(M)",
            "机械手点位调试(M)",
            "CCD视觉调试(M)",
            "更换小料(M)",
            "吃饭休息(M)"});
            this.CB_errorinfo.Location = new System.Drawing.Point(113, 8);
            this.CB_errorinfo.Margin = new System.Windows.Forms.Padding(0);
            this.CB_errorinfo.Name = "CB_errorinfo";
            this.CB_errorinfo.Size = new System.Drawing.Size(111, 36);
            this.CB_errorinfo.TabIndex = 13;
            this.CB_errorinfo.Text = "无";
            this.CB_errorinfo.SelectedIndexChanged += new System.EventHandler(this.CB_errorinfo_SelectedIndexChanged);
            // 
            // LB_ErrorCode
            // 
            this.LB_ErrorCode.AutoSize = true;
            this.LB_ErrorCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LB_ErrorCode.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.LB_ErrorCode.Location = new System.Drawing.Point(228, 1);
            this.LB_ErrorCode.Name = "LB_ErrorCode";
            this.LB_ErrorCode.Size = new System.Drawing.Size(105, 50);
            this.LB_ErrorCode.TabIndex = 17;
            this.LB_ErrorCode.Text = "[无,0]";
            this.LB_ErrorCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnManualOEEStatus
            // 
            this.btnManualOEEStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnManualOEEStatus.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnManualOEEStatus.Location = new System.Drawing.Point(340, 4);
            this.btnManualOEEStatus.Name = "btnManualOEEStatus";
            this.btnManualOEEStatus.Size = new System.Drawing.Size(105, 44);
            this.btnManualOEEStatus.TabIndex = 20;
            this.btnManualOEEStatus.Text = "停机选择确认";
            this.btnManualOEEStatus.UseVisualStyleBackColor = true;
            this.btnManualOEEStatus.Click += new System.EventHandler(this.btnManualOEEStatus_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label7.Location = new System.Drawing.Point(228, 103);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 50);
            this.label7.TabIndex = 24;
            this.label7.Text = "OEE选择提示";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LB_ManualSelect
            // 
            this.LB_ManualSelect.AutoSize = true;
            this.LB_ManualSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LB_ManualSelect.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.LB_ManualSelect.Location = new System.Drawing.Point(340, 103);
            this.LB_ManualSelect.Name = "LB_ManualSelect";
            this.LB_ManualSelect.Size = new System.Drawing.Size(105, 50);
            this.LB_ManualSelect.TabIndex = 25;
            this.LB_ManualSelect.Text = "未选择故障原因";
            this.LB_ManualSelect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ManualFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 500);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ManualFrm";
            this.Text = "ManualFrm";
            this.Load += new System.EventHandler(this.ManualFrm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart_TotalDT)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel11.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.DateTimePicker dtp_Start;
        private System.Windows.Forms.Button btn_Output;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.DateTimePicker dtp_End;
        private System.Windows.Forms.Button btn_Refresh;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_TotalDT;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lb_errorMsg;
        private System.Windows.Forms.ListView lv_OEEData;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox CB_errorinfo;
        private System.Windows.Forms.Label LB_ErrorCode;
        private System.Windows.Forms.Button btnManualOEEStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label LB_ManualSelect;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button Btn_Start_break;
        private System.Windows.Forms.Button Btn_UpLoad_break;
        private System.Windows.Forms.Button Btn_UpLoad_errortime;
        private System.Windows.Forms.Button Btn_Start_errortime;
    }
}