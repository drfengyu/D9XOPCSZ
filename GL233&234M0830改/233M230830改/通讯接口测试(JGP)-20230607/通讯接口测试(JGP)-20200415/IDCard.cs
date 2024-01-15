using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 通讯接口测试_JGP__20200415
{
    public class IDCard
    {
        public string cardNo
        {
            get;
            set;
        }

        public string dsn
        {
            get;
            set;
        }

        public int shift
        {
            get;
            set;
        }

        public int machineType
        {
            get;
            set;
        }

        public int swipeType
        {
            get;
            set;
        }

        public string[] stage
        {
            get;
            set;
        }


    }
}
