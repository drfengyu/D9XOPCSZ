using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Dynamic;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using HslCommunication.Profinet.Inovance;
using HslCommunication;
using System.IO.Ports;
using System.Threading;
using FlHelper.Models;
using FlHelper;
using System.IO;
using FlHelper.Helpers;
using 通讯接口测试_JGP__20200415.Properties;

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
        InovanceAMTcp plc1 = new InovanceAMTcp("192.168.1.10", 502);
        InovanceAMTcp plc2 = new InovanceAMTcp("192.168.1.40", 502);
        InovanceAMTcp plc3 = new InovanceAMTcp("192.168.1.50", 502);
        private void MainForm_Load(object sender, EventArgs e)
        {
            //plc1.ConnectServer();
            //plc2.ConnectServer();
            //plc3.ConnectServer();
            //this.cmbPort.DataSource = SerialPort.GetPortNames();
            //this.cmbPort.Text = "COM1";

            Sc = new SendSfc();
            Sc.UpdateUI += AppendRichText;
            //DeletePic(1);
            //Thread t = new Thread(ProductTotal);
            //t.IsBackground = true;
            //t.Start();
            DeletePic(2);
            Sc.DeleteAll("D:\\日志文件\\");
        }
        private void DeletePic(int HansId, int OutDays = 14)
        {
            try
            {
                string path = "E:\\public\\HG" + HansId + "#";
                Sc.DeleteAll(path);
            }
            catch (Exception ex)
            {

                Log.WriteLog("图片清除异常:" + ex.Message);
            }
        }
        public void ProductTotal() {
            while (true)
            {
                try
                {


                    if (DateTime.Now.Minute == 52 && DateTime.Now.Second == 57)
                    {
                        ///白班 I件记录 OK NG 左键 右键 良率 OK NG 左键 右键 良率 夜班 总OK 总NG 总良率 总OK 总NG 总良率
                        WriteI(new string[] { "MW30200", "MW30800", "MW30230", "MW30600", "MW30260", "MW31200", "MW31800", "MW31230", "MW31600", "MW31260", "MW30102", "MW30108", "MW30106", "MW31102", "MW31108", "MW31106" }, "I件产能统计", plc1);
                        ///LC记录 OK Trace 扫码 良率 OK Trace 扫码 良率 总OK 总NG 总良率 总OK 总NG 总良率
                        WriteI(new string[] { "MW32200", "MW32230", "MW32600", "MW32260","",  "MW33200", "MW33230", "MW33600", "MW33260","",
            "MW32102", "MW32104", "MW32106", "MW33102", "MW33104", "MW33106"}, "LC产能统计", plc1);
                        ///1# OK Trace 焊接 良率 无 OK Trace 焊接 良率 无 总OK 总NG 总良率 总OK 总NG 总良率
                        WriteI(new string[] { "MW30200", "MW30230", "MW31000", "MW30260", "", "MW31200", "MW31230", "MW32000", "MW31260", "",
            "MW30102", "MW30104", "MW30106", "MW31102", "MW31104", "MW31106"}, "焊接1#产能统计", plc2);
                        ///2# OK Trace 焊接 良率 无 OK Trace 焊接 良率 无 总OK 总NG 总良率 总OK 总NG 总良率
                        WriteI(new string[] { "MW30200", "MW30230", "MW31000", "MW30260", "", "MW31200", "MW31230", "MW32000", "MW31260", "",
            "MW30102", "MW30104", "MW30106", "MW31102", "MW31104", "MW31106"}, "焊接2#产能统计", plc3);
                        ///成品 OK NG 良率 无 无 OK NG 良率 无 无 总OK 总NG 总良率 总OK 总NG 总良率
                        WriteI(new string[] { "MW34200", "MW34230", "MW34260", "", "", "MW35200", "MW35230", "MW35260", "", "",
            "MW34102", "MW34104", "MW34106", "MW35102", "MW35104", "MW35106"}, "下料产能统计", plc1);

                        ///白班 I件时间记录 运行 报警 待机 稼动率 无 夜 运行 报警 待机 稼动率 无 白总稼动率 夜总稼动率
                        WriteI(new string[] { "MW30400", "MW30430", "MW30460", "MW30490", "", "MW31400", "MW31430", "MW31460", "MW31490", "", "MW30110", "MW30112", "MW30114", "MW31110", "MW31112", "MW31114", "MW30116", "MW31116" }, "I件时间统计", plc1);

                        ///白班 LC时间记录 运行 报警 待机 稼动率 无 夜 运行 报警 待机 稼动率 无
                        WriteI(new string[] { "MW32400", "MW32430", "MW32460", "MW32490", "", "MW33400", "MW33430", "MW33460", "MW33490", "", "MW32110", "MW32112", "MW32114", "MW33110", "MW33112", "MW33114", "MW32116", "MW33116" }, "LC时间统计", plc1);

                        ///白班 焊接1# 运行 报警 待机 稼动率 无 夜 运行 报警 待机 稼动率 无
                        WriteI(new string[] { "MW30400", "MW30430", "MW30460", "MW30490", "", "MW31400", "MW31430", "MW31460", "MW31490", "", "MW30110", "MW30112", "MW30114", "MW31110", "MW31112", "MW31114", "MW30116", "MW31116" }, "焊接1#时间统计", plc2);
                        ///白班 焊接2# 运行 报警 待机 稼动率 无 夜 运行 报警 待机 稼动率 无
                        WriteI(new string[] { "MW30400", "MW30430", "MW30460", "MW30490", "", "MW31400", "MW31430", "MW31460", "MW31490", "", "MW30110", "MW30112", "MW30114", "MW31110", "MW31112", "MW31114", "MW30116", "MW31116" }, "焊接2#时间统计", plc3);
                        ///成品 运行 报警 待机 稼动率 无 夜 运行 报警 待机 稼动率 无
                        WriteI(new string[] { "MW34400", "MW34430", "MW34460", "MW34490", "", "MW35400", "MW35430", "MW35460", "MW35490", "", "MW34110", "MW34112", "MW34114", "MW35110", "MW35112", "MW35114", "MW34116", "MW35116" }, "下料时间统计", plc1);
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        //private void IWrite(float[] a, float[] b, float[] c, float[] d, float[] e, string FileName = "")
        //{

        //    short[] A = convertFloatToShort(a);
        //    short[] B = convertFloatToShort(b);
        //    short[] C = convertFloatToShort(c);



        //    int i = 0;
        //    string time = "";
        //    switch (DateTime.Now.Hour)
        //    {
        //        case 8:

        //        case 20:
        //            i = 0;
        //            time = DateTime.Now.Hour == 8 ? "8:00-8:59" : "20:00-20:59";

        //            break;
        //        case 9:
        //        case 21:
        //            i = 2;
        //            time = DateTime.Now.Hour == 9 ? "9:00-9:59" : "21:00-21:59";
        //            break;
        //        case 10:
        //        case 22:
        //            i = 4;
        //            time = DateTime.Now.Hour == 10 ? "10:00-10:59" : "22:00-22:59";
        //            break;
        //        case 11:
        //        case 23:
        //            i = 6;
        //            time = DateTime.Now.Hour == 11 ? "11:00-11:59" : "23:00-23:59";
        //            break;
        //        case 12:
        //        case 24:
        //        case 0:
        //            i = 8;
        //            time = DateTime.Now.Hour == 12 ? "12:00-12:59" : "0:00-0:59";
        //            break;
        //        case 13:
        //        case 1:
        //            i = 10;
        //            time = DateTime.Now.Hour == 13 ? "13:00-13:59" : "1:00-1:59";
        //            break;
        //        case 14:
        //        case 2:
        //            i = 12;
        //            time = DateTime.Now.Hour == 14 ? "14:00-14:59" : "2:00-2:59";
        //            break;
        //        case 15:
        //        case 3:
        //            i = 14;
        //            time = DateTime.Now.Hour == 15 ? "15:00-15:59" : "3:00-3:59";
        //            break;
        //        case 16:
        //        case 4:
        //            i = 16;
        //            time = DateTime.Now.Hour == 16 ? "16:00-16:59" : "4:00-4:59";
        //            break;
        //        case 17:
        //        case 5:
        //            i = 18;
        //            time = DateTime.Now.Hour == 17 ? "17:00-17:59" : "5:00-5:59";
        //            break;
        //        case 18:
        //        case 6:
        //            i = 20;
        //            time = DateTime.Now.Hour == 18 ? "18:00-18:59" : "6:00-6:59";
        //            break;
        //        case 19:
        //        case 7:
        //            i = 22;
        //            time = DateTime.Now.Hour == 19 ? "19:00-19:59" : "7:00-7:59";
        //            break;
        //        default:
        //            break;
        //    }
        //    if (d != null)
        //    {
        //        short[] D = convertFloatToShort(d);

        //        if (e != null)
        //        {
        //            short[] E = convertFloatToShort(e);
        //            Log.WriteCSV(time + "," + A[i] + "," + B[i] + "," + C[i] + "," + D[i] + "," + E[i], FileName);
        //        }
        //        else
        //        {
        //            Log.WriteCSV(time + "," + A[i] + "," + B[i] + "," + C[i] + "," + D[i] + ",", FileName);
        //        }

        //    }
        //    else
        //    {
        //        Log.WriteCSV(time + "," + A[i] + "," + B[i] + "," + C[i], FileName);
        //    }


        //}
        //private void WriteI(string[] a, string FileName = "", InovanceTcpNet plc1 = null)
        //{
        //    //I件 LC  1# 2#焊接 成品
        //    try
        //    {
        //        if (DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 20)
        //        {
        //            //DAY
        //            //ok
        //            OperateResult<float[]> CountIEOK = plc1.ReadFloat(a[0], 12);
        //            //ng
        //            OperateResult<float[]> CountIENG = plc1.ReadFloat(a[1], 12);
        //            //l
        //            OperateResult<float[]> CountIEL = plc1.ReadFloat(a[2], 12);
        //            //R
        //            OperateResult<float[]> CountIER = new OperateResult<float[]>();
        //            if (!string.IsNullOrEmpty(a[3]))
        //            {
        //                CountIER = plc1.ReadFloat(a[3], 12);

        //                //LV
        //                OperateResult<float[]> CountIELV = new OperateResult<float[]>();

        //                if (!string.IsNullOrEmpty(a[4]))
        //                {
        //                    CountIELV = plc1.ReadFloat(a[4], 12);

        //                    IWrite(CountIEOK.Content, CountIENG.Content, CountIEL.Content, CountIER.Content, CountIELV.Content, FileName);
        //                }
        //                else
        //                {
        //                    IWrite(CountIEOK.Content, CountIENG.Content, CountIEL.Content, CountIER.Content, null, FileName);
        //                }
        //            }
        //            else
        //            {
        //                IWrite(CountIEOK.Content, CountIENG.Content, CountIEL.Content, null, null, FileName);
        //            }
        //            if (DateTime.Now.Hour == 19)
        //            {
        //                //写入总OK NG 良率
        //                //TotalCountIOK
        //                OperateResult<float> TotalCountIOK = plc1.ReadFloat(a[10]);
        //                //TotalCountING
        //                OperateResult<float> TotalCountING = plc1.ReadFloat(a[11]);
        //                //TotalCountILV
        //                OperateResult<float> TotalCountILV = plc1.ReadFloat(a[12]);

        //                if (FileName.Contains("时间统计"))
        //                {
        //                    OperateResult<float> TotalTimeILV = plc1.ReadFloat(a[16]);

        //                    Log.WriteCSV("合计," + Convert.ToDouble(TotalCountIOK.Content) + "," + Convert.ToDouble(TotalCountING.Content) + "," + Convert.ToDouble(TotalCountILV.Content) + "," + Convert.ToDouble(TotalTimeILV.Content), FileName);
        //                }
        //                else
        //                {
        //                    Log.WriteCSV("合计," + Convert.ToDouble(TotalCountIOK.Content) + "," + Convert.ToDouble(TotalCountING.Content) + "," + Convert.ToDouble(TotalCountILV.Content), FileName);
        //                }


        //            }
        //        }
        //        else
        //        {
        //            //NIGHT
        //            //ok
        //            OperateResult<float[]> CountIEOKN = plc1.ReadFloat(a[5], 12);
        //            //ng
        //            OperateResult<float[]> CountIENGN = plc1.ReadFloat(a[6], 12);
        //            //l
        //            OperateResult<float[]> CountIELN = plc1.ReadFloat(a[7], 12);
        //            //R
        //            OperateResult<float[]> CountIERN = new OperateResult<float[]>();
        //            if (!string.IsNullOrEmpty(a[8]))
        //            {
        //                CountIERN = plc1.ReadFloat(a[8], 12);
        //                //LV
        //                OperateResult<float[]> CountIELVN = new OperateResult<float[]>();
        //                if (!string.IsNullOrEmpty(a[9]))
        //                {
        //                    CountIELVN = plc1.ReadFloat(a[9], 12);
        //                    IWrite(CountIEOKN.Content, CountIENGN.Content, CountIELN.Content, CountIERN.Content, CountIELVN.Content, FileName);
        //                }
        //                else
        //                {
        //                    IWrite(CountIEOKN.Content, CountIENGN.Content, CountIELN.Content, CountIERN.Content, null, FileName);
        //                }
        //            }
        //            else
        //            {
        //                IWrite(CountIEOKN.Content, CountIENGN.Content, CountIELN.Content, null, null, FileName);
        //            }
        //            if (DateTime.Now.Hour == 7)
        //            {
        //                //TotalCountIOK
        //                OperateResult<float> TotalCountIOKN = plc1.ReadFloat(a[13]);
        //                //TotalCountING
        //                OperateResult<float> TotalCountINGN = plc1.ReadFloat(a[14]);
        //                //TotalCountILV
        //                OperateResult<float> TotalCountILVN = plc1.ReadFloat(a[15]);
        //                if (FileName.Contains("时间统计"))
        //                {
        //                    OperateResult<float> TotalTimeILVN = plc1.ReadFloat(a[17]);
        //                    Log.WriteCSV("合计," + Convert.ToDouble(TotalCountIOKN.Content) + "," + Convert.ToDouble(TotalCountINGN.Content) + "," + Convert.ToDouble(TotalCountILVN.Content) + "," + Convert.ToDouble(TotalTimeILVN.Content), FileName);
        //                }
        //                else
        //                {
        //                    Log.WriteCSV("合计," + Convert.ToDouble(TotalCountIOKN.Content) + "," + Convert.ToDouble(TotalCountINGN.Content) + "," + Convert.ToDouble(TotalCountILVN.Content), FileName);
        //                }
        //            }
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        private short[] convertInt32Toshort(int[] data)
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
        private short convertInt32Toshort(int data)
        {
            float data1;
            short data_short;
            byte[] buf = new byte[4];
            bool ok = ConvertIntToByteArray(data, ref buf);
            data1 = BitConverter.ToSingle(buf, 0);
            data_short = Convert.ToInt16(data1);
            return data_short;
        }

        private int convertInt32ToInt(int data)
        {
            float data1;
            int data_short;
            byte[] buf = new byte[4];
            bool ok = ConvertIntToByteArray(data, ref buf);
            data1 = BitConverter.ToSingle(buf, 0);
            data_short = Convert.ToInt32(data1);
            return data_short;
        }
        private bool ConvertIntToByteArray(int m, ref byte[] arry)
        {
            if (arry == null) { return false; }
            if (arry.Length < 4)
            {
                return false;
            }
            arry[2] = (byte)(m & 0xFF);
            arry[3] = (byte)((m & 0xFF00) >> 8);
            arry[0] = (byte)((m & 0xFF0000) >> 16);
            arry[1] = (byte)((m >> 24) & 0xFF);
            return true;
        }
        /// <summary>
        /// float[] a, float[] b, float[] c, float[] d, float[] e, string FileName = ""
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// <param name="FileName"></param>
        private void IWrite(short[] A, short[] B, short[] C, short[] D, short[] E, string FileName = "")
        {

            //short[] A= convertFloatToShort(a);
            //short[] B = convertFloatToShort(b);
            //short[] C = convertFloatToShort(c);

            int i = 0;
            string time = "";
            switch (DateTime.Now.Hour)
            {
                case 8:

                case 20:
                    i = 0;
                    time = DateTime.Now.Hour == 8 ? "8:00-8:59" : "20:00-20:59";

                    break;
                case 9:
                case 21:
                    i = 2;
                    time = DateTime.Now.Hour == 9 ? "9:00-9:59" : "21:00-21:59";
                    break;
                case 10:
                case 22:
                    i = 4;
                    time = DateTime.Now.Hour == 10 ? "10:00-10:59" : "22:00-22:59";
                    break;
                case 11:
                case 23:
                    i = 6;
                    time = DateTime.Now.Hour == 11 ? "11:00-11:59" : "23:00-23:59";
                    break;
                case 12:
                case 0:
                    i = 8;
                    time = DateTime.Now.Hour == 12 ? "12:00-12:59" : "0:00-0:59";
                    break;
                case 13:
                case 1:
                    i = 10;
                    time = DateTime.Now.Hour == 13 ? "13:00-13:59" : "1:00-1:59";
                    break;
                case 14:
                case 2:
                    i = 12;
                    time = DateTime.Now.Hour == 14 ? "14:00-14:59" : "2:00-2:59";
                    break;
                case 15:
                case 3:
                    i = 14;
                    time = DateTime.Now.Hour == 15 ? "15:00-15:59" : "3:00-3:59";
                    break;
                case 16:
                case 4:
                    i = 16;
                    time = DateTime.Now.Hour == 16 ? "16:00-16:59" : "4:00-4:59";
                    break;
                case 17:
                case 5:
                    i = 18;
                    time = DateTime.Now.Hour == 17 ? "17:00-17:59" : "5:00-5:59";
                    break;
                case 18:
                case 6:
                    i = 20;
                    time = DateTime.Now.Hour == 18 ? "18:00-18:59" : "6:00-6:59";
                    break;
                case 19:
                case 7:
                    i = 22;
                    time = DateTime.Now.Hour == 19 ? "19:00-19:59" : "7:00-7:59";
                    break;
                default:
                    break;
            }
            //d
            if (D != null)
            {
                //short[] D = convertFloatToShort(d);
                //e
                if (E != null)
                {
                    //short[] E = convertFloatToShort(e);
                    Log.WriteCSV(time + "," + A[i] + "," + B[i] + "," + C[i] + "," + D[i] + "," + E[i], FileName);
                }
                else
                {
                    Log.WriteCSV(time + "," + A[i] + "," + B[i] + "," + C[i] + "," + D[i] + ",", FileName);
                }

            }
            else
            {
                Log.WriteCSV(time + "," + A[i] + "," + B[i] + "," + C[i], FileName);
            }


        }
        private void WriteI(string[] a, string FileName = "", InovanceAMTcp plc1 = null)
        {
            //I件 LC 成品 1# 2#焊接
            try
            {
                if (DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 20)
                {
                    //DAY
                    //ok float  ReadFloat OperateResult<float[]> CountIEOK = plc1.ReadFloat(a[0], 12);
                    short[] CountIEOK = convertInt32Toshort(plc1.ReadInt32(a[0], 12).Content);
                    //ng OperateResult<float[]> CountIENG = plc1.ReadFloat(a[1], 12);

                    short[] CountIENG = convertInt32Toshort(plc1.ReadInt32(a[1], 12).Content);
                    //l OperateResult<float[]> CountIEL = plc1.ReadFloat(a[2], 12);
                    short[] CountIEL = convertInt32Toshort(plc1.ReadInt32(a[2], 12).Content);
                    //R OperateResult<float[]> CountIER = new OperateResult<float[]>();

                    // OperateResult<float[]> CountIER = new OperateResult<float[]>();
                    short[] CountIER;
                    if (!string.IsNullOrEmpty(a[3]))
                    {
                        // CountIER = plc1.ReadFloat(a[3], 12);
                        CountIER = convertInt32Toshort(plc1.ReadInt32(a[3], 12).Content);
                        //LV
                        // OperateResult<float[]> CountIELV = new OperateResult<float[]>();
                        short[] CountIELV;
                        if (!string.IsNullOrEmpty(a[4]))
                        {
                            // CountIELV = plc1.ReadFloat(a[4], 12);
                            CountIELV = convertInt32Toshort(plc1.ReadInt32(a[4], 12).Content);
                            //IWrite(CountIEOK.Content, CountIENG.Content, CountIEL.Content, CountIER.Content, CountIELV.Content, FileName);
                            IWrite(CountIEOK, CountIENG, CountIEL, CountIER, CountIELV, FileName);
                        }
                        else
                        {
                            //IWrite(CountIEOK.Content, CountIENG.Content, CountIEL.Content, CountIER.Content, null, FileName);
                            IWrite(CountIEOK, CountIENG, CountIEL, CountIER, null, FileName);
                        }
                    }
                    else
                    {
                        //IWrite(CountIEOK.Content, CountIENG.Content, CountIEL.Content, null, null, FileName);
                        IWrite(CountIEOK, CountIENG, CountIEL, null, null, FileName);
                    }
                    if (DateTime.Now.Hour == 14)
                    {
                        //写入总OK NG 良率
                        //TotalCountIOK
                        //OperateResult<float> TotalCountIOK = plc1.ReadFloat(a[10]);
                        short TotalCountIOK = convertInt32Toshort(plc1.ReadInt32(a[10]).Content);
                        //TotalCountING
                        // OperateResult<float> TotalCountING = plc1.ReadFloat(a[11]);
                        short TotalCountING = convertInt32Toshort(plc1.ReadInt32(a[11]).Content);
                        //TotalCountILV
                        // OperateResult<float> TotalCountILV = plc1.ReadFloat(a[12]);
                        short TotalCountILV = convertInt32Toshort(plc1.ReadInt32(a[12]).Content);
                        if (FileName.Contains("时间统计"))
                        {
                            // OperateResult<float> TotalTimeILV = plc1.ReadFloat(a[16]);
                            short TotalTimeILV = convertInt32Toshort(plc1.ReadInt32(a[16]).Content);
                            Log.WriteCSV("合计," + Convert.ToDouble(TotalCountIOK) + "," + Convert.ToDouble(TotalCountING) + "," + Convert.ToDouble(TotalCountILV) + "," + Convert.ToDouble(TotalTimeILV), FileName);
                        }
                        else
                        {
                            Log.WriteCSV("合计," + Convert.ToDouble(TotalCountIOK) + "," + Convert.ToDouble(TotalCountING) + "," + Convert.ToDouble(TotalCountILV), FileName);
                        }

                    }
                }
                else
                {
                    //NIGHT
                    //ok
                    // OperateResult<float[]> CountIEOKN = plc1.ReadFloat(a[5], 12);
                    short[] CountIEOKN = convertInt32Toshort(plc1.ReadInt32(a[5], 12).Content);
                    //ng
                    //OperateResult<float[]> CountIENGN = plc1.ReadFloat(a[6], 12);
                    short[] CountIENGN = convertInt32Toshort(plc1.ReadInt32(a[6], 12).Content);
                    //l
                    //OperateResult<float[]> CountIELN = plc1.ReadFloat(a[7], 12);
                    short[] CountIELN = convertInt32Toshort(plc1.ReadInt32(a[7], 12).Content);
                    //R
                    // OperateResult<float[]> CountIERN = new OperateResult<float[]>();
                    short[] CountIERN;
                    if (!string.IsNullOrEmpty(a[8]))
                    {
                        //CountIERN = plc1.ReadFloat(a[8], 12);
                        CountIERN = convertInt32Toshort(plc1.ReadInt32(a[8], 12).Content);
                        //LV
                        //OperateResult<float[]> CountIELVN = new OperateResult<float[]>();
                        short[] CountIELVN;
                        if (!string.IsNullOrEmpty(a[9]))
                        {
                            // CountIELVN = plc1.ReadFloat(a[9], 12);
                            CountIELVN = convertInt32Toshort(plc1.ReadInt32(a[9], 12).Content);
                            //IWrite(CountIEOKN.Content, CountIENGN.Content, CountIELN.Content, CountIERN.Content, CountIELVN.Content, FileName);
                            IWrite(CountIEOKN, CountIENGN, CountIELN, CountIERN, CountIELVN, FileName);
                        }
                        else
                        {
                            IWrite(CountIEOKN, CountIENGN, CountIELN, CountIERN, null, FileName);
                        }
                    }
                    else
                    {
                        IWrite(CountIEOKN, CountIENGN, CountIELN, null, null, FileName);
                    }
                    if (DateTime.Now.Hour == 7)
                    {
                        //TotalCountIOK
                        //OperateResult<float> TotalCountIOKN = plc1.ReadFloat(a[13]);
                        short TotalCountIOKN = convertInt32Toshort(plc1.ReadInt32(a[13]).Content);
                        //TotalCountING
                        //OperateResult<float> TotalCountINGN = plc1.ReadFloat(a[14]);
                        short TotalCountINGN = convertInt32Toshort(plc1.ReadInt32(a[14]).Content);
                        //TotalCountILV
                        //OperateResult<float> TotalCountILVN = plc1.ReadFloat(a[15]);
                        short TotalCountILVN = convertInt32Toshort(plc1.ReadInt32(a[15]).Content);
                        if (FileName.Contains("时间统计"))
                        {
                            //OperateResult<float> TotalTimeILVN = plc1.ReadFloat(a[17]);
                            short TotalTimeILVN = convertInt32Toshort(plc1.ReadInt32(a[17]).Content);
                            Log.WriteCSV("合计," + Convert.ToDouble(TotalCountIOKN) + "," + Convert.ToDouble(TotalCountINGN) + "," + Convert.ToDouble(TotalCountILVN) + "," + Convert.ToDouble(TotalTimeILVN), FileName);
                        }
                        else
                        {
                            Log.WriteCSV("合计," + Convert.ToDouble(TotalCountIOKN) + "," + Convert.ToDouble(TotalCountINGN) + "," + Convert.ToDouble(TotalCountILVN), FileName);
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private short[] convertFloatToShort(float[] data)
        {
            short[] data_short = new short[data.Length * 2];
            for (int i = 0; i < data.Length; i++)
            {
                data_short[i * 2] = Convert.ToInt16(data[i]);
            }
            return data_short;
        }

        private Dictionary<string, DateTime> PassLists = new Dictionary<string, DateTime>();

        private void DeletePic(int HansId)
        {
            try
            {
                string path = "E:\\public\\HG" + HansId + "#";

                if (Directory.Exists(path))
                {
                    var OldPic = Directory.GetDirectories(path).Where(t => Directory.GetCreationTime(t) < DateTime.Now.AddDays(-14));
                    if (OldPic.Count() > 0)
                    {
                        foreach (var item in OldPic)
                        {
                            Directory.Delete(item, true);
                        }
                        Log.WriteLog(DateTime.Now + "图片及文件夹清除(14天前):" + OldPic);
                    }
                }
            }
            catch (Exception ex)
            {

                Log.WriteLog("图片清除异常:" + ex.Message);
            }
        }
        public class PassStationModel
        {
            public DateTime Date { set; get; }
            public string Code { set; get; }
        }
        private List<PassStationModel> psLists = new List<PassStationModel>();
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
            ResultModel = pass_Trace(this.txt_SN.Text, this.txt_LeftRail.Text, this.txt_RightRail.Text, this.txt_Fixture.Text, textBox1.Text);
        }
        private ResultModel<FlHelper.Models.ParaInfo[]> ResultModel { set; get; }
        private ResultModel<FlHelper.Models.ParaInfo[]> pass_Trace(string SN, string left_rail, string right_rail, string fixture, string head_id)
        {

            var paraInfos = new FlHelper.Models.ParaInfo[17];
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
                trace_ua.data.insight.results = new Result[182];
                //trace_ua.data.insight.results = new Result[104‬]; new
                //230401
                //string sql = $"select * from HGData where barcode='{SN}'";
                string sql = "SELECT TOP 17 * FROM [ZHH].[dbo].[HGData] ORDER BY DateTime DESC";
                DataTable dt = Sqlserver.ExecuteQuery(sql);
                if (dt.Rows.Count < 17)
                {
                    Log.WriteLog("Trace上传失败:焊接数据不足，现有" + dt.Rows.Count + "条数据");

                    return new ResultModel<FlHelper.Models.ParaInfo[]>() { Result = false, Model = paraInfos };
                }

                for (int i = 0; i < 170; i++)
                {
                    trace_ua.data.insight.results[i] = new Result();

                    trace_ua.data.insight.results[i].result = "pass";


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
                            trace_ua.data.insight.results[i].test = "RM3";

                            break;
                        case 2:
                            trace_ua.data.insight.results[i].test = "LM1";

                            break;
                        case 3:
                            trace_ua.data.insight.results[i].test = "LT";

                            break;
                        case 4:
                            trace_ua.data.insight.results[i].test = "F-LT";

                            break;
                        case 5:
                            trace_ua.data.insight.results[i].test = "LM2";

                            break;
                        case 6:
                            trace_ua.data.insight.results[i].test = "LM3";

                            break;
                        case 7:
                            trace_ua.data.insight.results[i].test = "LB";

                            break;
                        case 8:
                            trace_ua.data.insight.results[i].test = "F-LB";

                            break;
                        case 9:
                            trace_ua.data.insight.results[i].test = "RM1";

                            break;
                        case 10:
                            trace_ua.data.insight.results[i].test = "RT";

                            break;
                        case 11:
                            trace_ua.data.insight.results[i].test = "F-RT";
                            break;
                        case 12:
                            trace_ua.data.insight.results[i].test = "RM4";

                            break;
                        case 13:
                            trace_ua.data.insight.results[i].test = "RB";

                            break;
                        case 14:
                            trace_ua.data.insight.results[i].test = "F-RB";

                            break;
                        case 15:
                            trace_ua.data.insight.results[i].test = "TM";

                            break;
                        case 16:
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
                #endregion

                for (int i = 0; i < 12; i++)
                {
                    trace_ua.data.insight.results[i + 170] = new Result();
                    trace_ua.data.insight.results[i + 170].result = "pass";
                    trace_ua.data.insight.results[i + 170].units = "mm";
                    switch (i)
                    {
                        case 0:
                            trace_ua.data.insight.results[i + 170].test = "LT";
                            trace_ua.data.insight.results[i + 170].sub_test = "TestHeightZ";
                            trace_ua.data.insight.results[i + 170].value = "130.71";
                            break;
                        case 1:
                            trace_ua.data.insight.results[i + 170].test = "LT";
                            trace_ua.data.insight.results[i + 170].sub_test = "Difference";
                            trace_ua.data.insight.results[i + 170].value = "0.039999999999992";
                            break;
                        case 2:
                            trace_ua.data.insight.results[i + 170].test = "LT";
                            trace_ua.data.insight.results[i + 170].sub_test = "ReferenceV";
                            trace_ua.data.insight.results[i + 170].value = "130.75";
                            break;

                        case 3:
                            trace_ua.data.insight.results[i + 170].test = "LB";
                            trace_ua.data.insight.results[i + 170].sub_test = "TestHeightZ";
                            trace_ua.data.insight.results[i + 170].value = "130.71";
                            break;
                        case 4:
                            trace_ua.data.insight.results[i + 170].test = "LB";
                            trace_ua.data.insight.results[i + 170].sub_test = "Difference";
                            trace_ua.data.insight.results[i + 170].value = "-0.0100000000000193";
                            break;
                        case 5:
                            trace_ua.data.insight.results[i + 170].test = "LB";
                            trace_ua.data.insight.results[i + 170].sub_test = "ReferenceV";
                            trace_ua.data.insight.results[i + 170].value = "130.7";
                            break;
                        case 6:
                            trace_ua.data.insight.results[i + 170].test = "RB";
                            trace_ua.data.insight.results[i + 170].sub_test = "TestHeightZ";
                            trace_ua.data.insight.results[i + 170].value = "130.84";
                            break;
                        case 7:
                            trace_ua.data.insight.results[i + 170].test = "RB";
                            trace_ua.data.insight.results[i + 170].sub_test = "Difference";
                            trace_ua.data.insight.results[i + 170].value = "-0.039999999999992";
                            break;
                        case 8:
                            trace_ua.data.insight.results[i + 170].test = "RB";
                            trace_ua.data.insight.results[i + 170].sub_test = "ReferenceV";
                            trace_ua.data.insight.results[i + 170].value = "130.8";
                            break;
                        case 9:
                            trace_ua.data.insight.results[i + 170].test = "RT";
                            trace_ua.data.insight.results[i + 170].sub_test = "TestHeightZ";
                            trace_ua.data.insight.results[i + 170].value = "130.63";
                            break;
                        case 10:
                            trace_ua.data.insight.results[i + 170].test = "RT";
                            trace_ua.data.insight.results[i + 170].sub_test = "Difference";
                            trace_ua.data.insight.results[i + 170].value = "-0.00999999999999091";
                            break;
                        case 11:
                            trace_ua.data.insight.results[i + 170].test = "RT";
                            trace_ua.data.insight.results[i + 170].sub_test = "ReferenceV";
                            trace_ua.data.insight.results[i + 170].value = "130.62";
                            break;


                        default:
                            break;
                    }


                }


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


                for (int i = 0; i < 17; i++)
                {
                    var area = "";
                    switch (i)
                    {
                        case 0:
                            area = "RM2";
                            break;
                        case 1:
                            area = "RM3";
                            break;
                        case 2:
                            area = "LM1";
                            break;
                        case 3:
                            area = "LT";
                            break;
                        case 4:
                            area = "F-LT";
                            break;
                        case 5:
                            area = "LM2";
                            break;
                        case 6:
                            area = "LM3";
                            break;
                        case 7:
                            area = "LB";
                            break;
                        case 8:
                            area = "F-LB";
                            break;
                        case 9:
                            area = "RM1";
                            break;
                        case 10:
                            area = "RT";
                            break;
                        case 11:
                            area = "F-RT";
                            break;
                        case 12:
                            area = "RM4";
                            break;
                        case 13:
                            area = "RB";
                            break;
                        case 14:
                            area = "F-RB";
                            break;
                        case 15:
                            area = "TM";
                            break;
                        case 16:
                            area = "BM";
                            break;
                        default:
                            break;
                    }
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
                RequestAPI3.CallBobcat3(Global.TraceURL, SendTraceLogs, out callResult, out errMsg);
                JArray recvObj = JsonConvert.DeserializeObject<JArray>(callResult);
                Log.WriteLog("Trace接收:" + callResult + "\r\n" + errMsg);

                Trace_id = recvObj[0]["id"].ToString();
                //richTextBox2.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "Trace上传OK!" + "\r\n");
                return new ResultModel<FlHelper.Models.ParaInfo[]>() { Result = true, Model = paraInfos };
            }
            catch (Exception ex)
            {
                Log.WriteLog(head_id + "Trace上传异常" + ex.ToString());


                return new ResultModel<FlHelper.Models.ParaInfo[]>() { Result = false, Model = paraInfos, Message = ex.Message };
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


            var modellist = new Dictionary<string, heightModel>();
            modellist.Add("LT", new heightModel { TestHeightZ = "130.71", Difference = "0.039999999999992", ReferenceV = "130.75" });
            modellist.Add("LB", new heightModel { TestHeightZ = "130.71", Difference = "-0.0100000000000193", ReferenceV = "130.7" });
            modellist.Add("RB", new heightModel { TestHeightZ = "130.84", Difference = "-0.039999999999992", ReferenceV = "130.8" });
            modellist.Add("RT", new heightModel { TestHeightZ = "130.63", Difference = "-0.00999999999999091", ReferenceV = "130.62" });
            heights.Add(this.txt_SN.Text, modellist);

            string callResult = "";
            string errMsg = "";

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
            pass_Stn.recipeInfo.cavity = "0" + (new Random().Next(10) + 1);
            pass_Stn.recipeInfo.judgement = "PASS";
            pass_Stn.recipeInfo.humidity = "63";
            pass_Stn.recipeInfo.temperature = "35";
            pass_Stn.recipeInfo.paraInfo = new ParaInfo[17];

            var heightsValue = new Dictionary<string, heightModel>();
            if (heights.ContainsKey(this.txt_SN.Text))
            {
                heights.TryGetValue(this.txt_SN.Text, out heightsValue);
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
                    barCode = this.txt_SN.Text,
                    barCodeType = Global.barCodeType,
                    clientCode = "",
                    station = Global.station,
                    billNo = "GL-233M-LC-EVT-X-NSF-01",
                    product = "233M",
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
            //PutTupian(Convert.ToInt32(textBox1.Text), this.txt_SN.Text, txt_Fixture.Text);
            #endregion
        }
        SendSfc Sc { set; get; }
        private void PutTupian(int HansId, string sn, string fixture)
        {
            try
            {
                string path = "E:\\public\\HG" + HansId + "#";
                if (Directory.Exists(path))
                {
                    string a = path + "\\" + DateTime.Now.ToString("yyyyMMdd") + sn;
                    string b = a + ".zip";
                    //string c= $"{path}\\{"20230411"}{"ASDSAD"}";
                    //string d = $"{c}.zip";
                    if (Directory.Exists(a))
                    {
                        //ZipHelper.CreateZip(c, d);
                        ZipHelper.ZipDirectory(a, b);
                        string str = Sc.PostFileMessage("http://10.176.46.59:8081/PicUpload/B053F/233M", new List<PostDateClass>() {
                          //requestId_check
                          //20230619-164702.535-000000000000000000M00093730002
                        new PostDateClass() {Prop="requestId",Type=0,Value=requestId_check, },
                        new PostDateClass(){Prop="productName",Type=0,Value="233M", },
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
        private byte[] buffer = new byte[1024 * 1024];
        private void btn_Connect_Click(object sender, EventArgs e)
        {
            //if (!comm.IsOpen)
            //{
            //    comm = new SerialPort(this.cmbPort.Text,115200);
            //    comm.Open();
            //    MessageBox.Show("串口已连接！");
            //    comm.DataReceived += Comm_DataReceived;
            //}
            //else
            //{
            //    MessageBox.Show("串口已经打开！");
            //}
        }

        private void Comm_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(300);
            int i = comm.Read(buffer, 0, comm.ReadBufferSize);

            string result = BitConverter.ToString(buffer, i);
            string result3 = comm.ReadExisting();
            comm.Read(buffer, 0, comm.ReadBufferSize);
            //string res = BitConverter.ToString(buffer);           
            string res = BitConverter.ToString(Encoding.Convert(Encoding.UTF8, Encoding.ASCII, buffer));
            //string result2 = BitConverter.ToString(buffer, i);
            this.Invoke(new Action(() => this.txt_SN.Text = result));
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
            plc2.Write("MW65101", Convert.ToInt16(2));
        }


        private void button2_Click(object sender, EventArgs e)
        {
            plc2.Write("MW65101", Convert.ToInt16(1));
        }



        private void btn_ReadPLC_Click(object sender, EventArgs e)
        {
            OperateResult<short[]> config_sn = plc1.ReadInt16("MW45240", 20);
            string barcode = ToASCII(config_sn.Content).Trim();
            OperateResult<short[]> left_sn = plc1.ReadInt16("MW45200", 20);
            string left_rail = ToASCII(left_sn.Content).Trim();
            OperateResult<short[]> right_sn = plc1.ReadInt16("MW45220", 20);
            string right_rail = ToASCII(right_sn.Content).Trim();
            this.txt_SN.Text = barcode;
            this.txt_LeftRail.Text = left_rail;
            this.txt_RightRail.Text = right_rail;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.txt_SN.Text = "";
            this.txt_LeftRail.Text = "";
            this.txt_RightRail.Text = "";
            this.label9.Text = "";
            this.lbl_result.Text = "";
            this.lbl_result.ForeColor = Color.Black;
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
            updatelabeltext(label9,SN);
            this.lbl_result.Text = "Waiting...";
            lbl_result.ForeColor = Color.Black;
            richTextBox1.Clear();
            AddRichText(richTextBox1,"LC码:"+SN);
            AddRichText(richTextBox1,"左键码:"+leftrail);
            AddRichText(richTextBox1,"右键码:"+rightrail);
            AddRichText(richTextBox1,"治具码:"+fixture);
            AddRichText(richTextBox1,"开始上传...");
            ResultModel bz = Baozhan(SN);
            AddRichText(richTextBox1,bz.Message);
            if (bz.Result == true)
            {
                
                ResultModel slqr = Shangliaojiaoyan(SN,leftrail,rightrail,fixture);
                AddRichText(richTextBox1, slqr.Message);
                if (slqr.Result == true)
                {

                    ResultModel = pass_Trace(SN, leftrail, rightrail, fixture, textBox1.Text);
                    
                    if (ResultModel.Result == true)
                    {

                        bz.Message = "Trace上传OK!";
                        AddRichText(richTextBox1, bz.Message);
                        ResultModel gz=Guozhan(SN, leftrail, rightrail, fixture);
                        AddRichText(richTextBox1, gz.Message);
                        Sfcupload(SN, leftrail, rightrail, fixture);
                        if (gz.Result==true)
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
                updatelabeltext(lbl_result,"Fail",false,true);
            }
            button4.Enabled = true;
            AddRichText(richTextBox1, "上传结束。");
        }

        private ResultModel Baozhan(string SN) {
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

        private ResultModel Shangliaojiaoyan(string SN,string leftrail,string rightrail,string fixture) {
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
                Log.WriteLog("A工位Equipment to  API 上料確認:" + send_check);
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

        private ResultModel Guozhan(string SN, string leftrail, string rightrail, string fixture) {
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
                Log.WriteLog("A工位Equipment to API 過站報告:" + send_Pass);
                RequestAPI2.CallBobcat4(passURL, send_Pass, hashCode_newstn, out callResult, out errMsg);
                Log.WriteLog("A工位 API to Equipment 過站報告:" + callResult+","+errMsg);
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
                Log.WriteLog("A工位 API to Equipment 過站報告异常:" + ex.Message);
                return new ResultModel() { Result = false, Message = "过站失败!" + callResult + "，" + errMsg };
                }            
            }

        private void Sfcupload(string SN, string leftrail, string rightrail, string fixture) {
            try {
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
                        billNo = "GL-233M-LC-EVT-X-NSF-01",
                        product = "233M",
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

        private void AddRichText(RichTextBox richTextBox,string message) {
            richTextBox.AppendText(message+"\r\n");
            Log.WriteLog(message);
        }
        private void updatelabeltext(Label label,string message,bool result=true,bool isuse=false) {
            label.Text = message;
            if (isuse)
            {
                label.ForeColor = result == false ? Color.Red : Color.Green;
            } 
        }
    }


}
