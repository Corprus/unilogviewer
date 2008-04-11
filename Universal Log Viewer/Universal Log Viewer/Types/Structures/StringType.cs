using System;
using System.Collections.Generic;
using System.Text;
using UniversalLogViewer.IniFiles;

namespace UniversalLogViewer.Types.Structures
{
    public class StringType : BaseType
    {
        public const string INI_TYPE_NAME = "String";
        const string INI_KEY_CONDITION = "Condition";
        const string INI_KEY_USE_SEPARATOR = "UseSeparator";
        const string INI_KEY_SEPARATOR = "Separator";
        const string INI_KEY_CHILD_TYPES = "ChildValueTypes";
        const string INI_KEY_TITLE_VALUE_INDEX = "TitleValueIndex";
        const string INI_KEY_TITLE_VALUE_TYPE = "TitleValueType";
        public ConditionType Condition { get; private set; }
        public bool UseSeparator { get; private set; }
        public char[] Separator { get; private set; }
        public int TitleValueIndex { get; private set; }
        public string TitleValueType { get; private set; }
        public List<ValuesType> ChildTypes { get; private set; }
        public StringType(LogType LogType, LogIniSection Section)
            : base(LogType, Section)
        {
        }
        public StringType()
            : base()
        {

        }

        public override void ReInit(LogType LogType, LogIniSection Section)
        {
            base.ReInit(LogType, Section);
            Condition = ParentLogType.Conditions[Section.Values[INI_KEY_CONDITION]];
            UseSeparator = (Section.Values[INI_KEY_USE_SEPARATOR] == "1");
            if (Section.Values[INI_KEY_SEPARATOR].Length > 0)
            {
                string[] SeparatorArray = Section.ArrayValues[INI_KEY_SEPARATOR, false];
                List<char> SeparatorList = new List<char>();
                foreach (string SeparatorString in SeparatorArray)
                    SeparatorList.Add(SeparatorString[0]);
                Separator = SeparatorList.ToArray();
                int Temporary;
                if (!(int.TryParse(Section.Values[INI_KEY_TITLE_VALUE_INDEX], out Temporary)))
                    Temporary = 0;
                TitleValueIndex = Temporary;
                TitleValueType = Section.Values[INI_KEY_TITLE_VALUE_TYPE];
            }
            else
                UseSeparator = false;

            string[] arChildren = Section.ArrayValues[INI_KEY_CHILD_TYPES];
            ChildTypes = new List<ValuesType>();
            foreach (string Child in arChildren)
                ChildTypes.Add(ParentLogType.ValueTypes[Child]);
        }
    }
}
