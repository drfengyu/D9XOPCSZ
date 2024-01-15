namespace 卓汇数据追溯系统
{
    public class ProcessControlData
    {
        public string Pass
        {
            get;
            set;
        }

        public string[] choice_ids
        {
            get;
            set;
        }

        public string control_id
        {
            get;
            set;
        }

        public Return[] processes
        {
            get;
            set;
        }
        public class Return
        {
            public string id
            {
                get;
                set;
            }

            public string name
            {
                get;
                set;
            }
            public string Event
            {
                get;
                set;
            }
            public bool pass
            {
                get;
                set;
            }
        }
    }
}