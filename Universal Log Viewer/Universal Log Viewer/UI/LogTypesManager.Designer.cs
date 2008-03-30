namespace Universal_Log_Viewer.UI
{
    partial class frmLogTypesManager
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
            System.Windows.Forms.Panel pnlLoadLogType;
            System.Windows.Forms.GroupBox gbxLogTypeDescription;
            this.memoLogDescription = new System.Windows.Forms.RichTextBox();
            this.pnlLogTypeToolBox = new System.Windows.Forms.Panel();
            this.btnImportLogType = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnReInitAll = new System.Windows.Forms.Button();
            this.btnReInit = new System.Windows.Forms.Button();
            this.lbxLogTypes = new System.Windows.Forms.ListBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.dlgOpenLogType = new System.Windows.Forms.OpenFileDialog();
            this.pnkToolbox = new System.Windows.Forms.Panel();
            pnlLoadLogType = new System.Windows.Forms.Panel();
            gbxLogTypeDescription = new System.Windows.Forms.GroupBox();
            pnlLoadLogType.SuspendLayout();
            gbxLogTypeDescription.SuspendLayout();
            this.pnlLogTypeToolBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLoadLogType
            // 
            pnlLoadLogType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            pnlLoadLogType.Controls.Add(gbxLogTypeDescription);
            pnlLoadLogType.Controls.Add(this.pnlLogTypeToolBox);
            pnlLoadLogType.Controls.Add(this.lbxLogTypes);
            pnlLoadLogType.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlLoadLogType.Location = new System.Drawing.Point(0, 0);
            pnlLoadLogType.Name = "pnlLoadLogType";
            pnlLoadLogType.Size = new System.Drawing.Size(525, 378);
            pnlLoadLogType.TabIndex = 1;
            // 
            // gbxLogTypeDescription
            // 
            gbxLogTypeDescription.Controls.Add(this.memoLogDescription);
            gbxLogTypeDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            gbxLogTypeDescription.Location = new System.Drawing.Point(296, 0);
            gbxLogTypeDescription.Name = "gbxLogTypeDescription";
            gbxLogTypeDescription.Size = new System.Drawing.Size(225, 374);
            gbxLogTypeDescription.TabIndex = 6;
            gbxLogTypeDescription.TabStop = false;
            gbxLogTypeDescription.Text = "LogType Description";
            // 
            // memoLogDescription
            // 
            this.memoLogDescription.BackColor = System.Drawing.SystemColors.Control;
            this.memoLogDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.memoLogDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoLogDescription.Location = new System.Drawing.Point(3, 16);
            this.memoLogDescription.Name = "memoLogDescription";
            this.memoLogDescription.Size = new System.Drawing.Size(219, 355);
            this.memoLogDescription.TabIndex = 5;
            this.memoLogDescription.Text = "";
            // 
            // pnlLogTypeToolBox
            // 
            this.pnlLogTypeToolBox.Controls.Add(this.btnImportLogType);
            this.pnlLogTypeToolBox.Controls.Add(this.btnDelete);
            this.pnlLogTypeToolBox.Controls.Add(this.btnReInitAll);
            this.pnlLogTypeToolBox.Controls.Add(this.btnReInit);
            this.pnlLogTypeToolBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLogTypeToolBox.Location = new System.Drawing.Point(180, 0);
            this.pnlLogTypeToolBox.Name = "pnlLogTypeToolBox";
            this.pnlLogTypeToolBox.Size = new System.Drawing.Size(116, 374);
            this.pnlLogTypeToolBox.TabIndex = 5;
            // 
            // btnImportLogType
            // 
            this.btnImportLogType.Location = new System.Drawing.Point(6, 91);
            this.btnImportLogType.Name = "btnImportLogType";
            this.btnImportLogType.Size = new System.Drawing.Size(104, 23);
            this.btnImportLogType.TabIndex = 4;
            this.btnImportLogType.Text = "Import Log Type";
            this.btnImportLogType.UseVisualStyleBackColor = true;
            this.btnImportLogType.Click += new System.EventHandler(this.btnImportLogType_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(6, 250);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnReInitAll
            // 
            this.btnReInitAll.Location = new System.Drawing.Point(6, 197);
            this.btnReInitAll.Name = "btnReInitAll";
            this.btnReInitAll.Size = new System.Drawing.Size(104, 23);
            this.btnReInitAll.TabIndex = 2;
            this.btnReInitAll.Text = "Re-Init All";
            this.btnReInitAll.UseVisualStyleBackColor = true;
            this.btnReInitAll.Click += new System.EventHandler(this.btnReInitAll_Click);
            // 
            // btnReInit
            // 
            this.btnReInit.Location = new System.Drawing.Point(6, 144);
            this.btnReInit.Name = "btnReInit";
            this.btnReInit.Size = new System.Drawing.Size(104, 23);
            this.btnReInit.TabIndex = 1;
            this.btnReInit.Text = "Re-Init";
            this.btnReInit.UseVisualStyleBackColor = true;
            this.btnReInit.Click += new System.EventHandler(this.btnReInit_Click);
            // 
            // lbxLogTypes
            // 
            this.lbxLogTypes.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbxLogTypes.FormattingEnabled = true;
            this.lbxLogTypes.Location = new System.Drawing.Point(0, 0);
            this.lbxLogTypes.Name = "lbxLogTypes";
            this.lbxLogTypes.Size = new System.Drawing.Size(180, 368);
            this.lbxLogTypes.TabIndex = 3;
            this.lbxLogTypes.SelectedValueChanged += new System.EventHandler(this.lbxLogTypes_SelectedValueChanged);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(438, 390);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dlgOpenLogType
            // 
            this.dlgOpenLogType.Filter = "LogType | *.ltp";
            // 
            // pnkToolbox
            // 
            this.pnkToolbox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnkToolbox.Location = new System.Drawing.Point(0, 378);
            this.pnkToolbox.Name = "pnkToolbox";
            this.pnkToolbox.Size = new System.Drawing.Size(525, 47);
            this.pnkToolbox.TabIndex = 2;
            // 
            // frmLogTypesManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(525, 425);
            this.Controls.Add(pnlLoadLogType);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnkToolbox);
            this.Name = "frmLogTypesManager";
            this.Text = "Log Types Manager";
            pnlLoadLogType.ResumeLayout(false);
            gbxLogTypeDescription.ResumeLayout(false);
            this.pnlLogTypeToolBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.OpenFileDialog dlgOpenLogType;
        private System.Windows.Forms.Panel pnkToolbox;
        private System.Windows.Forms.ListBox lbxLogTypes;
        private System.Windows.Forms.Panel pnlLogTypeToolBox;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnReInitAll;
        private System.Windows.Forms.Button btnReInit;
        private System.Windows.Forms.Button btnImportLogType;
        private System.Windows.Forms.RichTextBox memoLogDescription;
    }
}