using IniFiles;
using System.Windows.Forms;

namespace UniversalLogViewer.Common.Types.Managers
{
    public static class IniSettingsManager
    {

        const string SectionPaths = "Paths";
        const string KeyLogTypesSubfolder = "LogTypesSubFolder";
        const string SectionLogTypes = "Log Types";
        const string KeySeparateLog = "SeparateLog";
        const string KeyClearOldContents = "ClearOldContents";
        const string KeyOpen = "OpenIfGenerated";
        const string KeyOpenCommand = "OpenCommand";
        const string SectionVisual = "Visual";
        const string KeyShowValueMemo = "ShowValueMemo";

        static AutoSavedIniFile _iniFile;
        public static string LogTypesFolder
        {
            get {
                return _iniFile != null
                           ? Application.StartupPath + "\\" + _iniFile.ReadValue(SectionPaths, KeyLogTypesSubfolder)
                           : Application.StartupPath;
            }
        }

        public static string OpenLogTypeCommand
        {
            get { return _iniFile != null ? _iniFile.ReadValue(SectionLogTypes, KeyOpenCommand) : ""; }
        }

        public static bool UseSeparateInconsistenciesLog
        {
            get
            {
                return GetBoolIniValue(SectionLogTypes, KeySeparateLog, true);
            }
        }
        public static bool ClearOldInconsistenciesContents
        {
            get
            {
                return GetBoolIniValue(SectionLogTypes, KeyClearOldContents, false);
            }
        }
        public static bool OpenInconsistenciesLogIfGenerated
        {
            get
            {
                return GetBoolIniValue(SectionLogTypes, KeyOpen, true);
            }
        }

        public static bool UseExternalOpen { get { return (OpenLogTypeCommand.Length != 0); } }
        public static bool ShowValueMemo
        {
            get
            {
                return GetBoolIniValue(SectionVisual, KeyShowValueMemo, false);
            }
        }

        public static void InitFile()
        {
            try
            {
                _iniFile = new AutoSavedIniFile(Application.StartupPath + "\\" + Consts.SettingsIniFileName);
            }
            catch (System.IO.IOException e)
            {
                throw new Exceptions.LogSettingsIniException(e);
            }
        }
        private static bool GetBoolIniValue(string section, string key, bool defaultValue)
        {
            return _iniFile != null ? _iniFile.ReadBoolValue(section, key, defaultValue) : defaultValue;
        }
    }
}
