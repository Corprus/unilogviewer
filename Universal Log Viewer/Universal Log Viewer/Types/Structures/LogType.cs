using System;
using System.Collections.Generic;
using System.Text;
using UniversalLogViewer.Common;
using UniversalLogViewer.Common.Types.Managers;
using UniversalLogViewer.Types.Managers;
using UniversalLogViewer.LogIniFiles;

namespace UniversalLogViewer.Types.Structures
{
    public class LogType
    {
        const string KEY_LOG_SECTION = "Log File";
        const string KEY_ROOT_TYPE = "RootBlock";
        const string KEY_LOG_NAME = "Name";
        const string KEY_AUTHOR = "Author";
        const string KEY_VERSION = "Version";
        public string LogName { get; private set; }
        public string Author { get; private set; }
        public string Version { get; private set; }
        public LogTypeCollection<ConditionType> Conditions { get; private set; }
        public LogTypeCollection<ValuesType> ValueTypes { get; private set; }
        public LogTypeCollection<StringType> StringTypes { get; private set; }
        public LogTypeCollection<BlockType> BlockTypes { get; private set; }
        public LogTypeCollection<StyleType> Styles { get; private set; }

        public BlockType RootBlockType { get; private set; }
        public LogIniFile LogTypeFile { get; private set; }
        public string RTFDescription
        {
            get
            {
                const string NEW_LINE = @"\line ";
                const string TAB_SYMBOL = @"\tab ";
                string Result;
                Result = @"{\rtf1\ansi \b " + LogName + @" \b0" + NEW_LINE;
                Result += "Author:" + TAB_SYMBOL + Author + NEW_LINE;
                Result += "Version:" + TAB_SYMBOL + Version + NEW_LINE;
                Result += "File Path:" + TAB_SYMBOL + LogTypeFile.FileName.Replace("\\", "\\\\") + NEW_LINE;
                Result += "Last Modified:" + TAB_SYMBOL + System.IO.File.GetLastWriteTime(LogTypeFile.FileName) + NEW_LINE;
                Result += @"}";
                return Result;
            }
        }
        public string[] GetDescription()
        {
            List<string> Result = new List<string>();
            Result.Add(LogName);
            return Result.ToArray();
        }
        public void ReInit(string FileName)
        {
            LogTypeFile = new LogIniFile(FileName);
            Conditions = new LogTypeCollection<ConditionType>(this);
            ValueTypes = new LogTypeCollection<ValuesType>(this);
            StringTypes = new LogTypeCollection<StringType>(this);
            BlockTypes = new LogTypeCollection<BlockType>(this);
            Styles = new LogTypeCollection<StyleType>(this);
            foreach (LogIniSection Section in LogTypeFile.Sections)
            {
                switch (Section.SectionType)
                {
                    case ConditionType.INI_TYPE_NAME: Conditions.AddType(Section.SectionName); break;
                    case ValuesType.INI_TYPE_NAME: ValueTypes.AddType(Section.SectionName); break;
                    case StringType.INI_TYPE_NAME: StringTypes.AddType(Section.SectionName); break;
                    case BlockType.INI_TYPE_NAME: BlockTypes.AddType(Section.SectionName); break;
                    case StyleType.INI_TYPE_NAME: Styles.AddType(Section.SectionName); break;
                }
            }
            RootBlockType = BlockTypes[LogTypeFile.Sections[KEY_LOG_SECTION].Values[KEY_ROOT_TYPE]];
            LogName = LogTypeFile.Sections[KEY_LOG_SECTION].Values[KEY_LOG_NAME];
            Author = LogTypeFile.Sections[KEY_LOG_SECTION].Values[KEY_AUTHOR];
            Version = LogTypeFile.Sections[KEY_LOG_SECTION].Values[KEY_VERSION];

        }
        public LogType(string FileName)
        {
            try
            {
                ReInit(FileName);
            }
            catch (Common.Exceptions.UniLogViewerException e)
            {
                string ShortFileName = FileName.Substring(FileName.LastIndexOf('\\'));
                throw new Common.Exceptions.LogTypeLoadException("Cannot load log type from file " + FileName + "\n See description in the types load log file", e);
            }

        }
        public override string ToString() 
        {
            return LogName;
        }
        public void ExternalOpen()
        {
            if (IniSettingsManager.UseExternalOpen)
            {
                System.Diagnostics.Process batch = new System.Diagnostics.Process();
                batch.StartInfo.FileName = IniSettingsManager.OpenLogTypeCommand;
                batch.StartInfo.WorkingDirectory = IniSettingsManager.LogTypesFolder;
                batch.StartInfo.Arguments = this.LogTypeFile.FileName;
                batch.Start();
            }
                

        }


        

    }
}
