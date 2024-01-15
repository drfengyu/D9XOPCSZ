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
    public partial class UserLoginFrm : Form
    {
        private MainFrm _mainparent;
        private delegate void LabelText(Label lb, string bl);
        private delegate void Labelcolor(Label lb, Color color, string bl);
        private delegate void ShowTxt(TextBox tb, string txt);
        private delegate void cboSelect(int i);
        public UserLoginFrm(MainFrm mdiParent)
        {
            InitializeComponent();
            _mainparent = mdiParent;
            //Global.inidata = new IniProductFile(@"D:\ZHH\Upload\setting.ini");
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

        private void btn_Login_Click(object sender, EventArgs e)//用户登录
        {
            if (cbo_Login.SelectedIndex == 0)
            {
                if (txt_pwd.Text == Global.Technician_pwd)
                {
                    lbl_LoginLevel.Text = "技术员";
                    Global.Login = Global.LoginLevel.Technician;
                    labelcolor(lbl_LoginMes, Color.LimeGreen, "技术员登录成功");
                }
                else
                {
                    labelcolor(lbl_LoginMes, Color.Red, "密码错误,请重新输入！");
                    UiText(txt_pwd, "");
                }
            }
            else
            {
                if (txt_pwd.Text == Global.Administrator_pwd)
                {
                    lbl_LoginLevel.Text = "工程师";
                    Global.Login = Global.LoginLevel.Administrator;
                    labelcolor(lbl_LoginMes, Color.LimeGreen, "工程师登录成功");
                }
                else
                {
                    labelcolor(lbl_LoginMes, Color.Red, "密码错误,请重新输入！");
                    UiText(txt_pwd, "");
                }
            }
        }

        public void labelText(Label lb, string txt)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new LabelText(labelText), new object[] { lb, txt });
                return;
            }
            lb.Text = txt;
        }

        public void labelcolor(Label lb, Color color, string str)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Labelcolor(labelcolor), new object[] { lb, color, str });
                return;
            }
            lb.BackColor = color;
            lb.Text = str;
        }

        public void UiText(TextBox tb, string str)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowTxt(UiText), new object[] { tb, str });
                return;
            }
            tb.Text = str;
        }

        private void chk_DisplayPwd_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_DisplayPwd.Checked)
            {
                txt_pwd.PasswordChar = new char();
            }
            else
            {
                txt_pwd.PasswordChar = '*';
            }
        }

        private void chk_DisplayOldPwd_CheckedChanged(object sender, EventArgs e)
        {
            //if (chk_DisplayOldPwd.Checked)
            //{
            //    txt_oldPwd.PasswordChar = new char();
            //}
            //else
            //{
            //    txt_oldPwd.PasswordChar = '*';
            //}
        }

        private void chk_DisplayNewPwd_CheckedChanged(object sender, EventArgs e)
        {
            //if (chk_DisplayNewPwd.Checked)
            //{
            //    txt_NewPwd.PasswordChar = new char();
            //}
            //else
            //{
            //    txt_NewPwd.PasswordChar = '*';
            //}
        }

        private void cbo_Login_SelectedIndexChanged(object sender, EventArgs e)
        {
            UiText(txt_pwd, "");
            txt_pwd.Focus();
        }

        private void btn_ChangePwd_Click(object sender, EventArgs e)
        {
            if (Global.Login == Global.LoginLevel.Operator)
            {
                //if (txt_oldPwd.Text == Global.Operator_pwd)
                //{
                //    Global.inidata.productconfig.Operator_pwd = txt_NewPwd.Text;
                //    Global.inidata.WriteProductConfigSection();
                //    Global.Operator_pwd = Global.inidata.productconfig.Operator_pwd;
                //    labelcolor(lbl_ChangePwdMes, Color.LimeGreen, "操作员密码更改成功");
                //    UiText(txt_oldPwd, "");
                //    UiText(txt_NewPwd, "");
                //}
                //else
                //{
                //    labelcolor(lbl_ChangePwdMes, Color.Red, "旧密码不正确,请重新输入!");
                //    UiText(txt_oldPwd, "");
                //    UiText(txt_NewPwd, "");
                //    txt_oldPwd.Focus();
                //}
            }
            else if (Global.Login == Global.LoginLevel.Technician)
            {
                //if (txt_oldPwd.Text == Global.Technician_pwd)
                //{
                //    Global.inidata.productconfig.Technician_pwd = txt_NewPwd.Text;
                //    Global.inidata.WriteProductConfigSection();
                //    Global.Technician_pwd = Global.inidata.productconfig.Technician_pwd;
                //    labelcolor(lbl_ChangePwdMes, Color.LimeGreen, "技术员密码更改成功");
                //    UiText(txt_oldPwd, "");
                //    UiText(txt_NewPwd, "");
                //}
                //else
                //{
                //    labelcolor(lbl_ChangePwdMes, Color.Red, "旧密码不正确,请重新输入!");
                //    UiText(txt_oldPwd, "");
                //    UiText(txt_NewPwd, "");
                //    txt_oldPwd.Focus();
                //}
            }
            else if (Global.Login == Global.LoginLevel.Administrator)
            {
                //if (txt_oldPwd.Text == Global.Administrator_pwd)
                //{
                //    Global.inidata.productconfig.Administrator_pwd = txt_NewPwd.Text;
                //    Global.inidata.WriteProductConfigSection();
                //    Global.Administrator_pwd = Global.inidata.productconfig.Administrator_pwd;
                //    labelcolor(lbl_ChangePwdMes, Color.LimeGreen, "管理员密码更改成功");
                //    UiText(txt_oldPwd, "");
                //    UiText(txt_NewPwd, "");
                //}
                //else
                //{
                //    labelcolor(lbl_ChangePwdMes, Color.Red, "旧密码不正确,请重新输入!");
                //    UiText(txt_oldPwd, "");
                //    UiText(txt_NewPwd, "");
                //    txt_oldPwd.Focus();
                //}
            }
        }

        private void UserLoginFrm_Load(object sender, EventArgs e)
        {
            if (Global.Login == Global.LoginLevel.Operator)
            {
                lbl_LoginLevel.Text = "操作员";
                //cbo_Login.SelectedIndex = 0;
            }
            if (Global.Login == Global.LoginLevel.Technician)
            {
                lbl_LoginLevel.Text = "技术员";
                //cbo_Login.SelectedIndex = 1;
            }
            if (Global.Login == Global.LoginLevel.Administrator)
            {
                lbl_LoginLevel.Text = "工程师";
                //cbo_Login.SelectedIndex = 2;
            }
        }

        public void ComboBoxSelect(int i)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new cboSelect(ComboBoxSelect), new object[] { i });
                return;
            }
            cbo_Login.SelectedIndex = i;
            lbl_LoginLevel.Text = "技术员";
            lbl_LoginMes.BackColor = Color.Transparent;
            lbl_LoginMes.Text = "";
        }

        private void btn_PermissionModify_Click(object sender, EventArgs e)
        {

        }

        private void btn_PermissionDel_Click(object sender, EventArgs e)
        {

        }
    }
}
