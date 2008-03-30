using System;
using System.Collections.Generic;
using System.Text;
using Universal_Log_Viewer.Common.IniFile;

namespace Universal_Log_Viewer.Types.Structures
{
    public class CStringType : CBaseType
    {
        public const string INI_TYPE_NAME = "String";
        const string INI_KEY_CONDITION = "Condition";
        const string INI_KEY_USE_SEPARATOR = "UseSeparator";
        const string INI_KEY_SEPARATOR = "Separator";
        const string INI_KEY_CHILD_TYPES = "ChildValueTypes";
        const string INI_KEY_TITLE_VALUE_INDEX = "TitleValueIndex";
        const string INI_KEY_TITLE_VALUE_TYPE = "TitleValueType";
        public CCondition Condition;
        public bool UseSeparator { get; private set; }
        public char[] Separator { get; private set; }
        public int TitleValueIndex { get; private set; }
        public string TitleValueType { get; private set; }
        public List<CValueType> ChildTypes;
        public CStringType(CLogType LogType, CLogIniSection Section)
            : base(LogType, Section)
        {
        }
        public CStringType()
            : base()
        {

        }

        public override void ReInit(CLogType LogType, CLogIniSection Section)
        {
            base.ReInit(LogType, Section);
            Condition = ParentLogType.Conditions[IniSection.Values[INI_KEY_CONDITION]];
            UseSeparator = (IniSection.Values[INI_KEY_USE_SEPARATOR] == "1");
            if (IniSection.Values[INI_KEY_SEPARATOR].Length > 0)
            {
                string[] SeparatorArray = IniSection.ArrayValues[INI_KEY_SEPARATOR];
                List<char> SeparatorList = new List<char>();
                foreach (string SeparatorString in SeparatorArray)
                    SeparatorList.Add(SeparatorString[0]);
                Separator = SeparatorList.ToArray();
                int Temporary;
                if (!(int.TryParse(IniSection.Values[INI_KEY_TITLE_VALUE_INDEX], out Temporary)))
                    Temporary = 0;
                TitleValueIndex = Temporary;
                TitleValueType = IniSection.Values[INI_KEY_TITLE_VALUE_TYPE];
            }
            else
                UseSeparator = false;

            string[] arChildren = IniSection.ArrayValues[INI_KEY_CHILD_TYPES];
            ChildTypes = new List<CValueType>();
            foreach (string Child in arChildren)
                ChildTypes.Add(ParentLogType.ValueTypes[Child]);
        }
    }
}
