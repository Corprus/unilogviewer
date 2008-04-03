using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Universal_Log_Viewer.Types.Structures;
using Universal_Log_Viewer.Common;


namespace Universal_Log_Viewer.Types.Managers
{
    public class CLogTypeManager
    {
        public static void ReInit()
        {
            _oInstance = new CLogTypeManager();

        }

        static CLogTypeManager _oInstance;
        public void UpdateList(IList Items)
        {
            Items.Clear();
            foreach (CLogType LogType in TypesList)
                Items.Add(LogType);
        }

        CLogTypeManager()
        {
            TypesList = new List<CLogType>();
            if (!(Directory.Exists(CIniSettingsManager.LogTypesFolder)))
                Directory.CreateDirectory(CIniSettingsManager.LogTypesFolder);            
            string[] sLogTypes = Directory.GetFiles(CIniSettingsManager.LogTypesFolder, "*." + Consts.LOG_TYPE_EXTENSION);
            foreach (string sLogType in sLogTypes)
                TypesList.Add(new CLogType(sLogType));
        }
        public static CLogTypeManager oInstance
        {
            get
            {
                if (_oInstance == null)
                    _oInstance = new CLogTypeManager();
                return _oInstance;

            }
        }
        public List<CLogType> TypesList { get; private set; }
        public void AddLogType(CLogType LogType)
        {
            if (TypesList == null)
                TypesList = new List<CLogType>();
            TypesList.Add(LogType);
        }
        public void AddLogType(string LogTypeIniFileName)
        {
            if (TypesList == null)
                TypesList = new List<CLogType>();
            bool bHasSameName = false;
            
            foreach (CLogType oType in TypesList)
            {
                if (oType.LogName == LogTypeIniFileName)
                    bHasSameName = true;
            }
            bool bAddType = true;
            if (bHasSameName)
               bAddType = (MessageBox.Show(Consts.ASK_ADD_LOG_TYPE_WITH_SAME_NAME, Consts.HEADER_SAME_LOG_TYPE_PRESENT, MessageBoxButtons.YesNo) == DialogResult.Yes);

            if (bAddType)
            {                
                string LogFileIniFileNameWithoutFolders = LogTypeIniFileName.Substring(LogTypeIniFileName.LastIndexOf("\\") + 1, LogTypeIniFileName.Length - LogTypeIniFileName.LastIndexOf("\\") - 1);
                string sNewFileName = LogFileIniFileNameWithoutFolders;

                if (bHasSameName)
                {
                    string[] sLogTypes = Directory.GetFiles(CIniSettingsManager.LogTypesFolder, sNewFileName);
                    int i = 0;
                    while (sLogTypes.Length > 0)
                    {
                        sNewFileName = LogFileIniFileNameWithoutFolders.Substring(0, LogFileIniFileNameWithoutFolders.LastIndexOf(".")) + i.ToString() + "." + Consts.LOG_TYPE_EXTENSION;
                        sLogTypes = Directory.GetFiles(CIniSettingsManager.LogTypesFolder, sNewFileName);
                        i++;
                    }
                }
                sNewFileName = CIniSettingsManager.LogTypesFolder + "\\" +  sNewFileName;
                File.Copy(LogTypeIniFileName, sNewFileName);
                CLogType oNewType = new CLogType(sNewFileName);
                AddLogType(oNewType);
            }
        }

    }
    
}
