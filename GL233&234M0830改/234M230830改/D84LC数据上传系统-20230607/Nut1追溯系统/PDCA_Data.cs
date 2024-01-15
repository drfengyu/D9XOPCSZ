using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 卓汇数据追溯系统
{
    class PDCA_Data
    {
        public string test_result { get; set; }
        public string full_sn { get; set; }
        public string Fixture_id { get; set; }
        public string Start_Time { get; set; }
        public string Weld_start_time { get; set; }
        public string Weld_stop_time { get; set; }
        public string Stop_time { get; set; }
        public double Weld_wait_ct { get; set; }
        public double Actual_weld_ct { get; set; }
        public string status { get; set; }
        public string power_ll { get; set; }
        public string power_ul { get; set; }
        public string pattern_type { get; set; }
        public string frequency { get; set; }
        public string linear_speed { get; set; }
        public string spot_size { get; set; }
        public string pulse_energy { get; set; }
        public string power { get; set; }
        public string filling_pattern { get; set; }
        //public string swing_freq { get; set; }
        //public string swing { get; set; }
        public string hatch { get; set; }
    }
}
