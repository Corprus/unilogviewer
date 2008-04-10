using System;
using System.Collections.Generic;
using System.Text;
using UniversalLogViewer.Common.IniFiles;

namespace UniversalLogViewer.Types.Structures
{
    public class ValuesType : BaseType
    {
        public const string INI_TYPE_NAME = "Value";
        const string KEY_NAME = "Name";
        const string KEY_INCLUDE_CONDITIONS = "IncludeConditions";
        const string KEY_VALUE_TYPE = "ValueType";
        const string KEY_CONDITION = "Condition";
        public ConditionType Condition { get; private set; }
        public bool IncludeConditions { get; private set; }
        public string Type { get; private set; }

        void FInit(string ValueType, bool vIncludeConditions, ConditionType Condition)
        {
            Type = ValueType;
            IncludeConditions = vIncludeConditions;
            this.Condition = Condition;
        }
        public ValuesType(LogType LogType, LogIniSection Section)
            : base(LogType, Section)
        {
        }

        public override void ReInit(LogType vLogType, LogIniSection IniSection)
        {
            base.ReInit(vLogType, IniSection);
            FInit(IniSection.Values[KEY_VALUE_TYPE],
                (IniSection.Values[KEY_INCLUDE_CONDITIONS] == "1"),
                (ParentLogType.Conditions[ParentLogType.LogTypeFile.Sections[IniSection.Values[KEY_CONDITION]].SectionName]));
        }
        public ValuesType()
            : base()
        {
        }
    }
}
