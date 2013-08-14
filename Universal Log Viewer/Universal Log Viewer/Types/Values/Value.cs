using System;
using System.Windows.Forms;
using UniversalLogViewer.Types.Structures;
using UniversalLogViewer.Common;


namespace UniversalLogViewer.Types.Values 
{
    public class Value : BaseStringValueCollection<Value>
    {
        public new ValuesType StructureType { get { return (base.StructureType as ValuesType); } }

        public Value(ValuesType type, ref string source)
            : base(type, ref source)
        {

        }

        public override TreeNode TreeNode
        {
            get
            {
                TreeNodeValueString = string.Format("{0}: {1}", StructureType.Name, Value);
                return base.TreeNode;
            }
        }
        public override void Parse()
        {            
            Value = GetValue(Source);
        }
        
        public string GetValue(string parsed)
        {
            ConditionType condition = StructureType.Condition;
            int startLength = condition.StartsWith.Length;
            int endLength = condition.EndsWith.Length;
            bool emptyStartsWith = startLength == 0;
            bool emptyEndsWith = endLength == 0;
            int iStart = parsed.IndexOf(condition.StartsWith, StringComparison.Ordinal);
            int iEnd = parsed.IndexOf(condition.EndsWith, StringComparison.Ordinal);
            string preParsedValue = Consts.EmptySymbol;
            
            if (emptyStartsWith && emptyEndsWith)
                preParsedValue = parsed;
            else if (emptyStartsWith && (iEnd >= 0))
                preParsedValue = parsed.Substring(0, iEnd + endLength);
            else if (emptyEndsWith && (iStart >= 0))
                preParsedValue = parsed.Substring(iStart, parsed.Length - iStart);
            else if ((iStart >= 0) && (iEnd >= 0) && (iEnd > iStart))
                preParsedValue = parsed.Substring(iStart, (iEnd - iStart + 1));
            else
                return preParsedValue;
            if (condition.IsCorrect(preParsedValue))
            {
                if (!(StructureType.IncludeConditions))
                {
                    preParsedValue =
                        preParsedValue.Substring(startLength, (preParsedValue.Length - endLength - startLength));
                }
                return preParsedValue;
            }
            return Consts.EmptySymbol;
        }
        //¬џ–≈«ј“№(!!!) »з большой строки то что мы отпарсили
        public string CutSource(string source)
        {
            if ((Value.Length == 0) || (!source.Contains(Value)))
                return Consts.EmptySymbol;
            var start = source.IndexOf(Value, StringComparison.Ordinal);
            var end = start + Value.Length;
            if (!(StructureType.IncludeConditions))
                //≈сли не включаем граничные услови€ в значение - то еще не факт что мы не должны их вырезјть
            {
                start -= StructureType.Condition.StartsWith.Length;
                end += StructureType.Condition.EndsWith.Length;
            }
            return (source.Substring(0, start) + source.Substring(end, source.Length - end));
        }
    }
}
