using System.Collections.Generic;
using UniversalLogViewer.LogIniFiles;


namespace UniversalLogViewer.Types.Structures
{
    public class BlockType : BaseType
    {
        public const string IniTypeName = "Block";
        const string IniKeyStartCondition = "StartCondition";
        const string IniKeyEndCondition = "EndCondition";
        const string IniKeyChildBlockTypes = "ChildBlockTypes";
        const string IniKeyChildStringTypes = "ChildStringTypes";        
        public ConditionType StartCondition { get; private set; }
        public ConditionType EndCondition { get; private set; }
        public List<BlockType> ChildBlockTypes { get; private set; }
        public List<StringType> ChildStringTypes { get; private set; }

        public override void ReInit(LogType logType, LogIniSection section)
        {
            base.ReInit(logType, section);
            StartCondition = ParentLogType.Conditions[section.Values[IniKeyStartCondition]];
            EndCondition = ParentLogType.Conditions[section.Values[IniKeyEndCondition]];
            ChildBlockTypes = (ParentLogType.BlockTypes.GetList(section.ArrayValues[IniKeyChildBlockTypes]));
            ChildStringTypes = (ParentLogType.StringTypes.GetList(section.ArrayValues[IniKeyChildStringTypes]));
            if ((ChildBlockTypes.Count == 0) && (ChildStringTypes.Count == 0))
                throw new Common.Exceptions.LogTypeLoadException("Both ChildBLockTypes and ChildStringTypes are empty");
        }
    }
}
