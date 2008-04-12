using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversalLogViewer.Common;
using IniFiles;
using System.Windows.Forms;

namespace UniversalLogViewer.Types.Managers
{
    public static class IniSettingsManager
    {

        const string SECTION_PATHS = "Paths";
        const string KEY_LOG_TYPES_SUBFOLDER = "LogTypesSubFolder";
        const string SECTION_LOG_TYPES = "Log Types";
        const string KEY_OPEN_COMMAND = "OpenCommand";
        const string SECTION_VISUAL = "Visual";
        const string KEY_SHOW_VALUE_MEMO = "ShowValueMemo";

        static AutoSavedIniFile IniFile;
        public static string LogTypesFolder
        {
            get
            {
                if (IniFile != null)
                    return Application.StartupPath + "\\" + IniFile.ReadValue(SECTION_PATHS, KEY_LOG_TYPES_SUBFOLDER);
                else
                    return Application.StartupPath;
            }
        }
        public static string OpenLogTypeCommand
        {
            get
            {
                if (IniFile != null)
                    return IniFile.ReadValue(SECTION_LOG_TYPES, KEY_OPEN_COMMAND);
                else
                    return "";
            }
        }
        public static bool UseExternalOpen { get { return (OpenLogTypeCommand.Length != 0); } }
        public static bool ShowValueMemo
        {
            get
            {
                if (IniFile != null)
                    return ((IniFile.ReadValue(SECTION_VISUAL, KEY_SHOW_VALUE_MEMO) == "1"));
                else
                    return false;
            }
        }

        public static void InitFile()
        {
            try
            {
                IniFile = new AutoSavedIniFile(Application.StartupPath + "\\" + Consts.SETTINGS_INI_FILE_NAME);
            }
            catch (System.IO.IOException e)
            {
                throw new Common.Exceptions.LogSettingsIniException(e);
            }
        }


    }
}
