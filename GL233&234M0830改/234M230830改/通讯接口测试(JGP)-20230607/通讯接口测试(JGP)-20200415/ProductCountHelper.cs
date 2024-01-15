using System;
using System.Text;
using System.Threading;
using System.IO;

namespace 通讯接口测试_JGP__20200415
{
    //public class ProductCountHelper
    //{
    //   /// <summary>
    //   /// ProductCountHelper ph = new ProductCountHelper();
    //   ///     ThreadPool.QueueUserWorkItem(ph.ProductTotal);
    //   /// </summary>
    //   private static object _lock = new object();
    //    public void ProductTotal(object obj)
    //    {
    //        // "MW30400", "MW30430", "MW30460", "MW30490", "", "MW31400", "MW31430", "MW31460", "MW31490", "", "MW30110", "MW30112", "MW30114", "MW31110", "MW31112", "MW31114", "MW30116", "MW31116"
    //        //short[] CountIEOK = convertInt32Toshort(Global.plc1.ReadInt32("MW31400", 12).Content);
    //        //int TotalCountIOK = 0;

    //        //for (int i = 0; i < 12; i++)
    //        //{
    //        //    TotalCountIOK += CountIEOK[i * 2];
    //        //}
    //        //Log.WriteLog(TotalCountIOK.ToString());
    //        //int a = convertInt32ToInt(Global.plc1.ReadInt32("MW31110").Content);
    //        //int b = convertInt32ToInt(Global.plc1.ReadInt32("MW31112").Content);
    //        //int c = convertInt32ToInt(Global.plc1.ReadInt32("MW31114").Content);
    //        //int d = convertInt32ToInt(Global.plc1.ReadInt32("MW31116").Content);
    //        //Log.WriteLog("," + b + "," + c + "," + d);
    //        while (true)
    //        {
    //            try
    //            {
    //                if (DateTime.Now.Minute == 59 && DateTime.Now.Second == 57)
    //                {
    //                    ///白班 I件记录 OK NG 左键 右键 良率 OK NG 左键 右键 良率 夜班 总OK 总NG 总良率 总OK 总NG 总良率
    //                    WriteI(new string[] { "MW30200", "MW30800", "MW30230", "MW30600", "MW30260", "MW31200", "MW31800", "MW31230", "MW31600", "MW31260", "MW30102", "MW30108", "MW30106", "MW31102", "MW31108", "MW31106" }, "I件产能统计", Global.plc1);
    //                    ///LC记录 OK Trace 扫码 良率 OK Trace 扫码 良率 总OK 总NG 总良率 总OK 总NG 总良率
    //                    WriteI(new string[] { "MW32200", "MW32230", "MW32600", "MW32260","",  "MW33200", "MW33230", "MW33600", "MW33260","",
    //        "MW32102", "MW32104", "MW32106", "MW33102", "MW33104", "MW33106"}, "LC产能统计", Global.plc1);
    //                    ///1# OK Trace 焊接 良率 无 OK Trace 焊接 良率 无 总OK 总NG 总良率 总OK 总NG 总良率
    //                    WriteI(new string[] { "MW30200", "MW30230", "MW31000", "MW30260", "", "MW31200", "MW31230", "MW32000", "MW31260", "",
    //        "MW30102", "MW30104", "MW30106", "MW31102", "MW31104", "MW31106"}, "焊接1#产能统计", Global.plc2);
    //                    ///2# OK Trace 焊接 良率 无 OK Trace 焊接 良率 无 总OK 总NG 总良率 总OK 总NG 总良率
    //                    WriteI(new string[] { "MW30200", "MW30230", "MW31000", "MW30260", "", "MW31200", "MW31230", "MW32000", "MW31260", "",
    //        "MW30102", "MW30104", "MW30106", "MW31102", "MW31104", "MW31106"}, "焊接2#产能统计", Global.plc3);
    //                    ///成品 OK NG 良率 无 无 OK NG 良率 无 无 总OK 总NG 总良率 总OK 总NG 总良率
    //                    WriteI(new string[] { "MW34200", "MW34230", "MW34260", "", "", "MW35200", "MW35230", "MW35260", "", "",
    //        "MW34102", "MW34104", "MW34106", "MW35102", "MW35104", "MW35106"}, "下料产能统计", Global.plc1);
    //                    ///
    //                    ///白班 I件时间记录 运行 报警 待机 稼动率 无 夜 运行 报警 待机 稼动率 无 白总稼动率 夜总稼动率
    //                    WriteI(new string[] { "MW30400", "MW30430", "MW30460", "MW30490", "", "MW31400", "MW31430", "MW31460", "MW31490", "", "MW30110", "MW30112", "MW30114", "MW31110", "MW31112", "MW31114", "MW30116", "MW31116" }, "I件时间统计", Global.plc1);

