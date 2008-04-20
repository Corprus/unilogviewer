using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using UniversalLogViewer.Common.Types.Managers;

namespace UniversalLogViewer.Common.Exceptions
{
    public class ExceptionLogWriter
    {
        static LogWriting.LogWriter _Instance;
        static LogWriting.LogWriter _InconsistenciesInstance;

        public static LogWriting.LogWriter Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new LogWriting.LogWriter(Consts.ERROR_LOG_FILENAME);
                return _Instance;

            }
        }
        public static LogWriting.LogWriter InconsistenciesInstance
        {
            get
            {
                if (_InconsistenciesInstance == null)
                    _InconsistenciesInstance = new LogWriting.LogWriter(Consts.INCONSISTENCIES_LOG_FILENAME, !(IniSettingsManager.ClearOldInconsistenciesContents));
                return _InconsistenciesInstance;

            }
        }
    }


    public class UniLogViewerException : Exception
    {
        protected virtual LogWriting.TypeLogMessage ExceptionLevel
        {
            get
            {
                return LogWriting.TypeLogMessage.LMT_ERROR;
            }

        }

        protected virtual void WriteMessage(string message)
        {            
            LogTypeLoadException ExceptionConversion = this as LogTypeLoadException;
            if ((ExceptionConversion != null)&&(IniSettingsManager.UseSeparateInconsistenciesLog))
            {
                ExceptionLogWriter.InconsistenciesInstance.WriteLog(ExceptionLevel, this.GetType().Name + message);
            }
            else
                ExceptionLogWriter.Instance.WriteLog(ExceptionLevel, this.GetType().Name + message);
        }
        
        public UniLogViewerException(string message)
            :base(message)
        {            
            WriteMessage(message);
            if (ExceptionLevel == LogWriting.TypeLogMessage.LMT_FATAL)
            {
                System.Windows.Forms.MessageBox.Show(message, "FATAL Error happened. \n Program will terminate now", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, UniversalLogViewer.Common.Consts.DEFAULT_MESSAGE_BOX_OPTIONS);
                System.Windows.Forms.Application.Exit();
            }

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
            if (ExceptionLevel == LogWriting.TypeLogMessage.LMT_FATAL)
                System.Windows.Forms.Application.Exit();           

        }
        public UniLogViewerException(string message, Exception e, bool ThrowExternal)
            : base(message, e)
        {
            if (message.Length == 0)
                message = e.Message;

            WriteMessage("(" + e.GetType().Name + ") " + message);
            if (ThrowExternal)
            {
                if (ExceptionLevel == LogWriting.TypeLogMessage.LMT_FATAL)
                {
                    System.Windows.Forms.MessageBox.Show("FATAL Error: " + e.GetType().Name, "UNhandled FATAL Internal Error " + e.GetType().Name + "happened. \n Program will terminate now \n You can see aditional error information in eror log file", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, UniversalLogViewer.Common.Consts.DEFAULT_MESSAGE_BOX_OPTIONS);
                    WriteMessage("Stack trace: \n" + e.StackTrace);

                }
                throw e;
            }
            if (ExceptionLevel == LogWriting.TypeLogMessage.LMT_FATAL)
            {
                WriteMessage("Stack trace: \n" + e.StackTrace);
                System.Windows.Forms.MessageBox.Show(message, "Handled FATAL Internal Error happened. \n Program will terminate now", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, UniversalLogViewer.Common.Consts.DEFAULT_MESSAGE_BOX_OPTIONS);
                System.Windows.Forms.Application.Exit();
            }
        }
    }
    public class FatalUnhandledException : UniLogViewerException
    {
        protected override LogWriting.TypeLogMessage ExceptionLevel
        {
            get
            {
                return LogWriting.TypeLogMessage.LMT_FATAL;
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
                return LogWriting.TypeLogMessage.LMT_FATAL;
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
