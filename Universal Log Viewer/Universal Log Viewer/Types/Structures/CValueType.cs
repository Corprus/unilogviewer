using System;
using System.Collections.Generic;
using System.Text;
using Universal_Log_Viewer.Common.IniFile;

namespace Universal_Log_Viewer.Types.Structures
{
    public class CValueType : CBaseType
    {
        public const string INI_TYPE_NAME = "Value";
        const string KEY_NAME = "Name";
        const string KEY_INCLUDE_CONDITIONS = "IncludeConditions";
        const string KEY_VALUE_TYPE = "ValueType";
        const string KEY_CONDITION = "Condition";
        public CCondition Condition { get; private set; }
        public bool IncludeConditions { get; private set; }
        public string ValueType { get; private set; }

        void FInit(string vValueType, bool vIncludeConditions, CCondition vCondition)
        {
            ValueType = vValueType;
            IncludeConditions = vIncludeConditions;
            Condition = vCondition;
        }
        public CValueType(CLogType vLogType, CLogIniSection IniSection)
            : base(vLogType, IniSection)
        {
        }

        public override void ReInit(CLogType vLogType, CLogIniSection IniSection)
        {
            base.ReInit(vLogType, IniSection);
            FInit(IniSection.Values[KEY_VALUE_TYPE],
                (IniSection.Values[KEY_INCLUDE_CONDITIONS] == "1"),
                (ParentLogType.Conditions[ParentLogType.LogIniFile.Sections[IniSection.Values[KEY_CONDITION]].SectionName]));
        }
        public CValueType()
            : base()
        {
        }
    }
}