    //                    ///白班 LC时间记录 运行 报警 待机 稼动率 无 夜 运行 报警 待机 稼动率 无
    //                    WriteI(new string[] { "MW32400", "MW32430", "MW32460", "MW32490", "", "MW33400", "MW33430", "MW33460", "MW33490", "", "MW32110", "MW32112", "MW32114", "MW33110", "MW33112", "MW33114", "MW32116", "MW33116" }, "LC时间统计", Global.plc1);

    //                    ///白班 焊接1# 运行 报警 待机 稼动率 无 夜 运行 报警 待机 稼动率 无
    //                    WriteI(new string[] { "MW30400", "MW30430", "MW30460", "MW30490", "", "MW31400", "MW31430", "MW31460", "MW31490", "", "MW30110", "MW30112", "MW30114", "MW31110", "MW31112", "MW31114", "MW30116", "MW31116" }, "焊接1#时间统计", Global.plc2);
    //                    ///白班 焊接2# 运行 报警 待机 稼动率 无 夜 运行 报警 待机 稼动率 无
    //                    WriteI(new string[] { "MW30400", "MW30430", "MW30460", "MW30490", "", "MW31400", "MW31430", "MW31460", "MW31490", "", "MW30110", "MW30112", "MW30114", "MW31110", "MW31112", "MW31114", "MW30116", "MW31116" }, "焊接2#时间统计", Global.plc3);
    //                    ///成品 运行 报警 待机 稼动率 无 夜 运行 报警 待机 稼动率 无
    //                    WriteI(new string[] { "MW34400", "MW34430", "MW34460", "MW34490", "", "MW35400", "MW35430", "MW35460", "MW35490", "", "MW34110", "MW34112", "MW34114", "MW35110", "MW35112", "MW35114", "MW34116", "MW35116" }, "下料时间统计", Global.plc1);
    //                }
    //                Thread.Sleep(1000);
    //            }
    //            catch (Exception ex)
    //            {
    //                Log.WriteLog("产能存储异常:"+ex.Message); ;
    //            }
    //        }
    //    }
    //    /// <summary>
    //    /// float[] a, float[] b, float[] c, float[] d, float[] e, string FileName = ""
    //    /// </summary>
    //    /// <param name="a"></param>
    //    /// <param name="b"></param>
    //    /// <param name="c"></param>
    //    /// <param name="d"></param>
    //    /// <param name="e"></param>
    //    /// <param name="FileName"></param>
    //    private void IWrite(short[] A, short[] B, short[] C, short[] D, short[] E, string FileName = "")
    //    {

    //       //short[] A= convertFloatToShort(a);
    //       //short[] B = convertFloatToShort(b);
    //       //short[] C = convertFloatToShort(c);
          
    //        int i = 0;
    //        string time = "";
    //        switch (DateTime.Now.Hour)
    //        {
    //            case 8:

    //            case 20:
    //                i = 0;
    //                time = DateTime.Now.Hour == 8 ? "8:00-8:59" : "20:00-20:59";

