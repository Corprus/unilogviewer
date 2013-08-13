using System;
using System.IO;

[assembly: CLSCompliant(true)]
namespace LogWriting
{
    public enum ErrorCode { PathTooLong = -2, Error = -1, Success = 0, DirectoryCreated = 1 };
    public enum ErrorType { KernelPanic = 0 };
    public enum TypeLogMessage { Error = 0, Fatal = 1, Warning = 2, Information = 3 };

    public class LogWriter : IDisposable
    {
        private readonly TextWriter _textWriter;
        protected const String LogSeparator = "\t";        
        private readonly String[] _typeMessages = { "ERROR", "FATAL", "WARN", "INFORM" };
        protected readonly String[] Headers = { "Time", "Type", "Message", "ErrorType" };

        public ErrorCode ErrorResultCode { get; private set; }

        public LogWriter(String vsFileName, Boolean vbSaveOldContent = true)
        {
            ErrorResultCode = ErrorCode.Success;
            var s = Directory.GetParent(vsFileName);
            if (!s.Exists)
            {
                try
                {
                    s.Create();
                    ErrorResultCode = ErrorCode.DirectoryCreated;
                }
                catch (Exception)
                {
                    ErrorResultCode = ErrorCode.Error;
                    return;
                }
            }

            try
            {
                _textWriter = new StreamWriter(vsFileName, vbSaveOldContent);
            }
            catch (PathTooLongException)
            {
                ErrorResultCode = ErrorCode.PathTooLong;
            }
            catch (Exception)
            {
                ErrorResultCode = ErrorCode.Error;
            }           
        }

        public void Dispose()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposeUnmanaged)
        {
            if (_textWriter != null)
            {
                _textWriter.Close();
                _textWriter.Dispose();
            }
        }

        public ErrorCode WriteLog(TypeLogMessage ventType, String vsMessage)
        {
            string sMessage = string.Format("[{0}]{1}[{2}]{1}{3}", DateTime.Now, LogSeparator,
                                            _typeMessages[(int) ventType], vsMessage);
            try
            {
                _textWriter.WriteLine(sMessage);
                _textWriter.Flush();
            }
            catch (Exception)
            {
                return ErrorCode.Error;
            }
            return ErrorCode.Success;
        }

        protected virtual String GetHeaders()
        {
            return string.Format("{0}{1}{2}{1}{3}", Headers[0], LogSeparator, Headers[1], Headers[2]);
        }

        protected ErrorCode WriteHeaders(String vsHeaders)
        {

            try
            {
                _textWriter.WriteLine(vsHeaders);
                _textWriter.Flush();
            }
            catch (Exception)
            {
                return ErrorCode.Error;
            }
            return ErrorCode.Success;
        }

        public ErrorCode WriteHeaders()
        {
            return WriteHeaders(GetHeaders());
        }

    }
}
