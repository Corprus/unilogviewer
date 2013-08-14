using System.Collections.Generic;
using System.Linq;
using UniversalLogViewer.LogIniFiles;

namespace UniversalLogViewer.Types.Structures
{
    public class StringType : BaseType
    {
        public const string IniTypeName = "String";
        const string IniKeyCondition = "Condition";
        const string IniKeyUseSeparator = "UseSeparator";
        const string IniKeySeparator = "Separator";
        const string IniKeyChildTypes = "ChildValueTypes";
        const string IniKeyTitleValueIndex = "TitleValueIndex";
        const string IniKeyTitleValueType = "TitleValueType";
        public ConditionType Condition { get; private set; }
        public bool UseSeparator { get; private set; }
        public char[] Separator { get; private set; }
        public int TitleValueIndex { get; private set; }
        public string TitleValueType { get; private set; }
        public List<ValuesType> ChildTypes { get; private set; }

        public override void ReInit(LogType logType, LogIniSection section)
        {
            base.ReInit(logType, section);
            Condition = ParentLogType.Conditions[section.Values[IniKeyCondition]];
            UseSeparator = (section.Values[IniKeyUseSeparator] == "1");
            if (section.Values[IniKeySeparator].Length > 0)
            {
                var separatorArray = section.ArrayValues[IniKeySeparator, false];
                Separator = separatorArray.Select(separatorString => separatorString[0]).ToArray();
                int temporary;
                if (!(int.TryParse(section.Values[IniKeyTitleValueIndex], out temporary)))
                    temporary = 0;
                TitleValueIndex = temporary;
                TitleValueType = section.Values[IniKeyTitleValueType];
            }
            else
                UseSeparator = false;

            var children = section.ArrayValues[IniKeyChildTypes];
            ChildTypes = new List<ValuesType>();
            ChildTypes.AddRange(children.AsParallel().Select(typeName => ParentLogType.ValueTypes[typeName]));
        }
    }
}
