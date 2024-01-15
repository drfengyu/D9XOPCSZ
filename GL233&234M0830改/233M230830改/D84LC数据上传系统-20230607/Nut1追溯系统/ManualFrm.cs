using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace 卓汇数据追溯系统
{
    public partial class ManualFrm : Form
    {
        private MainFrm _mainparent;
        //private HomeFrm _homefrm;
        private delegate void buttonflag(bool flag, string name);
        private delegate void Labelcolor(Color color, string bl, string Name);
        private delegate void Labelvision(string bl, string Name);
        List<Control> List_Control = new List<Control>();
        SQLServer SQL = new SQLServer();
        delegate void RefreachTable(Chart chart, string[] Point_X, double[] Point_Y, int index);
        string[] HourDT_X = new string[] { "待料", "生产", "宕机", "机台/治具保养", "调试", "关机", "吃饭休息", "更换耗材", "首件" };
        double[] HourDT_Y = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public ManualFrm(MainFrm mdiParent)
        {
            InitializeComponent();
            _mainparent = mdiParent;
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
            e.Graphics.DrawString(text, new Font("微软雅黑", 12), brush, e.Bounds, sf);
        }

        private void btn_Output_Click(object sender, EventArgs e)
        {
            try
            {
                SaveDataToCSVFile(lv_OEEData);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }

        private void CB_errorinfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CB_errorinfo.SelectedItem.ToString().Contains("无"))
            {
                LB_ErrorCode.Text = "[无,无]";
            }
            else
            {
                if (CB_errorinfo.SelectedItem.ToString().Contains("机台做验证做首件(M)"))
                {
                    LB_ErrorCode.Text = "[11010001,5]";
                }
                if (CB_errorinfo.SelectedItem.ToString().Contains("设备保养(M)"))
                {
                    LB_ErrorCode.Text = "[30010001,4]";
                }
                if (CB_errorinfo.SelectedItem.ToString().Contains("治具保养(M)"))
                {
                    LB_ErrorCode.Text = "[30010002,4]";
                }
                if (CB_errorinfo.SelectedItem.ToString().Contains("更换零配件(M)"))
                {
                    LB_ErrorCode.Text = "[30010003,8]";
                }
                if (CB_errorinfo.SelectedItem.ToString().Contains("镭焊机参数调整(M)"))
                {
                    LB_ErrorCode.Text = "[70010001,5]";

                }
                if (CB_errorinfo.SelectedItem.ToString().Contains("其他原因工艺参数调整(M)"))
                {
                    LB_ErrorCode.Text = "[70010002,8]";
                }
                if (CB_errorinfo.SelectedItem.ToString().Contains("其他原因设备调试(M)"))
                {
                    LB_ErrorCode.Text = "[70020003,5]";
                }
                if (CB_errorinfo.SelectedItem.ToString().Contains("点位调试(M)"))
                {
                    LB_ErrorCode.Text = "[70020004,5]";
                }
                if (CB_errorinfo.SelectedItem.ToString().Contains("机械手点位调试(M)"))
                {
                    LB_ErrorCode.Text = "[70020005,5]";
                }
                if (CB_errorinfo.SelectedItem.ToString().Contains("CCD视觉调试(M)"))
                {
                    LB_ErrorCode.Text = "[70020006,5]";
                }
                if (CB_errorinfo.SelectedItem.ToString().Contains("更换小料(M)"))
                {
                    LB_ErrorCode.Text = "[80010001,8]";
                }


                Global.errorcode = LB_ErrorCode.Text.Split(',')[0].Replace("[", "");
                Global.errorStatus = LB_ErrorCode.Text.Split(',')[1].Replace("]", "");
                Global.errorinfo = CB_errorinfo.SelectedItem.ToString();
            }
        }

        //private void CB_PendingReason_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (CB_PendingReason.SelectedItem.ToString().Contains("其他原因待料"))
        //    {
        //        CB_PendingReason.Text = "[12010001,1]";
        //    }
        //    if (CB_PendingReason.SelectedItem.ToString().Contains("HSG待料"))
        //    {
        //        CB_PendingCode.Text = "[12010002,1]";
        //    }
        //    if (CB_PendingReason.SelectedItem.ToString().Contains("NUT待料"))
        //    {
        //        CB_PendingCode.Text = "[12010003,1]";
        //    }
        //    if (CB_PendingReason.SelectedItem.ToString().Contains("NUT和HSG待料"))
        //    {
        //        CB_PendingCode.Text = "[12010004,1]";
        //    }
        //    //Global.labelerror= CB_PendingCode.Text.Split(',')[0].Replace("[", "");
        //    //Global.labelStatus = CB_PendingCode.Text.Split(',')[1].Replace("]", "");
        //    //Global.errorselect = CB_PendingReason.SelectedItem.ToString();
        //}

        private void ManualFrm_Load(object sender, EventArgs e)
        {
            CB_errorinfo.SelectedIndex = 0;
            //CB_PendingReason.SelectedIndex = 0;
            List_Control = GetAllControls(this);//列表中添加所有窗体控件


            #region 饼图总DT
            //标题
            chart_TotalDT.Titles.Add("Total DT");
            chart_TotalDT.Titles[0].ForeColor = Color.Green;
            chart_TotalDT.Titles[0].Font = new Font("Calibri", 12f, FontStyle.Bold);
            chart_TotalDT.Titles[0].Alignment = ContentAlignment.TopCenter;
            //chart_TotalDT.Titles.Add("合计：25412 宗");
            //chart_TotalDT.Titles[1].ForeColor = Color.White;
            //chart_TotalDT.Titles[1].Font = new Font("微软雅黑", 8f, FontStyle.Regular);
            //chart_TotalDT.Titles[1].Alignment = ContentAlignment.TopRight;
            //控件背景
            chart_TotalDT.BackColor = Color.Transparent;
            //图表区背景
            chart_TotalDT.ChartAreas[0].BackColor = Color.Transparent;
            chart_TotalDT.ChartAreas[0].BorderColor = Color.Transparent;
            chart_TotalDT.ChartAreas[0].Area3DStyle.Enable3D = true;
            chart_TotalDT.ChartAreas[0].Area3DStyle.Inclination = 0;
            //X轴标签间距
            //chart_TotalDT.ChartAreas[0].AxisX.Interval = 1;
            //chart_TotalDT.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true;
            //chart_TotalDT.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            //chart_TotalDT.ChartAreas[0].AxisX.TitleFont = new Font("Calibri", 14f, FontStyle.Regular);
            //chart_TotalDT.ChartAreas[0].AxisX.TitleForeColor = Color.Black;
            //X坐标轴颜色
            //chart_TotalDT.ChartAreas[0].AxisX.LineColor = ColorTranslator.FromHtml("#38587a"); ;
            //chart_TotalDT.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Black;
            //chart_TotalDT.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Calibri", 10f, FontStyle.Regular);
            //X坐标轴标题
            //chart_TotalDT.ChartAreas[0].AxisX.Title = "数量(宗)";
            //chart_TotalDT.ChartAreas[0].AxisX.TitleFont = new Font("Calibri", 10f, FontStyle.Regular);
            //chart_TotalDT.ChartAreas[0].AxisX.TitleForeColor = Color.Black;
            //chart_TotalDT.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Horizontal;
            //chart_TotalDT.ChartAreas[0].AxisX.ToolTip = "数量(宗)";
            //X轴网络线条
            //chart_TotalDT.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            //chart_TotalDT.ChartAreas[0].AxisX.MajorGrid.LineColor = ColorTranslator.FromHtml("#2c4c6d");
            //Y坐标轴颜色
            //chart_TotalDT.ChartAreas[0].AxisY.LineColor = ColorTranslator.FromHtml("#38587a");
            //chart_TotalDT.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Black;
            //chart_TotalDT.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Calibri", 10f, FontStyle.Regular);
            //Y坐标轴标题
            //chart_TotalDT.ChartAreas[0].AxisY.Title = "数量(宗)";
            //chart_TotalDT.ChartAreas[0].AxisY.TitleFont = new Font("Calibri", 10f, FontStyle.Regular);
            //chart_TotalDT.ChartAreas[0].AxisY.TitleForeColor = Color.Black;
            //chart_TotalDT.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Rotated270;
            //chart_TotalDT.ChartAreas[0].AxisY.ToolTip = "数量(宗)";
            //Y轴网格线条
            //chart_TotalDT.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            //chart_TotalDT.ChartAreas[0].AxisY.MajorGrid.LineColor = ColorTranslator.FromHtml("#2c4c6d");
            //chart_TotalDT.ChartAreas[0].AxisY2.LineColor = Color.Transparent;
            //背景渐变
            chart_TotalDT.ChartAreas[0].BackGradientStyle = GradientStyle.None;
            //图例样式
            Legend legend3 = new Legend("#VALX #PERCENT{P2} #VAL{N2}");
            legend3.Title = "图例";
            legend3.TitleBackColor = Color.Transparent;
            legend3.BackColor = Color.Transparent;
            legend3.TitleForeColor = Color.Black;
            legend3.TitleFont = new Font("Calibri", 10f, FontStyle.Regular);
            legend3.Font = new Font("Calibri", 8f, FontStyle.Regular);
            legend3.ForeColor = Color.Black;
            chart_TotalDT.Series[0].XValueType = ChartValueType.String;  //设置X轴上的值类型
            chart_TotalDT.Series[0].Label = "#VALX";     //设置显示X Y的值    
            chart_TotalDT.Series[0].LabelForeColor = Color.Black;
            chart_TotalDT.Series[0].ToolTip = "#VALX:#VAL";     //鼠标移动到对应点显示数值
            chart_TotalDT.Series[0].ChartType = SeriesChartType.Pie;    //图类型(饼图)
            chart_TotalDT.Series[0].Color = Color.Lime;
            chart_TotalDT.Series[0].LegendText = legend3.Name;
            chart_TotalDT.Series[0].IsValueShownAsLabel = true;
            chart_TotalDT.Series[0].LabelForeColor = Color.Black;
            //chart_TotalDT.Series[0].CustomProperties = "DrawingStyle = Cylinder";
            //chart_TotalDT.Series[0].CustomProperties = "MinimumRelativePieSize = 70";
            //chart_TotalDT.Series[0].CustomProperties = "PieLabelStyle = Outside";
            chart_TotalDT.Legends.Add(legend3);
            chart_TotalDT.Legends[0].Position.Auto = true;
            chart_TotalDT.Series[0].IsValueShownAsLabel = true;
            //是否显示图例
            chart_TotalDT.Series[0].IsVisibleInLegend = true;
            chart_TotalDT.Series[0].ShadowOffset = 0;
            //饼图折线
            chart_TotalDT.Series[0]["PieLineColor"] = "Black";
            //绑定数据
            //chart_TotalDT.Series[0].Points.DataBindXY(x1, y1);
            //绑定颜色
            chart_TotalDT.Series[0].Palette = ChartColorPalette.BrightPastel;

            #endregion
        }


        public void DT_Pie(string StartTime, string EndTime)
        {
            int[] Runstatus = new int[9];
            double[] TimeSpan = new double[9];
            double[] Baifenbi = new double[9];
            try
            {
                string[] HourDT_X = new string[] { "待料", "生产", "宕机", "机台/治具保养", "调试", "关机", "吃饭休息", "更换耗材", "首件" };
                double[] HourDT_Y = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                double TotalTime = 0;
                string SelectStr = "SELECT sum(cast(分钟 as float)) ,运行状态 FROM Select_OEEDT " +
               string.Format("where cast(开始时间 as datetime) between '{0}' and '{1}' ", StartTime, EndTime) +
               "group by 运行状态 order by 运行状态 ";
                DataTable d1 = SQL.ExecuteQuery(SelectStr);
                if (d1.Rows.Count > 0)
                {
                    for (int i = 0; i < d1.Rows.Count; i++)
                    {
                        try
                        {
                            TimeSpan[i] = Convert.ToDouble(d1.Rows[i][0].ToString());
                            Runstatus[i] = Convert.ToInt32(d1.Rows[i][1].ToString());
                            TotalTime += Convert.ToDouble(d1.Rows[i][0].ToString());
                            HourDT_Y[Runstatus[i] - 1] = TimeSpan[i];
                        }
                        catch (Exception)
                        {

                            continue;
                        }
                        
                    }
                    //for (int i = 0; i < d1.Rows.Count; i++)
                    //{
                    //    Baifenbi[i] = (TimeSpan[i] / TotalTime) * 100;
                    //    HourDT_Y[Runstatus[i] - 1] = Baifenbi[i];
                    //}
                }
                RefreachData(chart_TotalDT, HourDT_X, HourDT_Y, 0);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString().Replace("\r\n", ""));
            }
        }



        private void btn_errortime_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (Global.j != 3)
            //    {
            //        Global.ed[Global.j].stop_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            //        DateTime t1 = Convert.ToDateTime(Global.ed[Global.j].start_time);
            //        DateTime t2 = Convert.ToDateTime(Global.ed[Global.j].stop_time);
            //        string ts = (t2 - t1).TotalMinutes.ToString("0.00");
            //        Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[Global.j].errorCode + "," + Global.ed[Global.j].start_time + "," + Global.ed[Global.j].start_time + "," + "自动发送成功" + "," + Global.ed[Global.j].errorStatus + "," + Global.ed[Global.j].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
            //    }
            //    else
            //    {
            //        Global.ed[Global.Error_num + 1].stop_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            //        DateTime t1 = Convert.ToDateTime(Global.ed[Global.Error_num + 1].start_time);
            //        DateTime t2 = Convert.ToDateTime(Global.ed[Global.Error_num + 1].stop_time);
            //        string ts = (t2 - t1).TotalMinutes.ToString("0.00");
            //        Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[Global.Error_num + 1].errorCode + "," + Global.ed[Global.Error_num + 1].start_time + "," + Global.ed[Global.Error_num + 1].start_time + "," + "自动发送成功" + "," + Global.ed[Global.Error_num + 1].errorStatus + "," + Global.ed[Global.Error_num + 1].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
            //    }
            //    string OEE_DT = "";
            //    string msg = "";
            //    string EventTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            //    var IP = _mainparent.GetIp();
            //    var Mac = _mainparent.GetMac();
            //    OEE_DT = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"CreateTime\":\"{3}\",\"Isreckon\":\"{4}\",\"Machineno\":\"{5}\"}}", "1", "21050004", EventTime, EventTime, "Y", "Alaska_Nut_001");
            //    Log.WriteLog("OEE_DT手动:" + OEE_DT);
            //    var rst = RequestAPI.Request(Global.inidata.productconfig.OEE_URL1, Global.inidata.productconfig.OEE_URL2, IP, Mac, Global.inidata.productconfig.OEE_Dsn, Global.inidata.productconfig.OEE_authCode, 2, OEE_DT, out msg);
            //    if (rst)
            //    {
            //        _mainparent.AppendText(listBox1, "       " + "21050004" + "    触发时间=" + EventTime + "  接受时间=" + EventTime + "    运行状态:" + "1" + "    故障描述:" + "机台做验证做首件" + "     手动发送成功", 1);
            //        //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + "21050004" + "," + EventTime + "," + EventTime + "," + "手动发送成功" + "," + "1" + "," + "机台做验证做首件", @"F:\装机软件\系统配置\System_ini\");
            //        Log.WriteLog("OEE_DT手动errorCode发送成功");
            //        Global.ConnectOEEFlag = true;
            //    }
            //    else
            //    {
            //        _mainparent.AppendText(listBox1, "       " + "21050004" + "    触发时间=" + EventTime + "  接受时间=" + EventTime + "    运行状态:" + "1" + "    故障描述:" + "机台做验证做首件" + "     手动发送失败", 1);
            //        //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + "21050004" + "," + EventTime + "," + EventTime + "," + "手动发送失败" + "," + "1" + "," + "机台做验证做首件", @"F:\装机软件\系统配置\System_ini\");
            //        Log.WriteLog("OEE_DT手动errorCode发送失败");
            //        Global.ConnectOEEFlag = false;
            //        //Access.InsertData_OEE_DT("PDCA", "OEE_DownTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "OEE_DT", "1", "21050004", EventTime, EventTime, "Y", "Alaska_Nut_001", "", "机台做验证做首件");
            //    }
            //    Global.PLC_Client.WritePLC_D(23021, new short[] { 1 });//通知PLC开始做首件做验证
            //}
            //catch (Exception ex)
            //{
            //    Log.WriteLog(ex.ToString());
            //}
            //Global.errorTime1 = true;
            //Global.errorStartTime = DateTime.Now;
            //Global.errordata.errorStatus = "1";
            //Global.errordata.errorCode = "21050004";
            //Global.errordata.errorinfo = "机台做验证做首件";
            //_mainparent.UiText(Global.errorStartTime.ToString("yyyy-MM-dd HH:mm:ss"), errortime_display_TB);
            //// errortime_display_TB.Text = errorStartTime.ToString("yyyy-MM-dd HH:mm:ss");
            ////_mainparent.Btn_IfEnable(true, label52);
        }

        public void ButtonFlag(bool Flag, string Name)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new buttonflag(ButtonFlag), new object[] { Flag, Name });
                return;
            }
            foreach (Control ctrl in List_Control)
            {
                if (ctrl.GetType() == typeof(Button))
                {
                    if (ctrl.Name == Name)
                    {
                        ctrl.Enabled = Flag;
                    }
                }
            }
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
                    //chart.Series[1].Points.DataBindXY(Point_X, Point_Y);
                    break;
                case 100:
                    double[] Y = new double[9];
                    for (int i = 0; i < Y.Length; i++)
                    {
                        Y[i] = 0;
                    }
                    chart.Series[0].Points.DataBindXY(Point_X, Y);
                    break;
                default:
                    break;
            }
        }

        public void labelcolor(Color color, string str, string Name)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Labelcolor(labelcolor), new object[] { color, str, Name });
                return;
            }
            foreach (Control ctrl in List_Control)
            {
                if (ctrl.GetType() == typeof(Label))
                {
                    if (ctrl.Name == Name)
                    {
                        ctrl.BackColor = color;
                        ctrl.Text = str;
                    }
                }
            }
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

        private void btnManualOEEStatus_Click(object sender, EventArgs e)
        {
            if (CB_errorinfo.SelectedItem.ToString().Contains("无"))
            {
                MessageBox.Show("故障信息不能选择为‘无’,请选择正确的故障信息！");
            }
            else
            {
                CB_errorinfo.SelectedIndex = 0;
                UpDatalabel("选择成功", "LB_ManualSelect");
                Global.SelectManualErrorCode = true;

                //Global.PLC_Client.WritePLC_D(20010, new short[] { 1 });//手动选择打开安全门原因，机台可以运行
                Global.plc1.Write("MW62006", Convert.ToInt16(1));
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

        public void dataTableToListView(ListView lv, DataTable dt, string startTime, string EndTime)
        {
            if (dt != null)
            {
                lv.View = View.Details;
                lv.GridLines = true;//显示网格线
                lv.Items.Clear();//所有的项
                lv.Columns.Clear();//标题
                //对表格重新排序赋值
                dt.Columns["EventTime"].SetOrdinal(1);
                dt.Columns["DateTime"].SetOrdinal(2);
                dt.Columns["TimeSpan"].SetOrdinal(3);
                dt.Columns["EventTime"].ColumnName = "开始时间";
                dt.Columns["DateTime"].ColumnName = "结束时间";
                dt.Columns["ErrorCode"].ColumnName = "故障代码";
                dt.Columns["ModuleCode"].ColumnName = "模组代码";
                dt.Columns["RunStatus"].ColumnName = "运行状态";
                dt.Columns["ErrorInfo"].ColumnName = "故障信息";
                dt.Columns["TimeSpan"].ColumnName = "分钟";
                dt.Rows[0][1] = startTime;          //把机台实际状态改变的时间替换为查找开始时间
                dt.Rows[0][3] = (Convert.ToDateTime(dt.Rows[0][2].ToString()) - Convert.ToDateTime(dt.Rows[0][1].ToString())).TotalMinutes.ToString("0.00");//计算状态发生的时长
                if (Convert.ToDateTime(EndTime) <= Convert.ToDateTime(dt.Rows[dt.Rows.Count - 1][2].ToString()))//判断结束时间是否大于database中的结束时间
                {
                    dt.Rows[dt.Rows.Count - 1][2] = EndTime;//把机台实际状态结束的时间替换为查找结束时间
                    dt.Rows[dt.Rows.Count - 1][3] = (Convert.ToDateTime(dt.Rows[dt.Rows.Count - 1][2].ToString()) - Convert.ToDateTime(dt.Rows[dt.Rows.Count - 1][1].ToString())).TotalMinutes.ToString("0.00");//计算状态发生的时长
                }
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    lv.Columns.Add(dt.Columns[i].Caption.ToString());//增加标题
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListViewItem lvi = new ListViewItem(dt.Rows[i][0].ToString());
                    for (int j = 1; j < dt.Columns.Count; j++)
                    {
                        // lvi.ImageIndex = 0;
                        lvi.SubItems.Add(dt.Rows[i][j].ToString());
                    }
                    lv.Items.Add(lvi);
                }
                lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//调整列的宽度

                string deleteStr = "delete from Select_OEEDT";
                SQL.ExecuteUpdate(deleteStr);
                foreach (DataRow datarow in dt.Rows)
                {
                    string InsertOEEStr = "insert into Select_OEEDT([开始时间],[结束时间],[故障代码],[模组代码],[运行状态],[故障信息],[分钟])" +
                    "VALUES('" + datarow["开始时间"].ToString() + "'" +
                    ",'" + datarow["结束时间"].ToString() + "'" +
                    ",'" + datarow["故障代码"].ToString() + "'" +
                    ",'" + datarow["模组代码"].ToString() + "'" +
                    ",'" + datarow["运行状态"].ToString() + "'" +
                    ",'" + datarow["故障信息"].ToString() + "'" +
                    ",'" + datarow["分钟"].ToString() + "')";
                    SQL.ExecuteUpdate(InsertOEEStr);
                }
                DT_Pie(dtp_Start.Text, dtp_End.Text);
                Log.WriteLog("插入Select_OEEDT表格OEEDT查询记录");
            }
        }


        private static bool SaveDataToCSVFile(ListView lv)
        {
            if (lv.Items.Count == 0)
            {
                MessageBox.Show("没有数据可导出!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            SaveFileDialog sf = new SaveFileDialog();
            sf.Title = "文档导出";
            sf.Filter = "文档(*.csv)|*.csv";
            sf.FileName = DateTime.Now.Date.ToString("yyyyMMdd");
            if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string str = sf.FileName;
                using (StreamWriter sw = new StreamWriter(str, false, Encoding.Default))
                {
                    try
                    {
                        sw.WriteLine("序号,开始时间,结束时间,分钟,故障代码,模组代码,运行状态,故障信息");
                        for (int t = 0; t < lv.Items.Count; t++)
                        {
                            string oeestr = "";
                            for (int t2 = 0; t2 < lv.Columns.Count; t2++)
                            {
                                oeestr += lv.Items[t].SubItems[t2].Text.ToString() + ",";
                            }
                            sw.WriteLine(oeestr);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "导出错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                    sw.Close();
                    sw.Dispose();
                }
            }
            return true;
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                string SelectStr = string.Format("select * from OEE_DT where cast(DateTime as datetime) >='{0}' and cast(EventTime as datetime) <='{1}'", dtp_Start.Text, dtp_End.Text);
                DataTable d1 = SQL.ExecuteQuery(SelectStr);
                if (d1.Rows.Count > 0)
                {
                    dataTableToListView(lv_OEEData, d1, dtp_Start.Text, dtp_End.Text);
                }
                else
                {
                    lv_OEEData.View = View.Details;
                    lv_OEEData.GridLines = true;//显示网格线
                    lv_OEEData.Items.Clear();//所有的项
                    lv_OEEData.Columns.Clear();//标题
                    DT_Pie(dtp_Start.Text, dtp_End.Text);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }
        }

        public string GetIp()//获取本机IP
        {
            string hostName = Dns.GetHostName();   //获取本机名
            IPHostEntry localhost = Dns.GetHostByName(hostName);    //方法已过期，可以获取IPv4的地址
            //IPHostEntry localhost = Dns.GetHostEntry(hostName);   //获取IPv6地址
            IPAddress localaddr = localhost.AddressList[0];
            return localaddr.ToString();
        }

        public string GetMac()//获取本机MAC地址
        {
            string strMac = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    strMac = mo["MacAddress"].ToString();
                    mo.Dispose();
                    break;
                }
            }
            moc = null;
            mc = null;
            return strMac;
        }

        private void Btn_UpLoad_errortime_Click(object sender, EventArgs e)
        {
            return;
            bool timeset = false;
            Global.SelectFirstModel = false;
            if (Global.errorTime1 == true)
            {
                Global.errordata.stop_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                timeset = true;
            }
            if (Global.errordata.errorCode != null && Global.errordata.start_time != null && timeset == true)
            {
                timeset = false;
                Global.errorTime1 = false;
                //txt_errortime_display.Text = "0000-00-00 00:00:00";
                string c = "c=UPLOAD_DOWNTIME&tsn=Test_station&mn=Machine#1&start_time=" + Global.errordata.start_time + "&stop_time=" + Global.errordata.stop_time + "&ec=" + Global.errordata.errorCode;
                Log.WriteLog(c);
                DateTime t1 = Convert.ToDateTime(Global.errordata.start_time);
                DateTime t2 = Convert.ToDateTime(Global.errordata.stop_time);
                string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                string InsertOEEStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.errordata.errorCode + "'" + "," + "'" + Global.errordata.start_time + "'" + "," + "'" + "" + "'" + "," + "'" + Global.errordata.errorStatus + "'" + "," + "'" + Global.errordata.errorinfo + "'" + "," + "'" + ts + "'" + ")";
                SQL.ExecuteUpdate(InsertOEEStr);

                _mainparent._homefrm.AppendRichText(Global.errordata.start_time + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.errordata.errorCode + "," + Global.errordata.errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.errordata.errorCode + "," + Global.errordata.start_time + "," + "手动发送成功" + "," + Global.errordata.errorStatus + "," + Global.errordata.errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
            }
            else
            {
                Log.WriteLog("1.请选择好错误信息和时间 2.请检查网线与网址！");
                Log.WriteLog("errorCode:" + Global.errordata.errorCode + "," + "start_time:" + Global.errordata.start_time + "," + "timeset:" + timeset);
            }
            Btn_UpLoad_errortime.Enabled = false;
            lb_errorMsg.Visible = false;
            Btn_Start_errortime.Enabled = true;
        }
        private void Btn_Start_errortime_Click(object sender, EventArgs e)
        {
            return;
            try
            {
                //Global.currentCount = 0;//定时计数清零
                //Global.timer.Enabled = true;//定时器启用
                //Global.timer.Start();//定时器开始
                Btn_Start_errortime.Enabled = false;
                //var IP = GetIp();
                //var Mac = GetMac();
                //short[] ReadTestRunStatus = Global.PLC_Client.ReadPLC_D(20006, 1);
                HslCommunication.OperateResult<short> r1 = Global.plc1.ReadInt16("MW62005");
                short ReadTestRunStatus = r1.Content;
                if (ReadTestRunStatus != 1)//判断是否处于空跑状态（PLC屏蔽部分功能如：安全门，扫码枪，机械手）
                {
                    if (!Global.BreakStatus)//不是吃饭休息时
                    {
                        if (Global.j == 1)//处于待料状态
                        {
                            Global.ed[Global.Error_PendingNum + 1].stop_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            DateTime t1 = Convert.ToDateTime(Global.ed[Global.Error_PendingNum + 1].start_time);
                            DateTime t2 = Convert.ToDateTime(Global.ed[Global.Error_PendingNum + 1].stop_time);
                            string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                            string InsertOEEStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorCode + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].start_time + "'" + "," + "'" + "" + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                            SQL.ExecuteUpdate(InsertOEEStr);

                            _mainparent._homefrm.AppendRichText(Global.ed[Global.Error_PendingNum + 1].start_time + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.Error_PendingNum + 1].errorCode + "," + Global.ed[Global.Error_PendingNum + 1].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                            //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[Global.Error_PendingNum + 1].errorCode + "," + Global.ed[Global.Error_PendingNum + 1].start_time + "," + "自动发送成功" + "," + Global.ed[Global.Error_PendingNum + 1].errorStatus + "," + Global.ed[Global.Error_PendingNum + 1].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                        }
                        else if (Global.j == 2)//处于运行状态
                        {
                            Global.ed[Global.j].stop_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            DateTime t1 = Convert.ToDateTime(Global.ed[Global.j].start_time);
                            DateTime t2 = Convert.ToDateTime(Global.ed[Global.j].stop_time);
                            string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                            string InsertOEEStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.j].errorCode + "'" + "," + "'" + Global.ed[Global.j].start_time + "'" + "," + "'" + "" + "'" + "," + "'" + Global.ed[Global.j].errorStatus + "'" + "," + "'" + Global.ed[Global.j].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                            SQL.ExecuteUpdate(InsertOEEStr);

                            _mainparent._homefrm.AppendRichText(Global.ed[Global.j].start_time + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.j].errorCode + "," + Global.ed[Global.j].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                            //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[Global.j].errorCode + "," + Global.ed[Global.j].start_time + "," + "自动发送成功" + "," + Global.ed[Global.j].errorStatus + "," + Global.ed[Global.j].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                        }
                        else if (Global.j == 3)
                        {
                            Global.ed[Global.Error_num + 1].stop_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            DateTime t1 = Convert.ToDateTime(Global.ed[Global.Error_num + 1].start_time);
                            DateTime t2 = Convert.ToDateTime(Global.ed[Global.Error_num + 1].stop_time);
                            string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                            if (Global.Error_num == 55)//机台打开安全门
                            {
                                //string OEE_DT2 = "";
                                //string msg2 = "";
                                //OEE_DT2 = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", Global.errorStatus, Global.errorcode, Global.ed[Global.Error_num + 1].start_time, "");
                                //string InsertStr2 = "insert into OEE_TraceDT([DateTime],[Status],[ErrorCode],[EventTime],[ModuleCode])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Global.errorStatus + "'" + "," + "'" + Global.errorcode + "'" + ","
                                //+ "'" + Global.ed[Global.Error_num + 1].start_time + "'" + "," + "'" + Global.ed[Global.Error_num + 1].ModuleCode + "'" + ")";
                                //SQL.ExecuteUpdate(InsertStr2);
                                //Log.WriteLog("OEE_DT安全门打开:" + OEE_DT2);
                                //var rst2 = RequestAPI.Request(Global.inidata.productconfig.OEE_URL1, Global.inidata.productconfig.OEE_URL2, IP, Mac, Global.inidata.productconfig.OEE_Dsn, Global.inidata.productconfig.OEE_authCode, 2, OEE_DT2, out msg2);
                                //if (rst2)
                                //{
                                //_mainparent._homefrm.AppendRichText(Global.errorcode + ",触发时间=" + Global.ed[Global.Error_num + 1].start_time + ",运行状态:" + Global.errorStatus + ",故障描述:" + Global.errorinfo + ",安全门打开自动发送成功", "rtx_DownTimeMsg");
                                //Log.WriteLog("OEE_DT安全门打开自动errorCode发送成功");
                                //Global.ConnectOEEFlag = true;
                                //}
                                //else
                                //{
                                //_mainparent._homefrm.AppendRichText(Global.errorcode + ",触发时间=" + Global.ed[Global.Error_num + 1].start_time + ",运行状态:" + Global.errorStatus + ",故障描述:" + Global.errorinfo + ",安全门打开自动发送失败", "rtx_DownTimeMsg");
                                //Log.WriteLog("OEE_DT安全门打开自动errorCode发送失败");
                                //Global.ConnectOEEFlag = false;
                                //string s = "insert into OEE_DTSendNG([DateTime],[Product],[Status],[ErrorCode],[EventTime],[ModuleCode],[Moduleinfo],[errorinfo])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + "OEE_DT" + "'" + "," + "'" + Global.errorStatus + "'" + "," + "'" + Global.errorcode + "'" + ","
                                // + "'" + Global.ed[Global.Error_num + 1].start_time + "'" + "," + "'" + "" + "'" + "," + "'" + "" + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorinfo + "'" + ")";
                                //int r = SQL.ExecuteUpdate(s);
                                //Log.WriteLog(string.Format("插入了{0}行OEE_DownTime缓存数据", r));
                                //}
                                labelcolor(Color.Transparent, "未选择", "LB_ManualSelect");
                                string InsertOEEStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.errorcode + "'" + "," + "'" + Global.ed[Global.Error_num + 1].start_time + "'" + "," + "'" + "" + "'" + "," + "'" + Global.errorStatus + "'" + "," + "'" + Global.errorinfo + "'" + "," + "'" + ts + "'" + ")";
                                SQL.ExecuteUpdate(InsertOEEStr);

                                _mainparent._homefrm.AppendRichText(Global.ed[Global.Error_num + 1].start_time + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.errorcode + "," + Global.errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                                //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.errorcode + "," + Global.ed[Global.Error_num + 1].start_time + "," + "" + "," + "自动发送成功" + "," + Global.errorStatus + "," + Global.errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                            }
                            else//机台处于其它异常状态中
                            {
                                string InsertOEEStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorCode + "'" + "," + "'" + Global.ed[Global.Error_num + 1].start_time + "'" + "," + "'" + Global.ed[Global.Error_num + 1].ModuleCode + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                                SQL.ExecuteUpdate(InsertOEEStr);

                                _mainparent._homefrm.AppendRichText(Global.ed[Global.Error_num + 1].start_time + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.Error_num + 1].errorCode + "," + Global.ed[Global.Error_num + 1].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                                //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[Global.Error_num + 1].errorCode + "," + Global.ed[Global.Error_num + 1].start_time + "," + "自动发送成功" + "," + Global.ed[Global.Error_num + 1].errorStatus + "," + Global.ed[Global.Error_num + 1].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                            }
                        }
                        else if (Global.j == 4)//处于人工停止状态
                        {
                            Global.ed[Global.Error_Stopnum + 1].stop_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            DateTime t1 = Convert.ToDateTime(Global.ed[Global.Error_Stopnum + 1].start_time);
                            DateTime t2 = Convert.ToDateTime(Global.ed[Global.Error_Stopnum + 1].stop_time);
                            string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                            //string OEE_DT2 = "";
                            //string msg2 = "";
                            //OEE_DT2 = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", Global.ed[Global.Error_Stopnum + 1].errorStatus, Global.ed[Global.Error_Stopnum + 1].errorCode, Global.ed[Global.Error_Stopnum + 1].start_time, "");
                            //string InsertStr2 = "insert into OEE_TraceDT([DateTime],[Status],[ErrorCode],[EventTime],[ModuleCode])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorCode + "'" + ","
                            //+ "'" + Global.ed[Global.Error_Stopnum + 1].start_time + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].ModuleCode + "'" + ")";
                            //SQL.ExecuteUpdate(InsertStr2);
                            //Log.WriteLog("OEE_DT人工停止复位:" + OEE_DT2);
                            //var rst2 = RequestAPI.Request(Global.inidata.productconfig.OEE_URL1, Global.inidata.productconfig.OEE_URL2, IP, Mac, Global.inidata.productconfig.OEE_Dsn, Global.inidata.productconfig.OEE_authCode, 2, OEE_DT2, out msg2);
                            //if (rst2)
                            //{
                            //_mainparent._homefrm.AppendRichText(Global.ed[Global.Error_Stopnum + 1].errorCode + ",触发时间=" + Global.ed[Global.Error_Stopnum + 1].start_time + ",运行状态:" + Global.ed[Global.Error_Stopnum + 1].errorStatus + ",故障描述:" + Global.ed[Global.Error_Stopnum + 1].errorinfo + ",人工停止复位自动发送成功", "rtx_DownTimeMsg");
                            //Log.WriteLog("OEE_DT人工停止复位自动errorCode发送成功");
                            //Global.ConnectOEEFlag = true;
                            //}
                            //else
                            //{
                            //_mainparent._homefrm.AppendRichText(Global.ed[Global.Error_Stopnum + 1].errorCode + ",触发时间=" + Global.ed[Global.Error_Stopnum + 1].start_time + ",运行状态:" + Global.ed[Global.Error_Stopnum + 1].errorStatus + ",故障描述:" + Global.ed[Global.Error_Stopnum + 1].errorinfo + ",人工停止复位自动发送失败", "rtx_DownTimeMsg");
                            //Log.WriteLog("OEE_DT人工停止复位自动errorCode发送失败");
                            //Global.ConnectOEEFlag = false;
                            //string s = "insert into OEE_DTSendNG([DateTime],[Product],[Status],[ErrorCode],[EventTime],[ModuleCode],[Moduleinfo],[errorinfo])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + "OEE_DT" + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorCode + "'" + ","
                            //+ "'" + Global.ed[Global.Error_Stopnum + 1].start_time + "'" + "," + "'" + "" + "'" + "," + "'" + "" + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorinfo + "'" + ")";
                            //int r = SQL.ExecuteUpdate(s);
                            //Log.WriteLog(string.Format("插入了{0}行OEE_DownTime缓存数据", r));
                            //}
                            _mainparent._manualfrm.labelcolor(Color.Transparent, "未选择", "LB_ManualSelect");
                            string InsertOEEStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorCode + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].start_time + "'" + "," + "'" + "" + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                            SQL.ExecuteUpdate(InsertOEEStr);

                            _mainparent._homefrm.AppendRichText(Global.ed[Global.Error_Stopnum + 1].start_time + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.Error_Stopnum + 1].errorCode + "," + Global.ed[Global.Error_Stopnum + 1].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                            //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[Global.Error_Stopnum + 1].errorCode + "," + Global.ed[Global.Error_Stopnum + 1].start_time + "," + "" + "," + "自动发送成功" + "," + Global.ed[Global.Error_Stopnum + 1].errorStatus + "," + Global.ed[Global.Error_Stopnum + 1].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                        }
                    }
                    else
                    {
                        Btn_UpLoad_break_Click(null, null);
                    }
                }
                else
                {
                    if (Global.SelectTestRunModel == true && Global.ed[57].start_time != null)//空运行结束写入OEE_DT数据表中
                    {
                        Global.ed[57].stop_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//空跑
                        DateTime t1 = Convert.ToDateTime(Global.ed[57].start_time);
                        DateTime t2 = Convert.ToDateTime(Global.ed[57].stop_time);
                        string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                        string InsertStr6 = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[57].errorCode + "'" + "," + "'" + Global.ed[57].start_time + "'" + "," + "'" + Global.ed[57].ModuleCode + "'" + "," + "'" + Global.ed[57].errorStatus + "'" + "," + "'" + Global.ed[57].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                        SQL.ExecuteUpdate(InsertStr6);

                        _mainparent._homefrm.AppendRichText(Global.ed[57].start_time + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[57].errorCode + "," + Global.ed[57].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                        //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[100].errorCode + "," + Global.ed[100].start_time + "," + Global.ed[100].ModuleCode + "," + "自动发送成功" + "," + Global.ed[100].errorStatus + "," + Global.ed[100].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                        Global.ed[57].start_time = null;
                        Global.ed[57].stop_time = null;
                    }
                    Global.SelectTestRunModel = false;
                }
                //string OEE_DT = "";
                //string msg = "";
                //string EventTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                //OEE_DT = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", "9", "20010001", EventTime, "");
                //string InsertStr = "insert into OEE_TraceDT([DateTime],[Status],[ErrorCode],[EventTime],[ModuleCode])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + "9" + "'" + "," + "'" + "20010001" + "'" + ","
                //+ "'" + EventTime + "'" + "," + "'" + "" + "'" + ")";
                //SQL.ExecuteUpdate(InsertStr);
                //Log.WriteLog("OEE_DT手动:" + OEE_DT);
                //var rst = RequestAPI.Request(Global.inidata.productconfig.OEE_URL1, Global.inidata.productconfig.OEE_URL2, IP, Mac, Global.inidata.productconfig.OEE_Dsn, Global.inidata.productconfig.OEE_authCode, 2, OEE_DT, out msg);
                //if (rst)
                //{
                //_mainparent._homefrm.AppendRichText("20010001" + ",触发时间=" + EventTime + ",运行状态:" + "9" + ",故障描述:" + "首件" + ",手动发送成功", "rtx_DownTimeMsg");
                //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + "20010001" + "," + EventTime + "," + "手动发送成功" + "," + "1" + "," + "机台做验证做首件", @"F:\装机软件\系统配置\System_ini\");
                //Log.WriteLog("OEE_DT机台做首件发送成功");
                //Global.ConnectOEEFlag = true;
                //}
                //else
                //{
                //_mainparent._homefrm.AppendRichText("20010001" + ",触发时间=" + EventTime + ",运行状态:" + "9" + ",故障描述:" + "首件" + ",手动发送失败", "rtx_DownTimeMsg");
                //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + "20010001" + "," + EventTime + "," + "手动发送失败" + "," + "1" + "," + "机台做验证做首件", @"F:\装机软件\系统配置\System_ini\");
                //Log.WriteLog("OEE_DT机台做首件发送失败");
                //Global.ConnectOEEFlag = false;
                //string s = "insert into OEE_DTSendNG([DateTime],[Product],[Status],[ErrorCode],[EventTime],[ModuleCode],[Moduleinfo],[errorinfo])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + "OEE_DT" + "'" + "," + "'" + "9" + "'" + "," + "'" + "21010001" + "'" + ","
                //+ "'" + EventTime + "'" + "," + "'" + "" + "'" + "," + "'" + "" + "'" + "," + "'" + "首件" + "'" + ")";
                //int r = SQL.ExecuteUpdate(s);
                //Log.WriteLog(string.Format("插入了{0}行OEE_DownTime缓存数据", r));
                //}
                //Global.PLC_Client.WritePLC_D(23022, new short[] { 1 });//通知PLC开始做首件做验证
                Global.errorTime1 = true;
                Global.errordata.start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Global.errordata.errorStatus = "9";
                Global.errordata.errorCode = "20010001";
                Global.errordata.errorinfo = "首件";
                //txt_errortime_display.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Btn_UpLoad_errortime.Enabled = true;
                lb_errorMsg.Visible = true;
                Global.SelectFirstModel = true;
                string InsertOEEStr3 = "insert into OEE_StartTime([Status],[ErrorCode],[EventTime],[ModuleCode],[Name])" + " " + "values(" + "'" + "9" + "'" + "," + "'" + "20010001" + "'" + "," + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + "" + "'" + "," + "'" + "首件" + "'" + ")";
                SQL.ExecuteUpdate(InsertOEEStr3);//插入首件开始时间

                _mainparent._homefrm.AppendRichText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + "20010001" + "," + "首件", "Rtxt_OEE_Detail");
            }
            catch (Exception ex)
            {
                Log.WriteLog("首件开始异常失败！" + ex.ToString().Replace("\r\n", ""));
            }
        }

        private void Btn_Start_break_Click(object sender, EventArgs e)
        {
            return;
            if (Global.j == 1 && !Global.Error_PendingStatus) //吃饭休息仅在待料时才能开始，待料时触发手选也不可以开始
            {
                Btn_Start_break.Enabled = false;
                Btn_UpLoad_break.Enabled = true;
                Global.BreakStatus = true;//吃饭休息开始
                //var IP = GetIp();
                //var Mac = GetMac();
                //string OEE_DT = "";
                //string msg = "";
                string EventTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Global.BreakStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                //OEE_DT = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", "7", "11010001", EventTime, "");
                //string InsertStr = "insert into OEE_TraceDT([DateTime],[Status],[ErrorCode],[EventTime],[ModuleCode])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + "7" + "'" + "," + "'" + "11010001" + "'" + ","
                //+ "'" + EventTime + "'" + "," + "'" + "" + "'" + ")";
                //SQL.ExecuteUpdate(InsertStr);
                //Log.WriteLog("OEE_DT吃饭休息:" + OEE_DT);
                //var rst = RequestAPI.Request(Global.inidata.productconfig.OEE_URL1, Global.inidata.productconfig.OEE_URL2, IP, Mac, Global.inidata.productconfig.OEE_Dsn, Global.inidata.productconfig.OEE_authCode, 2, OEE_DT, out msg);
                //if (rst)
                //{
                //_mainparent._homefrm.AppendRichText("11010001" + ",触发时间=" + EventTime + ",运行状态:" + "7" + ",故障描述:" + "吃饭休息" + ",手动发送成功", "rtx_DownTimeMsg");
                //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + "11010001" + "," + EventTime + "," + "手动发送成功" + "," + "1" + "," + "吃饭休息", @"F:\装机软件\系统配置\System_ini\");
                //Log.WriteLog("OEE_DT机台吃饭休息发送成功");
                //Global.ConnectOEEFlag = true;
                //}
                //else
                //{
                //_mainparent._homefrm.AppendRichText("11010001" + ",触发时间=" + EventTime + ",运行状态:" + "7" + ",故障描述:" + "吃饭休息" + ",手动发送失败", "rtx_DownTimeMsg");
                //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + "11010001" + "," + EventTime + "," + "手动发送失败" + "," + "1" + "," + "吃饭休息", @"F:\装机软件\系统配置\System_ini\");
                //Log.WriteLog("OEE_DT机台吃饭休息发送失败");
                //Global.ConnectOEEFlag = false;
                //string s = "insert into OEE_DTSendNG([DateTime],[Product],[Status],[ErrorCode],[EventTime],[ModuleCode],[Moduleinfo],[errorinfo])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + "OEE_DT" + "'" + "," + "'" + "7" + "'" + "," + "'" + "11010001" + "'" + ","
                //+ "'" + EventTime + "'" + "," + "'" + "" + "'" + "," + "'" + "" + "'" + "," + "'" + "吃饭休息" + "'" + ")";
                //int r = SQL.ExecuteUpdate(s);
                //Log.WriteLog(string.Format("插入了{0}行OEE_DownTime缓存数据", r));
                //}
                Global.ed[Global.Error_PendingNum + 1].stop_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                DateTime t1 = Convert.ToDateTime(Global.ed[Global.Error_PendingNum + 1].start_time);
                DateTime t2 = Convert.ToDateTime(Global.ed[Global.Error_PendingNum + 1].stop_time);
                string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                string InsertOEEStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorCode + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].start_time + "'" + "," + "'" + "" + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                SQL.ExecuteUpdate(InsertOEEStr);

                _mainparent._homefrm.AppendRichText(Global.ed[Global.Error_PendingNum + 1].start_time + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.Error_PendingNum + 1].errorCode + "," + Global.ed[Global.Error_PendingNum + 1].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[Global.Error_PendingNum + 1].errorCode + "," + Global.ed[Global.Error_PendingNum + 1].start_time + "," + "自动发送成功" + "," + Global.ed[Global.Error_PendingNum + 1].errorStatus + "," + Global.ed[Global.Error_PendingNum + 1].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");

            }
            else
            {
                MessageBox.Show("当前机台不是待料状态，不可点击吃饭休息开始!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        public void Btn_UpLoad_break_Click(object sender, EventArgs e)
        {
            return;
            Btn_Start_break.Enabled = true;
            Btn_UpLoad_break.Enabled = false;
            if (Global.BreakStatus && Global.BreakStartTime != null)
            {
                string BreakstopTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime t1 = Convert.ToDateTime(Global.BreakStartTime);
                DateTime t2 = Convert.ToDateTime(BreakstopTime);
                string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                string InsertOEEStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + "11010001" + "'" + "," + "'" + Global.BreakStartTime + "'" + "," + "'" + "" + "'" + "," + "'" + "7" + "'" + "," + "'" + "吃饭休息" + "'" + "," + "'" + ts + "'" + ")";
                SQL.ExecuteUpdate(InsertOEEStr);

                _mainparent._homefrm.AppendRichText(Global.BreakStartTime + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + "11010001" + "," + "吃饭休息" + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + "11010001" + "," + Global.BreakStartTime + "," + "自动发送成功" + "," + "7" + "," + "吃饭休息" + "," + ts, @"F:\装机软件\系统配置\System_ini\");
            }
            Global.BreakStatus = false;//吃饭休息结束
        }
    }
}
