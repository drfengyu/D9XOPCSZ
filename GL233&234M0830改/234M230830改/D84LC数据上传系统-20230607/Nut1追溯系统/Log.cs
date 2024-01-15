using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace 卓汇数据追溯系统
{
    public class Log
    {
        private static string logPath = string.Empty;
        private static string logPath2 = "D:\\ZHH\\操作记录\\";
        private static object _lock = new object();

        public static string LogPath
        {
            get
            {
                if (logPath == string.Empty)
                {
                    logPath = "D:\\日志文件\\";
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

        public static void WriteLog_ServerInfo(string text)
        {
            System.IO.StreamWriter sw = null;
            if (!Directory.Exists("D:\\ZHH\\ServerInfo\\"))
            {
                Directory.CreateDirectory("D:\\ZHH\\ServerInfo\\");
            }
            string filePath = "D:\\ZHH\\ServerInfo\\" + DateTime.Now.ToString("yyyyMMdd") + ".Log";
            lock (_lock)
            {
                try
                {
                    using (sw = new StreamWriter(filePath, true, Encoding.Default))
                    {
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + ": " + text);
                    }
                    sw.Close();
                    sw.Dispose();
                }
                catch
                { }
            }
        }

        public static void WriteCSV_HansInfo(string text)
        {
            System.IO.StreamWriter sw = null;
            if (!Directory.Exists("D:\\ZHH\\HansInfo\\"))
            {
                Directory.CreateDirectory("D:\\ZHH\\HansInfo\\");
            }
            string filePath = "D:\\ZHH\\HansInfo\\" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
            if (!File.Exists(filePath))
            {
                using (sw = new StreamWriter(filePath, true, Encoding.Default))
                {
                    string str = "Time,Barcode,Fixture,Laser_id,Cavity";
                    sw.WriteLine(str);
                }
            }
            lock (_lock)
            {
                try
                {
                    using (sw = new StreamWriter(filePath, true, Encoding.Default))
                    {
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "," + text);
                    }
                    sw.Close();
                    sw.Dispose();
                }
                catch
                {
                }
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

        public static void WriteCSV(string text, string LogPath1 = "D:\\ZHH\\ThreeMaRecord\\")
        {
            System.IO.StreamWriter sw = null;
            if (!Directory.Exists(LogPath1))
            {
                Directory.CreateDirectory(LogPath1);
            }
            string fileFullFileName = LogPath1 + DateTime.Now.ToString("yyyyMMdd") + ".csv";
            if (!File.Exists(fileFullFileName))
            {
                using (sw = new StreamWriter(fileFullFileName, true, Encoding.Default))
                {
                    string str = "";

                    if (LogPath1.Contains("ThreeMaRecord"))
                    {
                        str = "记录时间,所在进度,中板码,左键码,右键码,结果";
                    }
                    if (LogPath1.Contains("PassMaRecord"))
                    {
                        str = "记录时间,中板码,左键码,右键码,焊机号";
                    }
                    if (LogPath1.Contains("PassModelList记录"))
                    {
                        str = "记录时间,中板码,左键码,右键码,RequestId,操作";
                    }
                    if (LogPath1.Contains("测高记录"))
                    {
                        str = "记录时间,中板码,焊机号,焊点,测高标定值,差异值,实测值";
                    }
                    if (LogPath1.Contains("NG明细"))
                    {
                        str = "记录时间,中板码,左键码,右键码,原因";
                    }
                    //if (LogPath1.Contains("OEE_小料抛料计数数据"))
                    //{
                    //    str = "时间,合计抛料数量,总共抛料数量,UA抛料数量,LA抛料数量,上传状态";
                    //}
                    //if (LogPath1.Contains("UpLoad_HeartBeat"))
                    //{
                    //    str = "记录时间,故障状态,故障代码,故障信息,上传状态";
                    //}
                    //if (LogPath1.Contains("OEE_Default"))
                    //{
                    //    str = "Time,Procuct,full_sn,Fixture_id,Start_Time,EndTime,Status,ActualCT,SwVersion,DefectCode,SendStatus";
                    //}
                    //if (LogPath1.Contains("日志文件"))
                    //{
                    //    str = "Time,SystemInfo";
                    //}
                    //if (LogPath1.Contains("备件更换记录"))
                    //{
                    //    str = "类别,品名,规格,标准寿命,实际使用次数,上次更换时间,当前时间,更换原因,处理生技";
                    //}
                    //if (LogPath1.Contains("OEE_demo"))
                    //{
                    //    str = "记录时间,URL路径,Data";
                    //}
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
                    //foreach (Process process in System.Diagnostics.Process.GetProcesses())
                    //{
                    //    if (process.ProcessName.ToUpper().Equals("wps"))
                    //        process.Kill();    //杀进程
                    //}
                    //GC.Collect();
                    //Thread.Sleep(200);
                    //using (sw = System.IO.File.AppendText(fileFullFileName))
                    //{

                    //    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd,HH:mm:ss:,") + text);

                    //}
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
