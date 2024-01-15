using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using System.IO.Ports;
using System.Threading;
using FlHelper.Models;
using FlHelper;
using System.IO;
using FlHelper.Helpers;
using Label = System.Windows.Forms.Label;

namespace 通讯接口测试_JGP__20200415
{
    public partial class MainForm : Form
    {

        [DllImport("kernel32.dll"/*, SetLastError = false*/)]
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

        string hashCode_newstn = string.Empty;
        string billNo_newstn = string.Empty;
        string requestId_check = string.Empty;
        string curr_time = string.Empty;

        string checkURL = "http://localhost:8965/dochk";  //"http://localhost:8965/dochk";//上料确认URL
        string passURL = string.Empty;   //"http://localhost:8965/dopass";//过站URL
        string TraceURL = "http://localhost:8765/v2/log_batch";//TraceURL

        string Trace_id = string.Empty;



        //string SN = "HWWGS7300HL000029L+AD801FLK3ABP02611AG"; //string.Empty;
        ////string SN = "HWWGSA300AR000029L+AF801FLK3ABP02611AX";
        //string left_rail = "HRHGRQL31XW0000032+550YHK103A01";//string.Empty;
        //string right_rail = "HRHGRQL323D0000032+550YHK103A01"; //string.Empty;
        //string fixture = "H-D1BA-SML17-2000-A3-10001";// string.Empty;

        SQLServer Sqlserver = new SQLServer();
        public MainForm()
        {
            InitializeComponent();
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
           //Global.plc1.ConnectServer();
           // Global.plc2.ConnectServer();
           // Global.plc3.ConnectServer();
           // this.cmbPort.DataSource = SerialPort.GetPortNames();
           // this.cmbPort.Text = "COM1";

            Sc = new SendSfc();
            Sc.UpdateUI += AppendRichText;
            //string str = "0ms15%/285ms15%/50ms0%";
            //string[] str1 = str.Split(new string[3] { "ms", "%","/" }, StringSplitOptions.RemoveEmptyEntries);
            //string str2 = str1[0];
            //string str21 = str1[1];
            //string str211 = str1[2];
            //string str2111 = str1[3];
            //string str21111 = str1[4];
            //string str211111 = str1[5];
            //ProductCountHelper ph = new ProductCountHelper();
            //Thread t = new Thread(ReadFix);
            //t.Start();


            Sc.DeleteAll("D:\\ZHH\\ThreeMaRecord\\");
            Sc.DeleteAll("D:\\ZHH\\PassMaRecord\\");
            Sc.DeleteAll("D:\\ZHH\\PassModelList记录\\");
            Sc.DeleteAll("E:\\public\\测高记录\\");
            Sc.DeleteAll("D:\\ZHH\\ServerInfo\\");
            Sc.DeleteAll("D:\\ZHH\\操作记录\\");
            Sc.DeleteAll("D:\\ZHH\\HansInfo\\");
            Sc.DeleteAll("D:\\日志文件\\");

        }


        //private void btn_ReadPLC_Click(object sender, EventArgs e)
        //{
        //    InovanceAMTcp plc1 = new InovanceAMTcp("192.168.1.10", 502);
        //    InovanceAMTcp plc2 = new InovanceAMTcp("192.168.1.40", 502);
        //    if (plc1.ConnectServer().IsSuccess && plc2.ConnectServer().IsSuccess)
        //    {
        //        Log.WriteLog("两个PLC连接成功");
        //    }

        //    //读取产品码 左键码 右键码

        //    //产品码
        //    OperateResult<short[]> barcode = plc1.ReadInt16("MW45240", 20);
        //    SN = ToASCII(barcode.Content).Trim();
        //    txt_SN.Text = SN;
        //    //左键码
        //    OperateResult<short[]> left_sn = plc1.ReadInt16("MW45220", 20);
        //    left_rail = ToASCII(left_sn.Content).Trim();
        //    txt_LeftRail.Text = left_rail;
        //    //右键码
        //    OperateResult<short[]> right_sn = plc1.ReadInt16("MW45200", 20);
        //    right_rail = ToASCII(right_sn.Content).Trim();
        //    txt_RightRail.Text = right_rail;
        //    //治具码
        //    //OperateResult<short[]> fixture_sn = plc2.ReadInt16("MW45200", 20);
        //    //fixture = ToASCII(fixture_sn.Content).Trim();
        //    txt_Fixture.Text = fixture;


        //}
        //private void ReadFix() {
        //    while (true)
        //    {
        //        //OperateResult<short> int32_Trg_check = Global.plc3.ReadInt16("MW65100");
        //        //short Trg_check = int32_Trg_check.Content;

        //        //if (Trg_check == 1)
        //        //{
        //            OperateResult<short[]> config_sn = Global.plc2.ReadInt16("MW65060", 20);
        //            string barcode = ToASCII(config_sn.Content).Trim();
        //        if (!string.IsNullOrEmpty(barcode))
        //        {
        //            Log.WriteLog("1#"+barcode);
        //        }
        //        //}
        //        OperateResult<short> int32_Trg_check2 = Global.plc2.ReadInt16("MW65300");
        //        short Trg_check2 = int32_Trg_check2.Content;

        //        //if (Trg_check2 == 1)
        //        //{
        //            OperateResult<short[]> config_sn2 = Global.plc2.ReadInt16("MW65260", 20);
        //            string barcode2 = ToASCII(config_sn2.Content).Trim();
        //        if (!string.IsNullOrEmpty(barcode))
        //        {
        //            Log.WriteLog("2#" + barcode);
        //        }
        //        //}
        //        Thread.Sleep(2000);
        //    }
        //}

        public string ToASCII(short[] PLCData)//十进制转化为ASCII码
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

        //public static bool SetLocalTimeByStr(string timestr)
        //{
        //    bool flag = false;
        //    SYSTEMTIME sysTime = new SYSTEMTIME();
        //    //DateTime dt = Convert.ToDateTime(timestr);
        //    //sysTime.wYear = Convert.ToUInt16(dt.Year); //must be short           
        //    //sysTime.wMonth = Convert.ToUInt16(dt.Month);
        //    //sysTime.wDay = Convert.ToUInt16(dt.Day);
        //    //sysTime.wHour = Convert.ToUInt16(dt.Hour);
        //    //sysTime.wMinute = Convert.ToUInt16(dt.Minute);
        //    //sysTime.wSecond = Convert.ToUInt16(dt.Second);

        //    sysTime.wYear = Convert.ToUInt16(timestr.Substring(0, 4)); //must be short           
        //    sysTime.wMonth = Convert.ToUInt16(timestr.Substring(4, 2));
        //    sysTime.wDay = Convert.ToUInt16(timestr.Substring(6, 2));
        //    sysTime.wHour = Convert.ToUInt16(timestr.Substring(8, 2));
        //    sysTime.wMinute = Convert.ToUInt16(timestr.Substring(10, 2));
        //    sysTime.wSecond = Convert.ToUInt16(timestr.Substring(12, 2));

        //    try
        //    {
        //        flag = SetLocalTime(ref sysTime);
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    return flag;
        //}





