using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UniversalLogViewer.UI;
using UniversalLogViewer.Types.Managers;

[assembly:CLSCompliant(true)]
namespace UniversalLogViewer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            IniSettingsManager.InitFile();
            Application.Run(new MainForm());
        }
    }
}