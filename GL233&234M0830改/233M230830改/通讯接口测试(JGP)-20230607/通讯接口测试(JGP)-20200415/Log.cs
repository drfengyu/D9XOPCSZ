using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace 通讯接口测试_JGP__20200415
{
    public class Log
    {
        private static string logPath = string.Empty;
        private static string logPath2 = System.AppDomain.CurrentDomain.BaseDirectory + "操作记录\\";
        private static object _lock = new object();

        public static string LogPath
        {
            get
            {
                if (logPath == string.Empty)
                {
                    logPath = System.AppDomain.CurrentDomain.BaseDirectory + "日志文件\\";
                }
                return logPath;
            }
            set { Log.logPath = value; }
        }

        public static void WriteLog(string text)
        {
            System.IO.StreamWriter sw = null;
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }
            if (!Directory.Exists(logPath2))
            {
                Directory.CreateDirectory(logPath2);
            }
            string fileFullFileName = LogPath + DateTime.Now.ToString("yyyyMMdd") + ".Log";
            string fileFullFileName2 = logPath2 + DateTime.Now.ToString("yyyyMMdd") + ".csv";
            if (!File.Exists(fileFullFileName2))
            {
                using (sw = new StreamWriter(fileFullFileName2, true, Encoding.Default))
                {
                    string str = "Time,Data";
                    sw.WriteLine(str);
                }
            }
            lock (_lock)
            {
                try
                {
                    using (sw = new StreamWriter(fileFullFileName, true, Encoding.Default))
                    {
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + ": " + text);
                    }
                    using (sw = new StreamWriter(fileFullFileName2, true, Encoding.Default))
                    {
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + text.Replace(",", "|"));
                    }
                    sw.Close();
                    sw.Dispose();
                }
                catch
                { }
            }
        }

        public static void WriteOEELog(string text)
        {
            System.IO.StreamWriter sw = null;
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }
            string fileFullFileName = LogPath + "OEE-Demo" + ".Log";
            lock (_lock)
            {
                try
                {
                    using (sw = new StreamWriter(fileFullFileName, true, Encoding.Default))
                    {
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + text);
                    }

                    sw.Close();
                    sw.Dispose();
                }
                catch
                {

                }
            }
        }

        public static void WriteCSV(string text, string FileName)
        {
            System.IO.StreamWriter sw = null;
            string time = DateTime.Now.ToString("yyyyMMdd");

            ///夜班8小时也计入上一天产能记录表 当天 8-23 + 第二天0-7
            if (DateTime.Now.Hour < 8)
            {
                time = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");

            }
            string RootPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\产能与时间统计\\" + time + "\\";
            if (!Directory.Exists(RootPath))
            {
                Directory.CreateDirectory(RootPath);
            }
            string fileFullFileName = RootPath + "233M" + time + FileName + ".csv";
            if (!File.Exists(fileFullFileName))
            {
                using (sw = new StreamWriter(fileFullFileName, true, Encoding.Default))
                {
                    string str = "";


                    if (FileName.Contains("I件产能统计"))
                    {
                        str = "时间段,OK组,NG组,左键,右键,良率";
                    }
                    if (FileName.Contains("下料产能统计"))
                    {
                        str = "时间段,OK组,NG组,良率";
                    }
                    if (FileName.Contains("#产能统计"))
                    {
                        str = "时间段,OK组,Trace,焊接,良率";
                    }
                    if (FileName.Contains("LC产能统计"))
                    {
                        str = "时间段,OK组,Trace,扫码,良率";
                    }
                    if (FileName.Contains("时间统计"))
                    {
                        str = "时间段,运行,报警,待机,稼动率";
                    }
                    sw.WriteLine(time + FileName);
                    sw.WriteLine(str);
                }
            }
            lock (_lock)
            {
                try
                {
                    using (sw = new StreamWriter(fileFullFileName, true, Encoding.Default))
                    {
                        sw.WriteLine(text);
                    }
                    sw.Close();
                    sw.Dispose();
                }
                catch
                {

                }
            }
        }

        public static void WriteCSV_NG(string text, string LogPath1)
        {
            System.IO.StreamWriter sw = null;
            if (!Directory.Exists(LogPath1))
            {
                Directory.CreateDirectory(LogPath1);
            }
            string fileFullFileName = LogPath1 + "sendNG.csv";
            if (!File.Exists(fileFullFileName))
            {
                using (sw = new StreamWriter(fileFullFileName, true, Encoding.Default))
                {
                    string str = "Time,Procuct,full_sn,Fixture_id,Start_Time,Weld_start_time,Weld_stop_time,Stop_Time,Weld_wait_ct,Actual_weld_ct,Status";
                    sw.WriteLine(str);
                }
            }
            lock (_lock)
            {
                try
                {
                    using (sw = new StreamWriter(fileFullFileName, true, Encoding.Default))
                    {
                        sw.WriteLine(text);
                    }
                    sw.Close();
                    sw.Dispose();
                }
                catch
                {
                }
            }
        }

        public static void WriteCSV_NUM(string text)
        {
            System.IO.StreamWriter sw = null;
            if (!Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "\\月产量数据统计\\"))
            {
                Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + "\\月产量数据统计\\");
            }
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\月产量数据统计\\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + ".csv";
            if (!File.Exists(filePath))
            {
                using (sw = new StreamWriter(filePath, true, Encoding.Default))
                {
                    //string str = "Time,OK产品数量,NG产品数量,良率,稼动率";
                    string str = "Time,OK产品数量,NG产品数量";
                    sw.WriteLine(str);
                }
            }
            lock (_lock)
            {
                try
                {
                    using (sw = new StreamWriter(filePath, true, Encoding.Default))
                    {
                        sw.WriteLine(text);
                    }
                    sw.Close();
                    sw.Dispose();
                }
                catch
                {
                }
            }
        }
        public static void WriteCSV_PDCA(string text)
        {
            System.IO.StreamWriter sw = null;
            if (!Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "\\PDCA上传成功数据统计\\"))
            {
                Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + "\\PDCA上传成功数据统计\\");
            }
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\PDCA上传成功数据统计\\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + ".csv";
            if (!File.Exists(filePath))
            {
                using (sw = new StreamWriter(filePath, true, Encoding.Default))
                {
                    //string str = "Time,OK数量,NG数量,良率,稼动率";
                    string str = "Time,UA数量,LA数量";
                    sw.WriteLine(str);
                }
            }
            lock (_lock)
            {
                try
                {
                    using (sw = new StreamWriter(filePath, true, Encoding.Default))
                    {
                        sw.WriteLine(text);
                    }
                    sw.Close();
                    sw.Dispose();
                }
                catch
                {
                }
            }
        }

        public static void WriteCSV_DiscardLog(string text)
        {
            System.IO.StreamWriter sw = null;
            if (!Directory.Exists(@"F:\装机软件\系统配置\OEE_抛料日志数据\" + DateTime.Now.ToString("yyyyMM")))
            {
                Directory.CreateDirectory(@"F:\装机软件\系统配置\OEE_抛料日志数据\" + DateTime.Now.ToString("yyyyMM"));
            }
            string filePath = @"F:\装机软件\系统配置\OEE_抛料日志数据\" + DateTime.Now.ToString("yyyyMM") + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
            if (!File.Exists(filePath))
            {
                using (sw = new StreamWriter(filePath, true, Encoding.Default))
                {
                    //string str = "Time,OK数量,NG数量,良率,稼动率";
                    string str = "系统类型,上传时间,工站,机台号,SN,上传数据,报错信息,上传次数";
                    sw.WriteLine(str);
                }
            }
            lock (_lock)
            {
                try
                {
                    using (sw = new StreamWriter(filePath, true, Encoding.Default))
                    {
                        sw.WriteLine(text);
                    }
                    sw.Close();
                    sw.Dispose();
                }
                catch
                {
                }
            }
        }
    }
}
