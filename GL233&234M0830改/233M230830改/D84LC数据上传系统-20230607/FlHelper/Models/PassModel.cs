using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlHelper.Models
{
   public class PassModel
    {
       /// <summary>
       /// 上料确认结果
       /// </summary>
       public bool Result { set; get; }
       public string SN { set; get; }
       public string LeftRail { set; get; }
       public string RightRail { set; get; }
       public bool LoadCheck { set; get; }
       public string RequestId { set; get; }

       public string TraceId { set; get; }
       /// <summary>
       /// 20230830
       /// </summary>
       public string Fixture { set; get; }
    }
}
