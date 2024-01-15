using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace 通讯接口测试_JGP__20200415
{
    public class CentralControlAPI
    {
        public static object _lock = new object();
        public static bool RequestPost(string url, string msg, out string callResult, out string errMsg)
        {
            bool result;
            try
            {
                lock (_lock)
                {
                    url = url.Replace("+", "%2B");
                    byte[] bytes = Encoding.UTF8.GetBytes(msg);
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
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
