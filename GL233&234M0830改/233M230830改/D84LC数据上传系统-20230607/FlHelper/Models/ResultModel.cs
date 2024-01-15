using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlHelper.Models
{
   public class ResultModel<T> where T:class
    {
        public T Model { set; get; }
        
        public bool Result { set; get; }
        public string Message { set; get; }
    }
}
