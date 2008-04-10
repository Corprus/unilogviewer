using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniversalLogViewer.Common;

namespace UniversalLogViewer.UI
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            LoadTexts();
        }
        private void LoadTexts()
        {
            lblAuthorLink.Links[0].LinkData = "mailto: lazer999@gmail.com";
            lblProjectName.Text = Application.ProductName;
            lblVersion.Text = Application.ProductVersion;
            lblRevision.Text = Utilities.ProjectRevision;
        }

        private void lblAuthorLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string target = e.Link.LinkData as string;
            System.Diagnostics.Process.Start(target);


        }        
    }
}
