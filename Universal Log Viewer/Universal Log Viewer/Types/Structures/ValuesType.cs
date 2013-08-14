using UniversalLogViewer.LogIniFiles;

namespace UniversalLogViewer.Types.Structures
{
    public class ValuesType : BaseType
    {
        public const string IniTypeName = "Value";
        const string KeyIncludeConditions = "IncludeConditions";
        const string KeyValueType = "ValueType";
        const string KeyCondition = "Condition";
        public ConditionType Condition { get; private set; }
        public bool IncludeConditions { get; private set; }
        public string ValueType { get; private set; }

        private void FInit(string valueType, bool includeConditions, ConditionType condition)
        {
            ValueType = valueType;
            IncludeConditions = includeConditions;
            Condition = condition;
        }

        public override void ReInit(LogType logType, LogIniSection section)
        {
            base.ReInit(logType, section);
            FInit(section.Values[KeyValueType],
                (section.Values[KeyIncludeConditions] == "1"),
                (ParentLogType.Conditions[ParentLogType.LogTypeFile.Sections[section.Values[KeyCondition]].SectionName]));
        }
    }
}
