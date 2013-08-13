using System;

namespace LogWriting
{
    public class ErrorLogWriter :LogWriter
    {
        private readonly String[] _errorTypes= { "Panic", "Kernel Panic" };
        public ErrorLogWriter(String vsFileName, Boolean vbSaveOldContent)
            : base(vsFileName, vbSaveOldContent)
        {

        }

        public ErrorCode WriteLog(TypeLogMessage ventType, String vsMessage, ErrorType vsErrorType)
        {
            vsMessage = _errorTypes[(int)vsErrorType] + LogSeparator + vsMessage;
            return base.WriteLog(ventType, vsMessage);
        }

        protected override String GetHeaders()
        {
            return string.Format("{0}{1}{2}{1}{3}{1}{4}", Headers[0], LogSeparator, Headers[3], Headers[1], Headers[2]);
        }

    }
}
