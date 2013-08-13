using System;

namespace IniFiles.Exceptions
{
    public class IniFileException : Exception
    {
        public IniFileException(string message)
            : base(message)
        {
        }
    }
    public class IniFileWriteException : IniFileException
    {
        public IniFileWriteException(string message)
            : base(message)
        {
        }
    }
    public class IniFileReadException : IniFileException
    {
        public IniFileReadException(string message)
            : base(message)
        {
        }
    }
    public class IniFileRequiredFieldReadException : IniFileException
    {
        public IniFileRequiredFieldReadException(string message)
            : base(message)
        {
        }
    }

    public class IniFileSectionsReadException : IniFileReadException
    {
        public IniFileSectionsReadException(string message)
            : base(message)
        {
        }
    }


}
