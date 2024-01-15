using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 卓汇数据追溯系统
{
    public partial class HomeFrm : Form
    {
        private MainFrm _mainparent;
        delegate void AddItemToListBoxDelegate(string str, string Name);
        delegate void ShowDataGridView(DataGridView dgv, DataTable dt, int index);
        delegate void DGVAutoSize(DataGridView dgv);
        private delegate void UpdateDataGridView1();
        private delegate void Labelvision(string bl, string Name);
        private delegate void Labelcolor(Color color, string bl, string Name);
        private delegate void ShowTxt(string txt, string Name);
        string LogPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\日志文件\\";
        //string Conn = "provider=microsoft.jet.oledb.4.0;data source=mydata.mdb;";
        SQLServer server = new SQLServer();
        List<Control> List_Control = new List<Control>();
        int rth_Number = 0;
        public HomeFrm(MainFrm mdiParent)
        {
            InitializeComponent();
            _mainparent = mdiParent;
        }

        private void DGV_Refrush()
        {
            //this.dgv_SpareParts.Refresh();
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

        #region 备件管控
        private void btn_SparePartsQuery_Click(object sender, EventArgs e)//备件管理-查询明细
        {
            //try
            //{
            //    string myStr1 = "select ID,类别,品名,规格,上次更换时间,标准寿命,实际使用次数,部件寿命残值 from SparePartData where 1=1";
            //    if (txt_SparePartsType.Text != "")
            //    {
            //        myStr1 += " and 类别 = '" + txt_SparePartsType.Text + "'";
            //    }
            //    if (txt_SparePartsName.Text != "")
            //    {
            //        myStr1 += " and 品名 = '" + txt_SparePartsName.Text + "'";
            //    }
            //    if (txt_SparePartsModel.Text != "")
            //    {
            //        myStr1 += " and 规格= '" + txt_SparePartsModel.Text + "'";
            //    }
            //    if (txt_SparePartsMonth.Text != "请选择月.....")
            //    {
            //        myStr1 += " and 月= '" + txt_SparePartsMonth.Text + "'";
            //    }
            //    if (txt_SparePartsYear.Text != "请选择年份.....")
            //    {
            //        myStr1 += " and 年= '" + txt_SparePartsYear.Text + "'";
            //    }
            //    SQLServer server = new SQLServer();
            //    DataTable d1 = new DataTable();
            //    d1 = server.ExecuteQuery(myStr1);
            //    dgv_SpareParts.DataSource = d1;
            //    dgv_SpareParts.Columns["部件寿命残值"].DefaultCellStyle.Format = "p3";//设定datagridview寿命残值显示为百分比
            //    _mainparent.dgv_AutoSize(dgv_SpareParts);
            //    dgv_SpareParts.Sort(dgv_SpareParts.Columns["部件寿命残值"], ListSortDirection.Ascending);//升序排列
            //    int i, j; float k = 0.00F; j = dgv_SpareParts.Rows.Count;
            //    for (i = 0; i < j - 1; i++)
            //    {
            //        k = float.Parse(dgv_SpareParts.Rows[i].Cells["部件寿命残值"].Value.ToString());
            //        if (k < 0.1 && k > 0)
            //        {
            //            dgv_SpareParts.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
            //        }
            //        else if (k <= 0)
            //        {
            //            dgv_SpareParts.Rows[i].DefaultCellStyle.BackColor = Color.Red;
            //        }
            //        else
            //        {
            //            dgv_SpareParts.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
            //        }
            //    }
            //    j = dgv_SpareParts.Columns.Count;
            //    if (j == 8)
            //    {
            //        DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            //        btn.Name = "btnModify";
            //        btn.HeaderText = "更换按钮";
            //        btn.DefaultCellStyle.NullValue = "更换";
            //        dgv_SpareParts.Columns.Add(btn);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Log.WriteLog(ex.ToString());
            //    MessageBox.Show(ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.txt_SparePartsYear.SelectedIndex = 0;
            //this.txt_SparePartsMonth.SelectedIndex = 0;
        }



        private void btn_SparePartsExport_Click(object sender, EventArgs e)
        {
            //_mainparent.dataGridViewToCSV(dgv_SpareParts);
        }

        private void btn_ReplaceSpareParts_Click(object sender, EventArgs e)
        {
            //if (dgv_SpareParts.CurrentRow == null)
            //{
            //    MessageBox.Show("无数据可编辑！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //else
            //{
            //    DataRow row = (dgv_SpareParts.CurrentRow.DataBoundItem as DataRowView).Row;
            //    Frm_Update fm_xg = new Frm_Update(row);//为修改准备数据
            //    fm_xg.txt编号.Text = row["ID"].ToString();
            //    fm_xg.txt类别.Text = row["类别"].ToString();
            //    fm_xg.txt备件名.Text = row["品名"].ToString();
            //    fm_xg.txt型号.Text = row["规格"].ToString();
            //    fm_xg.txt寿命.Text = row["标准寿命"].ToString();
            //    fm_xg.txt使用.Text = row["实际使用次数"].ToString();
            //    fm_xg.txt寿命残值.Text = row["部件寿命残值"].ToString();
            //    fm_xg.txt上次更换时间.Text = row["上次更换时间"].ToString();
            //    fm_xg.ShowDialog();

            //    if (dgv_SpareParts.SelectedRows.Count > 1)
            //    {
            //        MessageBox.Show("请选中一条以进行编辑，不可多条一同编辑", "来自系统的消息");
            //    }
            //}
        }

        private void dgv_SpareParts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgv_SpareParts.Columns[e.ColumnIndex].Name == "btnModify" && e.RowIndex >= 0)
            //{
            //    DataRow row = (dgv_SpareParts.CurrentRow.DataBoundItem as DataRowView).Row;
            //    Frm_Update fm_xg = new Frm_Update(row);
            //    fm_xg.txt编号.Text = row["ID"].ToString();
            //    fm_xg.txt类别.Text = row["类别"].ToString();
            //    fm_xg.txt备件名.Text = row["品名"].ToString();
            //    fm_xg.txt型号.Text = row["规格"].ToString();
            //    fm_xg.txt寿命.Text = row["标准寿命"].ToString();
            //    fm_xg.txt使用.Text = row["实际使用次数"].ToString();
            //    fm_xg.txt寿命残值.Text = row["部件寿命残值"].ToString();
            //    fm_xg.txt上次更换时间.Text = row["上次更换时间"].ToString();
            //    fm_xg.Show();
            //}
        }

        private void btn_SparePartsDelete_Click(object sender, EventArgs e)
        {
            //SQLServer server = new SQLServer();
            //if (dgv_SpareParts.CurrentRow == null)
            //{
            //    MessageBox.Show("无数据可删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //else
            //{
            //    try
            //    {
            //        int a = dgv_SpareParts.CurrentRow.Index;
            //        string idname = dgv_SpareParts.Rows[a].Cells[0].Value.ToString();
            //        string delStr = "delete from SparePartData where ID=" + idname + " ";
            //        server.ExecuteUpdate(delStr);
            //        btn_SparePartsQuery_Click(null, null);
            //        MessageBox.Show("删除成功", "来自系统的消息");
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("删除失败", "来自系统的消息");
            //    }
            //}
        }

        private void dgv_SpareParts_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //int i, j; float k = 0.00F; j = dgv_SpareParts.Rows.Count;
            //for (i = 0; i < j; i++)
            //{
            //    k = float.Parse(dgv_SpareParts.Rows[i].Cells["部件寿命残值"].Value.ToString());
            //    if ((k < 0.7) && (k > 0.3))
            //    {
            //        dgv_SpareParts.Rows[i].DefaultCellStyle.BackColor = Color.Orange;
            //    }
            //    else if (k >= 0.7)
            //    {
            //        dgv_SpareParts.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
            //    }
            //    else if (k <= 0.3)
            //    {
            //        dgv_SpareParts.Rows[i].DefaultCellStyle.BackColor = Color.Red;
            //    }
            //}
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            //SQLServer server = new SQLServer();
            //if (cb_type.Text == "" || txt_name.Text == "" || txt_model.Text == "" || txt_life.Text == "")
            //{
            //    MessageBox.Show("有部分项目为空值,请填满!", "来自系统的消息");
            //}
            //else
            //{
            //    string insStr = "insert into SparePartData (类别,品名,规格,标准寿命,实际使用次数,部件寿命残值,上次更换时间,月,年) values ('" + cb_type.Text + "','" + txt_name.Text + "','" + txt_model.Text + "','" + txt_life.Text + "','" + 0 + "','" + 1 + "','" + DateTime.Now.ToString() + "','" + DateTime.Now.Month.ToString() + "','" + DateTime.Now.Year.ToString() + "')";
            //    int d1 = server.ExecuteUpdate(insStr);//执行插入数据至SQLServer
            //    if (d1 > 0)
            //    {
            //        MessageBox.Show("添加成功!", "来自系统的消息");
            //        txt_name.Text = "";
            //        txt_model.Text = "";
            //        txt_life.Text = "";
            //        cb_type.Text = "";
            //    }
            //    else
            //    {
            //        MessageBox.Show("添加失败!请重启软件或联系软件工程师!", "来自系统的消息");
            //        txt_name.Text = "";
            //        txt_model.Text = "";
            //        txt_life.Text = "";
            //        cb_type.Text = "";
            //    }
            //}
        }

        private void btn_Reset_Click(object sender, EventArgs e)
        {
            //txt_name.Text = "";
            //txt_model.Text = "";
            //txt_life.Text = "";
        }

        #endregion


        private void HomeFrm_Load(object sender, EventArgs e)
        {
            //ShowSparePartData();
            List_Control = GetAllControls(this);//列表中添加所有窗体控件
           

            //tabControl1.TabPages.Remove(tabPage4);

            Thread.Sleep(300);
        }


        private void ShowDGV(DataGridView dgv, DataTable dt, int index)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowDataGridView(ShowDGV), new object[] { dgv, dt, index });
                return;
            }
            switch (index)
            {
                case 0:
                    dgv.DataSource = dt;
                    break;
                case 1:
                    dgv.DataSource = dt;
                    dgv.Columns["部件寿命残值"].DefaultCellStyle.Format = "p3";//设定datagridview寿命残值显示为百分比
                    dgv_AutoSize(dgv);
                    dgv.Sort(dgv.Columns["部件寿命残值"], ListSortDirection.Ascending);//升序排列
                    break;
                default:
                    break;
            }
        }

        public void dgv_AutoSize(DataGridView dgv)//dgv表格自适应
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DGVAutoSize(dgv_AutoSize), new object[] { dgv });
                return;
            }
            int width = 0;
            //对于DataGridView的每一个列都调整
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                //将每一列都调整为自动适应模式
                dgv.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells);
                //记录整个DataGridView的宽度
                width += dgv.Columns[i].Width;
            }
            //判断调整后的宽度与原来设定的宽度的关系，如果是调整后的宽度大于原来设定的宽度，
            //则将DataGridView的列自动调整模式设置为显示的列即可，             //如果是小于原来设定的宽度，将模式改为填充。
            if (width > dgv.Size.Width)
            {
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            }
            else
            {
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            //设置表格字体居中
            dgv.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //设置表格列字体居中
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 12, FontStyle.Bold);
            dgv.RowsDefaultCellStyle.Font = new Font("微软雅黑", 9);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;//禁止列标题换行
        }

        public void UpDatalabel(string txt, string Name)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Labelvision(UpDatalabel), new object[] { txt, Name });
                return;
            }
            foreach (Control ctrl in List_Control)
            {
                if (ctrl.GetType() == typeof(Label))
                {
                    if (ctrl.Name == Name)
                    {
                        ctrl.Text = txt;
                    }
                }
            }
        }

        public void UpDatelabelcolor(Color color, string str, string Name)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Labelcolor(UpDatelabelcolor), new object[] { color, str, Name });
                return;
            }
            foreach (Control ctrl in List_Control)
            {
                if (ctrl.GetType() == typeof(Label))
                {
                    if (ctrl.Name == Name)
                    {
                        ctrl.BackColor = color;
                        ctrl.Text = str;
                    }
                }
            }
        }

        public void AddList(string msg, string Name)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new AddItemToListBoxDelegate(AddList), new object[] { msg, Name });
                return;
            }
            foreach (Control ctrl in List_Control)
            {
                if (ctrl.GetType() == typeof(ListBox))
                {
                    if (ctrl.Name == Name)
                    {
                        if (msg != "N/A")
                        {
                            ((ListBox)ctrl).SelectedItem = ((ListBox)ctrl).Items.Count;

                            ((ListBox)ctrl).Items.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + msg);

                            ((ListBox)ctrl).TopIndex = ((ListBox)ctrl).Items.Count - 1;
                        }
                        else
                        {
                            ((ListBox)ctrl).Items.Clear();
                        }


                    }
                }
            }
        }

        public void AppendRichText(string msg, string Name)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new AddItemToListBoxDelegate(AppendRichText), new object[] { msg, Name });
                return;
            }
            foreach (Control ctrl in List_Control)
            {
                if (ctrl.GetType() == typeof(RichTextBox))
                {
                    if (ctrl.Name == Name)
                    {
                        if (ctrl.Name == "Rtxt_OEE_Detail" || ctrl.Name == "Rtxt_OEE_TimeSpan" || ctrl.Name == "Rtxt_Send_newStn" || ctrl.Name == "Rtxt_Rec_newStn" || ctrl.Name == "Rtxt_Send_Check" || ctrl.Name == "Rtxt_Rec_Check" || ctrl.Name == "Rtxt_Send_Pass" || ctrl.Name == "Rtxt_Rec_Pass" || ctrl.Name == "Rtxt_Send_Trace" || ctrl.Name == "Rtxt_Rec_Trace")//判断上传多少次后清空Rth控件
                        {
                            rth_Number++;
                            if (rth_Number > 300)
                            {
                                rth_Number = 0;

                                Rtxt_OEE_Detail.Clear();
                                Rtxt_OEE_TimeSpan.Clear();
                                Rtxt_Send_newStn.Clear();
                                Rtxt_Rec_newStn.Clear();
                                Rtxt_Send_Check.Clear();
                                Rtxt_Rec_Check.Clear();
                                Rtxt_Send_Pass.Clear();
                                Rtxt_Rec_Pass.Clear();
                                Rtxt_Send_Trace.Clear();
                                Rtxt_Rec_Trace.Clear();
                            }
                        }
                        
                        if (msg != "N/A")
                        {
                            ((RichTextBox)ctrl).AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":  " + msg + "\r\n");
                            //将光标位置设置到当前内容的末尾
                            ((RichTextBox)ctrl).SelectionStart = ((RichTextBox)ctrl).Text.Length;
                            //滚动到光标位置
                            ((RichTextBox)ctrl).ScrollToCaret();
                        }
                        else
                        {
                            ((RichTextBox)ctrl).Clear();
                        }
                    }
                }
            }

        }
        private delegate void ClearText();
        public void ClearNG()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ClearText(ClearNG));
                return;
            }
            this.richTextBoxNG.Clear();
        }
       

        public void UiText(string str1, string Name)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowTxt(UiText), new object[] { str1, Name });
                return;
            }
            foreach (Control ctrl in List_Control)
            {
                if (ctrl.GetType() == typeof(TextBox))
                {
                    if (ctrl.Name == Name)
                    {
                        ctrl.Text = str1;
                    }
                }
            }
        }

        public List<Control> GetAllControls(Control control)
        {
            var list = new List<Control>();
            foreach (Control con in control.Controls)
            {
                list.Add(con);
                if (con.Controls.Count > 0)
                {
                    list.AddRange(GetAllControls(con));
                }
            }
            return list;
        }

        
    }
}
