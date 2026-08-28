namespace DecryptPDFs
{
    partial class EnD
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnD));
            listFilesFolders = new ListView();
            columnHeaderNo = new ColumnHeader();
            columnHeaderFilename = new ColumnHeader();
            columnHeaderState = new ColumnHeader();
            columnHeaderComment = new ColumnHeader();
            linkLabelFolderName = new LinkLabel();
            label1 = new Label();
            textBoxPassword = new TextBox();
            groupBoxOverwrite = new GroupBox();
            radioButtonPrefix = new RadioButton();
            textBoxOverwriteString = new TextBox();
            radioButtonSuffix = new RadioButton();
            checkBoxOverwrite = new CheckBox();
            buttonDecrypt = new Button();
            buttonClose = new Button();
            checkBoxRecurseDir = new CheckBox();
            panel1 = new Panel();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            labelCountError = new Label();
            labelCountPasswordProtected = new Label();
            labelCountHasSecuritySettings = new Label();
            labelCountNoSecurity = new Label();
            checkBoxRemoveSecurity = new CheckBox();
            btnOpenPWDMgr = new Button();
            btnSelectFiles = new Button();
            statusStrip1 = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            groupBoxOverwrite.SuspendLayout();
            panel1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // listFilesFolders
            // 
            listFilesFolders.AllowColumnReorder = true;
            listFilesFolders.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listFilesFolders.BackColor = Color.FromArgb(40, 40, 40);
            listFilesFolders.BorderStyle = BorderStyle.FixedSingle;
            listFilesFolders.Columns.AddRange(new ColumnHeader[] { columnHeaderNo, columnHeaderFilename, columnHeaderState, columnHeaderComment });
            listFilesFolders.ForeColor = Color.WhiteSmoke;
            listFilesFolders.FullRowSelect = true;
            listFilesFolders.Location = new Point(251, 12);
            listFilesFolders.Name = "listFilesFolders";
            listFilesFolders.ShowItemToolTips = true;
            listFilesFolders.Size = new Size(661, 542);
            listFilesFolders.TabIndex = 1;
            listFilesFolders.UseCompatibleStateImageBehavior = false;
            listFilesFolders.View = View.Details;
            listFilesFolders.ItemActivate += listFilesFolders_ItemActivate;
            // 
            // columnHeaderNo
            // 
            columnHeaderNo.Text = "No";
            // 
            // columnHeaderFilename
            // 
            columnHeaderFilename.Text = "Filename";
            columnHeaderFilename.Width = 200;
            // 
            // columnHeaderState
            // 
            columnHeaderState.Text = "State";
            // 
            // columnHeaderComment
            // 
            columnHeaderComment.Text = "Comment (Double-Click to view)";
            columnHeaderComment.Width = 100;
            // 
            // linkLabelFolderName
            // 
            linkLabelFolderName.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            linkLabelFolderName.AutoSize = true;
            linkLabelFolderName.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            linkLabelFolderName.LinkColor = Color.FromArgb(128, 128, 255);
            linkLabelFolderName.Location = new Point(251, 585);
            linkLabelFolderName.Name = "linkLabelFolderName";
            linkLabelFolderName.Size = new Size(51, 15);
            linkLabelFolderName.TabIndex = 7;
            linkLabelFolderName.TabStop = true;
            linkLabelFolderName.Text = "Filepath";
            linkLabelFolderName.LinkClicked += linkLabelFolderName_LinkClicked;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(787, 585);
            label1.Name = "label1";
            label1.Size = new Size(125, 15);
            label1.TabIndex = 9;
            label1.Text = "Only * .pdf files shown";
            label1.TextAlign = ContentAlignment.TopRight;
            // 
            // textBoxPassword
            // 
            textBoxPassword.AutoCompleteMode = AutoCompleteMode.Append;
            textBoxPassword.BackColor = Color.FromArgb(40, 40, 40);
            textBoxPassword.BorderStyle = BorderStyle.FixedSingle;
            textBoxPassword.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxPassword.ForeColor = Color.WhiteSmoke;
            textBoxPassword.Location = new Point(12, 12);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.PlaceholderText = "Enter Password";
            textBoxPassword.Size = new Size(225, 27);
            textBoxPassword.TabIndex = 0;
            textBoxPassword.MouseDown += textBoxPassword_MouseDown;
            // 
            // groupBoxOverwrite
            // 
            groupBoxOverwrite.Controls.Add(radioButtonPrefix);
            groupBoxOverwrite.Controls.Add(textBoxOverwriteString);
            groupBoxOverwrite.Controls.Add(radioButtonSuffix);
            groupBoxOverwrite.FlatStyle = FlatStyle.Popup;
            groupBoxOverwrite.ForeColor = Color.WhiteSmoke;
            groupBoxOverwrite.Location = new Point(12, 179);
            groupBoxOverwrite.Name = "groupBoxOverwrite";
            groupBoxOverwrite.Size = new Size(225, 99);
            groupBoxOverwrite.TabIndex = 10;
            groupBoxOverwrite.TabStop = false;
            groupBoxOverwrite.Text = "     Overwrite     ";
            groupBoxOverwrite.Visible = false;
            // 
            // radioButtonPrefix
            // 
            radioButtonPrefix.AutoSize = true;
            radioButtonPrefix.Checked = true;
            radioButtonPrefix.Location = new Point(17, 28);
            radioButtonPrefix.Name = "radioButtonPrefix";
            radioButtonPrefix.Size = new Size(54, 19);
            radioButtonPrefix.TabIndex = 3;
            radioButtonPrefix.TabStop = true;
            radioButtonPrefix.Text = "Prefix";
            radioButtonPrefix.UseVisualStyleBackColor = true;
            radioButtonPrefix.CheckedChanged += radioButtonPrefix_CheckedChanged;
            // 
            // textBoxOverwriteString
            // 
            textBoxOverwriteString.BackColor = Color.FromArgb(40, 40, 40);
            textBoxOverwriteString.BorderStyle = BorderStyle.FixedSingle;
            textBoxOverwriteString.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxOverwriteString.ForeColor = Color.WhiteSmoke;
            textBoxOverwriteString.Location = new Point(18, 55);
            textBoxOverwriteString.Name = "textBoxOverwriteString";
            textBoxOverwriteString.PlaceholderText = "Enter Prefix";
            textBoxOverwriteString.Size = new Size(191, 27);
            textBoxOverwriteString.TabIndex = 5;
            textBoxOverwriteString.Text = "decrypted_";
            // 
            // radioButtonSuffix
            // 
            radioButtonSuffix.AutoSize = true;
            radioButtonSuffix.Location = new Point(153, 28);
            radioButtonSuffix.Name = "radioButtonSuffix";
            radioButtonSuffix.Size = new Size(54, 19);
            radioButtonSuffix.TabIndex = 4;
            radioButtonSuffix.Text = "Suffix";
            radioButtonSuffix.UseVisualStyleBackColor = true;
            radioButtonSuffix.CheckedChanged += radioButtonSuffix_CheckedChanged;
            // 
            // checkBoxOverwrite
            // 
            checkBoxOverwrite.AutoSize = true;
            checkBoxOverwrite.Location = new Point(11, 175);
            checkBoxOverwrite.Name = "checkBoxOverwrite";
            checkBoxOverwrite.Size = new Size(98, 19);
            checkBoxOverwrite.TabIndex = 2;
            checkBoxOverwrite.Text = "Overwrite File";
            checkBoxOverwrite.UseVisualStyleBackColor = true;
            checkBoxOverwrite.CheckedChanged += checkBoxOverwrite_CheckedChanged;
            // 
            // buttonDecrypt
            // 
            buttonDecrypt.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonDecrypt.BackColor = SystemColors.ActiveCaption;
            buttonDecrypt.BackgroundImageLayout = ImageLayout.Zoom;
            buttonDecrypt.FlatAppearance.BorderColor = SystemColors.ActiveCaption;
            buttonDecrypt.FlatStyle = FlatStyle.Popup;
            buttonDecrypt.ForeColor = Color.Black;
            buttonDecrypt.Image = Properties.Resources.nS16;
            buttonDecrypt.ImageAlign = ContentAlignment.MiddleLeft;
            buttonDecrypt.Location = new Point(138, 570);
            buttonDecrypt.Name = "buttonDecrypt";
            buttonDecrypt.Size = new Size(100, 30);
            buttonDecrypt.TabIndex = 2;
            buttonDecrypt.Text = "&Decrypt";
            buttonDecrypt.UseVisualStyleBackColor = false;
            buttonDecrypt.Click += buttonDecrypt_Click;
            // 
            // buttonClose
            // 
            buttonClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonClose.BackColor = Color.Transparent;
            buttonClose.BackgroundImageLayout = ImageLayout.Zoom;
            buttonClose.FlatAppearance.BorderColor = Color.WhiteSmoke;
            buttonClose.FlatStyle = FlatStyle.Popup;
            buttonClose.ForeColor = Color.WhiteSmoke;
            buttonClose.ImageAlign = ContentAlignment.MiddleLeft;
            buttonClose.Location = new Point(12, 570);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(100, 30);
            buttonClose.TabIndex = 6;
            buttonClose.Text = "&Close";
            buttonClose.UseVisualStyleBackColor = false;
            buttonClose.Click += buttonClose_Click;
            // 
            // checkBoxRecurseDir
            // 
            checkBoxRecurseDir.AutoSize = true;
            checkBoxRecurseDir.Location = new Point(12, 65);
            checkBoxRecurseDir.Name = "checkBoxRecurseDir";
            checkBoxRecurseDir.Size = new Size(118, 19);
            checkBoxRecurseDir.TabIndex = 11;
            checkBoxRecurseDir.Text = "Recurse Directory";
            checkBoxRecurseDir.UseVisualStyleBackColor = true;
            checkBoxRecurseDir.CheckedChanged += checkBoxRecurseDir_CheckedChanged;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(labelCountError);
            panel1.Controls.Add(labelCountPasswordProtected);
            panel1.Controls.Add(labelCountHasSecuritySettings);
            panel1.Controls.Add(labelCountNoSecurity);
            panel1.Location = new Point(12, 465);
            panel1.Name = "panel1";
            panel1.Size = new Size(225, 89);
            panel1.TabIndex = 12;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.LightGreen;
            label5.Location = new Point(-1, 74);
            label5.Name = "label5";
            label5.Size = new Size(117, 15);
            label5.TabIndex = 19;
            label5.Text = "Password Protected :";
            label5.TextAlign = ContentAlignment.TopRight;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.LightSteelBlue;
            label4.Location = new Point(-2, 57);
            label4.Name = "label4";
            label4.Size = new Size(118, 15);
            label4.TabIndex = 18;
            label4.Text = "Has Security Setting :";
            label4.TextAlign = ContentAlignment.TopRight;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.DimGray;
            label3.Location = new Point(42, 13);
            label3.Name = "label3";
            label3.Size = new Size(74, 15);
            label3.TabIndex = 17;
            label3.Text = "No Security :";
            label3.TextAlign = ContentAlignment.TopRight;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.RosyBrown;
            label2.Location = new Point(78, 29);
            label2.Name = "label2";
            label2.Size = new Size(38, 15);
            label2.TabIndex = 16;
            label2.Text = "Error :\r\n";
            label2.TextAlign = ContentAlignment.TopRight;
            // 
            // labelCountError
            // 
            labelCountError.AutoSize = true;
            labelCountError.ForeColor = Color.RosyBrown;
            labelCountError.Location = new Point(124, 29);
            labelCountError.Name = "labelCountError";
            labelCountError.Size = new Size(25, 15);
            labelCountError.TabIndex = 15;
            labelCountError.Text = "999";
            // 
            // labelCountPasswordProtected
            // 
            labelCountPasswordProtected.AutoSize = true;
            labelCountPasswordProtected.ForeColor = Color.LightGreen;
            labelCountPasswordProtected.Location = new Point(124, 74);
            labelCountPasswordProtected.Name = "labelCountPasswordProtected";
            labelCountPasswordProtected.Size = new Size(25, 15);
            labelCountPasswordProtected.TabIndex = 5;
            labelCountPasswordProtected.Text = "999";
            // 
            // labelCountHasSecuritySettings
            // 
            labelCountHasSecuritySettings.AutoSize = true;
            labelCountHasSecuritySettings.ForeColor = Color.LightSteelBlue;
            labelCountHasSecuritySettings.Location = new Point(124, 57);
            labelCountHasSecuritySettings.Name = "labelCountHasSecuritySettings";
            labelCountHasSecuritySettings.Size = new Size(25, 15);
            labelCountHasSecuritySettings.TabIndex = 4;
            labelCountHasSecuritySettings.Text = "999";
            // 
            // labelCountNoSecurity
            // 
            labelCountNoSecurity.AutoSize = true;
            labelCountNoSecurity.ForeColor = Color.DimGray;
            labelCountNoSecurity.Location = new Point(124, 13);
            labelCountNoSecurity.Name = "labelCountNoSecurity";
            labelCountNoSecurity.Size = new Size(25, 15);
            labelCountNoSecurity.TabIndex = 3;
            labelCountNoSecurity.Text = "999";
            // 
            // checkBoxRemoveSecurity
            // 
            checkBoxRemoveSecurity.AutoSize = true;
            checkBoxRemoveSecurity.Location = new Point(12, 133);
            checkBoxRemoveSecurity.Name = "checkBoxRemoveSecurity";
            checkBoxRemoveSecurity.Size = new Size(159, 19);
            checkBoxRemoveSecurity.TabIndex = 13;
            checkBoxRemoveSecurity.Text = "Remove Security Settings";
            checkBoxRemoveSecurity.UseVisualStyleBackColor = true;
            // 
            // btnOpenPWDMgr
            // 
            btnOpenPWDMgr.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnOpenPWDMgr.BackColor = Color.Transparent;
            btnOpenPWDMgr.BackgroundImageLayout = ImageLayout.Zoom;
            btnOpenPWDMgr.FlatAppearance.BorderColor = Color.WhiteSmoke;
            btnOpenPWDMgr.FlatStyle = FlatStyle.Popup;
            btnOpenPWDMgr.ForeColor = Color.WhiteSmoke;
            btnOpenPWDMgr.ImageAlign = ContentAlignment.MiddleLeft;
            btnOpenPWDMgr.Location = new Point(12, 331);
            btnOpenPWDMgr.Name = "btnOpenPWDMgr";
            btnOpenPWDMgr.Size = new Size(225, 30);
            btnOpenPWDMgr.TabIndex = 15;
            btnOpenPWDMgr.Text = "&Password Manager...";
            btnOpenPWDMgr.UseVisualStyleBackColor = false;
            btnOpenPWDMgr.Click += btnOpenPWDMgr_Click;
            //
            // btnSelectFiles
            //
            btnSelectFiles.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSelectFiles.BackColor = Color.Transparent;
            btnSelectFiles.BackgroundImageLayout = ImageLayout.Zoom;
            btnSelectFiles.FlatAppearance.BorderColor = Color.WhiteSmoke;
            btnSelectFiles.FlatStyle = FlatStyle.Popup;
            btnSelectFiles.ForeColor = Color.WhiteSmoke;
            btnSelectFiles.ImageAlign = ContentAlignment.MiddleLeft;
            btnSelectFiles.Location = new Point(12, 380);
            btnSelectFiles.Name = "btnSelectFiles";
            btnSelectFiles.Size = new Size(225, 30);
            btnSelectFiles.TabIndex = 16;
            btnSelectFiles.Text = "&Select PDF Files...";
            btnSelectFiles.UseVisualStyleBackColor = false;
            btnSelectFiles.Click += btnSelectFiles_Click;
            //
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { statusLabel });
            statusStrip1.Location = new Point(0, 612);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(924, 22);
            statusStrip1.TabIndex = 16;
            statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(909, 17);
            statusLabel.Spring = true;
            statusLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // EnD
            // 
            AcceptButton = buttonDecrypt;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            CancelButton = buttonClose;
            ClientSize = new Size(924, 634);
            Controls.Add(btnOpenPWDMgr);
            Controls.Add(btnSelectFiles);
            Controls.Add(checkBoxRemoveSecurity);
            Controls.Add(checkBoxRecurseDir);
            Controls.Add(buttonClose);
            Controls.Add(checkBoxOverwrite);
            Controls.Add(buttonDecrypt);
            Controls.Add(textBoxPassword);
            Controls.Add(label1);
            Controls.Add(linkLabelFolderName);
            Controls.Add(listFilesFolders);
            Controls.Add(groupBoxOverwrite);
            Controls.Add(panel1);
            Controls.Add(statusStrip1);
            ForeColor = Color.WhiteSmoke;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Name = "EnD";
            Text = "Decrypt PDFs";
            FormClosing += EnD_FormClosing;
            Shown += EnD_Shown;
            KeyDown += EnD_KeyDown;
            groupBoxOverwrite.ResumeLayout(false);
            groupBoxOverwrite.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView listFilesFolders;
        private ColumnHeader columnHeaderNo;
        private ColumnHeader columnHeaderFilename;
        private ColumnHeader columnHeaderState;
        private ColumnHeader columnHeaderComment;
        private LinkLabel linkLabelFolderName;
        private Label label1;
        private TextBox textBoxPassword;
        private GroupBox groupBoxOverwrite;
        private RadioButton radioButtonPrefix;
        private TextBox textBoxOverwriteString;
        private RadioButton radioButtonSuffix;
        private CheckBox checkBoxOverwrite;
        private Button buttonDecrypt;
        private Button buttonClose;
        private CheckBox checkBoxRecurseDir;
        private Panel panel1;
        private CheckBox checkBoxRemoveSecurity;
        private Label labelCountPasswordProtected;
        private Label labelCountHasSecuritySettings;
        private Label labelCountNoSecurity;
        private Label labelCountError;
        private Label label2;
        private Label label3;
        private Label label5;
        private Label label4;
        private Button btnOpenPWDMgr;
        private Button btnSelectFiles;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel statusLabel;
    }
}
