namespace IniFile
{
    public class CIniSection
    {
        public string SectionName { get; private set; }
        public CIniFile IniFile { get; private set; }
        public CValueContainer Values { get; private set; }
        public CArrayValueContainer ArrayValues { get; private set; }

        public CIniSection(CIniFile vIniFile, string vSectionName)
        {
            IniFile = vIniFile;
            SectionName = vSectionName;
            Values = new CValueContainer(this);
            ArrayValues = new CArrayValueContainer(this);
        }
    }
}
