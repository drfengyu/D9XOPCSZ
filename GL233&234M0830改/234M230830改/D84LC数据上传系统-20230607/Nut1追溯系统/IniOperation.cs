using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace 卓汇数据追溯系统
{
    class IniOperation
    {
        // 声明INI文件的写操作函数 WritePrivateProfileString()
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        // 声明INI文件的读操作函数 GetPrivateProfileString()
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private string sPath = null;

        public IniOperation(string fileName)
        {
            this.sPath = fileName;
        }

        public void WriteValue(string section, string key, string value)
        {
            // section=配置节，key=键名，value=键值，path=路径
            WritePrivateProfileString(section, key, value, sPath);

        }

        public string ReadValue(string section, string key)
        {
            if (!CheckIniFileExist())
            {
                throw new ApplicationException("文件不存在");
            }
            // 每次从ini中读取多少字节
            System.Text.StringBuilder temp = new System.Text.StringBuilder(255);
            //System.Text.StringBuilder temp = new System.Text.StringBuilder(500);

            // section=配置节，key=键名，temp=上面，path=路径
            GetPrivateProfileString(section, key, "", temp, 255, sPath);

            return temp.ToString();
        }

        private bool CheckIniFileExist()
        {
            bool ret = false;
            ret = File.Exists(sPath);
            return ret;
        }
    }
}
