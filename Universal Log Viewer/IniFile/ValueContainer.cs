
namespace IniFiles
{
    public class ValueContainer
    {
        IniSection Section;
        bool AutoCreateValues;
        public ValueContainer(IniSection Section)
        {
            this.Section = Section;
        }
        public ValueContainer(IniSection Section, bool AutoCreateValues)
        {
            this.Section = Section;
            this.AutoCreateValues = AutoCreateValues;
        }
        public virtual string this[string ValueName]
        {
            get
            {
                return this[false, ValueName];
            }
        }

        public virtual string this[bool Required, string ValueName]
        {
            get
            {
                string Result = Section.IniFile.ReadValue(Section.SectionName, ValueName);
                if ((Result.Length == 0))
                {
                    if (AutoCreateValues)
                        Section.IniFile.WriteValue(Section.SectionName, ValueName, Result);
                    if (Required)
                        throw new Exceptions.IniFileRequiredFieldReadException("Cannot read required field \"" + ValueName + "\" from Section \""
                            + Section.SectionName +"\" in " + Section.IniFile.FileName);
                }
                return Result;
            }
        }
    }
    public class ArrayValueContainer : ValueContainer
    {
        public ArrayValueContainer(IniSection Section)
            : base(Section)
        {
        }
        public ArrayValueContainer(IniSection Section, bool AutoCreateValues)
            : base(Section, AutoCreateValues)
        {
        }

        new public string[] this[string ValueName]
        {
            get
            {
                string PlainResult = base[ValueName];
                if (PlainResult.Length == 0)
                    return new string[0];
                else
                {
                    string[] Result = PlainResult.Split(IniFiles.Common.Consts.ARRAY_SEPARATOR);
                    for (int i=0; i< Result.Length; i++)
                        Result[i] = Result[i].Trim();
                    return Result;

                }
            }
        }
        public string[] this[string ValueName, bool Trim]
        {
            get
            {
                string PlainResult = base[ValueName];
                if (PlainResult.Length == 0)
                    return new string[0];
                else
                {
                    string[] Result = PlainResult.Split(Common.Consts.ARRAY_SEPARATOR);
                    for (int i = 0; i < Result.Length; i++)
                        if (Trim)//Случай сепаратора
                            Result[i] = Result[i].Trim();
                    return Result;

                }
            }
        }
    }
}