    //                break;
    //            case 9:
    //            case 21:
    //                i = 2;
    //                time = DateTime.Now.Hour == 9 ? "9:00-9:59" : "21:00-21:59";
    //                break;
    //            case 10:
    //            case 22:
    //                i = 4;
    //                time = DateTime.Now.Hour == 10 ? "10:00-10:59" : "22:00-22:59";
    //                break;
    //            case 11:
    //            case 23:
    //                i = 6;
    //                time = DateTime.Now.Hour == 11 ? "11:00-11:59" : "23:00-23:59";
    //                break;
    //            case 12:
    //            case 0:
    //                i = 8;
    //                time = DateTime.Now.Hour == 12 ? "12:00-12:59" : "0:00-0:59";
    //                break;
    //            case 13:
    //            case 1:
    //                i = 10;
    //                time = DateTime.Now.Hour == 13 ? "13:00-13:59" : "1:00-1:59";
    //                break;
    //            case 14:
    //            case 2:
    //                i = 12;
    //                time = DateTime.Now.Hour == 14 ? "14:00-14:59" : "2:00-2:59";
    //                break;
    //            case 15:
    //            case 3:
    //                i = 14;
    //                time = DateTime.Now.Hour == 15 ? "15:00-15:59" : "3:00-3:59";
    //                break;
    //            case 16:
    //            case 4:
    //                i = 16;
    //                time = DateTime.Now.Hour == 16 ? "16:00-16:59" : "4:00-4:59";
    //                break;
    //            case 17:
    //            case 5:
    //                i = 18;
    //                time = DateTime.Now.Hour == 17 ? "17:00-17:59" : "5:00-5:59";
    //                break;
    //            case 18:
    //            case 6:
    //                i = 20;
    //                time = DateTime.Now.Hour == 18 ? "18:00-18:59" : "6:00-6:59";
    //                break;
    //            case 19:
    //            case 7:
    //                i = 22;
    //                time = DateTime.Now.Hour == 19 ? "19:00-19:59" : "7:00-7:59";
    //                break;
    //            default:
    //                break;
    //        }
    //        //d
    //        if (D != null)
    //        {
    //            //short[] D = convertFloatToShort(d);
    //            //e
    //            if (E!= null)
    //            {
    //                //short[] E = convertFloatToShort(e);
    //                WriteCSV(time + "," + A[i] + "," + B[i] + "," + C[i] + "," + D[i] + "," + E[i], FileName);
    //            }
    //            else
    //            {
    //                WriteCSV(time + "," + A[i] + "," + B[i] + "," + C[i] + "," + D[i] + ",", FileName);
    //            }

    //        }
    //        else
    //        {
    //            WriteCSV(time + "," + A[i] + "," + B[i] + "," + C[i], FileName);
    //        }


    //    }
    //    private void WriteI(string[] a, string FileName = "", InovanceAMTcp plc1 = null)
    //    {
    //        //I件 LC 成品 1# 2#焊接
    //        try
    //        {
    //            if (DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 20)
    //            {
    //                //DAY
    //                //ok float  ReadFloat OperateResult<float[]> CountIEOK = plc1.ReadFloat(a[0], 12);
    //                short[] CountIEOK =convertInt32Toshort(plc1.ReadInt32(a[0], 12).Content);
    //                //ng OperateResult<float[]> CountIENG = plc1.ReadFloat(a[1], 12);

    //                short[] CountIENG = convertInt32Toshort(plc1.ReadInt32(a[1], 12).Content);
    //                //l OperateResult<float[]> CountIEL = plc1.ReadFloat(a[2], 12);
    //                short[] CountIEL = convertInt32Toshort(plc1.ReadInt32(a[2], 12).Content);
    //                //R OperateResult<float[]> CountIER = new OperateResult<float[]>();
                   
