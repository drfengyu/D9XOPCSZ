
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.Profinet.Inovance;
using HslCommunication;


namespace 卓汇数据追溯系统
{
    public class Global
    {
        public static Bitmap ReadImageFile(string path)
        {
            if (!File.Exists(path))
            {
                return null;//文件不存在
            }
            FileStream fs = File.OpenRead(path); //OpenRead
            int filelength = 0;
            filelength = (int)fs.Length; //获得文件长度 
            byte[] image = new Byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
            fs.Close();
            Bitmap bit = new Bitmap(result);
            return bit;
        }

        public enum LoginLevel
        {
            Operator,
            Technician,
            Administrator
        }
        public static string Operator_pwd = string.Empty;
        public static string Technician_pwd = string.Empty;
        public static string Administrator_pwd = string.Empty;
        public static LoginLevel Login;
        public static IniProductFile inidata;
        //public static Melsoft_PLC_TCP2 PLC_Client = new Melsoft_PLC_TCP2();//三菱PLC



        public static string item_Check = string.Empty;
        public static string itemRevision_Check = string.Empty;
        public static AsyncTcpClient client1 { set; get; }
        //public static AsyncTcpServer server1;


        public static string hashCode;
        public static string billNo;

        public static bool b_Trace;//用于Trace上传和缓存重传的标志


        public static InovanceAMTcp plc1 = new InovanceAMTcp("192.168.1.10", 502);
        public static InovanceAMTcp plc2 = new InovanceAMTcp("192.168.1.40", 502);
        public static InovanceAMTcp plc3 = new InovanceAMTcp("192.168.1.50", 502);


        #region OEE----所有变量
        public static string errorStatus = "5";
        public static string errorcode = "70010001";
        public static string errorinfo = "焊机参数调整";

        public static bool SelectManualErrorCode = false;
        public static bool SelectFirstModel = false;
        public static bool SelectTestRunModel = false;
        public static bool Error_PendingStatus = false;//待料状态下是否开启安全门
        public static bool STOP = false;//是否按下停止按钮


        public static string BreakStartTime = "";//吃饭休息开始时间

        public static bool errorTime1 = false;

        public static bool BreakStatus = false;//是否是吃饭休息状态
        public static int j = -1;//OEE机台大状态初始值

        public static ErrorData errordata = new ErrorData();
        public static Dictionary<int, ErrorData> ed = new Dictionary<int, ErrorData>();



        public static int Error_PendingNum = -1;
        public static int Error_num = -1;
        public static int Error_Stopnum = -1;

        public static int OEE_Code = 1;

        public static string start_time;
        public static int OEE_CodeB=1;
        public static string start_timeB;
        public static int OEE_CodeC=1;
        public static string start_timeC;
        public static HomeFrm _homefrm;
        #endregion


        #region 报站|上料检验|过站的接口参数

        //接口获取的URL
        public static string checkURL = "http://localhost:8965/dochk";   //
        public static string passURL = "http://localhost:8965/dopass";
        public static string TraceURL = "http://localhost:8765/v2/log_batch";


        public static string equipmentNo = "M00093730002";//设备编号
        public static string station = "LCHWelding";//工站名称
        public static string barCodeType = "LC";//2D码类型
        public static string Trace_ip = "10.175.18.149";//Trace IP
        public static string version = "V2.0.4.0_T1.0.0";
        public static string Trace_line_id = "C12-4FA-01";//机台line_id
        public static string Trace_station_id = "IPGL_C12-4FA-01_7_STATION1661";//机台station_id
        

        public static AsyncTcpClient client2 { get;  set; }
        public static AsyncTcpClient client3 { get;  set; }
        public static AsyncTcpClient client4 { get;  set; }
        public static AsyncTcpServer server1 { get;  set; }


        #endregion



        
    }
}
