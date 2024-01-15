using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 卓汇数据追溯系统
{
    class JPRequestData
    {
        public string SN
        {
            get;
            set;
        }

        public string Station
        {
            get;
            set;
        }

        public string Station_id
        {
            get;
            set;
        }

        public string Step
        {
            get;
            set;
        }
        public string RequestTime
        {
            get;
            set;
        }
        public string Line_id
        {
            get;
            set;
        }
        public string MAC
        {
            get;
            set;
        }
        public string IP
        {
            get;
            set;
        }
        public requestCmd RequestCmd
        {
            get;
            set;
        }
        public class requestCmd
        {
            public string ppp
            {
                get;
                set;
            }
            public string eeee
            {
                get;
                set;
            }
            public string r
            {
                get;
                set;
            }
            public string suffix
            {
                get;
                set;
            }
        }
        public string ResponseTime
        {
            get;
            set;
        }
        public string Result
        {
            get;
            set;
        }
        public responseCmd ResponseCmd
        {
            get;
            set;
        }
        public class responseCmd
        {
            public serials serials
            {
                get;
                set;
            }
        }
        public class serials
        {
            public string band
            {
                get;
                set;
            }
        }
    }
}
