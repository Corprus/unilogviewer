﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

[assembly: CLSCompliant(true)]
namespace LogWriter
{
    public enum LWErrorCodes { EC_PATH_TOO_LONG = -2, EC_ERROR = -1, EC_SUCCESS = 0, EC_DIRECTORY_CREATED = 1 }

    public class LogWriter
    {
        private TextWriter _oTextWriter;
        private String sLogSeparator = "\t";        

        public enum TypeLogMessage { LMT_ERROR=0, LMT_FATAL=1, LMT_WARN=2, LMT_INFORM=3 };
        private String[] aTypeMessages = { "ERROR", "FATAL", "WARN", "INFORM" };

        public LogWriter(String vsFileName, out LWErrorCodes riResultCode)
        {
            riResultCode = LWErrorCodes.EC_SUCCESS;
            var s = Directory.GetParent(vsFileName);
            if (!s.Exists)
            {
                try
                {
                    s.Create();
                    riResultCode = LWErrorCodes.EC_DIRECTORY_CREATED;
                }
                catch (Exception)
                {
                    riResultCode = LWErrorCodes.EC_ERROR;
                    return;
                }
            }

            try
            {
                _oTextWriter = new StreamWriter(vsFileName, true);
            }
            catch (System.IO.PathTooLongException)
            {
                riResultCode = LWErrorCodes.EC_PATH_TOO_LONG;
                return;
            }
            catch (Exception)
            {
                riResultCode = LWErrorCodes.EC_ERROR;
                return;
            }           
        }

        ~LogWriter()
        {
            
        }


        public LWErrorCodes WriteLog(TypeLogMessage ventType, String vsMessage)
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
                return LWErrorCodes.EC_ERROR;
            }
            return LWErrorCodes.EC_SUCCESS;
        }

    }
}
