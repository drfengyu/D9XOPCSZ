using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 卓汇数据追溯系统
{
    public partial class PassWordFrm : Form
    {
        bool Permission = false;
        public delegate void PermissionEventHandler(int i);//声明委托
        public event PermissionEventHandler PermissionIndex;
        public PassWordFrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtPassWord.Text == Global.Administrator_pwd)
            {
                Permission = true;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                Permission = false;
                MessageBox.Show("密码输入错误！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PassWordFrm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtPassWord;
            //弹出窗体居中
            int x = (System.Windows.Forms.SystemInformation.WorkingArea.Width - this.Size.Width * 2);
            int y = this.Size.Height;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = (Point)new Size(x, y);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtPassWord.PasswordChar = new char();
            }
            else
            {
                txtPassWord.PasswordChar = '*';
            }
        }
    }
}
