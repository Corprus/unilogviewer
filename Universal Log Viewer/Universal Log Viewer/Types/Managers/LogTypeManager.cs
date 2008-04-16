using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniversalLogViewer.Types.Structures;
using UniversalLogViewer.Common;


namespace UniversalLogViewer.Types.Managers
{
    public class LogTypeManager
    {
        public static void ReInit()
        {
            _oInstance = new LogTypeManager();
        }

        static LogTypeManager _oInstance;
        public void UpdateList(IList Items)
        {
            Items.Clear();
            foreach (LogType LogType in TypesList)
                Items.Add(LogType);
        }

        LogTypeManager()
        {
            TypesList = new List<LogType>();
            try
            {
                if (!(Directory.Exists(IniSettingsManager.LogTypesFolder)))
                   Directory.CreateDirectory(IniSettingsManager.LogTypesFolder);
                string[] sLogTypes = Directory.GetFiles(IniSettingsManager.LogTypesFolder, "*." + Consts.LOG_TYPE_EXTENSION);
                foreach (string sLogType in sLogTypes)
                {
                    try
                    {
                        var NewLogType = new LogType(sLogType);
                        TypesList.Add(NewLogType);
                    }
                    catch (Common.Exceptions.UniLogViewerException)
                    {
                        Common.Exceptions.ExceptionLogWriter.Instance.WriteLog(LogWriting.TypeLogMessage.LMT_ERROR, "Cannot load log type " + sLogType);
                    }
                }
            }
            catch (System.IO.IOException)
            {
                throw new Common.Exceptions.UniLogViewerException("Cannot get access to Lot Types folder (" + IniSettingsManager.LogTypesFolder + ") or get it's file list");
            }

        }
        public static LogTypeManager oInstance
        {
            get
            {
                if (_oInstance == null)
                    _oInstance = new LogTypeManager();
                return _oInstance;

            }
        }
        public List<LogType> TypesList { get; private set; }
        public void AddLogType(LogType LogType)
        {
            if (TypesList == null)
                TypesList = new List<LogType>();
            TypesList.Add(LogType);
        }
        public void AddLogType(string LogTypeFileName)
        {
            if (TypesList == null)
                TypesList = new List<LogType>();
            bool bHasSameName = false;
            
            foreach (LogType oType in TypesList)
            {
                if (oType.LogName == LogTypeFileName)
                    bHasSameName = true;
            }
            bool bAddType = true;
            string LogFileIniFileNameWithoutFolders = LogTypeFileName.Substring(LogTypeFileName.LastIndexOf("\\", StringComparison.Ordinal) + 1, LogTypeFileName.Length - LogTypeFileName.LastIndexOf("\\", StringComparison.Ordinal) - 1);
            bHasSameName = (bHasSameName || (System.IO.File.Exists(IniSettingsManager.LogTypesFolder + "\\" +  LogFileIniFileNameWithoutFolders)));

            if (bHasSameName)
               bAddType = (MessageBox.Show(Consts.ASK_ADD_LOG_TYPE_WITH_SAME_NAME, Consts.HEADER_SAME_LOG_TYPE_PRESENT, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, Consts.DEFAULT_MESSAGE_BOX_OPTIONS) == DialogResult.Yes);

            if (bAddType)
            {                
                string sNewFileName = LogFileIniFileNameWithoutFolders;

                if (bHasSameName)
                {
                    string[] sLogTypes = Directory.GetFiles(IniSettingsManager.LogTypesFolder, sNewFileName);
                    int i = 0;
                    while (sLogTypes.Length > 0)
                    {
                        sNewFileName = LogFileIniFileNameWithoutFolders.Substring(0, LogFileIniFileNameWithoutFolders.LastIndexOf(".", StringComparison.Ordinal)) + i.ToString(System.Globalization.CultureInfo.InvariantCulture) + "." + Consts.LOG_TYPE_EXTENSION;
                        sLogTypes = Directory.GetFiles(IniSettingsManager.LogTypesFolder, sNewFileName);
                        i++;
                    }
                }
                sNewFileName = IniSettingsManager.LogTypesFolder + "\\" +  sNewFileName;
                File.Copy(LogTypeFileName, sNewFileName);
                try
                {
                    LogType oNewType = new LogType(sNewFileName);
                    AddLogType(oNewType);
                }
                catch (Common.Exceptions.LogTypeLoadException e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message, "Cannot load load type", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, Common.Consts.DEFAULT_MESSAGE_BOX_OPTIONS);
                }


            }
        }

    }
    
}
