using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 卓汇数据追溯系统
{
   public class OEEHelper
    {
        SQLServer SQL = new SQLServer();
        public DateTime NowTime { set; get; }
        public void Restart() {
            #region 开机记录软件关闭的结束时间

            string start_time = string.Empty;//查找最后一笔状态的开始时间
            DateTime t2 = DateTime.Now;
            Global.start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Global.start_timeB = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Global.start_timeC = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string sel_Endtime = "select EventTime from OEE_DT where ID=(select MAX(ID) from OEE_DT)";
            DataTable d_end = SQL.ExecuteQuery(sel_Endtime);
            if (d_end.Rows.Count > 0)
            {
                start_time = d_end.Rows[0][0].ToString();
            }
            DateTime t1 = Convert.ToDateTime(start_time);
            string ts = (t2 - t1).TotalMinutes.ToString("0.00");

            string up_oee = "update OEE_DT set DateTime='"+t2.ToString("yyyy-MM-dd HH:mm:ss.fff")+"',ErrorCode='10010001',ModuleCode='',RunStatus=6,ErrorInfo='软件关闭',TimeSpan='"+ts+"' where ID=(select MAX(ID) from OEE_DT)";
            SQL.ExecuteUpdate(up_oee);

           Global._homefrm.AppendRichText(start_time + " -> " + t2.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + "软件关闭" + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");
            
            Global.OEE_Code = Global.plc1.ReadInt16("MW62001").Content;
            Global.OEE_CodeB = Global.plc2.ReadInt16("MW62001").Content;
            Global.OEE_CodeC = Global.plc3.ReadInt16("MW62001").Content;
            try
            {

            
            if (Global.ed.ContainsKey(Global.OEE_Code))
            {
                Global._homefrm.AppendRichText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.OEE_Code].errorCode + "," +"1#"+ Global.ed[Global.OEE_Code].errorinfo, "Rtxt_OEE_Detail");
            }
           

            if (Global.ed.ContainsKey(Global.OEE_CodeB))
            {
                Global._homefrm.AppendRichText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.OEE_CodeB].errorCode + "," + "2#"+ Global.ed[Global.OEE_CodeB].errorinfo, "Rtxt_OEE_Detail");
            }
           
            if (Global.ed.ContainsKey(Global.OEE_CodeC))
            {
                Global._homefrm.AppendRichText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.OEE_CodeC].errorCode + "," + "3#" + Global.ed[Global.OEE_CodeC].errorinfo, "Rtxt_OEE_Detail");
            }
          
            }
            catch (Exception ex)
            {

                //Log.WriteLog_ServerInfo("OEE报警不存在:"+Global.OEE_Code+","+Global.OEE_CodeB+","+Global.OEE_CodeC);
            }

            #endregion
        }
        public void OEE_DT()
        {
            while (true)
            {
                short Trg_OEE = Global.plc1.ReadInt16("MW62001").Content;//现在的状态

                short Trg_OEEB = Global.plc2.ReadInt16("MW62001").Content;

                short Trg_OEEC = Global.plc3.ReadInt16("MW62001").Content;
                try
                {

                
                if (Global.ed.ContainsKey(Trg_OEE))
                {
                    if (Trg_OEE != Global.OEE_Code)//状态发生变化，则需要记录，进行状态切换
                    {
                        DateTime t1 = Convert.ToDateTime(Global.start_time);//上笔状态的开始时间
                        Global.start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        DateTime t2 = DateTime.Now;//上笔状态的结束时间
                        string ts = (t2 - t1).TotalMinutes.ToString("0.00");


                        string InsertStr2 = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + t2.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.OEE_Code].errorCode + "'" + "," + "'" + t1.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.OEE_Code].ModuleCode + "'" + "," + "'" + Global.ed[Global.OEE_Code].errorStatus + "'" + "," + "'" + "1#" + Global.ed[Global.OEE_Code].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                        SQL.ExecuteUpdate(InsertStr2);

                        Global._homefrm.AppendRichText(t1.ToString("yyyy-MM-dd HH:mm:ss.fff") + " -> " + t2.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + "1#" + Global.ed[Global.OEE_Code].errorCode + "," + Global.ed[Global.OEE_Code].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");





                        Global.OEE_Code = Trg_OEE;//上笔状态记录结束，将当前状态给OEE_Code

                        Global._homefrm.AppendRichText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.OEE_Code].errorCode + "," + "1#" + Global.ed[Global.OEE_Code].errorinfo, "Rtxt_OEE_Detail");
                    }

                }
               
                if (Global.ed.ContainsKey(Trg_OEEB))
                {

                    if (Trg_OEEB != Global.OEE_CodeB)//状态发生变化，则需要记录，进行状态切换
                    {
                        DateTime t1 = Convert.ToDateTime(Global.start_timeB);//上笔状态的开始时间
                        Global.start_timeB = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        DateTime t2 = DateTime.Now;//上笔状态的结束时间
                        string ts = (t2 - t1).TotalMinutes.ToString("0.00");


                        string InsertStr2 = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + t2.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.OEE_CodeB].errorCode + "'" + "," + "'" + t1.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.OEE_CodeB].ModuleCode + "'" + "," + "'" + Global.ed[Global.OEE_CodeB].errorStatus + "'" + "," + "'" + "2#" + Global.ed[Global.OEE_CodeB].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                        SQL.ExecuteUpdate(InsertStr2);

                        Global._homefrm.AppendRichText(t1.ToString("yyyy-MM-dd HH:mm:ss.fff") + " -> " + t2.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.OEE_CodeB].errorCode + "," + "2#" + Global.ed[Global.OEE_CodeB].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");





                        Global.OEE_CodeB = Trg_OEEB;//上笔状态记录结束，将当前状态给OEE_CodeB

                        Global._homefrm.AppendRichText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.OEE_CodeB].errorCode + "," + "2#" + Global.ed[Global.OEE_CodeB].errorinfo, "Rtxt_OEE_Detail");
                    }

                }
                
                if (Global.ed.ContainsKey(Trg_OEEC))
                {
                    if (Trg_OEEC != Global.OEE_CodeC)//状态发生变化，则需要记录，进行状态切换
                    {
                        DateTime t1 = Convert.ToDateTime(Global.start_timeC);//上笔状态的开始时间
                        Global.start_timeC = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        DateTime t2 = DateTime.Now;//上笔状态的结束时间
                        string ts = (t2 - t1).TotalMinutes.ToString("0.00");


                        string InsertStr2 = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + t2.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.OEE_CodeC].errorCode + "'" + "," + "'" + t1.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.OEE_CodeC].ModuleCode + "'" + "," + "'" + Global.ed[Global.OEE_CodeC].errorStatus + "'" + "," + "'" + "3#" + Global.ed[Global.OEE_CodeC].errorinfo + "'" + "," + "'" + ts + "'" + ")";
                        SQL.ExecuteUpdate(InsertStr2);

                        Global._homefrm.AppendRichText(t1.ToString("yyyy-MM-dd HH:mm:ss.fff") + " -> " + t2.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.OEE_CodeC].errorCode + "," + "3#" + Global.ed[Global.OEE_CodeC].errorinfo + "," + ts + "分钟", "Rtxt_OEE_TimeSpan");





                        Global.OEE_CodeC = Trg_OEEC;//上笔状态记录结束，将当前状态给OEE_CodeC

                        Global._homefrm.AppendRichText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Global.ed[Global.OEE_CodeC].errorCode + "," + "3#" + Global.ed[Global.OEE_CodeC].errorinfo, "Rtxt_OEE_Detail");
                    }
                }
                //else
                //{
                //    Log.WriteLog_ServerInfo("3#OEE报警不存在:" + Trg_OEEC);
                //}
                Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    //Log.WriteLog_ServerInfo("OEE报警不存在:" + Global.OEE_Code + "," + Global.OEE_CodeB + "," + Global.OEE_CodeC);
                }
            }
        }

        public void CloseOEE() {
            //结束当前状态，进行OEE记录
            DateTime t1 = Convert.ToDateTime(Global.start_time);//上笔状态的开始时间
            Global.start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            DateTime t1B = Convert.ToDateTime(Global.start_timeB);//上笔状态的开始时间
            Global.start_timeB = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            DateTime t1C = Convert.ToDateTime(Global.start_timeC);//上笔状态的开始时间
            Global.start_timeC = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            DateTime t2 = DateTime.Now;//上笔状态的结束时间
            string ts = (t2 - t1).TotalMinutes.ToString("0.00");
            string tsB = (t2 - t1B).TotalMinutes.ToString("0.00");
            string tsC = (t2 - t1C).TotalMinutes.ToString("0.00");
            string InsertStr2 = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + t2.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.OEE_Code].errorCode + "'" + "," + "'" + t1.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.OEE_Code].ModuleCode + "'" + "," + "'" + Global.ed[Global.OEE_Code].errorStatus + "'" + "," + "'" + "1#" + Global.ed[Global.OEE_Code].errorinfo + "'" + "," + "'" + ts + "'" + ")";
            SQL.ExecuteUpdate(InsertStr2);
            string InsertStr2B = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + t2.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.OEE_CodeB].errorCode + "'" + "," + "'" + t1B.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.OEE_CodeB].ModuleCode + "'" + "," + "'" + Global.ed[Global.OEE_CodeB].errorStatus + "'" + "," + "'" + "2#" + Global.ed[Global.OEE_CodeB].errorinfo + "'" + "," + "'" + ts + "'" + ")";
            SQL.ExecuteUpdate(InsertStr2B);
            string InsertStr2C = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[ModuleCode],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + t2.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.OEE_CodeC].errorCode + "'" + "," + "'" + t1C.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + Global.ed[Global.OEE_CodeC].ModuleCode + "'" + "," + "'" + Global.ed[Global.OEE_CodeC].errorStatus + "'" + "," + "'" + "3#" + Global.ed[Global.OEE_CodeC].errorinfo + "'" + "," + "'" + ts + "'" + ")";
            SQL.ExecuteUpdate(InsertStr2C);


            //记录软件关闭的开始时间
            string InsertStr_close = "insert into OEE_DT([DateTime],[ErrorCode],[EventTime],[RunStatus],[ErrorInfo],[TimeSpan])" + " " + "values(" + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + "10010001" + "'" + "," + "'" + t2.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "," + "'" + "6" + "'" + "," + "'" + "软件关闭" + "'" + "," + "'" + "0.00" + "'" + ")";
            SQL.ExecuteUpdate(InsertStr_close);
        }

        public void OEENG(string barcode, string leftRail, string RightRail, string msg, string Station)
        {
            if (NowTime.ToString("yyyyMMdd")!=DateTime.Now.ToString("yyyyMMdd"))
            {
                Global._homefrm.ClearNG();
            }
            string message = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + barcode + "," + leftRail + "," + RightRail + "," + Station + ":" + msg;
            Global._homefrm.AppendRichText(message, "richTextBoxNG");
            NowTime = DateTime.Now;
            Log.WriteCSV(message, AppDomain.CurrentDomain.BaseDirectory + "\\NG明细\\");
        }
    }
}
