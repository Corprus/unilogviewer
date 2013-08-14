using IniFiles;
using Consts = UniversalLogViewer.Common.Consts;

namespace UniversalLogViewer.LogIniFiles
{
    public class LogIniFile : IniFile
    {
        public SectionCollection<LogIniSection> Sections { get; private set; }
        public LogIniFile(string iniPath)
           : base(iniPath)
        {
            Sections = new SectionCollection<LogIniSection>();
            try
            {
                foreach (string sectionName in SectionNames)
                    Sections.AddSection(new LogIniSection(this, sectionName));
            }
            catch (IniFiles.Exceptions.IniFileSectionsReadException e)
            {
                throw new Common.Exceptions.LogIniSectionReadException("", e);                    
            }

        }
    }
    public class LogIniSection : IniSection
    {
        const string TypeKeyName = "Type";
        public string Name { get; private set; }
        public LogIniSection(IniFile logTypeFile, string sectionName)
            :base(logTypeFile, sectionName)
        {
            Name = Values[Consts.KeyName];
        }
        public string SectionType { get { return Values[TypeKeyName]; } }

    }
}
