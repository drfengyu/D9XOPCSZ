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
    public partial class AbnormalFrm : Form
    {
        private MainFrm _mainparent;
        private delegate void Labelcolor(Color color, string bl, string Name);
        List<Control> List_Control = new List<Control>();
        SQLServer SQL = new SQLServer();
        delegate void RefreachTable(Chart chart, string[] Point_X, double[] Point_Y, int index);
        private delegate void Labelvision(string bl, string Name);
        public AbnormalFrm(MainFrm mdiParent)
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
            e.Graphics.DrawString(text, new Font("微软雅黑", 15), brush, e.Bounds, sf);

        }

        private void btn_LogQuery_Click(object sender, EventArgs e)//日志信息查询
        {
            try
            {
                lst_AbnormalMes.Clear();
                if ((dtp_LogEndTime.Value - dtp_LogStartTime.Value).TotalSeconds > 0)
                {
                    int days = (dtp_LogEndTime.Value.Date - dtp_LogStartTime.Value.Date).Days;
                    if ((DateTime.Now.Date - dtp_LogStartTime.Value.Date).Days <= days)
                    {
                        days = (DateTime.Now.Date - dtp_LogStartTime.Value.Date).Days;
                    }
                    if (days == 0)
                    {
                        if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "操作记录\\" + dtp_LogStartTime.Value.Date.AddDays(days).ToString("yyyyMMdd") + ".csv"))
                        {
                            FileStream fs = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "操作记录\\" + dtp_LogStartTime.Value.Date.AddDays(days).ToString("yyyyMMdd") + ".csv", FileMode.Open, FileAccess.Read);
                            using (StreamReader sr = new StreamReader(fs, Encoding.Default))
                            {
                                string strline = "";
                                string[] anyline = null;
                                string[] tablehead = null;
                                int columncount = 0;
                                bool isfirst = true;
                                while ((strline = sr.ReadLine()) != null)
                                {
                                    if (isfirst)
                                    {
                                        tablehead = strline.Split(',');
                                        isfirst = false;
                                        columncount = tablehead.Length;
                                    }
                                    else
                                    {
                                        anyline = strline.Split(',');
                                        string str = anyline[0];
                                        if ((Convert.ToDateTime(str) - dtp_LogStartTime.Value).TotalSeconds >= 0 && (Convert.ToDateTime(str) - dtp_LogEndTime.Value).TotalSeconds <= 0)
                                        {
                                            if (cmb_type.SelectedIndex == 0)
                                            {
                                                if (txt_KeyWord.Text != null)
                                                {
                                                    if (anyline[1].Contains(txt_KeyWord.Text))
                                                    {
                                                        //_mainparent.AppendText(lst_AbnormalMes, anyline[0] + "," + anyline[1], 1);
                                                        _mainparent.AppendRichText(anyline[0] + "," + anyline[1], lst_AbnormalMes);

                                                    }
                                                }
                                                else
                                                {
                                                    _mainparent.AppendRichText(anyline[0] + "," + anyline[1], lst_AbnormalMes);

                                                }
                                            }
                                            else
                                            {
                                                if (txt_KeyWord.Text != null)
                                                {
                                                    if (anyline[1].Contains(txt_KeyWord.Text) && anyline[1].Contains(cmb_type.Text))
                                                    {
                                                        _mainparent.AppendRichText(anyline[0] + "," + anyline[1], lst_AbnormalMes);
                                                    }
                                                }
                                                else
                                                {
                                                    _mainparent.AppendRichText(anyline[0] + "," + anyline[1], lst_AbnormalMes);
                                                }
                                            }
                                        }
                                    }
                                }
                                sr.Close();
                            }
                            fs.Close();
                        }
                    }
                    else
                    {
                        for (int t = 0; t <= days; t++)
                        {
                            if ((dtp_LogStartTime.Value.Date.AddDays(t) - dtp_LogStartTime.Value.Date).Days == 0)
                            {
                                if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "操作记录\\" + dtp_LogStartTime.Value.Date.AddDays(t).ToString("yyyyMMdd") + ".csv"))
                                {
                                    FileStream fs = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "操作记录\\" + dtp_LogStartTime.Value.Date.AddDays(t).ToString("yyyyMMdd") + ".csv", FileMode.Open, FileAccess.Read);
                                    using (StreamReader sr = new StreamReader(fs, Encoding.Default))
                                    {
                                        string strline = "";
                                        string[] anyline = null;
                                        string[] tablehead = null;
                                        int columncount = 0;
                                        bool isfirst = true;
                                        while ((strline = sr.ReadLine()) != null)
                                        {
                                            if (isfirst)
                                            {
                                                tablehead = strline.Split(',');
                                                isfirst = false;
                                                columncount = tablehead.Length;
                                            }
                                            else
                                            {
                                                anyline = strline.Split(',');
                                                string str = anyline[0];
                                                if ((Convert.ToDateTime(str) - dtp_LogStartTime.Value).TotalSeconds >= 0)
                                                {
                                                    if (cmb_type.SelectedIndex == 0)
                                                    {
                                                        if (txt_KeyWord.Text != null)
                                                        {
                                                            if (anyline[1].Contains(txt_KeyWord.Text))
                                                            {
                                                                //_mainparent.AppendText(lst_AbnormalMes, anyline[0] + "," + anyline[1], 1);
                                                                _mainparent.AppendRichText(anyline[0] + "," + anyline[1], lst_AbnormalMes);

                                                            }
                                                        }
                                                        else
                                                        {
                                                            //_mainparent.AppendText(lst_AbnormalMes, anyline[0] + "," + anyline[1], 1);
                                                            _mainparent.AppendRichText(anyline[0] + "," + anyline[1], lst_AbnormalMes);

                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (txt_KeyWord.Text != null)
                                                        {
                                                            if (anyline[1].Contains(txt_KeyWord.Text) && anyline[1].Contains(cmb_type.Text))
                                                            {
                                                                //_mainparent.AppendText(lst_AbnormalMes, anyline[0] + "," + anyline[1], 1);
                                                                _mainparent.AppendRichText(anyline[0] + "," + anyline[1], lst_AbnormalMes);

                                                            }
                                                        }
                                                        else
                                                        {
                                                            //_mainparent.AppendText(lst_AbnormalMes, anyline[0] + "," + anyline[1], 1);
                                                            _mainparent.AppendRichText(anyline[0] + "," + anyline[1], lst_AbnormalMes);

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        sr.Close();
                                    }
                                    fs.Close();
                                }
                            }
                            else if ((dtp_LogStartTime.Value.Date.AddDays(t) - dtp_LogStartTime.Value.Date).Days > 0 && (dtp_LogStartTime.Value.Date.AddDays(t) - dtp_LogEndTime.Value.Date).Days < 0)
                            {
                                if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "操作记录\\" + dtp_LogStartTime.Value.Date.AddDays(t).ToString("yyyyMMdd") + ".csv"))
                                {
                                    FileStream fs = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "操作记录\\" + dtp_LogStartTime.Value.Date.AddDays(t).ToString("yyyyMMdd") + ".csv", FileMode.Open, FileAccess.Read);
                                    using (StreamReader sr = new StreamReader(fs, Encoding.Default))
                                    {
                                        string strline = "";
                                        string[] anyline = null;
                                        string[] tablehead = null;
                                        int columncount = 0;
                                        bool isfirst = true;
                                        while ((strline = sr.ReadLine()) != null)
                                        {
                                            if (isfirst)
                                            {
                                                tablehead = strline.Split(',');
                                                isfirst = false;
                                                columncount = tablehead.Length;
                                            }
                                            else
                                            {
                                                anyline = strline.Split(',');
                                                if (cmb_type.SelectedIndex == 0)
                                                {
                                                    if (txt_KeyWord.Text != null)
                                                    {
                                                        if (anyline[1].Contains(txt_KeyWord.Text))
                                                        {
                                                            //_mainparent.AppendText(lst_AbnormalMes, anyline[0] + "," + anyline[1], 1);
                                                            _mainparent.AppendRichText(anyline[0] + "," + anyline[1], lst_AbnormalMes);

                                                        }
                                                    }
                                                    else
                                                    {
                                                        //_mainparent.AppendText(lst_AbnormalMes, anyline[0] + "," + anyline[1], 1);
                                                        _mainparent.AppendRichText(anyline[0] + "," + anyline[1], lst_AbnormalMes);

                                                    }
                                                }
                                                else
                                                {
                                                    if (txt_KeyWord.Text != null)
                                                    {
                                                        if (anyline[1].Contains(txt_KeyWord.Text) && anyline[1].Contains(cmb_type.Text))
                                                        {
                                                            //_mainparent.AppendText(lst_AbnormalMes, anyline[0] + "," + anyline[1], 1);
                                                            _mainparent.AppendRichText(anyline[0] + "," + anyline[1], lst_AbnormalMes);

                                                        }
                                                    }
                                                    else
                                                    {
                                                        //_mainparent.AppendText(lst_AbnormalMes, anyline[0] + "," + anyline[1], 1);
                                                        _mainparent.AppendRichText(anyline[0] + "," + anyline[1], lst_AbnormalMes);

                                                    }
                                                }
                                            }
                                        }
                                        sr.Close();
                                    }
                                    fs.Close();
                                }
                            }
                            else if ((dtp_LogStartTime.Value.Date.AddDays(t) - dtp_LogEndTime.Value.Date).Days == 0)
                            {
                                if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "操作记录\\" + dtp_LogStartTime.Value.Date.AddDays(t).ToString("yyyyMMdd") + ".csv"))
                                {
                                    FileStream fs = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "操作记录\\" + dtp_LogStartTime.Value.Date.AddDays(t).ToString("yyyyMMdd") + ".csv", FileMode.Open, FileAccess.Read);
                                    using (StreamReader sr = new StreamReader(fs, Encoding.Default))
                                    {
                                        string strline = "";
                                        string[] anyline = null;
                                        string[] tablehead = null;
                                        int columncount = 0;
                                        bool isfirst = true;
                                        while ((strline = sr.ReadLine()) != null)
                                        {
                                            if (isfirst)
                                            {
                                                tablehead = strline.Split(',');
                                                isfirst = false;
                                                columncount = tablehead.Length;
                                            }
                                            else
                                            {
                                                anyline = strline.Split(',');
                                                string str = anyline[0];
                                                if ((Convert.ToDateTime(str) - dtp_LogEndTime.Value).TotalSeconds <= 0)
                                                {
                                                    if (cmb_type.SelectedIndex == 0)
                                                    {
                                                        if (txt_KeyWord.Text != null)
                                                        {
                                                            if (anyline[1].Contains(txt_KeyWord.Text))
                                                            {
                                                                //_mainparent.AppendText(lst_AbnormalMes, anyline[0] + "," + anyline[1], 1);
                                                                _mainparent.AppendRichText(anyline[0] + "," + anyline[1], lst_AbnormalMes);

                                                            }
                                                        }
                                                        else
                                                        {
                                                            //_mainparent.AppendText(lst_AbnormalMes, anyline[0] + "," + anyline[1], 1);
                                                            _mainparent.AppendRichText(anyline[0] + "," + anyline[1], lst_AbnormalMes);

                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (txt_KeyWord.Text != null)
                                                        {
                                                            if (anyline[1].Contains(txt_KeyWord.Text) && anyline[1].Contains(cmb_type.Text))
                                                            {
                                                                //_mainparent.AppendText(lst_AbnormalMes, anyline[0] + "," + anyline[1], 1);
                                                                _mainparent.AppendRichText(anyline[0] + "," + anyline[1], lst_AbnormalMes);

                                                            }
                                                        }
                                                        else
                                                        {
                                                            //_mainparent.AppendText(lst_AbnormalMes, anyline[0] + "," + anyline[1], 1);
                                                            _mainparent.AppendRichText(anyline[0] + "," + anyline[1], lst_AbnormalMes);

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        sr.Close();
                                    }
                                    fs.Close();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString().Replace("\r\n", ""));
            }
        }

        private void btn_LogExport_Click(object sender, EventArgs e)
        {
            List<string> lines = new List<string>();
            SaveFileDialog sf = new SaveFileDialog();
            sf.Title = "文档导出";
            sf.Filter = "文档(*.csv)|*.csv";
            sf.FileName = DateTime.Now.Date.ToString("yyyyMMdd");
            if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string str = sf.FileName;
                foreach (var item in lst_AbnormalMes.Lines)
                {
                    lines.Add(item.ToString().Replace(":(", ",").TrimEnd(new[] { ')' }));
                }
                File.WriteAllLines(sf.FileName, lines);
            }
        }

        private void AbnormalFrm_Load(object sender, EventArgs e)
        {

            List_Control = GetAllControls(this);//列表中添加所有窗体控件
            cmb_type.SelectedIndex = 0;
            string[] x = new string[] { "50011001", "50011010", "50012064", "60041023", "20010001" };
            double[] y = new double[] { 220, 200, 187, 130, 70 };

            string[] a = new string[] { "30010002", "11010001", "50011057", "60071005", "70010006" };
            double[] b = new double[] { 1220.1, 1050.6, 970.2, 930.8, 870.2 };
            #region 异常次数柱状图
            //标题
            chart_AbnormalNumber.Titles.Add("异常次数Top5");
            chart_AbnormalNumber.Titles[0].ForeColor = Color.Black;
            chart_AbnormalNumber.Titles[0].Font = new Font("微软雅黑", 12f, FontStyle.Regular);
            chart_AbnormalNumber.Titles[0].Alignment = ContentAlignment.TopCenter;
            //chart_AbnormalNumber.Titles.Add("合计：25414 宗");
            //chart_AbnormalNumber.Titles[1].ForeColor = Color.White;
            //chart_AbnormalNumber.Titles[1].Font = new Font("微软雅黑", 8f, FontStyle.Regular);
            //chart_AbnormalNumber.Titles[1].Alignment = ContentAlignment.TopRight;
            //控件背景
            //chart_AbnormalNumber.BackColor = Color.Transparent;
            //图表区背景
            //chart_AbnormalNumber.ChartAreas[0].BackColor = Color.Transparent;
            //chart_AbnormalNumber.ChartAreas[0].BorderColor = Color.Transparent;
            //X轴标签间距
            //chart_AbnormalNumber.ChartAreas[0].AxisX.Interval = 1;
            //chart_AbnormalNumber.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true;
            //chart_AbnormalNumber.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            //chart_AbnormalNumber.ChartAreas[0].AxisX.TitleFont = new Font("微软雅黑", 14f, FontStyle.Regular);
            //chart_AbnormalNumber.ChartAreas[0].AxisX.TitleForeColor = Color.White;
            //X坐标轴颜色
            //chart_AbnormalNumber.ChartAreas[0].AxisX.LineColor = ColorTranslator.FromHtml("#38587a"); ;
            //chart_AbnormalNumber.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            //chart_AbnormalNumber.ChartAreas[0].AxisX.LabelStyle.Font = new Font("微软雅黑", 10f, FontStyle.Regular);
            //X坐标轴标题
            chart_AbnormalNumber.ChartAreas[0].AxisX.Title = "异常类型";
            chart_AbnormalNumber.ChartAreas[0].AxisX.TitleFont = new Font("微软雅黑", 10f, FontStyle.Regular);
            chart_AbnormalNumber.ChartAreas[0].AxisX.TitleForeColor = Color.Black;
            chart_AbnormalNumber.ChartAreas[0].AxisX.ArrowStyle = AxisArrowStyle.Triangle;
            chart_AbnormalNumber.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Horizontal;
            chart_AbnormalNumber.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart_AbnormalNumber.ChartAreas[0].AxisX.ToolTip = "异常类型";
            //X轴网络线条
            //chart_AbnormalNumber.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            //chart_AbnormalNumber.ChartAreas[0].AxisX.MajorGrid.LineColor = ColorTranslator.FromHtml("#2c4c6d");
            //Y坐标轴颜色
            //chart_AbnormalNumber.ChartAreas[0].AxisY.LineColor = ColorTranslator.FromHtml("#38587a");
            //chart_AbnormalNumber.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Black;
            //chart_AbnormalNumber.ChartAreas[0].AxisY.LabelStyle.Font = new Font("微软雅黑", 10f, FontStyle.Regular);
            //Y坐标轴标题
            chart_AbnormalNumber.ChartAreas[0].AxisY.Title = "异常次数";
            chart_AbnormalNumber.ChartAreas[0].AxisY.TitleFont = new Font("微软雅黑", 10f, FontStyle.Regular);
            chart_AbnormalNumber.ChartAreas[0].AxisY.TitleForeColor = Color.Black;
            chart_AbnormalNumber.ChartAreas[0].AxisY.ArrowStyle = AxisArrowStyle.Triangle;
            chart_AbnormalNumber.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Stacked;
            chart_AbnormalNumber.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart_AbnormalNumber.ChartAreas[0].AxisY.ToolTip = "异常次数";
            //Y轴网格线条
            //chart_AbnormalNumber.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            //chart_AbnormalNumber.ChartAreas[0].AxisY.MajorGrid.LineColor = ColorTranslator.FromHtml("#2c4c6d");
            //chart_AbnormalNumber.ChartAreas[0].AxisY2.LineColor = Color.Transparent;
            //chart_AbnormalNumber.ChartAreas[0].BackGradientStyle = GradientStyle.TopBottom;
            Legend legend = new Legend("legend");
            legend.Title = "legendTitle";
            chart_AbnormalNumber.Series[0].XValueType = ChartValueType.String;  //设置X轴上的值类型
            chart_AbnormalNumber.Series[0].Label = "#VAL";                //设置显示X Y的值    
            chart_AbnormalNumber.Series[0].LabelForeColor = Color.Black;
            chart_AbnormalNumber.Series[0].ToolTip = "#VALX:#VAL";     //鼠标移动到对应点显示数值
            chart_AbnormalNumber.Series[0].ChartType = SeriesChartType.Column;    //图类型(柱状)
            chart_AbnormalNumber.Series[0].Color = Color.Lime;
            chart_AbnormalNumber.Series[0].LegendText = legend.Name;
            chart_AbnormalNumber.Series[0].IsValueShownAsLabel = true;
            chart_AbnormalNumber.Series[0].CustomProperties = "DrawingStyle = Cylinder";
            chart_AbnormalNumber.Legends.Add(legend);
            chart_AbnormalNumber.Legends[0].Position.Auto = false;
            //绑定数据
            //chart_AbnormalNumber.Series[0].Points.DataBindXY(x, y);
            chart_AbnormalNumber.Series[0].Palette = ChartColorPalette.BrightPastel;
            #endregion

            #region 异常时间柱状图
            //标题
            chart_AbnomalTime.Titles.Add("异常时间Top5");
            chart_AbnomalTime.Titles[0].ForeColor = Color.Black;
            chart_AbnomalTime.Titles[0].Font = new Font("微软雅黑", 12f, FontStyle.Regular);
            chart_AbnomalTime.Titles[0].Alignment = ContentAlignment.TopCenter;
            //chart_AnomalousTime.Titles.Add("合计：25414 宗");
            //chart_AnomalousTime.Titles[1].ForeColor = Color.White;
            //chart_AnomalousTime.Titles[1].Font = new Font("微软雅黑", 8f, FontStyle.Regular);
            //chart_AnomalousTime.Titles[1].Alignment = ContentAlignment.TopRight;
            //控件背景
            //chart_AnomalousTime.BackColor = Color.Transparent;
            //图表区背景
            //chart_AnomalousTime.ChartAreas[0].BackColor = Color.Transparent;
            //chart_AnomalousTime.ChartAreas[0].BorderColor = Color.Transparent;
            //X轴标签间距
            //chart_AnomalousTime.ChartAreas[0].AxisX.Interval = 1;
            //chart_AnomalousTime.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true;
            //chart_AnomalousTime.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            //chart_AnomalousTime.ChartAreas[0].AxisX.TitleFont = new Font("微软雅黑", 14f, FontStyle.Regular);
            //chart_AnomalousTime.ChartAreas[0].AxisX.TitleForeColor = Color.White;
            //X坐标轴颜色
            //chart_AnomalousTime.ChartAreas[0].AxisX.LineColor = ColorTranslator.FromHtml("#38587a"); ;
            //chart_AnomalousTime.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            //chart_AnomalousTime.ChartAreas[0].AxisX.LabelStyle.Font = new Font("微软雅黑", 10f, FontStyle.Regular);
            //X坐标轴标题
            chart_AbnomalTime.ChartAreas[0].AxisX.Title = "异常类型";
            chart_AbnomalTime.ChartAreas[0].AxisX.TitleFont = new Font("微软雅黑", 10f, FontStyle.Regular);
            chart_AbnomalTime.ChartAreas[0].AxisX.TitleForeColor = Color.Black;
            chart_AbnomalTime.ChartAreas[0].AxisX.ArrowStyle = AxisArrowStyle.Triangle;
            chart_AbnomalTime.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Horizontal;
            chart_AbnomalTime.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart_AbnomalTime.ChartAreas[0].AxisX.ToolTip = "异常类型";
            //X轴网络线条
            //chart_AnomalousTime.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            //chart_AnomalousTime.ChartAreas[0].AxisX.MajorGrid.LineColor = ColorTranslator.FromHtml("#2c4c6d");
            //Y坐标轴颜色
            //chart_AnomalousTime.ChartAreas[0].AxisY.LineColor = ColorTranslator.FromHtml("#38587a");
            //chart_AnomalousTime.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Black;
            //chart_AnomalousTime.ChartAreas[0].AxisY.LabelStyle.Font = new Font("微软雅黑", 10f, FontStyle.Regular);
            //Y坐标轴标题
            chart_AbnomalTime.ChartAreas[0].AxisY.Title = "异常时间（分）";
            chart_AbnomalTime.ChartAreas[0].AxisY.TitleFont = new Font("微软雅黑", 10f, FontStyle.Regular);
            chart_AbnomalTime.ChartAreas[0].AxisY.TitleForeColor = Color.Black;
            chart_AbnomalTime.ChartAreas[0].AxisY.ArrowStyle = AxisArrowStyle.Triangle;
            chart_AbnomalTime.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Stacked;
            chart_AbnomalTime.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart_AbnomalTime.ChartAreas[0].AxisY.ToolTip = "异常时间（分）";
            //Y轴网格线条
            //chart_AnomalousTime.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            //chart_AnomalousTime.ChartAreas[0].AxisY.MajorGrid.LineColor = ColorTranslator.FromHtml("#2c4c6d");
            //chart_AnomalousTime.ChartAreas[0].AxisY2.LineColor = Color.Transparent;
            //chart_AnomalousTime.ChartAreas[0].BackGradientStyle = GradientStyle.TopBottom;
            Legend legend2 = new Legend("legend");
            legend2.Title = "legendTitle";
            chart_AbnomalTime.Series[0].XValueType = ChartValueType.String;  //设置X轴上的值类型
            chart_AbnomalTime.Series[0].Label = "#VAL";                //设置显示X Y的值    
            chart_AbnomalTime.Series[0].LabelForeColor = Color.Black;
            chart_AbnomalTime.Series[0].ToolTip = "#VALX:#VAL";     //鼠标移动到对应点显示数值
            chart_AbnomalTime.Series[0].ChartType = SeriesChartType.Column;    //图类型(柱状)
            chart_AbnomalTime.Series[0].Color = Color.Lime;
            chart_AbnomalTime.Series[0].LegendText = legend2.Name;
            chart_AbnomalTime.Series[0].IsValueShownAsLabel = true;
            chart_AbnomalTime.Series[0].CustomProperties = "DrawingStyle = Cylinder";
            chart_AbnomalTime.Legends.Add(legend2);
            chart_AbnomalTime.Legends[0].Position.Auto = false;
            //绑定数据
            //chart_AbnomalTime.Series[0].Points.DataBindXY(a, b);
            chart_AbnomalTime.Series[0].Palette = ChartColorPalette.BrightPastel;
            #endregion
        }

        private void btn_ChartQuery_Click(object sender, EventArgs e)//图表查询
        {
            string[] x = new string[] { "待料", "运行", "宕机", "首件", "保养" };
            double[] y = new double[] { 20.00, 10.00, 30.00, 25.00, 15.00 };

            string[] a = new string[] { "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19" };
            double[] b = new double[] { 20.1, 10.50, 30.20, 25.19, 15.02, 20.1, 10.50, 30.20, 25.19, 15.02, 5.21, 3.26 };
            //#region 饼图总DT
            ////标题
            //chart_TotalDT.Titles.Add("Total DT");
            //chart_TotalDT.Titles[0].ForeColor = Color.Green;
            //chart_TotalDT.Titles[0].Font = new Font("Calibri", 12f, FontStyle.Bold);
            //chart_TotalDT.Titles[0].Alignment = ContentAlignment.TopCenter;
            ////chart_TotalDT.Titles.Add("合计：25412 宗");
            ////chart_TotalDT.Titles[1].ForeColor = Color.White;
            ////chart_TotalDT.Titles[1].Font = new Font("微软雅黑", 8f, FontStyle.Regular);
            ////chart_TotalDT.Titles[1].Alignment = ContentAlignment.TopRight;
            ////控件背景
            //chart_TotalDT.BackColor = Color.Transparent;
            ////图表区背景
            //chart_TotalDT.ChartAreas[0].BackColor = Color.Transparent;
            //chart_TotalDT.ChartAreas[0].BorderColor = Color.Transparent;
            ////X轴标签间距
            ////chart_TotalDT.ChartAreas[0].AxisX.Interval = 1;
            ////chart_TotalDT.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true;
            ////chart_TotalDT.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            ////chart_TotalDT.ChartAreas[0].AxisX.TitleFont = new Font("Calibri", 14f, FontStyle.Regular);
            ////chart_TotalDT.ChartAreas[0].AxisX.TitleForeColor = Color.Black;
            ////X坐标轴颜色
            ////chart_TotalDT.ChartAreas[0].AxisX.LineColor = ColorTranslator.FromHtml("#38587a"); ;
            ////chart_TotalDT.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Black;
            ////chart_TotalDT.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Calibri", 10f, FontStyle.Regular);
            ////X坐标轴标题
            ////chart_TotalDT.ChartAreas[0].AxisX.Title = "数量(宗)";
            ////chart_TotalDT.ChartAreas[0].AxisX.TitleFont = new Font("Calibri", 10f, FontStyle.Regular);
            ////chart_TotalDT.ChartAreas[0].AxisX.TitleForeColor = Color.Black;
            ////chart_TotalDT.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Horizontal;
            ////chart_TotalDT.ChartAreas[0].AxisX.ToolTip = "数量(宗)";
            ////X轴网络线条
            ////chart_TotalDT.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            ////chart_TotalDT.ChartAreas[0].AxisX.MajorGrid.LineColor = ColorTranslator.FromHtml("#2c4c6d");
            ////Y坐标轴颜色
            ////chart_TotalDT.ChartAreas[0].AxisY.LineColor = ColorTranslator.FromHtml("#38587a");
            ////chart_TotalDT.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Black;
            ////chart_TotalDT.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Calibri", 10f, FontStyle.Regular);
            ////Y坐标轴标题
            ////chart_TotalDT.ChartAreas[0].AxisY.Title = "数量(宗)";
            ////chart_TotalDT.ChartAreas[0].AxisY.TitleFont = new Font("Calibri", 10f, FontStyle.Regular);
            ////chart_TotalDT.ChartAreas[0].AxisY.TitleForeColor = Color.Black;
            ////chart_TotalDT.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Rotated270;
            ////chart_TotalDT.ChartAreas[0].AxisY.ToolTip = "数量(宗)";
            ////Y轴网格线条
            ////chart_TotalDT.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            ////chart_TotalDT.ChartAreas[0].AxisY.MajorGrid.LineColor = ColorTranslator.FromHtml("#2c4c6d");
            ////chart_TotalDT.ChartAreas[0].AxisY2.LineColor = Color.Transparent;
            ////背景渐变
            //chart_TotalDT.ChartAreas[0].BackGradientStyle = GradientStyle.None;
            ////图例样式
            //Legend legend = new Legend("#VALX");
            //legend.Title = "图例";
            //legend.TitleBackColor = Color.Transparent;
            //legend.BackColor = Color.Transparent;
            //legend.TitleForeColor = Color.Black;
            //legend.TitleFont = new Font("Calibri", 10f, FontStyle.Regular);
            //legend.Font = new Font("Calibri", 8f, FontStyle.Regular);
            //legend.ForeColor = Color.Black;
            //chart_TotalDT.Series[0].XValueType = ChartValueType.String;  //设置X轴上的值类型
            //chart_TotalDT.Series[0].Label = "#VALX;#PERCENT{P2}";                //设置显示X Y的值    
            //chart_TotalDT.Series[0].LabelForeColor = Color.Black;
            //chart_TotalDT.Series[0].ToolTip = "#VALX:#VAL";     //鼠标移动到对应点显示数值
            //chart_TotalDT.Series[0].ChartType = SeriesChartType.Pie;    //图类型(折线)
            //chart_TotalDT.Series[0].Color = Color.Lime;
            //chart_TotalDT.Series[0].LegendText = legend.Name;
            //chart_TotalDT.Series[0].IsValueShownAsLabel = true;
            //chart_TotalDT.Series[0].LabelForeColor = Color.Black;
            ////chart_TotalDT.Series[0].CustomProperties = "DrawingStyle = Cylinder";
            ////chart_TotalDT.Series[0].CustomProperties = "PieLabelStyle = Outside";
            //chart_TotalDT.Legends.Add(legend);
            //chart_TotalDT.Legends[0].Position.Auto = true;
            //chart_TotalDT.Series[0].IsValueShownAsLabel = true;
            ////是否显示图例
            //chart_TotalDT.Series[0].IsVisibleInLegend = true;
            //chart_TotalDT.Series[0].ShadowOffset = 0;
            ////饼图折线
            //chart_TotalDT.Series[0]["PieLineColor"] = "Black";
            ////绑定数据
            //chart_TotalDT.Series[0].Points.DataBindXY(x, y);
            ////绑定颜色
            //chart_TotalDT.Series[0].Palette = ChartColorPalette.BrightPastel;

            //#endregion

            //#region 柱状图每小时DT
            ////标题
            //chart_HourDT.Titles.Add("Hour DT");
            //chart_HourDT.Titles[0].ForeColor = Color.Green;
            //chart_HourDT.Titles[0].Font = new Font("Calibri", 12f, FontStyle.Bold);
            //chart_HourDT.Titles[0].Alignment = ContentAlignment.TopCenter;
            ////chart_AnomalousTime.Titles.Add("合计：25414 宗");
            ////chart_AnomalousTime.Titles[1].ForeColor = Color.White;
            ////chart_AnomalousTime.Titles[1].Font = new Font("微软雅黑", 8f, FontStyle.Regular);
            ////chart_AnomalousTime.Titles[1].Alignment = ContentAlignment.TopRight;
            ////控件背景
            ////chart_AnomalousTime.BackColor = Color.Transparent;
            ////图表区背景
            ////chart_AnomalousTime.ChartAreas[0].BackColor = Color.Transparent;
            ////chart_AnomalousTime.ChartAreas[0].BorderColor = Color.Transparent;
            ////X轴标签间距
            //chart_HourDT.ChartAreas[0].AxisX.Interval = 1;
            //chart_HourDT.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true;
            ////chart_HourDT.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            //chart_HourDT.ChartAreas[0].AxisX.TitleFont = new Font("Calibri", 14f, FontStyle.Regular);
            //chart_HourDT.ChartAreas[0].AxisX.TitleForeColor = Color.White;
            ////X坐标轴颜色
            ////chart_AnomalousTime.ChartAreas[0].AxisX.LineColor = ColorTranslator.FromHtml("#38587a"); ;
            ////chart_AnomalousTime.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            ////chart_AnomalousTime.ChartAreas[0].AxisX.LabelStyle.Font = new Font("微软雅黑", 10f, FontStyle.Regular);
            ////X坐标轴标题
            ////chart_HourDT.ChartAreas[0].AxisX.Title = "数量";
            ////chart_HourDT.ChartAreas[0].AxisX.TitleFont = new Font("Calibri", 10f, FontStyle.Regular);
            ////chart_HourDT.ChartAreas[0].AxisX.TitleForeColor = Color.Black;
            ////chart_HourDT.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Horizontal;
            ////chart_HourDT.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            ////chart_HourDT.ChartAreas[0].AxisX.ToolTip = "数量";
            ////X轴网络线条
            //chart_HourDT.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            //chart_HourDT.ChartAreas[0].AxisX.MajorGrid.LineColor = ColorTranslator.FromHtml("#2c4c6d");
            ////Y坐标轴颜色
            ////chart_AnomalousTime.ChartAreas[0].AxisY.LineColor = ColorTranslator.FromHtml("#38587a");
            ////chart_AnomalousTime.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Black;
            ////chart_AnomalousTime.ChartAreas[0].AxisY.LabelStyle.Font = new Font("微软雅黑", 10f, FontStyle.Regular);
            ////Y坐标轴标题
            ////chart_HourDT.ChartAreas[0].AxisY.Title = "时间(min)";
            ////chart_HourDT.ChartAreas[0].AxisY.TitleFont = new Font("Calibri", 10f, FontStyle.Regular);
            ////chart_HourDT.ChartAreas[0].AxisY.TitleForeColor = Color.Black;
            ////chart_HourDT.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Stacked;
            ////chart_HourDT.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            ////chart_HourDT.ChartAreas[0].AxisY.ToolTip = "时间(min)";
            ////Y轴网格线条
            //chart_HourDT.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            //chart_HourDT.ChartAreas[0].AxisY.MajorGrid.LineColor = ColorTranslator.FromHtml("#2c4c6d");
            //chart_HourDT.ChartAreas[0].AxisY2.LineColor = Color.Transparent;
            //chart_HourDT.ChartAreas[0].BackGradientStyle = GradientStyle.TopBottom;
            //Legend legend2 = new Legend("报警时长(min)");
            //legend2.Title = "legendTitle";
            //legend2.TitleBackColor = Color.Transparent;
            //legend2.BackColor = Color.Transparent;
            //legend2.TitleForeColor = Color.Black;
            //legend2.TitleFont = new Font("Calibri", 10f, FontStyle.Regular);
            //legend2.Font = new Font("Calibri", 8f, FontStyle.Regular);
            //legend2.ForeColor = Color.Black;
            //chart_HourDT.Series[0].XValueType = ChartValueType.String;  //设置X轴上的值类型
            //chart_HourDT.Series[0].Label = "#VAL";                //设置显示X Y的值    
            //chart_HourDT.Series[0].LabelForeColor = Color.Black;
            //chart_HourDT.Series[0].ToolTip = "#VALX:#VAL";     //鼠标移动到对应点显示数值
            //chart_HourDT.Series[0].ChartType = SeriesChartType.Column;    //图类型(柱状)
            //chart_HourDT.Series[0].Color = Color.LightCoral;
            //chart_HourDT.Series[0].LegendText = legend2.Name;
            //chart_HourDT.Series[0].IsValueShownAsLabel = true;
            //chart_HourDT.Series[0].IsVisibleInLegend = true;
            ////chart_HourDT.Series[0].CustomProperties = "DrawingStyle = Cylinder";
            //chart_HourDT.Legends.Add(legend2);
            //chart_HourDT.Legends[0].Position.Auto = true;
            ////绑定数据
            //chart_HourDT.Series[0].Points.DataBindXY(a, b);
            //for (int i = 0; i < 12; i++)
            //{
            //    chart_HourDT.Series[0].Points[i].Color = Color.LightCoral;
            //}
            //#endregion
        }

        private void chart_HourDT_MouseClick(object sender, MouseEventArgs e)
        {
            //HitTestResult myTestResult = chart_HourDT.HitTest(e.X, e.Y);
            //if (myTestResult.ChartElementType == ChartElementType.DataPoint)
            //{
            //    int index = myTestResult.PointIndex;
            //    var xValue = myTestResult.Series.Points[index].AxisLabel;
            //    MessageBox.Show(xValue.ToString());
            //}
            //else
            //{}
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
                    double[] Y = new double[5];
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

        public void UD_TOP5(object ob)
        {
            
            while (true)
            {
                try
                {
                    string[] AbnormalNumber_ErrorcodeTOP5 = new string[] { "", "", "", "", "" };
                    string[] AbnormalNumber_ErrorinfoTOP5 = new string[] { "", "", "", "", "" };
                    double[] AbnormalNumber_ErrorCountTOP5 = new double[] { 0, 0, 0, 0, 0 };
                    string[] AbnormalTime_ErrorcodeTOP5 = new string[] { "", "", "", "", "" };
                    string[] AbnormalTime_ErrorinfoTOP5 = new string[] { "", "", "", "", "" };
                    double[] AbnormalTime_ErrorTimeSpanTOP5 = new double[] { 0, 0, 0, 0, 0 };
                    // --where(cast(DateTime as datetime) between '2020-05-13 0:0:0' and '2020-05-14 15:21:0') and ErrorCode!= ''
                    string SelectStr = "SELECT TOP 5 count(ErrorCode) as number,Errorcode,ErrorInfo FROM OEE_DT " +
                    "where datediff(day, EventTime, getdate()) = 0 and ErrorCode!= '' " +
                    "group by ErrorCode ,ErrorInfo order by COUNT(ErrorCode) DESC";
                    DataTable d1 = SQL.ExecuteQuery(SelectStr);
                    if (d1.Rows.Count > 0)
                    {
                        for (int i = 0; i < d1.Rows.Count; i++)
                        {
                            AbnormalNumber_ErrorCountTOP5[i] = Convert.ToDouble(d1.Rows[i][0].ToString());
                            AbnormalNumber_ErrorcodeTOP5[i] = d1.Rows[i][1].ToString();
                            AbnormalNumber_ErrorinfoTOP5[i] = d1.Rows[i][2].ToString();
                        }
                        UpDatalabel(AbnormalNumber_ErrorcodeTOP5[0], "lb_errorcode_number_1");
                        UpDatalabel(AbnormalNumber_ErrorcodeTOP5[1], "lb_errorcode_number_2");
                        UpDatalabel(AbnormalNumber_ErrorcodeTOP5[2], "lb_errorcode_number_3");
                        UpDatalabel(AbnormalNumber_ErrorcodeTOP5[3], "lb_errorcode_number_4");
                        UpDatalabel(AbnormalNumber_ErrorcodeTOP5[4], "lb_errorcode_number_5");
                        UpDatalabel(AbnormalNumber_ErrorinfoTOP5[0], "lb_errorinfo_number_1");
                        UpDatalabel(AbnormalNumber_ErrorinfoTOP5[1], "lb_errorinfo_number_2");
                        UpDatalabel(AbnormalNumber_ErrorinfoTOP5[2], "lb_errorinfo_number_3");
                        UpDatalabel(AbnormalNumber_ErrorinfoTOP5[3], "lb_errorinfo_number_4");
                        UpDatalabel(AbnormalNumber_ErrorinfoTOP5[4], "lb_errorinfo_number_5");
                        RefreachData(chart_AbnormalNumber, AbnormalNumber_ErrorcodeTOP5, AbnormalNumber_ErrorCountTOP5, 0);
                    }
                    // --where(cast(DateTime as datetime) between '2020-05-13 0:0:0' and '2020-05-14 15:35:0') and ErrorCode!= ''
                    string SelectStr2 = "SELECT TOP 5 sum((cast(TimeSpan as float))) as '时长',Errorcode,ErrorInfo FROM OEE_DT " +
                    "where datediff(day, EventTime, getdate()) = 0 and ErrorCode!= '' " +
                    "group by Errorcode,ErrorInfo order by sum((cast(TimeSpan as float))) desc";
                    DataTable d2 = SQL.ExecuteQuery(SelectStr2);
                    if (d2.Rows.Count > 0)
                    {
                        for (int i = 0; i < d2.Rows.Count; i++)
                        {
                            AbnormalTime_ErrorTimeSpanTOP5[i] = Convert.ToDouble(d2.Rows[i][0].ToString());
                            AbnormalTime_ErrorcodeTOP5[i] = d2.Rows[i][1].ToString();
                            AbnormalTime_ErrorinfoTOP5[i] = d2.Rows[i][2].ToString();
                        }
                        UpDatalabel(AbnormalTime_ErrorcodeTOP5[0], "lb_errorcode_Time_1");
                        UpDatalabel(AbnormalTime_ErrorcodeTOP5[1], "lb_errorcode_Time_2");
                        UpDatalabel(AbnormalTime_ErrorcodeTOP5[2], "lb_errorcode_Time_3");
                        UpDatalabel(AbnormalTime_ErrorcodeTOP5[3], "lb_errorcode_Time_4");
                        UpDatalabel(AbnormalTime_ErrorcodeTOP5[4], "lb_errorcode_Time_5");
                        UpDatalabel(AbnormalTime_ErrorinfoTOP5[0], "lb_errorinfo_Time_1");
                        UpDatalabel(AbnormalTime_ErrorinfoTOP5[1], "lb_errorinfo_Time_2");
                        UpDatalabel(AbnormalTime_ErrorinfoTOP5[2], "lb_errorinfo_Time_3");
                        UpDatalabel(AbnormalTime_ErrorinfoTOP5[3], "lb_errorinfo_Time_4");
                        UpDatalabel(AbnormalTime_ErrorinfoTOP5[4], "lb_errorinfo_Time_5");
                        RefreachData(chart_AbnomalTime, AbnormalTime_ErrorcodeTOP5, AbnormalTime_ErrorTimeSpanTOP5, 0);
                    }
                    if (DateTime.Now.Hour == 0 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 1)
                    {
                        UpDatalabel("", "lb_errorcode_number_1");
                        UpDatalabel("", "lb_errorcode_number_2");
                        UpDatalabel("", "lb_errorcode_number_3");
                        UpDatalabel("", "lb_errorcode_number_4");
                        UpDatalabel("", "lb_errorcode_number_5");
                        UpDatalabel("", "lb_errorinfo_number_1");
                        UpDatalabel("", "lb_errorinfo_number_2");
                        UpDatalabel("", "lb_errorinfo_number_3");
                        UpDatalabel("", "lb_errorinfo_number_4");
                        UpDatalabel("", "lb_errorinfo_number_5");
                        UpDatalabel("", "lb_errorcode_Time_1");
                        UpDatalabel("", "lb_errorcode_Time_2");
                        UpDatalabel("", "lb_errorcode_Time_3");
                        UpDatalabel("", "lb_errorcode_Time_4");
                        UpDatalabel("", "lb_errorcode_Time_5");
                        UpDatalabel("", "lb_errorinfo_Time_1");
                        UpDatalabel("", "lb_errorinfo_Time_2");
                        UpDatalabel("", "lb_errorinfo_Time_3");
                        UpDatalabel("", "lb_errorinfo_Time_4");
                        UpDatalabel("", "lb_errorinfo_Time_5");
                        RefreachData(chart_AbnormalNumber, AbnormalNumber_ErrorcodeTOP5, AbnormalNumber_ErrorCountTOP5, 100);
                        RefreachData(chart_AbnomalTime, AbnormalTime_ErrorcodeTOP5, AbnormalTime_ErrorTimeSpanTOP5, 100);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString().Replace("\r\n", ""));
                }
                Thread.Sleep(500);
            }
        }

        private void btn_SelectTop5Data_Click(object sender, EventArgs e)
        {
            try
            {
                string[] AbnormalNumber_ErrorcodeTOP5 = new string[] { "", "", "", "", "" };
                string[] AbnormalNumber_ErrorinfoTOP5 = new string[] { "", "", "", "", "" };
                double[] AbnormalNumber_ErrorCountTOP5 = new double[] { 0, 0, 0, 0, 0 };
                string[] AbnormalTime_ErrorcodeTOP5 = new string[] { "", "", "", "", "" };
                string[] AbnormalTime_ErrorinfoTOP5 = new string[] { "", "", "", "", "" };
                double[] AbnormalTime_ErrorTimeSpanTOP5 = new double[] { 0, 0, 0, 0, 0 };
                string Select = string.Format("select * from OEE_DT where cast(DateTime as datetime) >='{0}' and cast(EventTime as datetime) <='{1}' ", Convert.ToDateTime(dtp_Top5StartTime.Text).ToString("yyyy/MM/dd") + " 05:30:00", Convert.ToDateTime(dtp_Top5EndTime.Text).AddDays(1).ToString("yyyy/MM/dd") + " 05:30:00");
                DataTable dt = SQL.ExecuteQuery(Select);//1、查找选择当天5：30-隔天5：30所有OEE数据
                if (dt.Rows.Count > 0)
                {   //2、对原始数据进行排序修改
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
                    dt.Rows[0][1] = dtp_Top5StartTime.Text + " 05:30:00";          //把机台实际状态改变的时间替换为查找开始时间
                    dt.Rows[0][3] = (Convert.ToDateTime(dt.Rows[0][2].ToString()) - Convert.ToDateTime(dt.Rows[0][1].ToString())).TotalMinutes.ToString("0.00");//计算状态发生的时长
                    if (Convert.ToDateTime(Convert.ToDateTime(dtp_Top5EndTime.Text).AddDays(1).ToString("yyyy/MM/dd") + " 05:30:00") <= Convert.ToDateTime(dt.Rows[dt.Rows.Count - 1][2].ToString()))//判断结束时间是否大于database中的结束时间
                    {
                        dt.Rows[dt.Rows.Count - 1][2] = Convert.ToDateTime(dtp_Top5EndTime.Text).AddDays(1).ToString("yyyy/MM/dd") + " 05:30:00";//把机台实际状态结束的时间替换为查找结束时间
                        dt.Rows[dt.Rows.Count - 1][3] = (Convert.ToDateTime(dt.Rows[dt.Rows.Count - 1][2].ToString()) - Convert.ToDateTime(dt.Rows[dt.Rows.Count - 1][1].ToString())).TotalMinutes.ToString("0.00");//计算状态发生的时长
                    }
                    //3、修改后的数据插入数据库
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
                }
                //4、对数据库修改后的数据进行统计分析
                string SelectStr = "SELECT TOP 5 count(故障代码) as number,故障代码,故障信息 FROM Select_OEEDT " +
                string.Format("where cast(开始时间 as datetime) between '{0}' + ' 05:30:00' and '{1}' + ' 05:30:00' and 运行状态!= '2' ", Convert.ToDateTime(dtp_Top5StartTime.Text).ToString("yyyy/MM/dd"), Convert.ToDateTime(dtp_Top5EndTime.Text).AddDays(1).ToString("yyyy/MM/dd")) +
                "group by 故障代码 ,故障信息 order by COUNT(故障代码) DESC";
                DataTable d1 = SQL.ExecuteQuery(SelectStr);
                if (d1.Rows.Count > 0)
                {
                    for (int i = 0; i < d1.Rows.Count; i++)
                    {
                        AbnormalNumber_ErrorCountTOP5[i] = Convert.ToDouble(d1.Rows[i][0].ToString());
                        AbnormalNumber_ErrorcodeTOP5[i] = d1.Rows[i][1].ToString();
                        AbnormalNumber_ErrorinfoTOP5[i] = d1.Rows[i][2].ToString();
                    }
                }
                UpDatalabel(AbnormalNumber_ErrorcodeTOP5[0], "lb_errorcode_number_1");
                UpDatalabel(AbnormalNumber_ErrorcodeTOP5[1], "lb_errorcode_number_2");
                UpDatalabel(AbnormalNumber_ErrorcodeTOP5[2], "lb_errorcode_number_3");
                UpDatalabel(AbnormalNumber_ErrorcodeTOP5[3], "lb_errorcode_number_4");
                UpDatalabel(AbnormalNumber_ErrorcodeTOP5[4], "lb_errorcode_number_5");
                UpDatalabel(AbnormalNumber_ErrorinfoTOP5[0], "lb_errorinfo_number_1");
                UpDatalabel(AbnormalNumber_ErrorinfoTOP5[1], "lb_errorinfo_number_2");
                UpDatalabel(AbnormalNumber_ErrorinfoTOP5[2], "lb_errorinfo_number_3");
                UpDatalabel(AbnormalNumber_ErrorinfoTOP5[3], "lb_errorinfo_number_4");
                UpDatalabel(AbnormalNumber_ErrorinfoTOP5[4], "lb_errorinfo_number_5");
                RefreachData(chart_AbnormalNumber, AbnormalNumber_ErrorcodeTOP5, AbnormalNumber_ErrorCountTOP5, 0);

                string SelectStr2 = "SELECT TOP 5 sum((cast(分钟 as float))) as '时长',故障代码,故障信息 FROM Select_OEEDT " +
                    string.Format("where cast(开始时间 as datetime) between '{0}' + ' 05:30:00' and '{1}' + ' 05:30:00' and 运行状态!= '2' ", Convert.ToDateTime(dtp_Top5StartTime.Text).ToString("yyyy/MM/dd"), Convert.ToDateTime(dtp_Top5EndTime.Text).AddDays(1).ToString("yyyy/MM/dd")) +
                    "group by 故障代码,故障信息 order by sum((cast(分钟 as float))) desc";
                DataTable d2 = SQL.ExecuteQuery(SelectStr2);
                if (d2.Rows.Count > 0)
                {
                    for (int i = 0; i < d2.Rows.Count; i++)
                    {
                        AbnormalTime_ErrorTimeSpanTOP5[i] = Convert.ToDouble((Convert.ToDouble(d2.Rows[i][0].ToString()).ToString("0.00")));
                        AbnormalTime_ErrorcodeTOP5[i] = d2.Rows[i][1].ToString();
                        AbnormalTime_ErrorinfoTOP5[i] = d2.Rows[i][2].ToString();
                    }
                }

                UpDatalabel(AbnormalTime_ErrorcodeTOP5[0], "lb_errorcode_Time_1");
                UpDatalabel(AbnormalTime_ErrorcodeTOP5[1], "lb_errorcode_Time_2");
                UpDatalabel(AbnormalTime_ErrorcodeTOP5[2], "lb_errorcode_Time_3");
                UpDatalabel(AbnormalTime_ErrorcodeTOP5[3], "lb_errorcode_Time_4");
                UpDatalabel(AbnormalTime_ErrorcodeTOP5[4], "lb_errorcode_Time_5");
                UpDatalabel(AbnormalTime_ErrorinfoTOP5[0], "lb_errorinfo_Time_1");
                UpDatalabel(AbnormalTime_ErrorinfoTOP5[1], "lb_errorinfo_Time_2");
                UpDatalabel(AbnormalTime_ErrorinfoTOP5[2], "lb_errorinfo_Time_3");
                UpDatalabel(AbnormalTime_ErrorinfoTOP5[3], "lb_errorinfo_Time_4");
                UpDatalabel(AbnormalTime_ErrorinfoTOP5[4], "lb_errorinfo_Time_5");
                RefreachData(chart_AbnomalTime, AbnormalTime_ErrorcodeTOP5, AbnormalTime_ErrorTimeSpanTOP5, 0);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString().Replace("\r\n", ""));
            }
        }

        private void dtp_Top5EndTime_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Global.SelectTOP5Time = Convert.ToDateTime(dtp_Top5StartTime.Value);
            }
            catch
            { }
        }
    }
}
