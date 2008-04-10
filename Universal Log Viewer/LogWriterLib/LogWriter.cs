
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

[assembly: CLSCompliant(true)]
namespace LogWriting
{
    public enum LWErrorCode { EC_PATH_TOO_LONG = -2, EC_ERROR = -1, EC_SUCCESS = 0, EC_DIRECTORY_CREATED = 1 }
    public enum TypeLogMessage { LMT_ERROR = 0, LMT_FATAL = 1, LMT_WARN = 2, LMT_INFORM = 3 };

    public class LogWriter
    {
        private TextWriter _oTextWriter;
        private String sLogSeparator = "\t";        
        private String[] aTypeMessages = { "ERROR", "FATAL", "WARN", "INFORM" };

        public LWErrorCode lwCreateResultCode {get ; private set;}

        public LogWriter(String vsFileName)
        {
            lwCreateResultCode = LWErrorCode.EC_SUCCESS;
            var s = Directory.GetParent(vsFileName);
            if (!s.Exists)
            {
                try
                {
                    s.Create();
                    lwCreateResultCode = LWErrorCode.EC_DIRECTORY_CREATED;
                }
                catch (Exception)
                {
                    lwCreateResultCode = LWErrorCode.EC_ERROR;
                    return;
                }
            }

            try
            {
                _oTextWriter = new StreamWriter(vsFileName, true);
            }
            catch (System.IO.PathTooLongException)
            {
                lwCreateResultCode = LWErrorCode.EC_PATH_TOO_LONG;
                return;
            }
            catch (Exception)
            {
                lwCreateResultCode = LWErrorCode.EC_ERROR;
                return;
            }           
        }

        ~LogWriter()
        {
            _oTextWriter.Dispose();
        }


        public LWErrorCode WriteLog(TypeLogMessage ventType, String vsMessage)
        {
            string sMessage = "[" + Convert.ToString(DateTime.Now) + "]" + sLogSeparator;
            sMessage += "[" + aTypeMessages[(int)ventType] + "]" + sLogSeparator + vsMessage;
            try
            {
                _oTextWriter.WriteLine((string)sMessage);
                _oTextWriter.Flush();
            }
            catch (Exception)
            {
                return LWErrorCode.EC_ERROR;
            }
            return LWErrorCode.EC_SUCCESS;
        }

    }
}
