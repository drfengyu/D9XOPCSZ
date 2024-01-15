using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 卓汇数据追溯系统
{
    public partial class Frm_Update : Form
    {
        string Conn = "provider=microsoft.jet.oledb.4.0;data source=mydata.mdb";
        //注意，这里的row是来自form3;

        DataRow row11;
        public Frm_Update(DataRow row)
        {
            InitializeComponent();
            row11 = row;
        }

        private void btn_confirmation_Click(object sender, EventArgs e)
        {
            SQLServer server = new SQLServer();
            string MyString = comboBox1.Text;
            if (MyString.Trim() != String.Empty)
            {
                Log.WriteCSV(txt类别.Text + "," + txt备件名.Text + "," + txt型号.Text + "," + txt寿命.Text + "," + txt使用.Text + "," + txt上次更换时间.Text + "," + DateTime.Now.ToString() + ", " + comboBox1.Text + "," + txt处理生技.Text, System.AppDomain.CurrentDomain.BaseDirectory + "备件更换记录\\");

                string insStrRefresh1 = "update SparePartData set 实际使用次数=" + 0 + " where ID=" + txt编号.Text + "";
                int d1 = server.ExecuteUpdate(insStrRefresh1);
                string insStrRefresh2 = "update SparePartData set 上次更换时间='" + DateTime.Now.ToString() + "' where ID=" + txt编号.Text + "";
                int d2 = server.ExecuteUpdate(insStrRefresh2);
                string insStrRefresh3 = "update SparePartData set 部件寿命残值='1'" + " where ID=" + txt编号.Text + "";
                int d3 = server.ExecuteUpdate(insStrRefresh3);
                if (d1 > 0 && d2 > 0 && d3 > 0)
                {
                    MessageBox.Show("更换完成!", "来自系统的消息");
                }
                else
                {
                    MessageBox.Show("更换失败!请重启软件或联系软件工程师!", "来自系统的消息");
                }
            }
            if (MyString.Trim() == String.Empty)
            {
                MessageBox.Show("未写明更换原因，请写入", "来自系统的消息");
            }
        }

        private void txt寿命_时间_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == '.' || e.KeyChar == '-' || e.KeyChar == '\b') //'\b'为回退字符
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("只能输入数字或者小数点!", "来自系统的消息");
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
