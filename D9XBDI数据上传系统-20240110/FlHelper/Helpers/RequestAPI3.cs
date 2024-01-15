using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FlHelper.Helpers
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


        public static string PostFileMessage(string strUrl, List<PostDateClass> postParaList)
        {
            try
            {
                string responseContent = "";
                var memStream = new MemoryStream();
                var webRequest = (HttpWebRequest)WebRequest.Create(strUrl);
                // 边界符
                var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
                // 边界符
                var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
                // 最后的结束符
                var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");
                memStream.Write(beginBoundary, 0, beginBoundary.Length);
                // 设置属性
                webRequest.Method = "POST";
                webRequest.Timeout = 10000;
                webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
                for (int i = 0; i < postParaList.Count; i++)
                {
                    PostDateClass temp = postParaList[i];
                    if (temp.Type == 1)
                    {
                        var fileStream = new FileStream(temp.Value, FileMode.Open, FileAccess.Read);
                        // 写入文件
                        const string filePartHeader =
                        "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                        "Content-Type: application/octet-stream\r\n\r\n";
                        var header = string.Format(filePartHeader, temp.Prop, temp.Value);
                        var headerbytes = Encoding.UTF8.GetBytes(header);
                        memStream.Write(headerbytes, 0, headerbytes.Length);
                        var buffer = new byte[1024];
                        int bytesRead; // =0
                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            memStream.Write(buffer, 0, bytesRead);
                        }
                        string end = "\r\n";
                        headerbytes = Encoding.UTF8.GetBytes(end);
                        memStream.Write(headerbytes, 0, headerbytes.Length);
                        fileStream.Close();
                    }
                    else if (temp.Type == 0)
                    {
                        // 写入字符串的 Key
                        var stringKeyHeader = "Content-Disposition: form-data; name=\"{0}\"" +
                        "\r\n\r\n{1}\r\n";
                        var header = string.Format(stringKeyHeader, temp.Prop, temp.Value);
                        var headerbytes = Encoding.UTF8.GetBytes(header);
                        memStream.Write(headerbytes, 0, headerbytes.Length);
                    }
                    if (i != postParaList.Count - 1)
                        memStream.Write(beginBoundary, 0, beginBoundary.Length);
                    else
                        // 写入最后的结束边界符
                        memStream.Write(endBoundary, 0, endBoundary.Length);
                }
                webRequest.ContentLength = memStream.Length;
                var requestStream = webRequest.GetRequestStream();
                memStream.Position = 0;
                var tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                memStream.Close();
                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                requestStream.Close();
                using (HttpWebResponse res = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (Stream resStream = res.GetResponseStream())
                    {
                        byte[] buffer = new byte[1024];
                        int read;
                        while ((read = resStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            responseContent += Encoding.UTF8.GetString(buffer, 0, read);
                        }
                    }
                    res.Close();
                }
                return responseContent;
            }
            catch
            {
            }
            return null;
        }
    }

    public class PostDateClass
    {
        public string Prop { get;  set; }
        public string Value { get;  set; }
        public int Type { get; set; }
    }
}
