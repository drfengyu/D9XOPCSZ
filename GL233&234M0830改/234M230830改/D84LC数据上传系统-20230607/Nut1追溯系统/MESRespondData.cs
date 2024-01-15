namespace 卓汇数据追溯系统
{
    internal class MESRespondData
    {
                public string ContentEncoding
        {
            get;
            set;
        }
                        public string ContentType
        {
            get;
            set;
        }
                        public Data Data
        {
            get;
            set;
        }
                        public string JsonRequestBehavior
        {
            get;
            set;
        }
                                public string MaxJsonLength
        {
            get;
            set;
        }
                                public string RecursionLimit
        {
            get;
            set;
        }
        //{"ContentEncoding":null,"ContentType":null,"Data":{"Result":"Fail","ReturnCode":"F","ReturnMsg":"Fail:插入本地数据库失败"},"JsonRequestBehavior":0,"MaxJsonLength":null,"RecursionLimit":null}

  
    }

    public class Data
    {
        public string Result
        {
            get;
            set;
        }

        public string ReturnCode
        {
            get;
            set;
        }

        public string ReturnMsg
        {
            get;
            set;
        }
    }
}