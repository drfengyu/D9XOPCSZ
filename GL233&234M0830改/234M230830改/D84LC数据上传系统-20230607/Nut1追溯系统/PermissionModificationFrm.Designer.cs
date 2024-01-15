namespace 卓汇数据追溯系统
{
    partial class PermissionModificationFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txt_ID = new System.Windows.Forms.TextBox();
            this.txt_pwd = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_Name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbo_Login = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_ChangePwd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_ID
            // 
            this.txt_ID.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.txt_ID.Location = new System.Drawing.Point(217, 86);
            this.txt_ID.Name = "txt_ID";
            this.txt_ID.Size = new System.Drawing.Size(121, 26);
            this.txt_ID.TabIndex = 19;
            // 
            // txt_pwd
            // 
            this.txt_pwd.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.txt_pwd.Location = new System.Drawing.Point(217, 140);
            this.txt_pwd.Name = "txt_pwd";
            this.txt_pwd.Size = new System.Drawing.Size(121, 26);
            this.txt_pwd.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(126, 142);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 25);
            this.label7.TabIndex = 17;
            this.label7.Text = "密码:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(126, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 25);
            this.label6.TabIndex = 16;
            this.label6.Text = "工号:";
            // 
            // txt_Name
            // 
            this.txt_Name.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.txt_Name.Location = new System.Drawing.Point(217, 35);
            this.txt_Name.Name = "txt_Name";
            this.txt_Name.Size = new System.Drawing.Size(121, 26);
            this.txt_Name.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(126, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 25);
            this.label2.TabIndex = 20;
            this.label2.Text = "姓名:";
            // 
            // cbo_Login
            // 
            this.cbo_Login.Font = new System.Drawing.Font("微软雅黑", 10.5F);
            this.cbo_Login.FormattingEnabled = true;
            this.cbo_Login.Items.AddRange(new object[] {
            "技术员A级",
            "技术员B级",
            "技术员C级",
            "技术员D级"});
            this.cbo_Login.Location = new System.Drawing.Point(217, 201);
            this.cbo_Login.Name = "cbo_Login";
            this.cbo_Login.Size = new System.Drawing.Size(121, 28);
            this.cbo_Login.TabIndex = 25;
            this.cbo_Login.Text = "技术员A级";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(107, 202);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 25);
            this.label5.TabIndex = 24;
            this.label5.Text = "权限授权:";
            // 
            // btn_ChangePwd
            // 
            this.btn_ChangePwd.BackColor = System.Drawing.Color.RoyalBlue;
            this.btn_ChangePwd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_ChangePwd.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btn_ChangePwd.Location = new System.Drawing.Point(217, 274);
            this.btn_ChangePwd.Margin = new System.Windows.Forms.Padding(0);
            this.btn_ChangePwd.Name = "btn_ChangePwd";
            this.btn_ChangePwd.Size = new System.Drawing.Size(111, 34);
            this.btn_ChangePwd.TabIndex = 26;
            this.btn_ChangePwd.Text = "确认";
            this.btn_ChangePwd.UseVisualStyleBackColor = false;
            this.btn_ChangePwd.Click += new System.EventHandler(this.btn_ChangePwd_Click);
            // 
            // PermissionModificationFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 347);
            this.Controls.Add(this.btn_ChangePwd);
            this.Controls.Add(this.cbo_Login);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_Name);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_ID);
            this.Controls.Add(this.txt_pwd);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Name = "PermissionModificationFrm";
            this.Text = "权限修改";
            this.Load += new System.EventHandler(this.PermissionModificationFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_ID;
        private System.Windows.Forms.TextBox txt_pwd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_Name;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbo_Login;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_ChangePwd;
    }
}