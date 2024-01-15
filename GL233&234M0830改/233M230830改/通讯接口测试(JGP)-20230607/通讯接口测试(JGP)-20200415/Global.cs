using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 通讯接口测试_JGP__20200415
{
    public class Global
    {
        #region 报站，上料检验，Trace，过站接口的参数

        public static string equipmentNo = "M00093730002";//设备编号
        public static string station = "LCHWelding";//工站名称
        public static string barCodeType = "LC";//2D码类型
        public static string Trace_ip = "10.175.18.149";//Trace IP
        public static string version = "V2.0.4.0_T1.0.0";


        public static string Trace_line_id = "B05-3FA-01";//机台line_id
        public static string Trace_station_id = "IPGL_B05-3FA-01_7_STATION1661";//机台station_id

        public static string hashCode = "A704F36AA4ED82E2D42A7646D74F0AC7226C6329EDA422BABAD744004E48BD0962259B6101C68903886501D9F95B9E9B2DADEED1097F1BDF";
        

        public static string TraceURL = "http://localhost:8765/v2/log_batch";

        #endregion

    }
}
