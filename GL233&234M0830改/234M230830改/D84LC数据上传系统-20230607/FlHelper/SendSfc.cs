using FlHelper.Helpers;
using FlHelper.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace FlHelper
{
   public class SendSfc
    {
        private static object lock_pass = new object();

        public Action<string, string> UpdateUI { set; get; }
        public bool PassStation(string url, SfcStn sfcStn, string hashcode) {
            lock (lock_pass)
            {
                try
                {
                    //UpdateUI("过站产品码：" + sfcStn.productInfo.barCode, "Rtxt_Send_Pass");
                    //UpdateUI("过站左键码：" + left_rail, "Rtxt_Send_Pass");
                    //UpdateUI("过站右键码：" + right_rail, "Rtxt_Send_Pass");
                    //UpdateUI("过站治具码：" + fixture, "Rtxt_Send_Pass");

                    string callResult = "";
                    string errMsg = "";

                    JsonSerializerSettings jsetting = new JsonSerializerSettings();
                    jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值

                    string send_Pass = (JsonConvert.SerializeObject(sfcStn, Formatting.None, jsetting)).Replace("a1st","1st").Replace("a2nd", "2nd").Replace("a3rd", "3rd").Replace("a4th", "4th").Replace("a5th", "5th").Replace("a6th", "6th");
                    Log.WriteLog("A工位 Equipment to API 参数上抛報告("+url+"):" + send_Pass);
                    UpdateUI(send_Pass, "Rtxt_Send_Pass");

                    RequestAPI3.CallBobcat2(url, send_Pass, hashcode, out callResult, out errMsg);
                    Log.WriteLog("A工位 API to Equipment 参数上抛報告:" + callResult);
                    UpdateUI(callResult, "Rtxt_Rec_Pass");
                  
                    //JObject recvObj = JsonConvert.DeserializeObject<JObject>(callResult);
                    //recvObj["rc"].ToString() == "000"
                    if (callResult.Contains("OK"))
                    {
                        UpdateUI("参数上抛OK！", "Rtxt_Rec_Pass");
                        //lstn_info.Remove(SN);
                        return true;
                    }
                    else
                    {
                       UpdateUI("参数上抛NG！", "Rtxt_Rec_Pass");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLog("参数上抛异常" + ex.ToString());
                    return false;
                }
            }

           
        }

        public string PostFileMessage(string strUrl, List<PostDateClass> postParaList) {
            Log.WriteLog("图片上传记录:"+JsonConvert.SerializeObject(postParaList));
            return RequestAPI3.PostFileMessage(strUrl,postParaList);
        }

        public void DeleteAll(string path, int OutDays = 14)
        {
            if (Directory.Exists(path))
            {
                var OldPic = Directory.GetDirectories(path).Where(t => Directory.GetCreationTime(t) < DateTime.Now.AddDays(-OutDays));

                if (OldPic.Count() > 0)
                {
                    Log.WriteLog(DateTime.Now + "," + path + "文件夹下子文件清除(" + OutDays + "天前)计数:" + OldPic.Count());
                    foreach (var item in OldPic)
                    {
                        Directory.Delete(item, true);
                    }
                }
                var OldPicFile = Directory.GetFiles(path).Where(t => File.GetCreationTime(t) < DateTime.Now.AddDays(-OutDays));

                if (OldPicFile.Count() > 0)
                {
                    Log.WriteLog(DateTime.Now + "," + path + "文件清除(" + OutDays + "天前)计数:" + OldPic.Count());
                    foreach (var item in OldPicFile)
                    {
                        File.Delete(item);
                    }
                }
            }
        }
    }
}