        private void btn_TraceSend_Click(object sender, EventArgs e)
        {
            //richTextBox1.Clear();
            //richTextBox2.Clear();
            //JsonSerializerSettings jsetting = new JsonSerializerSettings();
            //jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值

            //TraceMesRequest_ua trace_ua = new TraceMesRequest_ua();




            //trace_ua.serial_type = "rm";
            //trace_ua.serials = new string[3];
            //trace_ua.serials[0] = this.txt_SN.Text;
            //trace_ua.serials[1] = this.txt_LeftRail.Text;
            //trace_ua.serials[2] = this.txt_RightRail.Text;


            //trace_ua.data = new data();
            //trace_ua.data.insight = new Insight();
            //trace_ua.data.insight.test_attributes = new Test_attributes();
            //trace_ua.data.insight.test_station_attributes = new Test_station_attributes();
            //trace_ua.data.insight.uut_attributes = new Uut_attributes();
            //trace_ua.data.insight.results = new Result[72];//30个location，每个location 8个参数
            //for (int i = 0; i < trace_ua.data.insight.results.Length; i++)
            //{
            //    trace_ua.data.insight.results[i] = new Result();

            //    trace_ua.data.insight.results[i].result = "pass";

            //    //i除以8的结果，相当于用来区分每个location
            //    switch (i / 8)
            //    {
            //        //第一个焊点
            //        case 0:
            //            trace_ua.data.insight.results[i].test = "location1_RM2_layer1";
            //            break;
            //        //第二个焊点
            //        case 1:
            //            trace_ua.data.insight.results[i].test = "location2_LM1_layer2";
            //            break;
            //        case 2:
            //            trace_ua.data.insight.results[i].test = "location3_LT_layer3";
            //            break;
            //        case 3:
            //            trace_ua.data.insight.results[i].test = "location4_LM2_layer4";
            //            break;
            //        case 4:
            //            trace_ua.data.insight.results[i].test = "location5_LB_layer5";
            //            break;
            //        case 5:
            //            trace_ua.data.insight.results[i].test = "location6_RM1_layer6";
            //            break;
            //        case 6:
            //            trace_ua.data.insight.results[i].test = "location7_RT_layer7";
            //            break;
            //        case 7:
            //            trace_ua.data.insight.results[i].test = "location8_RM3_layer8";
            //            break;
            //        case 8:
            //            trace_ua.data.insight.results[i].test = "location9_RB_layer9";
            //            break;

            //        #region 短焊点
            //        //case 9:
            //        //    trace_ua.data.insight.results[i].test = "location10_LT6_layer10";
            //        //    break;
            //        //case 10:
            //        //    trace_ua.data.insight.results[i].test = "location11_LT5_layer11";
            //        //    break;
            //        //case 11:
            //        //    trace_ua.data.insight.results[i].test = "location12_LT4_layer12";
            //        //    break;
            //        //case 12:
            //        //    trace_ua.data.insight.results[i].test = "location13_LT3_layer13";
            //        //    break;
            //        //case 13:
            //        //    trace_ua.data.insight.results[i].test = "location14_LT2_layer14";
            //        //    break;
            //        //case 14:
            //        //    trace_ua.data.insight.results[i].test = "location15_LT1_layer15";
            //        //    break;
            //        //case 15:
            //        //    trace_ua.data.insight.results[i].test = "location16_LT10_layer16";
            //        //    break;
            //        //case 16:
            //        //    trace_ua.data.insight.results[i].test = "location17_LT11_layer17";
            //        //    break;
            //        //case 17:
            //        //    trace_ua.data.insight.results[i].test = "location18_LT12_layer18";
            //        //    break;
            //        //case 18:
            //        //    trace_ua.data.insight.results[i].test = "location19_LT13_layer19";
            //        //    break;
            //        //case 19:
            //        //    trace_ua.data.insight.results[i].test = "location20_LT14_layer20";
            //        //    break;
            //        //case 20:
            //        //    trace_ua.data.insight.results[i].test = "location21_LT15_layer21";
            //        //    break;
            //        //case 21:
            //        //    trace_ua.data.insight.results[i].test = "location22_LT16_layer22";
            //        //    break;
            //        //case 22:
            //        //    trace_ua.data.insight.results[i].test = "location23_LT17_layer23";
            //        //    break;
            //        //case 23:
            //        //    trace_ua.data.insight.results[i].test = "location24_LB1_layer24";
            //        //    break;
            //        //case 24:
            //        //    trace_ua.data.insight.results[i].test = "location25_LB2_layer25";
            //        //    break;
            //        //case 25:
            //        //    trace_ua.data.insight.results[i].test = "location26_RM3_layer26";
            //        //    break;
            //        //case 26:
            //        //    trace_ua.data.insight.results[i].test = "location27_RM2_layer27";
            //        //    break;
            //        //case 27:
            //        //    trace_ua.data.insight.results[i].test = "location28_RM1_layer28";
            //        //    break;
            //        //case 28:
            //        //    trace_ua.data.insight.results[i].test = "location29_RT2_layer29";
            //        //    break;
            //        //case 29:
            //        //    trace_ua.data.insight.results[i].test = "location30_RT1_layer30";
            //        //    break; 

            //        #endregion


            //        default:
            //            break;
            //    }
            //    //除以8的余数，相当于用来区分每个焊接参数（不同焊点）
            //    switch (i % 8)
            //    {
            //        //举例：i为0 8 16 24 32.....232，共计30个，，每个焊点的第一个参数
            //        case 0:
            //            trace_ua.data.insight.results[i].sub_test = "power";
            //            trace_ua.data.insight.results[i].units = "W";
            //            trace_ua.data.insight.results[i].value = "15%";
            //            break;
            //        //举例：i为 1 9 17 25...233，共计30个，每个焊点的第二个参数
            //        case 1:
            //            trace_ua.data.insight.results[i].sub_test = "frequency";
            //            trace_ua.data.insight.results[i].units = "KHz";
            //            trace_ua.data.insight.results[i].value = "0";
            //            break;
            //        //每个焊点的第三个参数，波形：有些焊点不一样
            //        case 2:
            //            trace_ua.data.insight.results[i].sub_test = "waveform";
            //            trace_ua.data.insight.results[i].units = "";

            //            if (i / 8 == 0)
            //            {
            //                trace_ua.data.insight.results[i].value = "13";
            //            }
            //            else if (i / 8 == 1)
            //            {
            //                trace_ua.data.insight.results[i].value = "11";
            //            }
            //            else if (i / 8 == 2)
            //            {
            //                trace_ua.data.insight.results[i].value = "12";
            //            }
            //            else if (i / 8 == 3 || i / 8 == 4 || i / 8 == 5 || i / 8 == 7)
            //            {
            //                trace_ua.data.insight.results[i].value = "14";
            //            }
            //            else if (i / 8 == 6)
            //            {
            //                trace_ua.data.insight.results[i].value = "10";
            //            }
            //            else
            //            {
            //                trace_ua.data.insight.results[i].value = "9";
            //            }
            //            break;
            //        case 3:
            //            trace_ua.data.insight.results[i].sub_test = "laser_speed";
            //            trace_ua.data.insight.results[i].units = "mm/s";
            //            trace_ua.data.insight.results[i].value = "40";
            //            break;
            //        case 4:
            //            trace_ua.data.insight.results[i].sub_test = "jump_speed";
            //            trace_ua.data.insight.results[i].units = "mm/s";
            //            trace_ua.data.insight.results[i].value = "250";
            //            break;
            //        case 5:
            //            trace_ua.data.insight.results[i].sub_test = "jump_delay";
            //            trace_ua.data.insight.results[i].units = "us";
            //            trace_ua.data.insight.results[i].value = "0";
            //            break;
            //        case 6:
            //            trace_ua.data.insight.results[i].sub_test = "position_delay";
            //            trace_ua.data.insight.results[i].units = "us";
            //            trace_ua.data.insight.results[i].value = "0";
            //            break;

            //        //波形曲线有变化，不是每个焊点都一样
            //        case 7:
            //            trace_ua.data.insight.results[i].sub_test = "pulse_profile";
            //            trace_ua.data.insight.results[i].units = "";

            //            if (i / 8 == 0)
            //            {
            //                trace_ua.data.insight.results[i].value = "0ms24%/500ms24%/50ms0%";
            //            }
            //            else if (i / 8 == 1)
            //            {
            //                trace_ua.data.insight.results[i].value = "0ms24%/435ms24%/50ms0%";
            //            }
            //            else if (i / 8 == 2)
            //            {
            //                trace_ua.data.insight.results[i].value = "0ms24%/465ms24%/50ms0%";
            //            }
            //            else if (i / 8 == 3 || i / 8 == 4 || i / 8 == 5 || i / 8 == 7)
            //            {
            //                trace_ua.data.insight.results[i].value = "0ms24%/535ms24%/50ms0%";
            //            }
            //            else if (i / 8 == 6)
            //            {
            //                trace_ua.data.insight.results[i].value = "0ms24%/305ms24%/50ms0%";
            //            }
            //            else
            //            {
            //                trace_ua.data.insight.results[i].value = "0ms24%/245ms24%/50ms0%";
            //            }

            //            break;
            //        default:
            //            break;
            //    }


            //}

            //#region 关于results里面的value参数（两个参数有变化，waveform，pulse_profile）
            ////trace_ua.data.insight.results[2].value = "0";
            ////trace_ua.data.insight.results[10].value = "0";
            ////trace_ua.data.insight.results[18].value = "0";
            ////trace_ua.data.insight.results[26].value = "0";
            ////trace_ua.data.insight.results[34].value = "0";
            ////trace_ua.data.insight.results[42].value = "0";
            ////trace_ua.data.insight.results[50].value = "0";
            ////trace_ua.data.insight.results[58].value = "0";
            ////trace_ua.data.insight.results[66].value = "0";
            ////trace_ua.data.insight.results[74].value = "0";
            ////trace_ua.data.insight.results[82].value = "0";
            ////trace_ua.data.insight.results[90].value = "0";
            ////trace_ua.data.insight.results[98].value = "0";
            ////trace_ua.data.insight.results[106].value = "0";
            ////trace_ua.data.insight.results[114].value = "0";
            ////trace_ua.data.insight.results[122].value = "0";
            ////trace_ua.data.insight.results[130].value = "0";
            ////trace_ua.data.insight.results[138].value = "0";
            ////trace_ua.data.insight.results[146].value = "0";
            ////trace_ua.data.insight.results[154].value = "0";
            ////trace_ua.data.insight.results[162].value = "0";
            ////trace_ua.data.insight.results[170].value = "1";
            ////trace_ua.data.insight.results[178].value = "1";
            ////trace_ua.data.insight.results[186].value = "0";
            ////trace_ua.data.insight.results[194].value = "0";
            ////trace_ua.data.insight.results[202].value = "0";
            ////trace_ua.data.insight.results[210].value = "0";
            ////trace_ua.data.insight.results[218].value = "2";
            ////trace_ua.data.insight.results[226].value = "0";
            ////trace_ua.data.insight.results[234].value = "0";

            ////trace_ua.data.insight.results[7].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[15].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[23].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[31].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[39].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[47].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[55].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[63].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[71].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[79].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[87].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[95].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[103].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[111].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[119].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[127].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[135].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[143].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[151].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[159].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[167].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[175].value = "0ms0%/5ms15%/95ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[183].value = "0ms0%/5ms15%/95ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[191].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[199].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[207].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[215].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[223].value = "0ms0%/5ms15%/100ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[231].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%";
            ////trace_ua.data.insight.results[239].value = "0ms0%/5ms15%/85ms15%/10ms10%/300ms10%/20ms0%"; 
            //#endregion


            //trace_ua.data.items = new ExpandoObject();

            //trace_ua.data.insight.test_attributes.test_result = "pass";
            //trace_ua.data.insight.test_attributes.unit_serial_number = this.txt_SN.Text.Substring(0, 17);
            //trace_ua.data.insight.test_attributes.uut_start = DateTime.Now.AddSeconds(-35).ToString("yyyy-MM-dd HH:mm:ss");
            //trace_ua.data.insight.test_attributes.uut_stop = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //trace_ua.data.insight.test_station_attributes.fixture_id = this.txt_Fixture.Text;
            //trace_ua.data.insight.test_station_attributes.head_id = "1";
            //trace_ua.data.insight.test_station_attributes.line_id = Global.Trace_line_id;
            //trace_ua.data.insight.test_station_attributes.software_name = "DEVELOPMENT1";
            //trace_ua.data.insight.test_station_attributes.software_version = Global.Trace_version;
            //trace_ua.data.insight.test_station_attributes.station_id = Global.Trace_station_id;

            //trace_ua.data.insight.uut_attributes.STATION_STRING = "Free-from string";
            //trace_ua.data.insight.uut_attributes.fixture_id = this.txt_Fixture.Text;
            //trace_ua.data.insight.uut_attributes.hatch = "0.04";
            //trace_ua.data.insight.uut_attributes.laser_start_time = DateTime.Now.AddSeconds(-12).ToString("yyyy-MM-dd HH:mm:ss");
            //trace_ua.data.insight.uut_attributes.laser_stop_time = DateTime.Now.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
            //trace_ua.data.insight.uut_attributes.lc_sn = this.txt_SN.Text;

            //trace_ua.data.insight.uut_attributes.left_rail_sn = this.txt_LeftRail.Text;

            //trace_ua.data.insight.uut_attributes.pattern_type = "1";
            //trace_ua.data.insight.uut_attributes.precitec_grading = "NA";
            //trace_ua.data.insight.uut_attributes.precitec_rev = "NA";
            //trace_ua.data.insight.uut_attributes.precitec_value = "NA";

            //trace_ua.data.insight.uut_attributes.right_rail_sn = this.txt_RightRail.Text;

            //trace_ua.data.insight.uut_attributes.spot_size = "NA";

            //trace_ua.data.insight.uut_attributes.station_vendor = "HG";

            //trace_ua.data.insight.uut_attributes.swing_amplitude = "0.4";
            //trace_ua.data.insight.uut_attributes.swing_freq = "10000";
            //trace_ua.data.insight.uut_attributes.tossing_item = "NA";





            //string callResult = "";
            //string errMsg = "";

            //for (int i = 0; i <= 0; i++)
            //{
            //    (trace_ua.data.items as ICollection<KeyValuePair<string, object>>).Add(new KeyValuePair<string, object>("error_" + (i + 1), "00000000" + "_" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            //}
            //string SendTraceLogs = JsonConvert.SerializeObject(trace_ua, Formatting.None, jsetting);
            //richTextBox1.AppendText("Trace上传:" + SendTraceLogs + "\r\n");
            //Log.WriteLog("Trace上传:" + SendTraceLogs);
            //string Trcae_logs_str = string.Empty;

            //RequestAPI2.CallBobcat3(TraceURL, SendTraceLogs, out callResult, out errMsg, false);
            //richTextBox2.AppendText("Trace接收:" + callResult + "\r\n");
            //Log.WriteLog("Trace接收:" + callResult + "\r\n");
            //JArray recvObj = JsonConvert.DeserializeObject<JArray>(callResult);
            //Trace_id = recvObj[0]["id"].ToString();
            //richTextBox2.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "Trace上传OK!" + "\r\n");
            ResultModel = pass_Trace(this.txt_SN.Text,this.txt_LeftRail.Text,this.txt_RightRail.Text,this.txt_Fixture.Text,textBox1.Text);
        }
        private ResultModel<FlHelper.Models.ParaInfo[]> ResultModel { set; get; }
        private ResultModel<FlHelper.Models.ParaInfo[]> pass_Trace(string SN, string left_rail, string right_rail, string fixture, string head_id)
        {
           
                var paraInfos = new FlHelper.Models.ParaInfo[15];
                try
                {
                //richTextBox1.Clear();
                //richTextBox2.Clear();
                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值

                TraceMesRequest_ua trace_ua = new TraceMesRequest_ua();
                //_homefrm.AppendRichText("产品码：" + SN, "Rtxt_Send_Trace");
                //_homefrm.AppendRichText("左键码：" + left_rail, "Rtxt_Send_Trace");
                //_homefrm.AppendRichText("右键码：" + right_rail, "Rtxt_Send_Trace");
                //_homefrm.AppendRichText("治具码：" + fixture, "Rtxt_Send_Trace");

                string callResult = "";
                    string errMsg = "";



               
                    //Set_Laser();//给焊接参数赋值

                    #region 数据库存储焊接参数
                    //string selectStr = $"select * from HGData where SN={SN}";

                    ////数据表的格式：自增长ID，时间，SN，location编号。location专有的参数。。。。。最后是location通用的参数
                    //DataTable d1 = SQL.ExecuteQuery(selectStr);
                    //if (d1 != null && d1.Rows.Count > 0)
                    //{
                    //    if (d1.Rows.Count < 12)
                    //    {

                    //    }

                    //    //通用参数赋值 跟location没关系的值
                    //    UA_data.SN = d1.Rows[0][2].ToString();



                    //    for (int i = 0; i < d1.Rows.Count; i++)
                    //    {
                    //        if (d1.Rows[i][3].ToString() == "location1")
                    //        {
                    //            UA_data.location1_power = d1.Rows[i][4].ToString();
                    //            UA_data.location1_frequency = d1.Rows[i][5].ToString();
                    //            UA_data.location1_waveform = d1.Rows[i][6].ToString();
                    //            UA_data.location1_laser_speed = d1.Rows[i][7].ToString();
                    //            UA_data.location1_jump_speed = d1.Rows[i][8].ToString();
                    //            UA_data.location1_jump_delay = d1.Rows[i][9].ToString();
                    //            UA_data.location1_position_delay = d1.Rows[i][10].ToString();
                    //            UA_data.location1_pulse_profile = d1.Rows[i][11].ToString();

                    //        }

                    //    }
                    //}

                    #endregion

                    


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
                    trace_ua.data.insight.results = new Result[162];
                    //trace_ua.data.insight.results = new Result[104‬]; new
                    //230401
                    //string sql = $"select * from HGData where barcode='{SN}'";
                    string sql = "SELECT TOP 15 * FROM [ZHH].[dbo].[HGData] ORDER BY DateTime DESC";
                    DataTable dt = Sqlserver.ExecuteQuery(sql);
                    if (dt.Rows.Count < 15)
                    {
                        Log.WriteLog("Trace上传失败:焊接数据不足，现有"+dt.Rows.Count+"条数据");
                       
                        return new ResultModel<FlHelper.Models.ParaInfo[]>() { Result = false, Model = paraInfos };
                    }

                    for (int i = 0; i < 150; i++)
                    {
                        trace_ua.data.insight.results[i] = new Result();

                        trace_ua.data.insight.results[i].result = "pass";

                    #region old

                    //    //i除以8的结果，相当于用来区分每个焊点
                    //    switch (i / 8)
                    //    {
                    //        //第一个焊点
                    //        case 0:
                    //            trace_ua.data.insight.results[i].test = "location1_RM2_layer1";
                    //            break;
                    //        //第二个焊点
                    //        case 1:
                    //            trace_ua.data.insight.results[i].test = "location2_LM1_layer2";
                    //            break;
                    //        case 2:
                    //            trace_ua.data.insight.results[i].test = "location3_LT_layer3";
                    //            break;
                    //        //case 3:
                    //        //    trace_ua.data.insight.results[i].test = "location3_LT-R_layer3";
                    //        //    break;
                    //        case 3:
                    //            trace_ua.data.insight.results[i].test = "location4_LM2_layer4";
                    //            break;
                    //        case 4:
                    //            trace_ua.data.insight.results[i].test = "location5_LB_layer5";
                    //            break;
                    //        //case 6:
                    //        //    trace_ua.data.insight.results[i].test = "location6_LB-R_layer6";
                    //        //    break;
                    //        case 5:
                    //            trace_ua.data.insight.results[i].test = "location6_RM1_layer6";
                    //            break;
                    //        case 6:
                    //            trace_ua.data.insight.results[i].test = "location7_RT_layer7";
                    //            break;
                    //        //case 9:
                    //        //    trace_ua.data.insight.results[i].test = "location8_RT-R_layer8";
                    //        //    break;
                    //        case 7:
                    //            trace_ua.data.insight.results[i].test = "location8_RM3_layer8";
                    //            break;
                    //        case 8:
                    //            trace_ua.data.insight.results[i].test = "location9_RB_layer9";
                    //            break;
                    //        //case 12:
                    //        //    trace_ua.data.insight.results[i].test = "location9_RB-R_layer9";
                    //            //break;
                    //        default:
                    //            break;
                    //    }
                    //    switch (i % 8)//除以8的余数，相当于用来区分每个焊接参数（不同焊点）
                    //    {
                    //        //举例：i为0 8 16 24 32.....232，共计30个，，每个焊点的第一个参数
                    //        case 0:
                    //            trace_ua.data.insight.results[i].sub_test = "power";
                    //            trace_ua.data.insight.results[i].units = "W";
                    //            trace_ua.data.insight.results[i].value = "15%";
                    //            break;
                    //        //举例：i为 1 9 17 25...233，共计30个，每个焊点的第二个参数
                    //        case 1:
                    //            trace_ua.data.insight.results[i].sub_test = "frequency";
                    //            trace_ua.data.insight.results[i].units = "KHz";
                    //            trace_ua.data.insight.results[i].value = "0";
                    //            break;
                    //        case 2:
                    //            trace_ua.data.insight.results[i].sub_test = "waveform";
                    //            trace_ua.data.insight.results[i].units = "";

                    //            if (i / 8 == 0)
                    //            {
                    //                trace_ua.data.insight.results[i].value = "13";
                    //            }
                    //            else if (i / 8 == 1)
                    //            {
                    //                trace_ua.data.insight.results[i].value = "11";
                    //            }
                    //            else if (i / 8 == 2)
                    //            {
                    //                trace_ua.data.insight.results[i].value = "12";
                    //            }
                    //            else if (i / 8 == 3 || i / 8 == 4 || i / 8 == 5 || i / 8 == 7)
                    //            {
                    //                trace_ua.data.insight.results[i].value = "14";
                    //            }
                    //            else if (i / 8 == 6)
                    //            {
                    //                trace_ua.data.insight.results[i].value = "10";
                    //            }
                    //            else
                    //            {
                    //                trace_ua.data.insight.results[i].value = "9";
                    //            }

                    //            break;
                    //        case 3:
                    //            trace_ua.data.insight.results[i].sub_test = "laser_speed";
                    //            trace_ua.data.insight.results[i].units = "mm/s";
                    //            trace_ua.data.insight.results[i].value = "40";
                    //            break;
                    //        case 4:
                    //            trace_ua.data.insight.results[i].sub_test = "jump_speed";
                    //            trace_ua.data.insight.results[i].units = "mm/s";
                    //            trace_ua.data.insight.results[i].value = "250";
                    //            break;
                    //        case 5:
                    //            trace_ua.data.insight.results[i].sub_test = "jump_delay";
                    //            trace_ua.data.insight.results[i].units = "us";
                    //            trace_ua.data.insight.results[i].value = "0";
                    //            break;
                    //        case 6:
                    //            trace_ua.data.insight.results[i].sub_test = "position_delay";
                    //            trace_ua.data.insight.results[i].units = "us";
                    //            trace_ua.data.insight.results[i].value = "0";
                    //            break;
                    //        case 7:
                    //            trace_ua.data.insight.results[i].sub_test = "pulse_profile";
                    //            trace_ua.data.insight.results[i].units = "";

                    //            if (i / 8 == 0)
                    //            {
                    //                trace_ua.data.insight.results[i].value = "0ms24%/500ms24%/50ms0%";
                    //            }
                    //            else if (i / 8 == 1)
                    //            {
                    //                trace_ua.data.insight.results[i].value = "0ms24%/435ms24%/50ms0%";
                    //            }
                    //            else if (i / 8 == 2)
                    //            {
                    //                trace_ua.data.insight.results[i].value = "0ms24%/465ms24%/50ms0%";
                    //            }
                    //            else if (i / 8 == 3 || i / 8 == 4 || i / 8 == 5 || i / 8 == 7)
                    //            {
                    //                trace_ua.data.insight.results[i].value = "0ms24%/535ms24%/50ms0%";
                    //            }
                    //            else if (i / 8 == 6)
                    //            {
                    //                trace_ua.data.insight.results[i].value = "0ms24%/305ms24%/50ms0%";
                    //            }
                    //            else
                    //            {
                    //                trace_ua.data.insight.results[i].value = "0ms24%/245ms24%/50ms0%";
                    //            }
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //}
                    #endregion

                    #region new
                    //230401
                    //i除以10的结果，相当于用来区分每个焊点
                    switch (i / 10)
                    {

                        //第一个焊点
                        case 0:
                            trace_ua.data.insight.results[i].test = "RM2";

                            break; ;
                        //第二个焊点
                        case 1:
                            trace_ua.data.insight.results[i].test = "LM1";

                            break;
                        case 2:
                            trace_ua.data.insight.results[i].test = "LT";

                            break;
                        case 3:
                            trace_ua.data.insight.results[i].test = "F-LT";

                            break;
                        case 4:
                            trace_ua.data.insight.results[i].test = "LM2";

                            break;
                        case 5:
                            trace_ua.data.insight.results[i].test = "LB";

                            break;
                        case 6:
                            trace_ua.data.insight.results[i].test = "F-LB";

                            break;
                        case 7:
                            trace_ua.data.insight.results[i].test = "RM1";

                            break;
                        case 8:
                            trace_ua.data.insight.results[i].test = "RT";

                            break;
                        case 9:
                            trace_ua.data.insight.results[i].test = "F-RT";

                            break;
                        case 10:
                            trace_ua.data.insight.results[i].test = "RM3";

                            break;
                        case 11:
                            trace_ua.data.insight.results[i].test = "RB";
                            break;
                        case 12:
                            trace_ua.data.insight.results[i].test = "F-RB";

                            break;
                        case 13:
                            trace_ua.data.insight.results[i].test = "TM";

                            break;
                        case 14:
                            trace_ua.data.insight.results[i].test = "BM";

                            break;

                        default:
                            break;
                    }
                    switch (i % 10)
                        {
                            case 0:
                                trace_ua.data.insight.results[i].sub_test = "PowerDiode";
                                trace_ua.data.insight.results[i].units = "%";
                                trace_ua.data.insight.results[i].value = dt.Rows[i / 10][3].ToString();
                                break;
                            case 1:
                                trace_ua.data.insight.results[i].sub_test = "PowerFiber";
                                trace_ua.data.insight.results[i].units = "%";
                                trace_ua.data.insight.results[i].value = dt.Rows[i / 10][4].ToString();
                                break;
                            case 2:
                                trace_ua.data.insight.results[i].sub_test = "frequency";
                                trace_ua.data.insight.results[i].units = "KHZ";
                                trace_ua.data.insight.results[i].value = dt.Rows[i / 10][5].ToString();
                                break;
                            case 3:
                                trace_ua.data.insight.results[i].sub_test = "waveform";
                                trace_ua.data.insight.results[i].units = "";
                                trace_ua.data.insight.results[i].value = dt.Rows[i / 10][6].ToString();
                                break;
                            case 4:
                                trace_ua.data.insight.results[i].sub_test = "laser_speed";
                                trace_ua.data.insight.results[i].units = "mm/s";
                                trace_ua.data.insight.results[i].value = dt.Rows[i / 10][7].ToString();
                                break;
                            case 5:
                                trace_ua.data.insight.results[i].sub_test = "jump_speed";
                                trace_ua.data.insight.results[i].units = "mm/s";
                                trace_ua.data.insight.results[i].value = dt.Rows[i / 10][8].ToString();
                                break;
                            case 6:
                                trace_ua.data.insight.results[i].sub_test = "jump_delay";
                                trace_ua.data.insight.results[i].units = "us";
                                trace_ua.data.insight.results[i].value = dt.Rows[i / 10][9].ToString();
                                break;
                            case 7:
                                trace_ua.data.insight.results[i].sub_test = "position_delay";
                                trace_ua.data.insight.results[i].units = "us";
                                trace_ua.data.insight.results[i].value = dt.Rows[i / 10][10].ToString();
                                break;
                            case 8:
                                trace_ua.data.insight.results[i].sub_test = "pulse_profile";
                                trace_ua.data.insight.results[i].units = "";
                                trace_ua.data.insight.results[i].value = dt.Rows[i / 10][11].ToString();
                                break;
                            case 9:
                                trace_ua.data.insight.results[i].sub_test = "pulse_profileFiber";
                                trace_ua.data.insight.results[i].units = "";
                                trace_ua.data.insight.results[i].value = dt.Rows[i / 10][12].ToString();
                                break;
                        }
                    }
               
                
                for (int i = 0; i < 12; i++)
                {
                    trace_ua.data.insight.results[i+150] = new Result();
                    trace_ua.data.insight.results[i + 150].result = "pass";
                    trace_ua.data.insight.results[i + 150].units = "mm";
                    switch (i)
                    {
                        case 0:
                            trace_ua.data.insight.results[i + 150].test = "LT";
                            trace_ua.data.insight.results[i + 150].sub_test = "TestHeightZ";
                            trace_ua.data.insight.results[i + 150].value = "130.71";
                            break;
                        case 1:
                            trace_ua.data.insight.results[i + 150].test = "LT";
                            trace_ua.data.insight.results[i + 150].sub_test = "Difference";
                            trace_ua.data.insight.results[i + 150].value = "0.039999999999992";
                            break;
                        case 2:
                            trace_ua.data.insight.results[i + 150].test = "LT";
                            trace_ua.data.insight.results[i + 150].sub_test = "ReferenceV";
                            trace_ua.data.insight.results[i + 150].value = "130.75";
                            break;
                           
                        case 3:
                            trace_ua.data.insight.results[i + 150].test = "LB";
                            trace_ua.data.insight.results[i + 150].sub_test = "TestHeightZ";
                            trace_ua.data.insight.results[i + 150].value = "130.71";
                            break;
                        case 4:
                            trace_ua.data.insight.results[i + 150].test = "LB";
                            trace_ua.data.insight.results[i + 150].sub_test = "Difference";
                            trace_ua.data.insight.results[i + 150].value = "-0.0100000000000193";
                            break;
                        case 5:
                            trace_ua.data.insight.results[i + 150].test = "LB";
                            trace_ua.data.insight.results[i + 150].sub_test = "ReferenceV";
                            trace_ua.data.insight.results[i + 150].value = "130.7";
                            break;
                        case 6:
                            trace_ua.data.insight.results[i + 150].test = "RB";
                            trace_ua.data.insight.results[i + 150].sub_test = "TestHeightZ";
                            trace_ua.data.insight.results[i + 150].value = "130.84";
                            break;
                        case 7:
                            trace_ua.data.insight.results[i + 150].test = "RB";
                            trace_ua.data.insight.results[i + 150].sub_test = "Difference";
                            trace_ua.data.insight.results[i + 150].value = "-0.039999999999992";
                            break;
                        case 8:
                            trace_ua.data.insight.results[i + 150].test = "RB";
                            trace_ua.data.insight.results[i + 150].sub_test = "ReferenceV";
                            trace_ua.data.insight.results[i + 150].value = "130.8";
                            break;
                        case 9:
                            trace_ua.data.insight.results[i + 150].test = "RT";
                            trace_ua.data.insight.results[i + 150].sub_test = "TestHeightZ";
                            trace_ua.data.insight.results[i + 150].value = "130.63";
                            break;
                        case 10:
                            trace_ua.data.insight.results[i + 150].test = "RT";
                            trace_ua.data.insight.results[i + 150].sub_test = "Difference";
                            trace_ua.data.insight.results[i + 150].value = "-0.00999999999999091";
                            break;
                        case 11:
                            trace_ua.data.insight.results[i + 150].test = "RT";
                            trace_ua.data.insight.results[i + 150].sub_test = "ReferenceV";
                            trace_ua.data.insight.results[i + 150].value = "130.62";
                            break;
                            
                          
                        default:
                            break;
                    }
                    
                    
                }
                    #endregion

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
                    trace_ua.data.insight.uut_attributes.hatch = "";
                    trace_ua.data.insight.uut_attributes.laser_start_time = DateTime.Now.AddSeconds(-12).ToString("yyyy-MM-dd HH:mm:ss");
                    trace_ua.data.insight.uut_attributes.laser_stop_time = DateTime.Now.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    trace_ua.data.insight.uut_attributes.lc_sn = SN;

                    trace_ua.data.insight.uut_attributes.left_rail_sn = left_rail;

                    //trace_ua.data.insight.uut_attributes.swing_amplitude = "0.4"; //1
                    //trace_ua.data.insight.uut_attributes.swing_freq = "10000";//2
                    //trace_ua.data.insight.uut_attributes.pattern_type = head_id;//3
                    //trace_ua.data.insight.uut_attributes.precitec_value = "NA";//4
                    //trace_ua.data.insight.uut_attributes.precitec_rev = "NA";//5
                    //trace_ua.data.insight.uut_attributes.precitec_grading = "NA";//6

                    //230401
                    trace_ua.data.insight.uut_attributes.swing_amplitude = dt.Rows[0][13].ToString();//1摆幅
                    trace_ua.data.insight.uut_attributes.swing_freq = dt.Rows[0][14].ToString(); //2摆动频率
                    trace_ua.data.insight.uut_attributes.pattern_type = dt.Rows[0][15].ToString();//3摆动图像类型
                    trace_ua.data.insight.uut_attributes.precitec_value = dt.Rows[0][16].ToString();//4  值 
                    trace_ua.data.insight.uut_attributes.precitec_rev = dt.Rows[0][17].ToString();//5   版本号 
                    trace_ua.data.insight.uut_attributes.precitec_grading = dt.Rows[0][18].ToString();//6 检测等级
                    //trace_ua.data.insight.uut_attributes.precitec_result = dt.Rows[0][19].ToString();//7总结果

                    trace_ua.data.insight.uut_attributes.right_rail_sn = right_rail;

                    trace_ua.data.insight.uut_attributes.spot_size = "";

                    trace_ua.data.insight.uut_attributes.station_vendor = "HG";


                    trace_ua.data.insight.uut_attributes.tossing_item = "NA";


                    for (int i = 0; i <= 0; i++)
                    {
                        (trace_ua.data.items as ICollection<KeyValuePair<string, object>>).Add(new KeyValuePair<string, object>("error_" + (i + 1), "00000000" + "_" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    }
                    string SendTraceLogs = JsonConvert.SerializeObject(trace_ua, Formatting.None, jsetting);


                for (int i = 0; i < 15; i++)
                {
                    var area = "";
                    switch (i)
                    {
                        case 0:
                            area = "RM2";
                            break;
                        case 1:
                            area = "LM1";
                            break;
                        case 2:
                            area = "LT";
                            break;
                        case 3:
                            area = "F-LT";
                            break;
                        case 4:
                            area = "LM2";
                            break;
                        case 5:
                            area = "LB";
                            break;
                        case 6:
                            area = "F-LB";
                            break;
                        case 7:
                            area = "RM1";
                            break;
                        case 8:
                            area = "RT";
                            break;
                        case 9:
                            area = "F-RT";
                            break;
                        case 10:
                            area = "RM3";
                            break;
                        case 11:
                            area = "RB";
                            break;
                        case 12:
                            area = "F-RB";
                            break;
                        case 13:
                            area = "TM";
                            break;
                        case 14:
                            area = "BM";
                            break;
                        default:
                            break;
                    }
                    //pulse_profile 0ms15%/285ms15%/50ms0% 0ms15%/285ms15%/50ms0%
                    string[] str1 = dt.Rows[i][11].ToString().Split(new string[3] { "ms", "%", "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] str2 = dt.Rows[i][12].ToString().Split(new string[3] { "ms", "%", "/" }, StringSplitOptions.RemoveEmptyEntries);
                    var paraInfo = new FlHelper.Models.ParaInfo()
                    {
                        /// <summary>
                        /// 焊接區域
                        /// </summary>

                        area = area,
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

                        powerUsl = dt.Rows[i][3].ToString(),
                        /// <summary>
                        /// 功率設置下限
                        /// </summary>

                        powerLsl = dt.Rows[i][4].ToString(),
                        /// <summary>
                        /// 脉衝波形
                        /// </summary>

                        pusleWaveform = dt.Rows[i][6].ToString(),
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

                        jumpingDelay = dt.Rows[i][9].ToString(),
                        /// <summary>
                        /// 攝像頭焊接間隙
                        /// </summary>

                        trimWasherGap = "",
                        /// <summary>
                        /// 掃描延時
                        /// </summary>

                        scanningDelay = dt.Rows[i][10].ToString(),
                        /// <summary>
                        /// Scan掃描層波形
                        /// </summary>

                        scanParametersWaveform = dt.Rows[i][6].ToString(),
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

                        parametersWaveform = dt.Rows[i][6].ToString(),
                        /// <summary>
                        /// Laser焊接層焊點尺寸
                        /// </summary>

                        spotSize = trace_ua.data.insight.uut_attributes.spot_size,

                        /// <summary>
                        ///Laser焊接層功率 GAI2
                        /// </summary>

                        power = dt.Rows[i][4].ToString(),
                        /// <summary>
                        /// Laser焊接層脈衝能量
                        /// </summary>

                        pulseEnergy = dt.Rows[i][3].ToString(),
                        /// <summary>
                        /// Laser焊接層頻率
                        /// </summary>

                        frequency = "",
                        /// <summary>
                        /// Laser焊接層線性速度 
                        /// </summary>

                        linearSpeed = dt.Rows[i][7].ToString(),
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

                        SettingOuterLASEREnergyPercentage = dt.Rows[i][3].ToString(),
                        a1stPulseWidth = "",
                        a2ndPulseWidth = "",
                        a3rdPulseWidth = "",
                        a4thPulseWidth = "",
                        a5thPulseWidth = "",
                        a6thPulseWidth = "",

                        /// <summary>
                        /// 標準速度(mm/s)
                        /// </summary>

                        Speed = dt.Rows[i][7].ToString(),
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

                        pulseProfile = dt.Rows[i][11].ToString(),

                    };

                    paraInfos[i] = paraInfo;
                }


                Log.WriteLog("Trace上传:" + SendTraceLogs);
                //richTextBox1.AppendText(SendTraceLogs);

                    RequestAPI3.CallBobcat3(Global.TraceURL, SendTraceLogs, out callResult, out errMsg);
                    Log.WriteLog("Trace接收：" + callResult);
                //richTextBox2.AppendText(callResult);
                //richTextBox2.AppendText("Trace接收：" + errMsg);
                JArray recvObj = JsonConvert.DeserializeObject<JArray>(callResult);
                //JArray recvObj = JArray.Parse(callResult);
                Log.WriteLog("Trace接收:" + callResult + "\r\n");
             
                Trace_id = recvObj[0]["id"].ToString();
                //richTextBox2.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "Trace上传OK!" + "\r\n");
                return new ResultModel<FlHelper.Models.ParaInfo[]>() { Result = true, Model = paraInfos };
            }
            catch (Exception ex)
                {
                    Log.WriteLog(head_id + "Trace上传异常" + ex.ToString());
               

                return new ResultModel<FlHelper.Models.ParaInfo[]>() { Result = false, Model = paraInfos };
                }
            

        }


        private void btn_GetSN_Click(object sender, EventArgs e)
        {
            string callResult = "";
            string errMsg = "";

            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值

            string SFC_url = "http://10.176.152.159:8081/NewSFCV2-center/NewSFCV2/getcodeinfo";

            SFCData sfc_data = new SFCData();
            sfc_data.code_no = this.txt_SN.Text; //"HWWGPL10EQD000029L+QS101BLK2G3V20471QU";
            sfc_data.equipment_ip = Global.Trace_ip;//"10.175.58.70";
            sfc_data.resv1 = "";
            sfc_data.resv2 = "";

            string send_SFC = JsonConvert.SerializeObject(sfc_data, Formatting.None, jsetting);
            Log.WriteLog("上传SFC系统：" + send_SFC);
            richTextBox3.AppendText("上传SFC系统：" + send_SFC);

            RequestAPI2.CallBobcat3(SFC_url, send_SFC, out callResult, out errMsg, false);
            Log.WriteLog("获取码信息接收:" + callResult);
            richTextBox4.AppendText("获取码信息接收:" + callResult);

            JObject recvObj = JsonConvert.DeserializeObject<JObject>(callResult);

            //config验证，目前待测。（进行判断）
            //recvObj["config"].ToString();




        }

        private void btn_newsStation_Click(object sender, EventArgs e)
        {
            string callResult = "";
            string errMsg = "";

            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值
        //http://10.197.246.63:8080/NewSFCV2/v2/enterequ
            //string newStn_url = "http://10.176.152.159:28080/NewSFCV2-enterequ-bcs01/NewSFCV2/bcs-enterequ";
            string newStn_url = "http://10.197.246.63:8080/NewSFCV2/v2/enterequ";

            NewStation new_Stn = new NewStation();
            new_Stn.equipmentNo = Global.equipmentNo;
            new_Stn.station = Global.station;
            new_Stn.billNo = "";
            new_Stn.barCode = this.txt_SN.Text;
            new_Stn.barCodeType = Global.barCodeType;
            new_Stn.resv1 = "";
            new_Stn.resv2 = "";

            string send_newStn = JsonConvert.SerializeObject(new_Stn, Formatting.None, jsetting);
            Log.WriteLog("A工位 Equipment to API 設備報站:" + send_newStn);
            //richTextBox6.AppendText("A工位 Equipment to API 設備報站:" + send_newStn + "\r\n");

            RequestAPI2.CallBobcat3(newStn_url, send_newStn, out callResult, out errMsg, false);
            Log.WriteLog("A工位 API to Equipment 設備報站:" + callResult);
            //richTextBox5.AppendText("A工位 API to Equipment 設備報站:" + callResult + "\r\n");

            JObject recvObj = JsonConvert.DeserializeObject<JObject>(callResult);

            //采集数据供上料检验，过站使用
            hashCode_newstn = recvObj["hashCode"].ToString();
            billNo_newstn = recvObj["billNo"].ToString();
            checkURL = recvObj["checkURL"].ToString();
            passURL = recvObj["passURL"].ToString();

            if (recvObj["rc"].ToString() == "000")
            {
                //richTextBox5.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "报站OK!" + "\r\n");
            }

            //20181030153811
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
                //richTextBox5.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "与系统时间同步成功" + "\r\n");
            }

        }

        private void btn_Loading_Click(object sender, EventArgs e)
        {
            string callResult = "";
            string errMsg = "";

            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值



            #region 富士康V1.0
            LoadingCheck check_load = new LoadingCheck();

            check_load.hashCode = hashCode_newstn;
            check_load.billNo = billNo_newstn;
            check_load.barCodeType = Global.barCodeType;
            check_load.barCode = this.txt_SN.Text;
            check_load.equipmentNo = Global.equipmentNo;
            check_load.station = Global.station;
            check_load.startTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            check_load.bindCode = new BindCode[4];
            check_load.bindCode[0] = new BindCode();
            check_load.bindCode[1] = new BindCode();
            check_load.bindCode[2] = new BindCode();
            check_load.bindCode[3] = new BindCode();

            check_load.bindCode[0].codeSn = this.txt_LeftRail.Text;
            check_load.bindCode[0].codeSnType = "IQC_LEFT_RAIL";
            check_load.bindCode[0].replace = "1";
            check_load.bindCode[1].codeSn = this.txt_RightRail.Text;
            check_load.bindCode[1].codeSnType = "IQC_RIGHT_RAIL";
            check_load.bindCode[1].replace = "1";
            check_load.bindCode[2].codeSn = this.txt_Fixture.Text;
            check_load.bindCode[2].codeSnType = "FIXTURE";
            check_load.bindCode[2].replace = "1";
            check_load.bindCode[3].codeSn = Global.equipmentNo;
            check_load.bindCode[3].codeSnType = "EQUIPMENT";
            check_load.bindCode[3].replace = "1";

            check_load.resv1 = "";
            check_load.resv2 = "";
            #endregion


            #region 富士康V1.1--D83EVT20220215暂时不用
            //Loading_Affirm check_load = new Loading_Affirm();

            //check_load.hashcode = hashCode_newstn;
            //check_load.request_id = DateTime.Now.ToString("yyyyMMdd-HHmmss.fff") + "-000000000000000" + Global.equipmentNo;
            //check_load.code_no = SN;
            //check_load.code_type = Global.barCodeType;
            //check_load.bill_no = billNo_newstn;
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
            Log.WriteLog("A工位 Equipment to API 上料確認:" + send_check);
            //richTextBox8.AppendText("A工位 Equipment to API 上料確認:" + send_check);

            RequestAPI2.CallBobcat4(checkURL, send_check, hashCode_newstn, out callResult, out errMsg);
            Log.WriteLog("A工位 API to Equipment 上料確認:" + callResult);
            //richTextBox7.AppendText("A工位 API to Equipment 上料確認:" + callResult + "\r\n");
            //richTextBox7.AppendText("A工位 API to Equipment 上料確認:" + errMsg + "\r\n");
            //JObject recvObj = JsonConvert.DeserializeObject<JObject>(callResult);
            try
            {
                JObject recvObj = JObject.Parse(callResult);

                requestId_check = recvObj["requestId"].ToString();
                TraceURL = recvObj["url"].ToString();


                if (recvObj["rc"].ToString() == "000")
                {
                    //richTextBox7.AppendText("上料确认OK!");
                }
            }
            catch (Exception ex)
            {

                Log.WriteLog("A工位 API to Equipment 上料確認异常:" + ex.Message);
            }
           



        }

        public void AppendRichText(string msg, string Name) { }
        Dictionary<string, Dictionary<string, heightModel>> heights = new Dictionary<string, Dictionary<string, heightModel>>();///存储测高数据
        private void btn_pass_Click(object sender, EventArgs e)
        {
            
           
            var modellist=new Dictionary<string,heightModel>();
            modellist.Add("LT", new heightModel {TestHeightZ= "130.71", Difference= "0.039999999999992", ReferenceV= "130.75" });
            modellist.Add("LB", new heightModel { TestHeightZ = "130.71", Difference = "-0.0100000000000193", ReferenceV = "130.7" });
            modellist.Add("RB", new heightModel { TestHeightZ = "130.84", Difference = "-0.039999999999992", ReferenceV = "130.8" });
            modellist.Add("RT", new heightModel { TestHeightZ = "130.63", Difference = "-0.00999999999999091", ReferenceV = "130.62" });
            heights.Add(this.txt_SN.Text,modellist);

            string callResult = "";
            string errMsg = "";
            string cavity= "0" + (new Random().Next(10) + 1);
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值

            PassStn pass_Stn = new PassStn();

            pass_Stn.hashCode = hashCode_newstn;
            pass_Stn.requestId = requestId_check;
            pass_Stn.resv1 = "";
            pass_Stn.productInfo = new ProductInfo();
            pass_Stn.productInfo.barCode = this.txt_SN.Text;
            pass_Stn.productInfo.barCodeType = Global.barCodeType;
            pass_Stn.productInfo.station = Global.station;
            pass_Stn.productInfo.billNo = billNo_newstn;


            pass_Stn.productInfo.bindCode = new BindCode1[4];
            pass_Stn.productInfo.bindCode[0] = new BindCode1();
            pass_Stn.productInfo.bindCode[1] = new BindCode1();
            pass_Stn.productInfo.bindCode[2] = new BindCode1();
            pass_Stn.productInfo.bindCode[3] = new BindCode1();

            pass_Stn.productInfo.bindCode[0].codeSn = this.txt_LeftRail.Text;
            pass_Stn.productInfo.bindCode[0].codeSnType = "IQC_LEFT_RAIL";
            pass_Stn.productInfo.bindCode[0].replace = "1";
            pass_Stn.productInfo.bindCode[1].codeSn = this.txt_RightRail.Text;
            pass_Stn.productInfo.bindCode[1].codeSnType = "IQC_RIGHT_RAIL";
            pass_Stn.productInfo.bindCode[1].replace = "1";
            pass_Stn.productInfo.bindCode[2].codeSn = this.txt_Fixture.Text;
            pass_Stn.productInfo.bindCode[2].codeSnType = "FIXTURE";
            pass_Stn.productInfo.bindCode[2].replace = "1";
            pass_Stn.productInfo.bindCode[3].codeSn = Global.equipmentNo;
            pass_Stn.productInfo.bindCode[3].codeSnType = "EQUIPMENT";
            pass_Stn.productInfo.bindCode[3].replace = "1";

            pass_Stn.equipmentInfo = new EquipmentInfo();
            pass_Stn.equipmentInfo.equipmentIp = Global.Trace_ip;
            pass_Stn.equipmentInfo.equipmentType = "L";
            pass_Stn.equipmentInfo.equipmentNo = Global.equipmentNo;
            pass_Stn.equipmentInfo.vendorId = "HG";
            pass_Stn.equipmentInfo.processRevs = Global.version;

            pass_Stn.recipeInfo = new RecipeInfo();
            pass_Stn.recipeInfo.startTime = DateTime.Now.AddSeconds(-1).ToString("yyyyMMddHHmmss");
            pass_Stn.recipeInfo.endTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            pass_Stn.recipeInfo.cavity = cavity;
            pass_Stn.recipeInfo.judgement = "PASS";
            pass_Stn.recipeInfo.humidity = "63";
            pass_Stn.recipeInfo.temperature = "35";
            pass_Stn.recipeInfo.paraInfo = new ParaInfo[15];

            var heightsValue = new Dictionary<string, heightModel>();
            if (heights.ContainsKey(this.txt_SN.Text))
            {
                heights.TryGetValue(this.txt_SN.Text, out heightsValue);
            }
            for (int i = 0; i <ResultModel.Model.Length; i++)
            {
                pass_Stn.recipeInfo.paraInfo[i] = new ParaInfo(ResultModel.Model[i], heightsValue);

            }
            heights.Remove(this.txt_SN.Text);
            //pass_Stn.recipeInfo.paraInfo = new ParaInfo[13];
            //for (int i = 0; i < 13; i++)
            //{

            //    pass_Stn.recipeInfo.paraInfo[i] = new ParaInfo();
            //    pass_Stn.recipeInfo.paraInfo[i].sTime = DateTime.Now.AddSeconds(-12).ToString("yyyyMMddHHmmss");
            //    pass_Stn.recipeInfo.paraInfo[i].eTime = DateTime.Now.AddSeconds(-1).ToString("yyyyMMddHHmmss");
            //    switch (i)
            //    {
            //        case 0:
            //            pass_Stn.recipeInfo.paraInfo[i].area = "RM2";
            //            break;
            //        case 1:
            //            pass_Stn.recipeInfo.paraInfo[i].area = "LM1";
            //            break;
            //        case 2:
            //            pass_Stn.recipeInfo.paraInfo[i].area = "LT";
            //            break;
            //        case 3:
            //            pass_Stn.recipeInfo.paraInfo[i].area = "AB1";
            //            break;
            //        case 4:
            //            pass_Stn.recipeInfo.paraInfo[i].area = "LM2";
            //            break;
            //        case 5:
            //            pass_Stn.recipeInfo.paraInfo[i].area = "LB";
            //            break;
            //        case 6:
            //            pass_Stn.recipeInfo.paraInfo[i].area = "AB2";
            //            break;
            //        case 7:
            //            pass_Stn.recipeInfo.paraInfo[i].area = "RM1";
            //            break;
            //        case 8:
            //            pass_Stn.recipeInfo.paraInfo[i].area = "RT";
            //            break;
            //        case 9:
            //            pass_Stn.recipeInfo.paraInfo[i].area = "AB3";
            //            break;
            //        case 10:
            //            pass_Stn.recipeInfo.paraInfo[i].area = "RM3";
            //            break;
            //        case 11:
            //            pass_Stn.recipeInfo.paraInfo[i].area = "RB";
            //            break;
            //        case 12:
            //            pass_Stn.recipeInfo.paraInfo[i].area = "AB4";
            //            break;
            //        default:
            //            break;
            //    }

            //}
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
            pass_Stn.appleReturnInfo.id = Trace_id;
            pass_Stn.appleReturnInfo.contact = "Vendor";
            pass_Stn.appleReturnInfo.error = "";

            string send_Pass = JsonConvert.SerializeObject(pass_Stn, Formatting.None, jsetting).Replace("a1st", "1st").Replace("a2nd", "2nd").Replace("a3rd", "3rd").Replace("a4th", "4th").Replace("a5th", "5th").Replace("a6th", "6th"); ;
            Log.WriteLog("A工位 Equipment to API 過站報告:" + send_Pass);
            //richTextBox10.AppendText("A工位 Equipment to API 過站報告:" + send_Pass);

            RequestAPI2.CallBobcat4(passURL, send_Pass, hashCode_newstn, out callResult, out errMsg);
            Log.WriteLog("A工位 API to Equipment 過站報告:" + callResult);
            //richTextBox9.AppendText("A工位 API to Equipment 過站報告:" + callResult + "\r\n");

            JObject recvObj = JsonConvert.DeserializeObject<JObject>(callResult);




            try
            {
                if (recvObj["rc"].ToString() == "000")
                {
                    //richTextBox9.AppendText("过站OK！");
                    this.lbl_result.Text = "Pass";
                    this.lbl_result.ForeColor = Color.Green;
                    this.txt_SN.Text = "";
                    this.txt_LeftRail.Text = "";
                    this.txt_RightRail.Text = "";
                }
                else
                {
                    this.lbl_result.Text = "Fail";
                    this.lbl_result.ForeColor = Color.Red;
                }
            }
            catch (Exception)
            {
                this.lbl_result.Text = "Fail";
                this.lbl_result.ForeColor = Color.Red;
            }


            #region SFC
            string left_rail = this.txt_LeftRail.Text;
            string right_rail = this.txt_RightRail.Text;
            string fixture = this.txt_Fixture.Text;
           
            
            //Sc.UpdateUI("过站产品码：" + barcode, "Rtxt_Send_Pass");
            //Sc.UpdateUI("过站左键码：" + left_rail, "Rtxt_Send_Pass");
            //Sc.UpdateUI("过站右键码：" + right_rail, "Rtxt_Send_Pass");
            //Sc.UpdateUI("过站治具码：" + fixture, "Rtxt_Send_Pass");
            var SfcStn = new SfcStn()
            {
                hashCode = hashCode_newstn,
                requestId = requestId_check,
                resv1 = "",

                productInfo = new FlHelper.Models.ProductInfo()
                {
                    barCode = this.txt_SN.Text,
                    barCodeType = Global.barCodeType,
                    clientCode = "",
                    station = Global.station,
                    billNo = "GL-234M-LC-EVT-X-NSF-01",
                    product = "234M",
                    phase = "EVT",
                    config = "Config999",
                    lineName = Global.Trace_line_id,
                    stationId = Global.Trace_station_id,
                    stationString = "",
                    bindCode = new FlHelper.Models.BindCode[] {
                                        new FlHelper.Models.BindCode() {codeSn=left_rail,codeSnType="IQC_LEFT_RAIL",replace="1" },
                                         new FlHelper.Models.BindCode() {codeSn=right_rail,codeSnType="IQC_RIGHT_RAIL",replace="1" },
                                          new FlHelper.Models.BindCode() {codeSn=fixture,codeSnType="FIXTURE",replace="1" },
                                           new FlHelper.Models.BindCode() {codeSn=Global.equipmentNo,codeSnType="EQUIPMENT",replace="1" },

                        },
                },
                equipmentInfo = new FlHelper.Models.EquipmentInfo()
                {
                    equipmentIp = Global.Trace_ip,
                    equipmentType = "L",
                    equipmentNo = Global.equipmentNo,
                    vendorId = "HG",
                    processRevs = Global.version,
                    softwareName = "HG数据追溯系统",
                    softwareVersion = Global.version,
                    patternName = "HG准直激光控制系统",
                    patternVersion = Global.version,
                    ccdVersion = Global.version,
                    ccdGuideVersion = Global.version,
                    ccdInspectionName = "CCD_Moniton",
                    ccdInspectionVersion = Global.version,
                },
                recipeInfo = new FlHelper.Models.RecipeInfo()
                {
                    startTime = DateTime.Now.AddSeconds(-25).ToString("yyyyMMddHHmmss"),
                    endTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    cavity = cavity,
                    judgement = "PASS",
                    humidity = "63",
                    temperature = "35",
                    processStartTime = DateTime.Now.AddSeconds(-25).ToString("yyyyMMddHHmmss"),
                    processEndTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    waitCt = "",
                    ct = "",
                    tossingItem = "",
                    errorInfo = new ErrorInfo[] { },
                    paraInfo = ResultModel.Model,
                },

            };

            send_Pass = JsonConvert.SerializeObject(SfcStn, Formatting.None, jsetting).Replace("a1st", "1st").Replace("a2nd", "2nd").Replace("a3rd", "3rd").Replace("a4th", "4th").Replace("a5th", "5th").Replace("a6th", "6th");
            //richTextBox10.AppendText("A工位 Equipment to API 参数上抛報告:" + send_Pass);
            if (Sc.PassStation("http://10.176.46.59:8081/LCH/proto", SfcStn, Global.hashCode))
            {
              //richTextBox9.AppendText("A工位 API to Equipment 参数上抛報告:" + "参数上抛OK!" + "\r\n");
            }
            else
            {
              //richTextBox9.AppendText("A工位 API to Equipment 参数上抛報告:" + "参数上抛失败!" + "\r\n");
            };
            PutTupian(Convert.ToInt32(textBox1.Text), this.txt_SN.Text, txt_Fixture.Text);
            #endregion
        }
        SendSfc Sc { set; get; }
        private void PutTupian(int HansId, string sn, string fixture)
        {
            try
            {
                string path = "E:\\public\\HG"+HansId+"#";
                if (Directory.Exists(path))
                {
                    string a = path+"\\"+DateTime.Now.ToString("yyyyMMdd")+sn;
                    string b = a+".zip";
                    //string c= $"{path}\\{"20230411"}{"ASDSAD"}";
                    //string d = $"{c}.zip";
                    if (Directory.Exists(a))
                    {
                        //ZipHelper.CreateZip(c, d);
                        ZipHelper.ZipDirectory(a,b);
                      string str=Sc.PostFileMessage("http://10.176.46.59:8081/PicUpload/B053F/234M", new List<PostDateClass>() {
                          //requestId_check
                          //20230619-164702.535-000000000000000000M00093730002
                        new PostDateClass() {Prop="requestId",Type=0,Value=requestId_check, },
                        new PostDateClass(){Prop="productName",Type=0,Value="234M", },
                          new PostDateClass() {Prop="barCode",Type=0,Value=sn, },
                        new PostDateClass(){Prop="station",Type=0,Value=Global.station, },
                          new PostDateClass() {Prop="fixture",Type=0,Value=fixture, },
                        new PostDateClass(){Prop="equipmentNo",Type=0,Value=Global.equipmentNo, },
                          new PostDateClass() {Prop="equipmentIp",Type=0,Value=Global.Trace_ip, },
                        new PostDateClass(){Prop="judgement",Type=0,Value="PASS", },
                         new PostDateClass() {Prop="createTime",Type=0,Value=DateTime.Now.ToString("yyyyMMddHHmmss"), },
                        new PostDateClass(){Prop="file",Type=1,Value=b, },
                    });
                      if (str.Contains("OK"))
                      {
                          Log.WriteLog("图片上抛成功!");
                      }
                      else
                      {
                          Log.WriteLog(str);
                      }
                    }
                    else
                    {

                    }

                }
            }
            catch (Exception ex)
            {

               
            }


        }
        SerialPort comm = new SerialPort();
        private byte[] buffer = new byte[1024*1024];
        //private void btn_Connect_Click(object sender, EventArgs e)
        //{
        //    if (!comm.IsOpen)
        //    {
        //        comm = new SerialPort(this.cmbPort.Text,115200);
        //        comm.Open();
        //        MessageBox.Show("串口已连接！");
        //        comm.DataReceived += Comm_DataReceived;
        //    }
        //    else
        //    {
        //        MessageBox.Show("串口已经打开！");
        //    }
        //}

        private void Comm_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //Thread.Sleep(300);
            // int i = comm.Read(buffer, 0, comm.ReadBufferSize);
             
            //string result  = BitConverter.ToString(buffer,i);
            //string result3 = comm.ReadExisting();
            //comm.Read(buffer, 0, comm.ReadBufferSize);
            ////string res = BitConverter.ToString(buffer);           
            //string res = BitConverter.ToString(Encoding.Convert(Encoding.UTF8, Encoding.ASCII, buffer));
            ////string result2 = BitConverter.ToString(buffer, i);
            //this.Invoke(new Action(() => this.txt_SN.Text = result));

            try
            {
                SerialPort sp = (SerialPort)sender;
                byte[] rec = new byte[sp.BytesToRead];
                sp.Read(rec,0,rec.Length);
                Thread.Sleep(150);
                string str = Encoding.UTF8.GetString(rec,0,rec.Length);
                if (string.IsNullOrEmpty(str))
                {
                    return;
                }
                else
                {
                   
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btn_DisConnect_Click(object sender, EventArgs e)
        {
            if (comm.IsOpen)
            {
                comm.Close();
                MessageBox.Show("串口已断开！");
            }
        }

        private void btn_pass2_Click(object sender, EventArgs e)
        {
            try
            {
                btn_newsStation_Click(null, null);
                btn_Loading_Click(null, null);
                btn_TraceSend_Click(null, null);
                btn_pass_Click(null, null);
            }
            catch (Exception)
            {
                this.lbl_result.Text = "Fail";
                this.lbl_result.ForeColor = Color.Red;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           //Global.plc2.Write("MW65101", Convert.ToInt16(2));
        }


        private void button2_Click(object sender, EventArgs e)
        {
            //Global.plc2.Write("MW65101", Convert.ToInt16(1));
        }

        private void btn_Test_Click(object sender, EventArgs e)
        {
            //string SendTraceLogs = this.txt_Test.Text;
            //string callResult = "";
            //string errMsg = "";
            //try
            //{
            //    richTextBox1.AppendText("Trace上传:" + SendTraceLogs + "\r\n");
            //    Log.WriteLog("Trace上传:" + SendTraceLogs);
            //    string Trcae_logs_str = string.Empty;
            //    RequestAPI2.CallBobcat3(TraceURL, SendTraceLogs, out callResult, out errMsg, false);
            //    //richTextBox2.AppendText("Trace接收:" + callResult + "\r\n");
            //    Log.WriteLog("Trace接收:" + callResult + "\r\n");
            //    JArray recvObj = JsonConvert.DeserializeObject<JArray>(callResult);
            //    Trace_id = recvObj[0]["id"].ToString();
            //    //richTextBox2.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "Trace上传OK!" + "\r\n");
            //}
            //catch (Exception)
            //{
            //    //richTextBox2.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "Trace上传出错：" + errMsg + "\r\n");
            //}
           
        }

        private void btn_ReadPLC_Click(object sender, EventArgs e)
        {
            //OperateResult<short[]> config_sn =Global.plc1.ReadInt16("MW45240", 20);
            //string barcode = ToASCII(config_sn.Content).Trim();
            //OperateResult<short[]> left_sn = Global.plc1.ReadInt16("MW45200", 20);
            //string left_rail = ToASCII(left_sn.Content).Trim();
            //OperateResult<short[]> right_sn = Global.plc1.ReadInt16("MW45220", 20);
            //string right_rail = ToASCII(right_sn.Content).Trim();
            //this.txt_SN.Text = barcode;
            //this.txt_LeftRail.Text = left_rail;
            //this.txt_RightRail.Text = right_rail;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.txt_SN.Text = "";
            this.txt_LeftRail.Text = "";
            this.txt_RightRail.Text = "";
            this.label9.Text = "";
            lbl_result.Text = "Waiting...";
            lbl_result.ForeColor = Color.Black;
        }


        /// <summary>
        /// 开始上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
            string SN = this.txt_SN.Text;
            string leftrail = this.txt_LeftRail.Text;
            string rightrail = this.txt_RightRail.Text;
            string fixture = this.txt_Fixture.Text;
            updatelabeltext(label9, SN);
            lbl_result.Text = "Waiting...";
            lbl_result.ForeColor = Color.Black;
            richTextBox1.Clear();
            AddRichText(richTextBox1, "LC码:" + SN);
            AddRichText(richTextBox1, "左键码:" + leftrail);
            AddRichText(richTextBox1, "右键码:" + rightrail);
            AddRichText(richTextBox1, "治具码:" + fixture);
            AddRichText(richTextBox1, "开始上传...");
            ResultModel bz = Baozhan(SN);
            AddRichText(richTextBox1, bz.Message);
            if (bz.Result == true)
            {

                ResultModel slqr = Shangliaojiaoyan(SN, leftrail, rightrail, fixture);
                AddRichText(richTextBox1, slqr.Message);
                if (slqr.Result == true)
                {

                    ResultModel = pass_Trace(SN, leftrail, rightrail, fixture, textBox1.Text);

                    if (ResultModel.Result == true)
                    {

                        bz.Message = "Trace上传OK!";
                        AddRichText(richTextBox1, bz.Message);
                        ResultModel gz = Guozhan(SN, leftrail, rightrail, fixture);
                        AddRichText(richTextBox1, gz.Message);
                        Sfcupload(SN, leftrail, rightrail, fixture);
                        if (gz.Result == true)
                        {
                            ///总结果OK.
                            updatelabeltext(lbl_result, "Pass", true, true);
                        }
                        else
                        {
                            //过站上传失败
                            updatelabeltext(lbl_result, "Fail", false, true);
                        }

                    }
                    else
                    {

                        bz.Message = "Trace上传失败!" + ResultModel.Message;
                        AddRichText(richTextBox1, bz.Message);
                        //Trace上传失败
                        updatelabeltext(lbl_result, "Fail", false, true);
                    }
                }
                else
                {
                    //上料确认失败
                    updatelabeltext(lbl_result, "Fail", false, true);
                }
            }
            else
            {
                //报站失败
                updatelabeltext(lbl_result, "Fail", false, true);
            }
            button4.Enabled = true;
            AddRichText(richTextBox1, "上传结束。");
        }

        private ResultModel Baozhan(string SN)
        {
            string callResult = "";
            string errMsg = "";
            try
            {
               
                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值
                string newStn_url = "http://10.197.246.63:8080/NewSFCV2/v2/enterequ";
                NewStation new_Stn = new NewStation();
                new_Stn.equipmentNo = Global.equipmentNo;
                new_Stn.station = Global.station;
                new_Stn.billNo = "";
                new_Stn.barCode = SN;
                new_Stn.barCodeType = Global.barCodeType;
                new_Stn.resv1 = "";
                new_Stn.resv2 = "";
                string send_newStn = JsonConvert.SerializeObject(new_Stn, Formatting.None, jsetting);
                RequestAPI2.CallBobcat3(newStn_url, send_newStn, out callResult, out errMsg, false);
                Log.WriteLog("A工位 API to Equipment 設備報站:" + callResult+","+errMsg);
                JObject recvObj = JsonConvert.DeserializeObject<JObject>(callResult);
                //采集数据供上料检验，过站使用
                hashCode_newstn = recvObj["hashCode"].ToString();
                billNo_newstn = recvObj["billNo"].ToString();
                checkURL = recvObj["checkURL"].ToString();
                passURL = recvObj["passURL"].ToString();

                if (recvObj["rc"].ToString() == "000")
                {
                    return new ResultModel() { Result = true, Message = SN + "报站OK!" };
                }
                else
                {
                    return new ResultModel() { Result = false, Message = SN + "报站失败:" + errMsg };
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("A工位 API to Equipment 設備報站异常:" + ex.Message);
                return new ResultModel() { Result = false, Message = SN + "报站失败:" + errMsg };
            }
        }

        private ResultModel Shangliaojiaoyan(string SN, string leftrail, string rightrail, string fixture)
        {
            string callResult = "";
            string errMsg = "";
            try
            {
                
                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值
                #region 富士康V1.0
                LoadingCheck check_load = new LoadingCheck();

                check_load.hashCode = hashCode_newstn;
                check_load.billNo = billNo_newstn;
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

                check_load.bindCode[0].codeSn = leftrail;
                check_load.bindCode[0].codeSnType = "IQC_LEFT_RAIL";
                check_load.bindCode[0].replace = "1";
                check_load.bindCode[1].codeSn = rightrail;
                check_load.bindCode[1].codeSnType = "IQC_RIGHT_RAIL";
                check_load.bindCode[1].replace = "1";
                check_load.bindCode[2].codeSn = fixture;
                check_load.bindCode[2].codeSnType = "FIXTURE";
                check_load.bindCode[2].replace = "1";
                check_load.bindCode[3].codeSn = Global.equipmentNo;
                check_load.bindCode[3].codeSnType = "EQUIPMENT";
                check_load.bindCode[3].replace = "1";

                check_load.resv1 = "";
                check_load.resv2 = "";
                #endregion
                string send_check = JsonConvert.SerializeObject(check_load, Formatting.None, jsetting);
                Log.WriteLog("A工位Equipment to API 上料確認:" + send_check);
                RequestAPI2.CallBobcat4(checkURL, send_check, hashCode_newstn, out callResult, out errMsg);
                Log.WriteLog("A工位 API to Equipment 上料確認:" + callResult + "," + errMsg);

                JObject recvObj = JObject.Parse(callResult);

                requestId_check = recvObj["requestId"].ToString();
                TraceURL = recvObj["url"].ToString();
                if (recvObj["rc"].ToString() == "000")
                {
                    return new ResultModel() { Result = true, Message = "上料确认OK!" };
                }
                else
                {
                    return new ResultModel() { Result = false, Message = "上料确认失败:" + callResult + "," + errMsg };
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("A工位 API to Equipment 上料確認异常:" + ex.Message);
                return new ResultModel() { Result = false, Message = "上料确认失败:" + errMsg };
            }
        }

        private ResultModel Guozhan(string SN, string leftrail, string rightrail, string fixture)
        {
            string callResult = "";
            string errMsg = "";
            try
            {

                var modellist = new Dictionary<string, heightModel>();
                modellist.Add("LT", new heightModel { TestHeightZ = "130.71", Difference = "0.039999999999992", ReferenceV = "130.75" });
                modellist.Add("LB", new heightModel { TestHeightZ = "130.71", Difference = "-0.0100000000000193", ReferenceV = "130.7" });
                modellist.Add("RB", new heightModel { TestHeightZ = "130.84", Difference = "-0.039999999999992", ReferenceV = "130.8" });
                modellist.Add("RT", new heightModel { TestHeightZ = "130.63", Difference = "-0.00999999999999091", ReferenceV = "130.62" });
                heights.Add(this.txt_SN.Text, modellist);



                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值

                PassStn pass_Stn = new PassStn();

                pass_Stn.hashCode = hashCode_newstn;
                pass_Stn.requestId = requestId_check;
                pass_Stn.resv1 = "";
                pass_Stn.productInfo = new ProductInfo();
                pass_Stn.productInfo.barCode = SN;
                pass_Stn.productInfo.barCodeType = Global.barCodeType;
                pass_Stn.productInfo.station = Global.station;
                pass_Stn.productInfo.billNo = billNo_newstn;


                pass_Stn.productInfo.bindCode = new BindCode1[4];
                pass_Stn.productInfo.bindCode[0] = new BindCode1();
                pass_Stn.productInfo.bindCode[1] = new BindCode1();
                pass_Stn.productInfo.bindCode[2] = new BindCode1();
                pass_Stn.productInfo.bindCode[3] = new BindCode1();

                pass_Stn.productInfo.bindCode[0].codeSn = leftrail;
                pass_Stn.productInfo.bindCode[0].codeSnType = "IQC_LEFT_RAIL";
                pass_Stn.productInfo.bindCode[0].replace = "1";
                pass_Stn.productInfo.bindCode[1].codeSn = rightrail;
                pass_Stn.productInfo.bindCode[1].codeSnType = "IQC_RIGHT_RAIL";
                pass_Stn.productInfo.bindCode[1].replace = "1";
                pass_Stn.productInfo.bindCode[2].codeSn = fixture;
                pass_Stn.productInfo.bindCode[2].codeSnType = "FIXTURE";
                pass_Stn.productInfo.bindCode[2].replace = "1";
                pass_Stn.productInfo.bindCode[3].codeSn = Global.equipmentNo;
                pass_Stn.productInfo.bindCode[3].codeSnType = "EQUIPMENT";
                pass_Stn.productInfo.bindCode[3].replace = "1";

                pass_Stn.equipmentInfo = new EquipmentInfo();
                pass_Stn.equipmentInfo.equipmentIp = Global.Trace_ip;
                pass_Stn.equipmentInfo.equipmentType = "L";
                pass_Stn.equipmentInfo.equipmentNo = Global.equipmentNo;
                pass_Stn.equipmentInfo.vendorId = "HG";
                pass_Stn.equipmentInfo.processRevs = Global.version;

                pass_Stn.recipeInfo = new RecipeInfo();
                pass_Stn.recipeInfo.startTime = DateTime.Now.AddSeconds(-1).ToString("yyyyMMddHHmmss");
                pass_Stn.recipeInfo.endTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                pass_Stn.recipeInfo.cavity = "0" + (new Random().Next(10) + 1);
                pass_Stn.recipeInfo.judgement = "PASS";
                pass_Stn.recipeInfo.humidity = "63";
                pass_Stn.recipeInfo.temperature = "35";
                pass_Stn.recipeInfo.paraInfo = new ParaInfo[17];

                var heightsValue = new Dictionary<string, heightModel>();
                if (heights.ContainsKey(SN))
                {
                    heights.TryGetValue(SN, out heightsValue);
                }
                for (int i = 0; i < ResultModel.Model.Length; i++)
                {
                    pass_Stn.recipeInfo.paraInfo[i] = new ParaInfo(ResultModel.Model[i], heightsValue);

                }
                heights.Remove(this.txt_SN.Text);
                pass_Stn.appleReturnInfo = new AppleReturnInfo();
                pass_Stn.appleReturnInfo.status = "200";
                pass_Stn.appleReturnInfo.id = Trace_id;
                pass_Stn.appleReturnInfo.contact = "Vendor";
                pass_Stn.appleReturnInfo.error = "";

                string send_Pass = JsonConvert.SerializeObject(pass_Stn, Formatting.None, jsetting).Replace("a1st", "1st").Replace("a2nd", "2nd").Replace("a3rd", "3rd").Replace("a4th", "4th").Replace("a5th", "5th").Replace("a6th", "6th"); ;
                Log.WriteLog("A工位Equipment to  API 過站報告:" + send_Pass);
                RequestAPI2.CallBobcat4(passURL, send_Pass, hashCode_newstn, out callResult, out errMsg);
                Log.WriteLog("A工位 API to Equipment 過站報告:" + callResult);
                JObject recvObj = JsonConvert.DeserializeObject<JObject>(callResult);
                if (recvObj["rc"].ToString() == "000")
                {
                    //richTextBox9.AppendText("过站OK！");
                    //this.lbl_result.Text = "Pass";
                    //this.lbl_result.ForeColor = Color.Green;
                    //this.txt_SN.Text = "";
                    //this.txt_LeftRail.Text = "";
                    //this.txt_RightRail.Text = "";
                    return new ResultModel() { Result = true, Message = "Pass" };
                }
                else
                {
                    //this.lbl_result.Text = "Fail";
                    //this.lbl_result.ForeColor = Color.Red;
                    return new ResultModel() { Result = false, Message = "过站失败!" + callResult + "，" + errMsg };
                }
            }
            catch (Exception ex)
            {
                //this.lbl_result.Text = "Fail";
                //this.lbl_result.ForeColor = Color.Red;
                Log.WriteLog("A工位 API to Equipment 过站异常:" + ex.Message);
                return new ResultModel() { Result = false, Message = "过站失败!" + callResult + "，" + errMsg };
            }
        }

        private void Sfcupload(string SN, string leftrail, string rightrail, string fixture)
        {
            try
            {
                string callResult = "";
                string errMsg = "";

                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值
                #region SFC

                string cavity = "1";

                //Sc.UpdateUI("过站产品码：" + barcode, "Rtxt_Send_Pass");
                //Sc.UpdateUI("过站左键码：" + left_rail, "Rtxt_Send_Pass");
                //Sc.UpdateUI("过站右键码：" + right_rail, "Rtxt_Send_Pass");
                //Sc.UpdateUI("过站治具码：" + fixture, "Rtxt_Send_Pass");
                var SfcStn = new SfcStn()
                {
                    hashCode = hashCode_newstn,
                    requestId = requestId_check,
                    resv1 = "",

                    productInfo = new FlHelper.Models.ProductInfo()
                    {
                        barCode = SN,
                        barCodeType = Global.barCodeType,
                        clientCode = "",
                        station = Global.station,
                        billNo = "GL-234M-LC-EVT-X-NSF-01",
                        product = "234M",
                        phase = "EVT",
                        config = "Config999",
                        lineName = Global.Trace_line_id,
                        stationId = Global.Trace_station_id,
                        stationString = "",
                        bindCode = new FlHelper.Models.BindCode[] {
                                        new FlHelper.Models.BindCode() {codeSn=leftrail,codeSnType="IQC_LEFT_RAIL",replace="1" },
                                         new FlHelper.Models.BindCode() {codeSn=rightrail,codeSnType="IQC_RIGHT_RAIL",replace="1" },
                                          new FlHelper.Models.BindCode() {codeSn=fixture,codeSnType="FIXTURE",replace="1" },
                                           new FlHelper.Models.BindCode() {codeSn=Global.equipmentNo,codeSnType="EQUIPMENT",replace="1" },

                        },
                    },
                    equipmentInfo = new FlHelper.Models.EquipmentInfo()
                    {
                        equipmentIp = Global.Trace_ip,
                        equipmentType = "L",
                        equipmentNo = Global.equipmentNo,
                        vendorId = "HG",
                        processRevs = Global.version,
                        softwareName = "HG数据追溯系统",
                        softwareVersion = Global.version,
                        patternName = "HG准直激光控制系统",
                        patternVersion = Global.version,
                        ccdVersion = Global.version,
                        ccdGuideVersion = Global.version,
                        ccdInspectionName = "CCD_Moniton",
                        ccdInspectionVersion = Global.version,
                    },
                    recipeInfo = new FlHelper.Models.RecipeInfo()
                    {
                        startTime = DateTime.Now.AddSeconds(-25).ToString("yyyyMMddHHmmss"),
                        endTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        cavity = "0" + cavity,
                        judgement = "PASS",
                        humidity = "63",
                        temperature = "35",
                        processStartTime = DateTime.Now.AddSeconds(-25).ToString("yyyyMMddHHmmss"),
                        processEndTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        waitCt = "",
                        ct = "",
                        tossingItem = "",
                        errorInfo = new ErrorInfo[] { },
                        paraInfo = ResultModel.Model,
                    },
                };

                string send_Pass = JsonConvert.SerializeObject(SfcStn, Formatting.None, jsetting).Replace("a1st", "1st").Replace("a2nd", "2nd").Replace("a3rd", "3rd").Replace("a4th", "4th").Replace("a5th", "5th").Replace("a6th", "6th");
                //richTextBox10.AppendText("A工位 Equipment to API 参数上抛報告:" + send_Pass);
                Sc.PassStation("http://10.176.46.59:8081/LCH/proto", SfcStn, Global.hashCode);

            }
            catch (Exception ex)
            {

            }
            #endregion
        }

        private void AddRichText(RichTextBox richTextBox, string message)
        {
            richTextBox.AppendText(message + "\r\n");
            Log.WriteLog(message);
        }
        private void updatelabeltext(Label label, string message, bool result = true, bool isuse = false)
        {
            label.Text = message;
            if (isuse)
            {
                label.ForeColor = result == false ? Color.Red : Color.Green;
            }
        }





    }


}
