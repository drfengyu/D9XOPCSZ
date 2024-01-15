using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 卓汇数据追溯系统
{
    public class IQCSystemDATA
    {
        /// <summary>
        /// 厂区名称
        /// </summary>
        public string Plant_Organization_Name { get; set; }
        /// <summary>
        /// OP名称
        /// </summary>
        public string BG_Organization_Name { get; set; }
        /// <summary>
        /// 功能厂名称 可空
        /// </summary>
        public string FunPlant_Organization_Name { get; set; }
        /// <summary>
        /// 专案名称
        /// </summary>
        public string Project_Name { get; set; }
        /// <summary>
        /// 工站名
        /// </summary>
        public string WorkStation_Name { get; set; }
    }
}