    //                // OperateResult<float[]> CountIER = new OperateResult<float[]>();
    //               short[] CountIER;
    //                if (!string.IsNullOrEmpty(a[3]))
    //                {
    //                    // CountIER = plc1.ReadFloat(a[3], 12);
    //                   CountIER = convertInt32Toshort(plc1.ReadInt32(a[3], 12).Content);
    //                    //LV
    //                    // OperateResult<float[]> CountIELV = new OperateResult<float[]>();
    //                    short[] CountIELV;
    //                    if (!string.IsNullOrEmpty(a[4]))
    //                    {
    //                        // CountIELV = plc1.ReadFloat(a[4], 12);
    //                        CountIELV = convertInt32Toshort(plc1.ReadInt32(a[4], 12).Content);
    //                        //IWrite(CountIEOK.Content, CountIENG.Content, CountIEL.Content, CountIER.Content, CountIELV.Content, FileName);
    //                        IWrite(CountIEOK, CountIENG, CountIEL, CountIER, CountIELV, FileName);
    //                    }
    //                    else
    //                    {
    //                        //IWrite(CountIEOK.Content, CountIENG.Content, CountIEL.Content, CountIER.Content, null, FileName);
    //                        IWrite(CountIEOK, CountIENG, CountIEL, CountIER, null, FileName);
    //                    }
    //                }
    //                else
    //                {
    //                    //IWrite(CountIEOK.Content, CountIENG.Content, CountIEL.Content, null, null, FileName);
    //                    IWrite(CountIEOK, CountIENG, CountIEL, null, null, FileName);
    //                }
    //                if (DateTime.Now.Hour==19)
    //                {
    //                    //写入总OK NG 良率
    //                    //TotalCountIOK
    //                    //OperateResult<float> TotalCountIOK = plc1.ReadFloat(a[10]);
    //                    // short TotalCountIOK = convertInt32Toshort(plc1.ReadInt32(a[10]).Content);
    //                    int TotalCountIOK = 0;
    //                    try
    //                    {
    //                        for (int i = 0; i < 12; i++)
    //                        {
    //                            TotalCountIOK += CountIEOK[i * 2];
    //                        }
    //                    }
    //                    catch (Exception ex)
    //                    {
                           
    //                        Log.WriteLog("TotalCountIOK异常");
    //                    }

    //                    //short TotalCountIOK = 0;


    //                    //TotalCountING
    //                    // OperateResult<float> TotalCountING = plc1.ReadFloat(a[11]);
    //                    short TotalCountING = convertInt32Toshort(plc1.ReadInt32(a[11]).Content);
    //                    //short TotalCountING = 0;
    //                    //for (int i = 0; i < 12; i++)
    //                    //{
    //                    //    TotalCountING += CountIENG[i * 2];
    //                    //}
    //                    //TotalCountILV
    //                    // OperateResult<float> TotalCountILV = plc1.ReadFloat(a[12]);
    //                     short TotalCountILV = convertInt32Toshort(plc1.ReadInt32(a[12]).Content);
                        
    //                    if (FileName.Contains("时间统计"))
    //                    {
    //                        // OperateResult<float> TotalTimeILV = plc1.ReadFloat(a[16]);
    //                         short TotalTimeILV = convertInt32Toshort(plc1.ReadInt32(a[16]).Content);
    //                        //int TotalTimeILV = 0;
    //                        //if (TotalCountIOK+TotalCountING+ TotalCountILV != 0)
    //                        //{
    //                        //    TotalTimeILV = TotalCountIOK*100 / (TotalCountIOK + TotalCountING+TotalCountILV);
    //                        //}
                           
    //                        WriteCSV("合计," + Convert.ToDouble(TotalCountIOK) + "," + Convert.ToDouble(TotalCountING) + "," + Convert.ToDouble(TotalCountILV) + "," + Convert.ToDouble(TotalTimeILV), FileName);
    //                    }
    //                    else
    //                    {
    //                        WriteCSV("合计," + Convert.ToDouble(TotalCountIOK) + "," + Convert.ToDouble(TotalCountING) + "," + Convert.ToDouble(TotalCountILV), FileName);
    //                    }
                        
