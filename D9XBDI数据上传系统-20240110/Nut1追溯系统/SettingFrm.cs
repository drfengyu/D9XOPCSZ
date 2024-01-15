using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 卓汇数据追溯系统
{
    public partial class SettingFrm : Form
    {
        private MainFrm _mainparent;
        string dataPath = AppDomain.CurrentDomain.BaseDirectory + "setting.ini";
        IniProductFile inidata;
        public SettingFrm(MainFrm mdiParent)
        {
            InitializeComponent();
            _mainparent = mdiParent;
            inidata = new IniProductFile(dataPath);
        }

        private void SettingFrm_Load(object sender, EventArgs e)
        {
            try
            {
                txt_MESURL.Text = inidata.productconfig.URL;
                txt_Site.Text = inidata.productconfig.site;
                txt_Resource.Text = inidata.productconfig.resource;
                txt_Operation.Text = inidata.productconfig.operation;

                txt_power.Text = inidata.productconfig.power;
                txt_frequency.Text = inidata.productconfig.frequency;
                txt_waveform.Text = inidata.productconfig.waveform;
                txt_laser_speed.Text = inidata.productconfig.laser_speed;
                txt_jump_speed.Text = inidata.productconfig.jump_speed;
                txt_jump_delay.Text = inidata.productconfig.jump_delay;
                txt_position_delay.Text = inidata.productconfig.position_delay;
                txt_pulse_profile.Text = inidata.productconfig.pulse_profile;
                txt_laser_height.Text = inidata.productconfig.laser_height;

                //Global.power = inidata.productconfig.power;
                //Global.frequency = inidata.productconfig.frequency;
                //Global.waveform = inidata.productconfig.waveform;
                //Global.laser_speed = inidata.productconfig.laser_speed;
                //Global.jump_speed = inidata.productconfig.jump_speed;
                //Global.jump_delay = inidata.productconfig.jump_delay;
                //Global.position_delay = inidata.productconfig.position_delay;
                //Global.pulse_profile = inidata.productconfig.pulse_profile;
                //Global.laser_height = inidata.productconfig.laser_height;




                //tabControl1.TabPages.Remove(tabPage3);
                //tabControl1.TabPages.Remove(tabPage1);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)//自定义绘制Tab标题
        {
            string text = ((TabControl)sender).TabPages[e.Index].Text;
            //标签背景填充颜色
            SolidBrush BackBrush = new SolidBrush(Color.Gray);
            SolidBrush brush = new SolidBrush(Color.Black);
            StringFormat sf = new StringFormat();
            //设置文字对齐方式
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            if (e.Index == this.tabControl1.SelectedIndex)//当前Tab页的样式
            {
                BackBrush = new SolidBrush(Color.DarkSeaGreen);
            }
            //绘制标签头背景颜色
            e.Graphics.FillRectangle(BackBrush, e.Bounds);
            //绘制标签头文字
            //e.Graphics.DrawString(text, SystemInformation.MenuFont, brush, e.Bounds, sf);
            e.Graphics.DrawString(text, new Font("微软雅黑", 15), brush, e.Bounds, sf);
        }


        //public static List<string> GetLocalIPv4AddressList()
        //{
        //    List<string> ipAddressList = new List<string>();
        //    try
        //    {
        //        IPHostEntry ipHost = Dns.Resolve(Dns.GetHostName());
        //        foreach (IPAddress ipAddress in ipHost.AddressList)
        //        {
        //            if (!ipAddressList.Contains(ipAddress.ToString()))
        //            {
        //                ipAddressList.Add(ipAddress.ToString());
        //            }
        //        }
        //    }
        //    catch
        //    { }
        //    return ipAddressList;
        //}

        public static List<string> GetMacAddress()
        {
            List<string> MacAddressList = new List<string>();
            try
            {
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        MacAddressList.Add(mo["MacAddress"].ToString());
                    }
                }
                moc = null;
                mc = null;
            }
            catch
            { }
            return MacAddressList;
        }

        private void btn_CommunicationSave_Click(object sender, EventArgs e)
        {
            inidata.productconfig.URL = txt_MESURL.Text;
            inidata.productconfig.site = txt_Site.Text;
            inidata.productconfig.resource = txt_Resource.Text;
            inidata.productconfig.operation = txt_Operation.Text;

            inidata.WriteProductConfigSection();
            MessageBox.Show("保存成功, 请重启软件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_savelaser_Click(object sender, EventArgs e)
        {
            inidata.productconfig.power = txt_power.Text;
            inidata.productconfig.frequency = txt_frequency.Text;
            inidata.productconfig.waveform = txt_waveform.Text;
            inidata.productconfig.laser_speed = txt_laser_speed.Text;
            inidata.productconfig.jump_speed = txt_jump_speed.Text;
            inidata.productconfig.jump_delay = txt_jump_delay.Text;
            inidata.productconfig.position_delay = txt_position_delay.Text;
            inidata.productconfig.pulse_profile = txt_pulse_profile.Text;
            inidata.productconfig.laser_height = txt_laser_height.Text;

            inidata.WriteProductConfigSection();
            MessageBox.Show("保存成功, 请重启软件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
    }
}
