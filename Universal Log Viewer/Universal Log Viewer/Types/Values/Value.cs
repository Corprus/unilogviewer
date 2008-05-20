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
        public Value(ValuesType Type, string Source)
            :base(Type, Source)
        {            
        }
        public override TreeNode GetTreeNode()
        {

            TreeNodeValueString = this.StructureType.Name + ": " + Value;
            return base.GetTreeNode();
        }
        public override void Parse()
        {            
            Value = GetValue(Source);
        }
        public string GetValue(string Parsed)
        {
            int iStart = Parsed.IndexOf(this.StructureType.Condition.StartsWith, StringComparison.Ordinal);
            int iEnd = Parsed.IndexOf(this.StructureType.Condition.EndsWith, StringComparison.Ordinal);
            string PreParsedValue = Consts.EMPTY_SYMBOL;
            if ((this.StructureType.Condition.StartsWith.Length == 0) && (this.StructureType.Condition.EndsWith.Length == 0))
                PreParsedValue = Parsed;
            else if ((this.StructureType.Condition.StartsWith.Length == 0) && (iEnd > -1))
                PreParsedValue = Parsed.Substring(0, iEnd + this.StructureType.Condition.EndsWith.Length);
            else if ((this.StructureType.Condition.EndsWith.Length == 0) && (iStart > -1))
                PreParsedValue = Parsed.Substring(iStart, Parsed.Length - iStart);
            else if ((iStart > -1) && (iEnd > -1) && (iEnd - iStart > 0))
                PreParsedValue = Parsed.Substring(iStart, (iEnd - iStart + 1));
            else
                return PreParsedValue;
            if (this.StructureType.Condition.IsCorrect(PreParsedValue))
            {
                if (!(this.StructureType.IncludeConditions))
                {
                    PreParsedValue = PreParsedValue.Substring(0, (PreParsedValue.Length - this.StructureType.Condition.EndsWith.Length));
                    PreParsedValue = PreParsedValue.Substring(this.StructureType.Condition.StartsWith.Length, PreParsedValue.Length - this.StructureType.Condition.StartsWith.Length);
                }
                return PreParsedValue;
            }
            else
                return Consts.EMPTY_SYMBOL;
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
                return Consts.EMPTY_SYMBOL;
        }
    }
}
