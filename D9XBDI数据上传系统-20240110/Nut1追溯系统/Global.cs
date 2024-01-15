
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
        public static DateTime SelectDateTime = DateTime.Now;
        public static DateTime SelectDTTime = DateTime.Now;
        public static DateTime SelectTOP5Time = DateTime.Now;
        public static DateTime SelectFixturetime = DateTime.Now;

        
        public static string[] PLC_Read_String = new string[50];//读OPC服务器
      
        public static short[] PLC_Read_Short = new short[50];//读OPC服务器

        public static short[] PLC_Read_Short_Before = new short[50];//PLC上次读的状态

        public static int[] PLC_Read_Int = new int[50];//读OPC服务器

        public static short[] PLC_Write_Short = new short[50];//写OPC服务器



        public static string item_Check = string.Empty;
        public static string itemRevision_Check = string.Empty;

        //public static AsyncTcpServer server1;
        public static List<int> Statistics_Index = new List<int>();//数据统计DataGridView索引
        public static short[] Product_NG_Detail = new short[20];//白班NG明细(PC)
        public static short[] Product_NG_Detail_N1 = new short[20];//夜班NG明细(PC)

        public static Dictionary<int, Tossing> Tossing_Item = new Dictionary<int, Tossing>();

        public static string hashCode;
        public static string billNo;

        public static bool b_Trace = true;//用于Trace上传和缓存重传的标志

        public static OpcUaHelper.OpcUaClient opcUaClient;

        //public static InovanceAMTcp plc1 = new InovanceAMTcp("192.168.1.10", 502);
        //public static InovanceAMTcp plc2 = new InovanceAMTcp("192.168.1.40", 502);
        //public static InovanceAMTcp plc3 = new InovanceAMTcp("192.168.1.50", 502);

        public static double CT = 8.5;//理论CT

        public static int Product_Total_D = 0;
        public static int Product_Total_N = 0;
        public static int Product_OK_D = 0;
        public static int Product_OK_N = 0;
        public static short[] Product_Total;//白班总产能
        public static short[] Product_NG;//白班NG产能
        public static short[] Product_OK;//白班OK产能
        public static short[] Product_Total_N_1;//夜班总产能1
        public static short[] Product_Total_N_2;//夜班总产能2
        public static short[] Product_NG_N_1;//夜班NG产能1
        public static short[] Product_NG_N_2;//夜班NG产能2
        public static short[] Product_OK_N_1;//夜班OK产能1
        public static short[] Product_OK_N_2;//夜班OK产能2
        public static short[] T_RunTime;//白班运行时间
        public static double[] DT_RunTime = new double[24];
        public static short[] T_ErrorTime;//白班异常时间
        public static double[] DT_ErrorTime = new double[24];
        public static short[] T_PendingTime;//白班待料时间
        public static double[] DT_PendingTime = new double[24];//白班待料时间
        public static double[] DT_RunTime_N1 = new double[24];//夜班运行时间
        public static short[] DT_RunTime_N2;//夜班运行时间
        public static double[] DT_ErrorTime_N1 = new double[24];//夜班异常时间
        public static short[] DT_ErrorTime_N2;//夜班异常时间
        public static double[] DT_PendingTime_N1 = new double[24];//夜班待料时间
        public static short[] DT_PendingTime_N2;//夜班待料时间

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




        #endregion

        #region OEE参数
        public static short OEE_Code = 1;//上笔状态

        public static string start_time = string.Empty;//每笔状态的开始时间

        #endregion

        #region 报站|上料检验|过站的接口参数

        //接口获取的URL
        /// <summary>
        /// 上料检验URL
        /// </summary>
        public static string checkURL = "http://localhost:8965/dochk";
        /// <summary>
        /// 过站URL
        /// </summary>
        public static string passURL = "http://localhost:8965/dopass";
        /// <summary>
        /// TraceURL
        /// </summary>
        public static string TraceURL = "http://localhost:8765/v2/log_batch";

        /// <summary>
        /// 参数上抛URL
        /// </summary>
        public static string para_URL = "http://10.176.46.59:8081/LCH/proto";


        public static string equipmentNo = "MO1300";//设备编号
        public static string station = "LCHWelding";//工站名称
        public static string barCodeType = "LC";//2D码类型
        public static string Trace_ip = "10.203.113.159";//Trace IP
        public static string version = "V2.0.4.0_T1.0.0";


        public static string Trace_line_id = "J01-3FA-02";//机台line_id
        public static string Trace_station_id = "IPWH_J01-3FA-02_15_STATION1661";//机台station_id

        public static AsyncTcpClient client1 { set; get; }
        public static AsyncTcpClient client2 { get; set; }
        public static AsyncTcpClient client3 { get; set; }
        public static AsyncTcpClient client4 { get; set; }
        public static AsyncTcpServer server1 { get; set; }


        #endregion       
    }

    public class Tossing
    {
        public Tossing()
        { }
        /// <summary>
        /// 抛料类型
        /// </summary>
        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// 抛料名称
        /// </summary>
        private string chinese_value;

        public string Chinese_Value
        {
            get { return chinese_value; }
            set { chinese_value = value; }
        }

        /// <summary>
        /// 抛料名称
        /// </summary>
        private string english_value;

        public string English_Value
        {
            get { return english_value; }
            set { english_value = value; }
        }

        /// <summary>
        /// 抛料代码
        /// </summary>
        private string code;

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

    }
}
