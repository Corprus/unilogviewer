using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogWriting
{
    public class ErrorLogWriter :LogWriter
    {
        private String[] aTypeErrors= { "Panic. Kernel Panic" };
        public ErrorLogWriter(String vsFileName, Boolean vbSaveOldContent)
            : base(vsFileName, vbSaveOldContent)
        {

        }

        public LWErrorCode WriteLog(TypeLogMessage ventType, String vsMessage, LWErrorType vsErrorType)
        {
            vsMessage = aTypeErrors[(int)vsErrorType] + sLogSeparator + vsMessage;
            return base.WriteLog(ventType, vsMessage);
        }

        protected override String sGetHeaders()
        {
            return aHeaders[0] + sLogSeparator + aHeaders[3] + sLogSeparator + aHeaders[1] + sLogSeparator + aHeaders[2];
        }

    }
}
