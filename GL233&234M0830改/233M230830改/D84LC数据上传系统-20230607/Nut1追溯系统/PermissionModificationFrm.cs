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
    public partial class PermissionModificationFrm : Form
    {
        string _str = null;
        public PermissionModificationFrm(string str)
        {
            InitializeComponent();
            _str = str;
        }

        private void PermissionModificationFrm_Load(object sender, EventArgs e)
        {
            if (_str != null)
            {
                string[] Permission = _str.Split(',');
                txt_Name.Text = Permission[0];
                txt_ID.Text = Permission[1];
                txt_pwd.Text = Permission[3];
                for (int i = 0; i < cbo_Login.Items.Count; i++)
                {
                    if (cbo_Login.Items[i].ToString() == Permission[2])
                    {
                        cbo_Login.SelectedIndex = i;
                    }
                }
            }
        }

        private void btn_ChangePwd_Click(object sender, EventArgs e)
        {

        }
    }
}
