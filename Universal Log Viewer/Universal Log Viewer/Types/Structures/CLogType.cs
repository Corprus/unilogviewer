using System;
using System.Collections.Generic;
using System.Text;
using Universal_Log_Viewer.Common;
using Universal_Log_Viewer.Common.IniFile;
using Universal_Log_Viewer.Types.Managers;

namespace Universal_Log_Viewer.Types.Structures
{
    public class CLogType
    {
        const string KEY_LOG_SECTION = "Log File";
        const string KEY_ROOT_TYPE = "RootBlock";
        const string KEY_LOG_NAME = "Name";
        const string KEY_AUTHOR = "Author";
        const string KEY_VERSION = "Version";
        public string LogName { get; private set; }
        public string Author { get; private set; }
        public string Version { get; private set; }        
        public LogTypeContainer<CCondition> Conditions;
        public LogTypeContainer<CValueType> ValueTypes;
        public LogTypeContainer<CStringType> StringTypes;
        public LogTypeContainer<CBlockType> BlockTypes;
        public LogTypeContainer<CStyle> Styles;

        public CBlockType RootBlockType { get; private set; }
        public CLogIniFile LogIniFile { get; private set; }
        public string GetRTFDescription()
        {
            const string NEW_LINE = @"\line ";
            const string TAB_SYMBOL = @"\tab ";
            string Result;
            Result = @"{\rtf1\ansi \b " + LogName + @" \b0" + NEW_LINE;
            Result += "Author:" + TAB_SYMBOL + Author + NEW_LINE;
            Result += "Version:" + TAB_SYMBOL + Version + NEW_LINE;
            Result += "Last Modified:" + TAB_SYMBOL + System.IO.File.GetLastWriteTime(LogIniFile.FileName) + NEW_LINE;
            Result += @"}";
            return Result;
        }
        public string[] GetDescription()
        {
            List<string> Result = new List<string>();
            Result.Add(LogName);
            return Result.ToArray();
        }
        public void ReInit(string IniFileName)
        {
            LogIniFile = new CLogIniFile(IniFileName);
            Conditions = new LogTypeContainer<CCondition>(this);
            ValueTypes = new LogTypeContainer<CValueType>(this);
            StringTypes = new LogTypeContainer<CStringType>(this);
            BlockTypes = new LogTypeContainer<CBlockType>(this);
            Styles = new LogTypeContainer<CStyle>(this);
            foreach (CLogIniSection Section in LogIniFile.Sections)
            {
                switch (Section.SectionType)
                {
                    case CCondition.INI_TYPE_NAME: Conditions.AddType(Section.SectionName); break;
                    case CValueType.INI_TYPE_NAME: ValueTypes.AddType(Section.SectionName); break;
                    case CStringType.INI_TYPE_NAME: StringTypes.AddType(Section.SectionName); break;
                    case CBlockType.INI_TYPE_NAME: BlockTypes.AddType(Section.SectionName); break;
                    case CStyle.INI_TYPE_NAME: Styles.AddType(Section.SectionName); break;
                }
            }
            RootBlockType = BlockTypes[LogIniFile.Sections[KEY_LOG_SECTION].Values[KEY_ROOT_TYPE]];
            LogName = LogIniFile.Sections[KEY_LOG_SECTION].Values[KEY_LOG_NAME];
            Author = LogIniFile.Sections[KEY_LOG_SECTION].Values[KEY_AUTHOR];
            Version = LogIniFile.Sections[KEY_LOG_SECTION].Values[KEY_VERSION];

        }
        public CLogType(string IniFileName)
        {
            ReInit(IniFileName);
        }
        public override string ToString() 
        {
            return LogName;
        }
        public void ExternalOpen()
        {
            if (CIniSettingsManager.UseExternalOpen)
            {
                System.Diagnostics.Process batch = new System.Diagnostics.Process();
                batch.StartInfo.FileName = CIniSettingsManager.OpenLogTypeCommand;
                batch.StartInfo.WorkingDirectory = CIniSettingsManager.LogTypesFolder;
                batch.StartInfo.Arguments = this.LogIniFile.FileName;
                batch.Start();
                batch.WaitForExit();
            }
                

        }


        

    }
}
