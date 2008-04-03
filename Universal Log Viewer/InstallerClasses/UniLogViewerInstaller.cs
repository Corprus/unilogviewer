using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace InstallerClasses
{
    [RunInstaller(true)]
    public partial class WhatsNew : Installer
    {
    
        public WhatsNew()
        {
            InitializeComponent();
        }
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
            System.Diagnostics.Process batch = new System.Diagnostics.Process();
            batch.StartInfo.FileName = "notepad.exe";
            string InstallPath = this.Context.Parameters["assemblypath"];
            InstallPath = InstallPath.Substring(0, InstallPath.LastIndexOf("\\"));
            batch.StartInfo.WorkingDirectory = this.Context.Parameters["assemblypath"];
            batch.StartInfo.Arguments = "\"" + InstallPath + "\\Docs\\WhatsNew.txt\"";
            batch.Start();
            
        }

        private void InitializeComponent()
        {

        }
    }
}
