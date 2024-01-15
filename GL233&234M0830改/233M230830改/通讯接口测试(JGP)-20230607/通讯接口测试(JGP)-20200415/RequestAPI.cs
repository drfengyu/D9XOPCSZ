using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace 通讯接口测试_JGP__20200415
{
    public class RequestAPI
    {
        public static string strReturnURL = "";
        public static object _lock = new object();
        public static bool Request(string strURL1, string strURL2, string strIP, string strMac, string dSn, string code, int station, string data, out string ReturnMsg)
        {
            string strReturn = "";
            string errMsg = "";
            bool bRst = false;
            if (string.IsNullOrWhiteSpace(strReturnURL))
            {
                //bRst = RequireConnection(strURL1, strURL2, strIP, strMac, dSn, code, out strReturn, out errMsg);
                //strReturnURL = strReturn;
                //if (!bRst)
                //{
                //    ReturnMsg = errMsg;
                //    return false;
                //}
                //strReturnURL = "http://cnctug0webapi01";//临时使用
            }

            //var str = strReturnURL.Split(',');
            var bRst1 = SetData(strURL1, strURL2, dSn, code, data, station, out strReturn, out errMsg);
            if (!bRst1)
            {
                strReturnURL = "";
                ReturnMsg = errMsg;
                return false;
            }
            if (strReturn.Substring(0, 1) == "W")
            {
                //var bRst2 = RequireConnection(strURL1, strURL2, strIP, strMac, dSn, code, out strReturn, out errMsg);
                //if (!bRst2)
                //{
                //    ReturnMsg = errMsg;
                //    return false;
                //}

                var str2 = strReturn.Split(',');
                var bRstr = SetData(strURL1, strURL2, dSn, code, data, station, out strReturn, out errMsg);
                if (!bRstr)
                {
                    ReturnMsg = errMsg;
                    return false;
                }
            }
            ReturnMsg = strReturn;
            return true;
        }
        /// <summary>
        /// 请求链接
        /// </summary>
        /// <param name="strURL1">服务器URL1</param>
        /// <param name="strURL2">服务器URL2</param>
        /// <param name="strIP">本地IP</param>
        /// <param name="strMac">本地Mac</param>
        /// <param name="dSn">设备注册码</param>
        /// <param name="code">设备授权码</param>
        /// <param name="strReturnCode">返回结果</param>
        /// <param name="strMessage">返回信息</param>
        /// <returns></returns>
        public static bool RequireConnection(string strURL1, string strURL2, string strIP, string strMac, string dSn, string code, out string strReturnCode, out string strMessage)
        {
            strReturnCode = "";

            strURL1 = string.Concat(new string[]
            {
                strURL1,"/RequireConnection?dSn=",dSn,"&authCode=",code,"&mac=",strMac,"&ip=",strIP
            });
            strURL2 = string.Concat(new string[]
            {
                strURL2, "/RequireConnection?dSn=", dSn, "&authCode=",code,"&mac=",strMac, "&ip=",strIP
            });
            strURL1 = strURL1.Replace("+", "%2B");
            strURL2 = strURL2.Replace("+", "%2B");
            bool result;
            try
            {
                lock (_lock)
                {
                    JsonConvert.SerializeObject(new MESRequestURL
                    {
                        dSn = dSn,
                        authCode = code,
                        mac = strMac,
                        ip = strIP
                    }).Replace(":", ": ");
                    string value = "";
                    string text = strURL1;
                    if (!CallBobcat(text, "", out value, out strMessage))
                    {
                        value = "";
                        text = strURL2;
                        if (!CallBobcat(text, "", out value, out strMessage))
                        {
                            strMessage = "RequireConnection." + strMessage;
                            //MessageBox.Show(errMsg);
                            strReturnCode = text;
                            result = false;
                            return result;
                        }
                    }
                    MESRespondURL mESRespondURL = JsonConvert.DeserializeObject<MESRespondURL>(value);
                    strReturnCode = mESRespondURL.ReturnMsg;
                    if (strReturnCode != "Pass")
                    {
                        strMessage = "获取URL的列表list不为Pass";
                        //MessageBox.Show(strReturn);
                        result = false;
                    }
                    else
                    {
                        string str = mESRespondURL.ListURL[0];
                        string str2 = mESRespondURL.ListURL[1];
                        strReturnCode = str + "," + str2;
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                strMessage = ex.Message;
                strMessage = "RequireConnection." + strMessage;
                result = false;
            }
            return result;
        }
        public static bool SetData(string strURL1, string strURL2, string dSn, string code, string data, int station, out string strReturnCode, out string strMessage)
        {
            strReturnCode = "";
            strMessage = "";
            bool result;
            try
            {
                lock (_lock)
                {
                    switch (station)
                    {
                        case 1:
                            strURL1 = string.Concat(new string[]
                {
                    strURL1, "/OEEWebAPI/api/Default/Post?dSn=",dSn,"&authCode=",code
                });
                            strURL2 = string.Concat(new string[]
                {
                    strURL2,"/OEEWebAPI/api/Default/Post?dSn=", dSn,"&authCode=",code
                });
                            break;
                        case 2:
                            strURL1 = string.Concat(new string[]
                {
                    strURL1, "/OEEWebAPI/api/DownTime/Post?dSn=",dSn,"&authCode=",code
                });
                            strURL2 = string.Concat(new string[]
                {
                    strURL2,"/OEEWebAPI/api/DownTime/Post?dSn=", dSn,"&authCode=",code
                });
                            break;
                        case 3:
                            strURL1 = string.Concat(new string[]
                {
                    strURL1, "/OEEWebAPI/api/Machine/Post?dSn=",dSn,"&authCode=",code
                });
                            strURL2 = string.Concat(new string[]
                {
                    strURL2,"/OEEWebAPI/api/Machine/Post?dSn=", dSn,"&authCode=",code
                });
                            break;
                        case 4:
                            strURL1 = string.Concat(new string[]
                {
                    strURL1, "/OEEWebAPI/api/Pant/Post?dSn=",dSn,"&authCode=",code
                });
                            strURL2 = string.Concat(new string[]
                {
                    strURL2,"/OEEWebAPI/api/Pant/Post?dSn=", dSn,"&authCode=",code
                });
                            break;
                        case 5:
                            strURL1 = string.Concat(new string[]
                {
                    strURL1, "/OEEWebAPI/api/logs/Post?dSn=",dSn,"&authCode=",code
                });
                            strURL2 = string.Concat(new string[]
                {
                    strURL2,"/OEEWebAPI/api/logs/Post?dSn=", dSn,"&authCode=",code
                });
                            break;
                        case 6:
                            strURL1 = string.Concat(new string[]
                {
                    strURL1, "/OEEWebAPI/api/materiel/Post?dSn=",dSn,"&authCode=",code
                });
                            strURL2 = string.Concat(new string[]
                {
                    strURL2,"/OEEWebAPI/api/materiel/Post?dSn=", dSn,"&authCode=",code
                });
                            break;
                    }

                    JsonConvert.SerializeObject(new MESRequestData
                    {
                        dSn = dSn,
                        authCode = code,
                        data = data
                    }).Replace(":", ": ");
                    string value = "";
                    string text = strURL1;
                    //Log.WriteOEELog(text + "," + data);
                    if (!CallBobcat(text, data, out value, out strMessage))
                    {
                        value = "";
                        text = strURL2;
                        if (!CallBobcat(text, data, out value, out strMessage))
                        {
                            strMessage = "SetData." + strMessage;
                            strReturnCode = text;
                            result = false;
                            return result;
                        }
                    }

                    MESRespondData MESRespondData = JsonConvert.DeserializeObject<MESRespondData>(value);
                    strReturnCode = MESRespondData.Data.ReturnMsg;
                    if (MESRespondData.Data.ReturnCode == "F")
                    {
                        result = false;
                    }
                    else if (MESRespondData.Data.ReturnCode == "W")
                    {
                        strReturnCode = "W" + strReturnCode;
                        result = true;
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                strMessage = ex.Message;
                strMessage = "SetData." + strMessage;
                result = false;
            }
            return result;
        }
        public static bool CallBobcat(string url, string msg, out string callResult, out string errMsg)
        {
            bool result;
            try
            {
                lock (_lock)
                {
                    url = url.Replace("+", "%2B");

                    byte[] bytes = Encoding.UTF8.GetBytes(msg);
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.Method = "POST";
                    httpWebRequest.ContentLength = (long)bytes.Length;
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Accept = "application/json";
                    httpWebRequest.Timeout = 5000;
                    httpWebRequest.KeepAlive = true;
                    if (bytes != null && bytes.Length > 0)
                    {
                        Stream requestStream = httpWebRequest.GetRequestStream();
                        requestStream.Write(bytes, 0, bytes.Length);
                    }
                    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    Stream responseStream = httpWebResponse.GetResponseStream();
                    StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    callResult = streamReader.ReadToEnd();
                    errMsg = "";
                    result = true;
                }
            }
            catch (Exception ex)
            {
                callResult = "";
                errMsg = ex.ToString();
                result = false;
            }
            return result;
        }
    }
}