    //                }
    //            }
    //            else
    //            {
    //                //NIGHT
    //                //ok
    //               // OperateResult<float[]> CountIEOKN = plc1.ReadFloat(a[5], 12);
    //                short[] CountIEOKN = convertInt32Toshort(plc1.ReadInt32(a[5], 12).Content);
    //                //ng
    //                //OperateResult<float[]> CountIENGN = plc1.ReadFloat(a[6], 12);
    //                short[] CountIENGN = convertInt32Toshort(plc1.ReadInt32(a[6], 12).Content);
    //                //l
    //                //OperateResult<float[]> CountIELN = plc1.ReadFloat(a[7], 12);
    //                short[] CountIELN = convertInt32Toshort(plc1.ReadInt32(a[7], 12).Content);
    //                //R
    //                // OperateResult<float[]> CountIERN = new OperateResult<float[]>();
    //                short[] CountIERN;
    //                if (!string.IsNullOrEmpty(a[8]))
    //                {
    //                    //CountIERN = plc1.ReadFloat(a[8], 12);
    //                    CountIERN= convertInt32Toshort(plc1.ReadInt32(a[8], 12).Content);
    //                    //LV
    //                    //OperateResult<float[]> CountIELVN = new OperateResult<float[]>();
    //                    short[] CountIELVN;
    //                    if (!string.IsNullOrEmpty(a[9]))
    //                    {
    //                        // CountIELVN = plc1.ReadFloat(a[9], 12);
    //                        CountIELVN = convertInt32Toshort(plc1.ReadInt32(a[9], 12).Content);
    //                        //IWrite(CountIEOKN.Content, CountIENGN.Content, CountIELN.Content, CountIERN.Content, CountIELVN.Content, FileName);
    //                        IWrite(CountIEOKN, CountIENGN, CountIELN, CountIERN, CountIELVN, FileName);
    //                    }
    //                    else
    //                    {
    //                        IWrite(CountIEOKN, CountIENGN, CountIELN, CountIERN, null, FileName);
    //                    }
    //                }
    //                else
    //                {
    //                    IWrite(CountIEOKN, CountIENGN, CountIELN, null, null, FileName);
    //                }
    //                if (DateTime.Now.Hour == 7)
    //                {
    //                    //TotalCountIOK
    //                    //OperateResult<float> TotalCountIOKN = plc1.ReadFloat(a[13]);
    //                   // short TotalCountIOKN = convertInt32Toshort(plc1.ReadInt32(a[13]).Content);
    //                    int TotalCountIOKN = 0;
    //                    for (int i = 0; i < 12; i++)
    //                    {
    //                        TotalCountIOKN += CountIEOKN[i * 2];
    //                    }
    //                    //TotalCountING
    //                    //OperateResult<float> TotalCountINGN = plc1.ReadFloat(a[14]);
    //                    short TotalCountINGN = convertInt32Toshort(plc1.ReadInt32(a[14]).Content);
    //                    //short TotalCountINGN = 0;
    //                    //for (int i = 0; i < 12; i++)
    //                    //{
    //                    //    TotalCountINGN += CountIENGN[i * 2];
    //                    //}
    //                    //TotalCountILV
    //                    //OperateResult<float> TotalCountILVN = plc1.ReadFloat(a[15]);
    //                    short TotalCountILVN = convertInt32Toshort(plc1.ReadInt32(a[15]).Content);
    //                    if (FileName.Contains("时间统计"))
    //                    {
    //                        //OperateResult<float> TotalTimeILVN = plc1.ReadFloat(a[17]);
    //                        short TotalTimeILVN = convertInt32Toshort(plc1.ReadInt32(a[17]).Content);
    //                        //int TotalTimeILVN = 0;
    //                        //if (TotalCountIOKN+TotalCountINGN+TotalCountILVN!=0)
    //                        //{
    //                        //    TotalTimeILVN = TotalCountIOKN * 100 / (TotalCountIOKN + TotalCountINGN + TotalCountILVN);
    //                        //}
    //                        WriteCSV("合计," + Convert.ToDouble(TotalCountIOKN) + "," + Convert.ToDouble(TotalCountINGN) + "," + Convert.ToDouble(TotalCountILVN) + "," + Convert.ToDouble(TotalTimeILVN), FileName);
    //                    }
    //                    else
    //                    {
    //                        WriteCSV("合计," + Convert.ToDouble(TotalCountIOKN) + "," + Convert.ToDouble(TotalCountINGN) + "," + Convert.ToDouble(TotalCountILVN), FileName);
    //                    }
    //                }
    //            }



    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    private void WriteCSV(string text, string FileName)
    //    {
    //        System.IO.StreamWriter sw = null;
    //        string time = DateTime.Now.ToString("yyyyMMdd");
            
