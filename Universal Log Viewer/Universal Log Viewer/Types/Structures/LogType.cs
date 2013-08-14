using System.Threading.Tasks;
using UniversalLogViewer.Common.Types.Managers;
using UniversalLogViewer.LogIniFiles;
using UniversalLogViewer.Types.Managers;

namespace UniversalLogViewer.Types.Structures
{
    public class LogType
    {
        const string KeyLogSection = "Log File";
        const string KeyRootType = "RootBlock";
        const string KeyLogName = "Name";
        const string KeyAuthor = "Author";
        const string KeyVersion = "Version";
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
                const string newLine = @"\line ";
                const string tabSymbol = @"\tab ";
                var result =
                    string.Format(
                        @"{{\rtf1\ansi \b {0} \b0{1}Author:{2}{3}{1}Version:{2}{4}{1}File Path:{2}{5}{1}Last Modified:{2}{6}{1}}}",
                        LogName, newLine, tabSymbol, Author, Version, LogTypeFile.FileName.Replace("\\", "\\\\"),
                        System.IO.File.GetLastWriteTime(LogTypeFile.FileName));
                return result;
            }
        }

        public void ReInit(string fileName)
        {
            LogTypeFile = new LogIniFile(fileName);
            Conditions = new LogTypeCollection<ConditionType>(this);
            ValueTypes = new LogTypeCollection<ValuesType>(this);
            StringTypes = new LogTypeCollection<StringType>(this);
            BlockTypes = new LogTypeCollection<BlockType>(this);
            Styles = new LogTypeCollection<StyleType>(this);
            Parallel.ForEach(LogTypeFile.Sections, section =>
                {
                    switch (section.SectionType)
                    {
                        case ConditionType.IniTypeName:
                            Conditions.AddType(section.SectionName);
                            break;
                        case ValuesType.IniTypeName:
                            ValueTypes.AddType(section.SectionName);
                            break;
                        case StringType.IniTypeName:
                            StringTypes.AddType(section.SectionName);
                            break;
                        case BlockType.IniTypeName:
                            BlockTypes.AddType(section.SectionName);
                            break;
                        case StyleType.IniTypeName:
                            Styles.AddType(section.SectionName);
                            break;
                    }
                });

            RootBlockType = BlockTypes[LogTypeFile.Sections[KeyLogSection].Values[KeyRootType]];
            LogName = LogTypeFile.Sections[KeyLogSection].Values[KeyLogName];
            Author = LogTypeFile.Sections[KeyLogSection].Values[KeyAuthor];
            Version = LogTypeFile.Sections[KeyLogSection].Values[KeyVersion];

        }
        public LogType(string fileName)
        {
            try
            {
                ReInit(fileName);
            }
            catch (Common.Exceptions.UniLogViewerException e)
            {
                throw new Common.Exceptions.LogTypeLoadException(
                    string.Format("Cannot load log type from file {0}\n See description in the types load log file", fileName), e);
            }

        }
        public override string ToString() 
        {
            return LogName;
        }
        public void ExternalOpen()
        {
            if (!IniSettingsManager.UseExternalOpen) return;

            var process = new System.Diagnostics.Process
                {
                    StartInfo =
                        {
                            FileName = IniSettingsManager.OpenLogTypeCommand,
                            WorkingDirectory = IniSettingsManager.LogTypesFolder,
                            Arguments = LogTypeFile.FileName
                        }
                };
            process.Start();
        }
    }
}
