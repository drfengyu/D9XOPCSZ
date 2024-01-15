using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
//using System.Xml;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace 卓汇数据追溯系统
{
    class RequestAPI3
    {
        private static object postlock = new object();
        private static object passlock = new object();

        /// <summary>
        /// 没有Authorization的POST访问
        /// </summary>
        /// <param name="url"></param>
        /// <param name="msg"></param>
        /// <param name="callResult"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool CallBobcat(string url, string msg, out string callResult, out string errMsg)
        {
            lock (postlock)
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

        /// <summary>
        /// 有Authorization的POST访问
        /// </summary>
        /// <param name="url"></param>
        /// <param name="msg"></param>
        /// <param name="hashcode">Authorization</param>
        /// <param name="callResult"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool CallBobcat2(string url, string msg, string hashcode, out string callResult, out string errMsg)
        {
            lock (passlock)
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

                    httpWebRequest.Headers.Add("Authorization:" + hashcode);

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
}
