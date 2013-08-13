using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using UniversalLogViewer.Types.Structures;
using UniversalLogViewer.Common;


namespace UniversalLogViewer.Types.Values 
{
    public class Value : BaseStringValueCollection<Value>
    {
        public new ValuesType StructureType { get { return (base.StructureType as ValuesType); } }
        public Value(ValuesType Type,ref string Source)
            :base(Type,ref Source)
        {
            
        }
        public override TreeNode TreeNode
        {
            get
            {
                TreeNodeValueString = this.StructureType.Name + ": " + Value;
                return base.TreeNode;
            }
        }
        public override void Parse()
        {            
            Value = GetValue(Source);
        }
        public string GetValue(string Parsed)
        {
            ConditionType condition = this.StructureType.Condition;
            int startLength = condition.StartsWith.Length;
            int endLength = condition.EndsWith.Length;
            bool emptyStartsWith = startLength == 0;
            bool emptyEndsWith = endLength == 0;
            int iStart = Parsed.IndexOf(condition.StartsWith, StringComparison.Ordinal);
            int iEnd = Parsed.IndexOf(condition.EndsWith, StringComparison.Ordinal);
            string PreParsedValue = Consts.EmptySymbol;
            
            if (emptyStartsWith && emptyEndsWith)
                PreParsedValue = Parsed;
            else if (emptyStartsWith && (iEnd >= 0))
                PreParsedValue = Parsed.Substring(0, iEnd + endLength);
            else if (emptyEndsWith && (iStart >= 0))
                PreParsedValue = Parsed.Substring(iStart, Parsed.Length - iStart);
            else if ((iStart >= 0) && (iEnd >= 0) && (iEnd > iStart))
                PreParsedValue = Parsed.Substring(iStart, (iEnd - iStart + 1));
            else
                return PreParsedValue;
            if (condition.IsCorrect(PreParsedValue))
            {
                if (!(this.StructureType.IncludeConditions))
                {
                    PreParsedValue =
                        PreParsedValue.Substring(startLength, (PreParsedValue.Length - endLength - startLength));
                }
                return PreParsedValue;
            }
            else
                return Consts.EmptySymbol;
        }
        //¬џ–≈«ј“№(!!!) »з большой строки то что мы отпарсили
        public string CutSource(string Source)
        {
            if ((Value.Length != 0) && (Source.Contains(Value)))
            {
                int iStart = Source.IndexOf(Value, StringComparison.Ordinal);
                int iEnd = iStart + Value.Length;
                if (!(this.StructureType.IncludeConditions)) //≈сли не включаем граничные услови€ в значение - то еще не факт что мы не должны их вырезјть
                {
                    iStart -= this.StructureType.Condition.StartsWith.Length;
                    iEnd += this.StructureType.Condition.EndsWith.Length;
                }
                return (Source.Substring(0, iStart) + Source.Substring(iEnd, Source.Length - iEnd));
            }
            else
                return Consts.EmptySymbol;
        }
    }
}
