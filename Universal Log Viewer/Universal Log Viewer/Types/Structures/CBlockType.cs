using System;
using System.Collections.Generic;
using System.Text;
using Universal_Log_Viewer.Common.IniFile;

namespace Universal_Log_Viewer.Types.Structures
{
    public class CBlockType : CBaseType
    {
        public const string INI_TYPE_NAME = "Block";
        const string INI_KEY_START_CONDITION = "StartCondition";
        const string INI_KEY_END_CONDITION = "EndCondition";
        const string INI_KEY_CHILD_BLOCK_TYPES = "ChildBlockTypes";
        const string INI_KEY_CHILD_STRING_TYPES = "ChildStringTypes";        
        public CCondition StartCondition { get; private set; }
        public CCondition EndCondition { get; private set; }
        public List<CBlockType> ChildBlockTypes { get; private set; }
        public List<CStringType> ChildStringTypes { get; private set; }
        public CBlockType(CLogType vLogType, CLogIniSection IniSection)
            : base(vLogType, IniSection)
        {
        }

        public override void ReInit(CLogType LogType, CLogIniSection Section)
        {
            base.ReInit(LogType, Section);
            StartCondition = ParentLogType.Conditions[IniSection.Values[INI_KEY_START_CONDITION]];
            EndCondition = ParentLogType.Conditions[IniSection.Values[INI_KEY_END_CONDITION]];
            ChildBlockTypes = (ParentLogType.BlockTypes.GetList(IniSection.ArrayValues[INI_KEY_CHILD_BLOCK_TYPES]));
            ChildStringTypes = (ParentLogType.StringTypes.GetList(IniSection.ArrayValues[INI_KEY_CHILD_STRING_TYPES]));

        }
        public CBlockType()
            : base()
        {
        }
    }
}
