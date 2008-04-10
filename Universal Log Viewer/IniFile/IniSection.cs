namespace IniFiles
{
    public class IniSection
    {
        public string SectionName { get; private set; }
        public IniFile IniFile { get; private set; }
        public ValueContainer Values { get; private set; }
        public ArrayValueContainer ArrayValues { get; private set; }

        public IniSection(IniFile IniFile, string SectionName)
        {
            this.IniFile = IniFile;
            this.SectionName = SectionName;
            Values = new ValueContainer(this);
            ArrayValues = new ArrayValueContainer(this);
        }
    }
}
