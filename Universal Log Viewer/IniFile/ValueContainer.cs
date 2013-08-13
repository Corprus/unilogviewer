
using System.Collections.Generic;
using System.Linq;

namespace IniFiles
{
    public class ValueContainer
    {
        protected readonly IniSection Section;
        readonly bool _autoCreateValues;
        public ValueContainer(IniSection section)
        {
            Section = section;
        }

        public ValueContainer(IniSection section, bool autoCreateValues)
        {
            Section = section;
            _autoCreateValues = autoCreateValues;
        }

        public virtual string this[string valueName]
        {
            get
            {
                return this[false, valueName];
            }
        }
        public virtual string this[bool required, string valueName]
        {
            get
            {
                return this[required, valueName, ""];
            }
        }
        public virtual string this[bool required, string valueName, string Default]
        {
            get
            {
                string result = Section.IniFile.ReadValue(Section.SectionName, valueName);
                if ((result.Length == 0))
                    result = Default;
                if ((result.Length == 0))
                {
                    if (_autoCreateValues)
                        Section.IniFile.WriteValue(Section.SectionName, valueName, result);
                    if (required)
                        throw new Exceptions.IniFileRequiredFieldReadException(
                            string.Format("Cannot read required field \"{0}\" from Section \"{1}\" in {2}", valueName,
                                          Section.SectionName, Section.IniFile.FileName));
                }
                return result;
            }
        }
    }
    public class ArrayValueContainer : ValueContainer
    {
        public ArrayValueContainer(IniSection section)
            : base(section)
        {
        }
        public ArrayValueContainer(IniSection section, bool autoCreateValues)
            : base(section, autoCreateValues)
        {
        }

        new public string[] this[string valueName]
        {
            get
            {
                var plainResult = base[valueName];
                if (plainResult.Length == 0)
                    return new string[0];
                return plainResult.Split(Consts.ArraySeparator).AsParallel().AsOrdered().Select(s => s.Trim()).ToArray();
            }
        }
        public IEnumerable<string> this[string valueName, bool trim]
        {
            get
            {
                string plainResult = base[valueName];
                if (plainResult.Length == 0)
                    return new string[0];
                var result = plainResult.Split(Consts.ArraySeparator);
                return trim ?  result.AsParallel().AsOrdered().Select(s => s.Trim()).ToArray() : result;
            }
        }
    }
    public class BoolValueContainer : ValueContainer
    {
        public BoolValueContainer(IniSection section)
            : base(section)
        {
        }
        public BoolValueContainer(IniSection section, bool autoCreateValues)
            : base(section, autoCreateValues)
        {
        }

        public bool this[string valueName, bool Default]
        {
            get
            {
                var result = Section.IniFile.ReadBoolValue(Section.SectionName, valueName, Default);
                return result;
            }
        }
        new public bool this[string valueName]
        {
            get
            {

                return this[valueName, false];
            }
        }

    }

}
