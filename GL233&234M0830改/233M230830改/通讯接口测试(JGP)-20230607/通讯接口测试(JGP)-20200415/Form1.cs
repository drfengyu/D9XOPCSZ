using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection.Emit;
using System.Reflection;
using Newtonsoft.Json;
using System.Dynamic;
using System.Net;
using System.Management;
using System.Collections.Specialized;
using System.IO;
using System.Threading;
using System.Security.Cryptography;

namespace 通讯接口测试_JGP__20200415
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string[] ErrorCode = new string[1000];
        string msg, msg1, msg2, msg3, msg4, msg5, msg6, msg7, msg8, msg9, msg10,
          msg11, msg12, msg13, msg14, msg15, msg16, msg17, msg18, msg19, msg20,
          msg21, msg22, msg23, msg24, msg25, msg26, msg27, msg28, msg29, msg30,
          msg31, msg32, msg33, msg34, msg35, msg36, msg37, msg38, msg39, msg40,
          msg41, msg42, msg43, msg44, msg45, msg46, msg47, msg48, msg49, msg50,
          msg51, msg52, msg53, msg54, msg55, msg56, msg57, msg58, msg59, msg60,
          msg61, msg62, msg63, msg64, msg65, msg66, msg67, msg68, msg69, msg70,
          msg71, msg72, msg73, msg74, msg75, msg76, msg77, msg78, msg79, msg80,
          msg81, msg82, msg83, msg84, msg85, msg86, msg87, msg88, msg89, msg90,
          msg91, msg92, msg93, msg94, msg95, msg96, msg97, msg98, msg99, msg100,
          msg101, msg102, msg103, msg104, msg105, msg106, msg107, msg108, msg109, msg110,
          msg111, msg112, msg113, msg114, msg115, msg116, msg117, msg118, msg119, msg120,
          msg121, msg122, msg123, msg124, msg125, msg126, msg127, msg128, msg129, msg130,
          msg131, msg132, msg133, msg134, msg135, msg136, msg137, msg138, msg139, msg140,
          msg141, msg142, msg143, msg144, msg145, msg146, msg147, msg148, msg149, msg150,
          msg151, msg152, msg153, msg154, msg155, msg156, msg157, msg158, msg159, msg160,
          msg161, msg162, msg163, msg164, msg165, msg166, msg167, msg168, msg169, msg170,
          msg171, msg172, msg173, msg174, msg175, msg176, msg177, msg178, msg179, msg180,
          msg181, msg182, msg183, msg184, msg185, msg186, msg187, msg188, msg189, msg190,
          msg191, msg192, msg193, msg194, msg195, msg196, msg197, msg198, msg199, msg200,
          msg999, msg1000;

        private void Btn_EMTCheck_Click(object sender, EventArgs e)
        {
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            string callResult1 = "";
            string callResult2 = "";
            string errMsg = "";
            EMTData EMTCheckData = new EMTData();
            EMTCheckData.SiteName = "WXI";
            EMTCheckData.EMT = "40003949";
            EMTCheckData.TraceIP = "72.23.200.163";
            EMTCheckData.TraceStationID = "JBGP_A03-2F-OQC02_001_DEVELOPMENT31";
            EMTCheckData.MacAddress = "9C-69-B4-61-AF-43";
            string SendEMTData = JsonConvert.SerializeObject(EMTCheckData, Formatting.None, jsetting);
            RequestAPI2.CallBobcat2("http://10.143.20.191/JMCCEdgeWebAPI_Prd/AssemblyMachineParameterUpdate", SendEMTData, out callResult1, out errMsg, false);
            richTextBox4.AppendText(callResult1 + "\r\n");
            if (callResult1.Contains("Success"))
            {
                richTextBox3.AppendText("EMT号校验成功" + "\r\n");
                Thread.Sleep(1000);
                RequestAPI2.CallBobcat2("http://10.143.20.191/JMCCEdgeWebAPI_Prd/AssemblyMachineParameterQuery", "{\"MacAddress\":\"" + "9C-69-B4-61-AF-43" + "\"}", out callResult2, out errMsg, false);
                richTextBox4.AppendText(callResult2);
                if (!callResult2.Contains("false"))
                {
                    richTextBox3.AppendText("EMT号校验成功" + "\r\n");
                }

            }



        }

        private void Btn_BandToSP_Click(object sender, EventArgs e)
        {
            string URL = "http://17.80.194.10/api/v2/parts?serial_type=sp&serial=DRD211212QQ1WX74F+02+E00101&process_name=primer&last_log=true";
            string callresult = "";
            string errmsg = "";
            var rst = RequestAPI3.CallBobcat(URL, "", "zhh", "DgQT4Thy", out callresult, out errmsg);
            string[] strs = callresult.Split(new string[] { "band\" : " }, 2, StringSplitOptions.None);
            string band = strs[1].Substring(1, 19);
        }

        string getMd5 = string.Empty;

        private void Form1_Load(object sender, EventArgs e)
        {
            //getMd5 = GetFileMd5Code(AppDomain.CurrentDomain.BaseDirectory + "通讯接口测试(JGP) - 20200415.exe");
            //rtxt_CentralControl2.AppendText(getMd5 + "\r\n");
            //Log.WriteLog("版本号为：" + getMd5);
        }

        private void btn_Hans_Click(object sender, EventArgs e)
        {
            DirectoryInfo Dir = new DirectoryInfo(@"D:\学习资料\练习路径\PORFile");
            FileInfo[] filelist = Dir.GetFiles();
            if (filelist.Length > 0)
            {
                int j = 0;
                foreach (FileInfo file in filelist)
                {
                    j++;
                    string fileName = file.FullName;
                    if (Path.GetExtension(fileName) == ".txt")
                    {
                        File.Move(fileName, string.Format(@"D:\学习资料\目的路径\{0}.txt", j));
                        string[] HansDatas = File.ReadAllLines(string.Format(@"D:\学习资料\目的路径\{0}.txt", j), Encoding.Default);
                        string[] hansLine = new string[6];
                        hansLine = HansDatas[0].Split(',');
                        for (int a = 0; a < 6; a++)
                        {
                            rtxt_CentralControl2.AppendText(hansLine[a].Trim() + "|");
                        }
                        rtxt_CentralControl2.Text += "\r\n";
                        //File.Delete(string.Format(@"F:\焊机参数修改\{0}.txt", j));
                    }
                }
            }




            //string RecData = string.Empty;
            //string errorMsg = string.Empty;
            //JsonSerializerSettings jsetting = new JsonSerializerSettings();
            //jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值

            ////关键日志上传
            //KeyLog keyLog = new KeyLog();
            //keyLog.logs = new Logs[1];
            //keyLog.logs[0] = new Logs();
            ////只有初始化类数组里面的每一个类，才能给类的成员赋值。
            ////keyLog[0] = new KeyLog();
            //keyLog.StationID = "JBGP_AE-1F-OQC01_001_DEVELOPMENT11";
            //keyLog.moduleCode = "1";
            //keyLog.logs[0].time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //keyLog.logs[0].operatorId = "123";
            //keyLog.logs[0].operatorName = "张三";
            //keyLog.logs[0].machineCode = "emt编号";
            //keyLog.logs[0].action = "param.set";
            //keyLog.logs[0].data = new Data_KeyLog();
            //keyLog.logs[0].data.name = "右皮带供料HSG扫码位置,取料Z轴";
            //keyLog.logs[0].data.old = "43.196";
            //keyLog.logs[0].data.new1 = "43.930";
            //string SendData = JsonConvert.SerializeObject(keyLog, Formatting.None, jsetting);
            //rtxt_CentralControl2.AppendText("关键日志上传：" + SendData + "\r\n");
            //Log.WriteLog("关键日志上传：" + SendData);
            //CentralControlAPI.RequestPost("http://acs.jabil.topiot.co/openApi/v1/machine/log/operation?access_token=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjE0NDc0NDAwMDY4MjkzMTQwNDgiLCJzbiI6MTQ0NzQ0NTI0NDkwNjgzNTk2OH0.pnTP-0hcDzjTPA5yg3PQLyd96yfmosSU0dyE-Bpc8sg", SendData, out RecData, out errorMsg);

            //Rec_KeyLog rec_log = JsonConvert.DeserializeObject<Rec_KeyLog>(RecData);
            //rtxt_CentralControl2.AppendText("日志上传结果：" + rec_log.ok + "\r\n");
            //Log.WriteLog("日志上传结果：" + rec_log.ok);




        }

        private void btn_Log_Click(object sender, EventArgs e)
        {
            string RecData = string.Empty;
            string errorMsg = string.Empty;
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值

            //关键日志上传
            KeyLog keyLog = new KeyLog();
            keyLog.logs = new Logs[1];
            keyLog.logs[0] = new Logs();
            //只有初始化类数组里面的每一个类，才能给类的成员赋值。
            //keyLog[0] = new KeyLog();
            keyLog.StationID = "JBGP_AE-1F-OQC01_001_DEVELOPMENT11";
            keyLog.moduleCode = "1";
            keyLog.logs[0].time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            keyLog.logs[0].operatorId = "123";
            keyLog.logs[0].operatorName = "张三";
            keyLog.logs[0].machineCode = "emt编号";
            keyLog.logs[0].action = "param.set";
            keyLog.logs[0].data = new Data_KeyLog();
            keyLog.logs[0].data.name = "右皮带供料HSG扫码位置,取料Z轴";
            keyLog.logs[0].data.old = "43.196";
            keyLog.logs[0].data.new1 = "43.930";
            string SendData = JsonConvert.SerializeObject(keyLog, Formatting.None, jsetting);
            SendData = SendData.Replace("new1", "new");
            rtxt_CentralControl2.AppendText("关键日志上传：" + SendData + "\r\n");
            Log.WriteLog("关键日志上传：" + SendData);
            CentralControlAPI.RequestPost("http://10.143.20.222:80/openApi/v1/machine/log/operation?access_token=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjE0NDc0NDAwMDY4MjkzMTQwNDgiLCJzbiI6MTQ0NzQ0NTI0NDkwNjgzNTk2OH0.pnTP-0hcDzjTPA5yg3PQLyd96yfmosSU0dyE-Bpc8sg", SendData, out RecData, out errorMsg);

            Rec_KeyLog rec_log = JsonConvert.DeserializeObject<Rec_KeyLog>(RecData);
            rtxt_CentralControl2.AppendText("日志上传结果：" + rec_log.ok + "\r\n");
            Log.WriteLog("日志上传结果：" + rec_log.ok);
        }
        private void btn_Version_Click(object sender, EventArgs e)
        {
            string RecData = string.Empty;
            string errorMsg = string.Empty;
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值

            //版本信息上传
            VersionData versionData = new VersionData();
            versionData.versions = new Versions[2];
            versionData.versions[0] = new Versions();
            versionData.versions[1] = new Versions();
            //只有初始化类数组里面的每一个类，才能给类的成员赋值。
            //versionData[0] = new VersionData();
            versionData.StationID = "JBGP_AE-1F-OQC01_001_DEVELOPMENT11";
            versionData.moduleCode = "1";
            versionData.versions[0].type = "ZHH-Bracket";
            versionData.versions[0].version = getMd5;
            versionData.versions[0].description = "更新中控系统版本上传";
            //versionData[1] = new VersionData();
            versionData.versions[1].type = "ZHH-Brace";
            versionData.versions[1].version = getMd5;
            versionData.versions[1].description = "更新中控系统版本上传";
            string SendData = JsonConvert.SerializeObject(versionData, Formatting.None, jsetting);
            rtxt_CentralControl2.AppendText("版本上传：" + SendData + "\r\n");
            Log.WriteLog("版本上传：" + SendData);
            CentralControlAPI.RequestPost("http://10.143.20.222:80/openApi/v1/machine/software/version?access_token=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjE0NDc0NDAwMDY4MjkzMTQwNDgiLCJzbiI6MTQ0NzQ0NTI0NDkwNjgzNTk2OH0.pnTP-0hcDzjTPA5yg3PQLyd96yfmosSU0dyE-Bpc8sg", SendData, out RecData, out errorMsg);

            Rec_VersionData rec_Version = JsonConvert.DeserializeObject<Rec_VersionData>(RecData);
            rec_Version.versions = new Rec_Versions[2];
            rec_Version.versions[0] = new Rec_Versions();
            rec_Version.versions[1] = new Rec_Versions();


            rtxt_CentralControl2.AppendText("上传结果：" + rec_Version.ok + "\r\n");
            rtxt_CentralControl1.AppendText("软件：" + rec_Version.versions[0].type + "，版本号：" + rec_Version.versions[0].version + "\r\n");
            rtxt_CentralControl1.AppendText("软件：" + rec_Version.versions[1].type + "，版本号：" + rec_Version.versions[1].version + "\r\n");
            Log.WriteLog("上传结果：" + rec_Version.ok);
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            string RecData = string.Empty;
            string errorMsg = string.Empty;
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值

            //信息登录
            Login login = new Login();
            login.StationID = "JBGP_AE-1F-OQC01_001_DEVELOPMENT11";
            login.moduleCode = "1";
            login.idType = "idCard";
            login.username = txt_Username.Text;
            string SendData = JsonConvert.SerializeObject(login, Formatting.None, jsetting);
            rtxt_CentralControl1.AppendText("请求登录：" + SendData + "\r\n");
            Log.WriteLog("请求登录：" + SendData);
            CentralControlAPI.RequestPost("http://10.143.20.222:80/openApi/v1/machine/login?access_token=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjE0NDc0NDAwMDY4MjkzMTQwNDgiLCJzbiI6MTQ0NzQ0NTI0NDkwNjgzNTk2OH0.pnTP-0hcDzjTPA5yg3PQLyd96yfmosSU0dyE-Bpc8sg", SendData, out RecData, out errorMsg);

            Log.WriteLog("登录结果：" + RecData);
            Rec_Login rec_login = JsonConvert.DeserializeObject<Rec_Login>(RecData);

            rtxt_CentralControl1.AppendText("登录结果：" + rec_login.ok + "\r\n");
            rtxt_CentralControl1.AppendText("详细数据：" + "\r\n" + "工号：" + rec_login.data.jobNumber + "\r\n");
            rtxt_CentralControl1.AppendText("姓名：" + rec_login.data.name + "\r\n");
            rtxt_CentralControl1.AppendText("权限：" + rec_login.data.role + "\r\n");
        }

        private void btn_Logout_Click(object sender, EventArgs e)
        {
            string RecData = string.Empty;
            string errorMsg = string.Empty;
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值
            //退出登录
            Logout logOut = new Logout();
            logOut.StationID = "JBGP_AE-1F-OQC01_001_DEVELOPMENT11";
            logOut.moduleCode = "1";
            logOut.idType = "idCard";
            logOut.username = txt_Username.Text;
            string SendData = JsonConvert.SerializeObject(logOut, Formatting.None, jsetting);
            rtxt_CentralControl1.AppendText("请求退出：" + SendData + "\r\n");
            Log.WriteLog("请求退出：" + SendData);
            CentralControlAPI.RequestPost("http://10.143.20.222:80/openApi/v1/machine/logout?access_token=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjE0NDc0NDAwMDY4MjkzMTQwNDgiLCJzbiI6MTQ0NzQ0NTI0NDkwNjgzNTk2OH0.pnTP-0hcDzjTPA5yg3PQLyd96yfmosSU0dyE-Bpc8sg", SendData, out RecData, out errorMsg);
            Rec_Logout rec_Logout = JsonConvert.DeserializeObject<Rec_Logout>(RecData);
            rtxt_CentralControl1.AppendText("退出结果：" + rec_Logout.ok + "\r\n");
            Log.WriteLog("退出结果：" + rec_Logout.ok);
        }



        private void button27_Click(object sender, EventArgs e)
        {
            Log.WriteCSV("王五,等级三,2021/02/05 16:40:01,X 轴运行速度,300,305", @"D:\参数修改记录");
            Log.WriteCSV("王五,等级三,2021/02/05 16:40:01,Z 轴运行速度,300,320", @"D:\参数修改记录");
            Log.WriteCSV("赵六,等级五,2021/02/05 16:45:25,X 轴运行速度,300,305", @"D:\参数修改记录");
            Log.WriteCSV("赵六,等级五,2021/02/05 16:45:25,Z 轴运行速度,300,320", @"D:\参数修改记录");
            Log.WriteCSV("赵六,等级五,2021/02/05 16:45:25,X 轴待机位置,20,30", @"D:\参数修改记录");
            Log.WriteCSV("赵六,等级五,2021/02/05 16:45:25,Y 轴待机位置,20,30", @"D:\参数修改记录");
            Log.WriteCSV("赵六,等级五,2021/02/05 16:45:25,Z 轴待机位置,0,5", @"D:\参数修改记录");
            Log.WriteCSV("赵六,等级五,2021/02/05 16:45:25,视觉定位功能(0关1开),1,0", @"D:\参数修改记录");
        }

        private void button26_Click(object sender, EventArgs e)
        {
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值
            IDCard CardData = new IDCard();
            CardData.cardNo = "3233018093";
            CardData.dsn = "032096749";
            CardData.shift = 1;
            CardData.machineType = 1;
            CardData.swipeType = 0;
            CardData.stage = new string[] { "Setup", "IQ" };
            string SendCardData = JsonConvert.SerializeObject(CardData, Formatting.None, jsetting);
            string Msg_ua;
            string Trace_str_ua = "";
            richTextBox19.AppendText("上传打卡数据:" + "http://127.0.0.1/Webapi/api/WorkTime/UpdateData.php" + "\r\n");
            Log.WriteLog("上传打卡数据:" + "http://127.0.0.1/Webapi/api/WorkTime/UpdateData.php");
            RequestAPI2.CallBobcat2("http://127.0.0.1/Webapi/api/WorkTime/UpdateData.php", SendCardData, out Trace_str_ua, out Msg_ua, false);
            richTextBox18.AppendText("打卡数据接收：" + JsonConvert.SerializeObject(Trace_str_ua) + "\r\n");
            Log.WriteLog("打卡数据接收:" + JsonConvert.SerializeObject(Trace_str_ua));
        }

        private void button24_Click(object sender, EventArgs e)
        {
            string Msg_ua;
            string Trace_str_ua = "";
            richTextBox19.AppendText("获取班别:" + "http://127.0.0.1/Webapi/api/WorkTime/GetClassInfo.php" + "\r\n");
            Log.WriteLog("获取班别:" + "http://127.0.0.1/Webapi/api/WorkTime/GetClassInfo.php");
            RequestAPI2.CallBobcat("http://127.0.0.1/Webapi/api/WorkTime/GetClassInfo.php", "", out Trace_str_ua, out Msg_ua);
            richTextBox18.AppendText("获取班别接收：" + JsonConvert.DeserializeObject(Trace_str_ua) + "\r\n");
            Log.WriteLog("获取班别接收:" + JsonConvert.DeserializeObject(Trace_str_ua));
        }

        private void button25_Click(object sender, EventArgs e)
        {
            string Msg_ua;
            string Trace_str_ua = "";
            richTextBox19.AppendText("获取用户信息:" + "http://127.0.0.1/Webapi/api/WorkTime/GetUserInfo.php" + "\r\n");
            Log.WriteLog("获取用户信息:" + "http://127.0.0.1/Webapi/api/WorkTime/GetUserInfo.php");
            RequestAPI2.CallBobcat("http://127.0.0.1/Webapi/api/WorkTime/GetUserInfo.php", "", out Trace_str_ua, out Msg_ua);
            richTextBox18.AppendText("获取用户信息接收：" + JsonConvert.SerializeObject(Trace_str_ua) + "\r\n");
            Log.WriteLog("获取用户信息接收:" + JsonConvert.SerializeObject(Trace_str_ua));
        }

        private void button23_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ErrorCode.Length; i++)
            {
                if (ErrorCode[i] != null)
                {
                    textBox1.Text = ErrorCode[i];
                    button3_Click(null, null);
                    Thread.Sleep(200);
                }
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream(@"C:\Users\11\Desktop\报警目录.csv", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.Default);
            string lineData;
            int i = 0;
            while ((lineData = sr.ReadLine()) != null)
            {
                i++;
                ErrorCode[i] += lineData.Split(',')[0];//载入OEE异常代码
            }
            sr.Close();
            fs.Close();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            //AsyncTcpClient client1 = new AsyncTcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), Convert.ToInt32("1111")));
            //client1.PlaintextReceived += new EventHandler<TcpDatagramReceivedEventArgs<string>>(client1_PlaintextReceived);
            //client1.Connect();
            //string data = "begin\nDRD950601SDPF7P29@start\nDRD950601SDPF7P29@priority@1\nDRD950601SDPF7P29@attr@unit_serial_number@DRD950601SDPF7P29\nDRD950601SDPF7P29@attr@test_result@PASS\nDRD950601SDPF7P29@start_time@2020-11-23 08:49:01\nDRD950601SDPF7P29@stop_time@2020-11-23 08:49:13\nDRD950601SDPF7P29@attr@line_id@JACD_B01-2F-OQC01\nDRD950601SDPF7P29@attr@station_id@JACD_B01-2F-OQC01_3_DEVELOPMENT7\nDRD950601SDPF7P29@dut_pos@H-76HO-SMA40-2200-A-00003@1\nDRD950601SDPF7P29@attr@software_name@DEVELOPMENT7\nDRD950601SDPF7P29@attr@software_version@V1.111\nDRD950601SDPF7P29@attr@sp_sn@DRD950601SDPF7P2911\nDRD950601SDPF7P29@attr@fixture_id@H-76HO-SMA40-2200-A-00003\nDRD950601SDPF7P29@attr@weld_start_time@2020-11-23 08:49:13\nDRD950601SDPF7P29@attr@weld_stop_time@2020-11-23 08:49:13\nDRD950601SDPF7P29@attr@precitec_rev@1.2" +
            //    "\nDRD950601SDPF7P29@attr@precitec_grading@2\nDRD950601SDPF7P29@attr@location1_layer1_pulse_profile@0.1ms90%/1.0ms90%/0.2ms0%\nDRD950601SDPF7P29@attr@location2_layer1_pulse_profile@0.1ms90%/1.0ms90%/0.2ms0%\nDRD950601SDPF7P29@attr@location3_layer1_pulse_profile@0.1ms90%/1.0ms90%/0.2ms0%\nDRD950601SDPF7P29@attr@location4_layer1_pulse_profile@0.1ms90%/1.0ms90%/0.2ms0%\nDRD950601SDPF7P29@attr@location4_layer2_pulse_profile@0.1ms90%/1.0ms90%/0.2ms0%\nDRD950601SDPF7P29@attr@location5_layer1_pulse_profile@0.1ms90%/1.0ms90%/0.2ms0%\nDRD950601SDPF7P29@attr@location5_layer2_pulse_profile@0.1ms90%/1.0ms90%/0.2ms0%\nDRD950601SDPF7P29@attr@location6_layer1_pulse_profile@0.1ms90%/1.0ms90%/0.2ms0%\nDRD950601SDPF7P29@attr@location6_layer2_pulse_profile@0.1ms90%/1.0ms90%/0.2ms0%" +
            //    "\nDRD950601SDPF7P29@attr@STATION_STRING@ActualCT:12\nDRD950601SDPF7P29@attr@location1@W23/24\nDRD950601SDPF7P29@attr@location2@W23/24\nDRD950601SDPF7P29@attr@location3@W23/24\nDRD950601SDPF7P29@attr@location4@W23/24\nDRD950601SDPF7P29@attr@location5@W23/24\nDRD950601SDPF7P29@attr@location6@W23/24\nDRD950601SDPF7P29@attr@tossing_item@location1 CCD NG\nDRD950601SDPF7P29@pdata@weld_wait_ct@2@@@s\nDRD950601SDPF7P29@pdata@actual_weld_ct@6@@@s\nDRD950601SDPF7P29@pdata@precitec_value@40@@@%\nDRD950601SDPF7P29@pdata@power_ll@20@@@W\nDRD950601SDPF7P29@pdata@power_ul@1500@@@W\nDRD950601SDPF7P29@pdata@pattern_type@1@@@\nDRD950601SDPF7P29@pdata@spot_size@0.45@@@mm\nDRD950601SDPF7P29@pdata@hatch@0.04@@@mm\nDRD950601SDPF7P29@pdata@swing amplitude@0.02@@@mm\nDRD950601SDPF7P29@pdata@swing_freq@10000@@@Hz" +
            //    "\nDRD950601SDPF7P29@pdata@location1_layer1_laser_power@20@@@W\nDRD950601SDPF7P29@pdata@location1_layer1_frequency@900@@@KHz\nDRD950601SDPF7P29@pdata@location1_layer1_waveform@1@@@\nDRD950601SDPF7P29@pdata@location1_layer1_pulse_energy@3@@@J\nDRD950601SDPF7P29@pdata@location1_layer1_laser_speed@100@@@mm/s\nDRD950601SDPF7P29@pdata@location1_layer1_jump_speed@2000@@@mm/s\nDRD950601SDPF7P29@pdata@location1_layer1_jump_delay@10000@@@us\nDRD950601SDPF7P29@pdata@location1_layer1_scanner_delay@10@@@us\nDRD950601SDPF7P29@pdata@location2_layer1_laser_power@20@@@W\nDRD950601SDPF7P29@pdata@location2_layer1_frequency@900@@@KHz\nDRD950601SDPF7P29@pdata@location2_layer1_waveform@1@@@\nDRD950601SDPF7P29@pdata@location2_layer1_pulse_energy@3@@@J\nDRD950601SDPF7P29@pdata@location2_layer1_laser_speed@100@@@mm/s" +
            //    "\nDRD950601SDPF7P29@pdata@location2_layer1_jump_speed@2000@@@mm/s\nDRD950601SDPF7P29@pdata@location2_layer1_jump_delay@10000@@@us\nDRD950601SDPF7P29@pdata@location2_layer1_scanner_delay@10@@@us\nDRD950601SDPF7P29@pdata@location3_layer1_laser_power@20@@@W\nDRD950601SDPF7P29@pdata@location3_layer1_frequency@900@@@KHz\nDRD950601SDPF7P29@pdata@location3_layer1_waveform@1@@@\nDRD950601SDPF7P29@pdata@location3_layer1_pulse_energy@3@@@J\nDRD950601SDPF7P29@pdata@location3_layer1_laser_speed@100@@@mm/s\nDRD950601SDPF7P29@pdata@location3_layer1_jump_speed@2000@@@mm/s\nDRD950601SDPF7P29@pdata@location3_layer1_jump_delay@10000@@@us\nDRD950601SDPF7P29@pdata@location3_layer1_scanner_delay@10@@@us\nDRD950601SDPF7P29@pdata@location4_layer1_laser_power@20@@@W\nDRD950601SDPF7P29@pdata@location4_layer1_frequency@900@@@KHz" +
            //    "\nDRD950601SDPF7P29@pdata@location4_layer1_waveform@1@@@\nDRD950601SDPF7P29@pdata@location4_layer1_pulse_energy@3@@@J\nDRD950601SDPF7P29@pdata@location4_layer1_laser_speed@100@@@mm/s\nDRD950601SDPF7P29@pdata@location4_layer1_jump_speed@2000@@@mm/s\nDRD950601SDPF7P29@pdata@location4_layer1_jump_delay@10000@@@us\nDRD950601SDPF7P29@pdata@location4_layer1_scanner_delay@10@@@us\nDRD950601SDPF7P29@pdata@location4_layer2_laser_power@20@@@W\nDRD950601SDPF7P29@pdata@location4_layer2_frequency@900@@@KHz\nDRD950601SDPF7P29@pdata@location4_layer2_waveform@1@@@\nDRD950601SDPF7P29@pdata@location4_layer2_pulse_energy@3@@@J\nDRD950601SDPF7P29@pdata@location4_layer2_laser_speed@100@@@mm/s\nDRD950601SDPF7P29@pdata@location4_layer2_jump_speed@2000@@@mm/s\nDRD950601SDPF7P29@pdata@location4_layer2_jump_delay@10000@@@us" +
            //    "\nDRD950601SDPF7P29@pdata@location4_layer2_scanner_delay@10@@@us\nDRD950601SDPF7P29@pdata@location5_layer1_laser_power@20@@@W\nDRD950601SDPF7P29@pdata@location5_layer1_frequency@900@@@KHz\nDRD950601SDPF7P29@pdata@location5_layer1_waveform@1@@@\nDRD950601SDPF7P29@pdata@location5_layer1_pulse_energy@3@@@J\nDRD950601SDPF7P29@pdata@location5_layer1_laser_speed@100@@@mm/s\nDRD950601SDPF7P29@pdata@location5_layer1_jump_speed@2000@@@mm/s\nDRD950601SDPF7P29@pdata@location5_layer1_jump_delay@10000@@@us\nDRD950601SDPF7P29@pdata@location5_layer1_scanner_delay@10@@@us\nDRD950601SDPF7P29@pdata@location5_layer2_laser_power@20@@@W\nDRD950601SDPF7P29@pdata@location5_layer2_frequency@900@@@KHz\nDRD950601SDPF7P29@pdata@location5_layer2_waveform@1@@@\nDRD950601SDPF7P29@pdata@location5_layer2_pulse_energy@3@@@J" +
            //    "\nDRD950601SDPF7P29@pdata@location5_layer2_laser_speed@100@@@mm/s\nDRD950601SDPF7P29@pdata@location5_layer2_jump_speed@2000@@@mm/s\nDRD950601SDPF7P29@pdata@location5_layer2_jump_delay@10000@@@us\nDRD950601SDPF7P29@pdata@location5_layer2_scanner_delay@10@@@us\nDRD950601SDPF7P29@pdata@location6_layer1_laser_power@20@@@W\nDRD950601SDPF7P29@pdata@location6_layer1_frequency@900@@@KHz\nDRD950601SDPF7P29@pdata@location6_layer1_waveform@1@@@\nDRD950601SDPF7P29@pdata@location6_layer1_pulse_energy@3@@@J\nDRD950601SDPF7P29@pdata@location6_layer1_laser_speed@100@@@mm/s\nDRD950601SDPF7P29@pdata@location6_layer1_jump_speed@2000@@@mm/s\nDRD950601SDPF7P29@pdata@location6_layer1_jump_delay@10000@@@us\nDRD950601SDPF7P29@pdata@location6_layer1_scanner_delay@10@@@us\nDRD950601SDPF7P29@pdata@location6_layer2_laser_power@20@@@W" +
            //    "\nDRD950601SDPF7P29@pdata@location6_layer2_frequency@900@@@KHz\nDRD950601SDPF7P29@pdata@location6_layer2_waveform@1@@@\nDRD950601SDPF7P29@pdata@location6_layer2_pulse_energy@3@@@J\nDRD950601SDPF7P29@pdata@location6_layer2_laser_speed@100@@@mm/s\nDRD950601SDPF7P29@pdata@location6_layer2_jump_speed@2000@@@mm/s\nDRD950601SDPF7P29@pdata@location6_layer2_jump_delay@10000@@@us\nDRD950601SDPF7P29@pdata@location6_layer2_scanner_delay@10@@@us\nDRD950601SDPF7P29@submit@V1.111\nend\n";
            //client1.Send(data + "\r\n");
            //richTextBox6.AppendText("PDCA发送:" + data);
            //Log.WriteLog("PDCA发送:" + data);
        }
        void client1_PlaintextReceived(object sender, TcpDatagramReceivedEventArgs<string> e)
        {
            this.Invoke(new ShowData(showdata), e.Datagram.Replace("\r\n", ""));
            //richTextBox6.AppendText("PDCA接收:" + e.Datagram.Replace("\r\n", ""));
            Log.WriteLog("PDCA接收:" + e.Datagram.Replace("\r\n", ""));
        }
        delegate void ShowData(string str);
        void showdata(string data)
        {
            richTextBox6.AppendText("PDCA接收:" + data);
        }

        private void button19_Click(object sender, EventArgs e)
        {

        }

        private void button20_Click(object sender, EventArgs e)
        {
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值
            CheckColor color = new CheckColor();
            color.sn = "FM20201118170703111";
            color.station = "bd-bc-qc";
            string SendColorData = JsonConvert.SerializeObject(color, Formatting.None, jsetting);
            string str = "";
            string str2 = "";
            richTextBox17.AppendText("颜色检查上传:" + SendColorData + "\r\n");
            Log.WriteLog("颜色检查上传:" + SendColorData);
            var result = RequestAPI.CallBobcat("http://127.0.0.1/Webapi/api/common/getlaststationv2.php", SendColorData, out str, out str2);
            richTextBox17.AppendText("颜色检查接收:" + str + "\r\n");
            Log.WriteLog("颜色检查接收:" + str);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            string HeartBeatmsg = "";
            string OEEHeartBeat = "http://127.0.0.1/HOME/RequireConnection.php?dSn=123&authCode=456&mac=00&ip=00";
            var IP = GetIp();
            var Mac = GetMac();

            //OEEHeartBeat = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\"}}", "2", "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            richTextBox4.AppendText("OEE接入上传:" + OEEHeartBeat + "\r\n");
            Log.WriteLog("OEE接入上传:" + OEEHeartBeat);
            var rst = RequestAPI.Request("http://127.0.0.1/HOME", "http://127.0.0.1/HOME", "00", "00", "123", "456", 4, OEEHeartBeat, out HeartBeatmsg);
            richTextBox4.AppendText("OEE接入接收:" + HeartBeatmsg + "\r\n");
            Log.WriteLog("OEE接入接收:" + HeartBeatmsg);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string callResult = "";
            string errMsg = "";
            bool b = false;
            string str = "http://127.0.0.1/PISM21/api/Oee/Setting/CheckTraceMachine.php";
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值
            CheckTraceMachine CheckMachine = new CheckTraceMachine();
            CheckMachine.mode = "query";
            CheckMachine.data = new Data();
            CheckMachine.data.plant = "CN36";
            CheckMachine.data.dsn = "922870105";
            CheckMachine.data.traceIP = "192.168.6.2";
            CheckMachine.data.machine = "werew";
            string SendTraceMachine = JsonConvert.SerializeObject(CheckMachine, Formatting.None, jsetting);
            richTextBox17.AppendText("OEE/Trace防呆上传:" + SendTraceMachine + "\r\n");
            Log.WriteLog("OEE/Trace防呆上传:" + SendTraceMachine);
            var result = RequestAPI2.CallBobcat2(str, SendTraceMachine, out callResult, out errMsg, b);
            richTextBox17.AppendText("OEE/Trace防呆接收:" + callResult + "\r\n");
            Log.WriteLog("OEE/Trace防呆接收:" + callResult);
        }

        public class IQCSystemDATA
        {
            /// <summary>
            /// 厂区名称
            /// </summary>
            public string Plant { get; set; }
            /// <summary>
            /// OP名称
            /// </summary>
            public string BG { get; set; }
            /// <summary>
            /// 功能厂名称 可空
            /// </summary>
            public string FunPlant { get; set; }
            /// <summary>
            /// 专案名称
            /// </summary>
            public string Project { get; set; }
            /// <summary>
            /// 工站名
            /// </summary>
            public string Station { get; set; }
        }

        public class OverSystemDATA
        {
            /// <summary>
            /// 厂区名称
            /// </summary>
            public string Plant { get; set; }
            /// <summary>
            /// OP名称
            /// </summary>
            public string BG { get; set; }
            /// <summary>
            /// 功能厂名称 可空
            /// </summary>
            public string FunPlant { get; set; }
            /// <summary>
            /// 专案名称
            /// </summary>
            public string Project { get; set; }
            /// <summary>
            /// 工站名
            /// </summary>
            public string Station { get; set; }
            /// <summary>
            /// 保养周期
            /// </summary>
            public string Type { get; set; }
        }
        public class CheckTraceMachine
        {
            public string mode { get; set; }
            public Data data { get; set; }
        }
        public class Data
        {
            public string plant { get; set; }
            public string dsn { get; set; }
            public string traceIP { get; set; }
            public string machine { get; set; }
        }
        public class CheckColor
        {
            public string sn { get; set; }
            public string station { get; set; }
        }
        private void button16_Click(object sender, EventArgs e)
        {
            string callResult = "";
            string errMsg = "";
            bool b = false;
            string str = "https://10.143.18.56/pis_m_api/api/fixture/NewGetFixtureIQCInfoApi";
            string s = "";
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值
            IQCSystemDATA IQCData = new IQCSystemDATA();
            IQCData.Plant = "WUXI_M";
            IQCData.BG = "WUXI Metal";
            IQCData.FunPlant = "组装";
            IQCData.Project = "Cairo-Housing";
            IQCData.Station = "GND_CR工站";
            string SendIQCData = JsonConvert.SerializeObject(IQCData, Formatting.None, jsetting);
            richTextBox17.AppendText("治具IQC上传:" + SendIQCData + "\r\n");
            Log.WriteLog("治具IQC上传:" + SendIQCData);
            var result = RequestAPI2.CallBobcat2(str, SendIQCData, out callResult, out errMsg, b);
            if (callResult != "")
            {
                IQCReData fixtureIQCInfo = JsonConvert.DeserializeObject<IQCReData>(callResult);
                string[] strs = fixtureIQCInfo.data;
                for (int i = 0; i < strs.Length; i++)
                {
                    s += strs[i];
                }


                richTextBox17.AppendText("治具IQC接收:" + s + "\r\n");
                Log.WriteLog("治具IQC接收:" + s);
            }

        }

        private void button15_Click(object sender, EventArgs e)
        {
            string callResult = "";
            string errMsg = "";
            string s = string.Empty;
            bool b = false;
            string str = "https://10.143.18.56/pis_m_api/api/fixture/GetNewOverMaintenanceMachine";
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值
            OverSystemDATA OverData = new OverSystemDATA();
            OverData.Plant = "WUXI_M";
            OverData.BG = "WUXI Metal";
            OverData.FunPlant = "组装";
            OverData.Project = "Cairo-Housing";
            OverData.Station = "GND_CR工站";
            OverData.Type = "7D_CR";
            string SendOverData = JsonConvert.SerializeObject(OverData, Formatting.None, jsetting);
            richTextBox17.AppendText("治具保养黑名单:" + SendOverData + "\r\n");
            Log.WriteLog("治具保养黑名单上传:" + SendOverData);
            var result = RequestAPI2.CallBobcat2(str, SendOverData, out callResult, out errMsg, b);
            if (callResult != "")
            {
                OverReData fixtureOverInfo = JsonConvert.DeserializeObject<OverReData>(callResult);
                string[] strs = fixtureOverInfo.data;
                if (strs == null)
                {
                    s = null;
                }
                else
                {
                    for (int i = 0; i < strs.Length; i++)
                    {
                        s += strs[i];
                    }
                }

            }

            richTextBox17.AppendText("治具保养黑名单接收:" + s + "\r\n");
            Log.WriteLog("治具保养黑名单接收:" + s);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            ProcessControlData Msg_ua;
            string Full_SN = "";
            string callResult = "";
            string errMsg = "";
            string OEEHeartBeat = "http://127.0.0.1/Webapi/api/IFactory/MoveWip.php?Customer=Boston-CTU-Housing&Resource=Boston-FVT-PreALT&Route=Boston-Housing-V1-2&RouteStep=Boston-FVT-PreALT&SerialNumber=FM702110097NYMGA80A&Material=Metal-Boston-Housing";
            richTextBox4.AppendText("OEE OKStart上传:" + OEEHeartBeat + "\r\n");
            Log.WriteLog("OEE OKStart上传:" + OEEHeartBeat);
            string IFactoryURL = string.Format("http://127.0.0.1/Webapi/api/IFactory/MoveWip.php?Customer=Boston-CTU-Housing&Resource=Boston-FVT-PreALT&Route=Boston-Housing-V1-2&RouteStep=Boston-FVT-PreALT&SerialNumber=FM702110097NYMGA80A&Material=Metal-Boston-Housing", Full_SN);
            var result = RequestAPI2.CallBobcat3(IFactoryURL, "", out callResult, out errMsg, false);//进行JGP IFactory前站校验
            richTextBox4.AppendText("OEE OKStart接收:" + callResult + "\r\n");
            Log.WriteLog("OEE OKStart接收:" + callResult);
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            string OEEMachineMsg = "";
            var IP = GetIp();
            var Mac = GetMac();
            string OEEMachine = string.Format("{{\"FixtureNum\":\"{0}\"}}", "25");
            var rst = RequestAPI.Request("http://127.0.0.1/HOME", "http://127.0.0.1/HOME", IP, Mac, "123", "456", 3, OEEMachine, out OEEMachineMsg);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string OEE_Data = string.Format("{{\"SerialNumber\":\"{0}\",\"BGBarcode\":\"{1}\",\"Fixture\":\"{2}\",\"StartTime\":\"{3}\",\"EndTime\":\"{4}\",\"Status\":\"{5}\",\"ActualCT\":\"{6}\",\"SwVersion\":\"{7}\",\"ScanCount\":\"{8}\"}}", DateTime.Now.ToString("yyyyMMddHHmmss"), "", "", DateTime.Now.ToString("yyyyMMddHHmmss"), DateTime.Now.ToString("yyyyMMddHHmmss"), "OK", "10.5", "V1.111", "1");
            var IP = GetIp();
            var Mac = GetMac();
            richTextBox4.AppendText("OEE_Product上传:" + OEE_Data + "\r\n");
            Log.WriteLog("OEE_Product上传:" + OEE_Data);
            var rst = RequestAPI.Request("http://127.0.0.1/HOME", "http://127.0.0.1/HOME", IP, Mac, "123", "456", 1, OEE_Data, out msg);
            richTextBox4.AppendText("OEE_Product接收:" + msg + "\r\n");
            Log.WriteLog("OEE_Product接收:" + msg);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string HeartBeatmsg = null;
            var IP = GetIp();
            var Mac = GetMac();
            string OEEHeartBeat = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\"}}", "2", "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            richTextBox4.AppendText(OEEHeartBeat);
            var rst = RequestAPI.Request("http://127.0.0.1/HOME", "http://127.0.0.1/HOME", "00", "00", "123", "456", 4, OEEHeartBeat, out HeartBeatmsg);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string OEEHeartBeat = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\"}}", "2", "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            string str = "";
            string str2 = "";
            var result = RequestAPI.CallBobcat("http://127.0.0.1/api/pant.php", OEEHeartBeat, out str, out str2);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string callresult = "";
            string errmsg = "";
            var rst = RequestAPI3.CallBobcat("http://17.80.194.10/api/v2/parts/serial_type=bg&serial=FY983237LJMK8HT90%2B04029092637187075687816932&process_name=processs_a", "", "cm", "password", out callresult, out errmsg);
        }

        class MaterielData
        {
            public string date;
            public string count;
            public string totalcount;
            public string parttype;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值
            MaterielData data = new MaterielData();
            data.date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            data.count = "1";
            data.totalcount = "12";
            data.parttype = string.Format("{0}", "1");
            string Materiel_data = JsonConvert.SerializeObject(data, Formatting.None, jsetting);
            var IP = GetIp();
            var Mac = GetMac();
            var rst = RequestAPI.Request("http://10.128.19.162/HOME", "http://10.128.19.162/HOME", IP, Mac, "041714415", "a2cfba48-e779-43ed-8364-f5a53c7fa6a8", 6, Materiel_data, out msg);
            richTextBox14.AppendText("小料抛料：" + Materiel_data + "\r\n");
            richTextBox13.AppendText(msg + "\r\n");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            bool Call_PIS_API_flag = false;
            string strMes = "";
            string[] PISMsg = new string[50];
            //string URL = "http://cnctug0pdmstst1/PIS_M_API/api/fixture/GetOverMaintenanceMachine?Project=Boston&Station=WeldSPNuts&Type=D15";
            string URL = "http://cnctug0pdmstst1/PIS_M_API/api/fixture/GetOverMaintenanceMachine?Project=Boston&Station=Weld SP Nuts&Type=D15";
            //string URL = Global.inidata.productconfig.PIS_URL.Replace("Project", "Project=" + Global.project).Replace("Station", "Station=" + Global.station).Replace("Type", "Type=" + Global.type);
            RequestAPI2.PIS_System(URL, out strMes, out PISMsg);
            Txt.WriteLine(PISMsg);
            //for (int J = 0; J < PISMsg.Length; i++)
            //{
            //    if (PISMsg[J] != null)
            //    {
            //_homefrm.AppendRichText(strMes + PISMsg[J], "rth_FixtureMsg");
            //Log.WriteLog("定时读取逾期保养治具编号：" + strMes + PISMsg[J]);
            //    }
            //}
        }

        public class LWM_Data
        {
            public string message_name
            {
                get;
                set;
            }
            public string ml_result
            {
                get;
                set;
            }
            public string serial_number
            {
                get;
                set;
            }
            public string status
            {
                get;
                set;
            }
            public string time_stamp
            {
                get;
                set;
            }
            public nut_data[] nut_results
            {
                get;
                set;
            }
        }
        public class nut_data
        {
            public string nut
            {
                get;
                set;
            }
            public string pull_ort_force
            {
                get;
                set;
            }
            public string pull_result
            {
                get;
                set;
            }
            public string shear_ort_force
            {
                get;
                set;
            }
            public string shear_result
            {
                get;
                set;
            }
        }
        SQLServer SQL = new SQLServer();
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            ProcessControlData Msg_ua;
            string Trace_str_ua = "";
            richTextBox10.AppendText("Trace process_control上传:" + "http://127.0.0.1/v2/process_control.php?serial=ABC84075MDRJKKX4E02&serial_type=band&agent_alias=fg-bc-le" + "\r\n");
            Log.WriteLog("Trace process_control上传:" + "http://127.0.0.1/v2/process_control.php?serial=ABC84075MDRJKKX4E02&serial_type=band&agent_alias=fg-bc-le");
            RequestAPI2.Trace_process_control("http://127.0.0.1/v2/process_control.php?serial=*&serial_type=band&agent_alias=fg-bc-le", "ABC84075MDRJKKX4E02", out Trace_str_ua, out Msg_ua);//Trace_la校验前站
            richTextBox10.AppendText("Trace校验UA前站SN：" + JsonConvert.SerializeObject(Msg_ua) + "\r\n");
            Log.WriteLog("Trace process_control接收:" + JsonConvert.SerializeObject(Msg_ua));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string PLST_Data = richTextBox8.Text;
            if (PLST_Data != string.Empty)
            {
                string value = PLST_Data.Substring(40, 8);
                string s = value.Substring(6, 2) + value.Substring(4, 2) + value.Substring(2, 2) + value.Substring(0, 2);
                int a = Convert.ToInt32(s, 16);//普雷斯特版本号
                richTextBox7.AppendText("普雷斯特版本号：" + a + "\r\n");
                string value2 = PLST_Data.Substring(64, 8);
                string s2 = value2.Substring(6, 2) + value2.Substring(4, 2) + value2.Substring(2, 2) + value2.Substring(0, 2);
                int a2 = Convert.ToInt32(s2, 16);//字符串转16进制32位无符号整数
                float fy = BitConverter.ToSingle(BitConverter.GetBytes(a2), 0);//IEEE754 字节转换float  普雷斯特评分等级
                richTextBox7.AppendText("普雷斯特评分：" + fy.ToString() + "\r\n");
                int garding = 0;
                if (fy > 0 && fy <= 90)//根据评分，判定焊接等级
                {
                    garding = 1;
                }
                else if (fy > 90 && fy <= 95)
                {
                    garding = 2;
                }
                else if (fy > 95 && fy <= 99)
                {
                    garding = 3;
                }
                else if (fy > 99 && fy <= 150)
                {
                    garding = 4;
                }
                else if (fy > 150)
                {
                    garding = 5;
                }
                else
                {
                    garding = 0;
                }
                richTextBox7.AppendText("普雷斯特评分等级：" + garding.ToString() + "\r\n");

                string value3 = PLST_Data.Substring(96, 50);//返回的SN
                byte[] buff = new byte[value3.Length];
                int index = 0;
                for (int i = 0; i < value3.Length; i += 2)
                {
                    buff[index] = Convert.ToByte(value3.Substring(i, 2), 16);
                    ++index;
                }
                string result = (Encoding.Default.GetString(buff)).Replace("\0", "");
                richTextBox7.AppendText("普雷斯特对应SN：" + result + "\r\n");
            }
            else
            {
                MessageBox.Show("数据为空，请在左侧文本框输入普雷斯特数据！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值

            TraceMesRequest_la trace_la = new TraceMesRequest_la();
            trace_la.serials = new SN2();
            trace_la.data = new data2();
            trace_la.data.insight = new Insight2();
            trace_la.data.insight.test_attributes = new Test_attributes2();
            trace_la.data.insight.test_station_attributes = new Test_station_attributes2();
            trace_la.data.insight.uut_attributes = new Uut_attributes4();
            trace_la.data.insight.results = new Result2[50];
            for (int i = 0; i < trace_la.data.insight.results.Length; i++)
            {
                trace_la.data.insight.results[i] = new Result2();
            }
            //trace_la.data.items = new ExpandoObject();
            trace_la.serials.sp = "DRD015301M7P56C3220H00101";
            trace_la.data.insight.test_attributes.test_result = "pass";
            trace_la.data.insight.test_attributes.unit_serial_number = "DRD015301M7P56C32";
            trace_la.data.insight.test_attributes.uut_start = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            trace_la.data.insight.test_attributes.uut_stop = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            trace_la.data.insight.test_station_attributes.fixture_id = "H-76HO-SMA40-2200-A-00003";
            trace_la.data.insight.test_station_attributes.head_id = "1";
            trace_la.data.insight.test_station_attributes.line_id = "JACD_E03-2F-OQC01";
            trace_la.data.insight.test_station_attributes.software_name = "DEVELOPMENT8";
            trace_la.data.insight.test_station_attributes.software_version = "V1.111";
            trace_la.data.insight.test_station_attributes.station_id = "JACD_E03-2F-OQC01_3_DEVELOPMENT8";
            trace_la.data.insight.uut_attributes.STATION_STRING = "";
            trace_la.data.insight.uut_attributes.fixture_id = "H-76HO-SMA40-2200-A-00003";
            trace_la.data.insight.uut_attributes.location1 = "W23/24";
            trace_la.data.insight.uut_attributes.location1_layer1_pulse_profile = "0.1ms90%/1.0ms90%/0.2ms0%";
            trace_la.data.insight.uut_attributes.location2 = "W23/24";
            trace_la.data.insight.uut_attributes.location2_layer1_pulse_profile = "0.1ms90%/1.0ms90%/0.2ms0%";
            trace_la.data.insight.uut_attributes.location3 = "W23/24";
            trace_la.data.insight.uut_attributes.location3_layer1_pulse_profile = "0.1ms90%/1.0ms90%/0.2ms0%";
            trace_la.data.insight.uut_attributes.location4 = "W23/24";
            trace_la.data.insight.uut_attributes.location4_layer1_pulse_profile = "0.1ms90%/1.0ms90%/0.2ms0%";
            trace_la.data.insight.uut_attributes.location5 = "W23/24";
            trace_la.data.insight.uut_attributes.location5_layer1_pulse_profile = "0.1ms90%/1.0ms90%/0.2ms0%";
            trace_la.data.insight.uut_attributes.ml_result = "Pass";
            //trace_la.data.insight.uut_attributes.location6 = "W23/24";
            //trace_la.data.insight.uut_attributes.location6_layer1_pulse_profile = "0.1ms90%/1.0ms90%/0.2ms0%";
            //trace_la.data.insight.uut_attributes.location7 = "W23/24";
            //trace_la.data.insight.uut_attributes.location7_layer1_pulse_profile = "0.1ms90%/1.0ms90%/0.2ms0%";
            trace_la.data.insight.uut_attributes.precitec_grading = "2";
            trace_la.data.insight.uut_attributes.precitec_rev = "1.2";
            trace_la.data.insight.uut_attributes.sp_sn = "DRD015301M7P56C3220";
            trace_la.data.insight.uut_attributes.tossing_item = "location1 CCD NG/location2 CCD NG";
            trace_la.data.insight.uut_attributes.weld_start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            trace_la.data.insight.uut_attributes.weld_stop_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            trace_la.data.insight.results[0].result = "pass";
            trace_la.data.insight.results[0].test = "weld_wait_ct";
            trace_la.data.insight.results[0].units = "s";
            trace_la.data.insight.results[0].value = "2";
            trace_la.data.insight.results[1].result = "pass";
            trace_la.data.insight.results[1].test = "actual_weld_ct";
            trace_la.data.insight.results[1].units = "s";
            trace_la.data.insight.results[1].value = "6";
            trace_la.data.insight.results[2].result = "pass";
            trace_la.data.insight.results[2].test = "precitec_value";
            trace_la.data.insight.results[2].units = "%";
            trace_la.data.insight.results[2].value = "40";
            trace_la.data.insight.results[3].result = "pass";
            trace_la.data.insight.results[3].test = "power_ll";
            trace_la.data.insight.results[3].units = "W";
            trace_la.data.insight.results[3].value = "20";
            trace_la.data.insight.results[4].result = "pass";
            trace_la.data.insight.results[4].test = "power_ul";
            trace_la.data.insight.results[4].units = "W";
            trace_la.data.insight.results[4].value = "1500";
            trace_la.data.insight.results[5].result = "pass";
            trace_la.data.insight.results[5].test = "pattern_type";
            trace_la.data.insight.results[5].units = "/";
            trace_la.data.insight.results[5].value = "1";
            trace_la.data.insight.results[6].result = "pass";
            trace_la.data.insight.results[6].test = "spot_size";
            trace_la.data.insight.results[6].units = "mm";
            trace_la.data.insight.results[6].value = "0.45";
            trace_la.data.insight.results[7].result = "pass";
            trace_la.data.insight.results[7].test = "hatch";
            trace_la.data.insight.results[7].units = "mm";
            trace_la.data.insight.results[7].value = "0.04";
            trace_la.data.insight.results[8].result = "pass";
            trace_la.data.insight.results[8].test = "swing_amplitude";
            trace_la.data.insight.results[8].units = "mm";
            trace_la.data.insight.results[8].value = "0.02";
            trace_la.data.insight.results[9].result = "pass";
            trace_la.data.insight.results[9].test = "swing_freq";
            trace_la.data.insight.results[9].units = "Hz";
            trace_la.data.insight.results[9].value = "10000";
            trace_la.data.insight.results[10].result = "pass";
            trace_la.data.insight.results[10].test = "location1_layer1_laser_power";
            trace_la.data.insight.results[10].units = "W";
            trace_la.data.insight.results[10].value = "20";
            trace_la.data.insight.results[11].result = "pass";
            trace_la.data.insight.results[11].test = "location1_layer1_frequency";
            trace_la.data.insight.results[11].units = "KHz";
            trace_la.data.insight.results[11].value = "900";
            trace_la.data.insight.results[12].result = "pass";
            trace_la.data.insight.results[12].test = "location1_layer1_waveform";
            trace_la.data.insight.results[12].units = "/";
            trace_la.data.insight.results[12].value = "1";
            trace_la.data.insight.results[13].result = "pass";
            trace_la.data.insight.results[13].test = "location1_layer1_pulse_energy";
            trace_la.data.insight.results[13].units = "J";
            trace_la.data.insight.results[13].value = "3";
            trace_la.data.insight.results[14].result = "pass";
            trace_la.data.insight.results[14].test = "location1_layer1_laser_speed";
            trace_la.data.insight.results[14].units = "mm/s";
            trace_la.data.insight.results[14].value = "100";
            trace_la.data.insight.results[15].result = "pass";
            trace_la.data.insight.results[15].test = "location1_layer1_jump_speed";
            trace_la.data.insight.results[15].units = "mm/s";
            trace_la.data.insight.results[15].value = "2000";
            trace_la.data.insight.results[16].result = "pass";
            trace_la.data.insight.results[16].test = "location1_layer1_jump_delay";
            trace_la.data.insight.results[16].units = "us";
            trace_la.data.insight.results[16].value = "10000";
            trace_la.data.insight.results[17].result = "pass";
            trace_la.data.insight.results[17].test = "location1_layer1_scanner_delay";
            trace_la.data.insight.results[17].units = "us";
            trace_la.data.insight.results[17].value = "10";

            trace_la.data.insight.results[18].result = "pass";
            trace_la.data.insight.results[18].test = "location2_layer1_laser_power";
            trace_la.data.insight.results[18].units = "W";
            trace_la.data.insight.results[18].value = "20";
            trace_la.data.insight.results[19].result = "pass";
            trace_la.data.insight.results[19].test = "location2_layer1_frequency";
            trace_la.data.insight.results[19].units = "KHz";
            trace_la.data.insight.results[19].value = "900";
            trace_la.data.insight.results[20].result = "pass";
            trace_la.data.insight.results[20].test = "location2_layer1_waveform";
            trace_la.data.insight.results[20].units = "/";
            trace_la.data.insight.results[20].value = "1";
            trace_la.data.insight.results[21].result = "pass";
            trace_la.data.insight.results[21].test = "location2_layer1_pulse_energy";
            trace_la.data.insight.results[21].units = "J";
            trace_la.data.insight.results[21].value = "3";
            trace_la.data.insight.results[22].result = "pass";
            trace_la.data.insight.results[22].test = "location2_layer1_laser_speed";
            trace_la.data.insight.results[22].units = "mm/s";
            trace_la.data.insight.results[22].value = "100";
            trace_la.data.insight.results[23].result = "pass";
            trace_la.data.insight.results[23].test = "location2_layer1_jump_speed";
            trace_la.data.insight.results[23].units = "mm/s";
            trace_la.data.insight.results[23].value = "2000";
            trace_la.data.insight.results[24].result = "pass";
            trace_la.data.insight.results[24].test = "location2_layer1_jump_delay";
            trace_la.data.insight.results[24].units = "us";
            trace_la.data.insight.results[24].value = "10000";
            trace_la.data.insight.results[25].result = "pass";
            trace_la.data.insight.results[25].test = "location2_layer1_scanner_delay";
            trace_la.data.insight.results[25].units = "us";
            trace_la.data.insight.results[25].value = "10";

            trace_la.data.insight.results[26].result = "pass";
            trace_la.data.insight.results[26].test = "location3_layer1_laser_power";
            trace_la.data.insight.results[26].units = "W";
            trace_la.data.insight.results[26].value = "20";
            trace_la.data.insight.results[27].result = "pass";
            trace_la.data.insight.results[27].test = "location3_layer1_frequency";
            trace_la.data.insight.results[27].units = "KHz";
            trace_la.data.insight.results[27].value = "900";
            trace_la.data.insight.results[28].result = "pass";
            trace_la.data.insight.results[28].test = "location3_layer1_waveform";
            trace_la.data.insight.results[28].units = "/";
            trace_la.data.insight.results[28].value = "1";
            trace_la.data.insight.results[29].result = "pass";
            trace_la.data.insight.results[29].test = "location3_layer1_pulse_energy";
            trace_la.data.insight.results[29].units = "J";
            trace_la.data.insight.results[29].value = "3";
            trace_la.data.insight.results[30].result = "pass";
            trace_la.data.insight.results[30].test = "location3_layer1_laser_speed";
            trace_la.data.insight.results[30].units = "mm/s";
            trace_la.data.insight.results[30].value = "100";
            trace_la.data.insight.results[31].result = "pass";
            trace_la.data.insight.results[31].test = "location3_layer1_jump_speed";
            trace_la.data.insight.results[31].units = "mm/s";
            trace_la.data.insight.results[31].value = "2000";
            trace_la.data.insight.results[32].result = "pass";
            trace_la.data.insight.results[32].test = "location3_layer1_jump_delay";
            trace_la.data.insight.results[32].units = "us";
            trace_la.data.insight.results[32].value = "10000";
            trace_la.data.insight.results[33].result = "pass";
            trace_la.data.insight.results[33].test = "location3_layer1_scanner_delay";
            trace_la.data.insight.results[33].units = "us";
            trace_la.data.insight.results[33].value = "10";

            trace_la.data.insight.results[34].result = "pass";
            trace_la.data.insight.results[34].test = "location4_layer1_laser_power";
            trace_la.data.insight.results[34].units = "W";
            trace_la.data.insight.results[34].value = "20";
            trace_la.data.insight.results[35].result = "pass";
            trace_la.data.insight.results[35].test = "location4_layer1_frequency";
            trace_la.data.insight.results[35].units = "KHz";
            trace_la.data.insight.results[35].value = "900";
            trace_la.data.insight.results[36].result = "pass";
            trace_la.data.insight.results[36].test = "location4_layer1_waveform";
            trace_la.data.insight.results[36].units = "/";
            trace_la.data.insight.results[36].value = "1";
            trace_la.data.insight.results[37].result = "pass";
            trace_la.data.insight.results[37].test = "location4_layer1_pulse_energy";
            trace_la.data.insight.results[37].units = "J";
            trace_la.data.insight.results[37].value = "3";
            trace_la.data.insight.results[38].result = "pass";
            trace_la.data.insight.results[38].test = "location4_layer1_laser_speed";
            trace_la.data.insight.results[38].units = "mm/s";
            trace_la.data.insight.results[38].value = "100";
            trace_la.data.insight.results[39].result = "pass";
            trace_la.data.insight.results[39].test = "location4_layer1_jump_speed";
            trace_la.data.insight.results[39].units = "mm/s";
            trace_la.data.insight.results[39].value = "2000";
            trace_la.data.insight.results[40].result = "pass";
            trace_la.data.insight.results[40].test = "location4_layer1_jump_delay";
            trace_la.data.insight.results[40].units = "us";
            trace_la.data.insight.results[40].value = "10000";
            trace_la.data.insight.results[41].result = "pass";
            trace_la.data.insight.results[41].test = "location4_layer1_scanner_delay";
            trace_la.data.insight.results[41].units = "us";
            trace_la.data.insight.results[41].value = "10";

            trace_la.data.insight.results[42].result = "pass";
            trace_la.data.insight.results[42].test = "location5_layer1_laser_power";
            trace_la.data.insight.results[42].units = "W";
            trace_la.data.insight.results[42].value = "20";
            trace_la.data.insight.results[43].result = "pass";
            trace_la.data.insight.results[43].test = "location5_layer1_frequency";
            trace_la.data.insight.results[43].units = "KHz";
            trace_la.data.insight.results[43].value = "900";
            trace_la.data.insight.results[44].result = "pass";
            trace_la.data.insight.results[44].test = "location5_layer1_waveform";
            trace_la.data.insight.results[44].units = "/";
            trace_la.data.insight.results[44].value = "1";
            trace_la.data.insight.results[45].result = "pass";
            trace_la.data.insight.results[45].test = "location5_layer1_pulse_energy";
            trace_la.data.insight.results[45].units = "J";
            trace_la.data.insight.results[45].value = "3";
            trace_la.data.insight.results[46].result = "pass";
            trace_la.data.insight.results[46].test = "location5_layer1_laser_speed";
            trace_la.data.insight.results[46].units = "mm/s";
            trace_la.data.insight.results[46].value = "100";
            trace_la.data.insight.results[47].result = "pass";
            trace_la.data.insight.results[47].test = "location5_layer1_jump_speed";
            trace_la.data.insight.results[47].units = "mm/s";
            trace_la.data.insight.results[47].value = "2000";
            trace_la.data.insight.results[48].result = "pass";
            trace_la.data.insight.results[48].test = "location5_layer1_jump_delay";
            trace_la.data.insight.results[48].units = "us";
            trace_la.data.insight.results[48].value = "10000";
            trace_la.data.insight.results[49].result = "pass";
            trace_la.data.insight.results[49].test = "location5_layer1_scanner_delay";
            trace_la.data.insight.results[49].units = "us";
            trace_la.data.insight.results[49].value = "10";

            //trace_la.data.insight.results[50].result = "pass";
            //trace_la.data.insight.results[50].test = "location6_layer1_laser_power";
            //trace_la.data.insight.results[50].units = "W";
            //trace_la.data.insight.results[50].value = "20";
            //trace_la.data.insight.results[51].result = "pass";
            //trace_la.data.insight.results[51].test = "location6_layer1_frequency";
            //trace_la.data.insight.results[51].units = "KHz";
            //trace_la.data.insight.results[51].value = "900";
            //trace_la.data.insight.results[52].result = "pass";
            //trace_la.data.insight.results[52].test = "location6_layer1_waveform";
            //trace_la.data.insight.results[52].units = "/";
            //trace_la.data.insight.results[52].value = "1";
            //trace_la.data.insight.results[53].result = "pass";
            //trace_la.data.insight.results[53].test = "location6_layer1_pulse_energy";
            //trace_la.data.insight.results[53].units = "J";
            //trace_la.data.insight.results[53].value = "3";
            //trace_la.data.insight.results[54].result = "pass";
            //trace_la.data.insight.results[54].test = "location6_layer1_laser_speed";
            //trace_la.data.insight.results[54].units = "mm/s";
            //trace_la.data.insight.results[54].value = "100";
            //trace_la.data.insight.results[55].result = "pass";
            //trace_la.data.insight.results[55].test = "location6_layer1_jump_speed";
            //trace_la.data.insight.results[55].units = "mm/s";
            //trace_la.data.insight.results[55].value = "2000";
            //trace_la.data.insight.results[56].result = "pass";
            //trace_la.data.insight.results[56].test = "location6_layer1_jump_delay";
            //trace_la.data.insight.results[56].units = "us";
            //trace_la.data.insight.results[56].value = "10000";
            //trace_la.data.insight.results[57].result = "pass";
            //trace_la.data.insight.results[57].test = "location6_layer1_scanner_delay";
            //trace_la.data.insight.results[57].units = "us";
            //trace_la.data.insight.results[57].value = "10";

            //trace_la.data.insight.results[58].result = "pass";
            //trace_la.data.insight.results[58].test = "location7_layer1_laser_power";
            //trace_la.data.insight.results[58].units = "W";
            //trace_la.data.insight.results[58].value = "20";
            //trace_la.data.insight.results[59].result = "pass";
            //trace_la.data.insight.results[59].test = "location7_layer1_frequency";
            //trace_la.data.insight.results[59].units = "KHz";
            //trace_la.data.insight.results[59].value = "900";
            //trace_la.data.insight.results[60].result = "pass";
            //trace_la.data.insight.results[60].test = "location7_layer1_waveform";
            //trace_la.data.insight.results[60].units = "/";
            //trace_la.data.insight.results[60].value = "1";
            //trace_la.data.insight.results[61].result = "pass";
            //trace_la.data.insight.results[61].test = "location7_layer1_pulse_energy";
            //trace_la.data.insight.results[61].units = "J";
            //trace_la.data.insight.results[61].value = "3";
            //trace_la.data.insight.results[62].result = "pass";
            //trace_la.data.insight.results[62].test = "location7_layer1_laser_speed";
            //trace_la.data.insight.results[62].units = "mm/s";
            //trace_la.data.insight.results[62].value = "100";
            //trace_la.data.insight.results[63].result = "pass";
            //trace_la.data.insight.results[63].test = "location7_layer1_jump_speed";
            //trace_la.data.insight.results[63].units = "mm/s";
            //trace_la.data.insight.results[63].value = "2000";
            //trace_la.data.insight.results[64].result = "pass";
            //trace_la.data.insight.results[64].test = "location7_layer1_jump_delay";
            //trace_la.data.insight.results[64].units = "us";
            //trace_la.data.insight.results[64].value = "10000";
            //trace_la.data.insight.results[65].result = "pass";
            //trace_la.data.insight.results[65].test = "location7_layer1_scanner_delay";
            //trace_la.data.insight.results[65].units = "us";
            //trace_la.data.insight.results[65].value = "10";

            //for (int i = 1; i <= 5; i++)
            //{
            //    (trace_la.data.items as ICollection<KeyValuePair<string, object>>).Add(new KeyValuePair<string, object>("error_" + i, "10003001_2020-04-15 00:00:00"));
            //}
            string SendTraceLogs = JsonConvert.SerializeObject(trace_la, Formatting.None, jsetting);
            Log.WriteLog(SendTraceLogs);
            richTextBox1.AppendText(SendTraceLogs);
            string Trcae_logs_str = string.Empty;
            TraceRespondID Msg = null;
            var Trcae_logs_result = RequestAPI2.Trcae_logs("http://localhost:8765/v2/logs?agent_alias=la-nut-wld", SendTraceLogs, out Trcae_logs_str, out Msg);
            Log.WriteLog(JsonConvert.SerializeObject(Msg, Formatting.Indented));
            richTextBox2.AppendText(Trcae_logs_str + "," + Msg.id);
        }

        private void btn_SendPLST_Click(object sender, EventArgs e)
        {
            richTextBox5.Clear();
            richTextBox6.Clear();
            NameValueCollection name = new NameValueCollection();
            name.Add("serial_number", "DRD015201VWP56C3Q20W00101");
            string MacRespond = HttpPostData("http://169.254.1.10:8076/predict", 2000, "test_file", @"C:\Users\user.USER-3QG1ALH75B\Desktop\DRD015201VWP56C3Q20W00101.csv", name);
            richTextBox5.AppendText(MacRespond);
            LWM_Data result = new LWM_Data();
            result = JsonConvert.DeserializeObject<LWM_Data>(MacRespond);//
            for (int i = 0; i < result.nut_results.Length; i++)
            {
                string insStr = string.Format("insert into LWMData([DateTime],[ml_result],[serial_number],[status],[time_stamp],[Nut],[pull_ort_force],[pull_result],[shear_ort_force],[shear_result]) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), result.ml_result, result.serial_number, result.status, result.time_stamp, result.nut_results[i].nut, result.nut_results[i].pull_ort_force, result.nut_results[i].pull_result, result.nut_results[i].shear_ort_force, result.nut_results[i].shear_result);
                int r = SQL.ExecuteUpdate(insStr);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string OEE_DT = "";
            string msg = "";
            var IP = GetIp();
            var Mac = GetMac();
            bool rst = false;
            OEE_DT = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", "3", textBox1.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "");
            richTextBox4.AppendText("OEE_DT上传:" + OEE_DT + "\r\n");
            Log.WriteLog("OEE_DT上传:" + OEE_DT);
            rst = RequestAPI.Request("http://10.143.18.202", "http://10.143.18.203", IP, Mac, "123", "456", 2, OEE_DT, out msg);
            richTextBox4.AppendText("OEE_DT接收:" + msg + "\r\n");
            Log.WriteLog("OEE_DT接收:" + msg);
            //Thread.Sleep(2000);
            //OEE_DT = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", "2", "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "");
            //richTextBox4.AppendText("OEE_DT上传:" + OEE_DT + "\r\n");
            //Log.WriteLog("OEE_DT上传:" + OEE_DT);
            //rst = RequestAPI.Request("http://127.0.0.1/HOME", "http://127.0.0.1/HOME", IP, Mac, "123", "456", 2, OEE_DT, out msg);
            //richTextBox4.AppendText("OEE_DT接收:" + msg + "\r\n");
            //Log.WriteLog("OEE_DT接收:" + msg);
            //Thread.Sleep(3000);
            //OEE_DT = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", "7", "11010001", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "");
            //richTextBox4.AppendText("OEE_DT上传:" + OEE_DT + "\r\n");
            //Log.WriteLog("OEE_DT上传:" + OEE_DT);
            //rst = RequestAPI.Request("http://127.0.0.1/HOME", "http://127.0.0.1/HOME", IP, Mac, "123", "456", 2, OEE_DT, out msg);
            //richTextBox4.AppendText("OEE_DT接收:" + msg + "\r\n");
            //Log.WriteLog("OEE_DT接收:" + msg);
            //Thread.Sleep(5000);
            //OEE_DT = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", "2", "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "");
            //richTextBox4.AppendText("OEE_DT上传:" + OEE_DT + "\r\n");
            //Log.WriteLog("OEE_DT上传:" + OEE_DT);
            //rst = RequestAPI.Request("http://127.0.0.1/HOME", "http://127.0.0.1/HOME", IP, Mac, "123", "456", 2, OEE_DT, out msg);
            //richTextBox4.AppendText("OEE_DT接收:" + msg + "\r\n");
            //Log.WriteLog("OEE_DT接收:" + msg);
            //Thread.Sleep(8000);
            //OEE_DT = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", "5", "70010001", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "");
            //richTextBox4.AppendText("OEE_DT上传:" + OEE_DT + "\r\n");
            //Log.WriteLog("OEE_DT上传:" + OEE_DT);
            //rst = RequestAPI.Request("http://127.0.0.1/HOME", "http://127.0.0.1/HOME", IP, Mac, "123", "456", 2, OEE_DT, out msg);
            //richTextBox4.AppendText("OEE_DT接收:" + msg + "\r\n");
            //Log.WriteLog("OEE_DT接收:" + msg);
            //Thread.Sleep(1000);
            //OEE_DT = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", "2", "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "");
            //richTextBox4.AppendText("OEE_DT上传:" + OEE_DT + "\r\n");
            //Log.WriteLog("OEE_DT上传:" + OEE_DT);
            //rst = RequestAPI.Request("http://127.0.0.1/HOME", "http://127.0.0.1/HOME", IP, Mac, "123", "456", 2, OEE_DT, out msg);
            //richTextBox4.AppendText("OEE_DT接收:" + msg + "\r\n");
            //Log.WriteLog("OEE_DT接收:" + msg);
            //Thread.Sleep(2000);
            //OEE_DT = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", "6", "10010001", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "");
            //richTextBox4.AppendText("OEE_DT上传:" + OEE_DT + "\r\n");
            //Log.WriteLog("OEE_DT上传:" + OEE_DT);
            //rst = RequestAPI.Request("http://127.0.0.1/HOME", "http://127.0.0.1/HOME", IP, Mac, "123", "456", 2, OEE_DT, out msg);
            //richTextBox4.AppendText("OEE_DT接收:" + msg + "\r\n");
            //Log.WriteLog("OEE_DT接收:" + msg);
            //Thread.Sleep(6000);
            //OEE_DT = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", "2", "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "");
            //richTextBox4.AppendText("OEE_DT上传:" + OEE_DT + "\r\n");
            //Log.WriteLog("OEE_DT上传:" + OEE_DT);
            //rst = RequestAPI.Request("http://127.0.0.1/HOME", "http://127.0.0.1/HOME", IP, Mac, "123", "456", 2, OEE_DT, out msg);
            //richTextBox4.AppendText("OEE_DT接收:" + msg + "\r\n");
            //Log.WriteLog("OEE_DT接收:" + msg);
            //Thread.Sleep(4000);
            //OEE_DT = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", "3", "60070002", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "");
            //richTextBox4.AppendText("OEE_DT上传:" + OEE_DT + "\r\n");
            //Log.WriteLog("OEE_DT上传:" + OEE_DT);
            //rst = RequestAPI.Request("http://127.0.0.1/HOME", "http://127.0.0.1/HOME", IP, Mac, "123", "456", 2, OEE_DT, out msg);
            //richTextBox4.AppendText("OEE_DT接收:" + msg + "\r\n");
            //Log.WriteLog("OEE_DT接收:" + msg);
            //Thread.Sleep(1000);
            //OEE_DT = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\",\"ModuleCode\":\"{3}\"}}", "2", "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "");
            //richTextBox4.AppendText("OEE_DT上传:" + OEE_DT + "\r\n");
            //Log.WriteLog("OEE_DT上传:" + OEE_DT);
            //rst = RequestAPI.Request("http://127.0.0.1/HOME", "http://127.0.0.1/HOME", IP, Mac, "123", "456", 2, OEE_DT, out msg);
            //richTextBox4.AppendText("OEE_DT接收:" + msg + "\r\n");
            //Log.WriteLog("OEE_DT接收:" + msg);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string HeartBeatmsg = "";
            string OEEHeartBeat = "";
            var IP = GetIp();
            var Mac = GetMac();
            OEEHeartBeat = string.Format("{{\"Status\":\"{0}\",\"ErrorCode\":\"{1}\",\"EventTime\":\"{2}\"}}", "2", "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            richTextBox4.AppendText("OEE心跳上传:" + OEEHeartBeat + "\r\n");
            Log.WriteLog("OEE心跳上传:" + OEEHeartBeat);
            var rst = RequestAPI.Request("http://10.143.18.202", "http://10.143.18.203", IP, Mac, "042125851", "aaaaf4a8-92c9-45f3-8c81-72aaa554e3fc", 4, OEEHeartBeat, out HeartBeatmsg);
            richTextBox4.AppendText("OEE心跳接收:" + HeartBeatmsg + "\r\n");
            Log.WriteLog("OEE心跳接收:" + HeartBeatmsg);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //richTextBox1.Clear();
            //richTextBox2.Clear();
            //JsonSerializerSettings jsetting = new JsonSerializerSettings();
            //jsetting.NullValueHandling = NullValueHandling.Ignore;//Json不输出空值

            //TraceMesRequest_ua trace_ua = new TraceMesRequest_ua();




            //trace_ua.serial_type = "rm";
            //trace_ua.serials = new string[3];
            //trace_ua.serials[0] = "PPPYWWDSSSSEEEERX+RR+AAAAABBBBBBCCCC";
            //trace_ua.serials[1] = "PPPYWWDSSSSEEEERX";
            //trace_ua.serials[2] = "XPPYWWDSSSSEEEERX";


            //trace_ua.data = new data();
            //trace_ua.data.insight = new Insight();
            //trace_ua.data.insight.test_attributes = new Test_attributes();
            //trace_ua.data.insight.test_station_attributes = new Test_station_attributes();
            //trace_ua.data.insight.uut_attributes = new Uut_attributes();
            //trace_ua.data.insight.results = new Result[2];
            //for (int i = 0; i < trace_ua.data.insight.results.Length; i++)
            //{
            //    trace_ua.data.insight.results[i] = new Result();
            //}

            //string URL = "http://17.80.194.10/api/v2/parts?serial_type=band&serial=DRD211300C51RXN4W23&process_name=primer&last_log=true";
            //string callresult = "";
            //string errmsg = "";
            //var rst = RequestAPI3.CallBobcat(URL, "", "zhh", "DgQT4Thy", out callresult, out errmsg);
            //string[] strs = callresult.Split(new string[] { "sp\" : " }, 2, StringSplitOptions.None);
            //string sp = strs[1].Substring(1, 27);


            //trace_ua.data.items = new ExpandoObject();
            ////trace_ua.serials.sp = sp; //"DRD211212QP1WX74G+02+E00101";//用sp码
            //trace_ua.data.insight.test_attributes.test_result = "pass";
            //trace_ua.data.insight.test_attributes.unit_serial_number = "DUMMYDUMMYDUMMYSN";//用band前面17位
            //trace_ua.data.insight.test_attributes.uut_start = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //trace_ua.data.insight.test_attributes.uut_stop = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //trace_ua.data.insight.test_station_attributes.fixture_id = "H-76HO-SMA40-2200-A-00003";
            //trace_ua.data.insight.test_station_attributes.head_id = "1";
            //trace_ua.data.insight.test_station_attributes.line_id = "IPGL_C09-3FA";
            //trace_ua.data.insight.test_station_attributes.software_name = "DEVELOPMENT1";
            //trace_ua.data.insight.test_station_attributes.software_version = "V1.111";
            //trace_ua.data.insight.test_station_attributes.station_id = "Site_LineID_MachineID_StationName";

            //trace_ua.data.insight.uut_attributes.STATION_STRING = "Free-from string";
            //trace_ua.data.insight.uut_attributes.fixture_id = "H-76HO-SMA40-2200-A-00003";
            //trace_ua.data.insight.uut_attributes.hatch = "0.04";
            //trace_ua.data.insight.uut_attributes.laser_start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //trace_ua.data.insight.uut_attributes.laser_stop_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //trace_ua.data.insight.uut_attributes.lc_sn = "";

            //trace_ua.data.insight.uut_attributes.left_rail_sn = "";

            //trace_ua.data.insight.uut_attributes.pattern_type = "1";
            //trace_ua.data.insight.uut_attributes.precitec_grading = "2";
            //trace_ua.data.insight.uut_attributes.precitec_rev = "1.2";
            //trace_ua.data.insight.uut_attributes.precitec_value = "40";

            //trace_ua.data.insight.uut_attributes.right_rail_sn = "";

            //trace_ua.data.insight.uut_attributes.spot_size = "0.45";

            //trace_ua.data.insight.uut_attributes.station_vendor = "HG";

            //trace_ua.data.insight.uut_attributes.swing_amplitude = "0.02";
            //trace_ua.data.insight.uut_attributes.swing_freq = "10000";
            //trace_ua.data.insight.uut_attributes.tossing_item = "location1 CCD NG/location2 CCD NG";
            //trace_ua.data.insight.uut_attributes.rework_item = "0";
            //trace_ua.data.insight.uut_attributes.rework_label = "pass";
            //trace_ua.data.insight.uut_attributes.tossing_item = "location1 CCD NG/location2 CCD NG";
            //trace_ua.data.insight.uut_attributes.actual_power_judgment = "0";
            //trace_ua.data.insight.uut_attributes.actual_power_measure_time = "0";
            //trace_ua.data.insight.uut_attributes.laser_machine_id = "0";
            //trace_ua.data.insight.uut_attributes.laser_pulse_profile_measure_setting = "0";
            //trace_ua.data.insight.results[0].result = "pass";
            //trace_ua.data.insight.results[0].test = "weld_wait_ct";
            //trace_ua.data.insight.results[0].units = "s";
            //trace_ua.data.insight.results[0].value = "2";
            //trace_ua.data.insight.results[1].result = "pass";
            //trace_ua.data.insight.results[1].test = "actual_weld_ct";
            //trace_ua.data.insight.results[1].units = "s";
            //trace_ua.data.insight.results[1].value = "6";
            //trace_ua.data.insight.results[2].test = "precitec_value";
            //trace_ua.data.insight.results[2].result = "pass";
            //trace_ua.data.insight.results[2].value = "0";
            //trace_ua.data.insight.results[2].units = "%";
            //trace_ua.data.insight.results[3].result = "pass";
            //trace_ua.data.insight.results[3].test = "power_ll";
            //trace_ua.data.insight.results[3].units = "W";
            //trace_ua.data.insight.results[3].value = "0.000";
            //trace_ua.data.insight.results[4].result = "pass";
            //trace_ua.data.insight.results[4].test = "power_ul";
            //trace_ua.data.insight.results[4].units = "W";
            //trace_ua.data.insight.results[4].value = "0.000";
            //trace_ua.data.insight.results[5].result = "pass";
            //trace_ua.data.insight.results[5].test = "pattern_type";
            //trace_ua.data.insight.results[5].units = "/";
            //trace_ua.data.insight.results[5].value = "0";
            //trace_ua.data.insight.results[6].result = "pass";
            //trace_ua.data.insight.results[6].test = "spot_size";
            //trace_ua.data.insight.results[6].units = "mm";
            //trace_ua.data.insight.results[6].value = "0";
            //trace_ua.data.insight.results[7].result = "pass";
            //trace_ua.data.insight.results[7].test = "hatch";
            //trace_ua.data.insight.results[7].units = "mm";
            //trace_ua.data.insight.results[7].value = "0";
            //trace_ua.data.insight.results[8].result = "pass";
            //trace_ua.data.insight.results[8].test = "swing_amplitude";
            //trace_ua.data.insight.results[8].units = "mm";
            //trace_ua.data.insight.results[8].value = "0";
            //trace_ua.data.insight.results[9].result = "pass";
            //trace_ua.data.insight.results[9].test = "swing_freq";
            //trace_ua.data.insight.results[9].units = "Hz";
            //trace_ua.data.insight.results[9].value = "0";
            //trace_ua.data.insight.results[10].result = "pass";
            //trace_ua.data.insight.results[10].test = "location1_W23";
            //trace_ua.data.insight.results[10].units = "/";
            //trace_ua.data.insight.results[10].value = "0";
            //trace_ua.data.insight.results[11].result = "pass";
            //trace_ua.data.insight.results[11].test = "location2_W24";
            //trace_ua.data.insight.results[11].units = "/";
            //trace_ua.data.insight.results[11].value = "0";
            //trace_ua.data.insight.results[12].result = "pass";
            //trace_ua.data.insight.results[12].test = "location3_W25";
            //trace_ua.data.insight.results[12].units = "/";
            //trace_ua.data.insight.results[12].value = "0";
            //trace_ua.data.insight.results[13].result = "pass";
            //trace_ua.data.insight.results[13].test = "location4_U-Z-1";
            //trace_ua.data.insight.results[13].units = "/";
            //trace_ua.data.insight.results[13].value = "0";
            //trace_ua.data.insight.results[14].result = "pass";
            //trace_ua.data.insight.results[14].test = "location5_U-Z-2";
            //trace_ua.data.insight.results[14].units = "/";
            //trace_ua.data.insight.results[14].value = "0";
            //trace_ua.data.insight.results[15].result = "pass";
            //trace_ua.data.insight.results[15].test = "actual_power";
            //trace_ua.data.insight.results[15].units = "W";
            //trace_ua.data.insight.results[15].value = "0";
            //trace_ua.data.insight.results[16].result = "pass";
            //trace_ua.data.insight.results[16].test = "laser_power_measure_setting";
            //trace_ua.data.insight.results[16].units = "W";
            //trace_ua.data.insight.results[16].value = "0";
            //trace_ua.data.insight.results[17].result = "pass";
            //trace_ua.data.insight.results[17].test = "laser_waveform_measure_setting";
            //trace_ua.data.insight.results[17].units = "/";
            //trace_ua.data.insight.results[17].value = "0";
            //trace_ua.data.insight.results[18].result = "pass";
            //trace_ua.data.insight.results[18].test = "laser_frequency_measure_setting";
            //trace_ua.data.insight.results[18].units = "KHz";
            //trace_ua.data.insight.results[18].value = "0";
            //trace_ua.data.insight.results[19].result = "pass";
            //trace_ua.data.insight.results[19].test = "laser_speed_measure_setting";
            //trace_ua.data.insight.results[19].units = "mm";
            //trace_ua.data.insight.results[19].value = "0";
            //trace_ua.data.insight.results[20].result = "pass";
            //trace_ua.data.insight.results[20].test = "laser_q_release_measure_setting";
            //trace_ua.data.insight.results[20].units = "us";
            //trace_ua.data.insight.results[20].value = "0";
            //trace_ua.data.insight.results[21].result = "pass";
            //trace_ua.data.insight.results[21].test = "laser_pulse_energy_measure_setting";
            //trace_ua.data.insight.results[21].units = "J";
            //trace_ua.data.insight.results[21].value = "0";
            //trace_ua.data.insight.results[22].result = "pass";
            //trace_ua.data.insight.results[22].test = "laser_peak_power_measure_setting";
            //trace_ua.data.insight.results[22].units = "J";
            //trace_ua.data.insight.results[22].value = "0";
            //trace_ua.data.insight.results[23].result = "pass";
            //trace_ua.data.insight.results[23].test = "location1_layer1_laser_power";
            //trace_ua.data.insight.results[23].units = "W";
            //trace_ua.data.insight.results[23].value = "6.0000";
            //trace_ua.data.insight.results[24].result = "pass";
            //trace_ua.data.insight.results[24].test = "location1_layer1_frequency";
            //trace_ua.data.insight.results[24].units = "KHz";
            //trace_ua.data.insight.results[24].value = "25.0000";
            //trace_ua.data.insight.results[25].result = "pass";
            //trace_ua.data.insight.results[25].test = "location1_layer1_waveform";
            //trace_ua.data.insight.results[25].units = "/";
            //trace_ua.data.insight.results[25].value = "6";
            //trace_ua.data.insight.results[26].result = "pass";
            //trace_ua.data.insight.results[26].test = "location1_layer1_pulse_energy";
            //trace_ua.data.insight.results[26].units = "J";
            //trace_ua.data.insight.results[26].value = "15.02";
            //trace_ua.data.insight.results[27].result = "pass";
            //trace_ua.data.insight.results[27].test = "location1_layer1_laser_speed";
            //trace_ua.data.insight.results[27].units = "mm/s";
            //trace_ua.data.insight.results[27].value = "60.0000";
            //trace_ua.data.insight.results[28].result = "pass";
            //trace_ua.data.insight.results[28].test = "location1_layer1_jump_speed";
            //trace_ua.data.insight.results[28].units = "mm/s";
            //trace_ua.data.insight.results[28].value = "300.0000";
            //trace_ua.data.insight.results[29].result = "pass";
            //trace_ua.data.insight.results[29].test = "location1_layer1_jump_delay";
            //trace_ua.data.insight.results[29].units = "us";
            //trace_ua.data.insight.results[29].value = "0";
            //trace_ua.data.insight.results[30].result = "pass";
            //trace_ua.data.insight.results[30].test = "location1_layer1_scanner_delay";
            //trace_ua.data.insight.results[30].units = "us";
            //trace_ua.data.insight.results[30].value = "0";
            //trace_ua.data.insight.results[31].result = "pass";
            //trace_ua.data.insight.results[31].test = "location2_layer1_laser_power";
            //trace_ua.data.insight.results[31].units = "W";
            //trace_ua.data.insight.results[31].value = "20";
            //trace_ua.data.insight.results[32].result = "pass";
            //trace_ua.data.insight.results[32].test = "location2_layer1_frequency";
            //trace_ua.data.insight.results[32].units = "KHz";
            //trace_ua.data.insight.results[32].value = "900";
            //trace_ua.data.insight.results[33].result = "pass";
            //trace_ua.data.insight.results[33].test = "location2_layer1_waveform";
            //trace_ua.data.insight.results[33].units = "/";
            //trace_ua.data.insight.results[33].value = "1";
            //trace_ua.data.insight.results[34].result = "pass";
            //trace_ua.data.insight.results[34].test = "location2_layer1_pulse_energy";
            //trace_ua.data.insight.results[34].units = "J";
            //trace_ua.data.insight.results[34].value = "3";
            //trace_ua.data.insight.results[35].result = "pass";
            //trace_ua.data.insight.results[35].test = "location2_layer1_laser_speed";
            //trace_ua.data.insight.results[35].units = "mm/s";
            //trace_ua.data.insight.results[35].value = "100";
            //trace_ua.data.insight.results[36].result = "pass";
            //trace_ua.data.insight.results[36].test = "location2_layer1_jump_speed";
            //trace_ua.data.insight.results[36].units = "mm/s";
            //trace_ua.data.insight.results[36].value = "2000";
            //trace_ua.data.insight.results[37].result = "pass";
            //trace_ua.data.insight.results[37].test = "location2_layer1_jump_delay";
            //trace_ua.data.insight.results[37].units = "us";
            //trace_ua.data.insight.results[37].value = "10000";
            //trace_ua.data.insight.results[38].result = "pass";
            //trace_ua.data.insight.results[38].test = "location2_layer1_scanner_delay";
            //trace_ua.data.insight.results[38].units = "us";
            //trace_ua.data.insight.results[38].value = "10";
            //trace_ua.data.insight.results[39].result = "pass";
            //trace_ua.data.insight.results[39].test = "location3_layer1_laser_power";
            //trace_ua.data.insight.results[39].units = "W";
            //trace_ua.data.insight.results[39].value = "20";
            //trace_ua.data.insight.results[40].result = "pass";
            //trace_ua.data.insight.results[40].test = "location3_layer1_frequency";
            //trace_ua.data.insight.results[40].units = "KHz";
            //trace_ua.data.insight.results[40].value = "900";
            //trace_ua.data.insight.results[41].result = "pass";
            //trace_ua.data.insight.results[41].test = "location3_layer1_waveform";
            //trace_ua.data.insight.results[41].units = "/";
            //trace_ua.data.insight.results[41].value = "1";
            //trace_ua.data.insight.results[42].result = "pass";
            //trace_ua.data.insight.results[42].test = "location3_layer1_pulse_energy";
            //trace_ua.data.insight.results[42].units = "J";
            //trace_ua.data.insight.results[42].value = "3";
            //trace_ua.data.insight.results[43].result = "pass";
            //trace_ua.data.insight.results[43].test = "location3_layer1_laser_speed";
            //trace_ua.data.insight.results[43].units = "mm/s";
            //trace_ua.data.insight.results[43].value = "100";
            //trace_ua.data.insight.results[44].result = "pass";
            //trace_ua.data.insight.results[44].test = "location3_layer1_jump_speed";
            //trace_ua.data.insight.results[44].units = "mm/s";
            //trace_ua.data.insight.results[44].value = "2000";
            //trace_ua.data.insight.results[45].result = "pass";
            //trace_ua.data.insight.results[45].test = "location3_layer1_jump_delay";
            //trace_ua.data.insight.results[45].units = "us";
            //trace_ua.data.insight.results[45].value = "10000";
            //trace_ua.data.insight.results[46].result = "pass";
            //trace_ua.data.insight.results[46].test = "location3_layer1_scanner_delay";
            //trace_ua.data.insight.results[46].units = "us";
            //trace_ua.data.insight.results[46].value = "10";


            //for (int i = 1; i <= 1; i++)
            //{
            //    (trace_ua.data.items as ICollection<KeyValuePair<string, object>>).Add(new KeyValuePair<string, object>("error_" + (i + 1), "00000000" + "_" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            //}
            //string SendTraceLogs = JsonConvert.SerializeObject(trace_ua, Formatting.None, jsetting);
            //richTextBox1.AppendText("Tracelogs上传:" + SendTraceLogs + "\r\n");
            //Log.WriteLog("Tracelogs上传:" + SendTraceLogs);
            //string Trcae_logs_str = string.Empty;
            //TraceRespondID Msg = null;
            //var Trcae_logs_result = RequestAPI2.Trcae_logs("http://localhost:8765/v2/logs?", SendTraceLogs, out Trcae_logs_str, out Msg);
            //richTextBox2.AppendText("Tracelogs接收:" + Trcae_logs_str + "," + Msg.id + "\r\n");
            //Log.WriteLog("Tracelogs接收:" + JsonConvert.SerializeObject(Msg, Formatting.Indented));
        }

        /// <summary>
        /// 转成可以扩充的对象
        /// </summary> 
        /// <param name="obj"></param>
        /// <returns></returns>
        public dynamic ConvertToDynamic(object obj)
        {
            IDictionary<string, object> result = new ExpandoObject();

            foreach (PropertyDescriptor pro in TypeDescriptor.GetProperties(obj.GetType()))
            {
                result.Add(pro.Name, pro.GetValue(obj));
            }
            return result as ExpandoObject;
        }

        private void btn_SendPDCA_Click(object sender, EventArgs e)
        {
            richTextBox5.Clear();
            richTextBox6.Clear();
            BailCilent bc = new BailCilent();
            bc.openServerConnection("169.254.1.10", Int32.Parse("1111"), "169.254.1.100");
            //bc.SN = "DRD1143C01W10DY5U";
            msg = bc.createStartMessage("DRD1143C01W10DY5U");
            //msg1 = bc.createPriority("1");
            msg1 = bc.createAttrMessage("unit_serial_number", "DRD1143C01W10DY5U");
            msg2 = bc.createAttrMessage("test_result", "PASS");
            msg3 = bc.createStartTimeMessage(DateTime.Now.AddSeconds(-12));
            msg4 = bc.createStopTimeMessage(DateTime.Now);
            msg5 = bc.createAttrMessage("head_id", "1");
            msg6 = bc.createAttrMessage("line_id", "JACD_A03-2F-OQC01");
            msg7 = bc.createAttrMessage("software_name", "DEVELOPMENT7");
            msg8 = bc.createAttrMessage("software_version", "210606A");
            msg9 = bc.createAttrMessage("station_id", "JACD_A03-2F-OQC01_1_DEVELOPMENT7");
            //msg8 = bc.createDUT_POSMessage("H-76HO-SMA40-2200-A-00003", "1");
            msg10 = bc.createAttrMessage("STATION_STRING", "220224A");
            msg12 = bc.createAttrMessage("fixture_id", "H-76HO-SMA40-2200-A-00003");
            msg11 = bc.createAttrMessage("full_sn", "DRD211212QP1WX74G+02+E00101");
            msg13 = bc.createAttrMessage("hatch", "0.04");

            msg14 = bc.createAttrMessage("laser_start_time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            msg15 = bc.createAttrMessage("laser_stop_time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            msg16 = bc.createAttrMessage("pattern_type", "1");
            msg17 = bc.createAttrMessage("precitec_grading", "2");
            msg18 = bc.createAttrMessage("precitec_rev", "1.2");
            msg19 = bc.createAttrMessage("precitec_value", "40");

            msg20 = bc.createAttrMessage("spot_size", "0.45");


            msg21 = bc.createAttrMessage("swing_amplitude", "0.02");
            msg22 = bc.createAttrMessage("swing_freq", "10000");

            //msg17 = bc.createAttrMessage("laser_machine_id", "0");
            //msg18 = bc.createAttrMessage("actual_power_measure_time", "0");
            //msg19 = bc.createAttrMessage("laser_pulse_profile_measure_setting", "0");
            //msg20 = bc.createAttrMessage("actual_power_judgment", "0");
            //msg21 = bc.createAttrMessage("rework_item", "0");
            //msg22 = bc.createAttrMessage("rework_label", "N");

            msg23 = bc.createPDataMessage("location1_L-X", "0", "", "", "/");
            msg24 = bc.createPDataMessage("location2_U-X", "0", "", "", "/");
            msg25 = bc.createPDataMessage("location3_L-Z", "0", "", "", "/");
            msg26 = bc.createPDataMessage("location4_U-Z-1", "0", "", "", "/");
            msg27 = bc.createPDataMessage("location5_U-Z-2", "0", "", "", "/");
            //msg17 = bc.createAttrMessage("location1_layer1_pulse_profile", "0.1ms90%/1.0ms90%/0.2ms0%");
            //msg19 = bc.createAttrMessage("location2_layer1_pulse_profile", "0.1ms90%/1.0ms90%/0.2ms0%");
            //msg21 = bc.createAttrMessage("location3_layer1_pulse_profile", "0.1ms90%/1.0ms90%/0.2ms0%");
            //msg22 = bc.createAttrMessage("location4_layer1_pulse_profile", "0.1ms90%/1.0ms90%/0.2ms0%");
            //msg23 = bc.createAttrMessage("location4_layer2_pulse_profile", "0.1ms90%/1.0ms90%/0.2ms0%");
            //msg24 = bc.createAttrMessage("location5_layer1_pulse_profile", "0.1ms90%/1.0ms90%/0.2ms0%");
            //msg25 = bc.createAttrMessage("location5_layer2_pulse_profile", "0.1ms90%/1.0ms90%/0.2ms0%");
            //msg26 = bc.createAttrMessage("location6_layer1_pulse_profile", "0.1ms90%/1.0ms90%/0.2ms0%");
            //msg27 = bc.createAttrMessage("location6_layer2_pulse_profile", "0.1ms90%/1.0ms90%/0.2ms0%");
            //msg28 = bc.createAttrMessage("STATION_STRING", "ActualCT:12");
            //msg29 = bc.createAttrMessage("location1", "W23/24");
            //msg30 = bc.createAttrMessage("location2", "W23/24");
            //msg31 = bc.createAttrMessage("location3", "W23/24");
            //msg32 = bc.createAttrMessage("location4", "W23/24");
            //msg33 = bc.createAttrMessage("location5", "W23/24");
            //msg34 = bc.createAttrMessage("location6", "W23/24");
            msg35 = bc.createAttrMessage("tossing_item", "location1 CCD NG");

            msg36 = bc.createPDataMessage("weld_wait_ct", "2", "", "", "s");
            msg37 = bc.createPDataMessage("actual_weld_ct", "6", "", "", "s");
            //msg38 = bc.createPDataMessage("precitec_value", "0", "", "", "%");
            msg39 = bc.createPDataMessage("power_ll", "20", "", "", "W");
            msg40 = bc.createPDataMessage("power_ul", "1500", "", "", "W");
            //msg41 = bc.createPDataMessage("pattern_type", "1", "", "", "");
            //msg42 = bc.createPDataMessage("spot_size", "0.45", "", "", "mm");
            //msg43 = bc.createPDataMessage("hatch", "0.04", "", "", "mm");
            //msg44 = bc.createPDataMessage("swing_amplitude", "0.02", "", "", "mm");
            //msg45 = bc.createPDataMessage("swing_freq", "10000", "", "", "Hz");

            msg46 = bc.createPDataMessage("location1_layer1_laser_power", "20", "", "", "W");
            msg47 = bc.createPDataMessage("location1_layer1_frequency", "900", "", "", "KHz");
            msg48 = bc.createPDataMessage("location1_layer1_waveform", "1", "", "", "");
            msg49 = bc.createPDataMessage("location1_layer1_pulse_energy", "3", "", "", "J");
            msg50 = bc.createPDataMessage("location1_layer1_laser_speed", "100", "", "", "mm/s");
            msg51 = bc.createPDataMessage("location1_layer1_jump_speed", "2000", "", "", "mm/s");
            msg52 = bc.createPDataMessage("location1_layer1_jump_delay", "10000", "", "", "us");
            msg53 = bc.createPDataMessage("location1_layer1_scanner_delay", "10", "", "", "us");

            msg54 = bc.createPDataMessage("location2_layer1_laser_power", "20", "", "", "W");
            msg55 = bc.createPDataMessage("location2_layer1_frequency", "900", "", "", "KHz");
            msg56 = bc.createPDataMessage("location2_layer1_waveform", "1", "", "", "");
            msg57 = bc.createPDataMessage("location2_layer1_pulse_energy", "3", "", "", "J");
            msg58 = bc.createPDataMessage("location2_layer1_laser_speed", "100", "", "", "mm/s");
            msg59 = bc.createPDataMessage("location2_layer1_jump_speed", "2000", "", "", "mm/s");
            msg60 = bc.createPDataMessage("location2_layer1_jump_delay", "10000", "", "", "us");
            msg61 = bc.createPDataMessage("location2_layer1_scanner_delay", "10", "", "", "us");

            msg62 = bc.createPDataMessage("location3_layer1_laser_power", "20", "", "", "W");
            msg63 = bc.createPDataMessage("location3_layer1_frequency", "900", "", "", "KHz");
            msg64 = bc.createPDataMessage("location3_layer1_waveform", "1", "", "", "");
            msg65 = bc.createPDataMessage("location3_layer1_pulse_energy", "3", "", "", "J");
            msg66 = bc.createPDataMessage("location3_layer1_laser_speed", "100", "", "", "mm/s");
            msg67 = bc.createPDataMessage("location3_layer1_jump_speed", "2000", "", "", "mm/s");
            msg68 = bc.createPDataMessage("location3_layer1_jump_delay", "10000", "", "", "us");
            msg69 = bc.createPDataMessage("location3_layer1_scanner_delay", "10", "", "", "us");

            msg70 = bc.createPDataMessage("location4_layer1_laser_power", "20", "", "", "W");
            msg71 = bc.createPDataMessage("location4_layer1_frequency", "900", "", "", "KHz");
            msg72 = bc.createPDataMessage("location4_layer1_waveform", "1", "", "", "");
            msg73 = bc.createPDataMessage("location4_layer1_pulse_energy", "3", "", "", "J");
            msg74 = bc.createPDataMessage("location4_layer1_laser_speed", "100", "", "", "mm/s");
            msg75 = bc.createPDataMessage("location4_layer1_jump_speed", "2000", "", "", "mm/s");
            msg76 = bc.createPDataMessage("location4_layer1_jump_delay", "10000", "", "", "us");
            msg77 = bc.createPDataMessage("location4_layer1_scanner_delay", "10", "", "", "us");

            msg78 = bc.createPDataMessage("location5_layer1_laser_power", "20", "", "", "W");
            msg79 = bc.createPDataMessage("location5_layer1_frequency", "900", "", "", "KHz");
            msg80 = bc.createPDataMessage("location5_layer1_waveform", "1", "", "", "");
            msg81 = bc.createPDataMessage("location5_layer1_pulse_energy", "3", "", "", "J");
            msg82 = bc.createPDataMessage("location5_layer1_laser_speed", "100", "", "", "mm/s");
            msg83 = bc.createPDataMessage("location5_layer1_jump_speed", "2000", "", "", "mm/s");
            msg84 = bc.createPDataMessage("location5_layer1_jump_delay", "10000", "", "", "us");
            msg85 = bc.createPDataMessage("location5_layer1_scanner_delay", "10", "", "", "us");

            #region 2021old
            //msg86 = bc.createPDataMessage("location5_layer1_laser_power", "20", "", "", "W");
            //msg87 = bc.createPDataMessage("location5_layer1_frequency", "900", "", "", "KHz");
            //msg88 = bc.createPDataMessage("location5_layer1_waveform", "1", "", "", "");
            //msg89 = bc.createPDataMessage("location5_layer1_pulse_energy", "3", "", "", "J");
            //msg90 = bc.createPDataMessage("location5_layer1_laser_speed", "100", "", "", "mm/s");
            //msg91 = bc.createPDataMessage("location5_layer1_jump_speed", "2000", "", "", "mm/s");
            //msg92 = bc.createPDataMessage("location5_layer1_jump_delay", "10000", "", "", "us");
            //msg93 = bc.createPDataMessage("location5_layer1_scanner_delay", "10", "", "", "us");

            //msg94 = bc.createPDataMessage("location5_layer2_laser_power", "20", "", "", "W");
            //msg95 = bc.createPDataMessage("location5_layer2_frequency", "900", "", "", "KHz");
            //msg96 = bc.createPDataMessage("location5_layer2_waveform", "1", "", "", "");
            //msg97 = bc.createPDataMessage("location5_layer2_pulse_energy", "3", "", "", "J");
            //msg98 = bc.createPDataMessage("location5_layer2_laser_speed", "100", "", "", "mm/s");
            //msg99 = bc.createPDataMessage("location5_layer2_jump_speed", "2000", "", "", "mm/s");
            //msg100 = bc.createPDataMessage("location5_layer2_jump_delay", "10000", "", "", "us");
            //msg101 = bc.createPDataMessage("location5_layer2_scanner_delay", "10", "", "", "us");

            //msg102 = bc.createPDataMessage("location6_layer1_laser_power", "20", "", "", "W");
            //msg103 = bc.createPDataMessage("location6_layer1_frequency", "900", "", "", "KHz");
            //msg104 = bc.createPDataMessage("location6_layer1_waveform", "1", "", "", "");
            //msg105 = bc.createPDataMessage("location6_layer1_pulse_energy", "3", "", "", "J");
            //msg106 = bc.createPDataMessage("location6_layer1_laser_speed", "100", "", "", "mm/s");
            //msg107 = bc.createPDataMessage("location6_layer1_jump_speed", "2000", "", "", "mm/s");
            //msg108 = bc.createPDataMessage("location6_layer1_jump_delay", "10000", "", "", "us");
            //msg109 = bc.createPDataMessage("location6_layer1_scanner_delay", "10", "", "", "us");
            //msg110 = bc.createPDataMessage("location6_layer2_laser_power", "20", "", "", "W");
            //msg111 = bc.createPDataMessage("location6_layer2_frequency", "900", "", "", "KHz");
            //msg112 = bc.createPDataMessage("location6_layer2_waveform", "1", "", "", "");
            //msg113 = bc.createPDataMessage("location6_layer2_pulse_energy", "3", "", "", "J");
            //msg114 = bc.createPDataMessage("location6_layer2_laser_speed", "100", "", "", "mm/s");
            //msg115 = bc.createPDataMessage("location6_layer2_jump_speed", "2000", "", "", "mm/s");
            //msg116 = bc.createPDataMessage("location6_layer2_jump_delay", "10000", "", "", "us");
            //msg117 = bc.createPDataMessage("location6_layer2_scanner_delay", "10", "", "", "us");

            //msg118 = bc.createPDataMessage("MODEL_STATUS_CODE", "1", "", "", "/");
            //msg119 = bc.createPDataMessage("MODEL_RESPONSE_TIME", "100", "", "", "ms");
            //msg120 = bc.createPDataMessage("MODEL_PULL_ORT_FORCE_ML1", "36.752586", "", "", "/");
            //msg121 = bc.createPDataMessage("MODEL_SHEAR_ORT_FORCE_ML1", "36.752586", "", "", "/");
            //msg122 = bc.createPDataMessage("MODEL_DECISION_ML1", "1", "", "", "/");
            //msg123 = bc.createPDataMessage("MODEL_PULL_ORT_FORCE_ML2", "36.752586", "", "", "/");
            //msg124 = bc.createPDataMessage("MODEL_SHEAR_ORT_FORCE_ML2", "36.752586", "", "", "/");
            //msg125 = bc.createPDataMessage("MODEL_DECISION_ML2", "1", "", "", "/");
            //msg126 = bc.createPDataMessage("MODEL_PULL_ORT_FORCE_ML3", "36.752586", "", "", "/");
            //msg127 = bc.createPDataMessage("MODEL_SHEAR_ORT_FORCE_ML3", "36.752586", "", "", "/");
            //msg128 = bc.createPDataMessage("MODEL_DECISION_ML3", "1", "", "", "/");
            //msg129 = bc.createPDataMessage("MODEL_PULL_ORT_FORCE_UA1", "36.752586", "", "", "/");
            //msg130 = bc.createPDataMessage("MODEL_SHEAR_ORT_FORCE_UA1", "36.752586", "", "", "/");
            //msg131 = bc.createPDataMessage("MODEL_DECISION_UA1", "1", "", "", "/");
            //msg132 = bc.createPDataMessage("MODEL_PULL_ORT_FORCE_UA5", "36.752586", "", "", "/");
            //msg133 = bc.createPDataMessage("MODEL_SHEAR_ORT_FORCE_UA5", "36.752586", "", "", "/");
            //msg134 = bc.createPDataMessage("MODEL_DECISION_UA5", "1", "", "", "/");
            //msg135 = bc.createPDataMessage("MODEL_PULL_ORT_FORCE_UA6", "36.752586", "", "", "/");
            //msg136 = bc.createPDataMessage("MODEL_SHEAR_ORT_FORCE_UA6", "36.752586", "", "", "/");
            //msg137 = bc.createPDataMessage("MODEL_DECISION_UA6", "1", "", "", "/");
            //msg138 = bc.createPDataMessage("MODEL_PULL_ORT_FORCE_LA1", "36.752586", "", "", "/");
            //msg139 = bc.createPDataMessage("MODEL_SHEAR_ORT_FORCE_LA1", "36.752586", "", "", "/");
            //msg140 = bc.createPDataMessage("MODEL_DECISION_LA1", "1", "", "", "/");
            //msg141 = bc.createPDataMessage("MODEL_PULL_ORT_FORCE_LA8", "36.752586", "", "", "/");
            //msg142 = bc.createPDataMessage("MODEL_SHEAR_ORT_FORCE_LA8", "36.752586", "", "", "/");
            //msg143 = bc.createPDataMessage("MODEL_DECISION_LA8", "1", "", "", "/");
            //msg144 = bc.createPDataMessage("MODEL_PULL_ORT_FORCE_LA14", "36.752586", "", "", "/");
            //msg145 = bc.createPDataMessage("MODEL_SHEAR_ORT_FORCE_LA14", "36.752586", "", "", "/");
            //msg146 = bc.createPDataMessage("MODEL_DECISION_LA14", "1", "", "", "/");
            //msg147 = bc.createPDataMessage("MODEL_PULL_ORT_FORCE_LA6", "36.752586", "", "", "/");
            //msg148 = bc.createPDataMessage("MODEL_SHEAR_ORT_FORCE_LA6", "36.752586", "", "", "/");
            //msg149 = bc.createPDataMessage("MODEL_DECISION_LA6", "1", "", "", "/");
            //msg150 = bc.createPDataMessage("MODEL_PULL_ORT_FORCE_LA10", "36.752586", "", "", "/");
            //msg151 = bc.createPDataMessage("MODEL_SHEAR_ORT_FORCE_LA10", "36.752586", "", "", "/");
            //msg152 = bc.createPDataMessage("MODEL_DECISION_LA10", "1", "", "", "/"); 
            #endregion

            msg999 = bc.createSubmitMessage("220224A");
            msg1000 = "begin\n" + msg + msg1 + msg2 + msg3 + msg4 + msg5 + msg6 + msg7 + msg8 + msg9 + msg10
                + msg11 + msg12 + msg13 + msg14 + msg15 + msg16 + msg17 + msg18 + msg19 + msg20
                + msg21 + msg22 + msg23 + msg24 + msg25 + msg26 + msg27 + msg28 + msg29 + msg30
                + msg31 + msg32 + msg33 + msg34 + msg35 + msg36 + msg37 + msg38 + msg39 + msg40
                + msg41 + msg42 + msg43 + msg44 + msg45 + msg46 + msg47 + msg48 + msg49 + msg50
                + msg51 + msg52 + msg53 + msg54 + msg55 + msg56 + msg57 + msg58 + msg59 + msg60
                + msg61 + msg62 + msg63 + msg64 + msg65 + msg66 + msg67 + msg68 + msg69 + msg70
                + msg71 + msg72 + msg73 + msg74 + msg75 + msg76 + msg77 + msg78 + msg79 + msg80
                + msg81 + msg82 + msg83 + msg84 + msg85 + msg86 + msg87 + msg88 + msg89 + msg90
                + msg91 + msg92 + msg93 + msg94 + msg95 + msg96 + msg97 + msg98 + msg99 + msg100
                + msg101 + msg102 + msg103 + msg104 + msg105 + msg106 + msg107 + msg108 + msg109 + msg110
                + msg111 + msg112 + msg113 + msg114 + msg115 + msg116 + msg117
                //+ msg118 + msg119 + msg120 + msg121 + msg122 + msg123 + msg124 + msg125 + msg126 + msg127
                //+ msg128 + msg129 + msg130 + msg131 + msg132 + msg133 + msg134 + msg135 + msg136 + msg137
                //+ msg138 + msg139 + msg140 + msg141 + msg142 + msg143 + msg144 + msg145 + msg146 + msg147
                //+ msg148 + msg149 + msg150 + msg151 + msg152
                + msg999 + "end\n";
            string reply = "";
            richTextBox6.AppendText(msg1000);
            bc.sendMessage(msg1000);
            Log.WriteLog(msg1000);
            reply = bc.getReply();
            richTextBox5.AppendText(reply);
            Log.WriteLog(reply);
        }

        public string GetIp()//获取本机IP
        {
            string hostName = Dns.GetHostName();   //获取本机名
            IPHostEntry localhost = Dns.GetHostByName(hostName);    //方法已过期，可以获取IPv4的地址
            //IPHostEntry localhost = Dns.GetHostEntry(hostName);   //获取IPv6地址
            IPAddress localaddr = localhost.AddressList[0];
            return localaddr.ToString();
        }
        public string GetMac()//获取本机MAC地址
        {
            string strMac = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    strMac = mo["MacAddress"].ToString();
                    mo.Dispose();
                    break;
                }
            }
            moc = null;
            mc = null;
            return strMac;
        }


        public string HttpPostData(string url, int timeOut, string fileKeyName,
                   string filePath, NameValueCollection stringDict)
        {
            string responseContent;
            var memStream = new MemoryStream();
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            // 边界符
            var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
            // 边界符
            var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            // 最后的结束符
            var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");

            // 设置属性
            webRequest.Method = "POST";
            webRequest.Timeout = timeOut;
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;

            // 写入文件
            const string filePartHeader =
        "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
        "Content-Type: application/octet-stream\r\n\r\n";
            var header = string.Format(filePartHeader, fileKeyName, filePath);
            var headerbytes = Encoding.UTF8.GetBytes(header);

            memStream.Write(beginBoundary, 0, beginBoundary.Length);
            memStream.Write(headerbytes, 0, headerbytes.Length);

            var buffer = new byte[1024];
            int bytesRead; // =0

            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                memStream.Write(buffer, 0, bytesRead);
            }

            // 写入字符串的Key
            var stringKeyHeader = "\r\n--" + boundary +
                 "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                 "\r\n\r\n{1}\r\n";

            foreach (byte[] formitembytes in from string key in stringDict.Keys
                                             select string.Format(stringKeyHeader, key, stringDict[key])
                                                 into formitem
                                             select Encoding.UTF8.GetBytes(formitem))
            {
                memStream.Write(formitembytes, 0, formitembytes.Length);
            }

            // 写入最后的结束边界符
            memStream.Write(endBoundary, 0, endBoundary.Length);

            webRequest.ContentLength = memStream.Length;

            var requestStream = webRequest.GetRequestStream();

            memStream.Position = 0;
            var tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();

            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();

            var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

            using (var httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream(),
                                    Encoding.GetEncoding("utf-8")))
            {
                responseContent = httpStreamReader.ReadToEnd();
            }

            fileStream.Close();
            httpWebResponse.Close();
            webRequest.Abort();

            return responseContent;
        }

        /// <summary>
        /// 生成MD5码版本号：读取目前软件执行档然后产生MD5码，作为软件版本
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        private string GetFileMd5Code(string filePath)
        {
            StringBuilder builder = new StringBuilder();
            if (filePath != "")
            {
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    File.Copy(filePath, filePath + "e");//复制一份，防止占用
                    //利用复制的执行档建立MD5码
                    using (FileStream fs = new FileStream(filePath + "e", FileMode.Open))
                    {
                        byte[] bt = md5.ComputeHash(fs);
                        for (int i = 0; i < bt.Length; i++)
                        {
                            builder.Append(bt[i].ToString("x2"));
                        }
                    }
                    File.Delete(filePath + "e");
                }
            }
            return builder.ToString();

        }

    }
}
