
namespace IniFile
{
    public class CValueContainer
    {
        CIniSection IniSection;
        bool AutoCreateValues;
        public CValueContainer(CIniSection IniSection)
        {
            this.IniSection = IniSection;
            this.AutoCreateValues = false;
        }
        public CValueContainer(CIniSection IniSection, bool AutoCreateValues)
        {
            this.IniSection = IniSection;
            this.AutoCreateValues = AutoCreateValues;
        }

        public string this[string ValueName]
        {
            get
            {
                string Result = IniSection.IniFile.IniReadValue(IniSection.SectionName, ValueName);
                if ((Result == "") && (AutoCreateValues))
                    IniSection.IniFile.IniWriteValue(IniSection.SectionName, ValueName, Result);
                return Result;
            }
        }
    }
    public class CArrayValueContainer : CValueContainer
    {
        public CArrayValueContainer(CIniSection IniSection)
            : base(IniSection)
        {
        }
        public CArrayValueContainer(CIniSection IniSection, bool AutoCreateValues)
            : base(IniSection, AutoCreateValues)
        {
        }

        new public string[] this[string ValueName]
        {
            get
            {
                string PlainResult = base[ValueName];
                if (PlainResult == "")
                    return new string[0];
                else
                    return PlainResult.Split(Consts.ARRAY_SEPARATOR);
            }
        }
    }
}
