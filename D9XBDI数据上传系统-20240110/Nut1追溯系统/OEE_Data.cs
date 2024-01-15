using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 卓汇数据追溯系统
{
    class OEE_Data
    {
        public string SerialNumber { get; set; }
        public string Fixture { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Status { get; set; }
        public string ActualCT { get; set; }
        public string SwVersion { get; set; }
        public string DefectCode { get; set; }
    }
}
