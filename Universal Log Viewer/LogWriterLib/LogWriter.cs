
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LogWriterLib
{
    
    public class LogWriter
    {
        TextWriter _oTextWriter;

        public enum TypeLogMessage { LMT_ERROR=0, LMT_FATAL=1, LMT_WARN=2, LMT_INFORM=3 };
        private String[] aTypeMessages = { "ERROR", "FATAL", "WARN", "INFORM" };

        public LogWriter(String vsFileName)
        {
            _oTextWriter = new StreamWriter(vsFileName, true);
        }

        ~LogWriter()
        {
            
        }

        public int WriteLog(TypeLogMessage ventType, String vsMessage)
        {
            string sMessage = "[" + Convert.ToString(DateTime.Now) + "]\t";
            sMessage += "[" + aTypeMessages[(int)ventType] + "]\t" + vsMessage;
            _oTextWriter.WriteLine((string)sMessage);
            _oTextWriter.Flush();
            return 0;
        }

    }
}
