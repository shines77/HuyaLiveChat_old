namespace HuyaLiveHelper
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.cbBoxRoomId = new System.Windows.Forms.ComboBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblRoomId = new System.Windows.Forms.Label();
            this.txtBoxRoomId = new System.Windows.Forms.TextBox();
            this.chatContent = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // cbBoxRoomId
            // 
            this.cbBoxRoomId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBoxRoomId.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbBoxRoomId.FormattingEnabled = true;
            this.cbBoxRoomId.Location = new System.Drawing.Point(217, 14);
            this.cbBoxRoomId.Name = "cbBoxRoomId";
            this.cbBoxRoomId.Size = new System.Drawing.Size(137, 22);
            this.cbBoxRoomId.TabIndex = 10;
            this.cbBoxRoomId.SelectedIndexChanged += new System.EventHandler(this.cbBoxRoomId_SelectedIndexChanged);
            // 
            // btnLogin
            // 
            this.btnLogin.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLogin.Location = new System.Drawing.Point(362, 13);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 9;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lblRoomId
            // 
            this.lblRoomId.AutoSize = true;
            this.lblRoomId.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRoomId.Location = new System.Drawing.Point(12, 19);
            this.lblRoomId.Name = "lblRoomId";
            this.lblRoomId.Size = new System.Drawing.Size(91, 14);
            this.lblRoomId.TabIndex = 8;
            this.lblRoomId.Text = "虎牙房间号：";
            // 
            // txtBoxRoomId
            // 
            this.txtBoxRoomId.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBoxRoomId.Location = new System.Drawing.Point(107, 14);
            this.txtBoxRoomId.Name = "txtBoxRoomId";
            this.txtBoxRoomId.Size = new System.Drawing.Size(100, 23);
            this.txtBoxRoomId.TabIndex = 7;
            // 
            // chatContent
            // 
            this.chatContent.AcceptsTab = true;
            this.chatContent.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chatContent.Location = new System.Drawing.Point(12, 48);
            this.chatContent.Name = "chatContent";
            this.chatContent.ReadOnly = true;
            this.chatContent.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.chatContent.Size = new System.Drawing.Size(656, 544);
            this.chatContent.TabIndex = 6;
            this.chatContent.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 605);
            this.Controls.Add(this.cbBoxRoomId);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lblRoomId);
            this.Controls.Add(this.txtBoxRoomId);
            this.Controls.Add(this.chatContent);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HuyaChatHelper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbBoxRoomId;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblRoomId;
        private System.Windows.Forms.TextBox txtBoxRoomId;
        private System.Windows.Forms.RichTextBox chatContent;
    }
}

