using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using UniversalLogViewer;
using UniversalLogViewer.Types.Structures;
using UniversalLogViewer.Common.Types.Managers;


namespace UniversalLogViewer.Types.Values
{
    
    public class StringValue : BaseStringValueCollection<Value>
    {
        public new StringType StructureType { get { return (base.StructureType as StringType); } private set { base.StructureType = value; } }
        public bool ConditionCorrect { get { return this.StructureType.Condition.IsCorrect(Value); } }
        public int StartIndex { get; set; }
        public override TreeNode TreeNode
        {
            get
            {

                string NodeTitle;
                switch (this.StructureType.TitleType)
                {
                    case Common.TitleType.Title: NodeTitle = this.StructureType.Title; break;
                    case Common.TitleType.Source: NodeTitle = this.Source; break;
                    case Common.TitleType.Value:
                        {
                            Value TitleElement = null;
                            if (ChildElements.Count > this.StructureType.TitleValueIndex)
                            {
                                Value childElementValue = ChildElements[this.StructureType.TitleValueIndex];
                                if (!((this.StructureType.TitleValueType.Length > 0) && 
                                   (childElementValue.StructureType.Name != this.StructureType.TitleValueType)))
                                    TitleElement = childElementValue;
                            }
                            NodeTitle = (TitleElement == null) ? this.StructureType.Title : TitleElement.Value;
                            break;
                        }
                    default: NodeTitle = this.StructureType.Title; break;
                }
                TreeNodeValueString = NodeTitle;
                TreeNode Result = base.TreeNode;
                if ((!IniSettingsManager.ShowValueMemo))
                {
                    for (int i = ChildElements.Count - 1; i >= 0; i--)
                    {
                        Value ResultValue = ChildElements[i];
                        if (ResultValue.StructureType.Style.Visible)
                        {
                            Result.Nodes.Insert(0, ResultValue.TreeNode);
                        }
                        ChildElements.RemoveAt(i);
                    }
                }
                ChildElements.Clear();
                Program.MainForm.LogProgress.IncreaseProgressLevel(1);
                return Result;
            }
        }
    

        List<Value> ProcessString()
        {
            bool bStringProcessed = false;
            string sProcessedString = Source;
            List<Value> Values = ChildElements;
            if (Values == null)
                Values = new List<Value>();
            Values.Clear();
            
            if (this.StructureType.UseSeparator)
            {
                String[] SeparatedStrings = sProcessedString.Split(this.StructureType.Separator);
                ValuesType[] Types = this.StructureType.ChildTypes.ToArray();
                /* “ут не до конца пон€тно как минимум брать и что делать с отсе€нными пол€ми,
                 * ну да и хрен с ними */
                for (int i = 0; i < SeparatedStrings.Length; i++)
                {
                    ValuesType UsedType;
                    if (i >= Types.Length)
                        UsedType = Types[Types.Length - 1];
                    else
                        UsedType = Types[i];
                    Value NewValue = new Value(UsedType,ref SeparatedStrings[i]);
                    if (NewValue.Value.Length != 0)
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
                    foreach (ValuesType VType in this.StructureType.ChildTypes)
                    {
                        Value NewValue = new Value(VType,ref sProcessedString);
                        if (NewValue.Value.Length != 0)
                        {
                            Values.Add(NewValue);
                            sProcessedString = NewValue.CutSource(sProcessedString);
                        }
                    }
                }
            return Values;
        }
        public override void Parse()
        {
            ProcessString();
        }
        public StringValue(StringType StringType,ref string Source)
            : base(StringType, ref Source)
        {
            this.Value = Source;
            this.StructureType = StringType;
        }
        public string[] GetValues()
        {
            List<string> Result = new List<string>();
            Result.Add("Contents of " + this.StructureType.Title);
            foreach (Value ChildValue in ChildElements)
                Result.Add(ChildValue.StructureType.Name + ": " + ChildValue.Value);
            return Result.ToArray();
        }
    }
}
