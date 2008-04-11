using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UniversalLogViewer.UI;
using UniversalLogViewer.Types.Managers;
using UniversalLogViewer.Common;

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
            try
            {
                Application.Run(new MainForm());
            }
            catch (UniLogViewerException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "Unhandled Internal Error happened. \n Program will terminate now", MessageBoxButtons.OK,  MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, Consts.DEFAULT_MESSAGE_BOX_OPTIONS);
            }
        }
    }
}