using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace 卓汇数据追溯系统
{
    public partial class DataStatisticsFrm : Form
    {
        private MainFrm _mainparent;
        private delegate void Labelvision(string bl, string Name);
        private delegate void DTP(string bl, DateTimePicker Name);
        delegate void RefreachTable(Chart chart, string[] Point_X, double[] Point_Y, int index);
        List<Control> List_Control = new List<Control>();
        private delegate void ShowDGV(int rows, int cells, string value);
        SQLServer SQL = new SQLServer();
        double[] UPH_Y_OK = new double[24];
        double[] UPH_Y_NG = new double[24];
        double[] ChartDT_Run = new double[24];
        double[] ChartDT_Error = new double[24];
        double[] ChartDT_Pending = new double[24];
        string[] x = new string[] { "08:00-09:00", "09:00-10:00", "10:00-11:00", "11:00-12:00", "12:00-13:00", "13:00-14:00", "14:00-15:00", "15:00-16:00", "16:00-17:00", "17:00-18:00", "18:00-19:00", "19:00-20:00"
                , "20:00-21:00", "21:00-22:00", "22:00-23:00", "23:00-00:00", "00:00-01:00", "01:00-02:00", "02:00-03:00", "03:00-04:00", "04:00-05:00", "05:00-06:00", "06:00-07:00", "07:00-08:00" };
        public DataStatisticsFrm(MainFrm mdiParent)
        {
            InitializeComponent();
            _mainparent = mdiParent;
            dgv_D.Rows.Add(10);
            dgv_N.Rows.Add(10);
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)//自定义绘制Tab标题
        {
            string text = ((TabControl)sender).TabPages[e.Index].Text;
            //标签背景填充颜色
            SolidBrush BackBrush = new SolidBrush(Color.Gray);
            SolidBrush brush = new SolidBrush(Color.Black);
            StringFormat sf = new StringFormat();
            //设置文字对齐方式
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            if (e.Index == this.tabControl1.SelectedIndex)//当前Tab页的样式
            {
                BackBrush = new SolidBrush(Color.DarkSeaGreen);
            }
            //绘制标签头背景颜色
            e.Graphics.FillRectangle(BackBrush, e.Bounds);
            //绘制标签头文字
            //e.Graphics.DrawString(text, SystemInformation.MenuFont, brush, e.Bounds, sf);
            e.Graphics.DrawString(text, new Font("微软雅黑", 15), brush, e.Bounds, sf);

        }

        private void DataStatisticsFrm_Load(object sender, EventArgs e)
        {
            List_Control = GetAllControls(this);//列表中添加所有窗体控件
            //double[] y1 = new double[] { 20, 50, 10, 130, 70, 23, 54, 17, 180, 80, 250, 30, 20, 50, 10, 130, 70, 23, 54, 17, 180, 80, 250, 30 };
            //double[] y2 = new double[] { 24, 70, 20, 160, 220, 213, 24, 176, 40, 60, 270, 39, 10, 40, 101, 10, 7, 123, 154, 171, 80, 67, 20, 50 };
            //double[] y3 = new double[] { 26, 80, 30, 138, 90, 233, 154, 107, 10, 90, 50, 38, 210, 150, 120, 30, 20, 231, 74, 107, 10, 280, 101, 30 };
            //double[] UPH_Y_OK = new double[] { 20, 50, 10, 130, 70, 23, 54, 17, 180, 80, 250, 30, 20, 50, 10, 130, 70, 23, 54, 17, 180, 80, 250, 30 };
            //double[] UPH_Y_NG = new double[] { 7, 5, 1, 13, 7, 2, 4, 17, 18, 8, 2, 3, 2, 5, 1, 13, 7, 3, 4, 7, 18, 8, 25, 3 };
            chart_DT.Titles.Add("Hour DT");
            chart_DT.Titles[0].ForeColor = Color.Blue;
            chart_DT.Titles[0].Font = new Font("微软雅黑", 12f, FontStyle.Regular);
            chart_DT.Titles[0].Alignment = ContentAlignment.TopCenter;
            chart_DT.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart_DT.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart_DT.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            chart_DT.ChartAreas[0].AxisX.Title = "时间段";
            chart_DT.ChartAreas[0].AxisY.Title = "持续时间(min)";

            chart_UPH.Titles.Add("UPH统计");
            chart_UPH.Titles[0].ForeColor = Color.Blue;
            chart_UPH.Titles[0].Font = new Font("微软雅黑", 12f, FontStyle.Regular);
            chart_UPH.Titles[0].Alignment = ContentAlignment.TopCenter;
            chart_UPH.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart_UPH.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart_UPH.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            chart_UPH.ChartAreas[0].AxisX.Title = "时间";
            chart_UPH.ChartAreas[0].AxisY.Title = "UPH";

            Legend legend_run = new Legend("运行时间");
            legend_run.Title = "legendTitle";
            Legend legend_error = new Legend("异常时间");
            legend_error.Title = "legendTitle";
            Legend legend_pending = new Legend("待料时间");
            legend_pending.Title = "legendTitle";
            Legend legend_UPH_OK = new Legend("OK数量");
            legend_UPH_OK.Title = "legendTitle";
            Legend legend_UPH_NG = new Legend("NG数量");
            legend_UPH_NG.Title = "legendTitle";
            chart_DT.Series[0].XValueType = ChartValueType.String;  //设置X轴上的值类型
            chart_DT.Series[0].Label = "#VAL";                //设置显示X Y的值    
            chart_DT.Series[0].LabelForeColor = Color.Black;
            chart_DT.Series[0].ToolTip = "#VALX:#VAL";     //鼠标移动到对应点显示数值
            chart_DT.Series[0].ChartType = SeriesChartType.StackedColumn;    //图类型(堆积柱状)
            chart_DT.Series[0].Color = Color.DarkSeaGreen;
            chart_DT.Series[0].LegendText = legend_run.Name;
            chart_DT.Series[0].IsValueShownAsLabel = true;
            chart_DT.Legends.Add(legend_run);
            chart_DT.Legends[0].Position.Auto = true;
            chart_DT.Series[1].XValueType = ChartValueType.String;  //设置X轴上的值类型
            chart_DT.Series[1].Label = "#VAL";                //设置显示X Y的值    
            chart_DT.Series[1].LabelForeColor = Color.Black;
            chart_DT.Series[1].ToolTip = "#VALX:#VAL";     //鼠标移动到对应点显示数值
            chart_DT.Series[1].ChartType = SeriesChartType.StackedColumn;    //图类型(堆积柱状)
            chart_DT.Series[1].Color = Color.Red;
            chart_DT.Series[1].LegendText = legend_error.Name;
            chart_DT.Series[1].IsValueShownAsLabel = true;
            chart_DT.Legends.Add(legend_error);
            chart_DT.Legends[1].Position.Auto = true;
            chart_DT.Series[2].XValueType = ChartValueType.String;  //设置X轴上的值类型
            chart_DT.Series[2].Label = "#VAL";                //设置显示X Y的值    
            chart_DT.Series[2].LabelForeColor = Color.Black;
            chart_DT.Series[2].ToolTip = "#VALX:#VAL";     //鼠标移动到对应点显示数值
            chart_DT.Series[2].ChartType = SeriesChartType.StackedColumn;    //图类型(堆积柱状)
            chart_DT.Series[2].Color = Color.LightSkyBlue;
            chart_DT.Series[2].LegendText = legend_pending.Name;
            chart_DT.Series[2].IsValueShownAsLabel = true;
            chart_DT.Legends.Add(legend_pending);
            chart_DT.Legends[2].Position.Auto = true;
            chart_UPH.Series[0].XValueType = ChartValueType.String;  //设置X轴上的值类型
            chart_UPH.Series[0].Label = "#VAL";                //设置显示X Y的值    
            chart_UPH.Series[0].LabelForeColor = Color.Black;
            chart_UPH.Series[0].ToolTip = "#VALX:#VAL";     //鼠标移动到对应点显示数值
            chart_UPH.Series[0].ChartType = SeriesChartType.StackedColumn;    //图类型(堆积柱状)
            chart_UPH.Series[0].Color = Color.DarkSeaGreen;
            chart_UPH.Series[0].LegendText = legend_UPH_OK.Name;
            chart_UPH.Series[0].IsValueShownAsLabel = true;
            chart_UPH.Legends.Add(legend_UPH_OK);
            chart_UPH.Legends[0].Position.Auto = true;
            chart_UPH.Series[1].XValueType = ChartValueType.String;  //设置X轴上的值类型
            chart_UPH.Series[1].Label = "#VAL";                //设置显示X Y的值    
            chart_UPH.Series[1].LabelForeColor = Color.Black;
            chart_UPH.Series[1].ToolTip = "#VALX:#VAL";     //鼠标移动到对应点显示数值
            chart_UPH.Series[1].ChartType = SeriesChartType.StackedColumn;    //图类型(堆积柱状)
            chart_UPH.Series[1].Color = Color.Red;
            chart_UPH.Series[1].LegendText = legend_UPH_NG.Name;
            chart_UPH.Series[1].IsValueShownAsLabel = true;
            chart_UPH.Legends.Add(legend_UPH_NG);
            chart_UPH.Legends[1].Position.Auto = true;
            chart_DT.Legends[0].Position = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(80, 0, 10, 10);
            chart_UPH.Legends[0].Position = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(80, 0, 10, 10);
            //绑定数据
            //chart_DT.Series[0].Points.DataBindXY(x, y1);
            //chart_DT.Series[1].Points.DataBindXY(x, y2);
            //chart_DT.Series[2].Points.DataBindXY(x, y3);
            //chart_UPH.Series[0].Points.DataBindXY(x, UPH_Y_OK);
            //chart_UPH.Series[1].Points.DataBindXY(x, UPH_Y_NG);
            //chart_DT.Series[0].Palette = ChartColorPalette.BrightPastel;


            dgv_D.Rows[0].Cells[0].Value = "投入数量";

            dgv_D.Rows[2].DefaultCellStyle.BackColor = Color.BlueViolet;
            dgv_D.Rows[2].Cells[0].Value = "1st Pass Yield";

            dgv_N.Rows[0].Cells[0].Value = "投入数量";

            dgv_N.Rows[2].DefaultCellStyle.BackColor = Color.BlueViolet;
            dgv_N.Rows[2].Cells[0].Value = "1st Pass Yield";

            Global.Statistics_Index.Add(0);//DataGridView第一行索引

            int Yeild_end_index = 0;
            int start_index = 2;
            for (int i = 1; i <= Global.Tossing_Item.Count; i++)
            {
                if (Global.Tossing_Item[i].Type == "Yeild")
                {
                    Yeild_end_index++;
                    Global.Statistics_Index.Add(start_index + Yeild_end_index);
                    dgv_D.Rows[start_index + Yeild_end_index].Cells[0].Value = Global.Tossing_Item[i].Chinese_Value;
                    dgv_N.Rows[start_index + Yeild_end_index].Cells[0].Value = Global.Tossing_Item[i].Chinese_Value;
                }
            }

            dgv_D.Rows[start_index + Yeild_end_index + 2].DefaultCellStyle.BackColor = Color.BlueViolet;
            dgv_D.Rows[start_index + Yeild_end_index + 2].Cells[0].Value = "PF";
            dgv_N.Rows[start_index + Yeild_end_index + 2].DefaultCellStyle.BackColor = Color.BlueViolet;
            dgv_N.Rows[start_index + Yeild_end_index + 2].Cells[0].Value = "PF";

            for (int i = 1; i <= Global.Tossing_Item.Count; i++)
            {
                if (Global.Tossing_Item[i].Type == "PF")
                {
                    Global.Statistics_Index.Add(start_index + Yeild_end_index + 2 + i);
                    dgv_D.Rows[start_index + Yeild_end_index + 2 + i].Cells[0].Value = Global.Tossing_Item[i].Chinese_Value;
                    dgv_N.Rows[start_index + Yeild_end_index + 2 + i].Cells[0].Value = Global.Tossing_Item[i].Chinese_Value;
                }
            }

            _mainparent.dgv_AutoSize(dgv_D);
            _mainparent.dgv_AutoSize(dgv_N);

            tabControl1.TabPages.Remove(tabPage9);
        }

        public List<Control> GetAllControls(Control control)
        {
            var list = new List<Control>();
            foreach (Control con in control.Controls)
            {
                list.Add(con);
                if (con.Controls.Count > 0)
                {
                    list.AddRange(GetAllControls(con));
                }
            }
            return list;
        }

        public void UpDataDGV_D(int j, int i, string value)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowDGV(UpDataDGV_D), new object[] { j, i, value });
                return;
            }
            dgv_D.Rows[j].Cells[i].Value = value;
        }
        public void UpDataDGV_N(int j, int i, string value)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowDGV(UpDataDGV_N), new object[] { j, i, value });
                return;
            }
            dgv_N.Rows[j].Cells[i].Value = value;
        }
        public void AppendText(string msg, string Name)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Labelvision(AppendText), new object[] { msg, Name });
                return;
            }
            foreach (Control ctrl in List_Control)
            {
                if (ctrl.GetType() == typeof(TextBox))
                {
                    if (ctrl.Name == Name)
                    {
                        ((TextBox)ctrl).Text = msg;
                    }
                }
            }
        }

        public void UpDatalabel(string txt, string Name)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Labelvision(UpDatalabel), new object[] { txt, Name });
                return;
            }
            foreach (Control ctrl in List_Control)
            {
                if (ctrl.GetType() == typeof(Label))
                {
                    if (ctrl.Name == Name)
                    {
                        ctrl.Text = txt;
                    }
                }
            }
        }

        public void RefreshDTP(string value, DateTimePicker dtp)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DTP(RefreshDTP), new object[] { value, dtp });
                return;
            }
            dtp.Text = value;
        }

        public void RefreachData(Chart chart, string[] Point_X, double[] Point_Y, int index)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new RefreachTable(RefreachData), new object[] { chart, Point_X, Point_Y, index });
                return;
            }
            switch (index)
            {
                case 0:
                    chart.Series[0].Points.DataBindXY(Point_X, Point_Y);
                    break;
                case 1:
                    chart.Series[1].Points.DataBindXY(Point_X, Point_Y);
                    break;
                case 2:
                    chart.Series[0].Points.DataBindXY(Point_X, Point_Y);
                    break;
                case 3:
                    chart.Series[1].Points.DataBindXY(Point_X, Point_Y);
                    break;
                case 4:
                    chart.Series[2].Points.DataBindXY(Point_X, Point_Y);
                    break;
                case 100:
                    double[] point_y = new double[24];
                    for (int i = 0; i < point_y.Length; i++)
                    {
                        point_y[i] = 0;
                    }
                    chart.Series[0].Points.DataBindXY(x, point_y);
                    chart.Series[1].Points.DataBindXY(x, point_y);
                    break;
                case 101:
                    double[] point_Y = new double[24];
                    for (int i = 0; i < point_Y.Length; i++)
                    {
                        point_Y[i] = 0;
                    }
                    chart.Series[0].Points.DataBindXY(x, point_Y);
                    chart.Series[1].Points.DataBindXY(x, point_Y);
                    chart.Series[2].Points.DataBindXY(x, point_Y);
                    break;
                default:
                    break;
            }
        }

        public void UD_DataTable(object ob)
        {
            while (true)
            {
                try
                {
                    if (DateTime.Now.ToString("yyyy-MM-dd") == (Convert.ToDateTime(dtp_UPH_Table.Value)).ToString("yyyy-MM-dd") && DateTime.Now.ToString("yyyy-MM-dd") == (Convert.ToDateTime(dtp_UPH_Data.Value)).ToString("yyyy-MM-dd"))
                    {
                        UPH_Y_OK[0] = Convert.ToDouble(lb_Product_Total_08_09.Text) - Convert.ToDouble(lb_Product_NG_08_09.Text);
                        UPH_Y_OK[1] = Convert.ToDouble(lb_Product_Total_09_10.Text) - Convert.ToDouble(lb_Product_NG_09_10.Text);
                        UPH_Y_OK[2] = Convert.ToDouble(lb_Product_Total_10_11.Text) - Convert.ToDouble(lb_Product_NG_10_11.Text);
                        UPH_Y_OK[3] = Convert.ToDouble(lb_Product_Total_11_12.Text) - Convert.ToDouble(lb_Product_NG_11_12.Text);
                        UPH_Y_OK[4] = Convert.ToDouble(lb_Product_Total_12_13.Text) - Convert.ToDouble(lb_Product_NG_12_13.Text);
                        UPH_Y_OK[5] = Convert.ToDouble(lb_Product_Total_13_14.Text) - Convert.ToDouble(lb_Product_NG_13_14.Text);
                        UPH_Y_OK[6] = Convert.ToDouble(lb_Product_Total_14_15.Text) - Convert.ToDouble(lb_Product_NG_14_15.Text);
                        UPH_Y_OK[7] = Convert.ToDouble(lb_Product_Total_15_16.Text) - Convert.ToDouble(lb_Product_NG_15_16.Text);
                        UPH_Y_OK[8] = Convert.ToDouble(lb_Product_Total_16_17.Text) - Convert.ToDouble(lb_Product_NG_16_17.Text);
                        UPH_Y_OK[9] = Convert.ToDouble(lb_Product_Total_17_18.Text) - Convert.ToDouble(lb_Product_NG_17_18.Text);
                        UPH_Y_OK[10] = Convert.ToDouble(lb_Product_Total_18_19.Text) - Convert.ToDouble(lb_Product_NG_18_19.Text);
                        UPH_Y_OK[11] = Convert.ToDouble(lb_Product_Total_19_20.Text) - Convert.ToDouble(lb_Product_NG_19_20.Text);
                        UPH_Y_OK[12] = Convert.ToDouble(lb_Product_Total_20_21.Text) - Convert.ToDouble(lb_Product_NG_20_21.Text);
                        UPH_Y_OK[13] = Convert.ToDouble(lb_Product_Total_21_22.Text) - Convert.ToDouble(lb_Product_NG_21_22.Text);
                        UPH_Y_OK[14] = Convert.ToDouble(lb_Product_Total_22_23.Text) - Convert.ToDouble(lb_Product_NG_22_23.Text);
                        UPH_Y_OK[15] = Convert.ToDouble(lb_Product_Total_23_00.Text) - Convert.ToDouble(lb_Product_NG_23_00.Text);
                        UPH_Y_OK[16] = Convert.ToDouble(lb_Product_Total_00_01.Text) - Convert.ToDouble(lb_Product_NG_00_01.Text);
                        UPH_Y_OK[17] = Convert.ToDouble(lb_Product_Total_01_02.Text) - Convert.ToDouble(lb_Product_NG_01_02.Text);
                        UPH_Y_OK[18] = Convert.ToDouble(lb_Product_Total_02_03.Text) - Convert.ToDouble(lb_Product_NG_02_03.Text);
                        UPH_Y_OK[19] = Convert.ToDouble(lb_Product_Total_03_04.Text) - Convert.ToDouble(lb_Product_NG_03_04.Text);
                        UPH_Y_OK[20] = Convert.ToDouble(lb_Product_Total_04_05.Text) - Convert.ToDouble(lb_Product_NG_04_05.Text);
                        UPH_Y_OK[21] = Convert.ToDouble(lb_Product_Total_05_06.Text) - Convert.ToDouble(lb_Product_NG_05_06.Text);
                        UPH_Y_OK[22] = Convert.ToDouble(lb_Product_Total_06_07.Text) - Convert.ToDouble(lb_Product_NG_06_07.Text);
                        UPH_Y_OK[23] = Convert.ToDouble(lb_Product_Total_07_08.Text) - Convert.ToDouble(lb_Product_NG_07_08.Text);
                        UPH_Y_NG[0] = Convert.ToDouble(lb_Product_NG_08_09.Text);
                        UPH_Y_NG[1] = Convert.ToDouble(lb_Product_NG_09_10.Text);
                        UPH_Y_NG[2] = Convert.ToDouble(lb_Product_NG_10_11.Text);
                        UPH_Y_NG[3] = Convert.ToDouble(lb_Product_NG_11_12.Text);
                        UPH_Y_NG[4] = Convert.ToDouble(lb_Product_NG_12_13.Text);
                        UPH_Y_NG[5] = Convert.ToDouble(lb_Product_NG_13_14.Text);
                        UPH_Y_NG[6] = Convert.ToDouble(lb_Product_NG_14_15.Text);
                        UPH_Y_NG[7] = Convert.ToDouble(lb_Product_NG_15_16.Text);
                        UPH_Y_NG[8] = Convert.ToDouble(lb_Product_NG_16_17.Text);
                        UPH_Y_NG[9] = Convert.ToDouble(lb_Product_NG_17_18.Text);
                        UPH_Y_NG[10] = Convert.ToDouble(lb_Product_NG_18_19.Text);
                        UPH_Y_NG[11] = Convert.ToDouble(lb_Product_NG_19_20.Text);
                        UPH_Y_NG[12] = Convert.ToDouble(lb_Product_NG_20_21.Text);
                        UPH_Y_NG[13] = Convert.ToDouble(lb_Product_NG_21_22.Text);
                        UPH_Y_NG[14] = Convert.ToDouble(lb_Product_NG_22_23.Text);
                        UPH_Y_NG[15] = Convert.ToDouble(lb_Product_NG_23_00.Text);
                        UPH_Y_NG[16] = Convert.ToDouble(lb_Product_NG_00_01.Text);
                        UPH_Y_NG[17] = Convert.ToDouble(lb_Product_NG_01_02.Text);
                        UPH_Y_NG[18] = Convert.ToDouble(lb_Product_NG_02_03.Text);
                        UPH_Y_NG[19] = Convert.ToDouble(lb_Product_NG_03_04.Text);
                        UPH_Y_NG[20] = Convert.ToDouble(lb_Product_NG_04_05.Text);
                        UPH_Y_NG[21] = Convert.ToDouble(lb_Product_NG_05_06.Text);
                        UPH_Y_NG[22] = Convert.ToDouble(lb_Product_NG_06_07.Text);
                        UPH_Y_NG[23] = Convert.ToDouble(lb_Product_NG_07_08.Text);
                        RefreachData(chart_UPH, x, UPH_Y_OK, 0);
                        RefreachData(chart_UPH, x, UPH_Y_NG, 1);
                    }
                    if (DateTime.Now.ToString("yyyy-MM-dd") == (Convert.ToDateTime(dtp_DT_Table.Value)).ToString("yyyy-MM-dd") && DateTime.Now.ToString("yyyy-MM-dd") == (Convert.ToDateTime(dtp_DT_Data.Value)).ToString("yyyy-MM-dd"))
                    {
                        ChartDT_Run[0] = Convert.ToDouble(lb_RunTime_08_09.Text);
                        ChartDT_Run[1] = Convert.ToDouble(lb_RunTime_09_10.Text);
                        ChartDT_Run[2] = Convert.ToDouble(lb_RunTime_10_11.Text);
                        ChartDT_Run[3] = Convert.ToDouble(lb_RunTime_11_12.Text);
                        ChartDT_Run[4] = Convert.ToDouble(lb_RunTime_12_13.Text);
                        ChartDT_Run[5] = Convert.ToDouble(lb_RunTime_13_14.Text);
                        ChartDT_Run[6] = Convert.ToDouble(lb_RunTime_14_15.Text);
                        ChartDT_Run[7] = Convert.ToDouble(lb_RunTime_15_16.Text);
                        ChartDT_Run[8] = Convert.ToDouble(lb_RunTime_16_17.Text);
                        ChartDT_Run[9] = Convert.ToDouble(lb_RunTime_17_18.Text);
                        ChartDT_Run[10] = Convert.ToDouble(lb_RunTime_18_19.Text);
                        ChartDT_Run[11] = Convert.ToDouble(lb_RunTime_19_20.Text);
                        ChartDT_Run[12] = Convert.ToDouble(lb_RunTime_20_21.Text);
                        ChartDT_Run[13] = Convert.ToDouble(lb_RunTime_21_22.Text);
                        ChartDT_Run[14] = Convert.ToDouble(lb_RunTime_22_23.Text);
                        ChartDT_Run[15] = Convert.ToDouble(lb_RunTime_23_00.Text);
                        ChartDT_Run[16] = Convert.ToDouble(lb_RunTime_00_01.Text);
                        ChartDT_Run[17] = Convert.ToDouble(lb_RunTime_01_02.Text);
                        ChartDT_Run[18] = Convert.ToDouble(lb_RunTime_02_03.Text);
                        ChartDT_Run[19] = Convert.ToDouble(lb_RunTime_03_04.Text);
                        ChartDT_Run[20] = Convert.ToDouble(lb_RunTime_04_05.Text);
                        ChartDT_Run[21] = Convert.ToDouble(lb_RunTime_05_06.Text);
                        ChartDT_Run[22] = Convert.ToDouble(lb_RunTime_06_07.Text);
                        ChartDT_Run[23] = Convert.ToDouble(lb_RunTime_07_08.Text);
                        ChartDT_Error[0] = Convert.ToDouble(lb_ErrorTime_08_09.Text);
                        ChartDT_Error[1] = Convert.ToDouble(lb_ErrorTime_09_10.Text);
                        ChartDT_Error[2] = Convert.ToDouble(lb_ErrorTime_10_11.Text);
                        ChartDT_Error[3] = Convert.ToDouble(lb_ErrorTime_11_12.Text);
                        ChartDT_Error[4] = Convert.ToDouble(lb_ErrorTime_12_13.Text);
                        ChartDT_Error[5] = Convert.ToDouble(lb_ErrorTime_13_14.Text);
                        ChartDT_Error[6] = Convert.ToDouble(lb_ErrorTime_14_15.Text);
                        ChartDT_Error[7] = Convert.ToDouble(lb_ErrorTime_15_16.Text);
                        ChartDT_Error[8] = Convert.ToDouble(lb_ErrorTime_16_17.Text);
                        ChartDT_Error[9] = Convert.ToDouble(lb_ErrorTime_17_18.Text);
                        ChartDT_Error[10] = Convert.ToDouble(lb_ErrorTime_18_19.Text);
                        ChartDT_Error[11] = Convert.ToDouble(lb_ErrorTime_19_20.Text);
                        ChartDT_Error[12] = Convert.ToDouble(lb_ErrorTime_20_21.Text);
                        ChartDT_Error[13] = Convert.ToDouble(lb_ErrorTime_21_22.Text);
                        ChartDT_Error[14] = Convert.ToDouble(lb_ErrorTime_22_23.Text);
                        ChartDT_Error[15] = Convert.ToDouble(lb_ErrorTime_23_00.Text);
                        ChartDT_Error[16] = Convert.ToDouble(lb_ErrorTime_00_01.Text);
                        ChartDT_Error[17] = Convert.ToDouble(lb_ErrorTime_01_02.Text);
                        ChartDT_Error[18] = Convert.ToDouble(lb_ErrorTime_02_03.Text);
                        ChartDT_Error[19] = Convert.ToDouble(lb_ErrorTime_03_04.Text);
                        ChartDT_Error[20] = Convert.ToDouble(lb_ErrorTime_04_05.Text);
                        ChartDT_Error[21] = Convert.ToDouble(lb_ErrorTime_05_06.Text);
                        ChartDT_Error[22] = Convert.ToDouble(lb_ErrorTime_06_07.Text);
                        ChartDT_Error[23] = Convert.ToDouble(lb_ErrorTime_07_08.Text);
                        ChartDT_Pending[0] = Convert.ToDouble(lb_PendingTime_08_09.Text);
                        ChartDT_Pending[1] = Convert.ToDouble(lb_PendingTime_09_10.Text);
                        ChartDT_Pending[2] = Convert.ToDouble(lb_PendingTime_10_11.Text);
                        ChartDT_Pending[3] = Convert.ToDouble(lb_PendingTime_11_12.Text);
                        ChartDT_Pending[4] = Convert.ToDouble(lb_PendingTime_12_13.Text);
                        ChartDT_Pending[5] = Convert.ToDouble(lb_PendingTime_13_14.Text);
                        ChartDT_Pending[6] = Convert.ToDouble(lb_PendingTime_14_15.Text);
                        ChartDT_Pending[7] = Convert.ToDouble(lb_PendingTime_15_16.Text);
                        ChartDT_Pending[8] = Convert.ToDouble(lb_PendingTime_16_17.Text);
                        ChartDT_Pending[9] = Convert.ToDouble(lb_PendingTime_17_18.Text);
                        ChartDT_Pending[10] = Convert.ToDouble(lb_PendingTime_18_19.Text);
                        ChartDT_Pending[11] = Convert.ToDouble(lb_PendingTime_19_20.Text);
                        ChartDT_Pending[12] = Convert.ToDouble(lb_PendingTime_20_21.Text);
                        ChartDT_Pending[13] = Convert.ToDouble(lb_PendingTime_21_22.Text);
                        ChartDT_Pending[14] = Convert.ToDouble(lb_PendingTime_22_23.Text);
                        ChartDT_Pending[15] = Convert.ToDouble(lb_PendingTime_23_00.Text);
                        ChartDT_Pending[16] = Convert.ToDouble(lb_PendingTime_00_01.Text);
                        ChartDT_Pending[17] = Convert.ToDouble(lb_PendingTime_01_02.Text);
                        ChartDT_Pending[18] = Convert.ToDouble(lb_PendingTime_02_03.Text);
                        ChartDT_Pending[19] = Convert.ToDouble(lb_PendingTime_03_04.Text);
                        ChartDT_Pending[20] = Convert.ToDouble(lb_PendingTime_04_05.Text);
                        ChartDT_Pending[21] = Convert.ToDouble(lb_PendingTime_05_06.Text);
                        ChartDT_Pending[22] = Convert.ToDouble(lb_PendingTime_06_07.Text);
                        ChartDT_Pending[23] = Convert.ToDouble(lb_PendingTime_07_08.Text);
                        RefreachData(chart_DT, x, ChartDT_Run, 2);
                        RefreachData(chart_DT, x, ChartDT_Error, 3);
                        RefreachData(chart_DT, x, ChartDT_Pending, 4);
                    }

                    //if (DateTime.Now.Hour == 7 && DateTime.Now.Minute == 49 && DateTime.Now.Second == 20 && InsertSQLFlag)
                    //{

                    //}
                    //if (DateTime.Now.Hour == 7 && DateTime.Now.Minute == 49 && DateTime.Now.Second == 21 && !InsertSQLFlag)
                    //{
                    //    InsertSQLFlag = true;
                    //}
                    if (DateTime.Now.Hour == 0 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 1)//定时更新日期
                    {
                        RefreshDTP(DateTime.Now.ToString("yyyy-MM-dd"), dtp_UPH_Table);
                        RefreshDTP(DateTime.Now.ToString("yyyy-MM-dd"), dtp_UPH_Data);
                        RefreshDTP(DateTime.Now.ToString("yyyy-MM-dd"), dtp_DT_Table);
                        RefreshDTP(DateTime.Now.ToString("yyyy-MM-dd"), dtp_DT_Data);
                        RefreachData(chart_UPH, x, UPH_Y_OK, 100);
                        RefreachData(chart_DT, x, ChartDT_Run, 101);
                        Log.WriteLog("刷新日期时间" + DateTime.Now.ToString("yyyy-MM-dd"));
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLog("更新数据异常" + ex.ToString().Replace("\r\n", ""));
                }
                Thread.Sleep(500);
            }
        }

        private void btn_SelectProductTable_Click(object sender, EventArgs e)
        {
            string SelectStr = string.Format("select * from Product where DateTime = '{0}' and Status='{1}'", (Convert.ToDateTime(dtp_UPH_Table.Value)).ToString("yyyy-MM-dd"), "OK产量");
            DataTable d1 = SQL.ExecuteQuery(SelectStr);
            string SelectStr2 = string.Format("select * from Product where DateTime = '{0}' and Status='{1}'", (Convert.ToDateTime(dtp_UPH_Table.Value)).ToString("yyyy-MM-dd"), "NG产量");
            DataTable d2 = SQL.ExecuteQuery(SelectStr2);
            double[] point_OK = new double[24];
            double[] point_NG = new double[24];
            if (d1.Rows.Count > 0 && d2.Rows.Count > 0)
            {
                for (int i = 0; i < 24; i++)
                {
                    point_OK[i] = Convert.ToDouble(d1.Rows[0][i + 3].ToString());
                    point_NG[i] = Convert.ToDouble(d2.Rows[0][i + 3].ToString());
                }
                chart_UPH.Series[0].Points.DataBindXY(x, point_OK);
                chart_UPH.Series[1].Points.DataBindXY(x, point_NG);
            }
            else
            {
                for (int i = 0; i < 24; i++)
                {
                    point_OK[i] = 0;
                    point_NG[i] = 0;
                }
                chart_UPH.Series[0].Points.DataBindXY(x, point_OK);
                chart_UPH.Series[1].Points.DataBindXY(x, point_NG);
            }
        }

        private void btn_SelectProductData_Click(object sender, EventArgs e)
        {
            string SelectStr = string.Format("select * from Product where DateTime = '{0}' and Status='{1}'", (Convert.ToDateTime(dtp_UPH_Data.Value)).ToString("yyyy-MM-dd"), "OK产量");
            DataTable d1 = SQL.ExecuteQuery(SelectStr);
            string SelectStr2 = string.Format("select * from Product where DateTime = '{0}' and Status='{1}'", (Convert.ToDateTime(dtp_UPH_Data.Value)).ToString("yyyy-MM-dd"), "NG产量");
            DataTable d2 = SQL.ExecuteQuery(SelectStr2);
            string SelectStr3 = string.Format("select * from ErrorDataStatistics where DateTime = '{0}' ", (Convert.ToDateTime(dtp_UPH_Data.Value)).ToString("yyyy-MM-dd"));
            DataTable d3 = SQL.ExecuteQuery(SelectStr3);
            string SelectStr4 = string.Format("select * from HourDT where DateTime = '{0}' and Status='{1}'", (Convert.ToDateTime(dtp_UPH_Data.Value)).ToString("yyyy-MM-dd"), "运行时间");
            DataTable d4 = SQL.ExecuteQuery(SelectStr4);
            if (d1.Rows.Count > 0 && d2.Rows.Count > 0 && d4.Rows.Count > 0)
            {
                lb_Product_Total_08_09.Text = d1.Rows[0][3].ToString();
                lb_Product_Total_09_10.Text = d1.Rows[0][4].ToString();
                lb_Product_Total_10_11.Text = d1.Rows[0][5].ToString();
                lb_Product_Total_11_12.Text = d1.Rows[0][6].ToString();
                lb_Product_Total_12_13.Text = d1.Rows[0][7].ToString();
                lb_Product_Total_13_14.Text = d1.Rows[0][8].ToString();
                lb_Product_Total_14_15.Text = d1.Rows[0][9].ToString();
                lb_Product_Total_15_16.Text = d1.Rows[0][10].ToString();
                lb_Product_Total_16_17.Text = d1.Rows[0][11].ToString();
                lb_Product_Total_17_18.Text = d1.Rows[0][12].ToString();
                lb_Product_Total_18_19.Text = d1.Rows[0][13].ToString();
                lb_Product_Total_19_20.Text = d1.Rows[0][14].ToString();
                lb_Product_Total_20_21.Text = d1.Rows[0][15].ToString();
                lb_Product_Total_21_22.Text = d1.Rows[0][16].ToString();
                lb_Product_Total_22_23.Text = d1.Rows[0][17].ToString();
                lb_Product_Total_23_00.Text = d1.Rows[0][18].ToString();
                lb_Product_Total_00_01.Text = d1.Rows[0][19].ToString();
                lb_Product_Total_01_02.Text = d1.Rows[0][20].ToString();
                lb_Product_Total_02_03.Text = d1.Rows[0][21].ToString();
                lb_Product_Total_03_04.Text = d1.Rows[0][22].ToString();
                lb_Product_Total_04_05.Text = d1.Rows[0][23].ToString();
                lb_Product_Total_05_06.Text = d1.Rows[0][24].ToString();
                lb_Product_Total_06_07.Text = d1.Rows[0][25].ToString();
                lb_Product_Total_07_08.Text = d1.Rows[0][26].ToString();
                lb_Product_NG_08_09.Text = d2.Rows[0][3].ToString();
                lb_Product_NG_09_10.Text = d2.Rows[0][4].ToString();
                lb_Product_NG_10_11.Text = d2.Rows[0][5].ToString();
                lb_Product_NG_11_12.Text = d2.Rows[0][6].ToString();
                lb_Product_NG_12_13.Text = d2.Rows[0][7].ToString();
                lb_Product_NG_13_14.Text = d2.Rows[0][8].ToString();
                lb_Product_NG_14_15.Text = d2.Rows[0][9].ToString();
                lb_Product_NG_15_16.Text = d2.Rows[0][10].ToString();
                lb_Product_NG_16_17.Text = d2.Rows[0][11].ToString();
                lb_Product_NG_17_18.Text = d2.Rows[0][12].ToString();
                lb_Product_NG_18_19.Text = d2.Rows[0][13].ToString();
                lb_Product_NG_19_20.Text = d2.Rows[0][14].ToString();
                lb_Product_NG_20_21.Text = d2.Rows[0][15].ToString();
                lb_Product_NG_21_22.Text = d2.Rows[0][16].ToString();
                lb_Product_NG_22_23.Text = d2.Rows[0][17].ToString();
                lb_Product_NG_23_00.Text = d2.Rows[0][18].ToString();
                lb_Product_NG_00_01.Text = d2.Rows[0][19].ToString();
                lb_Product_NG_01_02.Text = d2.Rows[0][20].ToString();
                lb_Product_NG_02_03.Text = d2.Rows[0][21].ToString();
                lb_Product_NG_03_04.Text = d2.Rows[0][22].ToString();
                lb_Product_NG_04_05.Text = d2.Rows[0][23].ToString();
                lb_Product_NG_05_06.Text = d2.Rows[0][24].ToString();
                lb_Product_NG_06_07.Text = d2.Rows[0][25].ToString();
                lb_Product_NG_07_08.Text = d2.Rows[0][26].ToString();
                if (lb_Product_Total_08_09.Text == "0")
                {
                    lb_Product_Lianglv_08_09.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_08_09.Text = ((Convert.ToDouble(d1.Rows[0][3].ToString()) / (Convert.ToDouble(d1.Rows[0][3].ToString()) + Convert.ToDouble(d2.Rows[0][3].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_09_10.Text == "0")
                {
                    lb_Product_Lianglv_09_10.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_09_10.Text = ((Convert.ToDouble(d1.Rows[0][4].ToString()) / (Convert.ToDouble(d1.Rows[0][4].ToString()) + Convert.ToDouble(d2.Rows[0][4].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_10_11.Text == "0")
                {
                    lb_Product_Lianglv_10_11.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_10_11.Text = ((Convert.ToDouble(d1.Rows[0][5].ToString()) / (Convert.ToDouble(d1.Rows[0][5].ToString()) + Convert.ToDouble(d2.Rows[0][5].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_11_12.Text == "0")
                {
                    lb_Product_Lianglv_11_12.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_11_12.Text = ((Convert.ToDouble(d1.Rows[0][6].ToString()) / (Convert.ToDouble(d1.Rows[0][6].ToString()) + Convert.ToDouble(d2.Rows[0][6].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_12_13.Text == "0")
                {
                    lb_Product_Lianglv_12_13.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_12_13.Text = ((Convert.ToDouble(d1.Rows[0][7].ToString()) / (Convert.ToDouble(d1.Rows[0][7].ToString()) + Convert.ToDouble(d2.Rows[0][7].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_13_14.Text == "0")
                {
                    lb_Product_Lianglv_13_14.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_13_14.Text = ((Convert.ToDouble(d1.Rows[0][8].ToString()) / (Convert.ToDouble(d1.Rows[0][8].ToString()) + Convert.ToDouble(d2.Rows[0][8].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_14_15.Text == "0")
                {
                    lb_Product_Lianglv_14_15.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_14_15.Text = ((Convert.ToDouble(d1.Rows[0][9].ToString()) / (Convert.ToDouble(d1.Rows[0][9].ToString()) + Convert.ToDouble(d2.Rows[0][9].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_15_16.Text == "0")
                {
                    lb_Product_Lianglv_15_16.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_15_16.Text = ((Convert.ToDouble(d1.Rows[0][10].ToString()) / (Convert.ToDouble(d1.Rows[0][10].ToString()) + Convert.ToDouble(d2.Rows[0][10].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_16_17.Text == "0")
                {
                    lb_Product_Lianglv_16_17.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_16_17.Text = ((Convert.ToDouble(d1.Rows[0][11].ToString()) / (Convert.ToDouble(d1.Rows[0][11].ToString()) + Convert.ToDouble(d2.Rows[0][11].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_17_18.Text == "0")
                {
                    lb_Product_Lianglv_17_18.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_17_18.Text = ((Convert.ToDouble(d1.Rows[0][12].ToString()) / (Convert.ToDouble(d1.Rows[0][12].ToString()) + Convert.ToDouble(d2.Rows[0][12].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_18_19.Text == "0")
                {
                    lb_Product_Lianglv_18_19.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_18_19.Text = ((Convert.ToDouble(d1.Rows[0][13].ToString()) / (Convert.ToDouble(d1.Rows[0][13].ToString()) + Convert.ToDouble(d2.Rows[0][13].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_19_20.Text == "0")
                {
                    lb_Product_Lianglv_19_20.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_19_20.Text = ((Convert.ToDouble(d1.Rows[0][14].ToString()) / (Convert.ToDouble(d1.Rows[0][14].ToString()) + Convert.ToDouble(d2.Rows[0][14].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_20_21.Text == "0")
                {
                    lb_Product_Lianglv_20_21.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_20_21.Text = ((Convert.ToDouble(d1.Rows[0][15].ToString()) / (Convert.ToDouble(d1.Rows[0][15].ToString()) + Convert.ToDouble(d2.Rows[0][15].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_21_22.Text == "0")
                {
                    lb_Product_Lianglv_21_22.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_21_22.Text = ((Convert.ToDouble(d1.Rows[0][16].ToString()) / (Convert.ToDouble(d1.Rows[0][16].ToString()) + Convert.ToDouble(d2.Rows[0][16].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_22_23.Text == "0")
                {
                    lb_Product_Lianglv_22_23.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_22_23.Text = ((Convert.ToDouble(d1.Rows[0][17].ToString()) / (Convert.ToDouble(d1.Rows[0][17].ToString()) + Convert.ToDouble(d2.Rows[0][17].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_23_00.Text == "0")
                {
                    lb_Product_Lianglv_23_00.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_23_00.Text = ((Convert.ToDouble(d1.Rows[0][18].ToString()) / (Convert.ToDouble(d1.Rows[0][18].ToString()) + Convert.ToDouble(d2.Rows[0][18].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_00_01.Text == "0")
                {
                    lb_Product_Lianglv_00_01.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_00_01.Text = ((Convert.ToDouble(d1.Rows[0][19].ToString()) / (Convert.ToDouble(d1.Rows[0][19].ToString()) + Convert.ToDouble(d2.Rows[0][19].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_01_02.Text == "0")
                {
                    lb_Product_Lianglv_01_02.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_01_02.Text = ((Convert.ToDouble(d1.Rows[0][20].ToString()) / (Convert.ToDouble(d1.Rows[0][20].ToString()) + Convert.ToDouble(d2.Rows[0][20].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_02_03.Text == "0")
                {
                    lb_Product_Lianglv_02_03.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_02_03.Text = ((Convert.ToDouble(d1.Rows[0][21].ToString()) / (Convert.ToDouble(d1.Rows[0][21].ToString()) + Convert.ToDouble(d2.Rows[0][21].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_03_04.Text == "0")
                {
                    lb_Product_Lianglv_03_04.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_03_04.Text = ((Convert.ToDouble(d1.Rows[0][22].ToString()) / (Convert.ToDouble(d1.Rows[0][22].ToString()) + Convert.ToDouble(d2.Rows[0][22].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_04_05.Text == "0")
                {
                    lb_Product_Lianglv_04_05.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_04_05.Text = ((Convert.ToDouble(d1.Rows[0][23].ToString()) / (Convert.ToDouble(d1.Rows[0][23].ToString()) + Convert.ToDouble(d2.Rows[0][23].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_05_06.Text == "0")
                {
                    lb_Product_Lianglv_05_06.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_05_06.Text = ((Convert.ToDouble(d1.Rows[0][24].ToString()) / (Convert.ToDouble(d1.Rows[0][24].ToString()) + Convert.ToDouble(d2.Rows[0][24].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_06_07.Text == "0")
                {
                    lb_Product_Lianglv_06_07.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_06_07.Text = ((Convert.ToDouble(d1.Rows[0][25].ToString()) / (Convert.ToDouble(d1.Rows[0][25].ToString()) + Convert.ToDouble(d2.Rows[0][25].ToString()))) * 100).ToString("0.00") + "%";
                }
                if (lb_Product_Total_07_08.Text == "0")
                {
                    lb_Product_Lianglv_07_08.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_07_08.Text = ((Convert.ToDouble(d1.Rows[0][26].ToString()) / (Convert.ToDouble(d1.Rows[0][26].ToString()) + Convert.ToDouble(d2.Rows[0][26].ToString()))) * 100).ToString("0.00") + "%";
                }
                double RunTime_day = 0;
                double RunTime_night = 0;
                double total_day = 0;
                double ok_day = 0;
                double ng_day = 0;
                double total_night = 0;
                double ok_night = 0;
                double ng_night = 0;
                for (int i = 0; i < 12; i++)
                {
                    RunTime_day += Convert.ToDouble(d4.Rows[0][i + 3].ToString());
                    RunTime_night += Convert.ToDouble(d4.Rows[0][i + 15].ToString());
                    total_day += Convert.ToDouble(d1.Rows[0][i + 3].ToString()) + Convert.ToDouble(d2.Rows[0][i + 3].ToString());
                    total_night += Convert.ToDouble(d1.Rows[0][i + 15].ToString()) + Convert.ToDouble(d2.Rows[0][i + 15].ToString());
                    ok_day += Convert.ToDouble(d1.Rows[0][i + 3].ToString());
                    ok_night += Convert.ToDouble(d1.Rows[0][i + 15].ToString());
                    ng_day += Convert.ToDouble(d2.Rows[0][i + 3].ToString());
                    ng_night += Convert.ToDouble(d2.Rows[0][i + 15].ToString());
                }
                lb_Product_Total_08_20.Text = ok_day.ToString();
                lb_Product_NG_08_20.Text = ng_day.ToString();
                lb_Product_propery.Text = (total_day / (RunTime_day * 60 / Convert.ToDouble(Global.CT)) * 100).ToString("0.00") + "%";
                lb_Product_propery_N1.Text = (total_night / (RunTime_night * 60 / Convert.ToDouble(Global.CT)) * 100).ToString("0.00") + "%";
                if (total_day == 0)
                {
                    lb_Product_Lianglv_08_20.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_08_20.Text = ((ok_day / total_day) * 100).ToString("0.00") + "%";
                }
                lb_Product_Total_20_08.Text = ok_night.ToString();
                lb_Product_NG_20_08.Text = ng_night.ToString();
                if (total_night == 0)
                {
                    lb_Product_Lianglv_20_08.Text = "0.00%";
                }
                else
                {
                    lb_Product_Lianglv_20_08.Text = ((ok_night / total_night) * 100).ToString("0.00") + "%";
                }
            }
            else
            {
                lb_Product_Total_08_09.Text = "0";
                lb_Product_Total_09_10.Text = "0";
                lb_Product_Total_10_11.Text = "0";
                lb_Product_Total_11_12.Text = "0";
                lb_Product_Total_12_13.Text = "0";
                lb_Product_Total_13_14.Text = "0";
                lb_Product_Total_14_15.Text = "0";
                lb_Product_Total_15_16.Text = "0";
                lb_Product_Total_16_17.Text = "0";
                lb_Product_Total_17_18.Text = "0";
                lb_Product_Total_18_19.Text = "0";
                lb_Product_Total_19_20.Text = "0";
                lb_Product_Total_20_21.Text = "0";
                lb_Product_Total_21_22.Text = "0";
                lb_Product_Total_22_23.Text = "0";
                lb_Product_Total_23_00.Text = "0";
                lb_Product_Total_00_01.Text = "0";
                lb_Product_Total_01_02.Text = "0";
                lb_Product_Total_02_03.Text = "0";
                lb_Product_Total_03_04.Text = "0";
                lb_Product_Total_04_05.Text = "0";
                lb_Product_Total_05_06.Text = "0";
                lb_Product_Total_06_07.Text = "0";
                lb_Product_Total_07_08.Text = "0";
                lb_Product_NG_08_09.Text = "0";
                lb_Product_NG_09_10.Text = "0";
                lb_Product_NG_10_11.Text = "0";
                lb_Product_NG_11_12.Text = "0";
                lb_Product_NG_12_13.Text = "0";
                lb_Product_NG_13_14.Text = "0";
                lb_Product_NG_14_15.Text = "0";
                lb_Product_NG_15_16.Text = "0";
                lb_Product_NG_16_17.Text = "0";
                lb_Product_NG_17_18.Text = "0";
                lb_Product_NG_18_19.Text = "0";
                lb_Product_NG_19_20.Text = "0";
                lb_Product_NG_20_21.Text = "0";
                lb_Product_NG_21_22.Text = "0";
                lb_Product_NG_22_23.Text = "0";
                lb_Product_NG_23_00.Text = "0";
                lb_Product_NG_00_01.Text = "0";
                lb_Product_NG_01_02.Text = "0";
                lb_Product_NG_02_03.Text = "0";
                lb_Product_NG_03_04.Text = "0";
                lb_Product_NG_04_05.Text = "0";
                lb_Product_NG_05_06.Text = "0";
                lb_Product_NG_06_07.Text = "0";
                lb_Product_NG_07_08.Text = "0";
                lb_Product_Lianglv_08_09.Text = "0.00%";
                lb_Product_Lianglv_09_10.Text = "0.00%";
                lb_Product_Lianglv_10_11.Text = "0.00%";
                lb_Product_Lianglv_11_12.Text = "0.00%";
                lb_Product_Lianglv_12_13.Text = "0.00%";
                lb_Product_Lianglv_13_14.Text = "0.00%";
                lb_Product_Lianglv_14_15.Text = "0.00%";
                lb_Product_Lianglv_15_16.Text = "0.00%";
                lb_Product_Lianglv_16_17.Text = "0.00%";
                lb_Product_Lianglv_17_18.Text = "0.00%";
                lb_Product_Lianglv_18_19.Text = "0.00%";
                lb_Product_Lianglv_19_20.Text = "0.00%";
                lb_Product_Lianglv_20_21.Text = "0.00%";
                lb_Product_Lianglv_21_22.Text = "0.00%";
                lb_Product_Lianglv_22_23.Text = "0.00%";
                lb_Product_Lianglv_23_00.Text = "0.00%";
                lb_Product_Lianglv_00_01.Text = "0.00%";
                lb_Product_Lianglv_01_02.Text = "0.00%";
                lb_Product_Lianglv_02_03.Text = "0.00%";
                lb_Product_Lianglv_03_04.Text = "0.00%";
                lb_Product_Lianglv_04_05.Text = "0.00%";
                lb_Product_Lianglv_05_06.Text = "0.00%";
                lb_Product_Lianglv_06_07.Text = "0.00%";
                lb_Product_Lianglv_07_08.Text = "0.00%";
                lb_Product_Total_08_20.Text = "0";
                lb_Product_NG_08_20.Text = "0";
                lb_Product_Lianglv_08_20.Text = "0.00%";
                lb_Product_Total_20_08.Text = "0";
                lb_Product_NG_20_08.Text = "0";
                lb_Product_Lianglv_20_08.Text = "0.00%";
                lb_Product_propery.Text = "0.00%";
                lb_Product_propery_N1.Text = "0.00%";
            }

            if (d3.Rows.Count > 0)
            {
                UpDataDGV_D(0, 1, d3.Rows[0][2].ToString());
                UpDataDGV_N(0, 1, d3.Rows[0][3].ToString());

                for (int i = 1; i < Global.Statistics_Index.Count; i++)
                {
                    UpDataDGV_D(Global.Statistics_Index[i], 1, d3.Rows[0][2 * (i + 1)].ToString());
                    if (d3.Rows[0][2].ToString() != "0")
                    {
                        double 比率 = Convert.ToDouble(d3.Rows[0][2 * (i + 1)].ToString()) / Convert.ToDouble(d3.Rows[0][2].ToString()) * 100;
                        UpDataDGV_D(Global.Statistics_Index[i], 2, 比率.ToString("0.00") + "%");
                    }
                    else
                    {
                        UpDataDGV_D(Global.Statistics_Index[i], 2, ("0.00") + "%");
                    }
                    UpDataDGV_N(Global.Statistics_Index[i], 1, d3.Rows[0][2 * (i + 1) + 1].ToString());
                    if (d3.Rows[0][2].ToString() != "0")
                    {
                        double 比率 = Convert.ToDouble(d3.Rows[0][2 * (i + 1) + 1].ToString()) / Convert.ToDouble(d3.Rows[0][2].ToString()) * 100;
                        UpDataDGV_N(Global.Statistics_Index[i], 2, 比率.ToString("0.00") + "%");
                    }
                    else
                    {
                        UpDataDGV_N(Global.Statistics_Index[i], 2, ("0.00") + "%");
                    }
                }
            }
            else
            {
                UpDataDGV_D(0, 1, "0");
                UpDataDGV_N(0, 1, "0");
                for (int i = 1; i < Global.Statistics_Index.Count; i++)
                {
                    UpDataDGV_D(Global.Statistics_Index[i], 1, "0");
                    UpDataDGV_N(Global.Statistics_Index[i], 1, "0");
                    UpDataDGV_D(Global.Statistics_Index[i], 2, ("0.00") + "%");
                    UpDataDGV_N(Global.Statistics_Index[i], 2, ("0.00") + "%");
                }
            }

        }

        private void btn_SelectDTTable_Click(object sender, EventArgs e)
        {
            string SelectStr = string.Format("select * from HourDT where DateTime = '{0}' and Status='{1}'", (Convert.ToDateTime(dtp_DT_Table.Value)).ToString("yyyy-MM-dd"), "运行时间");
            DataTable d1 = SQL.ExecuteQuery(SelectStr);
            string SelectStr2 = string.Format("select * from HourDT where DateTime = '{0}' and Status='{1}'", (Convert.ToDateTime(dtp_DT_Table.Value)).ToString("yyyy-MM-dd"), "异常时间");
            DataTable d2 = SQL.ExecuteQuery(SelectStr2);
            string SelectStr3 = string.Format("select * from HourDT where DateTime = '{0}' and Status='{1}'", (Convert.ToDateTime(dtp_DT_Table.Value)).ToString("yyyy-MM-dd"), "待料时间");
            DataTable d3 = SQL.ExecuteQuery(SelectStr3);
            double[] point_RUN = new double[24];
            double[] point_ERROR = new double[24];
            double[] point_PENDING = new double[24];
            if (d1.Rows.Count > 0)
            {
                for (int i = 0; i < d1.Rows.Count; i++)
                {
                    for (int j = 0; j < d1.Columns.Count; j++)
                    {
                        if (d1.Rows[i][j].ToString() == "" || d1.Rows[i][j].ToString() == null)
                        {
                            d1.Rows[i][j] = "0";
                        }
                    }
                }
            }
            if (d2.Rows.Count > 0)
            {
                for (int i = 0; i < d2.Rows.Count; i++)
                {
                    for (int j = 0; j < d2.Columns.Count; j++)
                    {
                        if (d2.Rows[i][j].ToString() == "" || d2.Rows[i][j].ToString() == null)
                        {
                            d2.Rows[i][j] = "0";
                        }
                    }
                }
            }
            if (d3.Rows.Count > 0)
            {
                for (int i = 0; i < d3.Rows.Count; i++)
                {
                    for (int j = 0; j < d3.Columns.Count; j++)
                    {
                        if (d3.Rows[i][j].ToString() == "")
                        {
                            d3.Rows[i][j] = "0";
                        }
                    }
                }
            }
            if (d1.Rows.Count > 0 && d2.Rows.Count > 0 && d3.Rows.Count > 0)
            {
                for (int i = 0; i < 24; i++)
                {
                    point_RUN[i] = Convert.ToDouble(d1.Rows[0][i + 3].ToString());
                    point_ERROR[i] = Convert.ToDouble(d2.Rows[0][i + 3].ToString());
                    point_PENDING[i] = Convert.ToDouble(d3.Rows[0][i + 3].ToString());
                }
                chart_DT.Series[0].Points.DataBindXY(x, point_RUN);
                chart_DT.Series[1].Points.DataBindXY(x, point_ERROR);
                chart_DT.Series[2].Points.DataBindXY(x, point_PENDING);
            }
            else
            {
                for (int i = 0; i < 24; i++)
                {
                    point_RUN[i] = 0;
                    point_ERROR[i] = 0;
                    point_PENDING[i] = 0;
                }
                chart_DT.Series[0].Points.DataBindXY(x, point_RUN);
                chart_DT.Series[1].Points.DataBindXY(x, point_ERROR);
                chart_DT.Series[2].Points.DataBindXY(x, point_PENDING);
            }
        }

        private void btn_SelectDTData_Click(object sender, EventArgs e)
        {
            string SelectStr = string.Format("select * from HourDT where DateTime = '{0}' and Status='{1}'", (Convert.ToDateTime(dtp_DT_Data.Value)).ToString("yyyy-MM-dd"), "运行时间");
            DataTable d1 = SQL.ExecuteQuery(SelectStr);
            string SelectStr2 = string.Format("select * from HourDT where DateTime = '{0}' and Status='{1}'", (Convert.ToDateTime(dtp_DT_Data.Value)).ToString("yyyy-MM-dd"), "异常时间");
            DataTable d2 = SQL.ExecuteQuery(SelectStr2);
            string SelectStr3 = string.Format("select * from HourDT where DateTime = '{0}' and Status='{1}'", (Convert.ToDateTime(dtp_DT_Data.Value)).ToString("yyyy-MM-dd"), "待料时间");
            DataTable d3 = SQL.ExecuteQuery(SelectStr3);
            if (d1.Rows.Count > 0 && d2.Rows.Count > 0 && d3.Rows.Count > 0)
            {
                lb_RunTime_08_09.Text = d1.Rows[0][3].ToString();
                lb_RunTime_09_10.Text = d1.Rows[0][4].ToString();
                lb_RunTime_10_11.Text = d1.Rows[0][5].ToString();
                lb_RunTime_11_12.Text = d1.Rows[0][6].ToString();
                lb_RunTime_12_13.Text = d1.Rows[0][7].ToString();
                lb_RunTime_13_14.Text = d1.Rows[0][8].ToString();
                lb_RunTime_14_15.Text = d1.Rows[0][9].ToString();
                lb_RunTime_15_16.Text = d1.Rows[0][10].ToString();
                lb_RunTime_16_17.Text = d1.Rows[0][11].ToString();
                lb_RunTime_17_18.Text = d1.Rows[0][12].ToString();
                lb_RunTime_18_19.Text = d1.Rows[0][13].ToString();
                lb_RunTime_19_20.Text = d1.Rows[0][14].ToString();
                lb_RunTime_20_21.Text = d1.Rows[0][15].ToString();
                lb_RunTime_21_22.Text = d1.Rows[0][16].ToString();
                lb_RunTime_22_23.Text = d1.Rows[0][17].ToString();
                lb_RunTime_23_00.Text = d1.Rows[0][18].ToString();
                lb_RunTime_00_01.Text = d1.Rows[0][19].ToString();
                lb_RunTime_01_02.Text = d1.Rows[0][20].ToString();
                lb_RunTime_02_03.Text = d1.Rows[0][21].ToString();
                lb_RunTime_03_04.Text = d1.Rows[0][22].ToString();
                lb_RunTime_04_05.Text = d1.Rows[0][23].ToString();
                lb_RunTime_05_06.Text = d1.Rows[0][24].ToString();
                lb_RunTime_06_07.Text = d1.Rows[0][25].ToString();
                lb_RunTime_07_08.Text = d1.Rows[0][26].ToString();
                lb_ErrorTime_08_09.Text = d2.Rows[0][3].ToString();
                lb_ErrorTime_09_10.Text = d2.Rows[0][4].ToString();
                lb_ErrorTime_10_11.Text = d2.Rows[0][5].ToString();
                lb_ErrorTime_11_12.Text = d2.Rows[0][6].ToString();
                lb_ErrorTime_12_13.Text = d2.Rows[0][7].ToString();
                lb_ErrorTime_13_14.Text = d2.Rows[0][8].ToString();
                lb_ErrorTime_14_15.Text = d2.Rows[0][9].ToString();
                lb_ErrorTime_15_16.Text = d2.Rows[0][10].ToString();
                lb_ErrorTime_16_17.Text = d2.Rows[0][11].ToString();
                lb_ErrorTime_17_18.Text = d2.Rows[0][12].ToString();
                lb_ErrorTime_18_19.Text = d2.Rows[0][13].ToString();
                lb_ErrorTime_19_20.Text = d2.Rows[0][14].ToString();
                lb_ErrorTime_20_21.Text = d2.Rows[0][15].ToString();
                lb_ErrorTime_21_22.Text = d2.Rows[0][16].ToString();
                lb_ErrorTime_22_23.Text = d2.Rows[0][17].ToString();
                lb_ErrorTime_23_00.Text = d2.Rows[0][18].ToString();
                lb_ErrorTime_00_01.Text = d2.Rows[0][19].ToString();
                lb_ErrorTime_01_02.Text = d2.Rows[0][20].ToString();
                lb_ErrorTime_02_03.Text = d2.Rows[0][21].ToString();
                lb_ErrorTime_03_04.Text = d2.Rows[0][22].ToString();
                lb_ErrorTime_04_05.Text = d2.Rows[0][23].ToString();
                lb_ErrorTime_05_06.Text = d2.Rows[0][24].ToString();
                lb_ErrorTime_06_07.Text = d2.Rows[0][25].ToString();
                lb_ErrorTime_07_08.Text = d2.Rows[0][26].ToString();
                lb_PendingTime_08_09.Text = d3.Rows[0][3].ToString();
                lb_PendingTime_09_10.Text = d3.Rows[0][4].ToString();
                lb_PendingTime_10_11.Text = d3.Rows[0][5].ToString();
                lb_PendingTime_11_12.Text = d3.Rows[0][6].ToString();
                lb_PendingTime_12_13.Text = d3.Rows[0][7].ToString();
                lb_PendingTime_13_14.Text = d3.Rows[0][8].ToString();
                lb_PendingTime_14_15.Text = d3.Rows[0][9].ToString();
                lb_PendingTime_15_16.Text = d3.Rows[0][10].ToString();
                lb_PendingTime_16_17.Text = d3.Rows[0][11].ToString();
                lb_PendingTime_17_18.Text = d3.Rows[0][12].ToString();
                lb_PendingTime_18_19.Text = d3.Rows[0][13].ToString();
                lb_PendingTime_19_20.Text = d3.Rows[0][14].ToString();
                lb_PendingTime_20_21.Text = d3.Rows[0][15].ToString();
                lb_PendingTime_21_22.Text = d3.Rows[0][16].ToString();
                lb_PendingTime_22_23.Text = d3.Rows[0][17].ToString();
                lb_PendingTime_23_00.Text = d3.Rows[0][18].ToString();
                lb_PendingTime_00_01.Text = d3.Rows[0][19].ToString();
                lb_PendingTime_01_02.Text = d3.Rows[0][20].ToString();
                lb_PendingTime_02_03.Text = d3.Rows[0][21].ToString();
                lb_PendingTime_03_04.Text = d3.Rows[0][22].ToString();
                lb_PendingTime_04_05.Text = d3.Rows[0][23].ToString();
                lb_PendingTime_05_06.Text = d3.Rows[0][24].ToString();
                lb_PendingTime_06_07.Text = d3.Rows[0][25].ToString();
                lb_PendingTime_07_08.Text = d3.Rows[0][26].ToString();
                double RunTime_day = 0;
                double ErrorTime_day = 0;
                double PendingTime_day = 0;
                double RunTime_night = 0;
                double ErrorTime_night = 0;
                double PendingTime_night = 0;
                for (int i = 0; i < 12; i++)
                {
                    RunTime_day += Convert.ToDouble(d1.Rows[0][i + 3].ToString());
                    ErrorTime_day += Convert.ToDouble(d2.Rows[0][i + 3].ToString());
                    PendingTime_day += Convert.ToDouble(d3.Rows[0][i + 3].ToString());
                    RunTime_night += Convert.ToDouble(d1.Rows[0][i + 15].ToString());
                    ErrorTime_night += Convert.ToDouble(d2.Rows[0][i + 15].ToString());
                    PendingTime_night += Convert.ToDouble(d3.Rows[0][i + 15].ToString());
                }
                lb_Product_time.Text = (RunTime_day / (RunTime_day + ErrorTime_day + PendingTime_day) * 100).ToString("0.00") + "%";//时间稼动
                lb_Product_time_N1.Text = (RunTime_night / (RunTime_night + ErrorTime_night + PendingTime_night) * 100).ToString("0.00") + "%";//时间稼动
                lb_RunTime_08_20.Text = RunTime_day.ToString("0.00");
                lb_ErrorTime_08_20.Text = ErrorTime_day.ToString("0.00");
                lb_PendingTime_08_20.Text = PendingTime_day.ToString("0.00");
                lb_RunTime_20_08.Text = RunTime_night.ToString("0.00");
                lb_ErrorTime_20_08.Text = ErrorTime_night.ToString("0.00");
                lb_PendingTime_20_08.Text = PendingTime_night.ToString("0.00");
            }
            else
            {
                lb_RunTime_08_09.Text = "0";
                lb_RunTime_09_10.Text = "0";
                lb_RunTime_10_11.Text = "0";
                lb_RunTime_11_12.Text = "0";
                lb_RunTime_12_13.Text = "0";
                lb_RunTime_13_14.Text = "0";
                lb_RunTime_14_15.Text = "0";
                lb_RunTime_15_16.Text = "0";
                lb_RunTime_16_17.Text = "0";
                lb_RunTime_17_18.Text = "0";
                lb_RunTime_18_19.Text = "0";
                lb_RunTime_19_20.Text = "0";
                lb_RunTime_20_21.Text = "0";
                lb_RunTime_21_22.Text = "0";
                lb_RunTime_22_23.Text = "0";
                lb_RunTime_23_00.Text = "0";
                lb_RunTime_00_01.Text = "0";
                lb_RunTime_01_02.Text = "0";
                lb_RunTime_02_03.Text = "0";
                lb_RunTime_03_04.Text = "0";
                lb_RunTime_04_05.Text = "0";
                lb_RunTime_05_06.Text = "0";
                lb_RunTime_06_07.Text = "0";
                lb_RunTime_07_08.Text = "0";
                lb_ErrorTime_08_09.Text = "0";
                lb_ErrorTime_09_10.Text = "0";
                lb_ErrorTime_10_11.Text = "0";
                lb_ErrorTime_11_12.Text = "0";
                lb_ErrorTime_12_13.Text = "0";
                lb_ErrorTime_13_14.Text = "0";
                lb_ErrorTime_14_15.Text = "0";
                lb_ErrorTime_15_16.Text = "0";
                lb_ErrorTime_16_17.Text = "0";
                lb_ErrorTime_17_18.Text = "0";
                lb_ErrorTime_18_19.Text = "0";
                lb_ErrorTime_19_20.Text = "0";
                lb_ErrorTime_20_21.Text = "0";
                lb_ErrorTime_21_22.Text = "0";
                lb_ErrorTime_22_23.Text = "0";
                lb_ErrorTime_23_00.Text = "0";
                lb_ErrorTime_00_01.Text = "0";
                lb_ErrorTime_01_02.Text = "0";
                lb_ErrorTime_02_03.Text = "0";
                lb_ErrorTime_03_04.Text = "0";
                lb_ErrorTime_04_05.Text = "0";
                lb_ErrorTime_05_06.Text = "0";
                lb_ErrorTime_06_07.Text = "0";
                lb_ErrorTime_07_08.Text = "0";
                lb_PendingTime_08_09.Text = "0";
                lb_PendingTime_09_10.Text = "0";
                lb_PendingTime_10_11.Text = "0";
                lb_PendingTime_11_12.Text = "0";
                lb_PendingTime_12_13.Text = "0";
                lb_PendingTime_13_14.Text = "0";
                lb_PendingTime_14_15.Text = "0";
                lb_PendingTime_15_16.Text = "0";
                lb_PendingTime_16_17.Text = "0";
                lb_PendingTime_17_18.Text = "0";
                lb_PendingTime_18_19.Text = "0";
                lb_PendingTime_19_20.Text = "0";
                lb_PendingTime_20_21.Text = "0";
                lb_PendingTime_21_22.Text = "0";
                lb_PendingTime_22_23.Text = "0";
                lb_PendingTime_23_00.Text = "0";
                lb_PendingTime_00_01.Text = "0";
                lb_PendingTime_01_02.Text = "0";
                lb_PendingTime_02_03.Text = "0";
                lb_PendingTime_03_04.Text = "0";
                lb_PendingTime_04_05.Text = "0";
                lb_PendingTime_05_06.Text = "0";
                lb_PendingTime_06_07.Text = "0";
                lb_PendingTime_07_08.Text = "0";
                lb_RunTime_08_20.Text = "0";
                lb_ErrorTime_08_20.Text = "0";
                lb_PendingTime_08_20.Text = "0";
                lb_RunTime_20_08.Text = "0";
                lb_ErrorTime_20_08.Text = "0";
                lb_PendingTime_20_08.Text = "0";
                lb_Product_time.Text = "0.00%";
                lb_Product_time_N1.Text = "0.00%";
            }
        }

        private void dtp_UPH_Data_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Global.SelectDateTime = Convert.ToDateTime(dtp_UPH_Data.Value);
            }
            catch
            { }
        }

        private void dtp_DT_Data_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Global.SelectDTTime = Convert.ToDateTime(dtp_DT_Data.Value);
            }
            catch
            { }
        }

        private void dt_SelectEfficiency_ValueChanged(object sender, EventArgs e)
        {
            string SelectStr = string.Format("select * from ErrorDataStatistics where DateTime = '{0}' ", (Convert.ToDateTime(dt_SelectEfficiency.Value)).ToString("yyyy-MM-dd"));
            DataTable d1 = SQL.ExecuteQuery(SelectStr);
            CreateHtml(d1);
            ShowHtml(1);
            ShowHtml(2);
            ShowHtml(3);
        }

        public void CreateHtml(DataTable dataTable)
        {
            string Data = "0";
            string data1 = "0";//OEE稼动率
            string data2 = "0";//产能稼动率
            string data3 = "0";//时间稼动率

            string Name = "NULL";
            string Name1 = "OEE";//
            string Name2 = "产能稼动(PF)";//
            string Name3 = "时间稼动(AV)";//

            string fontSize = "20";
            string fontSize1 = "30";
            string fontSize2 = "15";
            string fontSize3 = "15";
            string fileName = AppDomain.CurrentDomain.BaseDirectory;
            int i = 3;

            if (dataTable.Rows != null && dataTable.Rows.Count > 0)
            {
                if (dataTable.Rows[0][2].ToString() != "" && dataTable.Rows[0][3].ToString() != null)
                {
                    int product_num = Convert.ToInt16(dataTable.Rows[0][2]) + Convert.ToInt16(dataTable.Rows[0][3]);//一天产能
                    int product_NG_num = Convert.ToInt16(dataTable.Rows[0][4]) + Convert.ToInt16(dataTable.Rows[0][5]) + Convert.ToInt16(dataTable.Rows[0][6]) + Convert.ToInt16(dataTable.Rows[0][7]);//一次良率NG数量
                    int Scan_Left_NG_num = Convert.ToInt16(dataTable.Rows[0][8]) + Convert.ToInt16(dataTable.Rows[0][9]);//一天扫左件NG
                    int Scan_Right_NG_num = Convert.ToInt16(dataTable.Rows[0][10]) + Convert.ToInt16(dataTable.Rows[0][11]);//一天扫右件NG
                    int ScanLC_NG_num = Convert.ToInt16(dataTable.Rows[0][12]) + Convert.ToInt16(dataTable.Rows[0][13]);//一天扫LCNG

                    int Scan_NG = Scan_Left_NG_num >= Scan_Right_NG_num ? Scan_Left_NG_num : Scan_Right_NG_num;
                    Scan_NG = Scan_NG >= ScanLC_NG_num ? Scan_NG : ScanLC_NG_num;

                    double PF = 1 - (Convert.ToDouble(Scan_NG) / Convert.ToDouble(product_num));//扫码良率
                    data2 = (PF * 100.0).ToString("0.00");
                    double First_Yeild = 1 - (Convert.ToDouble(product_NG_num) / Convert.ToDouble(product_num));//一次良率
                    double OEE = Convert.ToDouble(product_num) / (3600 / Global.CT * 24);
                    data1 = (OEE * 100.0).ToString("0.00");
                    data3 = (OEE / PF / First_Yeild * 100.0).ToString("0.00");
                }
            }
           
            try
            {
                while (i > 0)
                {
                    fileName = AppDomain.CurrentDomain.BaseDirectory;
                    if (i == 3)
                    {
                        fileName = fileName + "OEE.html";
                        Data = data1;
                        Name = Name1;
                        fontSize = fontSize1;
                    }
                    if (i == 2)
                    {
                        fileName = fileName + "PF.html";
                        Data = data2;
                        Name = Name2;
                        fontSize = fontSize2;
                    }
                    if (i == 1)
                    {
                        fileName = fileName + "av.html";
                        Data = data3;
                        Name = Name3;
                        fontSize = fontSize3;
                    }
                    i--;
                    if (File.Exists(fileName))
                        File.Delete(fileName);
                    using (StreamWriter sw = new StreamWriter(fileName, false))
                    {
                        sw.WriteLine("<!DOCTYPE html>");
                        sw.WriteLine("<html lang=\"zh-CN\" style=\"height:100% \">");
                        sw.WriteLine("<head>");
                        sw.WriteLine("  <meta charset=\"utf-8\">");
                        sw.WriteLine("  <meta http-equiv=\"X-UA-Compatible\" content=\"ie=edge\">");
                        sw.WriteLine("</head>");
                        sw.WriteLine("<body style=\"height: 100%; margin: 0\">");
                        sw.WriteLine("  <div id=\"container\" style=\"height: 100% \"></div>");
                        sw.WriteLine("  <script type=\"text/javascript\" src=\"echarts.min.js\"></script>");
                        sw.WriteLine("  <script type=\"text/javascript\">");
                        sw.WriteLine("   var dom = document.getElementById('container');");
                        sw.WriteLine("    var myChart = echarts.init(dom, null, {");
                        sw.WriteLine("      renderer: 'canvas',");
                        sw.WriteLine("      useDirtyRect: false");
                        sw.WriteLine("    });");
                        sw.WriteLine("    var app = {};");
                        sw.WriteLine("   var option;");
                        sw.WriteLine("   option = {");
                        sw.WriteLine("      series: [");
                        sw.WriteLine("        {");
                        sw.WriteLine("          type: 'gauge',");
                        sw.WriteLine("          axisLine: {");
                        sw.WriteLine("           lineStyle: {");
                        sw.WriteLine("              width: 30,");
                        sw.WriteLine("              color: [");
                        sw.WriteLine("                [0.6, '#fd666d'],");
                        sw.WriteLine("                [0.8, '#e3e167'],");
                        sw.WriteLine("                [1, '#149935']");
                        sw.WriteLine("              ]");
                        sw.WriteLine("            }");
                        sw.WriteLine("          },");
                        sw.WriteLine("          pointer: {");
                        sw.WriteLine("            itemStyle: {");
                        sw.WriteLine("              color: 'inherit'");
                        sw.WriteLine("            }");
                        sw.WriteLine("          },");
                        sw.WriteLine("          axisTick: {");
                        sw.WriteLine("            distance: -30,");
                        sw.WriteLine("            length: 10,");
                        sw.WriteLine("            splitNumber: 5,");
                        sw.WriteLine("            lineStyle: {");
                        sw.WriteLine("              color: '#fff',");
                        sw.WriteLine("              width: 2");
                        sw.WriteLine("            }");
                        sw.WriteLine("          },");
                        sw.WriteLine("          splitLine: {");
                        sw.WriteLine("            distance: -30,");
                        sw.WriteLine("            length: 30,");
                        sw.WriteLine("            lineStyle: {");
                        sw.WriteLine("              color: '#fff',");
                        sw.WriteLine("              width: 0");
                        sw.WriteLine("            }");
                        sw.WriteLine("          },");
                        sw.WriteLine("          axisLabel: {");
                        sw.WriteLine("            color: 'inherit',");
                        sw.WriteLine("            distance: 40,");
                        sw.WriteLine("            fontSize: 15");
                        sw.WriteLine("          },");
                        sw.WriteLine("          detail: {");
                        sw.WriteLine("            valueAnimation: true,");
                        sw.WriteLine("            formatter: '{value}%',");
                        sw.WriteLine("            color: 'inherit'");
                        sw.WriteLine("          },");
                        sw.WriteLine("          data: [");
                        sw.WriteLine("            {");
                        sw.WriteLine("              value: " + Data + ",");
                        sw.WriteLine("              name: " + "'" + Name + "'" + ",");
                        sw.WriteLine("              title: {");
                        sw.WriteLine("              fontSize: " + fontSize);
                        sw.WriteLine("              }");
                        sw.WriteLine("            }");
                        sw.WriteLine("          ]");
                        sw.WriteLine("        }");
                        sw.WriteLine("      ]");
                        sw.WriteLine("    };");
                        sw.WriteLine("    if (option && typeof option === 'object') {");
                        sw.WriteLine("      myChart.setOption(option);");
                        sw.WriteLine("    }");
                        sw.WriteLine("    window.addEventListener('resize', myChart.resize);");
                        sw.WriteLine("  </script>");
                        sw.WriteLine("</body>");
                        sw.WriteLine("</html>");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("加载OEE图表异常");
            }
        }
        public void ShowHtml(int i)//1为OEE  2 产能  3 时间
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            if (i == 1)
            {
                path = path + "OEE.html";
                web_OEE.Url = new Uri(path);
            }
            if (i == 2)
            {
                path = path + "PF.html";
                web_PF.Url = new Uri(path);
            }
            if (i == 3)
            {
                path = path + "AV.html";
                web_AV.Url = new Uri(path);
            }
        }

        private void btn_SelectFixtureError_Click(object sender, EventArgs e)
        {

        }

        public void outStatisData( string LogPath ,bool b)
        {
            try
            {
                if (All.Checked)
                {
                    string str = LogPath;
                    string wrt = string.Empty;                   
                    using (StreamWriter sw = new StreamWriter(str, b, Encoding.Default))
                    {
                        if (!b)
                        {
                            wrt = "process,station_id,tossing_item,date,shift,result,qty";
                            sw.WriteLine(wrt);
                        }
                        for (int i = 0; i < dgv_D.Rows.Count; i++)
                        {
                            if (i != 2 && i != 3 && i != 6 && i != 7)
                            {
                                wrt = Global.inidata.productconfig.station + "," + Global.inidata.productconfig.Trace_station_id + "," + dgv_D.Rows[i].Cells[0].Value.ToString() + "," + DateTime.Now.ToString("yyyy-MM-dd") + "," + "A" + "," + "" + "," + dgv_D.Rows[i].Cells[1].Value.ToString();
                                sw.WriteLine(wrt);
                                wrt = Global.inidata.productconfig.station + "," + Global.inidata.productconfig.Trace_station_id + "," + dgv_N.Rows[i].Cells[0].Value.ToString() + "," + DateTime.Now.ToString("yyyy-MM-dd") + "," + "B" + "," + "" + "," + dgv_N.Rows[i].Cells[1].Value.ToString();
                                sw.WriteLine(wrt);
                            }
                        }
                        sw.Close();
                        sw.Dispose();
                    }
                }
                if (Yield.Checked)
                {
                    string str = LogPath;
                    string wrt = string.Empty;
                    using (StreamWriter sw = new StreamWriter(str, b, Encoding.Default))
                    {
                        if (!b)
                        {
                            wrt = "process,station_id,tossing_item,date,shift,result,qty";
                            sw.WriteLine(wrt);
                        }
                        for (int i = 4; i <= 5; i++)
                        {
                            wrt = Global.inidata.productconfig.station + "," + Global.inidata.productconfig.Trace_station_id + "," + dgv_D.Rows[i].Cells[0].Value.ToString() + "," + DateTime.Now.Day.ToString("yyyy-MM-dd") + "," + "A" + "," + "" + "," + dgv_D.Rows[i].Cells[1].Value.ToString();
                            sw.WriteLine(wrt);
                            wrt = Global.inidata.productconfig.station + "," + Global.inidata.productconfig.Trace_station_id + "," + dgv_N.Rows[i].Cells[0].Value.ToString() + "," + DateTime.Now.Day.ToString("yyyy-MM-dd") + "," + "B" + "," + "" + "," + dgv_N.Rows[i].Cells[1].Value.ToString();
                            sw.WriteLine(wrt);
                        }
                        sw.Close();
                        sw.Dispose();
                    }
                }
                if (PF.Checked)
                {
                    string str = LogPath;
                    string wrt = string.Empty;
                    using (StreamWriter sw = new StreamWriter(str, b, Encoding.Default))
                    {
                        if (!b)
                        {
                            wrt = "process,station_id,tossing_item,date,shift,result,qty";
                            sw.WriteLine(wrt);
                        }
                        for (int i = 8; i < dgv_D.Rows.Count; i++)
                        {
                            wrt = Global.inidata.productconfig.station + "," + Global.inidata.productconfig.Trace_station_id + "," + dgv_D.Rows[i].Cells[0].Value.ToString() + "," + DateTime.Now.Day.ToString("yyyy-MM-dd") + "," + "A" + "," + "" + "," + dgv_D.Rows[i].Cells[1].Value.ToString();
                            sw.WriteLine(wrt);
                            wrt = Global.inidata.productconfig.station + "," + Global.inidata.productconfig.Trace_station_id + "," + dgv_N.Rows[i].Cells[0].Value.ToString() + "," + DateTime.Now.Day.ToString("yyyy-MM-dd") + "," + "B" + "," + "" + "," + dgv_N.Rows[i].Cells[1].Value.ToString();
                            sw.WriteLine(wrt);
                        }
                        sw.Close();
                        sw.Dispose();
                    }
                }
            }
            catch(Exception ex)
            {
                Log.WriteLog("导出当天小料抛料失败:" + ex.ToString());
            }            
        }

        private void outError_Click(object sender, EventArgs e)
        {
            if ((DateTime.Compare(Convert.ToDateTime(EndDay.Text), Convert.ToDateTime(StartDay.Text)) < 0) || (Convert.ToDateTime(StartDay.Text) > Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))))
            {
                Log.WriteLog("选择的开始时间比结束时间大，无数据导出");
                MessageBox.Show("选择的开始与结束时间有误,请留意选择的时间", "操作提示");
                return;
            }
            if ((Convert.ToDateTime(EndDay.Text) >= Convert.ToDateTime(StartDay.Text)) && (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) == Convert.ToDateTime(StartDay.Text)) && (Convert.ToDateTime(DateTime.Now.ToString("HH:mm:ss")).CompareTo(Convert.ToDateTime("9:30:00")) >= 0 && Convert.ToDateTime(DateTime.Now.ToString("HH:mm:ss")).CompareTo(Convert.ToDateTime("21:30:00")) < 0))
            {
                SaveFileDialog sf = new SaveFileDialog();
                sf.Title = "文档导出";
                sf.Filter = "文档(*.csv)|*.csv";
                sf.FileName = DateTime.Now.Date.ToString("yyyyMMdd");
                if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    outStatisData(sf.FileName, false);
                }
                return;
            }
            Dictionary<int, string> columnName = new Dictionary<int, string>();
            columnName.Add(2, "投入数量");
            columnName.Add(4, "1#机台上料确认失败/焊接失败");
            columnName.Add(6, "2#机台上料确认失败/焊接失败");
            columnName.Add(8, "I件扫码失败");
            columnName.Add(10, "LC扫码失败");
            string tossing_item = string.Empty;
            string SelectStr = string.Format("select * from ErrorDataStatistics where cast(DateTime as datetime) >= '{0}' and cast(DateTime as datetime) <= '{1}'", Convert.ToDateTime(StartDay.Text), Convert.ToDateTime(EndDay.Text));
            DataTable d1 = SQL.ExecuteQuery(SelectStr);
            if (d1.Rows.Count > 0)
            {
                for (int i = 0; i < d1.Rows.Count; i++)
                {
                    for (int j = 0; j < d1.Columns.Count; j++)
                    {
                        string str = d1.Rows[i][j].ToString();
                        if (d1.Rows[i][j].ToString() == "")
                        {
                            d1.Rows[i][j] = "0";
                        }
                    }
                }
            }
            if (d1.Rows.Count > 0)
            {
                SaveFileDialog sf = new SaveFileDialog();
                sf.Title = "文档导出";
                sf.Filter = "文档(*.csv)|*.csv";
                if (All.Checked)
                {
                    sf.FileName = DateTime.Now.Date.ToString("yyyyMMdd");
                    if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string str = sf.FileName;
                        using (StreamWriter sw = new StreamWriter(str, false, Encoding.Default))
                        {
                            try
                            {
                                string wr = "process,station_id,tossing_item,date,shift,result,qty";
                                sw.WriteLine(wr);
                                for (int t = 0; t < d1.Rows.Count; t++)
                                {
                                    for (int i = 2; i < d1.Columns.Count; i++)
                                    {
                                        if (i % 2 == 0)
                                        {
                                            columnName.TryGetValue(i, out tossing_item);
                                            wr = Global.inidata.productconfig.station + "," + Global.inidata.productconfig.Trace_station_id + "," + tossing_item + "," + d1.Rows[t][1] + "," + "A" + "," + "" + "," + d1.Rows[t][i];
                                            sw.WriteLine(wr);
                                        }
                                        if (i % 2 != 0)
                                        {
                                            columnName.TryGetValue(i - 1, out tossing_item);
                                            wr = Global.inidata.productconfig.station + "," + Global.inidata.productconfig.Trace_station_id + "," + tossing_item + "," + d1.Rows[t][1] + "," + "B" + "," + "" + "," + d1.Rows[t][i];
                                            sw.WriteLine(wr);
                                        }
                                    }
                                }
                                if (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(EndDay.Text))
                                {
                                    outStatisData(sf.FileName, true);
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "导出错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Log.WriteLog("导出小料抛料错误:" + ex.ToString());
                            }
                            sw.Close();
                            sw.Dispose();
                            columnName.Clear();
                        }
                    }
                    return;
                }
                if (Yield.Checked)
                {
                    sf.FileName = DateTime.Now.Date.ToString("yyyyMMdd");
                    if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string str = sf.FileName;
                        using (StreamWriter sw = new StreamWriter(str, false, Encoding.Default))
                        {
                            try
                            {
                                string wr = "process,station_id,tossing_item,date,shift,result,qty";
                                sw.WriteLine(wr);
                                for (int t = 0; t < d1.Rows.Count; t++)
                                {
                                    for (int i = 6; i <= 9; i++)//16之后是PF的数据
                                    {
                                        if (i % 2 == 0)
                                        {
                                            columnName.TryGetValue(i, out tossing_item);
                                            wr = Global.inidata.productconfig.station + "," + Global.inidata.productconfig.Trace_station_id + "," + tossing_item + "," + d1.Rows[t][1] + "," + "A" + "," + "" + "," + d1.Rows[t][i];
                                            sw.WriteLine(wr);
                                        }
                                        if (i % 2 != 0)
                                        {
                                            columnName.TryGetValue(i - 1, out tossing_item);
                                            wr = Global.inidata.productconfig.station + "," + Global.inidata.productconfig.Trace_station_id + "," + tossing_item + "," + d1.Rows[t][1] + "," + "B" + "," + "" + "," + d1.Rows[t][i];
                                            sw.WriteLine(wr);
                                        }
                                    }
                                }
                                if (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(EndDay.Text))
                                {
                                    outStatisData(sf.FileName, true);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "导出错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Log.WriteLog("导出小料抛料错误:" + ex.ToString());
                            }
                            sw.Close();
                            sw.Dispose();
                            columnName.Clear();
                        }
                    }
                    return;
                }
                if (PF.Checked)
                {
                    sf.FileName = DateTime.Now.Date.ToString("yyyyMMdd");
                    if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string str = sf.FileName;
                        using (StreamWriter sw = new StreamWriter(str, false, Encoding.Default))
                        {
                            try
                            {
                                string wr = "process,station_id,tossing_item,date,shift,result,qty";
                                sw.WriteLine(wr);
                                for (int t = 0; t < d1.Rows.Count; t++)
                                {
                                    for (int i = 10; i < d1.Columns.Count; i++)
                                    {
                                        if (i % 2 == 0)
                                        {
                                            columnName.TryGetValue(i, out tossing_item);
                                            wr = Global.inidata.productconfig.station + "," + Global.inidata.productconfig.Trace_station_id + "," + tossing_item + "," + d1.Rows[t][1] + "," + "A" + "," + "" + "," + d1.Rows[t][i];
                                            sw.WriteLine(wr);
                                        }
                                        if (i % 2 != 0)
                                        {
                                            columnName.TryGetValue(i - 1, out tossing_item);
                                            wr = Global.inidata.productconfig.station + "," + Global.inidata.productconfig.Trace_station_id + "," + tossing_item + "," + d1.Rows[t][1] + "," + "B" + "," + "" + "," + d1.Rows[t][i];
                                            sw.WriteLine(wr);
                                        }
                                    }
                                }
                                if (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(EndDay.Text))
                                {
                                    outStatisData(sf.FileName, true);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "导出错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Log.WriteLog("导出小料抛料错误:" + ex.ToString());
                            }
                            sw.Close();
                            sw.Dispose();
                            columnName.Clear();
                        }
                    }
                    return;
                }
            }
            if (columnName != null)
            {
                columnName.Clear();
            }
        }
    }
    
}
