using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace 卓汇数据追溯系统
{
    public class RequestAPI2
    {
        public static string strReturnURL = "";
        //public static bool RequestGet(string strURL1, string strURL2, string strIP, string strMac, string dSn, string code, string data, out string ReturnMsg)
        //{
        //    string strReturn = "";
        //    string errMsg = "";
        //    bool bRst = false;
        //    if (string.IsNullOrWhiteSpace(strReturnURL))
        //    {
        //        bRst = RequireConnection(strURL1, strURL2, strIP, strMac, dSn, code, out strReturn, out errMsg);
        //        strReturnURL = strReturn;
        //        if (!bRst)
        //        {
        //            ReturnMsg = errMsg;
        //            return false;
        //        }
        //    }

        //    var str = strReturnURL.Split(',');
        //    var bRst1 = GetData(str[0], str[1], dSn, code, data, out strReturn, out errMsg);
        //    if (!bRst1)
        //    {
        //        ReturnMsg = errMsg;
        //        return false;
        //    }
        //    if (strReturn.Substring(0, 1) == "W")
        //    {
        //        var bRst2 = RequireConnection(strURL1, strURL2, strIP, strMac, dSn, code, out strReturn, out errMsg);
        //        if (!bRst2)
        //        {
        //            ReturnMsg = errMsg;
        //            return false;
        //        }

        //        var str2 = strReturn.Split(',');
        //        var bRstr = GetData(str[0], str[1], dSn, code, data, out strReturn, out errMsg);
        //        if (!bRstr)
        //        {
        //            ReturnMsg = errMsg;
        //            return false;
        //        }
        //    }
        //    ReturnMsg = strReturn;
        //    return true;
        //}

        //public static bool RequestSet(string strURL1, string strURL2, string strIP, string strMac, string dSn, string code, string data, out string ReturnMsg)
        //{
        //    string strReturn = "";
        //    string errMsg = "";
        //    bool bRst = false;
        //    if (string.IsNullOrWhiteSpace(strReturnURL))
        //    {
        //        bRst = RequireConnection(strURL1, strURL2, strIP, strMac, dSn, code, out strReturn, out errMsg);
        //        strReturnURL = strReturn;
        //        if (!bRst)
        //        {
        //            ReturnMsg = errMsg;
        //            return false;
        //        }
        //    }

        //    var str = strReturnURL.Split(',');
        //    var bRst1 = SetData(str[0], str[1], dSn, code, data, out strReturn, out errMsg);
        //    if (!bRst1)
        //    {
        //        ReturnMsg = errMsg;
        //        return false;
        //    }
        //    if (strReturn.Substring(0, 1) == "W")
        //    {
        //        var bRst2 = RequireConnection(strURL1, strURL2, strIP, strMac, dSn, code, out strReturn, out errMsg);
        //        if (!bRst2)
        //        {
        //            ReturnMsg = errMsg;
        //            return false;
        //        }

        //        var str2 = strReturn.Split(',');
        //        var bRstr = SetData(str[0], str[1], dSn, code, data, out strReturn, out errMsg);
        //        if (!bRstr)
        //        {
        //            ReturnMsg = errMsg;
        //            return false;
        //        }
        //    }
        //    ReturnMsg = strReturn;
        //    return true;
        //}

        public static bool PIS_System(string strURL, out string strMessage, out string[] Msg)
        {
            //string strReturnCode = "";
            strMessage = "";
            Msg = null;
            bool result;
            try
            {
                string value = "";
                if (!CallBobcat(strURL, "", out value, out strMessage))
                {
                    value = "";
                    strMessage = "PIS_System:" + strMessage;
                    result = false;
                    return result;
                }
                strMessage = "PIS_System:" + strMessage;
                string[] MESRespondData = JsonConvert.DeserializeObject<string[]>(value);
                Msg = MESRespondData;
                result = true;
            }
            catch
            {
                strMessage = "PIS_System:" + strMessage;
                result = false;
            }
            return result;
        }

        public static bool IQC_System(string strURL, string data, out string strMessage, out string[] Msg)
        {
            strMessage = "IQC_System:";
            Msg = null;
            bool result;
            try
            {
                //strURL = strURL.Replace("*", data);
                //JsonConvert.SerializeObject(new MESRequestData
                //{
                //    data = data
                //}).Replace(":", ": ");
                string value = "";
                string text = strURL;
                if (!CallBobcat2(text, data, out value, out strMessage, false))
                {
                    value = "";
                    strMessage = "IQC_System:" + strMessage;
                    result = false;
                    Msg = null;
                    return result;
                }
                string[] MESRespondData = JsonConvert.DeserializeObject<string[]>(value);
                Msg = MESRespondData;
                result = true;
            }
            catch (Exception ex)
            {
                strMessage = ex.Message;
                strMessage = "IQC_System:" + strMessage;
                Msg = null;
                result = false;
            }
            return result;
        }

        public static bool CallBobcat(string url, string msg, out string callResult, out string errMsg)
        {
            bool result;
            try
            {
                url = url.Replace("+", "%2B");

                byte[] bytes = Encoding.UTF8.GetBytes(msg);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "Get";
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
            catch (Exception ex)
            {
                callResult = "";
                errMsg = ex.Message;
                result = false;
            }
            return result;
        }

        public static bool CallBobcat2(string url, string msg, out string callResult, out string errMsg, bool b)
        {
            bool result;
            try
            {
                url = url.Replace("+", "%2B");

                byte[] bytes = Encoding.UTF8.GetBytes(msg);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = (long)bytes.Length;
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";
                if (b)
                {
                    httpWebRequest.Headers.Add("Authorization:Bearer c0beb2f1acc99ed57c1d97abb4fdfa400");
                }
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
            catch (Exception ex)
            {
                callResult = "";
                errMsg = ex.Message;
                result = false;
            }
            return result;
        }
    }
}
