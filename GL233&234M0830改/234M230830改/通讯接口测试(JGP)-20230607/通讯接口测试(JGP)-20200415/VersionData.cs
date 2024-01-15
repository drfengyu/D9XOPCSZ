using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 通讯接口测试_JGP__20200415
{
    public class VersionData
    {
        public string  StationID { get; set; }
        public string moduleCode { get; set; }
        public Versions[] versions { get; set; }
    }
    public class Versions
    {
        public string type { get; set; }
        public string version { get; set; }
        public string description { get; set; }
    }
    public class Rec_VersionData
    {
        public string ok { get; set; }
        public Rec_Versions[] versions { get; set; }

    }
    public class Rec_Versions
    {
        public string type { get; set; }
        public string version { get; set; }
    }
    public class KeyLog
    {
        public string StationID { get; set; }
        public string moduleCode { get; set; }
        public Logs[] logs { get; set; }
    }
    public class Logs
    {
        public string time { get; set; }
        public string operatorId { get; set; }
        public string operatorName { get; set; }
        public string machineCode { get; set; }
        public string action { get; set; }
        public Data_KeyLog data { get; set; }
    }
    public class Rec_KeyLog
    {
        public string ok { get; set; }
    }
    public class Data_KeyLog
    {
        public string name { get; set; }
        public string old { get; set; }
        public string new1 { get; set; }
    }
    internal class Login
    {
        public string StationID { get; set; }
        public string moduleCode { get; set; }
        public string idType { get; set; }
        public string username { get; set; }
    }
    internal class Rec_Login
    {
        public string ok { get; set; }
        public Data_Login data { get; set; }
    }
    internal class Data_Login
    {
        public string jobNumber { get; set; }
        public string name { get; set; }
        public string role { get; set; }
    }

    internal class Logout
    {
        public string StationID { get; set; }
        public string moduleCode { get; set; }
        public string idType { get; set; }
        public string username { get; set; }
    }
    internal class Rec_Logout
    {
        public string ok { get; set; }
    }

}
