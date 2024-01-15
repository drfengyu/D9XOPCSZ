using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace 卓汇数据追溯系统
{
    class ClearOverdueFile
    {
        string clearOverdueFilePath = string.Empty;
        string fileLastWriteTime = string.Empty;
        string targetFilePath = string.Empty;
        int overdue = 7;


        public ClearOverdueFile(string path, int overduTime = 14)
        {
            clearOverdueFilePath = path;
            overdue = overduTime;
        }

        public void FileDeal()
        {
            try
            {
                DirectoryInfo theFolder = new DirectoryInfo(clearOverdueFilePath);		//建立文件夹内对象
                foreach (FileSystemInfo nextFolder in theFolder.GetFileSystemInfos())
                {
                    if (nextFolder.ToString() != "RECYCLE.BIN" && nextFolder.ToString() != "System Volume Information"
                        && nextFolder.ToString() != "Buffer")    //排除系统文件
                    {
                        targetFilePath = clearOverdueFilePath + "\\" + nextFolder;              //建立完整路径
                        System.IO.FileInfo file = new System.IO.FileInfo(targetFilePath);       //获取文件对象
                        fileLastWriteTime = file.LastWriteTime.ToOADate().ToString();           //获取修改时间
                        if (double.Parse(DateTime.Now.ToOADate().ToString()) - double.Parse(fileLastWriteTime) >= overdue)     //判断是否超时
                        {
                            if (File.Exists(targetFilePath))
                            {
                                File.Delete(targetFilePath);      //删除
                            }
                            else if (Directory.Exists(targetFilePath))
                            {
                                Directory.Delete(targetFilePath, true);
                            }
                        }
                    }
                }
            }
            catch
            {
                //  MessageBox.Show("删除文件出现错误！"+e.ToString());
            }
        }
    }
}
