using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Dynamic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using HslCommunication.Profinet.Inovance;
using HslCommunication;
using System.Net.Sockets;
using FlHelper;
using FlHelper.Models;
using FlHelper.Helpers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using OpcUaHelper;
using Opc.Ua;
using Opc.Ua.Client.Controls;

namespace 卓汇数据追溯系统
{
    public partial class MainFrm : Form
    {
        #region 声明
        [DllImport("kernel32.dll")]
        public static extern bool SetLocalTime(ref SYSTEMTIME st);
        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }

        Dictionary<string, Last_Station> lstn_info = new Dictionary<string, Last_Station>();//存储接口返回要在后续上传中使用的信息
        Dictionary<string, FromHG> hg_data = new Dictionary<string, FromHG>();///存储HG焊机数据
        Dictionary<string, Dictionary<string, heightModel>> heights = new Dictionary<string, Dictionary<string, heightModel>>();///存储测高数据
        public HomeFrm _homefrm;
        public ManualFrm _manualfrm;
        public SettingFrm _sttingfrm;
        public AbnormalFrm _Abnormalfrm;
        public UserLoginFrm _userloginfrm;
        public HelpFrm _helpfrm;
        public MachineFrm _machinefrm;
        public DataStatisticsFrm _datastatisticsfrm;
        public IOMonitorFrm _iomonitorfrm;
        string LogPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\bmp\\";
        Image[] img = new Image[16];
        delegate void DGVAutoSize(DataGridView dgv);
        delegate void ShowDataTable(DataGridView dgv, DataTable dt, int index);
        delegate void ShowPlcStatue(string txt, Color color, int id);
        private delegate void tssLabelcolor(ToolStripStatusLabel tsslbl, Color color, string bl);
        private delegate void btnEnable(ToolStripButton btn, bool b);
        private delegate void AddItemToListBoxDelegate(ListBox listbox, string str, int index);
        private delegate void ShowTxt(string txt, TextBox tb);
        private delegate void Labelcolor(Label lb, Color color, string bl);
        private delegate void Labelvision(Label lb, string bl);
        private delegate void buttonflag(bool flag, Button bt);
        private delegate void AddItemToRichTextBoxDelegate(string msg, RichTextBox richtextbox);

        private static object LockHans = new object();
        private static object Lock = new object();
        private static object Lock2 = new object();
        private static object LockUA = new object();
        private static object LockLA = new object();
        private static object Lock1 = new object();

        private static object lock_newstn = new object();
        private static object lock_check = new object();
        private static object lock_Trace = new object();
        private static object lock_pass = new object();

        string barCode1;//获取焊接参数SN码
        string barCode2;
        string barCode3;
        string barCode4;

        //short[] ReadStatus = new short[20];
        short OEEStatus = -1;//机台开机状态,后续为当前状态
        short OEEStatus_Now = -1;//实时读取状态
        string startTime = "";//状态开始时间
        string endTime = "";//状态结束时间
        short ReadStatus = -1;//机台大状态
        short ReadStatus_Error = -1;//宕机细节
        short ReadStatus_Pending = -1;//待料细节
        short ReadStatus_Stop = -1;//停止细节
        //short[] ReadTestRunStatus = new short[20];
        short ReadTestRunStatus = -1;
        short ReadOpenDoorStatus = -1;
        //bool bclose = true;
        bool ison = false;//PLC联机信号取反     
        bool ConnectPLC = false;
        //bool Link_PLC = true;
        string IP = string.Empty;
        string Mac = string.Empty;
        //double timenum = 5;
        //AsyncTcpClient client1;
        //AsyncTcpClient client2;
        short[] process = new short[1];//产品校验触发标志位
        DateTime heart_time = DateTime.Now;//心跳时间
        DateTime newStation_time = DateTime.Now;//报站时间
        //bool InsertSQLFlag = true;
        SQLServer SQL = new SQLServer();

        UAData UA_data = new UAData();
        //private TcpClient clent;
        double Product_Lianglv_08_09 = 0;
        double Product_Lianglv_09_10 = 0;
        double Product_Lianglv_10_11 = 0;
        double Product_Lianglv_11_12 = 0;
        double Product_Lianglv_12_13 = 0;
        double Product_Lianglv_13_14 = 0;
        double Product_Lianglv_14_15 = 0;
        double Product_Lianglv_15_16 = 0;
        double Product_Lianglv_16_17 = 0;
        double Product_Lianglv_17_18 = 0;
        double Product_Lianglv_18_19 = 0;
        double Product_Lianglv_19_20 = 0;
        double Product_Lianglv_20_21 = 0;
        double Product_Lianglv_21_22 = 0;
        double Product_Lianglv_22_23 = 0;
        double Product_Lianglv_23_00 = 0;
        double Product_Lianglv_00_01 = 0;
        double Product_Lianglv_01_02 = 0;
        double Product_Lianglv_02_03 = 0;
        double Product_Lianglv_03_04 = 0;
        double Product_Lianglv_04_05 = 0;
        double Product_Lianglv_05_06 = 0;
        double Product_Lianglv_06_07 = 0;
        double Product_Lianglv_07_08 = 0;
        double Product_Lianglv_08_20 = 0;
        double Product_Lianglv_20_08 = 0;
        #endregion

        #region 初始化
        public MainFrm()
        {
            InitializeComponent();
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            // 读取开机配置文件
            string dataPath = AppDomain.CurrentDomain.BaseDirectory + "setting.ini";
            if (File.Exists(dataPath))
            {
                Global.inidata = new IniProductFile(dataPath);
                Log.WriteLog("读取参数成功");
            }
            else
            {
                MessageBox.Show("配置文件不存在");
                Log.WriteLog("配置文件不存在");
                Environment.Exit(1);
            }

            //连接1个PLC
            try
            {
                Global.opcUaClient = new OpcUaHelper.OpcUaClient();

                Global.opcUaClient.ConnectServer("opc.tcp://192.168.250.1:4840");
                Global.opcUaClient.OpcStatusChange += M_OpcUaClient_OpcStatusChange1; ;

                Log.WriteLog("PLC连接成功");
                ShowStatus("已连接PLC", Color.DarkSeaGreen, 0);
                ConnectPLC = true;


                //Global.PLC_Write_Short[0] = 1;
                //bool result = Global.opcUaClient.WriteNode("ns=4;s=intMES_PLC", Global.PLC_Write_Short);

            }
            catch (Exception ex)
            {
                ShowStatus("与PLC连接断开", Color.Red, 0);
                ConnectPLC = false;
                Log.WriteLog(ex.ToString());
                MessageBox.Show("PLC通信无法连接" + ex.ToString());
            }


            // 开启客户端
            //try
            //{
            //    //Global.server1 = new AsyncTcpServer(IPAddress.Parse("192.192.192.5"), 8080);
            //    //Global.server1.ClientDisconnected += new EventHandler<TcpClientDisconnectedEventArgs>(server1_ServerDisconnected);
            //    //Global.server1.PlaintextReceived += new EventHandler<TcpDatagramReceivedEventArgs<string>>(server1_PlaintextReceived);
            //    //Global.server1.ClientConnected += new EventHandler<TcpClientConnectedEventArgs>(server1_ClientConnected);
            //    //Global.server1.Start(100);
            //    Global.client1 = new AsyncTcpClient(IPAddress.Parse("192.192.192.1"), 8080);
            //    Global.client1.PlaintextReceived += Client1_PlaintextReceived1;
            //    Global.client1.ServerConnected += Client1_ServerConnected;
            //    Global.client1.ServerDisconnected += Client1_ServerDisconnected;
            //    Global.client1.Connect();

            //    Global.client2 = new AsyncTcpClient(IPAddress.Parse("192.192.192.2"), 8080);
            //    Global.client2.PlaintextReceived += client2_PlaintextReceived1;
            //    Global.client2.ServerConnected += client2_ServerConnected;
            //    Global.client2.ServerDisconnected += client2_ServerDisconnected;
            //    Global.client2.Connect();

            //    //Global.client3 = new AsyncTcpClient(IPAddress.Parse("192.192.192.3"), 8080);
            //    //Global.client3.PlaintextReceived += client3_PlaintextReceived1;
            //    //Global.client3.ServerConnected += client3_ServerConnected;
            //    //Global.client3.ServerDisconnected += client3_ServerDisconnected;
            //    //Global.client3.Connect();

            //    //Global.client4 = new AsyncTcpClient(IPAddress.Parse("192.192.192.4"), 8080);
            //    //Global.client4.PlaintextReceived += client4_PlaintextReceived1;
            //    //Global.client4.ServerConnected += client4_ServerConnected;
            //    //Global.client4.ServerDisconnected += client4_ServerDisconnected;
            //    //Global.client4.Connect();
            //    //Log.WriteLog_ServerInfo("焊机已连接！");

            //    if (!Global.client1.Connected)
            //    {
            //        ShowStatus("1#焊机已断开", Color.Red, 4);
            //    }
            //    if (!Global.client2.Connected)
            //    {
            //        ShowStatus("2#焊机已断开", Color.Red, 5);
            //    }
            //    //if (!Global.client3.Connected)
            //    //{
            //    //    ShowStatus("3#焊机已断开", Color.Red, 6);
            //    //}
            //    //if (!Global.client4.Connected)
            //    //{
            //    //    ShowStatus("4#焊机已断开", Color.Red, 7);
            //    //}

            //}
            //catch (Exception ex)
            //{
            //    Log.WriteLog("服务器侦听开启失败！" + ex.ToString());
            //    MessageBox.Show("服务器侦听开启失败！请确认！" + ex.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}



            //导入报警信息表&PLC参数信息
            try
            {
                FileStream fs = new FileStream("报警目录.csv", FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                string lineData;
                while ((lineData = sr.ReadLine()) != null)
                {
                    ErrorData er = new ErrorData();
                    int ed_key = Convert.ToInt32(lineData.Split(',')[0]);
                    er.errorCode = lineData.Split(',')[1];//载入OEE异常代码
                    er.errorinfo = lineData.Split(',')[2];//载入OEE异常信息
                    er.errorStatus = lineData.Split(',')[3];//载入OEE异常状态
                    er.ModuleCode = lineData.Split(',')[4].Replace("|", "");//载入OEE异常模组代码
                    er.Moduleinfo = lineData.Split(',')[5];//载入OEE异常模组状态
                    Global.ed.Add(ed_key, er);
                    //Global.ED.Add(ed_key, er);
                }
                sr.Close();
                fs.Close();
                Log.WriteLog("导入报警信息表成功");

            }
            catch (Exception ex)
            {
                MessageBox.Show("导入报警信息表失败！" + ex.ToString().Replace("\n", ""));
                Log.WriteLog("导入报警信息表失败！" + ex.ToString().Replace("\n", ""));
                Environment.Exit(1);
            }

            //读取抛料项
            string dataPath3 = System.AppDomain.CurrentDomain.BaseDirectory + "Tossing.xml";
            if (File.Exists(dataPath3))
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.IgnoreComments = true;//忽略文档里面的注释
                System.Xml.XmlReader reader = System.Xml.XmlReader.Create(dataPath3, settings);
                xmlDoc.Load(reader);
                // 得到根节点bookstore
                System.Xml.XmlNode xn = xmlDoc.SelectSingleNode("Tossing");
                // 得到根节点的所有子节点
                System.Xml.XmlNodeList xnl = xn.ChildNodes;
                int index;
                foreach (System.Xml.XmlNode xn1 in xnl)
                {
                    Tossing tossing = new Tossing();
                    // 将节点转换为元素，便于得到节点的属性值
                    System.Xml.XmlElement xe = (System.Xml.XmlElement)xn1;
                    // 得到Type和ISBN两个属性的属性值
                    tossing.Type = xe.GetAttribute("Type").ToString();
                    // 得到节点的所有子节点
                    System.Xml.XmlNodeList xnl0 = xe.ChildNodes;
                    index = Convert.ToInt16(xnl0.Item(0).InnerText);
                    tossing.Chinese_Value = xnl0.Item(1).InnerText;
                    tossing.English_Value = xnl0.Item(2).InnerText;
                    tossing.Code = xnl0.Item(3).InnerText;
                    Global.Tossing_Item.Add(index, tossing);
                }
                Log.WriteLog("读取抛料细项成功");
                reader.Close();
            }
            else
            {
                MessageBox.Show("抛料配置文件不存在");
                Log.WriteLog("抛料配置文件不存在");
                Environment.Exit(1);
            }


            //读取已配置数据并显示
            Global.Operator_pwd = Global.inidata.productconfig.Operator_pwd;
            Global.Technician_pwd = Global.inidata.productconfig.Technician_pwd;
            Global.Administrator_pwd = Global.inidata.productconfig.Administrator_pwd;
            Global.Login = Global.LoginLevel.Operator;
            
            Global.billNo = Global.inidata.productconfig.billNo;
            Global.equipmentNo = Global.inidata.productconfig.equipmentNo;
            Global.station = Global.inidata.productconfig.station;
            Global.Trace_ip = Global.inidata.productconfig.Trace_ip;
            Global.version = Global.inidata.productconfig.version;
            Global.Trace_line_id = Global.inidata.productconfig.Trace_line_id;
            Global.Trace_station_id = Global.inidata.productconfig.Trace_station_id;
            Global.CT = Convert.ToDouble(Global.inidata.productconfig.CT);


            //MDI父窗体
            _homefrm = new HomeFrm(this);
            _manualfrm = new ManualFrm(this);
            _sttingfrm = new SettingFrm(this);
            _Abnormalfrm = new AbnormalFrm(this);
            _userloginfrm = new UserLoginFrm(this);
            _helpfrm = new HelpFrm(this);
            _machinefrm = new MachineFrm(this);
            _datastatisticsfrm = new DataStatisticsFrm(this);
            _iomonitorfrm = new IOMonitorFrm(this);
            ShowView();
            Sc = new SendSfc();
            Sc.UpdateUI += _homefrm.AppendRichText;

            //进行报站获取参数，防止重启软件后后续工位直接进行上传
            //if (!ini_newStation())
            //{
            //    Log.WriteLog("开机报站失败");
            //}

            #region 开机记录软件关闭的结束时间
            //try
            //{
            //    string start_time = string.Empty;//查找最后一笔状态的开始时间
            //    DateTime t2 = DateTime.Now;
            //    Global.start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            //    string sel_Endtime = "select EventTime from OEE_DT where ID=(select MAX(ID) from OEE_DT)";
            //    DataTable d_end = SQL.ExecuteQuery(sel_Endtime);
            //    if (d_end.Rows.Count > 0)
            //    {
            //        start_time = d_end.Rows[0][0].ToString();
            //    }
            //    DateTime t1 = Convert.ToDateTime(start_time);
            //    string ts = (t2 - t1).TotalMinutes.ToString("0.00");

            //    string up_oee = $"update OEE_DT set DateTime='{t2.ToString("yyyy-MM-dd HH:mm:ss.fff")}',ErrorCode='10010001',ModuleCode='',RunStatus=6,ErrorInfo='软件关闭',TimeSpan='{ts}' where ID=(select MAX(ID) from OEE_DT)";
            //    SQL.ExecuteUpdate(up_oee);

            //    _homefrm.AppendRichText(start_time + " -> " + t2.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + "软件关闭" + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");

            //    //Global.OEE_Code = Global.plc1.ReadInt16("MW62001").Content;
            //    _homefrm.AppendRichText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.OEE_Code].errorCode + "," + Global.ed[Global.OEE_Code].errorinfo, "Rtxt_OEE_Detail");

            //}
            //catch (Exception ex)
            //{
            //    Log.WriteLog(ex.ToString().Replace("\r\n", ""));
            //}
            #endregion

            Worker_thread();
            this.WindowState = FormWindowState.Maximized;//初始化窗体最大化
        }


        /// <summary>
        /// OPC 客户端的状态变化后的消息提醒
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void M_OpcUaClient_OpcStatusChange1(object sender, OpcUaStatusEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    M_OpcUaClient_OpcStatusChange1(sender, e);
                }));
                return;
            }

            if (e.Text.Contains("Disconnected"))
            {
                ConnectPLC = false;
                ShowStatus("与PLC连接断开", Color.Red, 0);
            }
            else
            {
                ShowStatus("已连接PLC", Color.DarkSeaGreen, 0);
            }
        }


        private void server1_PlaintextReceived(object sender, TcpDatagramReceivedEventArgs<string> e)
        {
            Log.WriteLog_ServerInfo(JsonConvert.SerializeObject(e));
        }

        private void server1_ClientConnected(object sender, TcpClientConnectedEventArgs e)
        {

        }

        private void server1_ServerDisconnected(object sender, TcpClientDisconnectedEventArgs e)
        {

        }

        private void client4_PlaintextReceived1(object sender, TcpDatagramReceivedEventArgs<string> e)
        {
            try
            {
                if (e.Datagram.ToString().Contains("swing_amplitude"))
                {
                    Log.WriteLog_ServerInfo("接收到4#焊机数据：" + e.Datagram.ToString());
                    string dt = DateTime.Now.ToString("yyyy:MM:dd:HH:mm:ss");
                    FromHG HG_Data = JsonConvert.DeserializeObject<FromHG>(e.Datagram.Replace("\r\n", ""));//对华工数据进行反序列化
                    if (!hg_data.ContainsKey(barCode4))
                    {
                        hg_data.Add(barCode4, HG_Data);
                    }
                    //string sql = "insert into HGData values('" + dt + "','" + barCode4 + "','" + HG_Data.results[0].test + "','" + HG_Data.results[0].value + "','" + HG_Data.results[1].value + "','" + HG_Data.results[2].value + "','" + HG_Data.results[3].value + "','" + HG_Data.results[4].value + "','" + HG_Data.results[5].value + "','" + HG_Data.results[6].value + "','" + HG_Data.results[7].value + "','" + HG_Data.results[8].value + "','" + HG_Data.results[9].value + "','" + HG_Data.swing_amplitude + "','" + HG_Data.swing_freq + "','" + HG_Data.pattern_type + "','" + HG_Data.PrecitecValue + "','" + HG_Data.PrecitecProcessRev + "','" + HG_Data.PrecitecGrading + "','" + HG_Data.PrecitecResult + "') ";
                    //for (int i = 0; i < 16; i++)
                    //{
                    //    sql += "insert into HGData (DateTime,barcode,test,PowerDiode,PowerFiber,frequency,waveform,laser_speed,jump_speed,jump_delay,position_delay,pulse_profile,pulse_profileFiber) values('" + dt + "','" + barCode4 + "','" + HG_Data.results[10 + i * 10].test + "','" + HG_Data.results[10 + i * 10].value + "','" + HG_Data.results[11 + i * 10].value + "','" + HG_Data.results[12 + i * 10].value + "','" + HG_Data.results[13 + i * 10].value + "','" + HG_Data.results[14 + i * 10].value + "','" + HG_Data.results[15 + i * 10].value + "','" + HG_Data.results[16 + i * 10].value + "','" + HG_Data.results[17 + i * 10].value + "','" + HG_Data.results[18 + i * 10].value + "','" + HG_Data.results[19 + i * 10].value + "')";
                    //}

                    //SQL.ExecuteUpdate(sql);

                    var hg = new Dictionary<string, heightModel>();
                    var model = new heightModel();
                    for (int i = 0; i < HG_Data.heights.Length; i++)
                    {

                        switch (i % 3)
                        {
                            case 0:
                                model.TestHeightZ = HG_Data.heights[i].value;
                                break;
                            case 1:
                                model.Difference = HG_Data.heights[i].value;
                                break;
                            case 2:
                                model.ReferenceV = HG_Data.heights[i].value;
                                if (!hg.ContainsKey(HG_Data.heights[i].test))
                                {
                                    hg.Add(HG_Data.heights[i].test, model);
                                    Log.WriteCSV(DateTime.Now + "," + HG_Data.Barcode + "," + "4," + HG_Data.heights[i].test + "," + model.TestHeightZ + "," + model.Difference + "," + model.ReferenceV, "E:\\public\\测高记录\\");
                                }
                                else
                                {
                                    Log.WriteLog_ServerInfo("4#:" + HG_Data.heights[i].test + "," + HG_Data.Barcode);
                                }
                                model = new heightModel();
                                break;
                        }

                    }
                    heights.Add(barCode4, hg);               
                }
                else if (e.Datagram.ToString().Contains("OK"))
                {
                    Log.WriteLog_ServerInfo("焊机已经收到中板码");
                    //Global.plc1.Write("MW64302", Convert.ToInt16(1));

                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("华工客户端交互数据抛出异常。" + ex.ToString());
            }
        }



        private void client3_PlaintextReceived1(object sender, TcpDatagramReceivedEventArgs<string> e)
        {
            try
            {
                if (e.Datagram.ToString().Contains("swing_amplitude"))
                {
                    Log.WriteLog_ServerInfo("接收到3#焊机数据：" + e.Datagram.ToString());
                    string dt = DateTime.Now.ToString("yyyy:MM:dd:HH:mm:ss");
                    FromHG HG_Data = JsonConvert.DeserializeObject<FromHG>(e.Datagram.Replace("\r\n", ""));//对华工数据进行反序列化
                    if (!hg_data.ContainsKey(barCode3))
                    {
                        hg_data.Add(barCode3, HG_Data);
                    }

                    //string sql = "insert into HGData values('" + dt + "','" + barCode3 + "','" + HG_Data.results[0].test + "','" + HG_Data.results[0].value + "','" + HG_Data.results[1].value + "','" + HG_Data.results[2].value + "','" + HG_Data.results[3].value + "','" + HG_Data.results[4].value + "','" + HG_Data.results[5].value + "','" + HG_Data.results[6].value + "','" + HG_Data.results[7].value + "','" + HG_Data.results[8].value + "','" + HG_Data.results[9].value + "','" + HG_Data.swing_amplitude + "','" + HG_Data.swing_freq + "','" + HG_Data.pattern_type + "','" + HG_Data.PrecitecValue + "','" + HG_Data.PrecitecProcessRev + "','" + HG_Data.PrecitecGrading + "','" + HG_Data.PrecitecResult + "') ";
                    //for (int i = 0; i < 16; i++)
                    //{
                    //    sql += "insert into HGData (DateTime,barcode,test,PowerDiode,PowerFiber,frequency,waveform,laser_speed,jump_speed,jump_delay,position_delay,pulse_profile,pulse_profileFiber) values('" + dt + "','" + barCode3 + "','" + HG_Data.results[10 + i * 10].test + "','" + HG_Data.results[10 + i * 10].value + "','" + HG_Data.results[11 + i * 10].value + "','" + HG_Data.results[12 + i * 10].value + "','" + HG_Data.results[13 + i * 10].value + "','" + HG_Data.results[14 + i * 10].value + "','" + HG_Data.results[15 + i * 10].value + "','" + HG_Data.results[16 + i * 10].value + "','" + HG_Data.results[17 + i * 10].value + "','" + HG_Data.results[18 + i * 10].value + "','" + HG_Data.results[19 + i * 10].value + "')";
                    //}
                    //SQL.ExecuteUpdate(sql);
                    var hg = new Dictionary<string, heightModel>();
                    var model = new heightModel();
                    for (int i = 0; i < HG_Data.heights.Length; i++)
                    {

                        switch (i % 3)
                        {
                            case 0:
                                model.TestHeightZ = HG_Data.heights[i].value;
                                break;
                            case 1:
                                model.Difference = HG_Data.heights[i].value;
                                break;
                            case 2:
                                model.ReferenceV = HG_Data.heights[i].value;
                                if (!hg.ContainsKey(HG_Data.heights[i].test))
                                {
                                    hg.Add(HG_Data.heights[i].test, model);
                                    Log.WriteCSV(DateTime.Now + "," + HG_Data.Barcode + "," + "3," + HG_Data.heights[i].test + "," + model.TestHeightZ + "," + model.Difference + "," + model.ReferenceV, "E:\\public\\测高记录\\");
                                }
                                else
                                {
                                    Log.WriteLog_ServerInfo("3#:" + HG_Data.heights[i].test + "," + HG_Data.Barcode);
                                }
                                model = new heightModel();
                                break;
                        }

                    }
                    heights.Add(barCode3, hg);
                }
                else if (e.Datagram.ToString().Contains("OK"))
                {
                    Log.WriteLog_ServerInfo("焊机已经收到中板码");
                    //Global.plc1.Write("MW64102", Convert.ToInt16(1));
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("华工客户端交互数据抛出异常。" + ex.ToString());
            }
        }



        private void client2_PlaintextReceived1(object sender, TcpDatagramReceivedEventArgs<string> e)
        {
            try
            {
                if (e.Datagram.ToString().Contains("swing_amplitude"))
                {
                    Log.WriteLog_ServerInfo("接收到2#焊机数据：" + e.Datagram.ToString());
                    string dt = DateTime.Now.ToString("yyyy:MM:dd:HH:mm:ss");
                    FromHG HG_Data = JsonConvert.DeserializeObject<FromHG>(e.Datagram.Replace("\r\n", ""));//对华工数据进行反序列化
                    if (!hg_data.ContainsKey(barCode2))
                    {
                        hg_data.Add(barCode2, HG_Data);
                    }
                    //string sql = "insert into HGData values('" + dt + "','" + barCode2 + "','" + HG_Data.results[0].test + "','" + HG_Data.results[0].value + "','" + HG_Data.results[1].value + "','" + HG_Data.results[2].value + "','" + HG_Data.results[3].value + "','" + HG_Data.results[4].value + "','" + HG_Data.results[5].value + "','" + HG_Data.results[6].value + "','" + HG_Data.results[7].value + "','" + HG_Data.results[8].value + "','" + HG_Data.results[9].value + "','" + HG_Data.swing_amplitude + "','" + HG_Data.swing_freq + "','" + HG_Data.pattern_type + "','" + HG_Data.PrecitecValue + "','" + HG_Data.PrecitecProcessRev + "','" + HG_Data.PrecitecGrading + "','" + HG_Data.PrecitecResult + "') ";
                    //for (int i = 0; i < 16; i++)
                    //{
                    //    sql += "insert into HGData (DateTime,barcode,test,PowerDiode,PowerFiber,frequency,waveform,laser_speed,jump_speed,jump_delay,position_delay,pulse_profile,pulse_profileFiber) values('" + dt + "','" + barCode2 + "','" + HG_Data.results[10 + i * 10].test + "','" + HG_Data.results[10 + i * 10].value + "','" + HG_Data.results[11 + i * 10].value + "','" + HG_Data.results[12 + i * 10].value + "','" + HG_Data.results[13 + i * 10].value + "','" + HG_Data.results[14 + i * 10].value + "','" + HG_Data.results[15 + i * 10].value + "','" + HG_Data.results[16 + i * 10].value + "','" + HG_Data.results[17 + i * 10].value + "','" + HG_Data.results[18 + i * 10].value + "','" + HG_Data.results[19 + i * 10].value + "')";
                    //}
                    //SQL.ExecuteUpdate(sql);
                    var hg = new Dictionary<string, heightModel>();
                    var model = new heightModel();
                    for (int i = 0; i < HG_Data.heights.Length; i++)
                    {

                        switch (i % 3)
                        {
                            case 0:
                                model.TestHeightZ = HG_Data.heights[i].value;
                                break;
                            case 1:
                                model.Difference = HG_Data.heights[i].value;
                                break;
                            case 2:
                                model.ReferenceV = HG_Data.heights[i].value;
                                if (!hg.ContainsKey(HG_Data.heights[i].test))
                                {
                                    hg.Add(HG_Data.heights[i].test, model);
                                    Log.WriteCSV(DateTime.Now + "," + HG_Data.Barcode + "," + "2," + HG_Data.heights[i].test + "," + model.TestHeightZ + "," + model.Difference + "," + model.ReferenceV, "E:\\public\\测高记录\\");
                                }
                                else
                                {
                                    Log.WriteLog_ServerInfo("2#:" + HG_Data.heights[i].test + "," + HG_Data.Barcode);
                                }
                                model = new heightModel();
                                break;
                        }

                    }
                    heights.Add(barCode2, hg);
                }
                else if (e.Datagram.ToString().Contains("OK"))
                {
                    Log.WriteLog_ServerInfo("焊机已经收到中板码");
                    //Global.plc1.Write("MW63302", Convert.ToInt16(1));
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("华工客户端交互数据抛出异常。" + ex.ToString());
            }
        }

        private SendSfc Sc { set; get; }
        private void Client1_PlaintextReceived1(object sender, TcpDatagramReceivedEventArgs<string> e)
        {
            try
            {
                if (e.Datagram.ToString().Contains("swing_amplitude"))
                {
                    Log.WriteLog_ServerInfo("接收到1#焊机数据：" + e.Datagram.ToString());
                    string dt = DateTime.Now.ToString("yyyy:MM:dd:HH:mm:ss");
                    FromHG HG_Data = JsonConvert.DeserializeObject<FromHG>(e.Datagram.Replace("\r\n", ""));//对华工数据进行反序列化
                    if (!hg_data.ContainsKey(barCode1))
                    {
                        hg_data.Add(barCode1, HG_Data);
                    }
                    //string sql = "insert into HGData values('" + dt + "','" + barCode1 + "','" + HG_Data.results[0].test + "','" + HG_Data.results[0].value + "','" + HG_Data.results[1].value + "','" + HG_Data.results[2].value + "','" + HG_Data.results[3].value + "','" + HG_Data.results[4].value + "','" + HG_Data.results[5].value + "','" + HG_Data.results[6].value + "','" + HG_Data.results[7].value + "','" + HG_Data.results[8].value + "','" + HG_Data.results[9].value + "','" + HG_Data.swing_amplitude + "','" + HG_Data.swing_freq + "','" + HG_Data.pattern_type + "','" + HG_Data.PrecitecValue + "','" + HG_Data.PrecitecProcessRev + "','" + HG_Data.PrecitecGrading + "','" + HG_Data.PrecitecResult + "') ";
                    //for (int i = 0; i < 16; i++)
                    //{
                    //    sql += "insert into HGData (DateTime,barcode,test,PowerDiode,PowerFiber,frequency,waveform,laser_speed,jump_speed,jump_delay,position_delay,pulse_profile,pulse_profileFiber) values('" + dt + "','" + barCode1 + "','" + HG_Data.results[10 + i * 10].test + "','" + HG_Data.results[10 + i * 10].value + "','" + HG_Data.results[11 + i * 10].value + "','" + HG_Data.results[12 + i * 10].value + "','" + HG_Data.results[13 + i * 10].value + "','" + HG_Data.results[14 + i * 10].value + "','" + HG_Data.results[15 + i * 10].value + "','" + HG_Data.results[16 + i * 10].value + "','" + HG_Data.results[17 + i * 10].value + "','" + HG_Data.results[18 + i * 10].value + "','" + HG_Data.results[19 + i * 10].value + "')";
                    //}
                    //SQL.ExecuteUpdate(sql);
                    var hg = new Dictionary<string, heightModel>();
                    var model = new heightModel();
                    for (int i = 0; i < HG_Data.heights.Length; i++)
                    {

                        switch (i % 3)
                        {
                            case 0:
                                model.TestHeightZ = HG_Data.heights[i].value;
                                break;
                            case 1:
                                model.Difference = HG_Data.heights[i].value;
                                break;
                            case 2:
                                model.ReferenceV = HG_Data.heights[i].value;
                                if (!hg.ContainsKey(HG_Data.heights[i].test))
                                {
                                    hg.Add(HG_Data.heights[i].test, model);
                                    Log.WriteCSV(DateTime.Now + "," + HG_Data.Barcode + "," + "1," + HG_Data.heights[i].test + "," + model.TestHeightZ + "," + model.Difference + "," + model.ReferenceV, "E:\\public\\测高记录\\");
                                }
                                else
                                {
                                    Log.WriteLog_ServerInfo("1#:" + HG_Data.heights[i].test + "," + HG_Data.Barcode);
                                }
                                model = new heightModel();
                                break;
                        }

                    }
                    heights.Add(barCode1, hg);                  
                }
                else if (e.Datagram.ToString().Contains("OK"))
                {
                    Log.WriteLog_ServerInfo("焊机已经收到中板码");
                    //Global.plc1.Write("MW63102", Convert.ToInt16(1));

                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("华工客户端交互数据抛出异常。" + ex.ToString());
            }
        }

        /// <summary>
        /// 图片上传确认
        /// </summary>
        /// 

        private void UploadPic(object obj)
        {
            while (true)
            {
                try
                {
                    List<PicModel> pic = PicList.ToList();
                    foreach (var item in pic)
                    {
                        bool result = PutTupian(item.HansId, item.sn, item.fixture);
                        if (result)
                        {
                            PicList.Remove(item);
                            Log.WriteLog_ServerInfo(item.HansId + "#图片" + item.sn + "重传完成");
                        }
                        else
                        {
                            item.Count++;
                        }
                        if (item.Count > 3)
                        {
                            PicList.Remove(item);
                            Log.WriteLog_ServerInfo(item.HansId + "#图片" + item.sn + "失败重传" + item.Count + "次");
                        }
                        Thread.Sleep(20000);

                    }
                    if (DateTime.Now.Minute == 30 && DateTime.Now.Second == 0)
                    {
                        DeletePic(1);
                        DeletePic(2);
                        DeletePic(3);
                        DeletePic(4);

                        ClearOverdueFile cof1;
                        ClearOverdueFile cof2;
                        ClearOverdueFile cof3;
                        ClearOverdueFile cof4;
                        ClearOverdueFile cof5;
                        ClearOverdueFile cof6;
                        ClearOverdueFile cof7;
                        cof1 = new ClearOverdueFile("D:\\ZHH\\ThreeMaRecord", 14);     
                        cof1.FileDeal();
                        Thread.Sleep(500);
                        cof2 = new ClearOverdueFile("D:\\ZHH\\PassMaRecord", 14);    
                        cof2.FileDeal();
                        Thread.Sleep(500);
                        cof3 = new ClearOverdueFile("D:\\ZHH\\PassModelList记录", 14);    
                        cof3.FileDeal();
                        Thread.Sleep(500);
                        cof4 = new ClearOverdueFile("E:\\public\\测高记录", 14);    
                        cof4.FileDeal();
                        Thread.Sleep(500);
                        cof5 = new ClearOverdueFile("D:\\ZHH\\操作记录", 14);    
                        cof5.FileDeal();
                        Thread.Sleep(500);
                        cof6 = new ClearOverdueFile("D:\\ZHH\\HansInfo", 14);   
                        cof6.FileDeal();
                        Thread.Sleep(500);
                        cof7 = new ClearOverdueFile("D:\\ZHH\\ServerInfo", 14);   
                        cof7.FileDeal();
                    }

                }
                catch (Exception ex)
                {

                    Log.WriteLog_ServerInfo("图片上抛异常:" + ex.Message);
                }
                Thread.Sleep(1000);
            }
        }
        /// <summary>
        /// 定时清除历史图片
        /// </summary>
        private void DeletePic(int HansId, int OutDays = 14)
        {
            try
            {
                string path = "E:\\public\\HG" + HansId + "#";
                Sc.DeleteAll(path);
            }
            catch (Exception ex)
            {

                Log.WriteLog_ServerInfo("图片清除异常:" + ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="OutDays"></param>

        private List<PicModel> PicList = new List<PicModel>();
        private bool PutTupian(int HansId, string sn, string fixture)
        {
            try
            {
                string path = "E:\\public\\HG" + HansId + "#";
                string a = path + "\\" + DateTime.Now.ToString("yyyyMMdd") + sn;
                string b = a + ".zip";
                List<PostDateClass> pic = new List<PostDateClass>() {
                    //lstn_info[sn].Lstn_requestId
                        new PostDateClass() {Prop="requestId",Type=0,Value=lstn_info[sn].Lstn_requestId, },
                        new PostDateClass(){Prop="productName",Type=0,Value=Global.inidata.productconfig.productName, },
                          new PostDateClass() {Prop="barCode",Type=0,Value=sn, },
                        new PostDateClass(){Prop="station",Type=0,Value=Global.inidata.productconfig.station, },
                          new PostDateClass() {Prop="fixture",Type=0,Value=fixture, },
                        new PostDateClass(){Prop="equipmentNo",Type=0,Value=Global.inidata.productconfig.equipmentNo, },
                          new PostDateClass() {Prop="equipmentIp",Type=0,Value=Global.inidata.productconfig.Trace_ip, },
                        new PostDateClass(){Prop="judgement",Type=0,Value="PASS", },
                         new PostDateClass() {Prop="createTime",Type=0,Value=DateTime.Now.ToString("yyyyMMddHHmmss"), },
                        new PostDateClass(){Prop="file",Type=1,Value=b, },
                    };

                if (Directory.Exists(a))
                {
                    ZipHelper.ZipDirectory(a, b);
                    string str = Sc.PostFileMessage(Global.inidata.productconfig.TupianUrl, pic);
                    Log.WriteLog_ServerInfo(sn + "图片上抛:" + str);
                    if (str.Contains("OK"))
                    {
                        if (lstn_info.ContainsKey(sn))
                        {
                            lstn_info.Remove(sn);
                        }
                        Sc.UpdateUI(sn + "图片上抛OK！", "Rtxt_Rec_Pass");
                        if (File.Exists(b))
                        {
                            File.Delete(b);
                            Log.WriteCSV(DateTime.Now + "," + sn + "," + "" + "," + "" + "," + "" + "," + "完成," + str, "D:\\ZHH\\PassModelList记录\\");
                        }
                        return true;
                    }
                    else
                    {
                        Sc.UpdateUI(sn + "图片上抛:" + str, "Rtxt_Rec_Pass");

                        return false;
                    }
                }
                else
                {
                    Log.WriteLog_ServerInfo(path + "图片" + sn + "文件夹不存在！");
                    return false;
                }

            }
            catch (Exception ex)
            {

                Log.WriteLog_ServerInfo(ex.Message + ":图片" + HansId + "#上抛异常！" + sn);
                return false;
            }


        }

      

        private void Client1_ServerDisconnected(object sender, TcpServerDisconnectedEventArgs e)
        {
            Log.WriteLog_ServerInfo("1#焊机客户端已断开");
            ShowStatus("1#焊机已断开", Color.Red, 4);
            //尝试重新连接

            Log.WriteLog_ServerInfo("1#正在重新连接……");

            Global.client1.Connect();


            Thread.Sleep(1000);
        }

        private void Client1_ServerConnected(object sender, TcpServerConnectedEventArgs e)
        {
            Log.WriteLog_ServerInfo("1#焊机已连接");
            ShowStatus("1#焊机已连接", Color.DarkSeaGreen, 4);
        }
        private void client2_ServerDisconnected(object sender, TcpServerDisconnectedEventArgs e)
        {
            Log.WriteLog_ServerInfo("2#焊机客户端已断开");
            ShowStatus("2#焊机已断开", Color.Red, 5);
            //尝试重新连接

            Log.WriteLog_ServerInfo("2#正在重新连接……");

            Global.client2.Connect();


            Thread.Sleep(1000);
        }

        private void client2_ServerConnected(object sender, TcpServerConnectedEventArgs e)
        {
            Log.WriteLog_ServerInfo("2#焊机已连接");
            ShowStatus("2#焊机已连接", Color.DarkSeaGreen, 5);
        }

        private void client3_ServerDisconnected(object sender, TcpServerDisconnectedEventArgs e)
        {
            Log.WriteLog_ServerInfo("3#焊机客户端已断开");
            ShowStatus("3#焊机已断开", Color.Red, 6);
            //尝试重新连接

            Log.WriteLog_ServerInfo("3#正在重新连接……");
            Global.client3.Connect();
            Thread.Sleep(1000);
        }

        private void client3_ServerConnected(object sender, TcpServerConnectedEventArgs e)
        {
            Log.WriteLog_ServerInfo("3#焊机已连接");
            ShowStatus("3#焊机已连接", Color.DarkSeaGreen, 6);
        }

        private void client4_ServerDisconnected(object sender, TcpServerDisconnectedEventArgs e)
        {
            Log.WriteLog_ServerInfo("4#焊机客户端已断开");
            ShowStatus("4#焊机已断开", Color.Red, 7);
            //尝试重新连接

            Log.WriteLog_ServerInfo("4#正在重新连接……");

            Global.client4.Connect();


            Thread.Sleep(1000);
        }

        private void client4_ServerConnected(object sender, TcpServerConnectedEventArgs e)
        {
            Log.WriteLog_ServerInfo("4#焊机已连接");
            ShowStatus("4#焊机已连接", Color.DarkSeaGreen, 7);
        }
        #endregion

        #region 开启工作线程
        private void Worker_thread()
        {
            //OPC通信读写
            Task task1 = Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        if (ConnectPLC)
                        {
                            string[] nodes = new string[]
                            {
                               "ns=4;s=intPLC_MES",
                               "ns=4;s=dintPLC_MES",
                               "ns=4;s=stringPLC_MES"
                            };

                            IEnumerable<DataValue> data = Global.opcUaClient.ReadNodes(nodes);
                            // 因为不能保证读取的节点类型一致，所以只提供统一的DataValue读取，每个节点需要单独解析
                            foreach (Opc.Ua.DataValue value in data)
                            {
                                if (value.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.Int32)
                                {
                                    int[] temp = (int[])value.WrappedValue.Value;           // 最终值
                                    Global.PLC_Read_Int = temp;
                                }
                                if (value.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.Int16)
                                {
                                    short[] temp = (short[])value.WrappedValue.Value;           // 最终值
                                    Global.PLC_Read_Short = temp;
                                }
                                else if (value.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.String)
                                {
                                    string[] temp = (string[])value.WrappedValue.Value;
                                    Global.PLC_Read_String = temp;
                                }
                                else if (value.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.Float)
                                {
                                    float[] temp = (float[])value.WrappedValue.Value;
                                }
                            }
                            bool result = Global.opcUaClient.WriteNode("ns=4;s=intMES_PLC", Global.PLC_Write_Short);

                            //short[] s = Global.opcUaClient.ReadNode<short[]>("ns=4;s=intMES_PLC");
                            if (!result)
                            {
                                Log.WriteLog("写入PLC失败");
                            }
                        }                        
                    }
                    catch (Exception ex)
                    {
                        ConnectPLC = false;
                    }
                    Thread.Sleep(100);
                }
            });

            //实时判断读地址触发
            Task task2 = Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        short[] plc_read = Global.PLC_Read_Short;
                        if ((DateTime.Now - heart_time).TotalSeconds >= 1)//心跳
                        {
                            Global.PLC_Write_Short[20] = Convert.ToInt16(Global.PLC_Write_Short[20] == 0 ? 1 : 0);
                            heart_time = DateTime.Now;
                        }
                        for (int i = 0; i < plc_read.Length ; i++)
                        {
                            if (plc_read[i] == 1 && Global.PLC_Read_Short_Before[i] == 0)//上升沿触发
                            {
                                Task t = new Task(() =>
                                {
                                    PLC_Event(i);
                                });
                                t.Start();//PLC触发进入不同线程
                            }
                            else if (plc_read[i] == 0 && Global.PLC_Read_Short_Before[i] == 1)//下降沿触发
                            {
                                Global.PLC_Write_Short[i] = 0;//完成信号清零
                                Global.PLC_Write_Short[10 + i] = 0;//结果清零
                            }
                        }
                        Global.PLC_Read_Short_Before = plc_read;//当前状态赋值

                    }
                    catch (Exception ex)
                    {
                        
                    }
                    Thread.Sleep(10);
                }
            });

            //定时报站,获取HashCode
            Task task3 = Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        if ((DateTime.Now - newStation_time).TotalHours >= 1)//正常两小时报站一次，防止未实时获取到Hashcode实际一小时一次
                        {
                            newStation(Global.PLC_Read_String[0]);
                            newStation_time = DateTime.Now;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Thread.Sleep(5000);
                }
            });

            //Thread DownTime = new Thread(OEE_DT);//记录OEE DOWN TIME
            //DownTime.IsBackground = true;
            //DownTime.Start();

            //Thread plcData = new Thread(EthPolling_Data);//PLC 产能统计
            //plcData.IsBackground = true;
            //plcData.Start();


            //ThreadPool.QueueUserWorkItem(On_Time_doing);//按时做某事
            ////ThreadPool.QueueUserWorkItem(PLC_autolink);//PLC自动重连
            ////ThreadPool.QueueUserWorkItem(Ping_ip);//检测PLC与Macmini是否连接
            ThreadPool.QueueUserWorkItem(CheckConnected);//连接状态判断

            ////ThreadPool.QueueUserWorkItem(AutoStopBreak);//机台状态改变时自动结束吃饭休息
            //ThreadPool.QueueUserWorkItem(UploadPic);//图片重传检测机制

            //ThreadPool.QueueUserWorkItem(retry_TracePass);//Trace，过站缓存重传
            //ThreadPool.QueueUserWorkItem(ClearMemory);//定时回收内存
            Thread.Sleep(10);
        }
        #endregion

        #region 连接状态判断
        private void CheckConnected(object ob)
        {
            while (true)
            {
                try
                {
                    //if (!Global.client1.Connected)
                    //{
                    //    Global.client1.Close();
                    //    Global.client1 = new AsyncTcpClient(IPAddress.Parse("192.192.192.1"), 8080);
                    //    Global.client1.PlaintextReceived += Client1_PlaintextReceived1;
                    //    Global.client1.ServerConnected += Client1_ServerConnected;
                    //    Global.client1.ServerDisconnected += Client1_ServerDisconnected;
                    //    Global.client1.Connect();
                    //}
                    //if (!Global.client2.Connected)
                    //{
                    //    Global.client2.Close();
                    //    Global.client2 = new AsyncTcpClient(IPAddress.Parse("192.192.192.2"), 8080);
                    //    Global.client2.PlaintextReceived += client2_PlaintextReceived1;
                    //    Global.client2.ServerConnected += client2_ServerConnected;
                    //    Global.client2.ServerDisconnected += client2_ServerDisconnected;
                    //    Global.client2.Connect();
                    //}
                    //if (!Global.client3.Connected)
                    //{
                    //    Global.client3.Close();
                    //    Global.client3 = new AsyncTcpClient(IPAddress.Parse("192.192.192.3"), 8080);
                    //    Global.client3.PlaintextReceived += client3_PlaintextReceived1;
                    //    Global.client3.ServerConnected += client3_ServerConnected;
                    //    Global.client3.ServerDisconnected += client3_ServerDisconnected;
                    //    Global.client3.Connect();
                    //}
                    //if (!Global.client4.Connected)
                    //{
                    //    Global.client4.Close();
                    //    Global.client4 = new AsyncTcpClient(IPAddress.Parse("192.192.192.4"), 8080);
                    //    Global.client4.PlaintextReceived += client4_PlaintextReceived1;
                    //    Global.client4.ServerConnected += client4_ServerConnected;
                    //    Global.client4.ServerDisconnected += client4_ServerDisconnected;
                    //    Global.client4.Connect();
                    //}
                    if (!ConnectPLC)
                    {
                        Global.opcUaClient.ConnectServer("opc.tcp://192.168.250.1:4840");
                        Log.WriteLog("PLC连接成功");
                        ShowStatus("已连接PLC", Color.DarkSeaGreen, 0);
                        ConnectPLC = true;
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString());
                }
                Thread.Sleep(1000);
            }
        }


        #endregion

        #region PLC交互处理  报站->上料->过站->Trace上传

        /// <summary>
        /// PLC触发事件
        /// </summary>
        /// <param name="index">code_no</param>
        /// <returns></returns>
        private void PLC_Event(int index)
        {
            switch (index)
            {
                case 0:
                    PLC_Check(index);
                    break;
                case 1:
                    Pass_Station(index);
                    break;

            }
        }


        private void GetHansData(object info)
        {
            SendHans sendhans = info as SendHans;
            //string barcode = (string)info;
            try
            {
                switch (sendhans.HansId)
                {
                    case 1:
                        if (Global.client1.Connected)
                        {
                            //发送中板码给焊机
                            Global.client1.Send(sendhans.barcode);
                            Log.WriteLog_ServerInfo("1#焊前发送中板码：" + sendhans.barcode + "给焊机");
                        }
                        else
                        {
                            ShowStatus("1#焊机已断开", Color.Red, 4);
                            Log.WriteLog_ServerInfo("1#焊前发送中板码" + sendhans.barcode + "给焊机失败，请检查焊机连接状态！");
                        }
                        break;
                    case 2:
                        if (Global.client2.Connected)
                        {
                            //发送中板码给焊机
                            Global.client2.Send(sendhans.barcode);
                            Log.WriteLog_ServerInfo("2#焊前发送中板码：" + sendhans.barcode + "给焊机");
                        }
                        else
                        {
                            ShowStatus("2#焊机已断开", Color.Red, 5);
                            Log.WriteLog_ServerInfo("2#焊前发送中板码" + sendhans.barcode + "给焊机失败，请检查焊机连接状态！");
                        }
                        break;
                    //case 3:
                    //    if (Global.client3.Connected)
                    //    {
                    //        //发送中板码给焊机
                    //        Global.client3.Send(sendhans.barcode);
                    //        Log.WriteLog_ServerInfo("3#焊前发送中板码：" + sendhans.barcode + "给焊机");
                    //    }
                    //    else
                    //    {
                    //        ShowStatus("3#焊机已断开", Color.Red, 6);
                    //        Log.WriteLog_ServerInfo("3#焊前发送中板码" + sendhans.barcode + "给焊机失败，请检查焊机连接状态！");
                    //    }
                    //    break;
                    //case 4:
                    //    if (Global.client4.Connected)
                    //    {
                    //        //发送中板码给焊机
                    //        Global.client4.Send(sendhans.barcode);
                    //        Log.WriteLog_ServerInfo("4#焊前发送中板码：" + sendhans.barcode + "给焊机");
                    //    }
                    //    else
                    //    {
                    //        ShowStatus("4#焊机已断开", Color.Red, 7);
                    //        Log.WriteLog_ServerInfo("4#焊前发送中板码" + sendhans.barcode + "给焊机失败，请检查焊机连接状态！");
                    //    }
                    //    break;
                    default:

                        break;
                }

            }
            catch (Exception ex)
            {
                //ShowStatus("焊机已断开", Color.Red, 4);
                Log.WriteLog_ServerInfo("焊前发送中板码" + sendhans.barcode + "给焊机失败，请检查：" + ex.Message);
            }
        }


        /// <summary>
        /// config验证
        /// </summary>
        /// <param name="SN">code_no</param>
        /// <returns></returns>
        private bool config_check(string SN)
        {
            try
            {
                string callResult = "";
                string errMsg = "";

                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值

                string SFC_url = "http://10.176.152.159:8081/NewSFCV2-center/NewSFCV2/getcodeinfo";

                SFCData sfc_data = new SFCData();
                sfc_data.code_no = SN;
                sfc_data.equipment_ip = Global.Trace_ip;
                sfc_data.resv1 = "";
                sfc_data.resv2 = "";

                string send_SFC = JsonConvert.SerializeObject(sfc_data, Formatting.None, jsetting);
                Log.WriteLog("上传SFC系统：" + send_SFC);
                _homefrm.AppendRichText(send_SFC, "");

                RequestAPI3.CallBobcat(SFC_url, send_SFC, out callResult, out errMsg);
                Log.WriteLog("config信息接收:" + callResult);
                _homefrm.AppendRichText(callResult, "");

                JObject recvObj = JsonConvert.DeserializeObject<JObject>(callResult);

                //config验证，目前待测。（进行判断）
                if (recvObj["config"].ToString() == "")
                {
                    _homefrm.AppendRichText("config验证成功", "");
                    return true;
                }
                else
                {
                    Log.WriteLog("config验证失败" + recvObj["config"].ToString());
                    _homefrm.AppendRichText("config验证失败", "");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("config验证异常" + ex.ToString());
                _homefrm.AppendRichText("config验证异常", "");
                return false;
            }

        }

        /// <summary>
        /// 报站
        /// </summary>
        /// <param name="SN">产品码</param>
        /// <returns></returns>
        private bool newStation(string SN)
        {
            lock (lock_newstn)
            {
                try
                {
                    string callResult = "";
                    string errMsg = "";

                    JsonSerializerSettings jsetting = new JsonSerializerSettings();
                    jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值

                    //string newStn_url = "http://10.176.152.159:28080/NewSFCV2-enterequ-bcs01/NewSFCV2/bcs-enterequ";
                    //string newStn_url = "http://10.197.246.63:8080/NewSFCV2/v2/enterequ";
                    string newStn_url = Global.inidata.productconfig.BaozhanUrl;
                    NewStation new_Stn = new NewStation();
                    new_Stn.equipmentNo = Global.equipmentNo;
                    new_Stn.station = Global.station;
                    new_Stn.billNo = "";
                    new_Stn.barCode = SN;
                    new_Stn.barCodeType = Global.barCodeType;
                    new_Stn.resv1 = "";
                    new_Stn.resv2 = "";

                    string send_newStn = JsonConvert.SerializeObject(new_Stn, Formatting.None, jsetting);
                    Log.WriteLog_ServerInfo("A工位 Equipment to API 設備報站:" + send_newStn);
                    _homefrm.AppendRichText(send_newStn, "Rtxt_Send_newStn");

                    RequestAPI3.CallBobcat(newStn_url, send_newStn, out callResult, out errMsg);
                    Log.WriteLog_ServerInfo("A工位 API to Equipment 設備報站:" + callResult);
                    _homefrm.AppendRichText(callResult, "Rtxt_Rec_newStn");

                    JObject recvObj = JsonConvert.DeserializeObject<JObject>(callResult);


                    string curr_time = string.Empty;
                    curr_time = recvObj["sysDate"].ToString();//当前时间

                    SYSTEMTIME sysTime = new SYSTEMTIME();
                    sysTime.wYear = Convert.ToUInt16(curr_time.Substring(0, 4)); //must be short           
                    sysTime.wMonth = Convert.ToUInt16(curr_time.Substring(4, 2));
                    sysTime.wDay = Convert.ToUInt16(curr_time.Substring(6, 2));
                    sysTime.wHour = Convert.ToUInt16(curr_time.Substring(8, 2));
                    sysTime.wMinute = Convert.ToUInt16(curr_time.Substring(10, 2));
                    sysTime.wSecond = Convert.ToUInt16(curr_time.Substring(12, 2));

                    bool flag_time = SetLocalTime(ref sysTime);
                    if (flag_time)
                    {
                        Log.WriteLog("与系统时间同步成功");
                    }
                    if (recvObj["rc"].ToString() == "000")
                    {
                        //采集数据供上料检验，过站使用

                        Global.hashCode = recvObj["hashCode"].ToString();
                        Global.billNo = recvObj["billNo"].ToString();

                        Global.checkURL = recvObj["checkURL"].ToString();
                        Global.passURL = recvObj["passURL"].ToString();

                        _homefrm.AppendRichText("报站OK!", "Rtxt_Rec_newStn");
                        _homefrm.UpDatelabelcolor(Color.DarkSeaGreen, "报站OK！", "lb_报站");
                        return true;
                    }
                    else
                    {
                        Log.WriteLog("报站信息异常：" + recvObj["rm"].ToString());
                        _homefrm.AppendRichText("报站NG!", "Rtxt_Rec_newStn");
                        _homefrm.UpDatelabelcolor(Color.Red, "报站NG！", "lb_报站");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLog("报站接口异常" + ex.ToString());
                    _homefrm.AppendRichText("报站接口异常", "Rtxt_Rec_newStn");
                    _homefrm.UpDatelabelcolor(Color.Red, "报站接口异常！", "lb_报站");
                    return false;
                }
            }
        }

        /// <summary>
        /// 开机报站
        /// </summary>
        /// <param name="equipmentNo">设备编码</param>
        /// <returns></returns>
        private bool ini_newStation()
        {
            lock (lock_newstn)
            {
                try
                {
                    string callResult = "";
                    string errMsg = "";

                    JsonSerializerSettings jsetting = new JsonSerializerSettings();
                    jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值

                    //string newStn_url = "http://10.176.152.159:28080/NewSFCV2-enterequ-bcs01/NewSFCV2/bcs-enterequ";
                    //string newStn_url = "http://10.197.246.63:8080/NewSFCV2/v2/enterequ";
                    string newStn_url = Global.inidata.productconfig.BaozhanUrl;
                    NewStation new_Stn = new NewStation();
                    new_Stn.equipmentNo = Global.equipmentNo;
                    new_Stn.station = Global.station;
                    new_Stn.billNo = Global.billNo;
                    new_Stn.barCode = "";
                    new_Stn.barCodeType = Global.barCodeType;
                    new_Stn.resv1 = "";
                    new_Stn.resv2 = "";

                    string send_newStn = JsonConvert.SerializeObject(new_Stn, Formatting.None, jsetting);
                    Log.WriteLog_ServerInfo("A工位 Equipment to API 設備報站:" + send_newStn);
                    _homefrm.AppendRichText(send_newStn, "Rtxt_Send_newStn");

                    RequestAPI3.CallBobcat(newStn_url, send_newStn, out callResult, out errMsg);
                    Log.WriteLog_ServerInfo("A工位 API to Equipment 設備報站:" + callResult);
                    _homefrm.AppendRichText(callResult, "Rtxt_Rec_newStn");

                    JObject recvObj = JsonConvert.DeserializeObject<JObject>(callResult);

                    string curr_time = string.Empty;
                    curr_time = recvObj["sysDate"].ToString();//当前时间

                    SYSTEMTIME sysTime = new SYSTEMTIME();
                    sysTime.wYear = Convert.ToUInt16(curr_time.Substring(0, 4)); //must be short           
                    sysTime.wMonth = Convert.ToUInt16(curr_time.Substring(4, 2));
                    sysTime.wDay = Convert.ToUInt16(curr_time.Substring(6, 2));
                    sysTime.wHour = Convert.ToUInt16(curr_time.Substring(8, 2));
                    sysTime.wMinute = Convert.ToUInt16(curr_time.Substring(10, 2));
                    sysTime.wSecond = Convert.ToUInt16(curr_time.Substring(12, 2));

                    bool flag_time = SetLocalTime(ref sysTime);
                    if (flag_time)
                    {
                        Log.WriteLog("与系统时间同步成功");
                    }
                    if (recvObj["rc"].ToString() == "000")
                    {
                        //采集数据供上料检验，过站使用

                        Global.hashCode = recvObj["hashCode"].ToString();
                        Global.billNo = recvObj["billNo"].ToString();

                        Global.checkURL = recvObj["checkURL"].ToString();
                        Global.passURL = recvObj["passURL"].ToString();
                        return true;
                    }
                    else
                    {
                        Log.WriteLog("报站信息异常：" + recvObj["rm"].ToString());
                        _homefrm.AppendRichText("报站NG!", "Rtxt_Rec_newStn");
                        _homefrm.UpDatelabelcolor(Color.Red, "报站NG！", "lb_报站");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLog("报站接口异常" + ex.ToString());
                    _homefrm.AppendRichText("报站接口异常", "Rtxt_Rec_newStn");
                    _homefrm.UpDatelabelcolor(Color.Red, "报站接口异常！", "lb_报站");
                    return false;
                }
            }
        }

        public string fixture { set; get; }
     
        //public string barcode { set; get; }
        public string left_rail { set; get; }
        public string right_rail { set; get; }

        /// <summary>
        /// PLC焊机1上料确认线程
        /// </summary>
        private void PLC_Check(int index)
        {
            try
            {
                //上料检验
                string barcode = Global.PLC_Read_String[0];
                string left_rail = Global.PLC_Read_String[0];
                string right_rail = Global.PLC_Read_String[0];
                string fixture = Global.PLC_Read_String[0];

                //i++;
                //fixture = i % 2 == 0 ? "H-D2BA-EML20-0300-A1-00004" : "H-D1BA-SML17-2000-A3-10001";
                //发送中板码给焊机
                GetHansData(new SendHans() { HansId = 1, barcode = barcode });

                if (loading_check(barcode, left_rail, right_rail, fixture))//上料检验OK
                {
                    Global.PLC_Write_Short[index] = 1;//结果
                    Global.PLC_Write_Short[10 + index] = 1;//完成信号
                }
                else
                {
                    Global.PLC_Write_Short[index] = 2;//结果
                    Global.PLC_Write_Short[10 + index] = 1;//完成信号
                }
            }
            catch (Exception ex)
            {
                Global.PLC_Write_Short[index] = 2;//结果
                Global.PLC_Write_Short[10 + index] = 1;//完成信号
                Log.WriteLog("PC与PLC通讯异常：" + ex.ToString());
            }
        }

        /// <summary>
        /// 上料检验
        /// </summary>
        /// <param name="SN">产品码</param>
        /// <param name="left_rail">左键码</param>
        /// <param name="right_rail">右键码</param>
        /// <param name="fixture">治具码</param>
        /// <returns></returns>
        private bool loading_check(string SN, string left_rail, string right_rail, string fixture)
        {
            lock (lock_check)
            {
                try
                {
                    _homefrm.AppendRichText("产品码：" + SN, "Rtxt_Send_Check");
                    _homefrm.AppendRichText("左键码：" + left_rail, "Rtxt_Send_Check");
                    _homefrm.AppendRichText("右键码：" + right_rail, "Rtxt_Send_Check");
                    _homefrm.AppendRichText("治具码：" + fixture, "Rtxt_Send_Check");

                    string callResult = "";
                    string errMsg = "";

                    JsonSerializerSettings jsetting = new JsonSerializerSettings();
                    jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值

                    #region 富士康V1.0--P2
                    LoadingCheck check_load = new LoadingCheck();

                    check_load.hashCode = Global.hashCode;
                    check_load.billNo = Global.billNo;
                    check_load.barCodeType = Global.barCodeType;
                    check_load.barCode = SN;
                    check_load.equipmentNo = Global.equipmentNo;
                    check_load.station = Global.station;
                    check_load.startTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    check_load.bindCode = new BindCode[4];
                    check_load.bindCode[0] = new BindCode();
                    check_load.bindCode[1] = new BindCode();
                    check_load.bindCode[2] = new BindCode();
                    check_load.bindCode[3] = new BindCode();

                    check_load.bindCode[0].codeSn = left_rail;
                    check_load.bindCode[0].codeSnType = "IQC_LEFT_RAIL";
                    check_load.bindCode[0].replace = "1";
                    check_load.bindCode[1].codeSn = right_rail;
                    check_load.bindCode[1].codeSnType = "IQC_RIGHT_RAIL";
                    check_load.bindCode[1].replace = "1";
                    check_load.bindCode[2].codeSn = fixture;
                    check_load.bindCode[2].codeSnType = "FIXTURE";
                    check_load.bindCode[2].replace = "1";
                    check_load.bindCode[3].codeSn = Global.equipmentNo; //"MO0093730001-10";
                    check_load.bindCode[3].codeSnType = "EQUIPMENT";
                    check_load.bindCode[3].replace = "1";

                    check_load.resv1 = "";
                    check_load.resv2 = "";
                    #endregion

                    #region 富士康V1.1--EVT暂未用0215

                    //Loading_Affirm check_load = new Loading_Affirm();

                    //check_load.hashcode = Global.hashCode;
                    //check_load.request_id = DateTime.Now.ToString("yyyyMMdd-HHmmss.fff") + "-000000000000000" + Global.equipmentNo;
                    //check_load.code_no = SN;
                    //check_load.code_type = Global.barCodeType;
                    //check_load.bill_no = Global.billNo;
                    //check_load.wrkst_no = Global.station;
                    //check_load.trans_no = "deal";
                    //check_load.wo_no = "工单";//参数待确定
                    //check_load.equipment_code = Global.equipmentNo;
                    //check_load.equipment_ip = Global.Trace_ip;
                    //check_load.start_time = DateTime.Now.AddSeconds(-1).ToString("yyyyMMddHHmmss");
                    //check_load.end_time = DateTime.Now.ToString("yyyyMMddHHmmss");
                    //check_load.scan_user = "作业人"; //参数待确定

                    //check_load.bind_info = new Bind_Info[4];
                    //check_load.bind_info[0] = new Bind_Info();
                    //check_load.bind_info[1] = new Bind_Info();
                    //check_load.bind_info[2] = new Bind_Info();
                    //check_load.bind_info[3] = new Bind_Info();
                    //check_load.bind_info[0].bind_code = left_rail;
                    //check_load.bind_info[0].bind_codetype = "IQC_LEFT_RAIL";
                    //check_load.bind_info[0].is_replace = "1";
                    //check_load.bind_info[1].bind_code = right_rail;
                    //check_load.bind_info[1].bind_codetype = "IQC_RIGHT_RAIL";
                    //check_load.bind_info[1].is_replace = "1";
                    //check_load.bind_info[2].bind_code = fixture;
                    //check_load.bind_info[2].bind_codetype = "FIXTURE";
                    //check_load.bind_info[2].is_replace = "1";
                    //check_load.bind_info[3].bind_code = Global.equipmentNo;
                    //check_load.bind_info[3].bind_codetype = "EQUIPMENT";
                    //check_load.bind_info[3].is_replace = "1";

                    //check_load.others_info = new Others_Info[0];

                    #endregion


                    string send_check = JsonConvert.SerializeObject(check_load, Formatting.None, jsetting);
                    Log.WriteLog_ServerInfo("A工位 Equipment to API 上料確認:" + send_check);
                    _homefrm.AppendRichText(send_check, "Rtxt_Send_Check");

                    RequestAPI3.CallBobcat2(Global.checkURL, send_check, Global.hashCode, out callResult, out errMsg);
                    Log.WriteLog_ServerInfo("A工位 API to Equipment 上料確認:" + callResult);
                    _homefrm.AppendRichText(callResult, "Rtxt_Rec_Check");

                    JObject recvObj = JsonConvert.DeserializeObject<JObject>(callResult);



                    if (recvObj["rc"].ToString() == "000")
                    {
                        //Global.TraceURL = recvObj["url"].ToString();
                        Global.Trace_line_id = recvObj["lineName"].ToString();
                        Global.Trace_station_id = recvObj["stationId"].ToString();
                        if (lstn_info.ContainsKey(SN))
                        {
                            lstn_info[SN].Lstn_requestId = recvObj["requestId"].ToString();
                        }
                        else
                        {
                            Last_Station lst_Stn = new Last_Station();
                            lst_Stn.Lstn_requestId = recvObj["requestId"].ToString();

                            lstn_info.Add(SN, lst_Stn);
                        }

                        _homefrm.AppendRichText("上料确认OK!", "Rtxt_Rec_Check");
                        _homefrm.UpDatelabelcolor(Color.DarkSeaGreen, "上料确认OK!", "lb_上料确认");
                        return true;
                    }
                    else
                    {
                        _homefrm.AppendRichText("上料确认NG!", "Rtxt_Rec_Check");
                        Log.WriteLog_ServerInfo("上料确认异常信息：" + recvObj["rm"].ToString());
                        _homefrm.UpDatelabelcolor(Color.Red, "上料确认NG!", "lb_上料确认");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    _homefrm.AppendRichText("上料确认接口异常", "Rtxt_Rec_Check");
                    Log.WriteLog_ServerInfo("上料确认接口异常" + ex.ToString());
                    _homefrm.UpDatelabelcolor(Color.Red, "上料确认接口异常!", "lb_上料确认");
                    return false;
                }
            }
        }

        /// <summary>
        /// 焊机焊接后过站上传和Trace上传
        /// </summary>
        private void Pass_Station(int index)
        {
            try
            {
                //2#PLC过站信息上传
                string barcode = Global.PLC_Read_String[0];
                barCode1 = barcode;
                // 焊后给焊机触发，请求数据
                if (Global.client1.Connected)
                {
                    Global.client1.Send("D1");
                    Log.WriteLog_ServerInfo("Barcode:" + barcode + "1#焊后触发焊机，请求数据。");
                }
                string left_rail = Global.PLC_Read_String[0];
                string right_rail = Global.PLC_Read_String[0];
                string fixture = Global.PLC_Read_String[0];
                //i++;
                //fixture = i % 2 == 0 ? "H-D1BA-SML17-2000-A3-10001" : "H-D2BA-EML20-0300-A1-00004";
                string cavity = Global.PLC_Read_String[0];

                var ResultModel = pass_Trace(barcode, left_rail, right_rail, fixture, "1");
                if (ResultModel.Result)
                {
                    Sc.UpdateUI("过站产品码：" + barcode, "Rtxt_Send_Pass");
                    Sc.UpdateUI("过站左键码：" + left_rail, "Rtxt_Send_Pass");
                    Sc.UpdateUI("过站右键码：" + right_rail, "Rtxt_Send_Pass");
                    Sc.UpdateUI("过站治具码：" + fixture, "Rtxt_Send_Pass");
                    Log.WriteCSV_HansInfo(barcode + "," + left_rail + "," + right_rail + "," + fixture + "," + "1" + ",0" + cavity);
                    if (pass_station(barcode, left_rail, right_rail, fixture, cavity, ResultModel.Model))
                    {
                        Global.PLC_Write_Short[index] = 1;//结果
                        Global.PLC_Write_Short[10 + index] = 1;//完成信号
                        Global.b_Trace = true;
                        //string inStr2 = string.Format("insert into HansInfo values('{0}','1')", barcode);
                        //string inStr2 = string.Format("insert into Hans_Info values('{0}','{1}','{2}','1')", DateTime.Now.ToString("yyyy-MM-dd HH:mm"), barcode, fixture);
                        //SQL.ExecuteUpdate(inStr2);
                        #region SFC
                        //        if (Sc.PassStation("http://10.176.46.59:8081/LCH/proto", new SfcStn()
                        //        {
                        //            hashCode = Global.hashCode,

                        //            requestId = lstn_info[barcode].Lstn_requestId,
                        //            resv1 = "",

                        //            productInfo = new FlHelper.Models.ProductInfo()
                        //            {
                        //                barCode = barcode,
                        //                barCodeType = Global.barCodeType,
                        //                clientCode = lstn_info[barcode].Lstn_requestId,
                        //                station = Global.inidata.productconfig.station,
                        //                billNo = Global.billNo,
                        //                product = Global.inidata.productconfig.productName,
                        //                phase = "EVT",
                        //                config = "Config999",
                        //                lineName = Global.inidata.productconfig.Trace_line_id,
                        //                stationId = Global.inidata.productconfig.Trace_station_id,
                        //                stationString = "",
                        //                bindCode = new FlHelper.Models.BindCode[] {
                        //                new FlHelper.Models.BindCode() {codeSn=left_rail,codeSnType="IQC_LEFT_RAIL",replace="1" },
                        //                 new FlHelper.Models.BindCode() {codeSn=right_rail,codeSnType="IQC_RIGHT_RAIL",replace="1" },
                        //                  new FlHelper.Models.BindCode() {codeSn=fixture,codeSnType="FIXTURE",replace="1" },
                        //                   new FlHelper.Models.BindCode() {codeSn=Global.inidata.productconfig.equipmentNo,codeSnType="EQUIPMENT",replace="1" },

                        //},
                        //            },
                        //            equipmentInfo = new FlHelper.Models.EquipmentInfo()
                        //            {
                        //                equipmentIp = Global.inidata.productconfig.Trace_ip,
                        //                equipmentType = "L",
                        //                equipmentNo = Global.inidata.productconfig.equipmentNo,
                        //                vendorId = "HG",
                        //                processRevs = Global.inidata.productconfig.version,
                        //                softwareName = "HG数据追溯系统",
                        //                softwareVersion = "Ver2.5.0.5",
                        //                patternName = "HG准直激光控制系统",
                        //                patternVersion = "V1.2.10-20221111",
                        //                ccdVersion = "9.5SR2",
                        //                ccdGuideVersion = "9.5SR2",
                        //                ccdInspectionName = "CCD_Moniton",
                        //                ccdInspectionVersion = "9.5SR2",
                        //            },
                        //            recipeInfo = new FlHelper.Models.RecipeInfo()
                        //            {
                        //                startTime = DateTime.Now.AddSeconds(-25).ToString("yyyyMMddHHmmss"),
                        //                endTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        //                cavity = "0" + cavity,
                        //                judgement = "PASS",
                        //                humidity = "63",
                        //                temperature = "35",
                        //                processStartTime = DateTime.Now.AddSeconds(-25).ToString("yyyyMMddHHmmss"),
                        //                processEndTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        //                waitCt = "",
                        //                ct = "",
                        //                tossingItem = "",
                        //                errorInfo = new ErrorInfo[] { },
                        //                paraInfo = ResultModel.Model,
                        //            },

                        //        }, Global.hashCode))
                        {
                            if (Global.inidata.productconfig.TupianOpen == "1")
                            {
                                if (!PutTupian(1, barcode, fixture)) { PicList.Add(new PicModel() { HansId = 1, sn = barcode, fixture = fixture }); };
                            }
                            else
                            {
                                lstn_info.Remove(barcode);
                            }
                        };
                        hg_data.Remove(barcode);
                        #endregion
                    }
                    else//Trace上传成功，过站失败。
                    {
                        Global.PLC_Write_Short[index] = 2;//结果
                        Global.PLC_Write_Short[10 + index] = 1;//完成信号
                        Global.b_Trace = false;
                        //插入Trace和过站的数据
                        //string inStr2 = "insert into retry_Pass values('{barcode}','{left_rail}','{right_rail}','{fixture}','1','{lstn_info[barcode].Lstn_requestId}')";
                        string inStr2 = string.Format("insert into retry_Pass values('{0}','{1}','{2}','{3}','1','{4}')", barcode, left_rail, right_rail, fixture, lstn_info[barcode].Lstn_requestId);
                        SQL.ExecuteUpdate(inStr2);
                    }

                }
                else//Trace上传失败，还没过站。
                {
                    Global.PLC_Write_Short[index] = 2;//结果
                    Global.PLC_Write_Short[10 + index] = 1;//完成信号
                    Global.b_Trace = false;
                    //插入Trace和过站的数据
                    //string inStr2 = "insert into retry_Pass values('{barcode}','{left_rail}','{right_rail}','{fixture}','1','{lstn_info[barcode].Lstn_requestId}')";
                    string inStr2 = string.Format("insert into retry_Pass values('{0}','{1}','{2}','{3}','1','{4}')", barcode, left_rail, right_rail, fixture, lstn_info[barcode].Lstn_requestId);
                    SQL.ExecuteUpdate(inStr2);

                }
            }
            catch (Exception ex)
            {
                Global.PLC_Write_Short[index] = 2;//结果
                Global.PLC_Write_Short[10 + index] = 1;//完成信号
                Log.WriteLog("焊接1过站上传异常：" + ex.ToString());
                Global.b_Trace = false;
            }

        }


        /// <summary>
        /// Trace上传
        /// </summary>
        /// <param name="SN">产品码</param>
        /// <param name="left_rail">左键码</param>
        /// <param name="right_rail">右键码</param>
        /// <param name="fixture">治具码</param>
        /// /// <param name="head_id">焊接序号</param>
        private ResultModel<FlHelper.Models.ParaInfo[]> pass_Trace(string SN, string left_rail, string right_rail, string fixture, string head_id)
        {
            lock (lock_Trace)
            {
                if (!lstn_info.ContainsKey(SN) || string.IsNullOrEmpty(lstn_info[SN].Lstn_requestId))
                {
                    if (!loading_check(SN, left_rail, right_rail, fixture))//上料检验OK
                    {
                        Log.WriteLog("过站上传时进行上料检验失败");
                    }
                }

                var paraInfos = new FlHelper.Models.ParaInfo[17];
                try
                {
                    _homefrm.AppendRichText("产品码：" + SN, "Rtxt_Send_Trace");
                    _homefrm.AppendRichText("左键码：" + left_rail, "Rtxt_Send_Trace");
                    _homefrm.AppendRichText("右键码：" + right_rail, "Rtxt_Send_Trace");
                    _homefrm.AppendRichText("治具码：" + fixture, "Rtxt_Send_Trace");

                    string callResult = "";
                    string errMsg = "";



                    JsonSerializerSettings jsetting = new JsonSerializerSettings();
                    jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值

                    //Set_Laser();//给焊接参数赋值

                    TraceMesRequest trace_ua = new TraceMesRequest();


                    trace_ua.serial_type = "rm";
                    trace_ua.serials = new string[3];
                    trace_ua.serials[0] = SN;
                    trace_ua.serials[1] = left_rail;
                    trace_ua.serials[2] = right_rail;


                    trace_ua.data = new data();
                    trace_ua.data.insight = new Insight();
                    trace_ua.data.insight.test_attributes = new Test_attributes();
                    trace_ua.data.insight.test_station_attributes = new Test_station_attributes();
                    trace_ua.data.insight.uut_attributes = new Uut_attributes();
                    //trace_ua.data.insight.results = new Result[170];

                    int index = 0;
                    while (!hg_data.ContainsKey(SN))
                    {
                        Thread.Sleep(1500);
                        index++;
                        if (index > 5)
                        {
                            Log.WriteLog_ServerInfo(SN + "," + head_id + "#焊接参数未获取到。");
                            return new ResultModel<FlHelper.Models.ParaInfo[]>() { Result = false, Model = paraInfos };
                        }
                    }

                    trace_ua.data.insight.results = new Result[hg_data[SN].results.Length + hg_data[SN].heights.Length];
                    paraInfos = new FlHelper.Models.ParaInfo[hg_data[SN].results.Length / 10];
                    for (int i = 0; i < hg_data[SN].results.Length; i++)
                    {
                        trace_ua.data.insight.results[i] = new Result();
                        trace_ua.data.insight.results[i].test = hg_data[SN].results[i].test;
                        trace_ua.data.insight.results[i].result = hg_data[SN].results[i].result;
                        trace_ua.data.insight.results[i].units = hg_data[SN].results[i].units;
                        trace_ua.data.insight.results[i].sub_test = hg_data[SN].results[i].sub_test;
                        trace_ua.data.insight.results[i].value = hg_data[SN].results[i].value;
                    }
                    for (int i = 0; i < hg_data[SN].heights.Length; i++)
                    {
                        trace_ua.data.insight.results[i + hg_data[SN].results.Length] = new Result();
                        trace_ua.data.insight.results[i + hg_data[SN].results.Length].test = hg_data[SN].heights[i].test;
                        trace_ua.data.insight.results[i + hg_data[SN].results.Length].result = "pass";
                        trace_ua.data.insight.results[i + hg_data[SN].results.Length].units = hg_data[SN].heights[i].units;
                        trace_ua.data.insight.results[i + hg_data[SN].results.Length].sub_test = hg_data[SN].heights[i].sub_test;
                        trace_ua.data.insight.results[i + hg_data[SN].results.Length].value = hg_data[SN].heights[i].value;
                    }

                    //string sql = "select * from HGData where barcode='" + SN + "'";
                    //DataTable dt = SQL.ExecuteQuery(sql);
                    //int index = 0;
                    //while (dt.Rows.Count < 17)
                    //{
                    //    Thread.Sleep(1500);
                    //    dt = SQL.ExecuteQuery(sql);
                    //    index++;
                    //    if (index > 5)
                    //    {
                    //        Log.WriteLog_ServerInfo(SN + "," + head_id + "#焊接数据不足，现有" + dt.Rows.Count + "条数据");
                    //        return new ResultModel<FlHelper.Models.ParaInfo[]>() { Result = false, Model = paraInfos };
                    //    }
                    //    //
                    //}

                    //for (int i = 0; i < trace_ua.data.insight.results.Length; i++)
                    //{
                    //    trace_ua.data.insight.results[i] = new Result();

                    //    trace_ua.data.insight.results[i].result = "pass";

                    //    #region new
                    //    //
                    //    //i除以10的结果，相当于用来区分每个焊点
                    //    switch (i / 10)
                    //    {

                    //        //第一个焊点
                    //        case 0:
                    //            trace_ua.data.insight.results[i].test = "RM2";

                    //            break; ;
                    //        //第二个焊点
                    //        case 1:
                    //            trace_ua.data.insight.results[i].test = "RM3";

                    //            break;
                    //        case 2:
                    //            trace_ua.data.insight.results[i].test = "LM1";

                    //            break;
                    //        case 3:
                    //            trace_ua.data.insight.results[i].test = "LT";

                    //            break;
                    //        case 4:
                    //            trace_ua.data.insight.results[i].test = "F-LT";

                    //            break;
                    //        case 5:
                    //            trace_ua.data.insight.results[i].test = "LM2";

                    //            break;
                    //        case 6:
                    //            trace_ua.data.insight.results[i].test = "LM3";

                    //            break;
                    //        case 7:
                    //            trace_ua.data.insight.results[i].test = "LB";

                    //            break;
                    //        case 8:
                    //            trace_ua.data.insight.results[i].test = "F-LB";

                    //            break;
                    //        case 9:
                    //            trace_ua.data.insight.results[i].test = "RM1";

                    //            break;
                    //        case 10:
                    //            trace_ua.data.insight.results[i].test = "RT";

                    //            break;
                    //        case 11:
                    //            trace_ua.data.insight.results[i].test = "F-RT";
                    //            break;
                    //        case 12:
                    //            trace_ua.data.insight.results[i].test = "RM4";

                    //            break;
                    //        case 13:
                    //            trace_ua.data.insight.results[i].test = "RB";

                    //            break;
                    //        case 14:
                    //            trace_ua.data.insight.results[i].test = "F-RB";

                    //            break;
                    //        case 15:
                    //            trace_ua.data.insight.results[i].test = "TM";

                    //            break;
                    //        case 16:
                    //            trace_ua.data.insight.results[i].test = "BM";

                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //    switch (i % 10)
                    //    {
                    //        case 0:
                    //            trace_ua.data.insight.results[i].sub_test = "PowerDiode";
                    //            trace_ua.data.insight.results[i].units = "%";
                    //            trace_ua.data.insight.results[i].value = dt.Rows[i / 10][3].ToString();
                    //            break;
                    //        case 1:
                    //            trace_ua.data.insight.results[i].sub_test = "PowerFiber";
                    //            trace_ua.data.insight.results[i].units = "%";
                    //            trace_ua.data.insight.results[i].value = dt.Rows[i / 10][4].ToString();
                    //            break;
                    //        case 2:
                    //            trace_ua.data.insight.results[i].sub_test = "frequency";
                    //            trace_ua.data.insight.results[i].units = "KHZ";
                    //            trace_ua.data.insight.results[i].value = dt.Rows[i / 10][5].ToString();
                    //            break;
                    //        case 3:
                    //            trace_ua.data.insight.results[i].sub_test = "waveform";
                    //            trace_ua.data.insight.results[i].units = "";
                    //            trace_ua.data.insight.results[i].value = dt.Rows[i / 10][6].ToString();
                    //            break;
                    //        case 4:
                    //            trace_ua.data.insight.results[i].sub_test = "laser_speed";
                    //            trace_ua.data.insight.results[i].units = "mm/s";
                    //            trace_ua.data.insight.results[i].value = dt.Rows[i / 10][7].ToString();
                    //            break;
                    //        case 5:
                    //            trace_ua.data.insight.results[i].sub_test = "jump_speed";
                    //            trace_ua.data.insight.results[i].units = "mm/s";
                    //            trace_ua.data.insight.results[i].value = dt.Rows[i / 10][8].ToString();
                    //            break;
                    //        case 6:
                    //            trace_ua.data.insight.results[i].sub_test = "jump_delay";
                    //            trace_ua.data.insight.results[i].units = "us";
                    //            trace_ua.data.insight.results[i].value = dt.Rows[i / 10][9].ToString();
                    //            break;
                    //        case 7:
                    //            trace_ua.data.insight.results[i].sub_test = "position_delay";
                    //            trace_ua.data.insight.results[i].units = "us";
                    //            trace_ua.data.insight.results[i].value = dt.Rows[i / 10][10].ToString();
                    //            break;
                    //        case 8:
                    //            trace_ua.data.insight.results[i].sub_test = "pulse_profile";
                    //            trace_ua.data.insight.results[i].units = "";
                    //            trace_ua.data.insight.results[i].value = dt.Rows[i / 10][11].ToString();
                    //            break;
                    //        case 9:
                    //            trace_ua.data.insight.results[i].sub_test = "pulse_profileFiber";
                    //            trace_ua.data.insight.results[i].units = "";
                    //            trace_ua.data.insight.results[i].value = dt.Rows[i / 10][12].ToString();
                    //            break;
                    //    }
                    //}
                    //#endregion

                    trace_ua.data.items = new ExpandoObject();
                    trace_ua.data.insight.test_attributes.test_result = "pass";
                    trace_ua.data.insight.test_attributes.unit_serial_number = SN.Substring(0, 17);
                    trace_ua.data.insight.test_attributes.uut_start = DateTime.Now.AddSeconds(-35).ToString("yyyy-MM-dd HH:mm:ss");
                    trace_ua.data.insight.test_attributes.uut_stop = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    trace_ua.data.insight.test_station_attributes.fixture_id = fixture;
                    trace_ua.data.insight.test_station_attributes.head_id = head_id;//焊机序号 1 2 3 4
                    trace_ua.data.insight.test_station_attributes.line_id = Global.Trace_line_id;
                    trace_ua.data.insight.test_station_attributes.software_name = "HG";
                    trace_ua.data.insight.test_station_attributes.software_version = Global.version;
                    trace_ua.data.insight.test_station_attributes.station_id = Global.Trace_station_id;

                    trace_ua.data.insight.uut_attributes.STATION_STRING = "Free-from string";
                    trace_ua.data.insight.uut_attributes.fixture_id = fixture;
                    trace_ua.data.insight.uut_attributes.hatch = "NA";
                    trace_ua.data.insight.uut_attributes.laser_start_time = DateTime.Now.AddSeconds(-22).ToString("yyyy-MM-dd HH:mm:ss");
                    trace_ua.data.insight.uut_attributes.laser_stop_time = DateTime.Now.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    trace_ua.data.insight.uut_attributes.lc_sn = SN;

                    trace_ua.data.insight.uut_attributes.left_rail_sn = left_rail;

                    trace_ua.data.insight.uut_attributes.swing_amplitude = hg_data[SN].swing_amplitude;//1摆幅
                    trace_ua.data.insight.uut_attributes.swing_freq = hg_data[SN].swing_freq; //2摆动频率
                    trace_ua.data.insight.uut_attributes.pattern_type = hg_data[SN].pattern_type;//3摆动图像类型
                    trace_ua.data.insight.uut_attributes.precitec_value = hg_data[SN].PrecitecValue;//4  值 
                    trace_ua.data.insight.uut_attributes.precitec_rev = hg_data[SN].PrecitecProcessRev;//5   版本号 
                    trace_ua.data.insight.uut_attributes.precitec_grading = hg_data[SN].PrecitecGrading;//6 检测等级
                    //trace_ua.data.insight.uut_attributes.precitec_result = dt.Rows[0][19].ToString();//7总结果

                    trace_ua.data.insight.uut_attributes.right_rail_sn = right_rail;

                    trace_ua.data.insight.uut_attributes.spot_size = "NA";

                    trace_ua.data.insight.uut_attributes.station_vendor = "HG";


                    trace_ua.data.insight.uut_attributes.tossing_item = "NA";


                    for (int i = 0; i <= 0; i++)
                    {
                        (trace_ua.data.items as ICollection<KeyValuePair<string, object>>).Add(new KeyValuePair<string, object>("error_x" , "NA"));
                    }
                    string SendTraceLogs = JsonConvert.SerializeObject(trace_ua, Formatting.None, jsetting);


                    for (int i = 0; i < hg_data[SN].results.Length / 10; i++)
                    {
                        //var area = "";
                        //switch (i)
                        //{
                        //    case 0:
                        //        area = "RM2";
                        //        break;
                        //    case 1:
                        //        area = "RM3";
                        //        break;
                        //    case 2:
                        //        area = "LM1";
                        //        break;
                        //    case 3:
                        //        area = "LT";
                        //        break;
                        //    case 4:
                        //        area = "F-LT";
                        //        break;
                        //    case 5:
                        //        area = "LM2";
                        //        break;
                        //    case 6:
                        //        area = "LM3";
                        //        break;
                        //    case 7:
                        //        area = "LB";
                        //        break;
                        //    case 8:
                        //        area = "F-LB";
                        //        break;
                        //    case 9:
                        //        area = "RM1";
                        //        break;
                        //    case 10:
                        //        area = "RT";
                        //        break;
                        //    case 11:
                        //        area = "F-RT";
                        //        break;
                        //    case 12:
                        //        area = "RM4";
                        //        break;
                        //    case 13:
                        //        area = "RB";
                        //        break;
                        //    case 14:
                        //        area = "F-RB";
                        //        break;
                        //    case 15:
                        //        area = "TM";
                        //        break;
                        //    case 16:
                        //        area = "BM";
                        //        break;
                        //    default:
                        //        break;
                        //}
                        string[] str1 = hg_data[SN].results[i * 10 + 8].value.Split(new string[3] { "ms", "%", "/" }, StringSplitOptions.RemoveEmptyEntries);
                        string[] str2 = hg_data[SN].results[i * 10 + 9].value.Split(new string[3] { "ms", "%", "/" }, StringSplitOptions.RemoveEmptyEntries);
                        var paraInfo = new FlHelper.Models.ParaInfo()
                        {
                            /// <summary>
                            /// 焊接區域
                            /// </summary>

                            area = hg_data[SN].results[i * 10].test,
                            /// <summary>
                            /// 開始焊接時間
                            /// </summary>

                            sTime = DateTime.Parse(trace_ua.data.insight.uut_attributes.laser_start_time).ToString("yyyyMMddHHmmss"),
                            /// <summary>
                            /// 焊接完成時間
                            /// </summary>

                            eTime = DateTime.Parse(trace_ua.data.insight.uut_attributes.laser_stop_time).ToString("yyyyMMddHHmmss"),
                            /// <summary>
                            /// 功率設置上限
                            /// </summary>

                            powerUsl = hg_data[SN].results[i * 10].value,
                            /// <summary>
                            /// 功率設置下限
                            /// </summary>

                            powerLsl = hg_data[SN].results[i * 10 + 1].value,
                            /// <summary>
                            /// 脉衝波形
                            /// </summary>

                            pusleWaveform = hg_data[SN].results[i * 10 + 3].value,
                            /// <summary>
                            /// 填充間隙 gai5
                            /// </summary>

                            hatch = "",
                            /// <summary>
                            /// 填充角度
                            /// </summary>

                            angel = "",
                            /// <summary>
                            /// 搖擺振幅
                            /// </summary>

                            swingAmplitude = trace_ua.data.insight.uut_attributes.swing_amplitude,
                            /// <summary>
                            /// 搖擺頻率
                            /// </summary>

                            swingFreq = trace_ua.data.insight.uut_attributes.swing_freq,
                            /// <summary>
                            /// 電流
                            /// </summary>

                            current = "",
                            /// <summary>
                            /// 空跳速度
                            /// </summary>

                            jumpingSpeed = "",
                            /// <summary>
                            /// 空跳延時
                            /// </summary>

                            jumpingDelay = hg_data[SN].results[i * 10 + 6].value,
                            /// <summary>
                            /// 攝像頭焊接間隙
                            /// </summary>

                            trimWasherGap = "",
                            /// <summary>
                            /// 掃描延時
                            /// </summary>

                            scanningDelay = hg_data[SN].results[i * 10 + 7].value,
                            /// <summary>
                            /// Scan掃描層波形
                            /// </summary>

                            scanParametersWaveform = hg_data[SN].results[i * 10 + 3].value,
                            /// <summary>
                            /// Scan掃描層焊點大小
                            /// </summary>

                            scanSpotSize = trace_ua.data.insight.uut_attributes.spot_size,
                            /// <summary>
                            /// Scan掃描層設定功率
                            /// </summary>

                            scanPower = "",
                            /// <summary>
                            /// Scan掃描層脉衝能量
                            /// </summary>

                            scanPulseEnergy = "",
                            /// <summary>
                            /// Scan掃描層頻率
                            /// </summary>

                            scanFrequency = "",
                            /// <summary>
                            /// Scan掃描層焊接速度
                            /// </summary>

                            scanLinearSpeed = "",
                            /// <summary>
                            /// Scan掃描層填充類型
                            /// </summary>

                            scanFillingPattern = "",
                            /// <summary>
                            /// Scan掃描層電流
                            /// </summary>

                            scanCurrnet = "",
                            /// <summary>
                            ///Scan掃描層空跳速度
                            /// </summary>

                            scanJumpingSpeed = "",
                            /// <summary>
                            /// Scan掃描層空跳延時
                            /// </summary>

                            scanJumpingDelay = "",
                            /// <summary>
                            /// Scan掃描層掃描延時
                            /// </summary>

                            scanScanningDelay = "",
                            /// <summary>
                            /// 焊接類型 gai4
                            /// </summary>

                            patternType = "Swing",
                            /// <summary>
                            /// Laser焊接層參數波形
                            /// </summary>

                            parametersWaveform = hg_data[SN].results[i * 10 + 3].value,
                            /// <summary>
                            /// Laser焊接層焊點尺寸
                            /// </summary>

                            spotSize = trace_ua.data.insight.uut_attributes.spot_size,

                            /// <summary>
                            ///Laser焊接層功率 GAI2
                            /// </summary>

                            power = hg_data[SN].results[i * 10 + 1].value,
                            /// <summary>
                            /// Laser焊接層脈衝能量
                            /// </summary>

                            pulseEnergy = hg_data[SN].results[i * 10].value,
                            /// <summary>
                            /// Laser焊接層頻率
                            /// </summary>

                            frequency = "",
                            /// <summary>
                            /// Laser焊接層線性速度 
                            /// </summary>

                            linearSpeed = hg_data[SN].results[i * 10 + 4].value,
                            /// <summary>
                            /// Laser焊接層填充方式
                            /// </summary>

                            fillingPattern = "",
                            /// <summary>
                            /// CCD版本
                            /// </summary>

                            ccd = "9.5SR2",
                            /// <summary>
                            /// 普雷斯特檢測結果
                            /// </summary>

                            precitecResult = "",
                            /// <summary>
                            /// 普雷斯特檢測等級
                            /// </summary>

                            precitecGrading = trace_ua.data.insight.uut_attributes.precitec_grading,
                            /// <summary>
                            /// 普雷斯特數值
                            /// </summary>

                            precitecValue = trace_ua.data.insight.uut_attributes.precitec_value,
                            /// <summary>
                            /// 普雷斯特檢測版本
                            /// </summary>

                            precitecProcessRev = trace_ua.data.insight.uut_attributes.precitec_rev,
                            /// <summary>
                            /// 預扭
                            /// </summary>

                            preTorque = "",
                            /// <summary>
                            /// 預推
                            /// </summary>

                            prePush = "",
                            /// <summary>
                            /// 起點半徑
                            /// </summary>

                            startingRadius = "",
                            /// <summary>
                            /// 終點半徑
                            /// </summary>

                            endingRadius = "",
                            /// <summary>
                            /// 內圈圈數
                            /// </summary>

                            innerRingQty = "",
                            /// <summary>
                            /// 外圈圈數
                            /// </summary>

                            outerRingQty = "",
                            /// <summary>
                            /// 螺旋
                            /// </summary>

                            sprialPitch = "",
                            /// <summary>
                            /// 內環第一段波時間
                            /// </summary>

                            a1stFiberCoreWaveTimePeriod = str1[0],
                            /// <summary>
                            /// 內環第一段波功率
                            /// </summary>

                            a1stSettingInnerLASEREnergyPercentage = str1[1],
                            ///// <summary>
                            ///// 外環第一段波時間
                            ///// </summary>

                            //a1stOuterRingWaveTimePeriod = "",
                            ///// <summary>
                            ///// 外環第一段波功率
                            ///// </summary>

                            //a1stOuterRingWavePower = "",

                            /// <summary>
                            /// 內環第二段波時間
                            /// </summary>

                            a2ndFiberCoreWaveTimePeriod = str1[2],
                            /// <summary>
                            /// 內環第二段波功率
                            /// </summary>

                            a2ndSettingInnerLASEREnergyPercentage = str1[3],
                            /// <summary>
                            /// 外環第二段波時間
                            /// </summary>

                            //a2ndOuterRingWaveTimePeriod = "",
                            ///// <summary>
                            ///// 外環第二段波功率
                            ///// </summary>

                            //a2ndOuterRingWavePower = "",

                            /// <summary>
                            /// 內環第三段波時間
                            /// </summary>

                            a3rdFiberCoreWaveTimePeriod = str1[4],
                            /// <summary>
                            /// 內環第三段波功率
                            /// </summary>

                            a3rdSettingInnerLASEREnergyPercentage = str1[5],
                            /// <summary>
                            /// 外環第三段波時間
                            /// </summary>

                            //a3rdOuterRingWaveTimePeriod = "",
                            ///// <summary>
                            ///// 外環第三段波功率
                            ///// </summary>

                            //a3rdOuterRingWavePower = "",

                            /// <summary>
                            /// 內環第四段波時間
                            /// </summary>

                            a4thFiberCoreWaveTimePeriod = "",
                            /// <summary>
                            /// 內環第四段波功率
                            /// </summary>

                            a4thSettingInnerLASEREnergyPercentage = "",
                            /// <summary>
                            /// 外環第四段波時間
                            /// </summary>

                            //a4thOuterRingWaveTimePeriod = "",
                            ///// <summary>
                            ///// 外環第四段波功率
                            ///// </summary>

                            //a4thOuterRingWavePower = "",

                            /// <summary>
                            /// 內環第五段波時間
                            /// </summary>

                            a5thFiberCoreWaveTimePeriod = "",
                            /// <summary>
                            /// 內環第五段波功率
                            /// </summary>

                            a5thSettingInnerLASEREnergyPercentage = "",
                            /// <summary>
                            /// 外環第五段波時間
                            /// </summary>

                            //a5thOuterRingWaveTimePeriod = "",
                            ///// <summary>
                            ///// 外環第五段波功率
                            ///// </summary>

                            //a5thOuterRingWavePower = "",

                            /// <summary>
                            /// 內環第六段波時間
                            /// </summary>

                            a6thFiberCoreWaveTimePeriod = "",
                            /// <summary>
                            /// 內環第六段波功率
                            /// </summary>

                            a6thSettingInnerLASEREnergyPercentage = "",
                            /// <summary>
                            /// 外環第六段波時間
                            /// </summary>

                            //a6thOuterRingWaveTimePeriod = "",
                            ///// <summary>
                            ///// 外環第六段波功率
                            ///// </summary>

                            //a6thOuterRingWavePower = "",

                            SettingOuterLASEREnergyPercentage = hg_data[SN].results[i * 10].value,
                            a1stPulseWidth = "",
                            a2ndPulseWidth = "",
                            a3rdPulseWidth = "",
                            a4thPulseWidth = "",
                            a5thPulseWidth = "",
                            a6thPulseWidth = "",

                            /// <summary>
                            /// 標準速度(mm/s)
                            /// </summary>

                            Speed = hg_data[SN].results[i * 10 + 4].value,
                            /// <summary>
                            /// (mm)
                            /// </summary>

                            Position = "",
                            /// <summary>
                            /// 擺動幅度(mm)
                            /// </summary>


                            WobbleAmplitude = trace_ua.data.insight.uut_attributes.swing_amplitude,
                            /// <summary>
                            /// 擺動頻率(HZ)
                            /// </summary>

                            WobbleFrequency = trace_ua.data.insight.uut_attributes.swing_freq,
                            /// <summary>
                            /// 波形模式
                            /// </summary>

                            waveMode = "",
                            /// <summary>
                            /// 起點半徑
                            /// </summary>

                            scanStartingRadius = "",

                            /// <summary>
                            /// 終點半徑
                            /// </summary>

                            scanEndingRadius = "",
                            /// <summary>
                            /// 內圈圈數
                            /// </summary>

                            scanInnerRingQty = "0",
                            /// <summary>
                            /// 外圈圈數
                            /// </summary>

                            scanOuterRingQty = "0",
                            /// <summary>
                            /// 螺旋間距
                            /// </summary>

                            scanSprialPitch = "",
                            /// <summary>
                            /// 脉衝輪廓
                            /// </summary>

                            pulseProfile = hg_data[SN].results[i * 10 + 8].value,

                        };

                        paraInfos[i] = paraInfo;
                    }


                    Log.WriteLog_ServerInfo("Trace上传:" + SendTraceLogs);
                    _homefrm.AppendRichText(SendTraceLogs, "Rtxt_Send_Trace");

                    RequestAPI3.CallBobcat(Global.TraceURL, SendTraceLogs, out callResult, out errMsg);
                    Log.WriteLog_ServerInfo("Trace接收：" + callResult);
                    _homefrm.AppendRichText(callResult, "Rtxt_Rec_Trace");

                    JArray recvObj = JsonConvert.DeserializeObject<JArray>(callResult);
                    //JArray recvObj = JArray.Parse(callResult);
                    if (recvObj[0]["id"].ToString().Length == recvObj[1]["id"].ToString().Length)
                    {


                        if (lstn_info.ContainsKey(SN))
                        {
                            lstn_info[SN].Lstn_Traceid = recvObj[0]["id"].ToString();//获取到中板返回的id，给过站使用
                        }
                        else
                        {
                            Last_Station lst_Stn = new Last_Station();

                            lst_Stn.Lstn_Traceid = recvObj[0]["id"].ToString();

                            lstn_info.Add(SN, lst_Stn);
                        }
                        _homefrm.AppendRichText("Trace上传OK!", "Rtxt_Rec_Trace");
                        _homefrm.UpDatelabelcolor(Color.DarkSeaGreen, "Trace上传OK!", "lb_Trace");

                        return new ResultModel<FlHelper.Models.ParaInfo[]>() { Result = true, Model = paraInfos };
                    }
                    else
                    {
                        Log.WriteLog(head_id + "#焊机Trace上传失败");
                        _homefrm.UpDatelabelcolor(Color.Red, head_id + "#焊机Trace上传NG!", "lb_Trace");
                        return new ResultModel<FlHelper.Models.ParaInfo[]>() { Result = false, Model = paraInfos };
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLog(head_id + "#焊机Trace上传异常" + ex.ToString());
                    _homefrm.UpDatelabelcolor(Color.Red, head_id + "#焊机Trace上传异常!", "lb_Trace");

                    return new ResultModel<FlHelper.Models.ParaInfo[]>() { Result = false, Model = paraInfos };
                }
            }

        }

        /// <summary>
        /// 过站
        /// </summary>
        /// <param name="SN">产品码</param>
        /// <param name="left_rail">左键码</param>
        /// <param name="right_rail">右键码</param>
        /// <param name="fixture">治具码</param>
        private bool pass_station(string SN, string left_rail, string right_rail, string fixture, string cavity, FlHelper.Models.ParaInfo[] parainfos = null)
        {
            lock (lock_pass)
            {
                try
                {
                    
                    _homefrm.AppendRichText("产品码：" + SN, "Rtxt_Send_Pass");
                    _homefrm.AppendRichText("左键码：" + left_rail, "Rtxt_Send_Pass");
                    _homefrm.AppendRichText("右键码：" + right_rail, "Rtxt_Send_Pass");
                    _homefrm.AppendRichText("治具码：" + fixture, "Rtxt_Send_Pass");

                    string callResult = "";
                    string errMsg = "";

                    JsonSerializerSettings jsetting = new JsonSerializerSettings();
                    jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值

                    PassStn pass_Stn = new PassStn();

                    //lstn_info[SN] = new Last_Station();

                    pass_Stn.hashCode = Global.hashCode;
                    pass_Stn.requestId = lstn_info[SN].Lstn_requestId;
                    pass_Stn.resv1 = "";
                    pass_Stn.productInfo = new ProductInfo();
                    pass_Stn.productInfo.barCode = SN;
                    pass_Stn.productInfo.barCodeType = Global.barCodeType; //"LC";
                    pass_Stn.productInfo.station = Global.station; //"LCHWelding";
                    pass_Stn.productInfo.billNo = Global.billNo;


                    pass_Stn.productInfo.bindCode = new BindCode1[4];
                    pass_Stn.productInfo.bindCode[0] = new BindCode1();
                    pass_Stn.productInfo.bindCode[1] = new BindCode1();
                    pass_Stn.productInfo.bindCode[2] = new BindCode1();
                    pass_Stn.productInfo.bindCode[3] = new BindCode1();

                    pass_Stn.productInfo.bindCode[0].codeSn = left_rail;
                    pass_Stn.productInfo.bindCode[0].codeSnType = "IQC_LEFT_RAIL";
                    pass_Stn.productInfo.bindCode[0].replace = "1";
                    pass_Stn.productInfo.bindCode[1].codeSn = right_rail;
                    pass_Stn.productInfo.bindCode[1].codeSnType = "IQC_RIGHT_RAIL";
                    pass_Stn.productInfo.bindCode[1].replace = "1";
                    pass_Stn.productInfo.bindCode[2].codeSn = fixture;
                    pass_Stn.productInfo.bindCode[2].codeSnType = "FIXTURE";
                    pass_Stn.productInfo.bindCode[2].replace = "1";
                    pass_Stn.productInfo.bindCode[3].codeSn = Global.equipmentNo; //"MO0093730001-10";
                    pass_Stn.productInfo.bindCode[3].codeSnType = "EQUIPMENT";
                    pass_Stn.productInfo.bindCode[3].replace = "1";

                    pass_Stn.equipmentInfo = new EquipmentInfo();
                    pass_Stn.equipmentInfo.equipmentIp = Global.Trace_ip;
                    pass_Stn.equipmentInfo.equipmentType = "L";
                    pass_Stn.equipmentInfo.equipmentNo = Global.equipmentNo; //"MO0093730001-10";
                    pass_Stn.equipmentInfo.vendorId = "HG";
                    pass_Stn.equipmentInfo.processRevs = Global.version;

                    pass_Stn.recipeInfo = new RecipeInfo();
                    pass_Stn.recipeInfo.startTime = DateTime.Now.AddSeconds(-1).ToString("yyyyMMddHHmmss");
                    pass_Stn.recipeInfo.endTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    pass_Stn.recipeInfo.cavity = "0" + cavity;
                    pass_Stn.recipeInfo.judgement = "PASS";
                    pass_Stn.recipeInfo.humidity = "63";
                    pass_Stn.recipeInfo.temperature = "35";

                    pass_Stn.recipeInfo.paraInfo = new ParaInfo[hg_data[SN].results.Length / 10];
                    var heightsValue = new Dictionary<string, heightModel>();
                    if (heights.ContainsKey(SN))
                    {
                        heights.TryGetValue(SN, out heightsValue);
                    }
                    for (int i = 0; i < parainfos.Length; i++)
                    {
                        pass_Stn.recipeInfo.paraInfo[i] = new ParaInfo(parainfos[i], heightsValue);

                    }


                    pass_Stn.appleReturnInfo = new AppleReturnInfo();
                    pass_Stn.appleReturnInfo.status = "200";
                    pass_Stn.appleReturnInfo.id = lstn_info[SN].Lstn_Traceid;
                    pass_Stn.appleReturnInfo.contact = "Vendor";
                    pass_Stn.appleReturnInfo.error = "";


                    string send_Pass = JsonConvert.SerializeObject(pass_Stn, Formatting.None, jsetting).Replace("a1st", "1st").Replace("a2nd", "2nd").Replace("a3rd", "3rd").Replace("a4th", "4th").Replace("a5th", "5th").Replace("a6th", "6th"); ;
                    Log.WriteLog_ServerInfo("A工位 Equipment to API 過站報告:" + send_Pass);
                    _homefrm.AppendRichText(send_Pass, "Rtxt_Send_Pass");

                    RequestAPI3.CallBobcat2(Global.passURL, send_Pass, Global.hashCode, out callResult, out errMsg);
                    Log.WriteLog_ServerInfo("A工位 API to Equipment 過站報告:" + callResult);
                    _homefrm.AppendRichText(callResult, "Rtxt_Rec_Pass");

                    JObject recvObj = JsonConvert.DeserializeObject<JObject>(callResult);

                    if (recvObj["rc"].ToString() == "000")
                    {
                        _homefrm.AppendRichText("过站OK！", "Rtxt_Rec_Pass");
                        _homefrm.UpDatelabelcolor(Color.DarkSeaGreen, "过站OK!", "lb_过站");
                        heights.Remove(SN);
                        return true;
                    }
                    else
                    {
                        _homefrm.AppendRichText("过站NG！", "Rtxt_Rec_Pass");
                        _homefrm.UpDatelabelcolor(Color.Red, "过站NG!", "lb_过站");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLog("过站异常" + ex.ToString());
                    _homefrm.UpDatelabelcolor(Color.Red, "过站异常!", "lb_过站");
                    return false;
                }
            }

        }

        #endregion

        #region Trace，过站缓存重传

        private void retry_TracePass(object obj)
        {
            while (true)
            {
                try
                {
                    int i = 0;
                    if (!Global.b_Trace)
                    {
                        string selectStr1 = "select count(*) from retry_Pass";
                        DataTable d1 = SQL.ExecuteQuery(selectStr1);
                        if (Convert.ToInt32(d1.Rows[0][0].ToString()) > 0)
                        {
                            if (i < 3)
                            {
                                string selectStr2 = "select * from retry_Pass where ID=(SELECT MIN(ID) FROM retry_Pass)";
                                DataTable d2 = SQL.ExecuteQuery(selectStr2);
                                var ResultModel = pass_Trace(d2.Rows[0][1].ToString(), d2.Rows[0][2].ToString(), d2.Rows[0][3].ToString(), d2.Rows[0][4].ToString(), d2.Rows[0][5].ToString());
                                if (ResultModel.Result)
                                {
                                    if (pass_station(d2.Rows[0][1].ToString(), d2.Rows[0][2].ToString(), d2.Rows[0][3].ToString(), d2.Rows[0][4].ToString(), d2.Rows[0][5].ToString()))
                                    {
                                        string delStr = "delete from retry_Pass where ID=(select MIN(ID) from retry_Pass)";
                                        SQL.ExecuteUpdate(delStr);
                                        Log.WriteLog("Trace和过站补传成功");
                                        i = 0;
                                    }
                                    else
                                    {
                                        i++;
                                        Thread.Sleep(5000 * i);
                                    }
                                }
                                else
                                {
                                    i++;
                                    Thread.Sleep(5000 * i);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }


                Thread.Sleep(100);
            }
        }






        /// <summary>
        /// Trace补传
        /// </summary>
        /// <returns></returns>
        private bool retry_Trace()
        {
            try
            {
                string callResult = "";
                string errMsg = "";

                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值


                string selectStr2 = "select * from retry_Pass where ID=(SELECT MIN(ID) FROM retry_Pass)";
                DataTable d2 = SQL.ExecuteQuery(selectStr2);

                TraceMesRequest trace_ua = new TraceMesRequest();


                trace_ua.serial_type = "rm";
                trace_ua.serials = new string[3];
                trace_ua.serials[0] = d2.Rows[0][1].ToString();
                trace_ua.serials[1] = d2.Rows[0][2].ToString();
                trace_ua.serials[2] = d2.Rows[0][3].ToString();


                trace_ua.data = new data();
                trace_ua.data.insight = new Insight();
                trace_ua.data.insight.test_attributes = new Test_attributes();
                trace_ua.data.insight.test_station_attributes = new Test_station_attributes();
                trace_ua.data.insight.uut_attributes = new Uut_attributes();
                trace_ua.data.insight.results = new Result[72];
                for (int i = 0; i < trace_ua.data.insight.results.Length; i++)
                {
                    trace_ua.data.insight.results[i] = new Result();

                    trace_ua.data.insight.results[i].result = "pass";

                    switch (i / 8)//i除以8的结果，相当于用来区分每个焊点
                    {
                        //第一个焊点
                        case 0:
                            trace_ua.data.insight.results[i].test = "location1_RM2_layer1";
                            break;
                        //第二个焊点
                        case 1:
                            trace_ua.data.insight.results[i].test = "location2_LM1_layer2";
                            break;
                        case 2:
                            trace_ua.data.insight.results[i].test = "location3_LT_layer3";
                            break;
                        case 3:
                            trace_ua.data.insight.results[i].test = "location4_LM2_layer4";
                            break;
                        case 4:
                            trace_ua.data.insight.results[i].test = "location5_LB_layer5";
                            break;
                        case 5:
                            trace_ua.data.insight.results[i].test = "location6_RM1_layer6";
                            break;
                        case 6:
                            trace_ua.data.insight.results[i].test = "location7_RT_layer7";
                            break;
                        case 7:
                            trace_ua.data.insight.results[i].test = "location8_RM3_layer8";
                            break;
                        case 8:
                            trace_ua.data.insight.results[i].test = "location9_RB_layer9";
                            break;
                        default:
                            break;
                    }
                    switch (i % 8)//除以8的余数，相当于用来区分每个焊接参数（不同焊点）
                    {
                        case 0:
                            trace_ua.data.insight.results[i].sub_test = "power";
                            trace_ua.data.insight.results[i].units = "W";
                            trace_ua.data.insight.results[i].value = "15%";
                            break;
                        case 1:
                            trace_ua.data.insight.results[i].sub_test = "frequency";
                            trace_ua.data.insight.results[i].units = "KHz";
                            trace_ua.data.insight.results[i].value = "0";
                            break;
                        case 2:
                            trace_ua.data.insight.results[i].sub_test = "waveform";
                            trace_ua.data.insight.results[i].units = "";

                            if (i / 8 == 0)
                            {
                                trace_ua.data.insight.results[i].value = "13";
                            }
                            else if (i / 8 == 1)
                            {
                                trace_ua.data.insight.results[i].value = "11";
                            }
                            else if (i / 8 == 2)
                            {
                                trace_ua.data.insight.results[i].value = "12";
                            }
                            else if (i / 8 == 3 || i / 8 == 4 || i / 8 == 5 || i / 8 == 7)
                            {
                                trace_ua.data.insight.results[i].value = "14";
                            }
                            else if (i / 8 == 6)
                            {
                                trace_ua.data.insight.results[i].value = "10";
                            }
                            else
                            {
                                trace_ua.data.insight.results[i].value = "9";
                            }
                            break;
                        case 3:
                            trace_ua.data.insight.results[i].sub_test = "laser_speed";
                            trace_ua.data.insight.results[i].units = "mm/s";
                            trace_ua.data.insight.results[i].value = "40";
                            break;
                        case 4:
                            trace_ua.data.insight.results[i].sub_test = "jump_speed";
                            trace_ua.data.insight.results[i].units = "mm/s";
                            trace_ua.data.insight.results[i].value = "250";
                            break;
                        case 5:
                            trace_ua.data.insight.results[i].sub_test = "jump_delay";
                            trace_ua.data.insight.results[i].units = "us";
                            trace_ua.data.insight.results[i].value = "0";
                            break;
                        case 6:
                            trace_ua.data.insight.results[i].sub_test = "position_delay";
                            trace_ua.data.insight.results[i].units = "us";
                            trace_ua.data.insight.results[i].value = "0";
                            break;
                        case 7:
                            trace_ua.data.insight.results[i].sub_test = "pulse_profile";
                            trace_ua.data.insight.results[i].units = "";

                            if (i / 8 == 0)
                            {
                                trace_ua.data.insight.results[i].value = "0ms24%/500ms24%/50ms0%";
                            }
                            else if (i / 8 == 1)
                            {
                                trace_ua.data.insight.results[i].value = "0ms24%/435ms24%/50ms0%";
                            }
                            else if (i / 8 == 2)
                            {
                                trace_ua.data.insight.results[i].value = "0ms24%/465ms24%/50ms0%";
                            }
                            else if (i / 8 == 3 || i / 8 == 4 || i / 8 == 5 || i / 8 == 7)
                            {
                                trace_ua.data.insight.results[i].value = "0ms24%/535ms24%/50ms0%";
                            }
                            else if (i / 8 == 6)
                            {
                                trace_ua.data.insight.results[i].value = "0ms24%/305ms24%/50ms0%";
                            }
                            else
                            {
                                trace_ua.data.insight.results[i].value = "0ms24%/245ms24%/50ms0%";
                            }
                            break;
                        default:
                            break;
                    }
                }

                trace_ua.data.items = new ExpandoObject();
                trace_ua.data.insight.test_attributes.test_result = "pass";
                trace_ua.data.insight.test_attributes.unit_serial_number = d2.Rows[0][1].ToString().Substring(0, 17);
                trace_ua.data.insight.test_attributes.uut_start = DateTime.Now.AddSeconds(-35).ToString("yyyy-MM-dd HH:mm:ss");
                trace_ua.data.insight.test_attributes.uut_stop = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                trace_ua.data.insight.test_station_attributes.fixture_id = d2.Rows[0][4].ToString();
                trace_ua.data.insight.test_station_attributes.head_id = d2.Rows[0][5].ToString();//焊机序号 1 2 3 4
                trace_ua.data.insight.test_station_attributes.line_id = Global.Trace_line_id;
                trace_ua.data.insight.test_station_attributes.software_name = "HG";
                trace_ua.data.insight.test_station_attributes.software_version = Global.version;
                trace_ua.data.insight.test_station_attributes.station_id = Global.Trace_station_id;

                trace_ua.data.insight.uut_attributes.STATION_STRING = "Free-from string";
                trace_ua.data.insight.uut_attributes.fixture_id = d2.Rows[0][4].ToString();
                trace_ua.data.insight.uut_attributes.hatch = "0.04";
                trace_ua.data.insight.uut_attributes.laser_start_time = DateTime.Now.AddSeconds(-12).ToString("yyyy-MM-dd HH:mm:ss");
                trace_ua.data.insight.uut_attributes.laser_stop_time = DateTime.Now.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                trace_ua.data.insight.uut_attributes.lc_sn = d2.Rows[0][1].ToString();

                trace_ua.data.insight.uut_attributes.left_rail_sn = d2.Rows[0][2].ToString();

                trace_ua.data.insight.uut_attributes.pattern_type = d2.Rows[0][5].ToString();//焊机序号  1 2 3 4
                trace_ua.data.insight.uut_attributes.precitec_grading = "NA";
                trace_ua.data.insight.uut_attributes.precitec_rev = "NA";
                trace_ua.data.insight.uut_attributes.precitec_value = "NA";

                trace_ua.data.insight.uut_attributes.right_rail_sn = d2.Rows[0][3].ToString();

                trace_ua.data.insight.uut_attributes.spot_size = "NA";

                trace_ua.data.insight.uut_attributes.station_vendor = "HG";

                trace_ua.data.insight.uut_attributes.swing_amplitude = "0.4";
                trace_ua.data.insight.uut_attributes.swing_freq = "10000";
                trace_ua.data.insight.uut_attributes.tossing_item = "NA";


                for (int i = 0; i <= 0; i++)
                {
                    (trace_ua.data.items as ICollection<KeyValuePair<string, object>>).Add(new KeyValuePair<string, object>("error_" + (i + 1), "00000000" + "_" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                }
                string SendTraceLogs = JsonConvert.SerializeObject(trace_ua, Formatting.None, jsetting);
                Log.WriteLog("Trace重传发送:" + SendTraceLogs);

                RequestAPI3.CallBobcat(Global.TraceURL, SendTraceLogs, out callResult, out errMsg);
                Log.WriteLog("Trace重传接收：" + callResult);

                JArray recvObj = JsonConvert.DeserializeObject<JArray>(callResult);
                //JArray recvObj = JArray.Parse(callResult);
                if (recvObj[0]["id"].ToString().Length == recvObj[1]["id"].ToString().Length)
                {
                    if (lstn_info.ContainsKey(d2.Rows[0][1].ToString()))
                    {
                        lstn_info[d2.Rows[0][1].ToString()].Lstn_Traceid = recvObj[0]["id"].ToString();//获取到中板返回的id，给过站使用
                    }
                    else
                    {
                        Last_Station lst_Stn = new Last_Station();

                        lst_Stn.Lstn_Traceid = recvObj[0]["id"].ToString();

                        lstn_info.Add(d2.Rows[0][1].ToString(), lst_Stn);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("Trace补传异常" + ex.ToString());

                return false;
            }
        }

        private bool retry_Pass()
        {
            try
            {

                string callResult = "";
                string errMsg = "";

                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值



                string selectStr2 = "select * from retry_Pass where ID=(SELECT MIN(ID) FROM retry_Pass)";
                DataTable d2 = SQL.ExecuteQuery(selectStr2);



                PassStn pass_Stn = new PassStn();


                pass_Stn.hashCode = Global.hashCode;
                pass_Stn.requestId = d2.Rows[0][6].ToString();
                pass_Stn.resv1 = "";
                pass_Stn.productInfo = new ProductInfo();
                pass_Stn.productInfo.barCode = d2.Rows[0][1].ToString();
                pass_Stn.productInfo.barCodeType = Global.barCodeType; //"LC";
                pass_Stn.productInfo.station = Global.station; //"LCHWelding";
                pass_Stn.productInfo.billNo = Global.billNo;


                pass_Stn.productInfo.bindCode = new BindCode1[4];
                pass_Stn.productInfo.bindCode[0] = new BindCode1();
                pass_Stn.productInfo.bindCode[1] = new BindCode1();
                pass_Stn.productInfo.bindCode[2] = new BindCode1();
                pass_Stn.productInfo.bindCode[3] = new BindCode1();

                pass_Stn.productInfo.bindCode[0].codeSn = d2.Rows[0][2].ToString();
                pass_Stn.productInfo.bindCode[0].codeSnType = "IQC_LEFT_RAIL";
                pass_Stn.productInfo.bindCode[0].replace = "1";
                pass_Stn.productInfo.bindCode[1].codeSn = d2.Rows[0][3].ToString();
                pass_Stn.productInfo.bindCode[1].codeSnType = "IQC_RIGHT_RAIL";
                pass_Stn.productInfo.bindCode[1].replace = "1";
                pass_Stn.productInfo.bindCode[2].codeSn = d2.Rows[0][4].ToString();
                pass_Stn.productInfo.bindCode[2].codeSnType = "FIXTURE";
                pass_Stn.productInfo.bindCode[2].replace = "1";
                pass_Stn.productInfo.bindCode[3].codeSn = Global.equipmentNo; //"MO0093730001-10";
                pass_Stn.productInfo.bindCode[3].codeSnType = "EQUIPMENT";
                pass_Stn.productInfo.bindCode[3].replace = "1";

                pass_Stn.equipmentInfo = new EquipmentInfo();
                pass_Stn.equipmentInfo.equipmentIp = Global.Trace_ip;
                pass_Stn.equipmentInfo.equipmentType = "L";
                pass_Stn.equipmentInfo.equipmentNo = Global.equipmentNo; //"MO0093730001-10";
                pass_Stn.equipmentInfo.vendorId = "HG";
                pass_Stn.equipmentInfo.processRevs = Global.version;

                pass_Stn.recipeInfo = new RecipeInfo();
                pass_Stn.recipeInfo.startTime = DateTime.Now.AddSeconds(-1).ToString("yyyyMMddHHmmss");
                pass_Stn.recipeInfo.endTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                pass_Stn.recipeInfo.cavity = "05";
                pass_Stn.recipeInfo.judgement = "PASS";
                pass_Stn.recipeInfo.humidity = "63";
                pass_Stn.recipeInfo.temperature = "35";
                pass_Stn.recipeInfo.paraInfo = new ParaInfo[13];
                for (int i = 0; i < 13; i++)
                {

                    pass_Stn.recipeInfo.paraInfo[i] = new ParaInfo();
                    pass_Stn.recipeInfo.paraInfo[i].sTime = DateTime.Now.AddSeconds(-12).ToString("yyyyMMddHHmmss");
                    pass_Stn.recipeInfo.paraInfo[i].eTime = DateTime.Now.AddSeconds(-1).ToString("yyyyMMddHHmmss");
                    switch (i)
                    {
                        case 0:
                            pass_Stn.recipeInfo.paraInfo[i].area = "RM2";
                            break;
                        case 1:
                            pass_Stn.recipeInfo.paraInfo[i].area = "LM1";
                            break;
                        case 2:
                            pass_Stn.recipeInfo.paraInfo[i].area = "LT";
                            break;
                        case 3:
                            pass_Stn.recipeInfo.paraInfo[i].area = "AB1";
                            break;
                        case 4:
                            pass_Stn.recipeInfo.paraInfo[i].area = "LM2";
                            break;
                        case 5:
                            pass_Stn.recipeInfo.paraInfo[i].area = "LB";
                            break;
                        case 6:
                            pass_Stn.recipeInfo.paraInfo[i].area = "AB2";
                            break;
                        case 7:
                            pass_Stn.recipeInfo.paraInfo[i].area = "RM1";
                            break;
                        case 8:
                            pass_Stn.recipeInfo.paraInfo[i].area = "RT";
                            break;
                        case 9:
                            pass_Stn.recipeInfo.paraInfo[i].area = "AB3";
                            break;
                        case 10:
                            pass_Stn.recipeInfo.paraInfo[i].area = "RM3";
                            break;
                        case 11:
                            pass_Stn.recipeInfo.paraInfo[i].area = "RB";
                            break;
                        case 12:
                            pass_Stn.recipeInfo.paraInfo[i].area = "AB4";
                            break;
                        default:
                            break;
                    }

                }
                //pass_Stn.recipeInfo.paraInfo[0] = new ParaInfo();
                //pass_Stn.recipeInfo.paraInfo[1] = new ParaInfo();
                //pass_Stn.recipeInfo.paraInfo[0].area = "RM2";
                //pass_Stn.recipeInfo.paraInfo[0].sTime = DateTime.Now.AddSeconds(-12).ToString("yyyyMMddHHmmss");
                //pass_Stn.recipeInfo.paraInfo[0].eTime = DateTime.Now.AddSeconds(-1).ToString("yyyyMMddHHmmss");
                //pass_Stn.recipeInfo.paraInfo[1].area = "LM1";
                //pass_Stn.recipeInfo.paraInfo[1].sTime = DateTime.Now.AddSeconds(-12).ToString("yyyyMMddHHmmss");
                //pass_Stn.recipeInfo.paraInfo[1].eTime = DateTime.Now.AddSeconds(-1).ToString("yyyyMMddHHmmss");

                pass_Stn.appleReturnInfo = new AppleReturnInfo();
                pass_Stn.appleReturnInfo.status = "200";
                pass_Stn.appleReturnInfo.id = lstn_info[d2.Rows[0][1].ToString()].Lstn_Traceid;
                pass_Stn.appleReturnInfo.contact = "Vendor";
                pass_Stn.appleReturnInfo.error = "";


                string send_Pass = JsonConvert.SerializeObject(pass_Stn, Formatting.None, jsetting);
                Log.WriteLog("过站重传发送:" + send_Pass);

                RequestAPI3.CallBobcat2(Global.passURL, send_Pass, Global.hashCode, out callResult, out errMsg);
                Log.WriteLog("过站重传接收:" + callResult);

                JObject recvObj = JsonConvert.DeserializeObject<JObject>(callResult);

                if (recvObj["rc"].ToString() == "000")
                {
                    Log.WriteLog(d2.Rows[0][1].ToString() + "：过站重传OK!");
                    lstn_info.Remove(d2.Rows[0][1].ToString());
                    return true;
                }
                else
                {
                    Log.WriteLog(d2.Rows[0][1].ToString() + "：过站重传NG!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("过站异常" + ex.ToString());
                return false;
            }
        }

        #endregion

        #region 产能统计
        private void EthPolling_Data()//产能统计 DT统计
        {
            while (true)
            {
                try
                {
                    Product_DataStatistics();//产能统计
                    DT_DataStatistics();//运行状态统计                    
                }
                catch (Exception ex)
                {
                    Log.WriteLog("PC与PLC通讯异常：" + ex.ToString().Replace("\n", ""));
                    ConnectPLC = false;
                }
                Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// 把int32类型的数据转存到4个字节的byte数组中
        /// </summary>
        /// <param name="m">int32类型的数据</param>
        /// <param name="arry">4个字节大小的byte数组</param>
        /// <returns></returns>
        static bool ConvertIntToByteArray(Int32 m, ref byte[] arry)
        {
            if (arry == null) return false;
            if (arry.Length < 4) return false;

            arry[2] = (byte)(m & 0xFF);
            arry[3] = (byte)((m & 0xFF00) >> 8);
            arry[0] = (byte)((m & 0xFF0000) >> 16);
            arry[1] = (byte)((m >> 24) & 0xFF);

            return true;
        }

        private short[] convertInt32ToShort(Int32[] data)
        {
            float[] data1 = new float[data.Length];
            short[] data_short = new short[data.Length * 2];
            for (int i = 0; i < data.Length; i++)
            {
                byte[] buf = new byte[4];
                bool ok = ConvertIntToByteArray(data[i], ref buf);
                data1[i] = BitConverter.ToSingle(buf, 0);
                data_short[i * 2] = Convert.ToInt16(data1[i]);
            }
            return data_short;          
        }

        private short convertInt32ToShort(Int32 data)
        {
            float data1;
            short data_short;
            byte[] buf = new byte[4];
            bool ok = ConvertIntToByteArray(data, ref buf);
            data1 = BitConverter.ToSingle(buf, 0);
            data_short = Convert.ToInt16(data1);
            return data_short;
        }

        private Int32 convertInt32ToInt(Int32 data)
        {
            float data1;
            Int32 data_short;
            byte[] buf = new byte[4];
            bool ok = ConvertIntToByteArray(data, ref buf);
            data1 = BitConverter.ToSingle(buf, 0);
            data_short = Convert.ToInt32(data1);
            return data_short;
        }


        private void Product_DataStatistics()//产能数据统计
        {
            //-------------------------------白班产能统计--------------------------------------------
            //Global.Product_Total = Global.plc1.ReadInt16("MW34200", 24).Content;
            //Global.Product_NG = convertInt32ToShort(Global.plc1.ReadInt32("MW34230", 12).Content);//白班NG产能
            //Global.Product_OK = convertInt32ToShort(Global.plc1.ReadInt32("MW34200", 12).Content);//白班OK产能

            //short Product_OK_08_20 = convertInt32ToShort(Global.plc1.ReadInt32("MW34102").Content);//白班OK总产能
            //short Product_NG_08_20 = convertInt32ToShort(Global.plc1.ReadInt32("MW34104").Content);//白班NG总产能
            //short Product_Total_08_20 = Convert.ToInt16(Product_OK_08_20 + Product_NG_08_20);

            //short Product_OK_20_08 = convertInt32ToShort(Global.plc1.ReadInt32("MW35102").Content);//夜班OK产能
            //short Product_NG_20_08 = convertInt32ToShort(Global.plc1.ReadInt32("MW35104").Content);//夜班NG总产能
            //short Product_Total_20_08 = Convert.ToInt16(Product_OK_20_08 + Product_NG_20_08);

            //short[] Product_Left_NG = convertInt32ToShort(Global.plc1.ReadInt32("MW30230", 12).Content);//白班左件扫码失败
            //short[] Product_Right_NG = convertInt32ToShort(Global.plc1.ReadInt32("MW30600", 12).Content);//白班右件扫码失败

            //short[] Product_Left_NG_N1 = convertInt32ToShort(Global.plc1.ReadInt32("MW31230", 12).Content);//夜班左件扫码失败
            //short[] Product_Right_NG_N1 = convertInt32ToShort(Global.plc1.ReadInt32("MW31600", 12).Content);//夜班右件扫码失败

            Global.Product_NG_Detail[3] = 0;
            Global.Product_NG_Detail[4] = 0;
            Global.Product_NG_Detail_N1[3] = 0;
            Global.Product_NG_Detail_N1[4] = 0;
            //for (int i = 0; i < 24; i++)
            //{
            //    Global.Product_NG_Detail[3] += Product_Left_NG[i];
            //    Global.Product_NG_Detail[4] += Product_Right_NG[i];
            //    Global.Product_NG_Detail_N1[3] += Product_Left_NG_N1[i];
            //    Global.Product_NG_Detail_N1[4] += Product_Right_NG_N1[i];
            //}

            //Global.Product_NG_Detail[0] = Product_Total_08_20;
            //Global.Product_NG_Detail[1] = convertInt32ToShort(Global.plc1.ReadInt32("MW36104").Content);//白班1#机台上料确认失败/焊接失败
            //Global.Product_NG_Detail[2] = convertInt32ToShort(Global.plc1.ReadInt32("MW38104").Content);//白班2#机台上料确认失败/焊接失败
            ////Global.Product_NG_Detail[3] = convertInt32ToShort(Global.plc1.ReadInt32("MW30104").Content);//白班左件扫码失败
            ////Global.Product_NG_Detail[4] = convertInt32ToShort(Global.plc1.ReadInt32("MW30104").Content);//白班右件扫码失败
            //Global.Product_NG_Detail[5] = convertInt32ToShort(Global.plc1.ReadInt32("MW32104").Content);//白班LC扫码失败

            //Global.Product_NG_Detail_N1[0] = Product_Total_20_08;
            //Global.Product_NG_Detail_N1[1] = convertInt32ToShort(Global.plc1.ReadInt32("MW37104").Content);//夜班1#机台上料确认失败/焊接失败
            //Global.Product_NG_Detail_N1[2] = convertInt32ToShort(Global.plc1.ReadInt32("MW39104").Content);//夜班2#机台上料确认失败/焊接失败
            ////Global.Product_NG_Detail_N1[3] = convertInt32ToShort(Global.plc1.ReadInt32("MW31104").Content);//夜班左件扫码失败
            ////Global.Product_NG_Detail_N1[4] = convertInt32ToShort(Global.plc1.ReadInt32("MW31104").Content);//夜班右件扫码失败
            //Global.Product_NG_Detail_N1[5] = convertInt32ToShort(Global.plc1.ReadInt32("MW33104").Content);//夜班LC扫码失败

            short Product_Total_08_09 = Convert.ToInt16(Global.Product_NG[0] + Global.Product_OK[0]);//白班8-9点总产能
            short Product_Total_09_10 = Convert.ToInt16(Global.Product_NG[2] + Global.Product_OK[2]);
            short Product_Total_10_11 = Convert.ToInt16(Global.Product_NG[4] + Global.Product_OK[4]);
            short Product_Total_11_12 = Convert.ToInt16(Global.Product_NG[6] + Global.Product_OK[6]);
            short Product_Total_12_13 = Convert.ToInt16(Global.Product_NG[8] + Global.Product_OK[8]);
            short Product_Total_13_14 = Convert.ToInt16(Global.Product_NG[10] + Global.Product_OK[10]);
            short Product_Total_14_15 = Convert.ToInt16(Global.Product_NG[12] + Global.Product_OK[12]);
            short Product_Total_15_16 = Convert.ToInt16(Global.Product_NG[14] + Global.Product_OK[14]);
            short Product_Total_16_17 = Convert.ToInt16(Global.Product_NG[16] + Global.Product_OK[16]);
            short Product_Total_17_18 = Convert.ToInt16(Global.Product_NG[18] + Global.Product_OK[18]);
            short Product_Total_18_19 = Convert.ToInt16(Global.Product_NG[20] + Global.Product_OK[20]);
            short Product_Total_19_20 = Convert.ToInt16(Global.Product_NG[22] + Global.Product_OK[22]);//白班19-20点总产能

            short Product_NG_08_09 = Global.Product_NG[0];//白班8-9点NG产能
            short Product_NG_09_10 = Global.Product_NG[2];
            short Product_NG_10_11 = Global.Product_NG[4];
            short Product_NG_11_12 = Global.Product_NG[6];
            short Product_NG_12_13 = Global.Product_NG[8];
            short Product_NG_13_14 = Global.Product_NG[10];
            short Product_NG_14_15 = Global.Product_NG[12];
            short Product_NG_15_16 = Global.Product_NG[14];
            short Product_NG_16_17 = Global.Product_NG[16];
            short Product_NG_17_18 = Global.Product_NG[18];
            short Product_NG_18_19 = Global.Product_NG[20];
            short Product_NG_19_20 = Global.Product_NG[22];//白班19-20点NG产能

            if (Product_Total_08_09 == 0)
            {
                Product_Lianglv_08_09 = 0;
            }
            else
            {
                Product_Lianglv_08_09 = ((double)(Product_Total_08_09 - Product_NG_08_09) / (double)Product_Total_08_09) * 100;//白班8-9点良率
            }
            if (Product_Total_09_10 == 0)
            {
                Product_Lianglv_09_10 = 0;
            }
            else
            {
                Product_Lianglv_09_10 = ((double)(Product_Total_09_10 - Product_NG_09_10) / (double)Product_Total_09_10) * 100;
            }
            if (Product_Total_10_11 == 0)
            {
                Product_Lianglv_10_11 = 0;
            }
            else
            {
                Product_Lianglv_10_11 = ((double)(Product_Total_10_11 - Product_NG_10_11) / (double)Product_Total_10_11) * 100;
            }
            if (Product_Total_11_12 == 0)
            {
                Product_Lianglv_11_12 = 0;
            }
            else
            {
                Product_Lianglv_11_12 = ((double)(Product_Total_11_12 - Product_NG_11_12) / (double)Product_Total_11_12) * 100;
            }
            if (Product_Total_12_13 == 0)
            {
                Product_Lianglv_12_13 = 0;
            }
            else
            {
                Product_Lianglv_12_13 = ((double)(Product_Total_12_13 - Product_NG_12_13) / (double)Product_Total_12_13) * 100;
            }
            if (Product_Total_13_14 == 0)
            {
                Product_Lianglv_13_14 = 0;
            }
            else
            {
                Product_Lianglv_13_14 = ((double)(Product_Total_13_14 - Product_NG_13_14) / (double)Product_Total_13_14) * 100;
            }
            if (Product_Total_14_15 == 0)
            {
                Product_Lianglv_14_15 = 0;
            }
            else
            {
                Product_Lianglv_14_15 = ((double)(Product_Total_14_15 - Product_NG_14_15) / (double)Product_Total_14_15) * 100;
            }
            if (Product_Total_15_16 == 0)
            {
                Product_Lianglv_15_16 = 0;
            }
            else
            {
                Product_Lianglv_15_16 = ((double)(Product_Total_15_16 - Product_NG_15_16) / (double)Product_Total_15_16) * 100;
            }
            if (Product_Total_16_17 == 0)
            {
                Product_Lianglv_16_17 = 0;
            }
            else
            {
                Product_Lianglv_16_17 = ((double)(Product_Total_16_17 - Product_NG_16_17) / (double)Product_Total_16_17) * 100;
            }
            if (Product_Total_17_18 == 0)
            {
                Product_Lianglv_17_18 = 0;
            }
            else
            {
                Product_Lianglv_17_18 = ((double)(Product_Total_17_18 - Product_NG_17_18) / (double)Product_Total_17_18) * 100;
            }
            if (Product_Total_18_19 == 0)
            {
                Product_Lianglv_18_19 = 0;
            }
            else
            {
                Product_Lianglv_18_19 = ((double)(Product_Total_18_19 - Product_NG_18_19) / (double)Product_Total_18_19) * 100;
            }
            if (Product_Total_19_20 == 0)
            {
                Product_Lianglv_19_20 = 0;
            }
            else
            {
                Product_Lianglv_19_20 = ((double)(Product_Total_19_20 - Product_NG_19_20) / (double)Product_Total_19_20) * 100;//白班19-20点良率
            }

            //short Product_Total_08_20 = Global.plc1.ReadInt16("MW65001").Content;//白班总产能
           
            //if (Product_Total_08_20 == 0)
            //{
            //    Product_Lianglv_08_20 = 0;
            //}
            //else
            //{
            //    Product_Lianglv_08_20 = ((double)(Product_Total_08_20 - Product_NG_08_20) / (double)Product_Total_08_20) * 100;//白班总良率
            //}
            //Global.Product_Total_D = Product_Total_08_20;
            //Global.Product_OK_D = Product_Total_08_20 - Product_NG_08_20;
            if (DateTime.Now.ToString("yyyy-MM-dd") == Global.SelectDateTime.ToString("yyyy-MM-dd"))
            {
                _datastatisticsfrm.UpDatalabel(Global.Product_OK[0].ToString(), "lb_Product_Total_08_09");
                _datastatisticsfrm.UpDatalabel(Global.Product_OK[2].ToString(), "lb_Product_Total_09_10");
                _datastatisticsfrm.UpDatalabel(Global.Product_OK[4].ToString(), "lb_Product_Total_10_11");
                _datastatisticsfrm.UpDatalabel(Global.Product_OK[6].ToString(), "lb_Product_Total_11_12");
                _datastatisticsfrm.UpDatalabel(Global.Product_OK[8].ToString(), "lb_Product_Total_12_13");
                _datastatisticsfrm.UpDatalabel(Global.Product_OK[10].ToString(), "lb_Product_Total_13_14");
                _datastatisticsfrm.UpDatalabel(Global.Product_OK[12].ToString(), "lb_Product_Total_14_15");
                _datastatisticsfrm.UpDatalabel(Global.Product_OK[14].ToString(), "lb_Product_Total_15_16");
                _datastatisticsfrm.UpDatalabel(Global.Product_OK[16].ToString(), "lb_Product_Total_16_17");
                _datastatisticsfrm.UpDatalabel(Global.Product_OK[18].ToString(), "lb_Product_Total_17_18");
                _datastatisticsfrm.UpDatalabel(Global.Product_OK[20].ToString(), "lb_Product_Total_18_19");
                _datastatisticsfrm.UpDatalabel(Global.Product_OK[22].ToString(), "lb_Product_Total_19_20");

                _datastatisticsfrm.UpDatalabel(Product_NG_08_09.ToString(), "lb_Product_NG_08_09");
                _datastatisticsfrm.UpDatalabel(Product_NG_09_10.ToString(), "lb_Product_NG_09_10");
                _datastatisticsfrm.UpDatalabel(Product_NG_10_11.ToString(), "lb_Product_NG_10_11");
                _datastatisticsfrm.UpDatalabel(Product_NG_11_12.ToString(), "lb_Product_NG_11_12");
                _datastatisticsfrm.UpDatalabel(Product_NG_12_13.ToString(), "lb_Product_NG_12_13");
                _datastatisticsfrm.UpDatalabel(Product_NG_13_14.ToString(), "lb_Product_NG_13_14");
                _datastatisticsfrm.UpDatalabel(Product_NG_14_15.ToString(), "lb_Product_NG_14_15");
                _datastatisticsfrm.UpDatalabel(Product_NG_15_16.ToString(), "lb_Product_NG_15_16");
                _datastatisticsfrm.UpDatalabel(Product_NG_16_17.ToString(), "lb_Product_NG_16_17");
                _datastatisticsfrm.UpDatalabel(Product_NG_17_18.ToString(), "lb_Product_NG_17_18");
                _datastatisticsfrm.UpDatalabel(Product_NG_18_19.ToString(), "lb_Product_NG_18_19");
                _datastatisticsfrm.UpDatalabel(Product_NG_19_20.ToString(), "lb_Product_NG_19_20");

                _datastatisticsfrm.UpDatalabel(Product_Lianglv_08_09.ToString("0.00") + "%", "lb_Product_Lianglv_08_09");
                _datastatisticsfrm.UpDatalabel(Product_Lianglv_09_10.ToString("0.00") + "%", "lb_Product_Lianglv_09_10");
                _datastatisticsfrm.UpDatalabel(Product_Lianglv_10_11.ToString("0.00") + "%", "lb_Product_Lianglv_10_11");
                _datastatisticsfrm.UpDatalabel(Product_Lianglv_11_12.ToString("0.00") + "%", "lb_Product_Lianglv_11_12");
                _datastatisticsfrm.UpDatalabel(Product_Lianglv_12_13.ToString("0.00") + "%", "lb_Product_Lianglv_12_13");
                _datastatisticsfrm.UpDatalabel(Product_Lianglv_13_14.ToString("0.00") + "%", "lb_Product_Lianglv_13_14");
                _datastatisticsfrm.UpDatalabel(Product_Lianglv_14_15.ToString("0.00") + "%", "lb_Product_Lianglv_14_15");
                _datastatisticsfrm.UpDatalabel(Product_Lianglv_15_16.ToString("0.00") + "%", "lb_Product_Lianglv_15_16");
                _datastatisticsfrm.UpDatalabel(Product_Lianglv_16_17.ToString("0.00") + "%", "lb_Product_Lianglv_16_17");
                _datastatisticsfrm.UpDatalabel(Product_Lianglv_17_18.ToString("0.00") + "%", "lb_Product_Lianglv_17_18");
                _datastatisticsfrm.UpDatalabel(Product_Lianglv_18_19.ToString("0.00") + "%", "lb_Product_Lianglv_18_19");
                _datastatisticsfrm.UpDatalabel(Product_Lianglv_19_20.ToString("0.00") + "%", "lb_Product_Lianglv_19_20");

                //_datastatisticsfrm.UpDatalabel(Product_Total_08_20.ToString(), "lb_Product_Total_08_20");
                //_datastatisticsfrm.UpDatalabel(Product_NG_08_20.ToString(), "lb_Product_NG_08_20");
                //_datastatisticsfrm.UpDatalabel(Product_Lianglv_08_20.ToString("0.00") + "%", "lb_Product_Lianglv_08_20");


                for (int i = 0; i < Global.Statistics_Index.Count; i++)
                {
                    if (i < 1)//前一行无比率
                    {
                        _datastatisticsfrm.UpDataDGV_D(Global.Statistics_Index[i], 1, Global.Product_NG_Detail[i].ToString());//投入数量
                    }
                    else
                    {
                        _datastatisticsfrm.UpDataDGV_D(Global.Statistics_Index[i], 1, Global.Product_NG_Detail[i].ToString());
                        double 比率 = (Convert.ToDouble(Global.Product_NG_Detail[i]) / Global.Product_NG_Detail[0]) * 100;
                        _datastatisticsfrm.UpDataDGV_D(Global.Statistics_Index[i], 2, 比率.ToString("0.00") + "%");
                    }
                }
            }

            //-------------------------------夜班产能统计--------------------------------------------
            //Global.Product_Total_N_1 = Global.plc1.ReadInt16("MW65001", 24).Content;//夜班产能1
            //Global.Product_Total_N_2 = Global.PLC_Client.ReadPLC_D(2024, 6);//夜班产能2
            //Global.Product_NG_N_1 = convertInt32ToShort(Global.plc1.ReadInt32("MW35230", 12).Content);//夜班NG产能1
            //Global.Product_OK_N_1 = convertInt32ToShort(Global.plc1.ReadInt32("MW35200", 12).Content);//夜班OK产能1
            //Global.Product_NG_N_2 = Global.PLC_Client.ReadPLC_D(2050, 6);//夜班NG产能2
            //Global.Product_OK_N_2 = Global.PLC_Client.ReadPLC_D(2000, 6);//夜班OK产能2

            short Product_Total_20_21 = Convert.ToInt16(Global.Product_NG_N_1[0] + Global.Product_OK_N_1[0]);//夜班8-9点总产能
            short Product_Total_21_22 = Convert.ToInt16(Global.Product_NG_N_1[2] + Global.Product_OK_N_1[2]);
            short Product_Total_22_23 = Convert.ToInt16(Global.Product_NG_N_1[4] + Global.Product_OK_N_1[4]);
            short Product_Total_23_00 = Convert.ToInt16(Global.Product_NG_N_1[6] + Global.Product_OK_N_1[6]);
            short Product_Total_00_01 = Convert.ToInt16(Global.Product_NG_N_1[8] + Global.Product_OK_N_1[8]);
            short Product_Total_01_02 = Convert.ToInt16(Global.Product_NG_N_1[10] + Global.Product_OK_N_1[10]);
            short Product_Total_02_03 = Convert.ToInt16(Global.Product_NG_N_1[12] + Global.Product_OK_N_1[12]);
            short Product_Total_03_04 = Convert.ToInt16(Global.Product_NG_N_1[14] + Global.Product_OK_N_1[14]);
            short Product_Total_04_05 = Convert.ToInt16(Global.Product_NG_N_1[16] + Global.Product_OK_N_1[16]);
            short Product_Total_05_06 = Convert.ToInt16(Global.Product_NG_N_1[18] + Global.Product_OK_N_1[18]);
            short Product_Total_06_07 = Convert.ToInt16(Global.Product_NG_N_1[20] + Global.Product_OK_N_1[20]);
            short Product_Total_07_08 = Convert.ToInt16(Global.Product_NG_N_1[22] + Global.Product_OK_N_1[22]);//夜班19-20点总产能
            short Product_NG_20_21 = Global.Product_NG_N_1[0];//夜班8-9点NG产能
            short Product_NG_21_22 = Global.Product_NG_N_1[2];
            short Product_NG_22_23 = Global.Product_NG_N_1[4];
            short Product_NG_23_00 = Global.Product_NG_N_1[6];
            short Product_NG_00_01 = Global.Product_NG_N_1[8];
            short Product_NG_01_02 = Global.Product_NG_N_1[10];
            short Product_NG_02_03 = Global.Product_NG_N_1[12];
            short Product_NG_03_04 = Global.Product_NG_N_1[14];
            short Product_NG_04_05 = Global.Product_NG_N_1[16];
            short Product_NG_05_06 = Global.Product_NG_N_1[18];
            short Product_NG_06_07 = Global.Product_NG_N_1[20];
            short Product_NG_07_08 = Global.Product_NG_N_1[22];//夜班19-20点NG产能

            if (Product_Total_20_21 == 0)
            {
                Product_Lianglv_20_21 = 0;
            }
            else
            {
                Product_Lianglv_20_21 = ((double)(Product_Total_20_21 - Product_NG_20_21) / (double)Product_Total_20_21) * 100;//夜班20-21点良率
            }
            if (Product_Total_21_22 == 0)
            {
                Product_Lianglv_21_22 = 0;
            }
            else
            {
                Product_Lianglv_21_22 = ((double)(Product_Total_21_22 - Product_NG_21_22) / (double)Product_Total_21_22) * 100;
            }
            if (Product_Total_22_23 == 0)
            {
                Product_Lianglv_22_23 = 0;
            }
            else
            {
                Product_Lianglv_22_23 = ((double)(Product_Total_22_23 - Product_NG_22_23) / (double)Product_Total_22_23) * 100;
            }
            if (Product_Total_23_00 == 0)
            {
                Product_Lianglv_23_00 = 0;
            }
            else
            {
                Product_Lianglv_23_00 = ((double)(Product_Total_23_00 - Product_NG_23_00) / (double)Product_Total_23_00) * 100;
            }
            if (Product_Total_00_01 == 0)
            {
                Product_Lianglv_00_01 = 0;
            }
            else
            {
                Product_Lianglv_00_01 = ((double)(Product_Total_00_01 - Product_NG_00_01) / (double)Product_Total_00_01) * 100;
            }
            if (Product_Total_01_02 == 0)
            {
                Product_Lianglv_01_02 = 0;
            }
            else
            {
                Product_Lianglv_01_02 = ((double)(Product_Total_01_02 - Product_NG_01_02) / (double)Product_Total_01_02) * 100;
            }
            if (Product_Total_02_03 == 0)
            {
                Product_Lianglv_02_03 = 0;
            }
            else
            {
                Product_Lianglv_02_03 = ((double)(Product_Total_02_03 - Product_NG_02_03) / (double)Product_Total_02_03) * 100;
            }
            if (Product_Total_03_04 == 0)
            {
                Product_Lianglv_03_04 = 0;
            }
            else
            {
                Product_Lianglv_03_04 = ((double)(Product_Total_03_04 - Product_NG_03_04) / (double)Product_Total_03_04) * 100;
            }
            if (Product_Total_04_05 == 0)
            {
                Product_Lianglv_04_05 = 0;
            }
            else
            {
                Product_Lianglv_04_05 = ((double)(Product_Total_04_05 - Product_NG_04_05) / (double)Product_Total_04_05) * 100;
            }
            if (Product_Total_05_06 == 0)
            {
                Product_Lianglv_05_06 = 0;
            }
            else
            {
                Product_Lianglv_05_06 = ((double)(Product_Total_05_06 - Product_NG_05_06) / (double)Product_Total_05_06) * 100;
            }
            if (Product_Total_06_07 == 0)
            {
                Product_Lianglv_06_07 = 0;
            }
            else
            {
                Product_Lianglv_06_07 = ((double)(Product_Total_06_07 - Product_NG_06_07) / (double)Product_Total_06_07) * 100;
            }
            if (Product_Total_07_08 == 0)
            {
                Product_Lianglv_07_08 = 0;
            }
            else
            {
                Product_Lianglv_07_08 = ((double)(Product_Total_07_08 - Product_NG_07_08) / (double)Product_Total_07_08) * 100;//夜班07-08点良率
            }

            //short Product_Total_20_08 = Global.plc1.ReadInt16("MW65001").Content;//夜班总产能
            
            //if (Product_Total_20_08 == 0)
            //{
            //    Product_Lianglv_20_08 = 0;
            //}
            //else
            //{
            //    Product_Lianglv_20_08 = ((double)(Product_Total_20_08 - Product_NG_20_08) / (double)Product_Total_20_08) * 100;//夜班总良率
            //}
            //Global.Product_Total_N = Product_Total_20_08;
            //Global.Product_OK_N = Product_Total_20_08 - Product_NG_20_08;
            if (DateTime.Now.ToString("yyyy-MM-dd") == Global.SelectDateTime.ToString("yyyy-MM-dd"))
            {
                if (Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("20:00")) >= 0 || Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("8:00")) < 0)
                {
                    _datastatisticsfrm.UpDatalabel(Global.Product_OK_N_1[0].ToString(), "lb_Product_Total_20_21");
                    _datastatisticsfrm.UpDatalabel(Global.Product_OK_N_1[2].ToString(), "lb_Product_Total_21_22");
                    _datastatisticsfrm.UpDatalabel(Global.Product_OK_N_1[4].ToString(), "lb_Product_Total_22_23");
                    _datastatisticsfrm.UpDatalabel(Global.Product_OK_N_1[6].ToString(), "lb_Product_Total_23_00");
                    _datastatisticsfrm.UpDatalabel(Global.Product_OK_N_1[8].ToString(), "lb_Product_Total_00_01");
                    _datastatisticsfrm.UpDatalabel(Global.Product_OK_N_1[10].ToString(), "lb_Product_Total_01_02");
                    _datastatisticsfrm.UpDatalabel(Global.Product_OK_N_1[12].ToString(), "lb_Product_Total_02_03");
                    _datastatisticsfrm.UpDatalabel(Global.Product_OK_N_1[14].ToString(), "lb_Product_Total_03_04");
                    _datastatisticsfrm.UpDatalabel(Global.Product_OK_N_1[16].ToString(), "lb_Product_Total_04_05");
                    _datastatisticsfrm.UpDatalabel(Global.Product_OK_N_1[18].ToString(), "lb_Product_Total_05_06");
                    _datastatisticsfrm.UpDatalabel(Global.Product_OK_N_1[20].ToString(), "lb_Product_Total_06_07");
                    _datastatisticsfrm.UpDatalabel(Global.Product_OK_N_1[22].ToString(), "lb_Product_Total_07_08");

                    _datastatisticsfrm.UpDatalabel(Product_NG_20_21.ToString(), "lb_Product_NG_20_21");
                    _datastatisticsfrm.UpDatalabel(Product_NG_21_22.ToString(), "lb_Product_NG_21_22");
                    _datastatisticsfrm.UpDatalabel(Product_NG_22_23.ToString(), "lb_Product_NG_22_23");
                    _datastatisticsfrm.UpDatalabel(Product_NG_23_00.ToString(), "lb_Product_NG_23_00");
                    _datastatisticsfrm.UpDatalabel(Product_NG_00_01.ToString(), "lb_Product_NG_00_01");
                    _datastatisticsfrm.UpDatalabel(Product_NG_01_02.ToString(), "lb_Product_NG_01_02");
                    _datastatisticsfrm.UpDatalabel(Product_NG_02_03.ToString(), "lb_Product_NG_02_03");
                    _datastatisticsfrm.UpDatalabel(Product_NG_03_04.ToString(), "lb_Product_NG_03_04");
                    _datastatisticsfrm.UpDatalabel(Product_NG_04_05.ToString(), "lb_Product_NG_04_05");
                    _datastatisticsfrm.UpDatalabel(Product_NG_05_06.ToString(), "lb_Product_NG_05_06");
                    _datastatisticsfrm.UpDatalabel(Product_NG_06_07.ToString(), "lb_Product_NG_06_07");
                    _datastatisticsfrm.UpDatalabel(Product_NG_07_08.ToString(), "lb_Product_NG_07_08");

                    _datastatisticsfrm.UpDatalabel(Product_Lianglv_20_21.ToString("0.00") + "%", "lb_Product_Lianglv_20_21");
                    _datastatisticsfrm.UpDatalabel(Product_Lianglv_21_22.ToString("0.00") + "%", "lb_Product_Lianglv_21_22");
                    _datastatisticsfrm.UpDatalabel(Product_Lianglv_22_23.ToString("0.00") + "%", "lb_Product_Lianglv_22_23");
                    _datastatisticsfrm.UpDatalabel(Product_Lianglv_23_00.ToString("0.00") + "%", "lb_Product_Lianglv_23_00");
                    _datastatisticsfrm.UpDatalabel(Product_Lianglv_00_01.ToString("0.00") + "%", "lb_Product_Lianglv_00_01");
                    _datastatisticsfrm.UpDatalabel(Product_Lianglv_01_02.ToString("0.00") + "%", "lb_Product_Lianglv_01_02");
                    _datastatisticsfrm.UpDatalabel(Product_Lianglv_02_03.ToString("0.00") + "%", "lb_Product_Lianglv_02_03");
                    _datastatisticsfrm.UpDatalabel(Product_Lianglv_03_04.ToString("0.00") + "%", "lb_Product_Lianglv_03_04");
                    _datastatisticsfrm.UpDatalabel(Product_Lianglv_04_05.ToString("0.00") + "%", "lb_Product_Lianglv_04_05");
                    _datastatisticsfrm.UpDatalabel(Product_Lianglv_05_06.ToString("0.00") + "%", "lb_Product_Lianglv_05_06");
                    _datastatisticsfrm.UpDatalabel(Product_Lianglv_06_07.ToString("0.00") + "%", "lb_Product_Lianglv_06_07");
                    _datastatisticsfrm.UpDatalabel(Product_Lianglv_07_08.ToString("0.00") + "%", "lb_Product_Lianglv_07_08");

                    //_datastatisticsfrm.UpDatalabel(Product_Total_20_08.ToString(), "lb_Product_Total_20_08");
                    //_datastatisticsfrm.UpDatalabel(Product_NG_20_08.ToString(), "lb_Product_NG_20_08");
                    //_datastatisticsfrm.UpDatalabel(Product_Lianglv_20_08.ToString("0.00") + "%", "lb_Product_Lianglv_20_08");

                    for (int i = 0; i < Global.Statistics_Index.Count; i++)
                    {
                        if (i < 1)//前一行无比率
                        {
                            _datastatisticsfrm.UpDataDGV_N(Global.Statistics_Index[i], 1, Global.Product_NG_Detail_N1[i].ToString());//投入数量
                        }
                        else
                        {
                            _datastatisticsfrm.UpDataDGV_N(Global.Statistics_Index[i], 1, Global.Product_NG_Detail_N1[i].ToString());
                            double 比率 = (Convert.ToDouble(Global.Product_NG_Detail_N1[i]) / Global.Product_NG_Detail_N1[0]) * 100;
                            _datastatisticsfrm.UpDataDGV_N(Global.Statistics_Index[i], 2, 比率.ToString("0.00") + "%");
                        }
                    }
                }
                else
                {
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_Total_20_21");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_Total_21_22");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_Total_22_23");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_Total_23_00");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_Total_00_01");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_Total_01_02");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_Total_02_03");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_Total_03_04");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_Total_04_05");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_Total_05_06");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_Total_06_07");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_Total_07_08");

                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_NG_20_21");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_NG_21_22");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_NG_22_23");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_NG_23_00");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_NG_00_01");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_NG_01_02");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_NG_02_03");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_NG_03_04");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_NG_04_05");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_NG_05_06");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_NG_06_07");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_NG_07_08");

                    _datastatisticsfrm.UpDatalabel("0.00%", "lb_Product_Lianglv_20_21");
                    _datastatisticsfrm.UpDatalabel("0.00%", "lb_Product_Lianglv_21_22");
                    _datastatisticsfrm.UpDatalabel("0.00%", "lb_Product_Lianglv_22_23");
                    _datastatisticsfrm.UpDatalabel("0.00%", "lb_Product_Lianglv_23_00");
                    _datastatisticsfrm.UpDatalabel("0.00%", "lb_Product_Lianglv_00_01");
                    _datastatisticsfrm.UpDatalabel("0.00%", "lb_Product_Lianglv_01_02");
                    _datastatisticsfrm.UpDatalabel("0.00%", "lb_Product_Lianglv_02_03");
                    _datastatisticsfrm.UpDatalabel("0.00%", "lb_Product_Lianglv_03_04");
                    _datastatisticsfrm.UpDatalabel("0.00%", "lb_Product_Lianglv_04_05");
                    _datastatisticsfrm.UpDatalabel("0.00%", "lb_Product_Lianglv_05_06");
                    _datastatisticsfrm.UpDatalabel("0.00%", "lb_Product_Lianglv_06_07");
                    _datastatisticsfrm.UpDatalabel("0.00%", "lb_Product_Lianglv_07_08");

                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_Total_20_08");
                    _datastatisticsfrm.UpDatalabel("0", "lb_Product_NG_20_08");
                    _datastatisticsfrm.UpDatalabel("0.00%", "lb_Product_Lianglv_20_08");


                    for (int i = 0; i < Global.Statistics_Index.Count; i++)
                    {
                        if (i < 1)//前一行无比率
                        {
                            _datastatisticsfrm.UpDataDGV_N(Global.Statistics_Index[i], 1, "0");//Spring&&Bracket投入数量
                        }
                        else
                        {
                            _datastatisticsfrm.UpDataDGV_N(Global.Statistics_Index[i], 1, "0");
                            //double 比率 = (Convert.ToDouble(Global.Product_NG_Detail_N1[i]) / Global.Product_NG_Detail_N1[0]) * 100;
                            _datastatisticsfrm.UpDataDGV_N(Global.Statistics_Index[i], 2, "0.00%");
                        }
                    }
                    //_datastatisticsfrm.UpDataDGV_N(0, 1, "0");
                    //_datastatisticsfrm.UpDataDGV_N(1, 1, "0");
                    //_datastatisticsfrm.UpDataDGV_N(4, 1, "0");
                    //_datastatisticsfrm.UpDataDGV_N(4, 2, "0.00%");
                    //_datastatisticsfrm.UpDataDGV_N(5, 1, "0");
                    //_datastatisticsfrm.UpDataDGV_N(5, 2, "0.00%");
                    //_datastatisticsfrm.UpDataDGV_N(6, 1, "0");
                    //_datastatisticsfrm.UpDataDGV_N(6, 2, "0.00%");
                    //_datastatisticsfrm.UpDataDGV_N(7, 1, "0");
                    //_datastatisticsfrm.UpDataDGV_N(7, 2, "0.00%");
                    //_datastatisticsfrm.UpDataDGV_N(8, 1, "0");
                    //_datastatisticsfrm.UpDataDGV_N(8, 2, "0.00%");
                    //_datastatisticsfrm.UpDataDGV_N(9, 1, "0");
                    //_datastatisticsfrm.UpDataDGV_N(9, 2, "0.00%");
                    //_datastatisticsfrm.UpDataDGV_N(12, 1, "0");
                    //_datastatisticsfrm.UpDataDGV_N(12, 2, "0.00%");
                    //_datastatisticsfrm.UpDataDGV_N(13, 1, "0");
                    //_datastatisticsfrm.UpDataDGV_N(13, 2, "0.00%");
                    //_datastatisticsfrm.UpDataDGV_N(14, 1, "0");
                    //_datastatisticsfrm.UpDataDGV_N(14, 2, "0.00%");
                    //_datastatisticsfrm.UpDataDGV_N(15, 1, "0");
                    //_datastatisticsfrm.UpDataDGV_N(15, 2, "0.00%");
                    //_datastatisticsfrm.UpDataDGV_N(16, 1, "0");
                    //_datastatisticsfrm.UpDataDGV_N(16, 2, "0.00%");
                    //_datastatisticsfrm.UpDataDGV_N(17, 1, "0");
                    //_datastatisticsfrm.UpDataDGV_N(17, 2, "0.00%");

                }
            }

        }

        private void DT_DataStatistics()//运行状态统计
        {
            //-------------------------------白班DT统计--------------------------------------------
            //Global.T_RunTime = convertInt32ToShort(Global.plc1.ReadInt32("MW30400", 12).Content);//白班运行时间        
            //for (int i = 0; i < Global.T_RunTime.Length; i++)
            //{
            //    Global.DT_RunTime[i] = Convert.ToDouble(Global.T_RunTime[i]) / Convert.ToDouble(60);
            //}
            //Global.T_ErrorTime = convertInt32ToShort(Global.plc1.ReadInt32("MW30430", 12).Content);//白班异常时间        
            //for (int i = 0; i < Global.T_ErrorTime.Length; i++)
            //{
            //    Global.DT_ErrorTime[i] = Convert.ToDouble(Global.T_ErrorTime[i]) / Convert.ToDouble(60);
            //}
            //Global.T_PendingTime = convertInt32ToShort(Global.plc1.ReadInt32("MW30460", 12).Content);//白班异常时间        
            //for (int i = 0; i < Global.T_PendingTime.Length; i++)
            //{
            //    Global.DT_PendingTime[i] = Convert.ToDouble(Global.T_PendingTime[i]) / Convert.ToDouble(60);
            //}
            double DT_RunTime_08_09 = Global.DT_RunTime[0];//白班8-9点运行时间
            double DT_RunTime_09_10 = Global.DT_RunTime[2];
            double DT_RunTime_10_11 = Global.DT_RunTime[4];
            double DT_RunTime_11_12 = Global.DT_RunTime[6];
            double DT_RunTime_12_13 = Global.DT_RunTime[8];
            double DT_RunTime_13_14 = Global.DT_RunTime[10];
            double DT_RunTime_14_15 = Global.DT_RunTime[12];
            double DT_RunTime_15_16 = Global.DT_RunTime[14];
            double DT_RunTime_16_17 = Global.DT_RunTime[16];
            double DT_RunTime_17_18 = Global.DT_RunTime[18];
            double DT_RunTime_18_19 = Global.DT_RunTime[20];
            double DT_RunTime_19_20 = Global.DT_RunTime[22];//白班19-20点运行时间
            double DT_ErrorTime_08_09 = Global.DT_ErrorTime[0];//白班8-9点异常时间
            double DT_ErrorTime_09_10 = Global.DT_ErrorTime[2];
            double DT_ErrorTime_10_11 = Global.DT_ErrorTime[4];
            double DT_ErrorTime_11_12 = Global.DT_ErrorTime[6];
            double DT_ErrorTime_12_13 = Global.DT_ErrorTime[8];
            double DT_ErrorTime_13_14 = Global.DT_ErrorTime[10];
            double DT_ErrorTime_14_15 = Global.DT_ErrorTime[12];
            double DT_ErrorTime_15_16 = Global.DT_ErrorTime[14];
            double DT_ErrorTime_16_17 = Global.DT_ErrorTime[16];
            double DT_ErrorTime_17_18 = Global.DT_ErrorTime[18];
            double DT_ErrorTime_18_19 = Global.DT_ErrorTime[20];
            double DT_ErrorTime_19_20 = Global.DT_ErrorTime[22];//白班19-20点异常时间
            double DT_PendingTime_08_09 = Global.DT_PendingTime[0];//白班8-9点待料时间
            double DT_PendingTime_09_10 = Global.DT_PendingTime[2];
            double DT_PendingTime_10_11 = Global.DT_PendingTime[4];
            double DT_PendingTime_11_12 = Global.DT_PendingTime[6];
            double DT_PendingTime_12_13 = Global.DT_PendingTime[8];
            double DT_PendingTime_13_14 = Global.DT_PendingTime[10];
            double DT_PendingTime_14_15 = Global.DT_PendingTime[12];
            double DT_PendingTime_15_16 = Global.DT_PendingTime[14];
            double DT_PendingTime_16_17 = Global.DT_PendingTime[16];
            double DT_PendingTime_17_18 = Global.DT_PendingTime[18];
            double DT_PendingTime_18_19 = Global.DT_PendingTime[20];
            double DT_PendingTime_19_20 = Global.DT_PendingTime[22];//白班19-20点待料时间
            //int T_RunTime_08_20 = convertInt32ToInt(Global.plc1.ReadInt32("MW30110").Content);//白班总运行时间
            //double DT_RunTime_08_20 = Convert.ToDouble(T_RunTime_08_20) / Convert.ToDouble(60);
            //int T_ErrorTime_08_20 = convertInt32ToInt(Global.plc1.ReadInt32("MW30112").Content);//白班总异常时间
            //double DT_ErrorTime_08_20 = Convert.ToDouble(T_ErrorTime_08_20) / Convert.ToDouble(60);
            //int T_PendingTime_08_20 = convertInt32ToInt(Global.plc1.ReadInt32("MW30114").Content);//白班总待料时间
            //double DT_PendingTime_08_20 = Convert.ToDouble(T_PendingTime_08_20) / Convert.ToDouble(60);
            //int DT_jiadonglv_08_20 = Global.plc1.ReadInt32("MW34116").Content;//白班稼动率时间
            if (DateTime.Now.ToString("yyyy-MM-dd") == Global.SelectDTTime.ToString("yyyy-MM-dd"))
            {
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_08_09)).ToString("0.0"), "lb_RunTime_08_09");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_09_10)).ToString("0.0"), "lb_RunTime_09_10");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_10_11)).ToString("0.0"), "lb_RunTime_10_11");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_11_12)).ToString("0.0"), "lb_RunTime_11_12");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_12_13)).ToString("0.0"), "lb_RunTime_12_13");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_13_14)).ToString("0.0"), "lb_RunTime_13_14");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_14_15)).ToString("0.0"), "lb_RunTime_14_15");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_15_16)).ToString("0.0"), "lb_RunTime_15_16");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_16_17)).ToString("0.0"), "lb_RunTime_16_17");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_17_18)).ToString("0.0"), "lb_RunTime_17_18");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_18_19)).ToString("0.0"), "lb_RunTime_18_19");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_19_20)).ToString("0.0"), "lb_RunTime_19_20");

                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_08_09)).ToString("0.0"), "lb_ErrorTime_08_09");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_09_10)).ToString("0.0"), "lb_ErrorTime_09_10");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_10_11)).ToString("0.0"), "lb_ErrorTime_10_11");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_11_12)).ToString("0.0"), "lb_ErrorTime_11_12");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_12_13)).ToString("0.0"), "lb_ErrorTime_12_13");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_13_14)).ToString("0.0"), "lb_ErrorTime_13_14");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_14_15)).ToString("0.0"), "lb_ErrorTime_14_15");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_15_16)).ToString("0.0"), "lb_ErrorTime_15_16");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_16_17)).ToString("0.0"), "lb_ErrorTime_16_17");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_17_18)).ToString("0.0"), "lb_ErrorTime_17_18");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_18_19)).ToString("0.0"), "lb_ErrorTime_18_19");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_19_20)).ToString("0.0"), "lb_ErrorTime_19_20");

                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_08_09)).ToString("0.0"), "lb_PendingTime_08_09");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_09_10)).ToString("0.0"), "lb_PendingTime_09_10");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_10_11)).ToString("0.0"), "lb_PendingTime_10_11");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_11_12)).ToString("0.0"), "lb_PendingTime_11_12");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_12_13)).ToString("0.0"), "lb_PendingTime_12_13");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_13_14)).ToString("0.0"), "lb_PendingTime_13_14");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_14_15)).ToString("0.0"), "lb_PendingTime_14_15");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_15_16)).ToString("0.0"), "lb_PendingTime_15_16");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_16_17)).ToString("0.0"), "lb_PendingTime_16_17");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_17_18)).ToString("0.0"), "lb_PendingTime_17_18");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_18_19)).ToString("0.0"), "lb_PendingTime_18_19");
                _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_19_20)).ToString("0.0"), "lb_PendingTime_19_20");

                //_datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_08_20)).ToString("0.0"), "lb_RunTime_08_20");
                //_datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_08_20)).ToString("0.0"), "lb_ErrorTime_08_20");
                //_datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_08_20)).ToString("0.0"), "lb_PendingTime_08_20");
                //_datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_jiadonglv_08_20) / 100).ToString("0.00") + "%", "lb_jiadonglv_08_20");
            }

            //-------------------------------夜班DT统计--------------------------------------------
            //Global.DT_RunTime_N2 = convertInt32ToShort(Global.plc1.ReadInt32("MW31400", 12).Content);//夜班运行时间        
            //for (int i = 0; i < Global.DT_RunTime_N2.Length; i++)
            //{
            //    Global.DT_RunTime_N1[i] = Convert.ToDouble(Global.DT_RunTime_N2[i]) / Convert.ToDouble(60);
            //}
            //Global.DT_ErrorTime_N2 = convertInt32ToShort(Global.plc1.ReadInt32("MW31430", 12).Content);//夜班异常时间        
            //for (int i = 0; i < Global.DT_ErrorTime_N2.Length; i++)
            //{
            //    Global.DT_ErrorTime_N1[i] = Convert.ToDouble(Global.DT_ErrorTime_N2[i]) / Convert.ToDouble(60);
            //}
            //Global.DT_PendingTime_N2 = convertInt32ToShort(Global.plc1.ReadInt32("MW31460", 12).Content);//夜班待料时间        
            //for (int i = 0; i < Global.DT_PendingTime_N2.Length; i++)
            //{
            //    Global.DT_PendingTime_N1[i] = Convert.ToDouble(Global.DT_PendingTime_N2[i]) / Convert.ToDouble(60);
            //}
            double DT_RunTime_20_21 = Global.DT_RunTime_N1[0];
            double DT_RunTime_21_22 = Global.DT_RunTime_N1[2];
            double DT_RunTime_22_23 = Global.DT_RunTime_N1[4];
            double DT_RunTime_23_00 = Global.DT_RunTime_N1[6];
            double DT_RunTime_00_01 = Global.DT_RunTime_N1[8];
            double DT_RunTime_01_02 = Global.DT_RunTime_N1[10];
            double DT_RunTime_02_03 = Global.DT_RunTime_N1[12];
            double DT_RunTime_03_04 = Global.DT_RunTime_N1[14];
            double DT_RunTime_04_05 = Global.DT_RunTime_N1[16];
            double DT_RunTime_05_06 = Global.DT_RunTime_N1[18];
            double DT_RunTime_06_07 = Global.DT_RunTime_N1[20];
            double DT_RunTime_07_08 = Global.DT_RunTime_N1[22];
            double DT_ErrorTime_20_21 = Global.DT_ErrorTime_N1[0];
            double DT_ErrorTime_21_22 = Global.DT_ErrorTime_N1[2];
            double DT_ErrorTime_22_23 = Global.DT_ErrorTime_N1[4];
            double DT_ErrorTime_23_00 = Global.DT_ErrorTime_N1[6];
            double DT_ErrorTime_00_01 = Global.DT_ErrorTime_N1[8];
            double DT_ErrorTime_01_02 = Global.DT_ErrorTime_N1[10];
            double DT_ErrorTime_02_03 = Global.DT_ErrorTime_N1[12];
            double DT_ErrorTime_03_04 = Global.DT_ErrorTime_N1[14];
            double DT_ErrorTime_04_05 = Global.DT_ErrorTime_N1[16];
            double DT_ErrorTime_05_06 = Global.DT_ErrorTime_N1[18];
            double DT_ErrorTime_06_07 = Global.DT_ErrorTime_N1[20];
            double DT_ErrorTime_07_08 = Global.DT_ErrorTime_N1[22];
            double DT_PendingTime_20_21 = Global.DT_PendingTime_N1[0];
            double DT_PendingTime_21_22 = Global.DT_PendingTime_N1[2];
            double DT_PendingTime_22_23 = Global.DT_PendingTime_N1[4];
            double DT_PendingTime_23_00 = Global.DT_PendingTime_N1[6];
            double DT_PendingTime_00_01 = Global.DT_PendingTime_N1[8];
            double DT_PendingTime_01_02 = Global.DT_PendingTime_N1[10];
            double DT_PendingTime_02_03 = Global.DT_PendingTime_N1[12];
            double DT_PendingTime_03_04 = Global.DT_PendingTime_N1[14];
            double DT_PendingTime_04_05 = Global.DT_PendingTime_N1[16];
            double DT_PendingTime_05_06 = Global.DT_PendingTime_N1[18];
            double DT_PendingTime_06_07 = Global.DT_PendingTime_N1[20];
            double DT_PendingTime_07_08 = Global.DT_PendingTime_N1[22];
            //int T_RunTime_20_08 = convertInt32ToInt(Global.plc1.ReadInt32("MW31110").Content);//夜班总运行时间
            //double DT_RunTime_20_08 = Convert.ToDouble(T_RunTime_20_08) / Convert.ToDouble(60);
            //int T_ErrorTime_20_08 = convertInt32ToInt(Global.plc1.ReadInt32("MW31112").Content);//夜班总异常时间
            //double DT_ErrorTime_20_08 = Convert.ToDouble(T_ErrorTime_20_08) / Convert.ToDouble(60);
            //int T_PendingTime_20_08 = convertInt32ToInt(Global.plc1.ReadInt32("MW31114").Content);//夜班总待料时间
            //double DT_PendingTime_20_08 = Convert.ToDouble(T_PendingTime_20_08) / Convert.ToDouble(60);
            //int DT_jiadonglv_20_08 = Global.plc1.ReadInt32("MW65001").Content;//夜班稼动率时间

            if (DateTime.Now.ToString("yyyy-MM-dd") == Global.SelectDTTime.ToString("yyyy-MM-dd"))
            {
                if (Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("20:00")) >= 0 || Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("8:00")) < 0)
                {
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_20_21)).ToString("0.0"), "lb_RunTime_20_21");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_21_22)).ToString("0.0"), "lb_RunTime_21_22");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_22_23)).ToString("0.0"), "lb_RunTime_22_23");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_23_00)).ToString("0.0"), "lb_RunTime_23_00");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_00_01)).ToString("0.0"), "lb_RunTime_00_01");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_01_02)).ToString("0.0"), "lb_RunTime_01_02");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_02_03)).ToString("0.0"), "lb_RunTime_02_03");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_03_04)).ToString("0.0"), "lb_RunTime_03_04");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_04_05)).ToString("0.0"), "lb_RunTime_04_05");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_05_06)).ToString("0.0"), "lb_RunTime_05_06");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_06_07)).ToString("0.0"), "lb_RunTime_06_07");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_07_08)).ToString("0.0"), "lb_RunTime_07_08");

                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_20_21)).ToString("0.0"), "lb_ErrorTime_20_21");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_21_22)).ToString("0.0"), "lb_ErrorTime_21_22");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_22_23)).ToString("0.0"), "lb_ErrorTime_22_23");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_23_00)).ToString("0.0"), "lb_ErrorTime_23_00");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_00_01)).ToString("0.0"), "lb_ErrorTime_00_01");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_01_02)).ToString("0.0"), "lb_ErrorTime_01_02");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_02_03)).ToString("0.0"), "lb_ErrorTime_02_03");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_03_04)).ToString("0.0"), "lb_ErrorTime_03_04");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_04_05)).ToString("0.0"), "lb_ErrorTime_04_05");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_05_06)).ToString("0.0"), "lb_ErrorTime_05_06");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_06_07)).ToString("0.0"), "lb_ErrorTime_06_07");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_07_08)).ToString("0.0"), "lb_ErrorTime_07_08");

                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_20_21)).ToString("0.0"), "lb_PendingTime_20_21");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_21_22)).ToString("0.0"), "lb_PendingTime_21_22");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_22_23)).ToString("0.0"), "lb_PendingTime_22_23");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_23_00)).ToString("0.0"), "lb_PendingTime_23_00");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_00_01)).ToString("0.0"), "lb_PendingTime_00_01");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_01_02)).ToString("0.0"), "lb_PendingTime_01_02");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_02_03)).ToString("0.0"), "lb_PendingTime_02_03");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_03_04)).ToString("0.0"), "lb_PendingTime_03_04");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_04_05)).ToString("0.0"), "lb_PendingTime_04_05");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_05_06)).ToString("0.0"), "lb_PendingTime_05_06");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_06_07)).ToString("0.0"), "lb_PendingTime_06_07");
                    _datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_07_08)).ToString("0.0"), "lb_PendingTime_07_08");

                    //_datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_RunTime_20_08)).ToString("0.0"), "lb_RunTime_20_08");
                    //_datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_ErrorTime_20_08)).ToString("0.0"), "lb_ErrorTime_20_08");
                    //_datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_PendingTime_20_08)).ToString("0.0"), "lb_PendingTime_20_08");
                    //_datastatisticsfrm.UpDatalabel((Convert.ToDouble(DT_jiadonglv_20_08) / 100).ToString("0.00") + "%", "lb_jiadonglv_20_08");
                }
                else
                {
                    _datastatisticsfrm.UpDatalabel("0", "lb_RunTime_20_21");
                    _datastatisticsfrm.UpDatalabel("0", "lb_RunTime_21_22");
                    _datastatisticsfrm.UpDatalabel("0", "lb_RunTime_22_23");
                    _datastatisticsfrm.UpDatalabel("0", "lb_RunTime_23_00");
                    _datastatisticsfrm.UpDatalabel("0", "lb_RunTime_00_01");
                    _datastatisticsfrm.UpDatalabel("0", "lb_RunTime_01_02");
                    _datastatisticsfrm.UpDatalabel("0", "lb_RunTime_02_03");
                    _datastatisticsfrm.UpDatalabel("0", "lb_RunTime_03_04");
                    _datastatisticsfrm.UpDatalabel("0", "lb_RunTime_04_05");
                    _datastatisticsfrm.UpDatalabel("0", "lb_RunTime_05_06");
                    _datastatisticsfrm.UpDatalabel("0", "lb_RunTime_06_07");
                    _datastatisticsfrm.UpDatalabel("0", "lb_RunTime_07_08");

                    _datastatisticsfrm.UpDatalabel("0", "lb_ErrorTime_20_21");
                    _datastatisticsfrm.UpDatalabel("0", "lb_ErrorTime_21_22");
                    _datastatisticsfrm.UpDatalabel("0", "lb_ErrorTime_22_23");
                    _datastatisticsfrm.UpDatalabel("0", "lb_ErrorTime_23_00");
                    _datastatisticsfrm.UpDatalabel("0", "lb_ErrorTime_00_01");
                    _datastatisticsfrm.UpDatalabel("0", "lb_ErrorTime_01_02");
                    _datastatisticsfrm.UpDatalabel("0", "lb_ErrorTime_02_03");
                    _datastatisticsfrm.UpDatalabel("0", "lb_ErrorTime_03_04");
                    _datastatisticsfrm.UpDatalabel("0", "lb_ErrorTime_04_05");
                    _datastatisticsfrm.UpDatalabel("0", "lb_ErrorTime_05_06");
                    _datastatisticsfrm.UpDatalabel("0", "lb_ErrorTime_06_07");
                    _datastatisticsfrm.UpDatalabel("0", "lb_ErrorTime_07_08");

                    _datastatisticsfrm.UpDatalabel("0", "lb_PendingTime_20_21");
                    _datastatisticsfrm.UpDatalabel("0", "lb_PendingTime_21_22");
                    _datastatisticsfrm.UpDatalabel("0", "lb_PendingTime_22_23");
                    _datastatisticsfrm.UpDatalabel("0", "lb_PendingTime_23_00");
                    _datastatisticsfrm.UpDatalabel("0", "lb_PendingTime_00_01");
                    _datastatisticsfrm.UpDatalabel("0", "lb_PendingTime_01_02");
                    _datastatisticsfrm.UpDatalabel("0", "lb_PendingTime_02_03");
                    _datastatisticsfrm.UpDatalabel("0", "lb_PendingTime_03_04");
                    _datastatisticsfrm.UpDatalabel("0", "lb_PendingTime_04_05");
                    _datastatisticsfrm.UpDatalabel("0", "lb_PendingTime_05_06");
                    _datastatisticsfrm.UpDatalabel("0", "lb_PendingTime_06_07");
                    _datastatisticsfrm.UpDatalabel("0", "lb_PendingTime_07_08");

                    _datastatisticsfrm.UpDatalabel("0", "lb_RunTime_20_08");
                    _datastatisticsfrm.UpDatalabel("0", "lb_ErrorTime_20_08");
                    _datastatisticsfrm.UpDatalabel("0", "lb_PendingTime_20_08");
                    //_datastatisticsfrm.UpDatalabel("0.00%", "lb_jiadonglv_20_08");
                }
            }

        }
        #endregion

        #region 断线重连
        //private void PLC_autolink(object ob)
        //{
        //    while (true)
        //    {
        //        if (Link_PLC == true && (Global.PLC_Client.client == null || !Global.PLC_Client.IsConnected))
        //        {
        //            //连接PLC
        //            try
        //            {
        //                Global.PLC_Client.sClient(Global.inidata.productconfig.Plc_IP, Global.inidata.productconfig.Plc_Port);
        //                Global.PLC_Client.Connect();
        //                Log.WriteLog("PLC通信已建立");
        //                //isopen = true;
        //            }
        //            catch
        //            {
        //                Log.WriteLog("PLC通信无法连接");
        //                Environment.Exit(1);
        //            }
        //        }
        //        Thread.Sleep(100);
        //    }
        //}
        #endregion

        #region Timer定时方法
        private void timer1_Tick(object sender, EventArgs e)
        {
            tsslabelcolor(tsslbl_time, Color.Black, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            switch (Global.Login)
            {
                case Global.LoginLevel.Operator:
                    tsslabelcolor(tsslbl_UserLogin, Color.Black, "当前用户：操作员");
                    //Btn_IfEnable(btn_Manual, false);
                    Btn_IfEnable(btn_Setting, false);
                    break;
                case Global.LoginLevel.Technician:
                    tsslabelcolor(tsslbl_UserLogin, Color.Black, "当前用户：技术员");
                    //Btn_IfEnable(btn_Manual, true);
                    Btn_IfEnable(btn_Setting, false);
                    break;
                case Global.LoginLevel.Administrator:
                    tsslabelcolor(tsslbl_UserLogin, Color.Black, "当前用户：工程师");
                    //Btn_IfEnable(btn_Manual, true);
                    Btn_IfEnable(btn_Setting, true);
                    break;
            }
        }
        #endregion

        #region 手动按钮
        private void btn_home_Click(object sender, EventArgs e)
        {
            btn_home.Image = Global.ReadImageFile(LogPath + "home1" + ".bmp");
            btn_DataStatistics.Image = Global.ReadImageFile(LogPath + "tu" + ".bmp");
            btn_IOMonitor.Image = Global.ReadImageFile(LogPath + "monitor" + ".bmp");
            btn_Manual.Image = Global.ReadImageFile(LogPath + "man" + ".bmp");
            btn_Setting.Image = Global.ReadImageFile(LogPath + "set" + ".bmp");
            btn_Abnormal.Image = Global.ReadImageFile(LogPath + "alarm" + ".bmp");
            btn_UserLogin.Image = Global.ReadImageFile(LogPath + "user" + ".bmp");
            btn_Help.Image = Global.ReadImageFile(LogPath + "help" + ".bmp");
            label_MachineName.BackColor = Color.White;
            if (!ExistsMdiChildrenInstance(_homefrm.Name))
            {
                this._homefrm.MdiParent = this;
                this._homefrm.Dock = DockStyle.Fill;
                this._homefrm.Show();
            }
            else
            {
                ShowView();
                this._homefrm.Activate();
            }
            //Cursor.Current = Cursors.Arrow;

        }

        private void btn_DataStatistics_Click(object sender, EventArgs e)
        {
            btn_DataStatistics.Image = Global.ReadImageFile(LogPath + "tu1" + ".bmp");
            btn_home.Image = Global.ReadImageFile(LogPath + "home" + ".bmp");
            btn_IOMonitor.Image = Global.ReadImageFile(LogPath + "monitor" + ".bmp");
            btn_Manual.Image = Global.ReadImageFile(LogPath + "man" + ".bmp");
            btn_Setting.Image = Global.ReadImageFile(LogPath + "set" + ".bmp");
            btn_Abnormal.Image = Global.ReadImageFile(LogPath + "alarm" + ".bmp");
            btn_UserLogin.Image = Global.ReadImageFile(LogPath + "user" + ".bmp");
            btn_Help.Image = Global.ReadImageFile(LogPath + "help" + ".bmp");
            label_MachineName.BackColor = Color.White;
            if (!ExistsMdiChildrenInstance(_datastatisticsfrm.Name))
            {
                this._datastatisticsfrm.MdiParent = this;
                this._datastatisticsfrm.Dock = DockStyle.Fill;
                this._datastatisticsfrm.Show();
            }
            else
            {
                _datastatisticsfrm.Show();
                this._datastatisticsfrm.Activate();
            }
        }

        private void btn_IOMonitor_Click(object sender, EventArgs e)
        {
            btn_IOMonitor.Image = Global.ReadImageFile(LogPath + "monitor1" + ".bmp");
            btn_DataStatistics.Image = Global.ReadImageFile(LogPath + "tu" + ".bmp");
            btn_home.Image = Global.ReadImageFile(LogPath + "home" + ".bmp");
            btn_Manual.Image = Global.ReadImageFile(LogPath + "man" + ".bmp");
            btn_Setting.Image = Global.ReadImageFile(LogPath + "set" + ".bmp");
            btn_Abnormal.Image = Global.ReadImageFile(LogPath + "alarm" + ".bmp");
            btn_UserLogin.Image = Global.ReadImageFile(LogPath + "user" + ".bmp");
            btn_Help.Image = Global.ReadImageFile(LogPath + "help" + ".bmp");
            label_MachineName.BackColor = Color.White;
            if (!ExistsMdiChildrenInstance(_iomonitorfrm.Name))
            {
                this._iomonitorfrm.MdiParent = this;
                this._iomonitorfrm.Dock = DockStyle.Fill;
                this._iomonitorfrm.Show();
            }
            else
            {
                _iomonitorfrm.Show();
                this._iomonitorfrm.Activate();
            }
        }

        private void btn_Manual_Click(object sender, EventArgs e)
        {
            btn_Manual.Image = Global.ReadImageFile(LogPath + "man1" + ".bmp");
            btn_IOMonitor.Image = Global.ReadImageFile(LogPath + "monitor" + ".bmp");
            btn_DataStatistics.Image = Global.ReadImageFile(LogPath + "tu" + ".bmp");
            btn_home.Image = Global.ReadImageFile(LogPath + "home" + ".bmp");
            btn_Setting.Image = Global.ReadImageFile(LogPath + "set" + ".bmp");
            btn_Abnormal.Image = Global.ReadImageFile(LogPath + "alarm" + ".bmp");
            btn_UserLogin.Image = Global.ReadImageFile(LogPath + "user" + ".bmp");
            btn_Help.Image = Global.ReadImageFile(LogPath + "help" + ".bmp");
            label_MachineName.BackColor = Color.White;
            if (!ExistsMdiChildrenInstance(_manualfrm.Name))
            {
                this._manualfrm.MdiParent = this;
                this._manualfrm.Dock = DockStyle.Fill;
                this._manualfrm.Show();
            }
            else
            {
                _manualfrm.Show();
                this._manualfrm.Activate();
            }
        }

        private void btn_Setting_Click(object sender, EventArgs e)
        {
            btn_Setting.Image = Global.ReadImageFile(LogPath + "set1" + ".bmp");
            btn_Manual.Image = Global.ReadImageFile(LogPath + "man" + ".bmp");
            btn_IOMonitor.Image = Global.ReadImageFile(LogPath + "monitor" + ".bmp");
            btn_DataStatistics.Image = Global.ReadImageFile(LogPath + "tu" + ".bmp");
            btn_home.Image = Global.ReadImageFile(LogPath + "home" + ".bmp");
            btn_Abnormal.Image = Global.ReadImageFile(LogPath + "alarm" + ".bmp");
            btn_UserLogin.Image = Global.ReadImageFile(LogPath + "user" + ".bmp");
            btn_Help.Image = Global.ReadImageFile(LogPath + "help" + ".bmp");
            label_MachineName.BackColor = Color.White;
            if (!ExistsMdiChildrenInstance(_sttingfrm.Name))
            {
                this._sttingfrm.MdiParent = this;
                this._sttingfrm.Dock = DockStyle.Fill;
                this._sttingfrm.Show();
            }
            else
            {
                _sttingfrm.Show();
                this._sttingfrm.Activate();
            }
        }

        private void btn_Abnormal_Click(object sender, EventArgs e)
        {
            btn_Abnormal.Image = Global.ReadImageFile(LogPath + "alarm1" + ".bmp");
            btn_Setting.Image = Global.ReadImageFile(LogPath + "set" + ".bmp");
            btn_Manual.Image = Global.ReadImageFile(LogPath + "man" + ".bmp");
            btn_IOMonitor.Image = Global.ReadImageFile(LogPath + "monitor" + ".bmp");
            btn_DataStatistics.Image = Global.ReadImageFile(LogPath + "tu" + ".bmp");
            btn_home.Image = Global.ReadImageFile(LogPath + "home" + ".bmp");
            btn_UserLogin.Image = Global.ReadImageFile(LogPath + "user" + ".bmp");
            btn_Help.Image = Global.ReadImageFile(LogPath + "help" + ".bmp");
            label_MachineName.BackColor = Color.White;
            if (!ExistsMdiChildrenInstance(_Abnormalfrm.Name))
            {
                this._Abnormalfrm.MdiParent = this;
                this._Abnormalfrm.Dock = DockStyle.Fill;
                this._Abnormalfrm.Show();
            }
            else
            {
                _Abnormalfrm.Show();
                this._Abnormalfrm.Activate();
            }
        }

        private void btn_UserLogin_Click(object sender, EventArgs e)
        {
            btn_UserLogin.Image = Global.ReadImageFile(LogPath + "user1" + ".bmp");
            btn_Abnormal.Image = Global.ReadImageFile(LogPath + "alarm" + ".bmp");
            btn_Setting.Image = Global.ReadImageFile(LogPath + "set" + ".bmp");
            btn_Manual.Image = Global.ReadImageFile(LogPath + "man" + ".bmp");
            btn_IOMonitor.Image = Global.ReadImageFile(LogPath + "monitor" + ".bmp");
            btn_DataStatistics.Image = Global.ReadImageFile(LogPath + "tu" + ".bmp");
            btn_home.Image = Global.ReadImageFile(LogPath + "home" + ".bmp");
            btn_Help.Image = Global.ReadImageFile(LogPath + "help" + ".bmp");
            label_MachineName.BackColor = Color.White;
            if (!ExistsMdiChildrenInstance(_userloginfrm.Name))
            {
                this._userloginfrm.MdiParent = this;
                this._userloginfrm.Dock = DockStyle.Fill;
                this._userloginfrm.Show();
            }
            else
            {
                _userloginfrm.Show();
                this._userloginfrm.Activate();
            }
        }

        private void btn_Help_Click(object sender, EventArgs e)
        {
            btn_Help.Image = Global.ReadImageFile(LogPath + "help1" + ".bmp");
            btn_UserLogin.Image = Global.ReadImageFile(LogPath + "user" + ".bmp");
            btn_Abnormal.Image = Global.ReadImageFile(LogPath + "alarm" + ".bmp");
            btn_Setting.Image = Global.ReadImageFile(LogPath + "set" + ".bmp");
            btn_Manual.Image = Global.ReadImageFile(LogPath + "man" + ".bmp");
            btn_IOMonitor.Image = Global.ReadImageFile(LogPath + "monitor" + ".bmp");
            btn_DataStatistics.Image = Global.ReadImageFile(LogPath + "tu" + ".bmp");
            btn_home.Image = Global.ReadImageFile(LogPath + "home" + ".bmp");
            label_MachineName.BackColor = Color.White;
            if (!ExistsMdiChildrenInstance(_helpfrm.Name))
            {
                this._helpfrm.MdiParent = this;
                this._helpfrm.Dock = DockStyle.Fill;
                this._helpfrm.Show();
            }
            else
            {
                _helpfrm.Show();
                this._helpfrm.Activate();
            }
        }

        private void label_MachineName_Click(object sender, EventArgs e)
        {
            label_MachineName.BackColor = Color.DarkSeaGreen;
            btn_Help.Image = Global.ReadImageFile(LogPath + "help" + ".bmp");
            btn_UserLogin.Image = Global.ReadImageFile(LogPath + "user" + ".bmp");
            btn_Abnormal.Image = Global.ReadImageFile(LogPath + "alarm" + ".bmp");
            btn_Setting.Image = Global.ReadImageFile(LogPath + "set" + ".bmp");
            btn_Manual.Image = Global.ReadImageFile(LogPath + "man" + ".bmp");
            btn_IOMonitor.Image = Global.ReadImageFile(LogPath + "monitor" + ".bmp");
            btn_DataStatistics.Image = Global.ReadImageFile(LogPath + "tu" + ".bmp");
            btn_home.Image = Global.ReadImageFile(LogPath + "home" + ".bmp");
            if (!ExistsMdiChildrenInstance(_machinefrm.Name))
            {
                this._machinefrm.MdiParent = this;
                this._machinefrm.Dock = DockStyle.Fill;
                this._machinefrm.Show();
            }
            else
            {
                _machinefrm.Show();
                this._machinefrm.Activate();
            }
        }

        public void ButtonFlag(bool Flag, Button bt)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new buttonflag(ButtonFlag), new object[] { Flag, bt });
                return;
            }
            bt.Enabled = Flag;
        }
        #endregion

        #region 委托显示
        public void Btn_IfEnable(ToolStripButton btn, bool b)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new btnEnable(Btn_IfEnable), new object[] { btn, b });
                return;
            }
            btn.Enabled = b;
        }

        public void tsslabelcolor(ToolStripStatusLabel lb, Color color, string str)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new tssLabelcolor(tsslabelcolor), new object[] { lb, color, str });
                return;
            }
            lb.ForeColor = color;
            lb.Text = str;
        }

        public void dgv_AutoSize(DataGridView dgv)//dgv表格自适应
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DGVAutoSize(dgv_AutoSize), new object[] { dgv });
                return;
            }
            int width = 0;
            //对于DataGridView的每一个列都调整
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                //将每一列都调整为自动适应模式
                dgv.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells);
                //记录整个DataGridView的宽度
                width += dgv.Columns[i].Width;
            }
            //判断调整后的宽度与原来设定的宽度的关系，如果是调整后的宽度大于原来设定的宽度，
            //则将DataGridView的列自动调整模式设置为显示的列即可，             //如果是小于原来设定的宽度，将模式改为填充。
            if (width > dgv.Size.Width)
            {
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            }
            else
            {
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            //设置表格字体居中
            dgv.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //设置表格列字体居中
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 12, FontStyle.Bold);
            dgv.RowsDefaultCellStyle.Font = new Font("微软雅黑", 9);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;//禁止列标题换行
        }


        public void ShowData(DataGridView dgv, DataTable dt, int index)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowDataTable(ShowData), new object[] { dt, index });
                return;
            }
            switch (index)
            {
                case 0:
                    dgv.DataSource = dt;
                    break;
                case 1:
                    break;
                default:
                    break;
            }
        }

        public void AppendRichText(string msg, RichTextBox richtextbox)
        {
            if (this.InvokeRequired)
            {

                this.BeginInvoke(new AddItemToRichTextBoxDelegate(AppendRichText), new object[] { msg, richtextbox });
                return;
            }

            richtextbox.AppendText(msg + "\r\n");

        }

        public void AppendText(ListBox listbox1, string msg, int index)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new AddItemToListBoxDelegate(AppendText), new object[] { listbox1, msg, index });
                return;
            }
            listbox1.SelectedItem = listbox1.Items.Count;
            switch (index)
            {
                case 0:
                    listbox1.Items.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + msg);
                    break;
                case 1:
                    listbox1.Items.Add(msg);
                    break;
            }
            listbox1.TopIndex = listbox1.Items.Count - 1;
        }

        //public void AppendRichText(string msg, RichTextBox richtextbox)
        //{
        //    if (this.InvokeRequired)
        //    {

        //        this.BeginInvoke(new AddItemToRichTextBoxDelegate(AppendRichText), new object[] { msg, richtextbox });
        //        return;
        //    }

        //    richtextbox.AppendText(msg + "\r\n");

        //}


        public void UiText(string str1, TextBox tb)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowTxt(UiText), new object[] { str1, tb });
                return;
            }
            tb.Text = str1;
        }

        public void labelcolor(Label lb, Color color, string str)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Labelcolor(labelcolor), new object[] { lb, color, str });
                return;
            }
            lb.BackColor = color;
            lb.Text = str;
        }

        public void labelenvision(Label lb, string txt)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Labelvision(labelenvision), new object[] { lb, txt });
                return;
            }
            lb.Text = txt;
        }

        public void ShowStatus(string txt, Color color, int id)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowPlcStatue(ShowStatus), new object[] { txt, color, id });
                return;
            }
            switch (id)
            {
                case 0:
                    tssl_PLCStatus.Text = txt;
                    tssl_PLCStatus.BackColor = color;
                    break;
                case 1:
                    //tssl_PDCAStatus.Text = txt;
                    //tssl_PDCAStatus.BackColor = color;
                    break;
                case 2:
                    tssl_TraceStatus.Text = txt;
                    tssl_TraceStatus.BackColor = color;
                    break;
                case 3:
                    //tssl_OEEStatus.Text = txt;
                    //tssl_OEEStatus.BackColor = color;
                    break;
                case 4:
                    tssl_HansStatus1.Text = txt;
                    tssl_HansStatus1.BackColor = color;
                    break;
                case 5:
                    tssl_HansStatus2.Text = txt;
                    tssl_HansStatus2.BackColor = color;
                    break;
                case 6:
                    tssl_HansStatus3.Text = txt;
                    tssl_HansStatus3.BackColor = color;
                    break;
                case 7:
                    tssl_HansStatus4.Text = txt;
                    tssl_HansStatus4.BackColor = color;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region TCP/IP通讯 事件方法

        #endregion

        private bool ExistsMdiChildrenInstance(string mdiChildrenClassName)//检测子窗体是否存在
        {
            foreach (Form childForm in this.MdiChildren)
            {
                if (mdiChildrenClassName == childForm.Name)
                {
                    return true;
                }
            }
            return false;
        }

        private void ShowView()//窗体显示
        {
            _homefrm.MdiParent = this;
            _homefrm.Dock = DockStyle.Fill;
            _homefrm.Show();
        }

        private void Ping_ip(object ob) //PLC连线检测
        {
            while (true)
            {
                try
                {
                    //using (System.Net.NetworkInformation.Ping PingSender = new System.Net.NetworkInformation.Ping())
                    //{
                    //    PingOptions Options = new PingOptions();
                    //    Options.DontFragment = true;
                    //    string Data = "test";
                    //    byte[] DataBuffer = Encoding.ASCII.GetBytes(Data);
                    //    PingReply Reply = PingSender.Send(Global.inidata.productconfig.Plc_IP, 1000, DataBuffer, Options);
                    //    if (Reply.Status == IPStatus.Success)
                    //    {
                    //        Link_PLC = true;
                    //    }
                    //    else
                    //    {
                    //        Link_PLC = false;
                    //    }
                    //}
                }
                catch
                {
                    Log.WriteLog("Ping PLC IP ERROR!!!");
                }
                Thread.Sleep(5000);
            }
        }

        private void On_Time_doing(object ob)//按时做某事
        {
            while (true)
            {
                #region 产能与小料抛料数据
                //写入夜班产能与小料抛料数据
                if (DateTime.Now.Hour == 9 && DateTime.Now.Minute == 30 && DateTime.Now.Second == 0)
                {
                    int ValusIndex = 8;
                    string InsertStr1 = string.Format("UPDATE [Product] SET [Hour_13] = '{0}',[Hour_14] = '{1}',[Hour_15] = '{2}',[Hour_16] = '{3}',[Hour_17] = '{4}',[Hour_18]= '{5}',[Hour_19]= '{6}'"
                                + " ,[Hour_20]= '{7}',[Hour_21]= '{8}',[Hour_22] = '{9}',[Hour_23] = '{10}',[Hour_24] = '{11}' where [DateTime] = '{12}' and [Status] = '{13}'",
                                 Global.Product_OK_N_1[0], Global.Product_OK_N_1[2], Global.Product_OK_N_1[4], Global.Product_OK_N_1[6], Global.Product_OK_N_1[8], Global.Product_OK_N_1[10], Global.Product_OK_N_1[12],
                                 Global.Product_OK_N_1[14], Global.Product_OK_N_1[16], Global.Product_OK_N_1[18], Global.Product_OK_N_1[20], Global.Product_OK_N_1[22], DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), "OK产量");
                    SQL.ExecuteUpdate(InsertStr1);
                    Log.WriteLog("写入夜班OK产能数据" + InsertStr1);

                    string InsertStr2 = string.Format("UPDATE [Product] SET [Hour_13] = '{0}',[Hour_14] = '{1}',[Hour_15] = '{2}',[Hour_16] = '{3}',[Hour_17] = '{4}',[Hour_18]= '{5}',[Hour_19]= '{6}'"
                                + " ,[Hour_20]= '{7}',[Hour_21]= '{8}',[Hour_22] = '{9}',[Hour_23] = '{10}',[Hour_24] = '{11}' where [DateTime] = '{12}' and [Status] = '{13}'",
                                 Global.Product_NG_N_1[0], Global.Product_NG_N_1[2], Global.Product_NG_N_1[4], Global.Product_NG_N_1[6], Global.Product_NG_N_1[8], Global.Product_NG_N_1[10], Global.Product_NG_N_1[12],
                                 Global.Product_NG_N_1[14], Global.Product_NG_N_1[16], Global.Product_NG_N_1[18], Global.Product_NG_N_1[20], Global.Product_NG_N_1[22], DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), "NG产量");
                    SQL.ExecuteUpdate(InsertStr2);
                    Log.WriteLog("写入夜班NG产能数据" + InsertStr2);

                    string InsertStr3 = string.Format("UPDATE [HourDT] SET [Hour_DT13] = '{0}',[Hour_DT14] = '{1}',[Hour_DT15] = '{2}',[Hour_DT16] = '{3}',[Hour_DT17] = '{4}',[Hour_DT18]= '{5}',[Hour_DT19]= '{6}'"
                                + " ,[Hour_DT20]= '{7}',[Hour_DT21]= '{8}',[Hour_DT22] = '{9}',[Hour_DT23] = '{10}',[Hour_DT24] = '{11}' where [DateTime] = '{12}' and [Status] = '{13}'",
                                  Global.DT_RunTime_N1[0].ToString("0.00"), Global.DT_RunTime_N1[2].ToString("0.00"), Global.DT_RunTime_N1[4].ToString("0.00"),
                                  Global.DT_RunTime_N1[6].ToString("0.00"), Global.DT_RunTime_N1[8].ToString("0.00"), Global.DT_RunTime_N1[10].ToString("0.00"),
                                  Global.DT_RunTime_N1[12].ToString("0.00"), Global.DT_RunTime_N1[14].ToString("0.00"), Global.DT_RunTime_N1[16].ToString("0.00"),
                                  Global.DT_RunTime_N1[18].ToString("0.00"), Global.DT_RunTime_N1[20].ToString("0.00"), Global.DT_RunTime_N1[22].ToString("0.00"),
                                 DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), "运行时间");
                    SQL.ExecuteUpdate(InsertStr3);
                    Log.WriteLog("写入夜班运行时间" + InsertStr3);

                    string InsertStr4 = string.Format("UPDATE [HourDT] SET [Hour_DT13] = '{0}',[Hour_DT14] = '{1}',[Hour_DT15] = '{2}',[Hour_DT16] = '{3}',[Hour_DT17] = '{4}',[Hour_DT18]= '{5}',[Hour_DT19]= '{6}'"
                                + " ,[Hour_DT20]= '{7}',[Hour_DT21]= '{8}',[Hour_DT22] = '{9}',[Hour_DT23] = '{10}',[Hour_DT24] = '{11}' where [DateTime] = '{12}' and [Status] = '{13}'",
                                  Global.DT_ErrorTime_N1[0].ToString("0.00"), Global.DT_ErrorTime_N1[2].ToString("0.00"), Global.DT_ErrorTime_N1[4].ToString("0.00"),
                                  Global.DT_ErrorTime_N1[6].ToString("0.00"), Global.DT_ErrorTime_N1[8].ToString("0.00"), Global.DT_ErrorTime_N1[10].ToString("0.00"),
                                  Global.DT_ErrorTime_N1[12].ToString("0.00"), Global.DT_ErrorTime_N1[14].ToString("0.00"), Global.DT_ErrorTime_N1[16].ToString("0.00"),
                                  Global.DT_ErrorTime_N1[18].ToString("0.00"), Global.DT_ErrorTime_N1[20].ToString("0.00"), Global.DT_ErrorTime_N1[22].ToString("0.00"),
                                 DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), "异常时间");
                    SQL.ExecuteUpdate(InsertStr4);
                    Log.WriteLog("写入夜班异常时间" + InsertStr4);

                    string InsertStr5 = string.Format("UPDATE [HourDT] SET [Hour_DT13] = '{0}',[Hour_DT14] = '{1}',[Hour_DT15] = '{2}',[Hour_DT16] = '{3}',[Hour_DT17] = '{4}',[Hour_DT18]= '{5}',[Hour_DT19]= '{6}'"
                                + " ,[Hour_DT20]= '{7}',[Hour_DT21]= '{8}',[Hour_DT22] = '{9}',[Hour_DT23] = '{10}',[Hour_DT24] = '{11}' where [DateTime] = '{12}' and [Status] = '{13}'",
                                  Global.DT_PendingTime_N1[0].ToString("0.00"), Global.DT_PendingTime_N1[2].ToString("0.00"), Global.DT_PendingTime_N1[4].ToString("0.00"),
                                  Global.DT_PendingTime_N1[6].ToString("0.00"), Global.DT_PendingTime_N1[8].ToString("0.00"), Global.DT_PendingTime_N1[10].ToString("0.00"),
                                  Global.DT_PendingTime_N1[12].ToString("0.00"), Global.DT_PendingTime_N1[14].ToString("0.00"), Global.DT_PendingTime_N1[16].ToString("0.00"),
                                  Global.DT_PendingTime_N1[18].ToString("0.00"), Global.DT_PendingTime_N1[20].ToString("0.00"), Global.DT_PendingTime_N1[22].ToString("0.00"),
                                 DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), "待料时间");
                    SQL.ExecuteUpdate(InsertStr5);
                    Log.WriteLog("写入夜班待料时间" + InsertStr5);

                    string InsertStr6 = string.Format("UPDATE [ErrorDataStatistics] SET [Product_Total_N] = '{0}',[Product_NG1_N] = '{1}',[Product_NG2_N] = '{2}',[Product_NG3_N] = '{3}',[Product_NG4_N] = '{4}',[Product_NG5_N] = '{5}'"
                        + "where DateTime = '{6}'",
                         Global.Product_NG_Detail_N1[0].ToString(), Global.Product_NG_Detail_N1[1].ToString(), Global.Product_NG_Detail_N1[2].ToString(), Global.Product_NG_Detail_N1[3].ToString(),
                         Global.Product_NG_Detail_N1[4].ToString(), Global.Product_NG_Detail_N1[5].ToString(), DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                    SQL.ExecuteUpdate(InsertStr6);
                    Log.WriteLog("写入夜班小料抛料数据" + InsertStr6);
                    Thread.Sleep(1000);
                }
                //写入白班产能与小料抛料数据
                if (DateTime.Now.Hour == 21 && DateTime.Now.Minute == 30 && DateTime.Now.Second == 0)
                {
                    int ValusIndex = 8;
                    string InsertStr1 = "insert into Product([DateTime],[Status],[Hour_1],[Hour_2],[Hour_3],[Hour_4],[Hour_5],"
                       + "[Hour_6],[Hour_7],[Hour_8],[Hour_9],[Hour_10],[Hour_11],[Hour_12]"
                       + ")" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'" + "," + "'" + "OK产量" + "'" + ","
                       + "'" + Global.Product_OK[0] + "'" + "," + "'" + Global.Product_OK[2] + "'" + "," + "'" + Global.Product_OK[4] + "'" + ","
                       + "'" + Global.Product_OK[6] + "'" + "," + "'" + Global.Product_OK[8] + "'" + "," + "'" + Global.Product_OK[10] + "'" + ","
                       + "'" + Global.Product_OK[12] + "'" + "," + "'" + Global.Product_OK[14] + "'" + "," + "'" + Global.Product_OK[16] + "'" + ","
                       + "'" + Global.Product_OK[18] + "'" + "," + "'" + Global.Product_OK[20] + "'" + "," + "'" + Global.Product_OK[22] + "'" + ")";
                    SQL.ExecuteUpdate(InsertStr1);
                    Log.WriteLog("写入白班OK产量数据" + InsertStr1);

                    string InsertStr2 = "insert into Product([DateTime],[Status],[Hour_1],[Hour_2],[Hour_3],[Hour_4],[Hour_5],"
                       + "[Hour_6],[Hour_7],[Hour_8],[Hour_9],[Hour_10],[Hour_11],[Hour_12]"
                       + ")" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'" + "," + "'" + "NG产量" + "'" + ","
                       + "'" + Global.Product_NG[0] + "'" + "," + "'" + Global.Product_NG[2] + "'" + "," + "'" + Global.Product_NG[4] + "'" + ","
                       + "'" + Global.Product_NG[6] + "'" + "," + "'" + Global.Product_NG[8] + "'" + "," + "'" + Global.Product_NG[10] + "'" + ","
                       + "'" + Global.Product_NG[12] + "'" + "," + "'" + Global.Product_NG[14] + "'" + "," + "'" + Global.Product_NG[16] + "'" + ","
                       + "'" + Global.Product_NG[18] + "'" + "," + "'" + Global.Product_NG[20] + "'" + "," + "'" + Global.Product_NG[22] + "'" + ")";
                    SQL.ExecuteUpdate(InsertStr2);
                    Log.WriteLog("写入白班NG产能数据" + InsertStr2);
                
                    string InsertStr3 = "insert into HourDT([DateTime],[Status],[Hour_DT1],[Hour_DT2],[Hour_DT3],[Hour_DT4],[Hour_DT5],"
                     + "[Hour_DT6],[Hour_DT7],[Hour_DT8],[Hour_DT9],[Hour_DT10],[Hour_DT11],[Hour_DT12]"
                     + ")" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'" + "," + "'" + "运行时间" + "'" + ","
                     + "'" + Global.DT_RunTime[0].ToString("0.00") + "'" + "," + "'" + Global.DT_RunTime[2].ToString("0.00") + "'" + "," + "'" + Global.DT_RunTime[4].ToString("0.00") + "'" + ","
                     + "'" + Global.DT_RunTime[6].ToString("0.00") + "'" + "," + "'" + Global.DT_RunTime[8].ToString("0.00") + "'" + "," + "'" + Global.DT_RunTime[10].ToString("0.00") + "'" + ","
                     + "'" + Global.DT_RunTime[12].ToString("0.00") + "'" + "," + "'" + Global.DT_RunTime[14].ToString("0.00") + "'" + "," + "'" + Global.DT_RunTime[16].ToString("0.00") + "'" + ","
                     + "'" + Global.DT_RunTime[18].ToString("0.00") + "'" + "," + "'" + Global.DT_RunTime[20].ToString("0.00") + "'" + "," + "'" + Global.DT_RunTime[22].ToString("0.00") + "'" + ")";
                    SQL.ExecuteUpdate(InsertStr3);
                    Log.WriteLog("写入白班运行时间数据" + InsertStr3);

                    string InsertStr4 = "insert into HourDT([DateTime],[Status],[Hour_DT1],[Hour_DT2],[Hour_DT3],[Hour_DT4],[Hour_DT5],"
                     + "[Hour_DT6],[Hour_DT7],[Hour_DT8],[Hour_DT9],[Hour_DT10],[Hour_DT11],[Hour_DT12]"
                     + ")" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'" + "," + "'" + "异常时间" + "'" + ","
                    + "'" + Global.DT_ErrorTime[0].ToString("0.00") + "'" + "," + "'" + Global.DT_ErrorTime[2].ToString("0.00") + "'" + "," + "'" + Global.DT_ErrorTime[4].ToString("0.00") + "'" + ","
                     + "'" + Global.DT_ErrorTime[6].ToString("0.00") + "'" + "," + "'" + Global.DT_ErrorTime[8].ToString("0.00") + "'" + "," + "'" + Global.DT_ErrorTime[10].ToString("0.00") + "'" + ","
                     + "'" + Global.DT_ErrorTime[12].ToString("0.00") + "'" + "," + "'" + Global.DT_ErrorTime[14].ToString("0.00") + "'" + "," + "'" + Global.DT_ErrorTime[16].ToString("0.00") + "'" + ","
                     + "'" + Global.DT_ErrorTime[18].ToString("0.00") + "'" + "," + "'" + Global.DT_ErrorTime[20].ToString("0.00") + "'" + "," + "'" + Global.DT_ErrorTime[22].ToString("0.00") + "'" + ")";
                    SQL.ExecuteUpdate(InsertStr4);
                    Log.WriteLog("写入白班异常时间数据" + InsertStr4);

                    string InsertStr5 = "insert into HourDT([DateTime],[Status],[Hour_DT1],[Hour_DT2],[Hour_DT3],[Hour_DT4],[Hour_DT5],"
                     + "[Hour_DT6],[Hour_DT7],[Hour_DT8],[Hour_DT9],[Hour_DT10],[Hour_DT11],[Hour_DT12]"
                     + ")" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'" + "," + "'" + "待料时间" + "'" + ","
                     + "'" + Global.DT_PendingTime[0].ToString("0.00") + "'" + "," + "'" + Global.DT_PendingTime[2].ToString("0.00") + "'" + "," + "'" + Global.DT_PendingTime[4].ToString("0.00") + "'" + ","
                     + "'" + Global.DT_PendingTime[6].ToString("0.00") + "'" + "," + "'" + Global.DT_PendingTime[8].ToString("0.00") + "'" + "," + "'" + Global.DT_PendingTime[10].ToString("0.00") + "'" + ","
                     + "'" + Global.DT_PendingTime[12].ToString("0.00") + "'" + "," + "'" + Global.DT_PendingTime[14].ToString("0.00") + "'" + "," + "'" + Global.DT_PendingTime[16].ToString("0.00") + "'" + ","
                     + "'" + Global.DT_PendingTime[18].ToString("0.00") + "'" + "," + "'" + Global.DT_PendingTime[20].ToString("0.00") + "'" + "," + "'" + Global.DT_PendingTime[22].ToString("0.00") + "'" + ")";
                    SQL.ExecuteUpdate(InsertStr5);
                    Log.WriteLog("写入白班待料时间数据" + InsertStr5);

                    string InsertStr6 = "insert into ErrorDataStatistics([DateTime],[Product_Total],[Product_NG1],[Product_NG2],[Product_NG3],[Product_NG4],[Product_NG5]"
                    + ")" + " " + "values(" + "'"
                    + DateTime.Now.ToString("yyyy-MM-dd") + "'" + "," + "'" + Global.Product_NG_Detail[0].ToString() + "'" + "," + "'" + Global.Product_NG_Detail[1].ToString() + "'" + "," + "'"
                    + Global.Product_NG_Detail[2].ToString() + "'" + "," + "'" + Global.Product_NG_Detail[3].ToString() + "'" + "," + "'" + Global.Product_NG_Detail[4].ToString() + "'" + "," + "'" + Global.Product_NG_Detail[5].ToString() + "'" + ")";
                    SQL.ExecuteUpdate(InsertStr6);
                    Log.WriteLog("写入白班抛料统计数据" + InsertStr6);
                    Thread.Sleep(1000);
                }
                #endregion
                Thread.Sleep(50);
            }
        }

        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                PassWordFrm pw = new PassWordFrm();
                //pw.PermissionIndex += new Form1.PermissionEventHandler(SetPermissionIndex);
                pw.ShowDialog();
                if (pw.DialogResult == DialogResult.OK)
                {
                    //DateTime t1 = Convert.ToDateTime(startTime);
                    //DateTime t2 = DateTime.Now;
                    //string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                    //_homefrm.AppendRichText(startTime + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[OEEStatus].errorCode + "," + Global.ed[OEEStatus].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                    //string InsertStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[OEEStatus].errorCode + "'" + "," + "'" + startTime + "'" + "," + "'" + Global.ed[OEEStatus].ModuleCode + "'" + "," + "'" + Global.ed[OEEStatus].errorStatus + "'" + "," + "'" + Global.ed[OEEStatus].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                    //SQL.ExecuteUpdate(InsertStr);

                    //string StopTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");


                    //if (!Global.SelectFirstModel)//当前是否属于手动(首件)状态
                    //{
                    //    if (!Global.SelectTestRunModel)//判断是否处于空跑状态（PLC屏蔽部分功能如：安全门，扫码枪，机械手）
                    //    {
                    //        if ((Global.j == 1 || Global.j == 2 || Global.j == 3 || Global.j == 4) && !Global.BreakStatus)//j为机台运行大状态（-1初始值、1待料、2运行、3宕机、4人工停止）
                    //        {
                    //            if (Global.j == 1 && Global.ed[Global.Error_PendingNum + 1].start_time != null)//当前是否属于待料状态
                    //            {
                    //                DateTime t1 = Convert.ToDateTime(Global.ed[Global.Error_PendingNum + 1].start_time);
                    //                DateTime t2 = Convert.ToDateTime(StopTime);
                    //                string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                    //                string InsertStr2 = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorCode + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].start_time + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].ModuleCode + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                    //                SQL.ExecuteUpdate(InsertStr2);

                    //                _homefrm.AppendRichText(Global.ed[Global.Error_PendingNum + 1].start_time + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.Error_PendingNum + 1].errorCode + "," + Global.ed[Global.Error_PendingNum + 1].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                    //                //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[Global.Error_PendingNum + 1].errorCode + "," + Global.ed[Global.Error_PendingNum + 1].start_time + "," + Global.ed[Global.Error_PendingNum + 1].ModuleCode + "," + "自动发送成功" + "," + Global.ed[Global.Error_PendingNum + 1].errorStatus + "," + Global.ed[Global.Error_PendingNum + 1].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                    //            }
                    //            else if (Global.j == 2 && Global.ed[Global.j].start_time != null)//当前是否属于运行状态
                    //            {
                    //                DateTime t1 = Convert.ToDateTime(Global.ed[Global.j].start_time);
                    //                DateTime t2 = Convert.ToDateTime(StopTime);
                    //                string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                    //                string InsertStr2 = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.j].errorCode + "'" + "," + "'" + Global.ed[Global.j].start_time + "'" + "," + "'" + Global.ed[Global.j].ModuleCode + "'" + "," + "'" + Global.ed[Global.j].errorStatus + "'" + "," + "'" + Global.ed[Global.j].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                    //                SQL.ExecuteUpdate(InsertStr2);

                    //                _homefrm.AppendRichText(Global.ed[Global.j].start_time + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.j].errorCode + "," + Global.ed[Global.j].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                    //                //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[Global.j].errorCode + "," + Global.ed[Global.j].start_time + "," + Global.ed[Global.j].ModuleCode + "," + "自动发送成功" + "," + Global.ed[Global.j].errorStatus + "," + Global.ed[Global.j].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                    //            }
                    //            else if (Global.j == 3 && Global.ed[Global.Error_num + 1].start_time != null)//当前是否属于宕机状态
                    //            {
                    //                DateTime t1 = Convert.ToDateTime(Global.ed[Global.Error_num + 1].start_time);
                    //                DateTime t2 = Convert.ToDateTime(StopTime);
                    //                string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                    //                string InsertStr3 = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorCode + "'" + "," + "'" + Global.ed[Global.Error_num + 1].start_time + "'" + "," + "'" + Global.ed[Global.Error_num + 1].ModuleCode + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                    //                SQL.ExecuteUpdate(InsertStr3);

                    //                _homefrm.AppendRichText(Global.ed[Global.Error_num + 1].start_time + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.Error_num + 1].errorCode + "," + Global.ed[Global.Error_num + 1].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                    //                //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[Global.Error_num + 1].errorCode + "," + Global.ed[Global.Error_num + 1].start_time + "," + Global.ed[Global.Error_num + 1].ModuleCode + "," + "自动发送成功" + "," + Global.ed[Global.Error_num + 1].errorStatus + "," + Global.ed[Global.Error_num + 1].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                    //            }
                    //            else if (Global.j == 4 && Global.ed[Global.Error_Stopnum + 1].start_time != null)//当前是否属于人工停止复位状态
                    //            {
                    //                DateTime t1 = Convert.ToDateTime(Global.ed[Global.Error_Stopnum + 1].start_time);
                    //                DateTime t2 = Convert.ToDateTime(StopTime);
                    //                string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                    //                string InsertStr4 = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorCode + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].start_time + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].ModuleCode + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                    //                SQL.ExecuteUpdate(InsertStr4);

                    //                _homefrm.AppendRichText(Global.ed[Global.Error_Stopnum + 1].start_time + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.Error_Stopnum + 1].errorCode + "," + Global.ed[Global.Error_Stopnum + 1].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                    //                //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[Global.Error_Stopnum + 1].errorCode + "," + Global.ed[Global.Error_Stopnum + 1].start_time + "," + Global.ed[Global.Error_Stopnum + 1].ModuleCode + "," + "自动发送成功" + "," + Global.ed[Global.Error_Stopnum + 1].errorStatus + "," + Global.ed[Global.Error_Stopnum + 1].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                    //            }
                    //        }
                    //        else
                    //        {
                    //            _manualfrm.Btn_UpLoad_break_Click(null, null);
                    //        }
                    //    }
                    //    else//当前状态为（空跑）状态
                    //    {
                    //        DateTime t1 = Convert.ToDateTime(Global.ed[57].start_time);
                    //        DateTime t2 = Convert.ToDateTime(StopTime);
                    //        string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                    //        string InsertStr5 = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[57].errorCode + "'" + "," + "'" + Global.ed[57].start_time + "'" + "," + "'" + Global.ed[57].ModuleCode + "'" + "," + "'" + Global.ed[57].errorStatus + "'" + "," + "'" + Global.ed[57].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                    //        SQL.ExecuteUpdate(InsertStr5);

                    //        _homefrm.AppendRichText(Global.ed[57].start_time + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[57].errorCode + "," + Global.ed[57].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                    //        //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[280].errorCode + "," + Global.ed[280].start_time + "," + Global.ed[280].ModuleCode + "," + "自动发送成功" + "," + Global.ed[280].errorStatus + "," + Global.ed[280].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                    //    }
                    //}
                    //else//当前状态为（首件）状态
                    //{
                    //    DateTime t1 = Convert.ToDateTime(Global.errordata.start_time);
                    //    DateTime t2 = Convert.ToDateTime(StopTime);
                    //    string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                    //    string InsertOEEStr6 = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.errordata.errorCode + "'" + "," + "'" + Global.errordata.start_time + "'" + "," + "'" + "" + "'" + "," + "'" + Global.errordata.errorStatus + "'" + "," + "'" + Global.errordata.errorinfo + "'" + "," + "'" + ts + "'" + ")";
                    //    SQL.ExecuteUpdate(InsertOEEStr6);

                    //    _homefrm.AppendRichText(Global.errordata.start_time + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.errordata.errorCode + "," + Global.errordata.errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                    //    //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.errordata.errorCode + "," + Global.errordata.start_time + "," + "手动发送成功" + "," + Global.errordata.errorStatus + "," + Global.errordata.errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                    //}

                    //结束当前状态，进行OEE记录
                    DateTime t1 = Convert.ToDateTime(Global.start_time);//上笔状态的开始时间
                    Global.start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    DateTime t2 = DateTime.Now;//上笔状态的结束时间
                    string ts = (t2 - t1).TotalMinutes.ToString("0.00");

                    string InsertStr2 = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + t2.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.OEE_Code].errorCode + "'" + "," + "'" + t1.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.OEE_Code].ModuleCode + "'" + "," + "'" + Global.ed[Global.OEE_Code].errorStatus + "'" + "," + "'" + Global.ed[Global.OEE_Code].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                    SQL.ExecuteUpdate(InsertStr2);

                    //记录软件关闭的开始时间
                    string InsertStr_close = "insert into OEE_DT([DateTime],[EventTime],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + t2.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + "6" + "'" + "," + "'" + "软件关闭" + "'" + "," + "'" + "0.00" + "'" + ")";
                    SQL.ExecuteUpdate(InsertStr_close);



                    this.FormClosing -= new FormClosingEventHandler(this.MainFrm_FormClosing);
                    Process.GetCurrentProcess().Kill();
                    this.Dispose();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception EX)
            {
                Log.WriteLog(EX.ToString());
                this.FormClosing -= new FormClosingEventHandler(this.MainFrm_FormClosing);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                this.Dispose();
            }
        }

        private void Set_Laser()
        {
            UA_data.location1_power = Global.inidata.productconfig.power;
            UA_data.location1_frequency = Global.inidata.productconfig.frequency;
            UA_data.location1_waveform = Global.inidata.productconfig.waveform;
            UA_data.location1_laser_speed = Global.inidata.productconfig.laser_speed;
            UA_data.location1_jump_speed = Global.inidata.productconfig.jump_speed;
            UA_data.location1_jump_delay = Global.inidata.productconfig.jump_delay;
            UA_data.location1_position_delay = Global.inidata.productconfig.position_delay;
            UA_data.location1_pulse_profile = Global.inidata.productconfig.pulse_profile;
            UA_data.location1_laser_height1 = Global.inidata.productconfig.laser_height;

            UA_data.location2_power = Global.inidata.productconfig.power;
            UA_data.location2_frequency = Global.inidata.productconfig.frequency;
            UA_data.location2_waveform = Global.inidata.productconfig.waveform;
            UA_data.location2_laser_speed = Global.inidata.productconfig.laser_speed;
            UA_data.location2_jump_speed = Global.inidata.productconfig.jump_speed;
            UA_data.location2_jump_delay = Global.inidata.productconfig.jump_delay;
            UA_data.location2_position_delay = Global.inidata.productconfig.position_delay;
            UA_data.location2_pulse_profile = Global.inidata.productconfig.pulse_profile;
            UA_data.location2_laser_height1 = Global.inidata.productconfig.laser_height;

            UA_data.location3_power = Global.inidata.productconfig.power;
            UA_data.location3_frequency = Global.inidata.productconfig.frequency;
            UA_data.location3_waveform = Global.inidata.productconfig.waveform;
            UA_data.location3_laser_speed = Global.inidata.productconfig.laser_speed;
            UA_data.location3_jump_speed = Global.inidata.productconfig.jump_speed;
            UA_data.location3_jump_delay = Global.inidata.productconfig.jump_delay;
            UA_data.location3_position_delay = Global.inidata.productconfig.position_delay;
            UA_data.location3_pulse_profile = Global.inidata.productconfig.pulse_profile;
            UA_data.location3_laser_height1 = Global.inidata.productconfig.laser_height;

            UA_data.location4_power = Global.inidata.productconfig.power;
            UA_data.location4_frequency = Global.inidata.productconfig.frequency;
            UA_data.location4_waveform = Global.inidata.productconfig.waveform;
            UA_data.location4_laser_speed = Global.inidata.productconfig.laser_speed;
            UA_data.location4_jump_speed = Global.inidata.productconfig.jump_speed;
            UA_data.location4_jump_delay = Global.inidata.productconfig.jump_delay;
            UA_data.location4_position_delay = Global.inidata.productconfig.position_delay;
            UA_data.location4_pulse_profile = Global.inidata.productconfig.pulse_profile;
            UA_data.location4_laser_height1 = Global.inidata.productconfig.laser_height;

            UA_data.location5_power = Global.inidata.productconfig.power;
            UA_data.location5_frequency = Global.inidata.productconfig.frequency;
            UA_data.location5_waveform = Global.inidata.productconfig.waveform;
            UA_data.location5_laser_speed = Global.inidata.productconfig.laser_speed;
            UA_data.location5_jump_speed = Global.inidata.productconfig.jump_speed;
            UA_data.location5_jump_delay = Global.inidata.productconfig.jump_delay;
            UA_data.location5_position_delay = Global.inidata.productconfig.position_delay;
            UA_data.location5_pulse_profile = Global.inidata.productconfig.pulse_profile;
            UA_data.location5_laser_height1 = Global.inidata.productconfig.laser_height;

            UA_data.location6_power = Global.inidata.productconfig.power;
            UA_data.location6_frequency = Global.inidata.productconfig.frequency;
            UA_data.location6_waveform = Global.inidata.productconfig.waveform;
            UA_data.location6_laser_speed = Global.inidata.productconfig.laser_speed;
            UA_data.location6_jump_speed = Global.inidata.productconfig.jump_speed;
            UA_data.location6_jump_delay = Global.inidata.productconfig.jump_delay;
            UA_data.location6_position_delay = Global.inidata.productconfig.position_delay;
            UA_data.location6_pulse_profile = Global.inidata.productconfig.pulse_profile;
            UA_data.location6_laser_height1 = Global.inidata.productconfig.laser_height;

            UA_data.location7_power = Global.inidata.productconfig.power;
            UA_data.location7_frequency = Global.inidata.productconfig.frequency;
            UA_data.location7_waveform = Global.inidata.productconfig.waveform;
            UA_data.location7_laser_speed = Global.inidata.productconfig.laser_speed;
            UA_data.location7_jump_speed = Global.inidata.productconfig.jump_speed;
            UA_data.location7_jump_delay = Global.inidata.productconfig.jump_delay;
            UA_data.location7_position_delay = Global.inidata.productconfig.position_delay;
            UA_data.location7_pulse_profile = Global.inidata.productconfig.pulse_profile;
            UA_data.location7_laser_height1 = Global.inidata.productconfig.laser_height;

            UA_data.location8_power = Global.inidata.productconfig.power;
            UA_data.location8_frequency = Global.inidata.productconfig.frequency;
            UA_data.location8_waveform = Global.inidata.productconfig.waveform;
            UA_data.location8_laser_speed = Global.inidata.productconfig.laser_speed;
            UA_data.location8_jump_speed = Global.inidata.productconfig.jump_speed;
            UA_data.location8_jump_delay = Global.inidata.productconfig.jump_delay;
            UA_data.location8_position_delay = Global.inidata.productconfig.position_delay;
            UA_data.location8_pulse_profile = Global.inidata.productconfig.pulse_profile;
            UA_data.location8_laser_height1 = Global.inidata.productconfig.laser_height;

            UA_data.location9_power = Global.inidata.productconfig.power;
            UA_data.location9_frequency = Global.inidata.productconfig.frequency;
            UA_data.location9_waveform = Global.inidata.productconfig.waveform;
            UA_data.location9_laser_speed = Global.inidata.productconfig.laser_speed;
            UA_data.location9_jump_speed = Global.inidata.productconfig.jump_speed;
            UA_data.location9_jump_delay = Global.inidata.productconfig.jump_delay;
            UA_data.location9_position_delay = Global.inidata.productconfig.position_delay;
            UA_data.location9_pulse_profile = Global.inidata.productconfig.pulse_profile;
            UA_data.location9_laser_height1 = Global.inidata.productconfig.laser_height;

            UA_data.location10_power = Global.inidata.productconfig.power;
            UA_data.location10_frequency = Global.inidata.productconfig.frequency;
            UA_data.location10_waveform = Global.inidata.productconfig.waveform;
            UA_data.location10_laser_speed = Global.inidata.productconfig.laser_speed;
            UA_data.location10_jump_speed = Global.inidata.productconfig.jump_speed;
            UA_data.location10_jump_delay = Global.inidata.productconfig.jump_delay;
            UA_data.location10_position_delay = Global.inidata.productconfig.position_delay;
            UA_data.location10_pulse_profile = Global.inidata.productconfig.pulse_profile;
            UA_data.location10_laser_height1 = Global.inidata.productconfig.laser_height;

            UA_data.location11_power = Global.inidata.productconfig.power;
            UA_data.location11_frequency = Global.inidata.productconfig.frequency;
            UA_data.location11_waveform = Global.inidata.productconfig.waveform;
            UA_data.location11_laser_speed = Global.inidata.productconfig.laser_speed;
            UA_data.location11_jump_speed = Global.inidata.productconfig.jump_speed;
            UA_data.location11_jump_delay = Global.inidata.productconfig.jump_delay;
            UA_data.location11_position_delay = Global.inidata.productconfig.position_delay;
            UA_data.location11_pulse_profile = Global.inidata.productconfig.pulse_profile;
            UA_data.location11_laser_height1 = Global.inidata.productconfig.laser_height;

            UA_data.location12_power = Global.inidata.productconfig.power;
            UA_data.location12_frequency = Global.inidata.productconfig.frequency;
            UA_data.location12_waveform = Global.inidata.productconfig.waveform;
            UA_data.location12_laser_speed = Global.inidata.productconfig.laser_speed;
            UA_data.location12_jump_speed = Global.inidata.productconfig.jump_speed;
            UA_data.location12_jump_delay = Global.inidata.productconfig.jump_delay;
            UA_data.location12_position_delay = Global.inidata.productconfig.position_delay;
            UA_data.location12_pulse_profile = Global.inidata.productconfig.pulse_profile;
            UA_data.location12_laser_height1 = Global.inidata.productconfig.laser_height;

        }

        private byte[] ConvertPrecitecData(string Head, int Length, string sn)//发送precitec数据
        {
            //普雷斯特数据长度格式转换
            string binaryNum = Convert.ToString(Length, 16);
            string result = string.Empty;
            if (binaryNum.Length < 8)
            {
                int length = 8 - binaryNum.Length;
                for (int i = 0; i < length; i++)
                {
                    result += "0";
                }
                result += binaryNum;
            }
            //普雷斯特数据长度格式高低位转换
            string SNlength = result.Substring(6, 2) + result.Substring(4, 2) + result.Substring(2, 2) + result.Substring(0, 2);
            //普雷斯特sn格式转换ASCII码
            byte[] ba = System.Text.ASCIIEncoding.Default.GetBytes(sn);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in ba)
            {
                sb.Append(b.ToString("x"));
            }
            //所有数据格式转换16进制
            string SendData = Head + SNlength + sb.ToString();
            Log.WriteLog(string.Format("发送普雷斯特SN:{0}", SendData));
            byte[] buffer = new byte[SendData.Length / 2];
            for (int i = 0; i < SendData.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(SendData.Substring(i, 2), 16);
            return buffer;

        }

        public static byte[] HexStringToByteArray(string s)//字符串转化16进制
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        public static string ByteArrayToHexSring(byte[] data)//16进制转化字符串
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
            {
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            }
            return sb.ToString().ToUpper();

        }

        public static float HexStringToFloat(string s)//字符串转化float
        {
            UInt32 x = Convert.ToUInt32(s, 16);//字符串转16进制32位无符号整数
            float fy = BitConverter.ToSingle(BitConverter.GetBytes(x), 0);//IEEE754 字节转换float
            return fy;
        }

        private string ASCIITo16(String str) //ASCII字符串转16进制数
        {
            byte[] ba = System.Text.ASCIIEncoding.Default.GetBytes(str);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in ba)
            {
                sb.Append(b.ToString("x"));
            }
            return sb.ToString();
        }


        public string ToASCII(short[] PLCData)//十进制转化为ASCII码
        {
            lock (Lock1)
            {
                string data = string.Empty;
                for (int j = 0; j < PLCData.Length; j++)
                {
                    int lpshDeviceValue = PLCData[j];
                    if (lpshDeviceValue != 0)
                    {
                        string DisplayData = lpshDeviceValue.ToString("X"); //十进制转换成十六进制
                        byte[] array = new byte[(DisplayData.Length + 1) / 2];
                        int index = ((DisplayData.Length + 1) / 2) - 1;   //PLC中输入与显示的顺序相反，所以这块index从最后一位开始
                        for (int i = 0; i < DisplayData.Length; i += 2)
                        {
                            array[index] = Convert.ToByte(DisplayData.Substring(i, 2), 16);
                            index--;
                        }
                        DisplayData = Encoding.Default.GetString(array);
                        data += DisplayData;
                    }
                }
                return data;
            }
        }

        #region socket通讯服务器
        //void server1_ServerDisconnected(object sender, TcpClientDisconnectedEventArgs e)
        //{
        //    _homefrm.AppendRichText("焊机已断开", "Rtxt_Receive");
        //    Log.WriteLog("焊机客户端已断开");
        //}
        //void server1_ClientConnected(object sender, TcpClientConnectedEventArgs e)
        //{
        //    clent = e.TcpClient;
        //    _homefrm.AppendRichText("焊机已连接：" + e.TcpClient.Client.RemoteEndPoint.ToString(), "Rtxt_Receive");
        //    Log.WriteLog("焊机已连接：" + e.TcpClient.Client.RemoteEndPoint.ToString());
        //}

        ////接收到TCP客户端反馈
        //void server1_PlaintextReceived(object sender, TcpDatagramReceivedEventArgs<string> e)
        //{
        //    try
        //    {
        //        FromHG HG_Data = JsonConvert.DeserializeObject<FromHG>(e.Datagram.Replace("\r\n", ""));//对华工数据进行反序列化
        //        string insertStr1 = string.Format("insert into HGData values('{0}','{1}','{2}')", HG_Data.SN, HG_Data.location1_power, HG_Data.location2_frequency);
        //        SQL.ExecuteUpdate(insertStr1);

        //        //接收后回复焊机
        //        Global.server1.Send(e.TcpClient, HG_Data.SN + "OK");


        //    }
        //    catch (Exception ex)
        //    {
        //        Log.WriteLog("华工客户端交互数据抛出异常。" + ex.ToString());
        //    }
        //}

        #endregion

        #region OEE
        private void EthDownTime()
        {
            #region 记录OEE 关闭软件时长
            try
            {
                string SelectStr = "select * from OEE_MCOff where  1=1";
                DataTable d1 = SQL.ExecuteQuery(SelectStr);
                string StartTime = string.Empty;
                if (d1.Rows.Count > 0)//判断上一次是否正常关闭软件-有正常关机时间
                {
                    StartTime = d1.Rows[0][1].ToString();
                    DateTime T1 = Convert.ToDateTime(StartTime);
                    DateTime T2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    string TS = (T2 - T1).TotalMinutes.ToString("0.00");
                    string InsertOEEStr3 = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + "10010001" + "'" + "," + "'" + (Convert.ToDateTime(StartTime)).ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + "" + "'" + "," + "'" + "6" + "'" + "," + "'" + "软件关闭" + "'" + "," + "'" + TS + "'" + ")";
                    SQL.ExecuteUpdate(InsertOEEStr3);

                    _homefrm.AppendRichText(StartTime + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ",10010001," + "软件关闭," + TS + "分钟", "Rtxt_OEE_TimeSpan");


                    //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + "10010001" + "," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + "" + "," + "自动发送成功" + "," + "6" + "," + "软件关闭" + "," + TS, @"E:\装机软件\系统配置\System_ini\");
                }
                else//非正常关机
                {
                    string SelectStr2 = "select * from OEE_StartTime where ID =(SELECT MAX(ID) from OEE_StartTime)";
                    DataTable d2 = SQL.ExecuteQuery(SelectStr2);
                    string StartTime2 = string.Empty;
                    if (d2.Rows.Count > 0)//搜索非正常关机前的最后一次状态开始时间,记录写入OEE数据库中
                    {
                        StartTime = d2.Rows[0][3].ToString();
                        DateTime T11 = Convert.ToDateTime(StartTime);
                        DateTime T12 = Convert.ToDateTime(DateTime.Now.AddMinutes(-1).ToString("yyyy-MM-dd HH:mm:ss"));
                        string TS2 = (T12 - T11).TotalMinutes.ToString("0.00");
                        string InsertOEEStr4 = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.AddMinutes(-1).ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + d2.Rows[0][2].ToString() + "'" + "," + "'" + (Convert.ToDateTime(StartTime)).ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + d2.Rows[0][4].ToString() + "'" + "," + "'" + d2.Rows[0][1].ToString() + "'" + "," + "'" + d2.Rows[0][5].ToString() + "'" + "," + "'" + TS2 + "'" + ")";
                        SQL.ExecuteUpdate(InsertOEEStr4);

                        _homefrm.AppendRichText(StartTime + " -> " + DateTime.Now.AddMinutes(-1).ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + d2.Rows[0][2].ToString() + "," + d2.Rows[0][5].ToString() + "," + TS2 + "分钟", "Rtxt_OEE_TimeSpan");
                        //Log.WriteCSV(DateTime.Now.AddMinutes(-1).ToString("HH:mm:ss") + "," + d2.Rows[0][2].ToString() + "," + DateTime.Now.AddMinutes(-1).ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + d2.Rows[0][4].ToString() + "," + "自动发送成功" + "," + d2.Rows[0][1].ToString() + "," + d2.Rows[0][5].ToString() + "," + TS2, @"E:\装机软件\系统配置\System_ini\");
                    }
                    //非正常关机后，默认下一次开机时间的之前一分钟为关机开始时间
                    //string OEEDownTime = "";
                    //string DownTimemsg = "";

                    ///1
                    ///
                    string date = DateTime.Now.AddMinutes(-1).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    //OEEDownTime = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", "6", "10010001", date, "");
                    //string InsertStr = "insert into OEE_TraceDT([DateTime],[Status],[ErrorCode],[EventTime],[ModuleCode])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + "6" + "'" + "," + "'" + "10010001" + "'" + "," + "'" + date + "'" + "," + "'" + "" + "'" + ")";
                    //SQL.ExecuteUpdate(InsertStr);
                    ///20211118
                    //var rst = RequestAPI.Request(Global.inidata.productconfig.OEE_URL1, Global.inidata.productconfig.OEE_URL2, IP, Mac, Global.inidata.productconfig.OEE_Dsn, Global.inidata.productconfig.OEE_authCode, 2, OEEDownTime, out DownTimemsg);
                    /////0909
                    ///// 
                    /////Night
                    ///// 
                    //string poorNum = string.Empty;
                    //string TotalNum = string.Empty;
                    //if (Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("18:00")) >= 0 || Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("6:00")) < 0)
                    //{
                    //    poorNum = (Global.Product_Total_N - Global.Product_OK_N).ToString();
                    //    TotalNum = Global.Product_Total_N.ToString();
                    //}
                    //else
                    //{
                    //    poorNum = (Global.Product_Total_D - Global.Product_OK_D).ToString();
                    //    TotalNum = Global.Product_Total_D.ToString();
                    //}
                    //Goee.UploadDowntime(poorNum, TotalNum, "6", "10010001", "", false, date, "关机");

                    //if (rst)
                    //{
                    //    _homefrm.AppendRichText("10010001" + ",触发时间=" + date + ",运行状态:" + "6" + ",故障描述:" + "关机" + ",自动发送成功", "rtx_DownTimeMsg");
                    //    Log.WriteLog("OEE_DT补传关机errorCode发送成功");
                    //}
                    //else
                    //{
                    //    _homefrm.AppendRichText("10010001" + ",触发时间=" + date + ",运行状态:" + "6" + ",故障描述:" + "关机" + ",自动发送失败", "rtx_DownTimeMsg");
                    //    Log.WriteLog("OEE_DT补传关机errorCode发送失败");
                    //    Global.ConnectOEEFlag = false;
                    //    string s = "insert into OEE_DTSendNG([DateTime],[Product],[Status],[ErrorCode],[EventTime],[ModuleCode],[Moduleinfo],[errorinfo])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + "OEE_DT" + "'" + "," + "'" + "6" + "'" + "," + "'" + "10010001" + "'" + ","
                    // + "'" + date + "'" + "," + "'" + "" + "'" + "," + "'" + "" + "'" + "," + "'" + "关机" + "'" + ")";
                    //    int r = SQL.ExecuteUpdate(s);
                    //    Log.WriteLog(string.Format("插入了{0}行OEE_DownTime补传关机缓存数据", r));
                    //}
                    //Log.WriteLog("OEE_DT:" + OEEDownTime);

                    DateTime T1 = Convert.ToDateTime(DateTime.Now.AddMinutes(-1).ToString("yyyy-MM-dd HH:mm:ss"));
                    DateTime T2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    string TS = (T2 - T1).TotalMinutes.ToString("0.00");
                    string InsertOEEStr3 = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + "10010001" + "'" + "," + "'" + date + "'" + "," + "'" + "" + "'" + "," + "'" + "6" + "'" + "," + "'" + "软件关闭" + "'" + "," + "'" + TS + "'" + ")";
                    SQL.ExecuteUpdate(InsertOEEStr3);

                    _homefrm.AppendRichText(date + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + "10010001" + "," + "软件关闭" + "," + TS + "分钟", "Rtxt_OEE_TimeSpan");
                    //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + "10010001" + "," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + "" + "," + "自动发送成功" + "," + "6" + "," + "软件关闭" + "," + TS, @"E:\装机软件\系统配置\System_ini\");
                }
                string DeleteOEEStr = "delete OEE_MCOff";
                SQL.ExecuteUpdate(DeleteOEEStr);//清空关机时间
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString() + ",OEELog");
            }
            #endregion

            while (true)
            {
                //ReadStatus = Global.PLC_Client.ReadPLC_D(Address_errorCode, 4);
                //OperateResult<short> r1 = Global.plc1.ReadInt16("MW62000");
                //ReadStatus = r1.Content;//大状态：1待料，2运行，3宕机，4人工停止

                //OperateResult<short> r4 = Global.plc1.ReadInt16("MW62001");
                //ReadStatus_Error = r4.Content;//宕机细节-报警目录

                //OperateResult<short> r5 = Global.plc1.ReadInt16("MW62003");
                //ReadStatus_Pending = r5.Content;//待料细节-报警目录

                //OperateResult<short> r6 = Global.plc1.ReadInt16("MW62002");
                //ReadStatus_Stop = r6.Content;//停止细节-报警目录



                ////ReadTestRunStatus = Global.PLC_Client.ReadPLC_D(10030, 1);
                //OperateResult<short> r2 = Global.plc1.ReadInt16("MW62005");
                //ReadTestRunStatus = r2.Content;//空跑标志位

                ////ReadOpenDoorStatus = Global.PLC_Client.ReadPLC_D(10040, 1);//待料中开启安全门或者暂停标志位
                //OperateResult<short> r3 = Global.plc1.ReadInt16("MW62004");
                //ReadOpenDoorStatus = r3.Content;//待料中开启安全门

                if (ReadStatus != -1)
                {
                    try
                    {
                        if (!Global.SelectFirstModel)//当前是否属于手动(首件)状态
                        {
                            if (ReadTestRunStatus != 1)//判断是否处于空跑状态（PLC屏蔽部分功能如：安全门，扫码枪，机械手）
                            {
                                //非空跑状态
                                if (Global.SelectTestRunModel == true && Global.ed[57].start_time != null)//空运行结束写入OEE_DT数据表中
                                {
                                    Global.ed[57].stop_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    DateTime t1 = Convert.ToDateTime(Global.ed[57].start_time);
                                    DateTime t2 = Convert.ToDateTime(Global.ed[57].stop_time);
                                    string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                                    string InsertStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[57].errorCode + "'" + "," + "'" + Global.ed[57].start_time + "'" + "," + "'" + Global.ed[57].ModuleCode + "'" + "," + "'" + Global.ed[57].errorStatus + "'" + "," + "'" + Global.ed[57].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                                    SQL.ExecuteUpdate(InsertStr);

                                    _homefrm.AppendRichText(Global.ed[57].start_time + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[57].errorCode + "," + Global.ed[57].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");

                                    //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[280].errorCode + "," + Global.ed[280].start_time + "," + Global.ed[280].ModuleCode + "," + "自动发送成功" + "," + Global.ed[280].errorStatus + "," + Global.ed[280].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                                    Global.ed[57].start_time = null;
                                    Global.ed[57].stop_time = null;
                                }
                                Global.SelectTestRunModel = false;
                                if ((ReadStatus == 1 || ReadStatus == 2 || ReadStatus == 3 || ReadStatus == 4) && !Global.BreakStatus)//j为机台运行大状态（-1初始值、1待料、2运行、3宕机、4人工停止），判断是否是吃饭休息
                                {
                                    string EventTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    if (Global.j == -1)//判断是否初始化/结束首件状态
                                    {
                                        if (ReadStatus == 4)//机台处于人工停止4状态
                                        {
                                            StopStatus();
                                        }
                                        else if (ReadStatus == 3)//机台处于宕机3状态
                                        {
                                            ErrorStatus();
                                        }
                                        else if (ReadStatus == 2)//机台处于运行2状态
                                        {
                                            RunStatus();
                                        }
                                        else if (ReadStatus == 1)//处于待料1状态
                                        {
                                            PendingStatus();
                                        }
                                    }
                                    else//上一个状态与当前状态发生变动且上一个状态为非宕机状态时1、2
                                    {
                                        if (Global.STOP)//机台运行中人工停止
                                        {
                                            if (ReadStatus_Stop == 55)//并且打开安全门
                                            {
                                                Global.STOP = false;
                                                //Global.plc1.Write("MW62006", Convert.ToInt16(2));// 未手动选择打开安全门原因，机台不能运行
                                            }
                                        }

                                        if (ReadStatus == 1)//判断当前状态为待料状态时1
                                        {
                                            if (ReadOpenDoorStatus == 1)//10040 判断是否在待料中开安全门或者按下暂停按钮
                                            {
                                                //Global.PLC_Client.WritePLC_D(10020, new short[] { 2 });
                                                //未手动选择打开安全门原因，机台不能运行
                                                //Global.plc1.Write("MW62006", Convert.ToInt16(2));
                                                Global.Error_PendingStatus = true;
                                                Global.ed[Global.Error_PendingNum + 1].stop_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                                //string c = "c=UPLOAD_DOWNTIME&tsn=Test_station&mn=Machine#1&start_time=" + Global.ed[Global.Error_PendingNum + 1].start_time + "&stop_time=" + Global.ed[Global.Error_PendingNum + 1].stop_time + "&ec=" + Global.ed[Global.Error_PendingNum + 1].errorCode;
                                                //Log.WriteLog(c + ",OEELog");
                                                if (Global.ed[Global.Error_PendingNum + 1].start_time != null)
                                                {
                                                    DateTime t1 = Convert.ToDateTime(Global.ed[Global.Error_PendingNum + 1].start_time);
                                                    DateTime t2 = Convert.ToDateTime(Global.ed[Global.Error_PendingNum + 1].stop_time);
                                                    string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                                                    string InsertStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorCode + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].start_time + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].ModuleCode + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                                                    SQL.ExecuteUpdate(InsertStr);

                                                    _homefrm.AppendRichText(Global.ed[Global.Error_PendingNum + 1].start_time + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.Error_PendingNum + 1].errorCode + "," + Global.ed[Global.Error_PendingNum + 1].errorinfo + ts + "分钟", "Rtxt_OEE_TimeSpan");
                                                    //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[Global.Error_PendingNum + 1].errorCode + "," + Global.ed[Global.Error_PendingNum + 1].start_time + "," + Global.ed[Global.Error_PendingNum + 1].ModuleCode + "," + "自动发送成功" + "," + Global.ed[Global.Error_PendingNum + 1].errorStatus + "," + Global.ed[Global.Error_PendingNum + 1].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                                                    _manualfrm.labelcolor(Color.Transparent, "未选择", "LB_ManualSelect");
                                                }
                                                Log.WriteLog(Global.ed[Global.Error_PendingNum + 1].Moduleinfo + "_" + Global.ed[Global.Error_PendingNum + 1].errorinfo + "：结束计时 " + Global.ed[Global.Error_PendingNum + 1].stop_time);
                                                Global.ed[Global.Error_PendingNum + 1].start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//待料中开启安全门或者按下暂停后,待料结束,进入手动选择异常原因状态,待料结束时间为手动选择状态的开始时间
                                            }
                                        }

                                        if (Global.j != ReadStatus && Global.j == 1)//上一个状态与当前状态发生变动且上一个状态为待料状态时1
                                        {
                                            string date = Global.ed[Global.Error_PendingNum + 1].start_time;
                                            if (Global.Error_PendingStatus)//判断待料时是否打开安全门/按下暂停键
                                            {
                                                Global.Error_PendingStatus = false;
                                                Global.ed[Global.Error_PendingNum + 1].stop_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                                if (Global.ed[Global.Error_PendingNum + 1].start_time != null)
                                                {
                                                    DateTime t1 = Convert.ToDateTime(Global.ed[Global.Error_PendingNum + 1].start_time);
                                                    DateTime t2 = Convert.ToDateTime(Global.ed[Global.Error_PendingNum + 1].stop_time);
                                                    string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                                                    //string OEE_DT = "";


                                                    //OEE_DT = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", Global.errorStatus, Global.errorcode, date, "");
                                                    //string InsertStr = "insert into OEE_TraceDT([DateTime],[Status],[ErrorCode],[EventTime],[ModuleCode])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Global.errorStatus + "'" + "," + "'" + Global.errorcode + "'" + "," + "'" + date + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].ModuleCode + "'" + ")";
                                                    //SQL.ExecuteUpdate(InsertStr);
                                                    //Log.WriteLog("OEE_DT安全门打开:" + OEE_DT + ",OEELog");
                                                    //var rst = RequestAPI.Request(Global.inidata.productconfig.OEE_URL1, Global.inidata.productconfig.OEE_URL2, IP, Mac, Global.inidata.productconfig.OEE_Dsn, Global.inidata.productconfig.OEE_authCode, 2, OEE_DT, out msg);

                                                    /////0909
                                                    ///// 
                                                    /////Night
                                                    ///// 
                                                    //string poorNum = string.Empty;
                                                    //string TotalNum = string.Empty;
                                                    //if (Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("18:00")) >= 0 || Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("6:00")) < 0)
                                                    //{
                                                    //    poorNum = (Global.Product_Total_N - Global.Product_OK_N).ToString();
                                                    //    TotalNum = Global.Product_Total_N.ToString();
                                                    //}
                                                    //else
                                                    //{
                                                    //    poorNum = (Global.Product_Total_D - Global.Product_OK_D).ToString();
                                                    //    TotalNum = Global.Product_Total_D.ToString();
                                                    //}
                                                    //Goee.UploadDowntime(poorNum, TotalNum, Global.errorStatus, Global.errorcode, Global.ed[Global.Error_PendingNum + 1].ModuleCode, false, date, Global.errorinfo);
                                                    /////

                                                    //if (rst)
                                                    //{
                                                    //    _homefrm.AppendRichText(Global.errorcode + ",触发时间=" + date + ",运行状态:" + Global.errorStatus + ",故障描述:" + Global.errorinfo + ",安全门打开自动发送成功", "rtx_DownTimeMsg");
                                                    //    Log.WriteLog("OEE_DT安全门打开自动errorCode发送成功" + ",OEELog");
                                                    //    Global.ConnectOEEFlag = true;
                                                    //}
                                                    //else
                                                    //{
                                                    //    _homefrm.AppendRichText(Global.errorcode + ",触发时间=" + date + ",运行状态:" + Global.errorStatus + ",故障描述:" + Global.errorinfo + ",安全门打开自动发送失败", "rtx_DownTimeMsg");
                                                    //    Log.WriteLog("OEE_DT安全门打开自动errorCode发送失败" + ",OEELog");
                                                    //    Global.ConnectOEEFlag = false;
                                                    //    string s = "insert into OEE_DTSendNG([DateTime],[Product],[Status],[ErrorCode],[EventTime],[ModuleCode],[Moduleinfo],[errorinfo])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + "OEE_DT" + "'" + "," + "'" + Global.errorStatus + "'" + "," + "'" + Global.errorcode + "'" + ","
                                                    //     + "'" + date + "'" + "," + "'" + "" + "'" + "," + "'" + "" + "'" + "," + "'" + Global.errorinfo + "'" + ")";
                                                    //    int r = SQL.ExecuteUpdate(s);
                                                    //    Log.WriteLog(string.Format("插入了{0}行OEE_DownTime缓存数据", r) + ",OEELog");
                                                    //}
                                                    _manualfrm.labelcolor(Color.Transparent, "未选择", "LB_ManualSelect");
                                                    string InsertOEEStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.errorcode + "'" + "," + "'" + date + "'" + "," + "'" + "" + "'" + "," + "'" + Global.errorStatus + "'" + "," + "'" + Global.errorinfo + "'" + "," + "'" + ts + "'" + ")";
                                                    SQL.ExecuteUpdate(InsertOEEStr);

                                                    _homefrm.AppendRichText(date + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.errorcode + "," + Global.errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                                                    //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.errorcode + "," + date + "," + "" + "," + "自动发送成功" + "," + Global.errorStatus + "," + Global.errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                                                    //Log.WriteLog("" + "_" + Global.errorinfo + "：结束计时 " + Global.ed[Global.Error_PendingNum + 1].stop_time);
                                                }
                                            }
                                            else
                                            {
                                                Global.ed[Global.Error_PendingNum + 1].stop_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                                _manualfrm.labelcolor(Color.Transparent, "未选择", "LB_ManualSelect");
                                                //_manualfrm.ButtonFlag(false, "btnManualOEEStatus");
                                                //string c = "c=UPLOAD_DOWNTIME&tsn=Test_station&mn=Machine#1&start_time=" + Global.ed[Global.Error_PendingNum + 1].start_time + "&stop_time=" + Global.ed[Global.Error_PendingNum + 1].stop_time + "&ec=" + Global.ed[Global.Error_PendingNum + 1].errorCode;
                                                //Log.WriteLog(c + ",OEELog");
                                                if (Global.ed[Global.Error_PendingNum + 1].start_time != null)
                                                {
                                                    DateTime t1 = Convert.ToDateTime(Global.ed[Global.Error_PendingNum + 1].start_time);
                                                    DateTime t2 = Convert.ToDateTime(Global.ed[Global.Error_PendingNum + 1].stop_time);
                                                    string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                                                    string InsertStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorCode + "'" + "," + "'" + date + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].ModuleCode + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                                                    SQL.ExecuteUpdate(InsertStr);

                                                    _homefrm.AppendRichText(date + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.Error_PendingNum + 1].errorCode + "," + Global.ed[Global.Error_PendingNum + 1].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                                                    //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[Global.Error_PendingNum + 1].errorCode + "," + Global.ed[Global.Error_PendingNum + 1].start_time + "," + Global.ed[Global.Error_PendingNum + 1].ModuleCode + "," + "自动发送成功" + "," + Global.ed[Global.Error_PendingNum + 1].errorStatus + "," + Global.ed[Global.Error_PendingNum + 1].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                                                    _manualfrm.labelcolor(Color.Transparent, "未选择", "LB_ManualSelect");
                                                }
                                                Log.WriteLog(Global.ed[Global.Error_PendingNum + 1].Moduleinfo + "_" + Global.ed[Global.Error_PendingNum + 1].errorinfo + "：结束计时 " + Global.ed[Global.Error_PendingNum + 1].stop_time + ",OEELog");
                                            }
                                            Global.ed[Global.Error_PendingNum + 1].start_time = null;
                                            Global.ed[Global.Error_PendingNum + 1].stop_time = null;
                                            Global.j = ReadStatus;
                                            //-------------上一个状态结束，当前状态开始计时--------------
                                            if (ReadStatus == 4)//机台处于人工停止4状态
                                            {
                                                StopStatus();
                                            }
                                            else if (ReadStatus == 3)//机台处于宕机3状态
                                            {
                                                ErrorStatus();
                                            }
                                            else if (ReadStatus == 2)//机台处于运行2状态
                                            {
                                                RunStatus();
                                            }
                                            else if (ReadStatus == 1)//机台处于待机待料1状态
                                            {
                                                PendingStatus();
                                            }
                                        }
                                        else if (Global.j != ReadStatus && Global.j == 2)//上一个状态与当前状态发生变动且上一个状态为运行状态时2
                                        {
                                            Global.ed[Global.j].stop_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                            if (Global.ed[Global.j].start_time != null)
                                            {
                                                DateTime t1 = Convert.ToDateTime(Global.ed[Global.j].start_time);
                                                DateTime t2 = Convert.ToDateTime(Global.ed[Global.j].stop_time);
                                                string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                                                string InsertStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.j].errorCode + "'" + "," + "'" + Global.ed[Global.j].start_time + "'" + "," + "'" + Global.ed[Global.j].ModuleCode + "'" + "," + "'" + Global.ed[Global.j].errorStatus + "'" + "," + "'" + Global.ed[Global.j].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                                                SQL.ExecuteUpdate(InsertStr);

                                                _homefrm.AppendRichText(Global.ed[Global.j].start_time + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.j].errorCode + "," + Global.ed[Global.j].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                                                //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[Global.j].errorCode + "," + Global.ed[Global.j].start_time + "," + Global.ed[Global.j].ModuleCode + "," + "自动发送成功" + "," + Global.ed[Global.j].errorStatus + "," + Global.ed[Global.j].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                                                _manualfrm.labelcolor(Color.Transparent, "未选择", "LB_ManualSelect");
                                            }
                                            Log.WriteLog(Global.ed[Global.j].Moduleinfo + "_" + Global.ed[Global.j].errorinfo + "：结束计时 " + Global.ed[Global.j].stop_time);
                                            Global.ed[Global.j].start_time = null;
                                            Global.ed[Global.j].stop_time = null;
                                            Global.j = ReadStatus;
                                            //-------------上一个状态结束，当前状态开始计时--------------
                                            if (ReadStatus == 4)//机台处于人工停止4状态
                                            {
                                                StopStatus();
                                            }
                                            else if (ReadStatus == 3)//机台处于宕机3状态
                                            {
                                                ErrorStatus();
                                            }
                                            else if (ReadStatus == 2)//机台处于运行2状态
                                            {
                                                RunStatus();
                                            }
                                            else if (ReadStatus == 1)//机台处于待机待料1状态
                                            {
                                                PendingStatus();
                                            }
                                        }
                                        else if (Global.j != ReadStatus && Global.j == 3)//上一个状态与当前状态发生变动且上一个状态为宕机状态3时
                                        {
                                            Global.ed[Global.Error_num + 1].stop_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                            //string c = "c=UPLOAD_DOWNTIME&tsn=Test_station&mn=Machine#1&start_time=" + Global.ed[Global.Error_num + 1].start_time + "&stop_time=" + Global.ed[Global.Error_num + 1].stop_time + "&ec=" + Global.ed[Global.Error_num + 1].errorCode;
                                            //Log.WriteLog(c + ",OEELog");
                                            if (Global.ed[Global.Error_num + 1].start_time != null)
                                            {
                                                DateTime t1 = Convert.ToDateTime(Global.ed[Global.Error_num + 1].start_time);
                                                DateTime t2 = Convert.ToDateTime(Global.ed[Global.Error_num + 1].stop_time);
                                                string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                                                string date = Global.ed[Global.Error_num + 1].start_time;
                                                if (Global.Error_num == 55)//机台打开安全门
                                                {
                                                    string OEE_DT = "";
                                                    //string msg = "";

                                                    OEE_DT = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", Global.errorStatus, Global.errorcode, date, "");
                                                    string InsertStr = "insert into OEE_TraceDT([DateTime],[Status],[ErrorCode],[EventTime],[ModuleCode])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Global.errorStatus + "'" + "," + "'" + Global.errorcode + "'" + ","
                                                    + "'" + date + "'" + "," + "'" + Global.ed[Global.Error_num + 1].ModuleCode + "'" + ")";
                                                    SQL.ExecuteUpdate(InsertStr);
                                                    Log.WriteLog("OEE_DT安全门打开:" + OEE_DT + ",OEELog");

                                                    /////0909
                                                    ///// 
                                                    /////Night
                                                    ///// 
                                                    //string poorNum = string.Empty;
                                                    //string TotalNum = string.Empty;
                                                    //if (Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("18:00")) >= 0 || Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("6:00")) < 0)
                                                    //{
                                                    //    poorNum = (Global.Product_Total_N - Global.Product_OK_N).ToString();
                                                    //    TotalNum = Global.Product_Total_N.ToString();
                                                    //}
                                                    //else
                                                    //{
                                                    //    poorNum = (Global.Product_Total_D - Global.Product_OK_D).ToString();
                                                    //    TotalNum = Global.Product_Total_D.ToString();
                                                    //}
                                                    //Goee.UploadDowntime(poorNum, TotalNum, Global.errorStatus, Global.errorcode, Global.ed[Global.Error_num + 1].ModuleCode, false, date, Global.errorinfo + ",安全门打开");
                                                    /////


                                                    //var rst = RequestAPI.Request(Global.inidata.productconfig.OEE_URL1, Global.inidata.productconfig.OEE_URL2, IP, Mac, Global.inidata.productconfig.OEE_Dsn, Global.inidata.productconfig.OEE_authCode, 2, OEE_DT, out msg);
                                                    //if (rst)
                                                    //{
                                                    //    _homefrm.AppendRichText(Global.errorcode + ",触发时间=" + date + ",运行状态:" + Global.errorStatus + ",故障描述:" + Global.errorinfo + ",安全门打开自动发送成功", "rtx_DownTimeMsg");
                                                    //    Log.WriteLog("OEE_DT安全门打开自动errorCode发送成功" + ",OEELog");
                                                    //    Global.ConnectOEEFlag = true;
                                                    //}
                                                    //else
                                                    //{
                                                    //    _homefrm.AppendRichText(Global.errorcode + ",触发时间=" + date + ",运行状态:" + Global.errorStatus + ",故障描述:" + Global.errorinfo + ",安全门打开自动发送失败", "rtx_DownTimeMsg");
                                                    //    Log.WriteLog("OEE_DT安全门打开自动errorCode发送失败" + ",OEELog");
                                                    //    Global.ConnectOEEFlag = false;
                                                    //    string s = "insert into OEE_DTSendNG([DateTime],[Product],[Status],[ErrorCode],[EventTime],[ModuleCode],[Moduleinfo],[errorinfo])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + "OEE_DT" + "'" + "," + "'" + Global.errorStatus + "'" + "," + "'" + Global.errorcode + "'" + ","
                                                    //     + "'" + date + "'" + "," + "'" + "" + "'" + "," + "'" + "" + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorinfo + "'" + ")";
                                                    //    int r = SQL.ExecuteUpdate(s);
                                                    //    Log.WriteLog(string.Format("插入了{0}行OEE_DownTime缓存数据", r) + ",OEELog");
                                                    //}
                                                    _manualfrm.labelcolor(Color.Transparent, "未选择", "LB_ManualSelect");
                                                    string InsertOEEStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.errorcode + "'" + "," + "'" + date + "'" + "," + "'" + "" + "'" + "," + "'" + Global.errorStatus + "'" + "," + "'" + Global.errorinfo + "'" + "," + "'" + ts + "'" + ")";
                                                    SQL.ExecuteUpdate(InsertOEEStr);

                                                    _homefrm.AppendRichText(date + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.errorcode + "," + Global.errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                                                    //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.errorcode + "," + Global.ed[Global.Error_num + 1].start_time + "," + "" + "," + "自动发送成功" + "," + Global.errorStatus + "," + Global.errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                                                }
                                                else//机台处于其它异常状态中
                                                {
                                                    string InsertOEEStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorCode + "'" + "," + "'" + date + "'" + "," + "'" + Global.ed[Global.Error_num + 1].ModuleCode + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                                                    SQL.ExecuteUpdate(InsertOEEStr);

                                                    _homefrm.AppendRichText(date + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.Error_num + 1].errorCode + "," + Global.ed[Global.Error_num + 1].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                                                    //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[Global.Error_num + 1].errorCode + "," + date + "," + Global.ed[Global.Error_num + 1].ModuleCode + "," + "自动发送成功" + "," + Global.ed[Global.Error_num + 1].errorStatus + "," + Global.ed[Global.Error_num + 1].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                                                }
                                            }
                                            Log.WriteLog(Global.ed[Global.Error_num + 1].Moduleinfo + "_" + Global.ed[Global.Error_num + 1].errorinfo + "：结束计时 " + Global.ed[Global.Error_num + 1].stop_time + ",OEELog");
                                            Global.ed[Global.Error_num + 1].start_time = null;
                                            Global.ed[Global.Error_num + 1].stop_time = null;
                                            Global.j = ReadStatus;
                                            //-------------上一个状态结束，当前状态开始计时--------------
                                            if (ReadStatus == 4)//机台处于人工停止4状态
                                            {
                                                StopStatus();
                                            }
                                            else if (ReadStatus == 3)//机台处于宕机3状态
                                            {
                                                ErrorStatus();
                                            }
                                            else if (ReadStatus == 2)//机台处于运行2状态
                                            {
                                                RunStatus();
                                            }
                                            else if (ReadStatus == 1)//机台处于待机待料1状态
                                            {
                                                PendingStatus();
                                            }
                                        }
                                        else if (Global.j != ReadStatus && Global.j == 4)//上一个状态与当前状态发生变动且上一个状态为人工停止状态4时
                                        {
                                            Global.ed[Global.Error_Stopnum + 1].stop_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                            //string c = "c=UPLOAD_DOWNTIME&tsn=Test_station&mn=Machine#1&start_time=" + Global.ed[Global.Error_Stopnum + 1].start_time + "&stop_time=" + Global.ed[Global.Error_Stopnum + 1].stop_time + "&ec=" + Global.ed[Global.Error_Stopnum + 1].errorCode;
                                            //Log.WriteLog(c + ",OEELog");
                                            if (Global.ed[Global.Error_Stopnum + 1].start_time != null)
                                            {
                                                DateTime t1 = Convert.ToDateTime(Global.ed[Global.Error_Stopnum + 1].start_time);
                                                DateTime t2 = Convert.ToDateTime(Global.ed[Global.Error_Stopnum + 1].stop_time);
                                                string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                                                if (ReadStatus_Stop == 55 || Global.SelectManualErrorCode)
                                                {
                                                    //string OEE_DT = "";
                                                    //string msg = "";

                                                    string date = Global.ed[Global.Error_Stopnum + 1].start_time;
                                                    //OEE_DT = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", Global.errorStatus, Global.errorcode, date, "");
                                                    //string InsertStr = "insert into OEE_TraceDT([DateTime],[Status],[ErrorCode],[EventTime],[ModuleCode])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Global.errorStatus + "'" + "," + "'" + Global.errorcode + "'" + ","+ "'" + date + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].ModuleCode + "'" + ")";
                                                    //SQL.ExecuteUpdate(InsertStr);
                                                    //Log.WriteLog("OEE_DT安全门打开:" + OEE_DT + ",OEELog");
                                                    //var rst = RequestAPI.Request(Global.inidata.productconfig.OEE_URL1, Global.inidata.productconfig.OEE_URL2, IP, Mac, Global.inidata.productconfig.OEE_Dsn, Global.inidata.productconfig.OEE_authCode, 2, OEE_DT, out msg);

                                                    /////0909
                                                    ///// 
                                                    /////Night
                                                    ///// 
                                                    //string poorNum = string.Empty;
                                                    //string TotalNum = string.Empty;
                                                    //if (Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("18:00")) >= 0 || Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("6:00")) < 0)
                                                    //{
                                                    //    poorNum = (Global.Product_Total_N - Global.Product_OK_N).ToString();
                                                    //    TotalNum = Global.Product_Total_N.ToString();
                                                    //}
                                                    //else
                                                    //{
                                                    //    poorNum = (Global.Product_Total_D - Global.Product_OK_D).ToString();
                                                    //    TotalNum = Global.Product_Total_D.ToString();
                                                    //}
                                                    //Goee.UploadDowntime(poorNum, TotalNum, Global.errorStatus, Global.errorcode, Global.ed[Global.Error_num + 1].ModuleCode, false, date, Global.errorinfo + ",安全门打开");
                                                    /////

                                                    //if (rst)
                                                    //{
                                                    //    _homefrm.AppendRichText(Global.errorcode + ",触发时间=" + Global.ed[Global.Error_Stopnum + 1].start_time + ",运行状态:" + Global.errorStatus + ",故障描述:" + Global.errorinfo + ",安全门打开自动发送成功", "rtx_DownTimeMsg");
                                                    //    Log.WriteLog("OEE_DT安全门打开自动errorCode发送成功" + ",OEELog");
                                                    //    Global.ConnectOEEFlag = true;
                                                    //}
                                                    //else
                                                    //{
                                                    //    _homefrm.AppendRichText(Global.errorcode + ",触发时间=" + Global.ed[Global.Error_Stopnum + 1].start_time + ",运行状态:" + Global.errorStatus + ",故障描述:" + Global.errorinfo + ",安全门打开自动发送失败", "rtx_DownTimeMsg");
                                                    //    Log.WriteLog("OEE_DT安全门打开自动errorCode发送失败" + ",OEELog");
                                                    //    Global.ConnectOEEFlag = false;
                                                    //    string s = "insert into OEE_DTSendNG([DateTime],[Product],[Status],[ErrorCode],[EventTime],[ModuleCode],[Moduleinfo],[errorinfo])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + "OEE_DT" + "'" + "," + "'" + Global.errorStatus + "'" + "," + "'" + Global.errorcode + "'" + ","
                                                    //     + "'" + date + "'" + "," + "'" + "" + "'" + "," + "'" + "" + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorinfo + "'" + ")";
                                                    //    int r = SQL.ExecuteUpdate(s);
                                                    //    Log.WriteLog(string.Format("插入了{0}行OEE_DownTime缓存数据", r) + ",OEELog");
                                                    //}
                                                    _manualfrm.labelcolor(Color.Transparent, "未选择", "LB_ManualSelect");
                                                    string InsertOEEStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.errorcode + "'" + "," + "'" + date + "'" + "," + "'" + "" + "'" + "," + "'" + Global.errorStatus + "'" + "," + "'" + Global.errorinfo + "'" + "," + "'" + ts + "'" + ")";
                                                    SQL.ExecuteUpdate(InsertOEEStr);

                                                    _homefrm.AppendRichText(date + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.errorcode + "," + Global.errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                                                    //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.errorcode + "," + Global.ed[Global.Error_Stopnum + 1].start_time + "," + "" + "," + "自动发送成功" + "," + Global.errorStatus + "," + Global.errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                                                }
                                                else if (Global.Error_Stopnum == 278)//机台人工停止
                                                {
                                                    //string OEE_DT = "";
                                                    //string msg = "";
                                                    string date = Global.ed[Global.Error_Stopnum + 1].start_time;
                                                    //OEE_DT = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", Global.ed[Global.Error_Stopnum + 1].errorStatus, Global.ed[Global.Error_Stopnum + 1].errorCode, date, "");
                                                    //string InsertStr = "insert into OEE_TraceDT([DateTime],[Status],[ErrorCode],[EventTime],[ModuleCode])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorCode + "'" + "," + "'" + date + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].ModuleCode + "'" + ")";
                                                    //SQL.ExecuteUpdate(InsertStr);
                                                    //Log.WriteLog("OEE_DT安全门打开:" + OEE_DT + ",OEELog");
                                                    //var rst = RequestAPI.Request(Global.inidata.productconfig.OEE_URL1, Global.inidata.productconfig.OEE_URL2, IP, Mac, Global.inidata.productconfig.OEE_Dsn, Global.inidata.productconfig.OEE_authCode, 2, OEE_DT, out msg);

                                                    ///0909
                                                    /// 
                                                    ///Night
                                                    /// 
                                                    //string poorNum = string.Empty;
                                                    //string TotalNum = string.Empty;
                                                    //if (Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("18:00")) >= 0 || Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("6:00")) < 0)
                                                    //{
                                                    //    poorNum = (Global.Product_Total_N - Global.Product_OK_N).ToString();
                                                    //    TotalNum = Global.Product_Total_N.ToString();
                                                    //}
                                                    //else
                                                    //{
                                                    //    poorNum = (Global.Product_Total_D - Global.Product_OK_D).ToString();
                                                    //    TotalNum = Global.Product_Total_D.ToString();
                                                    //}
                                                    //Goee.UploadDowntime(poorNum, TotalNum, Global.errorStatus, Global.errorcode, Global.ed[Global.Error_num + 1].ModuleCode, false, date, Global.ed[Global.Error_Stopnum + 1].errorinfo + ",安全门打开");
                                                    ///

                                                    //if (rst)
                                                    //{
                                                    //    _homefrm.AppendRichText(Global.ed[Global.Error_Stopnum + 1].errorCode + ",触发时间=" + Global.ed[Global.Error_Stopnum + 1].start_time + ",运行状态:" + Global.ed[Global.Error_Stopnum + 1].errorStatus + ",故障描述:" + Global.ed[Global.Error_Stopnum + 1].errorinfo + ",安全门打开自动发送成功", "rtx_DownTimeMsg");
                                                    //    Log.WriteLog("OEE_DT安全门打开自动errorCode发送成功" + ",OEELog");
                                                    //    Global.ConnectOEEFlag = true;
                                                    //}
                                                    //else
                                                    //{
                                                    //    _homefrm.AppendRichText(Global.ed[Global.Error_Stopnum + 1].errorCode + ",触发时间=" + Global.ed[Global.Error_Stopnum + 1].start_time + ",运行状态:" + Global.ed[Global.Error_Stopnum + 1].errorStatus + ",故障描述:" + Global.ed[Global.Error_Stopnum + 1].errorinfo + ",安全门打开自动发送失败", "rtx_DownTimeMsg");
                                                    //    Log.WriteLog("OEE_DT安全门打开自动errorCode发送失败" + ",OEELog");
                                                    //    Global.ConnectOEEFlag = false;
                                                    //    string s = "insert into OEE_DTSendNG([DateTime],[Product],[Status],[ErrorCode],[EventTime],[ModuleCode],[Moduleinfo],[errorinfo])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + "OEE_DT" + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorCode + "'" + ","
                                                    //     + "'" + Global.ed[Global.Error_Stopnum + 1].start_time + "'" + "," + "'" + "" + "'" + "," + "'" + "" + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorinfo + "'" + ")";
                                                    //    int r = SQL.ExecuteUpdate(s);
                                                    //    Log.WriteLog(string.Format("插入了{0}行OEE_DownTime缓存数据", r) + ",OEELog");
                                                    //}
                                                    _manualfrm.labelcolor(Color.Transparent, "未选择", "LB_ManualSelect");
                                                    string InsertOEEStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorCode + "'" + "," + "'" + date + "'" + "," + "'" + "" + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                                                    SQL.ExecuteUpdate(InsertOEEStr);

                                                    _homefrm.AppendRichText(date + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.Error_Stopnum + 1].errorCode + "," + Global.ed[Global.Error_Stopnum + 1].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                                                    //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[Global.Error_Stopnum + 1].errorCode + "," + date + "," + "" + "," + "自动发送成功" + "," + Global.ed[Global.Error_Stopnum + 1].errorStatus + "," + Global.ed[Global.Error_Stopnum + 1].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                                                }
                                                else//机台处于其它异常状态中
                                                {
                                                    Log.WriteLog("PLC人工停止ErrorCode异常" + Global.Error_Stopnum + ",OEELog");
                                                }
                                            }
                                            Log.WriteLog(Global.ed[Global.Error_Stopnum + 1].Moduleinfo + "_" + Global.ed[Global.Error_Stopnum + 1].errorinfo + "：结束计时 " + Global.ed[Global.Error_Stopnum + 1].stop_time + ",OEELog");
                                            Global.ed[Global.Error_Stopnum + 1].start_time = null;
                                            Global.ed[Global.Error_Stopnum + 1].stop_time = null;
                                            Global.j = ReadStatus;
                                            //-------------上一个状态结束，当前状态开始计时--------------
                                            if (ReadStatus == 4)//机台处于人工停止4状态
                                            {
                                                StopStatus();
                                            }
                                            else if (ReadStatus == 3)//机台处于宕机3状态
                                            {
                                                ErrorStatus();
                                            }
                                            else if (ReadStatus == 2)//机台处于运行2状态
                                            {
                                                RunStatus();
                                            }
                                            else if (ReadStatus == 1)//机台处于待机待料1状态
                                            {
                                                PendingStatus();
                                            }
                                        }
                                        else
                                        { }
                                    }
                                }
                                else
                                {
                                }
                            }
                            else//处于空跑(PLC屏蔽部分功能)状态
                            {
                                if (Global.SelectTestRunModel == false)
                                {
                                    Global.SelectTestRunModel = true;
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

                                            _homefrm.AppendRichText(Global.ed[Global.Error_PendingNum + 1].start_time + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.Error_PendingNum + 1].errorCode + "," + Global.ed[Global.Error_PendingNum + 1].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                                            //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[Global.Error_PendingNum + 1].errorCode + "," + Global.ed[Global.Error_PendingNum + 1].start_time + "," + "'" + "" + "'" + "," + "自动发送成功" + "," + Global.ed[Global.Error_PendingNum + 1].errorStatus + "," + Global.ed[Global.Error_PendingNum + 1].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                                        }
                                        else if (Global.j == 2)//处于运行状态
                                        {
                                            Global.ed[Global.j].stop_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                            DateTime t1 = Convert.ToDateTime(Global.ed[Global.j].start_time);
                                            DateTime t2 = Convert.ToDateTime(Global.ed[Global.j].stop_time);
                                            string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                                            string InsertOEEStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.j].errorCode + "'" + "," + "'" + Global.ed[Global.j].start_time + "'" + "," + "'" + "" + "'" + "," + "'" + Global.ed[Global.j].errorStatus + "'" + "," + "'" + Global.ed[Global.j].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                                            SQL.ExecuteUpdate(InsertOEEStr);

                                            _homefrm.AppendRichText(Global.ed[Global.j].start_time + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.j].errorCode + "," + Global.ed[Global.j].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                                            //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[Global.j].errorCode + "," + Global.ed[Global.j].start_time + "," + "'" + "" + "'" + "," + "自动发送成功" + "," + Global.ed[Global.j].errorStatus + "," + Global.ed[Global.j].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                                        }
                                        else if (Global.j == 3)//处于宕机状态
                                        {
                                            Global.ed[Global.Error_num + 1].stop_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                            DateTime t1 = Convert.ToDateTime(Global.ed[Global.Error_num + 1].start_time);
                                            DateTime t2 = Convert.ToDateTime(Global.ed[Global.Error_num + 1].stop_time);
                                            string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                                            if (Global.Error_num == 55)//机台打开安全门
                                            {
                                                //string OEE_DT2 = "";
                                                //string msg2 = "";
                                                string date = Global.ed[Global.Error_num + 1].start_time;
                                                //OEE_DT2 = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", Global.errorStatus, Global.errorcode, date, "");
                                                //string InsertStr2 = "insert into OEE_TraceDT([DateTime],[Status],[ErrorCode],[EventTime],[ModuleCode])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Global.errorStatus + "'" + "," + "'" + Global.errorcode + "'" + ","+ "'" + date + "'" + "," + "'" + Global.ed[Global.Error_num + 1].ModuleCode + "'" + ")";
                                                //SQL.ExecuteUpdate(InsertStr2);
                                                //Log.WriteLog("OEE_DT安全门打开:" + OEE_DT2);
                                                //var rst2 = RequestAPI.Request(Global.inidata.productconfig.OEE_URL1, Global.inidata.productconfig.OEE_URL2, IP, Mac, Global.inidata.productconfig.OEE_Dsn, Global.inidata.productconfig.OEE_authCode, 2, OEE_DT2, out msg2);

                                                ///0909
                                                /// 
                                                ///Night
                                                /// 
                                                //string poorNum1 = string.Empty;
                                                //string TotalNum1 = string.Empty;
                                                //if (Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("18:00")) >= 0 || Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("6:00")) < 0)
                                                //{
                                                //    poorNum1 = (Global.Product_Total_N - Global.Product_OK_N).ToString();
                                                //    TotalNum1 = Global.Product_Total_N.ToString();
                                                //}
                                                //else
                                                //{
                                                //    poorNum1 = (Global.Product_Total_D - Global.Product_OK_D).ToString();
                                                //    TotalNum1 = Global.Product_Total_D.ToString();
                                                //}
                                                //Goee.UploadDowntime(poorNum1, TotalNum1, Global.errorStatus, Global.errorcode, Global.ed[Global.Error_num + 1].ModuleCode, false, date, Global.errorinfo + ",安全门打开");
                                                ///



                                                //if (rst2)
                                                //{
                                                //    _homefrm.AppendRichText(Global.errorcode + ",触发时间=" + Global.ed[Global.Error_num + 1].start_time + ",运行状态:" + Global.errorStatus + ",故障描述:" + Global.errorinfo + ",安全门打开自动发送成功", "rtx_DownTimeMsg");
                                                //    Log.WriteLog("OEE_DT安全门打开自动errorCode发送成功");
                                                //    Global.ConnectOEEFlag = true;
                                                //}
                                                //else
                                                //{
                                                //    _homefrm.AppendRichText(Global.errorcode + ",触发时间=" + Global.ed[Global.Error_num + 1].start_time + ",运行状态:" + Global.errorStatus + ",故障描述:" + Global.errorinfo + ",安全门打开自动发送失败", "rtx_DownTimeMsg");
                                                //    Log.WriteLog("OEE_DT安全门打开自动errorCode发送失败");
                                                //    Global.ConnectOEEFlag = false;
                                                //    string s = "insert into OEE_DTSendNG([DateTime],[Product],[Status],[ErrorCode],[EventTime],[ModuleCode],[Moduleinfo],[errorinfo])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + "OEE_DT" + "'" + "," + "'" + Global.errorStatus + "'" + "," + "'" + Global.errorcode + "'" + ","
                                                //     + "'" + Global.ed[Global.Error_num + 1].start_time + "'" + "," + "'" + "" + "'" + "," + "'" + "" + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorinfo + "'" + ")";
                                                //    int r = SQL.ExecuteUpdate(s);
                                                //    Log.WriteLog(string.Format("插入了{0}行OEE_DownTime缓存数据", r));
                                                //}
                                                string InsertOEEStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.errorcode + "'" + "," + "'" + date + "'" + "," + "'" + "" + "'" + "," + "'" + Global.errorStatus + "'" + "," + "'" + Global.errorinfo + "'" + "," + "'" + ts + "'" + ")";
                                                SQL.ExecuteUpdate(InsertOEEStr);

                                                _homefrm.AppendRichText(date + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.errorcode + "," + Global.errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                                                //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.errorcode + "," + Global.ed[Global.Error_num + 1].start_time + "," + "" + "," + "自动发送成功" + "," + Global.errorStatus + "," + Global.errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                                            }
                                            else//机台处于其它异常状态中
                                            {
                                                string InsertOEEStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorCode + "'" + "," + "'" + Global.ed[Global.Error_num + 1].start_time + "'" + "," + "'" + Global.ed[Global.Error_num + 1].ModuleCode + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                                                SQL.ExecuteUpdate(InsertOEEStr);

                                                _homefrm.AppendRichText(Global.ed[Global.Error_num + 1].start_time + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.Error_num + 1].errorCode + "," + Global.ed[Global.Error_num + 1].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
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
                                            string date = Global.ed[Global.Error_Stopnum + 1].start_time;
                                            //OEE_DT2 = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", Global.ed[Global.Error_Stopnum + 1].errorStatus, Global.ed[Global.Error_Stopnum + 1].errorCode, date, "");
                                            //string InsertStr2 = "insert into OEE_TraceDT([DateTime],[Status],[ErrorCode],[EventTime],[ModuleCode])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorCode + "'" + "," + "'" + date + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].ModuleCode + "'" + ")";
                                            //SQL.ExecuteUpdate(InsertStr2);
                                            //Log.WriteLog("OEE_DT人工停止复位:" + OEE_DT2);
                                            //var rst2 = RequestAPI.Request(Global.inidata.productconfig.OEE_URL1, Global.inidata.productconfig.OEE_URL2, IP, Mac, Global.inidata.productconfig.OEE_Dsn, Global.inidata.productconfig.OEE_authCode, 2, OEE_DT2, out msg2);

                                            ///0909
                                            /// 
                                            ///Night
                                            /// 
                                            //string poorNum2 = string.Empty;
                                            //string TotalNum2 = string.Empty;
                                            //if (Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("18:00")) >= 0 || Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("6:00")) < 0)
                                            //{
                                            //    poorNum2 = (Global.Product_Total_N - Global.Product_OK_N).ToString();
                                            //    TotalNum2 = Global.Product_Total_N.ToString();
                                            //}
                                            //else
                                            //{
                                            //    poorNum2 = (Global.Product_Total_D - Global.Product_OK_D).ToString();
                                            //    TotalNum2 = Global.Product_Total_D.ToString();
                                            //}
                                            //Goee.UploadDowntime(poorNum2, TotalNum2, Global.ed[Global.Error_Stopnum + 1].errorStatus, Global.ed[Global.Error_Stopnum + 1].errorCode, Global.ed[Global.Error_Stopnum + 1].ModuleCode, false, date, Global.ed[Global.Error_Stopnum + 1].errorinfo + ",人工停止复位");
                                            ///




                                            //if (rst2)
                                            //{
                                            //    _homefrm.AppendRichText(Global.ed[Global.Error_Stopnum + 1].errorCode + ",触发时间=" + Global.ed[Global.Error_Stopnum + 1].start_time + ",运行状态:" + Global.ed[Global.Error_Stopnum + 1].errorStatus + ",故障描述:" + Global.ed[Global.Error_Stopnum + 1].errorinfo + ",人工停止复位自动发送成功", "rtx_DownTimeMsg");
                                            //    Log.WriteLog("OEE_DT人工停止复位自动errorCode发送成功");
                                            //    Global.ConnectOEEFlag = true;
                                            //}
                                            //else
                                            //{
                                            //    _homefrm.AppendRichText(Global.ed[Global.Error_Stopnum + 1].errorCode + ",触发时间=" + Global.ed[Global.Error_Stopnum + 1].start_time + ",运行状态:" + Global.ed[Global.Error_Stopnum + 1].errorStatus + ",故障描述:" + Global.ed[Global.Error_Stopnum + 1].errorinfo + ",人工停止复位自动发送失败", "rtx_DownTimeMsg");
                                            //    Log.WriteLog("OEE_DT人工停止复位自动errorCode发送失败");
                                            //    Global.ConnectOEEFlag = false;
                                            //    string s = "insert into OEE_DTSendNG([DateTime],[Product],[Status],[ErrorCode],[EventTime],[ModuleCode],[Moduleinfo],[errorinfo])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + "OEE_DT" + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorCode + "'" + ","
                                            //     + "'" + date + "'" + "," + "'" + "" + "'" + "," + "'" + "" + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorinfo + "'" + ")";
                                            //    int r = SQL.ExecuteUpdate(s);
                                            //    Log.WriteLog(string.Format("插入了{0}行OEE_DownTime缓存数据", r));
                                            //}
                                            string InsertOEEStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorCode + "'" + "," + "'" + date + "'" + "," + "'" + "" + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                                            SQL.ExecuteUpdate(InsertOEEStr);

                                            _homefrm.AppendRichText(date + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.Error_Stopnum + 1].errorCode + "," + Global.ed[Global.Error_Stopnum + 1].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                                            //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[Global.Error_Stopnum + 1].errorCode + "," + Global.ed[Global.Error_Stopnum + 1].start_time + "," + "" + "," + "自动发送成功" + "," + Global.ed[Global.Error_Stopnum + 1].errorStatus + "," + Global.ed[Global.Error_Stopnum + 1].errorinfo + "," + ts, @"F:\装机软件\系统配置\System_ini\");
                                        }
                                    }
                                    else
                                    {
                                        _manualfrm.Btn_UpLoad_break_Click(null, null);
                                    }

                                    //string OEE_DT = "";
                                    //string msg = "";
                                    string EventTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    Global.ed[57].start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//空跑开始时间
                                    //OEE_DT = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", "5", Global.ed[57].errorCode, EventTime, "");
                                    //string InsertStr = "insert into OEE_TraceDT([DateTime],[Status],[ErrorCode],[EventTime],[ModuleCode])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + "5" + "'" + "," + "'" + Global.ed[280].errorCode + "'" + "," + "'" + EventTime + "'" + "," + "'" + "" + "'" + ")";
                                    //SQL.ExecuteUpdate(InsertStr);
                                    //Log.WriteLog("OEE_DT空跑:" + OEE_DT);
                                    //var rst = RequestAPI.Request(Global.inidata.productconfig.OEE_URL1, Global.inidata.productconfig.OEE_URL2, IP, Mac, Global.inidata.productconfig.OEE_Dsn, Global.inidata.productconfig.OEE_authCode, 2, OEE_DT, out msg);


                                    ///0909
                                    /// 
                                    ///Night
                                    /// 
                                    //string poorNum = string.Empty;
                                    //string TotalNum = string.Empty;
                                    //if (Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("18:00")) >= 0 || Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("6:00")) < 0)
                                    //{
                                    //    poorNum = (Global.Product_Total_N - Global.Product_OK_N).ToString();
                                    //    TotalNum = Global.Product_Total_N.ToString();
                                    //}
                                    //else
                                    //{
                                    //    poorNum = (Global.Product_Total_D - Global.Product_OK_D).ToString();
                                    //    TotalNum = Global.Product_Total_D.ToString();
                                    //}
                                    //Goee.UploadDowntime(poorNum, TotalNum, "5", Global.ed[280].errorCode, "", false, EventTime, "空跑");
                                    ///



                                    //if (rst)
                                    //{
                                    //    _homefrm.AppendRichText(Global.ed[280].errorCode + ",触发时间=" + EventTime + ",运行状态:" + "5" + ",故障描述:" + "空跑" + ",自动发送成功", "rtx_DownTimeMsg");
                                    //    Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[280].errorCode + "," + EventTime + "," + "手动发送成功" + "," + "5" + "," + "空跑", @"F:\装机软件\系统配置\System_ini\");
                                    //    Log.WriteLog("OEE_DT机台空跑发送成功");
                                    //    Global.ConnectOEEFlag = true;
                                    //}
                                    //else
                                    //{
                                    //    _homefrm.AppendRichText(Global.ed[280].errorCode + ",触发时间=" + EventTime + ",运行状态:" + "5" + ",故障描述:" + "空跑" + ",自动发送失败", "rtx_DownTimeMsg");
                                    //    Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + Global.ed[280].errorCode + "," + EventTime + "," + "手动发送失败" + "," + "5" + "," + "空跑", @"F:\装机软件\系统配置\System_ini\");
                                    //    Log.WriteLog("OEE_DT机台空跑发送失败");
                                    //    Global.ConnectOEEFlag = false;
                                    //    string s = "insert into OEE_DTSendNG([DateTime],[Product],[Status],[ErrorCode],[EventTime],[ModuleCode],[Moduleinfo],[errorinfo])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + "OEE_DT" + "'" + "," + "'" + "5" + "'" + "," + "'" + Global.ed[280].errorCode + "'" + ","
                                    //        + "'" + EventTime + "'" + "," + "'" + "" + "'" + "," + "'" + "" + "'" + "," + "'" + "空跑" + "'" + ")";
                                    //    int r = SQL.ExecuteUpdate(s);
                                    //    Log.WriteLog(string.Format("插入了{0}行OEE_DownTime缓存数据", r));
                                    //}
                                    Global.j = -1;
                                    string InsertOEEStr3 = "insert into OEE_StartTime([Status],[ErrorCode],[EventTime],[ModuleCode],[Name])" + " " + "values(" + "'" + "5" + "'" + "," + "'" + Global.ed[57].errorCode + "'" + "," + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + "" + "'" + "," + "'" + Global.ed[57].errorinfo + "'" + ")";
                                    SQL.ExecuteUpdate(InsertOEEStr3);//插入空跑开始时间

                                    _homefrm.AppendRichText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[57].errorCode + "," + Global.ed[57].errorinfo, "Rtxt_OEE_Detail");
                                }
                            }
                        }
                        else//处于手动(首件)状态
                        {
                            Global.j = -1;
                            Global.SelectTestRunModel = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLog("OEE-DwonTime异常！" + ex.ToString() + ",OEELog");
                        //_homefrm.AppendRichText("OEE-DwonTime异常！", "rtx_DownTimeMsg");
                    }
                }
                Thread.Sleep(100);
            }
        }

        private void OEE_DT()
        {
            while (true)
            {
                try
                {
                    short Trg_OEE = Global.PLC_Read_Short[20];//现在的状态

                    if (Trg_OEE != Global.OEE_Code)//状态发生变化，则需要记录，进行状态切换
                    {
                        if (Global.ed.ContainsKey(Trg_OEE))
                        {
                            DateTime t1 = Convert.ToDateTime(Global.start_time);//上笔状态的开始时间
                            Global.start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            DateTime t2 = DateTime.Now;//上笔状态的结束时间
                            string ts = (t2 - t1).TotalMinutes.ToString("0.00");


                            string InsertStr2 = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + t2.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.OEE_Code].errorCode + "'" + "," + "'" + t1.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.OEE_Code].ModuleCode + "'" + "," + "'" + Global.ed[Global.OEE_Code].errorStatus + "'" + "," + "'" + Global.ed[Global.OEE_Code].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                            SQL.ExecuteUpdate(InsertStr2);

                            _homefrm.AppendRichText(t1.ToString("yyyy-MM-dd HH:mm:ss.fff") + " -> " + t2.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.OEE_Code].errorCode + "," + Global.ed[Global.OEE_Code].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");

                            Global.OEE_Code = Trg_OEE;//上笔状态记录结束，将当前状态给OEE_Code
                            _homefrm.AppendRichText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.OEE_Code].errorCode + "," + Global.ed[Global.OEE_Code].errorinfo, "Rtxt_OEE_Detail");
                        }
                        else
                        {
                            Log.WriteLog("PLC给定值不在报警目录中！" + Trg_OEE.ToString() + ",OEELog");
                        }
                    }

                }
                catch (Exception ex)
                {
                    Log.WriteLog("OEE-DwonTime异常！" + ex.ToString() + ",OEELog");
                }
                Thread.Sleep(1000);
            }
        }

        private void EthDownTime_New()//新版OEE逻辑
        {
            #region 记录OEE 关闭软件时长
            try
            {
                string SelectStr = "select * from OEE_MCOff where  1=1";
                DataTable d1 = SQL.ExecuteQuery(SelectStr);
                string StartTime = string.Empty;
                if (d1.Rows.Count > 0)//判断上一次是否正常关闭软件-有正常关机时间
                {
                    StartTime = d1.Rows[0][1].ToString();
                    DateTime T1 = Convert.ToDateTime(StartTime);
                    DateTime T2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    string TS = (T2 - T1).TotalMinutes.ToString("0.00");
                    string InsertOEEStr3 = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + "10010001" + "'" + "," + "'" + (Convert.ToDateTime(StartTime)).ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + "" + "'" + "," + "'" + "6" + "'" + "," + "'" + "软件关闭" + "'" + "," + "'" + TS + "'" + ")";
                    SQL.ExecuteUpdate(InsertOEEStr3);

                    _homefrm.AppendRichText(StartTime + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ",10010001," + "软件关闭," + TS + "分钟", "Rtxt_OEE_TimeSpan");


                    //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + "10010001" + "," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + "" + "," + "自动发送成功" + "," + "6" + "," + "软件关闭" + "," + TS, @"E:\装机软件\系统配置\System_ini\");
                }
                else//非正常关机,更新最后一条结束时间
                {
                    string InsertOEEStr3 = "Update OEE_DT set DateTime = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' where ID = (select MAX(ID) from OEE_DT)";
                    SQL.ExecuteUpdate(InsertOEEStr3);

                    //Log.WriteCSV(DateTime.Now.ToString("HH:mm:ss") + "," + "10010001" + "," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + "" + "," + "自动发送成功" + "," + "6" + "," + "软件关闭" + "," + TS, @"E:\装机软件\系统配置\System_ini\");
                }
                string DeleteOEEStr = "delete OEE_MCOff";
                SQL.ExecuteUpdate(DeleteOEEStr);//清空关机时间
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString() + ",OEELog");
            }

            #endregion

            while (true)
            {
                try
                {
                    //OperateResult<short> r1 = Global.plc1.ReadInt16("MW62001");
                    //OEEStatus_Now = r1.Content;//报警目录对应OEE细节

                    if (OEEStatus_Now != OEEStatus)//OEE细节发生改变
                    {
                        if (OEEStatus != -1)//不是刚开机
                        {
                            DateTime t1 = Convert.ToDateTime(startTime);
                            DateTime t2 = DateTime.Now;
                            string ts = (t2 - t1).TotalMinutes.ToString("0.00");
                            _homefrm.AppendRichText(startTime + " -> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[OEEStatus].errorCode + "," + Global.ed[OEEStatus].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
                            string InsertStr = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[OEEStatus].errorCode + "'" + "," + "'" + startTime + "'" + "," + "'" + Global.ed[OEEStatus].ModuleCode + "'" + "," + "'" + Global.ed[OEEStatus].errorStatus + "'" + "," + "'" + Global.ed[OEEStatus].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                            SQL.ExecuteUpdate(InsertStr);
                            OEEStatus = OEEStatus_Now;
                            startTime = t2.ToString("yyyy-MM-dd HH:mm:ss.fff");//下一个状态开始时间
                        }
                        else
                        {
                            startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//开机初始时间
                            OEEStatus = OEEStatus_Now;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLog("OEE-DwonTime异常！" + ex.ToString() + ",OEELog");
                }
                Thread.Sleep(100);
            }
        }

        private void AutoStopBreak(object ob)
        {
            while (true)
            {
                if (Global.BreakStatus && ReadStatus != 1)//机台吃饭休息状态并且不是待料时
                {
                    _manualfrm.Btn_UpLoad_break_Click(null, null);
                }
                Thread.Sleep(1000);
            }
        }


        private void StopStatus()//OEE 处于人工停止状态
        {

            Global.STOP = true;
            Global.j = ReadStatus;
            Global.Error_Stopnum = ReadStatus_Stop;
            Global.ed[Global.Error_Stopnum + 1].start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Log.WriteLog(Global.ed[Global.Error_Stopnum + 1].Moduleinfo + "_" + Global.ed[Global.Error_Stopnum + 1].errorinfo + "：开始计时 " + Global.ed[Global.Error_Stopnum + 1].start_time);
            if (Global.Error_num == 55)//机台打开安全门
            {
                //Global.PLC_Client.WritePLC_D(10020, new short[] { 2 });//未手动选择打开安全门原因，机台不能运行
                //Global.plc1.Write("MW62006", Convert.ToInt16(2));
            }
            string InsertOEEStr = "insert into OEE_StartTime([Status],[ErrorCode],[EventTime],[ModuleCode],[Name])" + " " + "values(" + "'" + Global.ed[Global.Error_Stopnum + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorCode + "'" + "," + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].ModuleCode + "'" + "," + "'" + Global.ed[Global.Error_Stopnum + 1].errorinfo + "'" + ")";
            SQL.ExecuteUpdate(InsertOEEStr);//插入人工停止复位开始时间

            _homefrm.AppendRichText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.Error_Stopnum + 1].errorCode + "," + Global.ed[Global.Error_Stopnum + 1].errorinfo, "Rtxt_OEE_Detail");
        }
        private void ErrorStatus()//OEE 处于异常状态
        {
            Global.j = ReadStatus;
            Global.Error_num = ReadStatus_Error;
            Global.ed[Global.Error_num + 1].start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Log.WriteLog(Global.ed[Global.Error_num + 1].Moduleinfo + "_" + Global.ed[Global.Error_num + 1].errorinfo + "：开始计时 " + Global.ed[Global.Error_num + 1].start_time + ",OEELog");
            if (Global.Error_num == 55)//机台打开安全门
            {
                //Global.PLC_Client.WritePLC_D(10020, new short[] { 2 });//未手动选择打开安全门原因，机台不能运行
                //Global.plc1.Write("MW62006", Convert.ToInt16(2));

            }
            else
            {
                //string OEE_DT = "";
                //string msg = "";
                //string date = Global.ed[Global.Error_num + 1].start_time;
                //OEE_DT = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", Global.ed[Global.Error_num + 1].errorStatus, Global.ed[Global.Error_num + 1].errorCode, date, Global.ed[Global.Error_num + 1].ModuleCode);
                //string InsertStr = "insert into OEE_TraceDT([DateTime],[Status],[ErrorCode],[EventTime],[ModuleCode])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorCode + "'" + "," + "'" + date + "'" + "," + "'" + Global.ed[Global.Error_num + 1].ModuleCode + "'" + ")";
                //SQL.ExecuteUpdate(InsertStr);
                //Log.WriteLog("OEE_DT:" + OEE_DT + ",OEELog");
                //var rst = RequestAPI.Request(Global.inidata.productconfig.OEE_URL1, Global.inidata.productconfig.OEE_URL2, IP, Mac, Global.inidata.productconfig.OEE_Dsn, Global.inidata.productconfig.OEE_authCode, 2, OEE_DT, out msg);
                ///0924
                //string poorNum = string.Empty;
                //string TotalNum = string.Empty;
                //if (Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("18:00")) >= 0 || Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("6:00")) < 0)
                //{
                //    poorNum = (Global.Product_Total_N - Global.Product_OK_N).ToString();
                //    TotalNum = Global.Product_Total_N.ToString();
                //}
                //else
                //{
                //    poorNum = (Global.Product_Total_D - Global.Product_OK_D).ToString();
                //    TotalNum = Global.Product_Total_D.ToString();
                //}
                //Goee.UploadDowntime(poorNum, TotalNum, Global.ed[Global.Error_num + 1].errorStatus, Global.ed[Global.Error_num + 1].errorCode, Global.ed[Global.Error_num + 1].ModuleCode, false, date, Global.ed[Global.Error_num + 1].errorinfo);
                ///0924


                //if (rst)
                //{
                //    _homefrm.AppendRichText(Global.ed[Global.Error_num + 1].ModuleCode + "," + Global.ed[Global.Error_num + 1].errorCode + ",触发时间=" + Global.ed[Global.Error_num + 1].start_time + ",运行状态:" + Global.ed[Global.Error_num + 1].errorStatus + ",故障描述:" + Global.ed[Global.Error_num + 1].errorinfo + ",自动发送成功", "rtx_DownTimeMsg");
                //    Log.WriteLog("OEE_DT自动errorCode发送成功" + ",OEELog");
                //    Global.ConnectOEEFlag = true;
                //}
                //else
                //{
                //    _homefrm.AppendRichText(Global.ed[Global.Error_num + 1].ModuleCode + "," + Global.ed[Global.Error_num + 1].errorCode + ",触发时间=" + Global.ed[Global.Error_num + 1].start_time + ",运行状态:" + Global.ed[Global.Error_num + 1].errorStatus + ",故障描述:" + Global.ed[Global.Error_num + 1].errorinfo + ",自动发送失败", "rtx_DownTimeMsg");
                //    Log.WriteLog("OEE_DT自动errorCode发送失败" + ",OEELog");
                //    Global.ConnectOEEFlag = false;
                //    string s = "insert into OEE_DTSendNG([DateTime],[Product],[Status],[ErrorCode],[EventTime],[ModuleCode],[Moduleinfo],[errorinfo])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + "OEE_DT" + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorCode + "'" + ","
                //       + "'" + Global.ed[Global.Error_num + 1].start_time + "'" + "," + "'" + Global.ed[Global.Error_num + 1].ModuleCode + "'" + "," + "'" + Global.ed[Global.Error_num + 1].Moduleinfo + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorinfo + "'" + ")";
                //    int r = SQL.ExecuteUpdate(s);
                //    Log.WriteLog(string.Format("插入了{0}行OEE_DownTime缓存数据", r) + ",OEELog");
                //}
                _manualfrm.labelcolor(Color.Transparent, "未选择", "LB_ManualSelect");
                //ButtonFlag(false, errortime_But);
                //ButtonFlag(false, button22);
                string InsertOEEStr = "insert into OEE_StartTime([Status],[ErrorCode],[EventTime],[ModuleCode],[Name])" + " " + "values(" + "'" + Global.ed[Global.Error_num + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorCode + "'" + "," + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.Error_num + 1].ModuleCode + "'" + "," + "'" + Global.ed[Global.Error_num + 1].errorinfo + "'" + ")";
                SQL.ExecuteUpdate(InsertOEEStr);//插入异常开始时间

                _homefrm.AppendRichText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.Error_num + 1].errorCode + "," + Global.ed[Global.Error_num + 1].errorinfo, "Rtxt_OEE_Detail");
            }
        }
        private void RunStatus()//OEE 处于运行状态
        {
            Global.SelectManualErrorCode = false;//结束手动选择ErrorCode状态
            Global.j = ReadStatus;
            Global.ed[Global.j].start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Log.WriteLog(Global.ed[Global.j].Moduleinfo + "_" + Global.ed[Global.j].errorinfo + "：开始计时 " + Global.ed[Global.j].start_time + ",OEELog");
            //string OEE_DT = "";
            //string msg = "";
            //string date = Global.ed[Global.j].start_time;
            //OEE_DT = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", Global.ed[Global.j].errorStatus, "", date, Global.ed[Global.j].ModuleCode);
            //string InsertStr = "insert into OEE_TraceDT([DateTime],[Status],[ErrorCode],[EventTime],[ModuleCode])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Global.ed[Global.j].errorStatus + "'" + "," + "'" + "" + "'" + "," + "'" + date + "'" + "," + "'" + Global.ed[Global.j].ModuleCode + "'" + ")";
            //SQL.ExecuteUpdate(InsertStr);
            //Log.WriteLog("OEE_DT:" + OEE_DT + ",OEELog");
            //var rst = RequestAPI.Request(Global.inidata.productconfig.OEE_URL1, Global.inidata.productconfig.OEE_URL2, IP, Mac, Global.inidata.productconfig.OEE_Dsn, Global.inidata.productconfig.OEE_authCode, 2, OEE_DT, out msg);

            ///0924
            //string poorNum = string.Empty;
            //string TotalNum = string.Empty;
            //if (Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("18:00")) >= 0 || Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("6:00")) < 0)
            //{
            //    poorNum = (Global.Product_Total_N - Global.Product_OK_N).ToString();
            //    TotalNum = Global.Product_Total_N.ToString();
            //}
            //else
            //{
            //    poorNum = (Global.Product_Total_D - Global.Product_OK_D).ToString();
            //    TotalNum = Global.Product_Total_D.ToString();
            //}
            //Goee.UploadDowntime(poorNum, TotalNum, Global.ed[Global.j].errorStatus, "", Global.ed[Global.j].ModuleCode, false, date, Global.ed[Global.j].errorinfo);
            ///0924


            //if (rst)
            //{
            //    _homefrm.AppendRichText(Global.ed[Global.j].ModuleCode + "," + "Run" + ",触发时间=" + Global.ed[Global.j].start_time + ",运行状态:" + Global.ed[Global.j].errorStatus + ",故障描述:" + Global.ed[Global.j].errorinfo + ",自动发送成功", "rtx_DownTimeMsg");
            //    Log.WriteLog("OEE_DT自动errorCode发送成功" + ",OEELog");
            //    Global.ConnectOEEFlag = true;
            //}
            //else
            //{
            //    _homefrm.AppendRichText(Global.ed[Global.j].ModuleCode + "," + "Run" + ",触发时间=" + Global.ed[Global.j].start_time + ",运行状态:" + Global.ed[Global.j].errorStatus + ",故障描述:" + Global.ed[Global.j].errorinfo + ",自动发送失败", "rtx_DownTimeMsg");
            //    Log.WriteLog("OEE_DT自动errorCode发送失败" + ",OEELog");
            //    Global.ConnectOEEFlag = false;
            //    string s = "insert into OEE_DTSendNG([DateTime],[Product],[Status],[ErrorCode],[EventTime],[ModuleCode],[Moduleinfo],[errorinfo])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + "OEE_DT" + "'" + "," + "'" + Global.ed[Global.j].errorStatus + "'" + "," + "'" + Global.ed[Global.j].errorCode + "'" + ","
            //           + "'" + Global.ed[Global.j].start_time + "'" + "," + "'" + Global.ed[Global.j].ModuleCode + "'" + "," + "'" + Global.ed[Global.j].Moduleinfo + "'" + "," + "'" + Global.ed[Global.j].errorinfo + "'" + ")";
            //    int r = SQL.ExecuteUpdate(s);
            //    Log.WriteLog(string.Format("插入了{0}行OEE_DownTime缓存数据", r) + ",OEELog");
            //}
            _manualfrm.labelcolor(Color.Transparent, "未选择", "LB_ManualSelect");
            //ButtonFlag(true, errortime_But);
            //ButtonFlag(true, button22);
            string InsertOEEStr = "insert into OEE_StartTime([Status],[ErrorCode],[EventTime],[ModuleCode],[Name])" + " " + "values(" + "'" + Global.ed[Global.j].errorStatus + "'" + "," + "'" + Global.ed[Global.j].errorCode + "'" + "," + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.j].ModuleCode + "'" + "," + "'" + Global.ed[Global.j].errorinfo + "'" + ")";
            SQL.ExecuteUpdate(InsertOEEStr);//插入运行开始时间

            _homefrm.AppendRichText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.j].errorCode + "," + Global.ed[Global.j].errorinfo, "Rtxt_OEE_Detail");
        }
        private void PendingStatus()//OEE 处于待料状态
        {
            Global.j = ReadStatus;
            Global.Error_PendingNum = ReadStatus_Pending;//待料细节字
            Global.ed[Global.Error_PendingNum + 1].start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Log.WriteLog(Global.ed[Global.Error_PendingNum + 1].Moduleinfo + "_" + Global.ed[Global.Error_PendingNum + 1].errorinfo + "：开始计时 " + Global.ed[Global.Error_PendingNum + 1].start_time + ",OEELog");
            //string OEE_DT = "";
            //string msg = "";
            //string date = Global.ed[Global.Error_PendingNum + 1].start_time;
            //OEE_DT = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", Global.ed[Global.Error_PendingNum + 1].errorStatus, Global.ed[Global.Error_PendingNum + 1].errorCode, date, Global.ed[Global.Error_PendingNum + 1].ModuleCode);
            //string InsertStr = "insert into OEE_TraceDT([DateTime],[Status],[ErrorCode],[EventTime],[ModuleCode])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorCode + "'" + "," + "'" + date + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].ModuleCode + "'" + ")";
            //SQL.ExecuteUpdate(InsertStr);
            //Log.WriteLog("OEE_DT:" + OEE_DT + ",OEELog");
            //var rst = RequestAPI.Request(Global.inidata.productconfig.OEE_URL1, Global.inidata.productconfig.OEE_URL2, IP, Mac, Global.inidata.productconfig.OEE_Dsn, Global.inidata.productconfig.OEE_authCode, 2, OEE_DT, out msg);

            ///0924
            //string poorNum = string.Empty;
            //string TotalNum = string.Empty;
            //if (Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("18:00")) >= 0 || Convert.ToDateTime(DateTime.Now.ToString("HH:mm")).CompareTo(Convert.ToDateTime("6:00")) < 0)
            //{
            //    poorNum = (Global.Product_Total_N - Global.Product_OK_N).ToString();
            //    TotalNum = Global.Product_Total_N.ToString();
            //}
            //else
            //{
            //    poorNum = (Global.Product_Total_D - Global.Product_OK_D).ToString();
            //    TotalNum = Global.Product_Total_D.ToString();
            //}
            //Goee.UploadDowntime(poorNum, TotalNum, Global.ed[Global.Error_PendingNum + 1].errorStatus, Global.ed[Global.Error_PendingNum + 1].errorCode, Global.ed[Global.Error_PendingNum + 1].ModuleCode, false, date, Global.ed[Global.Error_PendingNum + 1].errorinfo);
            ///0924

            //if (rst)
            //{
            //    _homefrm.AppendRichText(Global.ed[Global.Error_PendingNum + 1].ModuleCode + "," + Global.ed[Global.Error_PendingNum + 1].errorCode + ",触发时间=" + Global.ed[Global.Error_PendingNum + 1].start_time + ",运行状态:" + Global.ed[Global.Error_PendingNum + 1].errorStatus + ",故障描述:" + Global.ed[Global.Error_PendingNum + 1].errorinfo + ",自动发送成功", "rtx_DownTimeMsg");
            //    Log.WriteLog("OEE_DT自动errorCode发送成功" + ",OEELog");
            //    Global.ConnectOEEFlag = true;
            //}
            //else
            //{
            //    _homefrm.AppendRichText(Global.ed[Global.Error_PendingNum + 1].ModuleCode + "," + Global.ed[Global.Error_PendingNum + 1].errorCode + ",触发时间=" + Global.ed[Global.Error_PendingNum + 1].start_time + ",运行状态:" + Global.ed[Global.Error_PendingNum + 1].errorStatus + ",故障描述:" + Global.ed[Global.Error_PendingNum + 1].errorinfo + ",自动发送失败", "rtx_DownTimeMsg");
            //    Log.WriteLog("OEE_DT自动errorCode发送失败" + ",OEELog");
            //    Global.ConnectOEEFlag = false;
            //    string s = "insert into OEE_DTSendNG([DateTime],[Product],[Status],[ErrorCode],[EventTime],[ModuleCode],[Moduleinfo],[errorinfo])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + "OEE_DT" + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorCode + "'" + ","
            //          + "'" + Global.ed[Global.Error_PendingNum + 1].start_time + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].ModuleCode + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].Moduleinfo + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorinfo + "'" + ")";
            //    int r = SQL.ExecuteUpdate(s);
            //    Log.WriteLog(string.Format("插入了{0}行OEE_DownTime缓存数据", r) + ",OEELog");
            //}
            _manualfrm.labelcolor(Color.Transparent, "未选择", "LB_ManualSelect");
            //ButtonFlag(false, errortime_But);
            //ButtonFlag(false, button22);
            string InsertOEEStr = "insert into OEE_StartTime([Status],[ErrorCode],[EventTime],[ModuleCode],[Name])" + " " + "values(" + "'" + Global.ed[Global.Error_PendingNum + 1].errorStatus + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorCode + "'" + "," + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].ModuleCode + "'" + "," + "'" + Global.ed[Global.Error_PendingNum + 1].errorinfo + "'" + ")";
            SQL.ExecuteUpdate(InsertOEEStr);//插入待料开始时间

            _homefrm.AppendRichText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.Error_PendingNum + 1].errorCode + "," + Global.ed[Global.Error_PendingNum + 1].errorinfo, "Rtxt_OEE_Detail");
        }


        #endregion



        #region 内存回收
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        /// <summary>
        /// 释放内存
        /// </summary>
        public static void ClearMemory(object obj)
        {
            while (true)
            {
                try
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    {
                        //FrmMain为我窗体的类名
                        SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString());
                }
                Thread.Sleep(10000);
            }

        }

        #endregion

    }
}
