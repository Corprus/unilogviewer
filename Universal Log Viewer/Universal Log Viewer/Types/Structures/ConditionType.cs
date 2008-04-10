using System;
using System.Collections.Generic;
using System.Text;
using UniversalLogViewer.Types.Structures;
using UniversalLogViewer.Common.IniFiles;

namespace UniversalLogViewer.Types.Structures
{    
    public class ConditionType : BaseType
    {

        public const string INI_TYPE_NAME = "Condition";
        const string KEY_STARTS_WITH = "StartsWith";
        const string KEY_ENDS_WITH = "EndsWith";
        const string KEY_CONTAIN = "Contain";
        const string KEY_EXCLUDE = "Exclude";

        public string StartsWith { get; private set; }
        public string EndsWith { get; private set; }
        public List<string> Contain { get; private set; }
        public List<string> Exclude { get; private set; }
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
        void FInit(string StartsWith, string EndsWith, string[] Contain, string[] Exclude)
        {            
            this.StartsWith = StartsWith;
            this.EndsWith = EndsWith;
            this.Contain = new List<string>();
            this.Contain.AddRange(Contain);
            this.Exclude = new List<string>();
            this.Exclude.AddRange(Exclude);
        }
        public ConditionType(LogType LogType, LogIniSection Section)
            : base(LogType, Section)
        {
        }
        public override void ReInit(LogType LogType, LogIniSection Section)
        {
            base.ReInit(LogType, Section);
            FInit(
                Section.Values[KEY_STARTS_WITH],
                Section.Values[KEY_ENDS_WITH],
                Section.ArrayValues[KEY_CONTAIN],
                Section.ArrayValues[KEY_EXCLUDE]);

        }
        public ConditionType()
            : base ()
        {
        }
    }
}
