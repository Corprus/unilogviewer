using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UniversalLogViewer.Properties;
using UniversalLogViewer.Types.Structures;
using UniversalLogViewer.Common;
using UniversalLogViewer.Common.Types.Managers;


namespace UniversalLogViewer.Types.Managers
{
    public class LogTypeManager
    {
        public static void ReInit()
        {
            _instance = new LogTypeManager();
        }

        static LogTypeManager _instance;
        public void UpdateList(IList items)
        {
            items.Clear();
            foreach (var logType in TypesList)
                items.Add(logType);
        }

        LogTypeManager()
        {
            TypesList = new List<LogType>();
            try
            {
                _hadInconsistencies = false;
                if (!(Directory.Exists(IniSettingsManager.LogTypesFolder)))
                   Directory.CreateDirectory(IniSettingsManager.LogTypesFolder);
                string[] sLogTypes = Directory.GetFiles(IniSettingsManager.LogTypesFolder, string.Format("*.{0}", Consts.LogTypeExtension));
                try
                {
                    TypesList.AddRange(sLogTypes.Select(type => new LogType(type)));
                }
                catch (Common.Exceptions.UniLogViewerException)
                {
                    _hadInconsistencies = true;
                    Common.Exceptions.ExceptionLogWriter.Instance.WriteLog(LogWriting.TypeLogMessage.Error,
                                                                           string.Format("Cannot load log types"));
                }

                if ((!_hadInconsistencies) || (!IniSettingsManager.UseSeparateInconsistenciesLog) ||
                    (!IniSettingsManager.OpenInconsistenciesLogIfGenerated)) return;
                var batch = new System.Diagnostics.Process
                    {
                        StartInfo =
                            {
                                FileName = "notepad",
                                WorkingDirectory = Application.StartupPath,
                                Arguments = Consts.InconsistenciesLogFilename
                            }
                    };
                batch.Start();
            }
            catch (IOException)
            {
                throw new Common.Exceptions.UniLogViewerException(
                    string.Format("Cannot get access to Lot Types folder ({0}) or get it's file list", IniSettingsManager.LogTypesFolder));
            }

        }

        public static LogTypeManager Instance
        {
            get { return _instance ?? (_instance = new LogTypeManager()); }
        }
        public List<LogType> TypesList { get; private set; }

        private void AddLogType(LogType logType)
        {
            if (TypesList == null)
                TypesList = new List<LogType>();
            TypesList.Add(logType);
        }
        private bool _hadInconsistencies;
        public void AddLogType(string logTypeFileName)
        {
            if (TypesList == null)
                TypesList = new List<LogType>();
            bool bHasSameName = (TypesList.Any(type => type.LogName == logTypeFileName));

            bool addTypeAccepted = true;
            string logFileIniFileNameWithoutFolders = logTypeFileName.Substring(logTypeFileName.LastIndexOf("\\", StringComparison.Ordinal) + 1, logTypeFileName.Length - logTypeFileName.LastIndexOf("\\", StringComparison.Ordinal) - 1);
            bHasSameName = (bHasSameName || (File.Exists(string.Format("{0}\\{1}", IniSettingsManager.LogTypesFolder, logFileIniFileNameWithoutFolders))));

            if (bHasSameName)
               addTypeAccepted = (MessageBox.Show(Consts.AskAddLogTypeWithSameName, Consts.HeaderSameLogTypePresent, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, Consts.DefaultMessageBoxOptions) == DialogResult.Yes);

            if (!addTypeAccepted) return;

            var newFileName = logFileIniFileNameWithoutFolders;

            if (bHasSameName)
            {
                string[] sLogTypes = Directory.GetFiles(IniSettingsManager.LogTypesFolder, newFileName);
                int i = 0;
                while (sLogTypes.Length > 0)
                {
                    newFileName = logFileIniFileNameWithoutFolders.Substring(0, logFileIniFileNameWithoutFolders.LastIndexOf(".", StringComparison.Ordinal)) + i.ToString(System.Globalization.CultureInfo.InvariantCulture) + "." + Consts.LogTypeExtension;
                    sLogTypes = Directory.GetFiles(IniSettingsManager.LogTypesFolder, newFileName);
                    i++;
                }
            }
            newFileName = string.Format("{0}\\{1}", IniSettingsManager.LogTypesFolder, newFileName);
            File.Copy(logTypeFileName, newFileName);
            try
            {
                var newType = new LogType(newFileName);
                AddLogType(newType);
            }
            catch (Common.Exceptions.LogTypeLoadException e)
            {
                _hadInconsistencies = true;
                MessageBox.Show(e.Message, Resources.CannotLoadLogTypeMessage, MessageBoxButtons.OK,
                                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, Consts.DefaultMessageBoxOptions);
            }
        }

    }
    
}
