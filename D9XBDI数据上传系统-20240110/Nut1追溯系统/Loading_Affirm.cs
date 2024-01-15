using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 卓汇数据追溯系统
{
    class Loading_Affirm
    {
        public string hashcode { get; set; }
        public string request_id { get; set; }
        public string code_no { get; set; }
        public string code_type { get; set; }
        public string bill_no { get; set; }
        public string wrkst_no { get; set; }
        public string trans_no { get; set; }
        public string wo_no { get; set; }
        public string equipment_code { get; set; }
        public string equipment_ip { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string scan_user { get; set; }
        public Bind_Info[] bind_info { get; set; }
        public Others_Info[] others_info { get; set; }
    }
    class Bind_Info
    {
        public string bind_code { get; set; }
        public string bind_codetype { get; set; }
        public string is_replace { get; set; }
    }
    class Others_Info
    {
        public string key_code { get; set; }
        public string key_value { get; set; }
    }
}
