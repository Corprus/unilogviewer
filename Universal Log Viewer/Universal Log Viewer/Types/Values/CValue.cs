using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Universal_Log_Viewer.Types.Structures;


namespace Universal_Log_Viewer.Types.Values 
{
    public class CValue : CBaseStringValueType<CValue>
    {
        public new CValueType Type { get { return (base.Type as CValueType); } private set { base.Type = value; } }
        public CValue(CValueType Type, string Source)
            :base(Type, Source)
        {            
        }
        protected override TreeNode GetTreeNode()
        {
            TreeNode Result = null;
            if (this.Type.Style.Visible)
            {
                Result = new TreeNode(this.Type.Name + ":" + this.Value);
                Result.BeginEdit();
                Result.ForeColor = this.Type.Style.Color;
                if (Result.NodeFont == null)
                    Result.NodeFont = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 8);

                if (this.Type.Style.Bold)
                    Result.NodeFont = new System.Drawing.Font(Result.NodeFont, System.Drawing.FontStyle.Bold);
                Result.EndEdit(false);

            }
            return Result;
        }
        public override void Parse()
        {            
            Value = GetValue(Source);
        }
        public string GetValue(string ParsedString)
        {
            int iStart = ParsedString.IndexOf(this.Type.Condition.StartsWith);
            int iEnd = ParsedString.IndexOf(this.Type.Condition.EndsWith);
            string PreParsedValue = Consts.EMPTY_SYMBOL;
            if ((this.Type.Condition.StartsWith == Consts.EMPTY_SYMBOL) && (this.Type.Condition.EndsWith == Consts.EMPTY_SYMBOL))
                PreParsedValue = ParsedString;
            else if ((this.Type.Condition.StartsWith == Consts.EMPTY_SYMBOL) && (iEnd > -1))
                PreParsedValue = ParsedString.Substring(0, iEnd + this.Type.Condition.EndsWith.Length);
            else if ((this.Type.Condition.EndsWith == Consts.EMPTY_SYMBOL) && (iStart > -1))
                PreParsedValue = ParsedString.Substring(iStart, ParsedString.Length - iEnd);
            else if ((iStart > -1) && (iEnd > -1) && (iEnd - iStart > 0))
                PreParsedValue = ParsedString.Substring(iStart, (iEnd - iStart + 1));
            else
                return PreParsedValue;
            if (this.Type.Condition.IsCorrect(PreParsedValue))
            {
                if (!(this.Type.IncludeConditions))
                {
                    PreParsedValue = PreParsedValue.Substring(0, (PreParsedValue.Length - this.Type.Condition.EndsWith.Length));
                    PreParsedValue = PreParsedValue.Substring(this.Type.Condition.StartsWith.Length, PreParsedValue.Length - this.Type.Condition.StartsWith.Length);
                }
                return PreParsedValue;
            }
            else
                return Consts.EMPTY_SYMBOL;
        }
        //¬џ–≈«ј“№(!!!) »з большой строки то что мы отпарсили
        public string CutSourceString(string SourceString)
        {
            if ((Value != Consts.EMPTY_SYMBOL) && (SourceString.Contains(Value)))
            {
                int iStart = SourceString.IndexOf(Value);
                int iEnd = iStart + Value.Length;
                if (!(this.Type.IncludeConditions)) //≈сли не включаем граничные услови€ в значение - то еще не факт что мы не должны их вырезјть
                {
                    iStart -= this.Type.Condition.StartsWith.Length;
                    iEnd += this.Type.Condition.EndsWith.Length;
                }
                return (SourceString.Substring(0, iStart) + SourceString.Substring(iEnd, SourceString.Length - iEnd));
            }
            else
                return Consts.EMPTY_SYMBOL;
        }
    }
}
