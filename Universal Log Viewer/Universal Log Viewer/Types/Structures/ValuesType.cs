using System;
using System.Collections.Generic;
using System.Text;
using UniversalLogViewer.LogIniFiles;

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
        public string ValueType { get; private set; }

        void FInit(string ValueType, bool vIncludeConditions, ConditionType Condition)
        {
            this.ValueType = ValueType;
            IncludeConditions = vIncludeConditions;
            this.Condition = Condition;
        }
        public ValuesType(LogType LogType, LogIniSection Section)
            : base(LogType, Section)
        {
        }

        public override void ReInit(LogType LogType, LogIniSection Section)
        {
            base.ReInit(LogType, Section);
            FInit(Section.Values[KEY_VALUE_TYPE],
                (Section.Values[KEY_INCLUDE_CONDITIONS] == "1"),
                (ParentLogType.Conditions[ParentLogType.LogTypeFile.Sections[Section.Values[KEY_CONDITION]].SectionName]));
        }
        public ValuesType()
            : base()
        {
        }
    }
}
