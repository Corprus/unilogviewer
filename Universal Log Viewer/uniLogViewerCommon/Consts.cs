using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalLogViewer.Common
{
    public static class Consts
    {
        public const string ERROR_LOG_FILENAME = "unilogviewer.log";
        public const string INCONSISTENCIES_LOG_FILENAME = "inconsistencies.log";
        public const string EMPTY_SYMBOL = "";
        public const char ARRAY_SEPARATOR = ',';
        public const string KEY_NAME = "Name";
        public const string SETTINGS_INI_FILE_NAME = "logviewer.ini";
        public const string LOG_TYPE_EXTENSION = "ltp";
        public const string ASK_ADD_LOG_TYPE_WITH_SAME_NAME = "The Log type with the same name is already in manager. Do you want to add new one?";
        public const string HEADER_SAME_LOG_TYPE_PRESENT = "Same log type is present";
        public const string SELECT_CORRECT_LOG_TYPE = "Please select correct log type from the list";
        public const string TEXT_DELETE_LOG_TYPE = "Do you really want to delete log type (file will be deleted permanently)?";
        public const string HEADER_DELETE_LOG_TYPE = "Do you really want to delete?";
        public const System.Windows.Forms.MessageBoxOptions DEFAULT_MESSAGE_BOX_OPTIONS = (System.Windows.Forms.MessageBoxOptions)0;
        public static System.Drawing.Font DFAULT_FONT = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 8);

    }
}
