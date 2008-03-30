using System;
using System.Collections.Generic;
using System.Text;
using Universal_Log_Viewer.Types.Structures;
using Universal_Log_Viewer.Common.IniFile;

namespace Universal_Log_Viewer.Types.Structures
{    
    public class CCondition : CBaseType
    {

        public const string INI_TYPE_NAME = "Condition";
        const string KEY_STARTS_WITH = "StartsWith";
        const string KEY_ENDS_WITH = "EndsWith";
        const string KEY_CONTAIN = "Contain";
        const string KEY_EXCLUDE = "Exclude";

        public string StartsWith;
        public string EndsWith;
        public List<string> Contain;
        public List<string> Exclude;
        public bool IsCorrect(string Value)
        {
            //Проверка на старт
            if ((StartsWith != Consts.EMPTY_SYMBOL) && (!(Value.StartsWith(StartsWith))))
                return false;
            //Проверка на конец
            if ((EndsWith != Consts.EMPTY_SYMBOL) && (!(Value.EndsWith(EndsWith))))
                return false;
            //Проверка на содержание
            foreach (string Condition in Contain)
                if (!(Value.Contains(Condition)))
                    return false;
            //Проверка на исключение
            foreach (string Condition in Exclude)
                if ((Value.Contains(Condition)))
                    return false;
            return true;
         }        
        void FInit(string vStartsWith, string vEndsWith, string[] arContain, string[] arExclude)
        {            
            StartsWith = vStartsWith;
            EndsWith = vEndsWith;
            Contain = new List<string>();
            Contain.AddRange(arContain);
            Exclude = new List<string>();
            Exclude.AddRange(arExclude);
        }
        public CCondition(CLogType vLogType, CLogIniSection IniSection)
            : base(vLogType, IniSection)
        {
        }
        public override void ReInit(CLogType vLogType, CLogIniSection IniSection)
        {
            base.ReInit(vLogType, IniSection);
            FInit(
                IniSection.Values[KEY_STARTS_WITH],
                IniSection.Values[KEY_ENDS_WITH],
                IniSection.ArrayValues[KEY_CONTAIN],
                IniSection.ArrayValues[KEY_EXCLUDE]);

        }
        public CCondition()
            : base ()
        {
        }
    }
}
