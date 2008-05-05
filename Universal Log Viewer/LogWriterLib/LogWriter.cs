
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

[assembly: CLSCompliant(true)]
namespace LogWriting
{
    public enum LWErrorCode { EC_PATH_TOO_LONG = -2, EC_ERROR = -1, EC_SUCCESS = 0, EC_DIRECTORY_CREATED = 1 };
    public enum LWErrorType { KERNEL_PANIC = 0 };
    public enum TypeLogMessage { LMT_ERROR = 0, LMT_FATAL = 1, LMT_WARN = 2, LMT_INFORM = 3 };

    public class LogWriter : IDisposable
    {
        private TextWriter _oTextWriter;
        protected const String sLogSeparator = "\t";        
        private String[] aTypeMessages = { "ERROR", "FATAL", "WARN", "INFORM" };

        public LWErrorCode ErrorResultCode {get ; private set;}

        public LogWriter(String vsFileName, Boolean vbSaveOldContent)
        {
            ErrorResultCode = LWErrorCode.EC_SUCCESS;
            var s = Directory.GetParent(vsFileName);
            if (!s.Exists)
            {
                try
                {
                    s.Create();
                    ErrorResultCode = LWErrorCode.EC_DIRECTORY_CREATED;
                }
                catch (Exception)
                {
                    ErrorResultCode = LWErrorCode.EC_ERROR;
                    return;
                }
            }

            try
            {
                _oTextWriter = new StreamWriter(vsFileName, vbSaveOldContent);
            }
            catch (System.IO.PathTooLongException)
            {
                ErrorResultCode = LWErrorCode.EC_PATH_TOO_LONG;
                return;
            }
            catch (Exception)
            {
                ErrorResultCode = LWErrorCode.EC_ERROR;
                return;
            }           
        }

        public LogWriter(String vsFileName)
            :this(vsFileName, true)
        {

        }

        public void Dispose()
        {
            this.Dispose(false);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool DisposeUnmanaged)
        {
            if (_oTextWriter != null)
            {
                _oTextWriter.Close();
                _oTextWriter.Dispose();
            }
        }

        public LWErrorCode WriteLog(TypeLogMessage ventType, String vsMessage)
        {
            string sMessage = "[" + Convert.ToString(DateTime.Now, System.Globalization.CultureInfo.CurrentCulture) + "]" + sLogSeparator;
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
