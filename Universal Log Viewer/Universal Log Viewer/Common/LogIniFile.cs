using System;
using System.Collections.Generic;
using System.Text;
using IniFiles;
using UniversalLogViewer.Common;


namespace UniversalLogViewer.LogIniFiles
{
    public class LogIniFile : IniFile
    {
        public SectionCollection<LogIniSection> Sections { get; private set; }
        public LogIniFile(string INIPath)
           : base(INIPath)
        {
            Sections = new SectionCollection<LogIniSection>();
            try
            {
                foreach (string SectionName in SectionNames)
                    Sections.AddSection(new LogIniSection(this, SectionName));
            }
            catch (IniFiles.Exceptions.IniFileSectionsReadException e)
            {
                throw new Common.Exceptions.LogIniSectionReadException("", e);                    
            }

        }
    }
    public class LogIniSection : IniSection
    {
        const string TYPE_KEY_NAME = "Type";
        public string Name { get; private set; }
        public LogIniSection(LogIniFile LogTypeFile, string SectionName)
            :base(LogTypeFile, SectionName)
        {
            Name = Values[Consts.KEY_NAME];
        }
        public string SectionType { get { return Values[TYPE_KEY_NAME]; } }

    }
}
