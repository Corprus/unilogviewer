using System;
using System.Collections.Generic;
using System.Text;
using IniFiles;
using UniversalLogViewer.Common;


namespace UniversalLogViewer.IniFiles
{
    public class LogIniFile : IniFile
    {
        public SectionCollection<LogIniSection> Sections { get; private set; }
        public LogIniFile(string INIPath)
           : base(INIPath)
        {
            Sections = new SectionCollection<LogIniSection>();
            foreach (string SectionName in SectionNames)
                Sections.AddSection(new LogIniSection(this, SectionName));
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
