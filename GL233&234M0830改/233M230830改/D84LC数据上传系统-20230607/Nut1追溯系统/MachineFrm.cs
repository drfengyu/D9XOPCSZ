using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 卓汇数据追溯系统
{
    public partial class MachineFrm : Form
    {
        private MainFrm _mainparent;
        //string dataPath = System.AppDomain.CurrentDomain.BaseDirectory + "setting.ini";
        public MachineFrm(MainFrm mdiParent)
        {
            InitializeComponent();
            _mainparent = mdiParent;
        }

        private void MachineFrm_Load(object sender, EventArgs e)
        {
            //if (File.Exists(dataPath))
            //{
            //    Global.inidata = new IniProductFile(dataPath);
            //}
            //lb_SoftWareVersion.Text = Global.inidata.productconfig.Sw_version;
        }
    }
}
