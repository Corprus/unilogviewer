using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalLogViewer.Common
{
    class ExceptionLogWriter
    {
        static LogWriting.LogWriter _Instance;

        public static LogWriting.LogWriter Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new LogWriting.LogWriter(Consts.ERROR_LOG_FILENAME);
                return _Instance;

            }
        }        
    }


    public class UniLogViewerException : Exception
    {
        protected virtual void WriteMessage(string message)
        {
            ExceptionLogWriter.Instance.WriteLog(LogWriting.TypeLogMessage.LMT_ERROR, message);
        }
        public UniLogViewerException(string message)
            :base(message)
        {
            WriteMessage(message);

        }
    }
}
