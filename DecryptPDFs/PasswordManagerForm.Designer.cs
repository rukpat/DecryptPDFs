namespace DecryptPDFs
{
    partial class PasswordManagerForm
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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasswordManagerForm));
            dataGridViewPWDMgr = new DataGridView();
            ID = new DataGridViewTextBoxColumn();
            Nickname = new DataGridViewTextBoxColumn();
            Description = new DataGridViewTextBoxColumn();
            PDFPassword = new DataGridViewTextBoxColumn();
            SuccessCount = new DataGridViewTextBoxColumn();
            LastUsedAt = new DataGridViewTextBoxColumn();
            CreatedAt = new DataGridViewTextBoxColumn();
            LastModified = new DataGridViewTextBoxColumn();
            passwordManagerEntityBindingSource = new BindingSource(components);
            btnSave = new Button();
            btnClose = new Button();
            linkLabelDBPath = new LinkLabel();
            toolTipDBPath = new ToolTip(components);
            checkBoxShowPasswords = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPWDMgr).BeginInit();
            ((System.ComponentModel.ISupportInitialize)passwordManagerEntityBindingSource).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewPWDMgr
            // 
            dataGridViewCellStyle1.BackColor = Color.WhiteSmoke;
            dataGridViewPWDMgr.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewPWDMgr.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewPWDMgr.AutoGenerateColumns = false;
            dataGridViewPWDMgr.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewPWDMgr.BackgroundColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.SteelBlue;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dataGridViewPWDMgr.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewPWDMgr.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewPWDMgr.Columns.AddRange(new DataGridViewColumn[] { ID, Nickname, Description, PDFPassword, SuccessCount, LastUsedAt, CreatedAt, LastModified });
            dataGridViewPWDMgr.DataSource = passwordManagerEntityBindingSource;
            dataGridViewPWDMgr.Location = new Point(12, 12);
            dataGridViewPWDMgr.Name = "dataGridViewPWDMgr";
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Control;
            dataGridViewCellStyle8.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle8.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            dataGridViewPWDMgr.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewPWDMgr.Size = new Size(881, 410);
            dataGridViewPWDMgr.TabIndex = 0;
            dataGridViewPWDMgr.CellFormatting += dataGridViewPWDMgr_CellFormatting;
            dataGridViewPWDMgr.UserDeletingRow += dataGridViewPWDMgr_UserDeletingRow;
            // 
            // ID
            // 
            ID.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            ID.DataPropertyName = "ID";
            dataGridViewCellStyle3.BackColor = Color.Gainsboro;
            dataGridViewCellStyle3.ForeColor = Color.Gray;
            ID.DefaultCellStyle = dataGridViewCellStyle3;
            ID.DividerWidth = 2;
            ID.HeaderText = "ID";
            ID.Name = "ID";
            ID.ReadOnly = true;
            ID.Width = 47;
            // 
            // Nickname
            // 
            Nickname.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Nickname.DataPropertyName = "Nickname";
            Nickname.HeaderText = "Nickname";
            Nickname.MaxInputLength = 20;
            Nickname.Name = "Nickname";
            Nickname.Width = 88;
            // 
            // Description
            // 
            Description.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Description.DataPropertyName = "Description";
            Description.HeaderText = "Description";
            Description.MaxInputLength = 200;
            Description.Name = "Description";
            // 
            // PDFPassword
            // 
            PDFPassword.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            PDFPassword.DataPropertyName = "PDFPassword";
            PDFPassword.HeaderText = "PDF Password";
            PDFPassword.MaxInputLength = 25;
            PDFPassword.Name = "PDFPassword";
            // 
            // SuccessCount
            // 
            SuccessCount.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            SuccessCount.DataPropertyName = "SuccessCount";
            dataGridViewCellStyle4.BackColor = Color.Gainsboro;
            dataGridViewCellStyle4.ForeColor = Color.Gray;
            SuccessCount.DefaultCellStyle = dataGridViewCellStyle4;
            SuccessCount.HeaderText = "Success Count";
            SuccessCount.Name = "SuccessCount";
            SuccessCount.ReadOnly = true;
            SuccessCount.Width = 102;
            // 
            // LastUsedAt
            // 
            LastUsedAt.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            LastUsedAt.DataPropertyName = "LastUsedAt";
            dataGridViewCellStyle5.BackColor = Color.Gainsboro;
            dataGridViewCellStyle5.ForeColor = Color.Gray;
            LastUsedAt.DefaultCellStyle = dataGridViewCellStyle5;
            LastUsedAt.HeaderText = "Last Used";
            LastUsedAt.Name = "LastUsedAt";
            LastUsedAt.ReadOnly = true;
            LastUsedAt.Width = 79;
            // 
            // CreatedAt
            // 
            CreatedAt.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            CreatedAt.DataPropertyName = "CreatedAt";
            dataGridViewCellStyle6.BackColor = Color.Gainsboro;
            dataGridViewCellStyle6.ForeColor = Color.Gray;
            CreatedAt.DefaultCellStyle = dataGridViewCellStyle6;
            CreatedAt.HeaderText = "Created";
            CreatedAt.Name = "CreatedAt";
            CreatedAt.ReadOnly = true;
            CreatedAt.Width = 76;
            // 
            // LastModified
            // 
            LastModified.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            LastModified.DataPropertyName = "LastModified";
            dataGridViewCellStyle7.BackColor = Color.Gainsboro;
            dataGridViewCellStyle7.ForeColor = Color.Gray;
            LastModified.DefaultCellStyle = dataGridViewCellStyle7;
            LastModified.HeaderText = "Modified";
            LastModified.Name = "LastModified";
            LastModified.ReadOnly = true;
            LastModified.Width = 82;
            // 
            // passwordManagerEntityBindingSource
            // 
            passwordManagerEntityBindingSource.DataSource = typeof(PasswordManagerEntity);
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.Location = new Point(629, 438);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(122, 23);
            btnSave.TabIndex = 1;
            btnSave.Text = "&Save Changes";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.Location = new Point(770, 438);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(122, 23);
            btnClose.TabIndex = 2;
            btnClose.Text = "&Close";
            btnClose.UseCompatibleTextRendering = true;
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // linkLabelDBPath
            // 
            linkLabelDBPath.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            linkLabelDBPath.AutoSize = true;
            linkLabelDBPath.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            linkLabelDBPath.LinkColor = Color.FromArgb(128, 128, 255);
            linkLabelDBPath.Location = new Point(12, 442);
            linkLabelDBPath.Name = "linkLabelDBPath";
            linkLabelDBPath.Size = new Size(51, 15);
            linkLabelDBPath.TabIndex = 8;
            linkLabelDBPath.TabStop = true;
            linkLabelDBPath.Text = "Filepath";
            linkLabelDBPath.LinkClicked += linkLabelDBPath_LinkClicked;
            // 
            // checkBoxShowPasswords
            // 
            checkBoxShowPasswords.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            checkBoxShowPasswords.AutoSize = true;
            checkBoxShowPasswords.Location = new Point(500, 441);
            checkBoxShowPasswords.Name = "checkBoxShowPasswords";
            checkBoxShowPasswords.Size = new Size(113, 19);
            checkBoxShowPasswords.TabIndex = 9;
            checkBoxShowPasswords.Text = "Show Passwords";
            checkBoxShowPasswords.UseVisualStyleBackColor = true;
            checkBoxShowPasswords.CheckedChanged += checkBoxShowPasswords_CheckedChanged;
            // 
            // PasswordManagerForm
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new Size(904, 475);
            Controls.Add(btnClose);
            Controls.Add(btnSave);
            Controls.Add(dataGridViewPWDMgr);
            Controls.Add(linkLabelDBPath);
            Controls.Add(checkBoxShowPasswords);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimizeBox = false;
            Name = "PasswordManagerForm";
            Text = "Passwords Manager";
            FormClosing += PasswordManagerForm_FormClosing;
            FormClosed += PasswordManagerForm_FormClosed;
            Load += PasswordManagerForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewPWDMgr).EndInit();
            ((System.ComponentModel.ISupportInitialize)passwordManagerEntityBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private DataGridView dataGridViewPWDMgr;
        private Button btnSave;
        private Button btnClose;
        private LinkLabel linkLabelDBPath;
        private ToolTip toolTipDBPath;
        private CheckBox checkBoxShowPasswords;
        private BindingSource passwordManagerEntityBindingSource;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewTextBoxColumn Nickname;
        private DataGridViewTextBoxColumn Description;
        private DataGridViewTextBoxColumn PDFPassword;
        private DataGridViewTextBoxColumn SuccessCount;
        private DataGridViewTextBoxColumn LastUsedAt;
        private DataGridViewTextBoxColumn CreatedAt;
        private DataGridViewTextBoxColumn LastModified;
    }
}