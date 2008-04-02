using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Universal_Log_Viewer.Common;
using IniFile;
using System.Windows.Forms;

namespace Universal_Log_Viewer.Types.Managers
{
    public static class CIniSettingsManager
    {

        const string SECTION_PATHS = "Paths";
        const string KEY_LOG_TYPES_SUBFOLDER = "LogTypesSubFolder";
        const string SECTION_LOG_TYPES = "Log Types";
        const string KEY_OPEN_COMMAND = "OpenCommand";        

        static CAutoSavedIniFile IniFile;
        public static string LogTypesFolder
        {
            get
            {
                if (IniFile != null)
                    return Application.StartupPath + "\\" + IniFile.IniReadValue(SECTION_PATHS, KEY_LOG_TYPES_SUBFOLDER);
                else
                    return Application.StartupPath;
            }
        }
        public static string OpenLogTypeCommand
        {
            get
            {
                if (IniFile != null)
                    return IniFile.IniReadValue(SECTION_LOG_TYPES, KEY_OPEN_COMMAND);
                else
                    return "";
            }
        }
        public static bool UseExternalOpen { get { return (OpenLogTypeCommand != ""); } }
        
        public static void InitIniFile()
        {
            IniFile = new CAutoSavedIniFile(Application.StartupPath + "\\" + Consts.SETTINGS_INI_FILE_NAME);
        }


    }
}
