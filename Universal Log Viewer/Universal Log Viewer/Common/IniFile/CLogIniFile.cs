using System;
using System.Collections.Generic;
using System.Text;
using IniFile;


namespace Universal_Log_Viewer.Common.IniFile
{
    public class CLogIniFile : CIniFile
    {
        public CSectionContainer<CLogIniSection> Sections;
        public CLogIniFile(string INIPath)
           : base(INIPath)
        {
            Sections = new CSectionContainer<CLogIniSection>();
            foreach (string SectionName in SectionNames)
                Sections.AddSection(new CLogIniSection(this, SectionName));
        }
    }
    public class CLogIniSection : CIniSection
    {
        const string TYPE_KEY_NAME = "Type";
        public string Name { get; private set; }
        public CLogIniSection(CLogIniFile vIniFile, string vSectionName)
            :base(vIniFile, vSectionName)
        {
            Name = Values[Consts.KEY_NAME];
        }
        public string SectionType { get { return Values[TYPE_KEY_NAME]; } }

    }
}
