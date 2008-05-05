using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UniversalLogViewer.UI;
using UniversalLogViewer.Types.Managers;
using UniversalLogViewer.Common.Exceptions;
using UniversalLogViewer.Common.Types.Managers;
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
            UniversalLogViewer.Common.Exceptions.ExceptionLogWriter.Instance.WriteLog(LogWriting.TypeLogMessage.LMT_INFORM, "Program Started");

            try
            {
                try
                {
                    IniSettingsManager.InitFile();
                    Application.Run(new MainForm());
                    UniversalLogViewer.Common.Exceptions.ExceptionLogWriter.Instance.WriteLog(LogWriting.TypeLogMessage.LMT_INFORM, "Program Successfully Finished");
                }
                catch (UniLogViewerException e)
                {
                    //если мы ошибку не прибили заранее, но она не фатальная (фатальный случай прерывает приложение сразу в конструкторе исключения)
                    System.Windows.Forms.MessageBox.Show(e.Message, "Handled Critical Internal Error happened. \n Program will terminate now", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, UniversalLogViewer.Common.Consts.DEFAULT_MESSAGE_BOX_OPTIONS);
                }
                catch (Exception e)
                {
                    throw new Common.Exceptions.FatalUnhandledException(e);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}