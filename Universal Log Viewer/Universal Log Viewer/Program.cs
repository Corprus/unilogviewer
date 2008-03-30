using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Universal_Log_Viewer.UI;
using Universal_Log_Viewer.Types.Managers;

namespace Universal_Log_Viewer
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
            CIniSettingsManager.InitIniFile();
            Application.Run(new MainForm());
        }
    }
}