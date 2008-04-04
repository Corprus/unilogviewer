using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Universal_Log_Viewer.Types.Structures;


namespace Universal_Log_Viewer.Types.Values
{
    
    public class CString : CBaseStringValueType<CValue>
    {
        public new CStringType Type { get { return (base.Type as CStringType); } private set { base.Type = value; } }
        public bool ConditionCorrect { get { return this.Type.Condition.IsCorrect(Value); } }
        public int StartIndex;
        protected override TreeNode GetTreeNode()
        {
            string NodeTitle;
            switch (this.Type.TitleType)
            {
                case CBaseType.TitleTypes.Title: NodeTitle = this.Type.Title; break;
                case CBaseType.TitleTypes.Source: NodeTitle = this.Source; break;
                case CBaseType.TitleTypes.Value:
                    {
                        CValue TitleElement = null;
                        if (ChildElements.Count > this.Type.TitleValueIndex)
                        {
                            TitleElement = ChildElements[this.Type.TitleValueIndex];
                            if ((this.Type.TitleValueType != "") && (TitleElement.Type.Name != this.Type.TitleValueType))
                                TitleElement = null;
                        }
                        if (TitleElement == null)
                            NodeTitle = this.Type.Title;
                        else
                            NodeTitle = TitleElement.Value;
                        break;
                    }
                default: NodeTitle = this.Type.Title; break;
            }
            _TreeNodeValueString = NodeTitle;
            TreeNode Result = base.GetTreeNode();
            foreach (CValue ResultValue in ChildElements)
                if (ResultValue.Type.Style.Visible)
                    Result.Nodes.Add(ResultValue.TreeNode);
            return Result;
        }

        List<CValue> ProcessString()
        {
            bool bStringProcessed = false;
            string sProcessedString = Source;
            List<CValue> Values = new List<CValue>();
            
            if (this.Type.UseSeparator)
            {
                String[] SeparatedStrings = sProcessedString.Split(this.Type.Separator);
                CValueType[] Types = this.Type.ChildTypes.ToArray();
                /* “ут не до конца пон€тно как минимум брать и что делать с отсе€нными пол€ми,
                 * ну да и хрен с ними */
                for (int i = 0; i < SeparatedStrings.Length; i++)
                {
                    CValueType UsedType;
                    if (i >= Types.Length)
                        UsedType = Types[Types.Length - 1];
                    else
                        UsedType = Types[i];
                    CValue NewValue = new CValue(UsedType, SeparatedStrings[i]);
                    if (NewValue.Value != Consts.EMPTY_SYMBOL)
                        Values.Add(NewValue);
                }
            }
            else
            /* ≈сли опци€ Ќ≈ использовать сепаратор, тогда начинаем веселитьс€,
             * провер€€ на соответствие и приставива€ соотв. пол€. “о есть автоматическое 
             * распарсивание по услови€м   */
                while (!(bStringProcessed))
                {
                    bStringProcessed = true;
                    foreach (CValueType VType in this.Type.ChildTypes)
                    {
                        CValue NewValue = new CValue(VType, sProcessedString);
                        if (NewValue.Value != Consts.EMPTY_SYMBOL)
                        {
                            Values.Add(NewValue);
                            sProcessedString = NewValue.CutSourceString(sProcessedString);
                        }
                    }
                }
            ChildElements = Values;
            return Values;
        }
        public override void Parse()
        {
            ChildElements = ProcessString();
        }
        public CString(CStringType StringType, string Source)
            : base(StringType, Source)
        {
            this.Value = Source;
            this.Type = StringType;
        }        
    }
}
