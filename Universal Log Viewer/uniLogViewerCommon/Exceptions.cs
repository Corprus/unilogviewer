using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using UniversalLogViewer.Common.Types.Managers;

namespace UniversalLogViewer.Common.Exceptions
{
    public class ExceptionLogWriter
    {
        static LogWriting.LogWriter _instance;
        static LogWriting.LogWriter _inconsistenciesInstance;

        public static LogWriting.LogWriter Instance
        {
            get
            {
                return _instance ??
                       (_instance =
                        new LogWriting.LogWriter(string.Format("{0}\\{1}", Application.StartupPath,
                                                               Consts.ErrorLogFilename)));
            }
        }
        public static LogWriting.LogWriter InconsistenciesInstance
        {
            get {
                return _inconsistenciesInstance ??
                       (_inconsistenciesInstance =
                        new LogWriting.LogWriter(Consts.InconsistenciesLogFilename,
                                                 !(IniSettingsManager.ClearOldInconsistenciesContents)));
            }
        }
    }


    public class UniLogViewerException : Exception
    {
        protected virtual LogWriting.TypeLogMessage ExceptionLevel
        {
            get
            {
                return LogWriting.TypeLogMessage.Error;
            }

        }

        private void WriteMessage(string message)
        {            
            var exceptionConversion = this as LogTypeLoadException;
            if ((exceptionConversion != null)&&(IniSettingsManager.UseSeparateInconsistenciesLog))
            {
                ExceptionLogWriter.InconsistenciesInstance.WriteLog(ExceptionLevel, GetType().Name + message);
            }
            else
                ExceptionLogWriter.Instance.WriteLog(ExceptionLevel, GetType().Name + message);
        }
        
        public UniLogViewerException(string message)
            :base(message)
        {            
            WriteMessage(message);
            if (ExceptionLevel != LogWriting.TypeLogMessage.Fatal) return;
            MessageBox.Show(message, "FATAL Error happened. \n Program will terminate now", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, UniversalLogViewer.Common.Consts.DefaultMessageBoxOptions);
            Application.Exit();
        }
        public UniLogViewerException(Exception e)
            :this(e.Message, e)
        {
        }


        public UniLogViewerException(string message, Exception e)
            : base(message, e)
        {
            if (message.Length == 0)
                message = e.Message;
            WriteMessage("(" + e.GetType().Name + ") " + message);
            if (ExceptionLevel == LogWriting.TypeLogMessage.Fatal)
                Application.Exit();           

        }
        public UniLogViewerException(string message, Exception e, bool throwExternal)
            : base(message, e)
        {
            if (message.Length == 0)
                message = e.Message;

            WriteMessage(string.Format("({0}) {1}", e.GetType().Name, message));
            if (throwExternal)
            {
                if (ExceptionLevel == LogWriting.TypeLogMessage.Fatal)
                {
                    MessageBox.Show(string.Format("FATAL Error: {0}", e.GetType().Name),
                                    string.Format("Unhandled FATAL Internal Error {0} happened. \n Program will terminate now. \n You can see aditional error information in eror log file.", e.GetType().Name),
                                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                                    Consts.DefaultMessageBoxOptions);
                    WriteMessage(string.Format("Stack trace: \n{0}", e.StackTrace));

                }
                throw e;
            }
            if (ExceptionLevel == LogWriting.TypeLogMessage.Fatal)
            {
                WriteMessage("Stack trace: \n" + e.StackTrace);
                MessageBox.Show(message, "Handled FATAL Internal Error happened. \n Program will terminate now", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, UniversalLogViewer.Common.Consts.DefaultMessageBoxOptions);
                Application.Exit();
            }
        }
    }
    public class FatalUnhandledException : UniLogViewerException
    {
        protected override LogWriting.TypeLogMessage ExceptionLevel
        {
            get
            {
                return LogWriting.TypeLogMessage.Fatal;
            }
        }
        public FatalUnhandledException(Exception e)
            : base("", e, true)
        {
        }

    }
    public class LogIniException : UniLogViewerException
    {
        public LogIniException(string message)
            : base(message)
        {
        }
        public LogIniException(string message, Exception e)
            : base(message, e)
        {
        }
    }
    public class LogTypeLoadException : UniLogViewerException
    {
        public LogTypeLoadException(string message)
            : base(message)
        {
        }
        public LogTypeLoadException(string message, Exception e)
            : base(message, e)
        {
        }

    }
    public class LogSettingsIniException : UniLogViewerException
    {
        protected override LogWriting.TypeLogMessage ExceptionLevel
        {
            get
            {
                return LogWriting.TypeLogMessage.Fatal;
            }
        }
        public LogSettingsIniException(string message)
            : base(message)
        {
        }
        public LogSettingsIniException(Exception e)
            : base("", e)
        {
        }

    }

    public class LogIniReadException : LogIniException
    {
        public LogIniReadException(string message)
            : base(message)
        {
        }
        public LogIniReadException(string message, Exception e)
            : base(message, e)
        {
        }
    }
    public class LogIniRequiredFieldMissingException : LogIniReadException
    {
        public LogIniRequiredFieldMissingException(string message)
            : base(message)
        {
        }
        public LogIniRequiredFieldMissingException(string message, Exception e)
            : base(message, e)
        {
        }
    }

    public class LogIniSectionReadException : LogIniReadException
    {
        public LogIniSectionReadException(string message)
            : base(message)
        {
        }
        public LogIniSectionReadException(string message, Exception e)
            : base(message, e)
        {
        }
    }
}
