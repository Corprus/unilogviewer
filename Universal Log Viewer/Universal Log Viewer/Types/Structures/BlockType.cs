using System;
using System.Collections.Generic;
using System.Text;
using UniversalLogViewer.Common.IniFiles;

namespace UniversalLogViewer.Types.Structures
{
    public class BlockType : BaseType
    {
        public const string INI_TYPE_NAME = "Block";
        const string INI_KEY_START_CONDITION = "StartCondition";
        const string INI_KEY_END_CONDITION = "EndCondition";
        const string INI_KEY_CHILD_BLOCK_TYPES = "ChildBlockTypes";
        const string INI_KEY_CHILD_STRING_TYPES = "ChildStringTypes";        
        public ConditionType StartCondition { get; private set; }
        public ConditionType EndCondition { get; private set; }
        public List<BlockType> ChildBlockTypes { get; private set; }
        public List<StringType> ChildStringTypes { get; private set; }
        public BlockType(LogType LogType, LogIniSection Section)
            : base(LogType, Section)
        {
        }

        public override void ReInit(LogType LogType, LogIniSection Section)
        {
            base.ReInit(LogType, Section);
            StartCondition = ParentLogType.Conditions[Section.Values[INI_KEY_START_CONDITION]];
            EndCondition = ParentLogType.Conditions[Section.Values[INI_KEY_END_CONDITION]];
            ChildBlockTypes = (ParentLogType.BlockTypes.GetList(Section.ArrayValues[INI_KEY_CHILD_BLOCK_TYPES]));
            ChildStringTypes = (ParentLogType.StringTypes.GetList(Section.ArrayValues[INI_KEY_CHILD_STRING_TYPES]));
        }
        public BlockType()
            : base()
        {
        }
    }
}