    //        ///夜班8小时也计入上一天产能记录表 当天 8-23 + 第二天0-7
    //        if (DateTime.Now.Hour < 8)
    //        {
    //            time = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
               
    //        }
    //       string RootPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\产能与时间统计\\" + time + "\\";
    //        if (!Directory.Exists(RootPath))
    //        {
    //            Directory.CreateDirectory(RootPath);
    //        }
    //        string fileFullFileName = RootPath +"234M"+ time+FileName + ".csv";
    //        if (!File.Exists(fileFullFileName))
    //        {
    //            using (sw = new StreamWriter(fileFullFileName, true, Encoding.Default))
    //            {
    //                string str = "";


    //                if (FileName.Contains("I件产能统计"))
    //                {
    //                    str = "时间段,OK组,NG组,左键,右键,良率";
    //                }
    //                if (FileName.Contains("下料产能统计"))
    //                {
    //                    str = "时间段,OK组,NG组,良率";
    //                }
    //                if (FileName.Contains("#产能统计"))
    //                {
    //                    str = "时间段,OK组,Trace,焊接,良率";
    //                }
    //                if (FileName.Contains("LC产能统计"))
    //                {
    //                    str = "时间段,OK组,Trace,扫码,良率";
    //                }
    //                if (FileName.Contains("时间统计"))
    //                {
    //                    str = "时间段,运行,报警,待机,稼动率";
    //                }
    //                //sw.WriteLine(time + FileName);
    //                sw.WriteLine(str);
    //            }
    //        }
    //        lock (_lock)
    //        {
    //            try
    //            {
    //                using (sw = new StreamWriter(fileFullFileName, true, Encoding.Default))
    //                {
    //                    sw.WriteLine(text);
    //                }
    //                sw.Close();
    //                sw.Dispose();
    //            }
    //            catch
    //            {

    //            }
    //        }
    //    }

    //    private short[] convertFloatToShort(float[] data)
    //    {
    //        short[] data_short = new short[data.Length * 2];
    //        for (int i = 0; i < data.Length; i++)
    //        {
    //            data_short[i * 2] = Convert.ToInt16(data[i]);
    //        }
    //        return data_short;
    //    }

    //    private short[] convertInt32Toshort(int[] data) {
    //        float[] data1 = new float[data.Length];
    //        short[] data_short = new short[data.Length * 2];
    //        for (int i = 0; i < data.Length; i++)
    //        {
    //            byte[] buf = new byte[4];
    //            bool ok = ConvertIntToByteArray(data[i],ref buf);
    //            data1[i] = BitConverter.ToSingle(buf,0);
    //            data_short[i * 2] = Convert.ToInt16(data1[i]);
    //        }
    //        return data_short;
    //    }
    //    private short convertInt32Toshort(int data)
    //    {
    //        float data1;
    //        short data_short;
    //            byte[] buf = new byte[4];
    //            bool ok = ConvertIntToByteArray(data, ref buf);
    //            data1 = BitConverter.ToSingle(buf, 0);
    //            data_short = Convert.ToInt16(data1);
    //        return data_short;
    //    }

    //    private int convertInt32ToInt(int data)
    //    {
    //        float data1;
    //        int data_short;
    //        byte[] buf = new byte[4];
    //        bool ok = ConvertIntToByteArray(data, ref buf);
    //        data1 = BitConverter.ToSingle(buf, 0);
    //        data_short = Convert.ToInt32(data1);
    //        return data_short;
    //    }
    //    private bool ConvertIntToByteArray(int m, ref byte[] arry)
    //    {
    //        if (arry == null) { return false; }
    //        if (arry.Length<4)
    //        {
    //            return false;
    //        }
    //        arry[2] = (byte)(m & 0xFF);
    //        arry[3] = (byte)((m & 0xFF00)>>8);
    //        arry[0] = (byte)((m & 0xFF0000) >> 16);
    //        arry[1] = (byte)((m >> 24) & 0xFF);
    //        return true;
    //    }
    //}
}
