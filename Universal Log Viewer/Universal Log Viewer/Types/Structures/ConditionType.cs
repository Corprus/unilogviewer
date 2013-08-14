using System;
using System.Collections.Generic;
using System.Linq;
using UniversalLogViewer.LogIniFiles;

namespace UniversalLogViewer.Types.Structures
{    
    public class ConditionType : BaseType
    {

        public const string IniTypeName = "Condition";
        const string KeyStartsWith = "StartsWith";
        const string KeyEndsWith = "EndsWith";
        const string KeyContain = "Contain";
        const string KeyExclude = "Exclude";

        public string StartsWith { get; private set; }
        public string EndsWith { get; private set; }
        public List<string> Contain { get; private set; }
        public List<string> Exclude { get; private set; }
        public bool IsCorrect(string value)
        {
            //�������� �� �����
            if ((StartsWith.Length != 0) && (!(value.StartsWith(StartsWith, StringComparison.Ordinal))))
                return false;
            //�������� �� �����
            if ((EndsWith.Length != 0) && (!(value.EndsWith(EndsWith, StringComparison.Ordinal))))
                return false;
            //�������� �� ����������
            return Contain.All(condition => (value.Contains(condition))) &&
                   Exclude.All(condition => (!value.Contains(condition)));
            //�������� �� ����������
        }        
        void FInit(string startsWith, string endsWith, IEnumerable<string> contain, IEnumerable<string> exclude)
        {            
            StartsWith = startsWith;
            EndsWith = endsWith;
            Contain = new List<string>();
            Contain.AddRange(contain);
            Exclude = new List<string>();
            Exclude.AddRange(exclude);
        }

        public override void ReInit(LogType logType, LogIniSection section)
        {
            base.ReInit(logType, section);
            FInit(
                section.Values[KeyStartsWith],
                section.Values[KeyEndsWith],
                section.ArrayValues[KeyContain],
                section.ArrayValues[KeyExclude]);

        }
    }
}
