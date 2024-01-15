using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace 通讯接口测试_JGP__20200415
{
    public class Txt
    {
        public static bool FixtureTimeOut(string Fixture)
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "PIS超时保养治具\\" + "超时保养治具" + ".txt";
            StreamReader sR = new StreamReader(path);
            string nextLine;
            while ((nextLine = sR.ReadLine()) != null)
            {
                if (nextLine.Contains(Fixture))
                {
                    return false;
                }
            }
            sR.Close();
            return true;
        }

        public static void WriteLine(string[] FixtureID)
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "PIS超时保养治具\\";
            string PathTxt = path + "超时保养治具"+ ".txt";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            System.IO.File.Delete(PathTxt);
            using (StreamWriter sW = new StreamWriter(PathTxt, true, Encoding.Default))
            {
                for (int i = 0; i < FixtureID.Length; i++)
                {
                    sW.WriteLine(FixtureID[i]);
                }
            }
        }
    }
}
