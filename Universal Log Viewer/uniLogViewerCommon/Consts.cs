using System.Windows.Forms;

namespace UniversalLogViewer.Common
{
    public static class Consts
    {
        public const string ErrorLogFilename = "unilogviewer.log";
        public const string InconsistenciesLogFilename = "inconsistencies.log";
        public const string EmptySymbol = "";
        public const char ArraySeparator = ',';
        public const string KeyName = "Name";
        public const string SettingsIniFileName = "logviewer.ini";
        public const string LogTypeExtension = "ltp";

        public const string AskAddLogTypeWithSameName =
            "The Log type with the same name is already in manager. Do you want to add new one?";

        public const string HeaderSameLogTypePresent = "Same log type is present";
        public const string SelectCorrectLogType = "Please select correct log type from the list";

        public const string TextDeleteLogType =
            "Do you really want to delete log type (file will be deleted permanently)?";

        public const string HeaderDeleteLogType = "Do you really want to delete?";
        public const MessageBoxOptions DefaultMessageBoxOptions = 0;
    }
}