namespace Universal_Log_Viewer.UI
{
    partial class FormAbout
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
            this.lblProjectNameHeader = new System.Windows.Forms.Label();
            this.lblVersionHeader = new System.Windows.Forms.Label();
            this.lblRevisionHeader = new System.Windows.Forms.Label();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblRevision = new System.Windows.Forms.Label();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.lblAuthorLink = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lblProjectNameHeader
            // 
            this.lblProjectNameHeader.AutoSize = true;
            this.lblProjectNameHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblProjectNameHeader.Location = new System.Drawing.Point(30, 63);
            this.lblProjectNameHeader.Name = "lblProjectNameHeader";
            this.lblProjectNameHeader.Size = new System.Drawing.Size(79, 13);
            this.lblProjectNameHeader.TabIndex = 0;
            this.lblProjectNameHeader.Text = "ProjectName";
            // 
            // lblVersionHeader
            // 
            this.lblVersionHeader.AutoSize = true;
            this.lblVersionHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblVersionHeader.Location = new System.Drawing.Point(30, 88);
            this.lblVersionHeader.Name = "lblVersionHeader";
            this.lblVersionHeader.Size = new System.Drawing.Size(49, 13);
            this.lblVersionHeader.TabIndex = 1;
            this.lblVersionHeader.Text = "Version";
            // 
            // lblRevisionHeader
            // 
            this.lblRevisionHeader.AutoSize = true;
            this.lblRevisionHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblRevisionHeader.Location = new System.Drawing.Point(30, 113);
            this.lblRevisionHeader.Name = "lblRevisionHeader";
            this.lblRevisionHeader.Size = new System.Drawing.Size(56, 13);
            this.lblRevisionHeader.TabIndex = 2;
            this.lblRevisionHeader.Text = "Revision";
            // 
            // lblProjectName
            // 
            this.lblProjectName.AutoSize = true;
            this.lblProjectName.Location = new System.Drawing.Point(143, 63);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(109, 13);
            this.lblProjectName.TabIndex = 4;
            this.lblProjectName.Text = "%PROJECT_NAME%";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(143, 88);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(71, 13);
            this.lblVersion.TabIndex = 5;
            this.lblVersion.Text = "%VERSION%";
            // 
            // lblRevision
            // 
            this.lblRevision.AutoSize = true;
            this.lblRevision.Location = new System.Drawing.Point(143, 113);
            this.lblRevision.Name = "lblRevision";
            this.lblRevision.Size = new System.Drawing.Size(74, 13);
            this.lblRevision.TabIndex = 6;
            this.lblRevision.Text = "%REVISION%";
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAuthor.Location = new System.Drawing.Point(30, 31);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(44, 13);
            this.lblAuthor.TabIndex = 7;
            this.lblAuthor.Text = "Author";
            // 
            // lblAuthorLink
            // 
            this.lblAuthorLink.AutoSize = true;
            this.lblAuthorLink.Location = new System.Drawing.Point(143, 31);
            this.lblAuthorLink.Name = "lblAuthorLink";
            this.lblAuthorLink.Size = new System.Drawing.Size(102, 13);
            this.lblAuthorLink.TabIndex = 8;
            this.lblAuthorLink.TabStop = true;
            this.lblAuthorLink.Text = "Konstantin Lebedev";
            this.lblAuthorLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAuthorLink_LinkClicked);
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.lblAuthorLink);
            this.Controls.Add(this.lblAuthor);
            this.Controls.Add(this.lblRevision);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblProjectName);
            this.Controls.Add(this.lblRevisionHeader);
            this.Controls.Add(this.lblVersionHeader);
            this.Controls.Add(this.lblProjectNameHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormAbout";
            this.Text = "About Project";
            this.Load += new System.EventHandler(this.FormAbout_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProjectNameHeader;
        private System.Windows.Forms.Label lblVersionHeader;
        private System.Windows.Forms.Label lblRevisionHeader;
        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblRevision;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.LinkLabel lblAuthorLink;
    }
}