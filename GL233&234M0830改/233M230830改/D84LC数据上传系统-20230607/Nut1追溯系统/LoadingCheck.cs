using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 卓汇数据追溯系统
{
    class LoadingCheck
    {
        public string hashCode { get; set; }
        public string billNo { get; set; }
        public string barCodeType { get; set; }
        public string barCode { get; set; }
        public string equipmentNo { get; set; }
        public string station { get; set; }
        public string startTime { get; set; }
        public BindCode[] bindCode { get; set; }
        public string resv1 { get; set; }
        public string resv2 { get; set; }

    }
    class BindCode
    {
        public string codeSn { get; set; }
        public string codeSnType { get; set; }
        public string replace { get; set; }
    }
}
