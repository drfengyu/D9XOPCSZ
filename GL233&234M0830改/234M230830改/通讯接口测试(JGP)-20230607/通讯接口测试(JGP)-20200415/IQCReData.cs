using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 通讯接口测试_JGP__20200415
{
    public class IQCReData
    {
        public string code { get; set; }
        public string[] data { get; set; }
        public string message { get; set; }
        public string isSuccess { get; set; }
        public string FileName { get; set; }
        public string isAutoClose { get; set; }
    }
    public class OverReData
    {
        public string code { get; set; }
        public string[] data { get; set; }
        public string message { get; set; }
        public string isSuccess { get; set; }
        public string FileName { get; set; }
        public string isAutoClose { get; set; }
    }
}
