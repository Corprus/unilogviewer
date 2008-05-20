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
        public override TreeNode GetTreeNode()
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
                            TitleElement = ChildElements[this.StructureType.TitleValueIndex];
                            if ((this.StructureType.TitleValueType.Length > 0) && (TitleElement.StructureType.Name != this.StructureType.TitleValueType))
                                TitleElement = null;
                        }
                        if (TitleElement == null)
                            NodeTitle = this.StructureType.Title;
                        else
                            NodeTitle = TitleElement.Value;
                        break;
                    }
                default: NodeTitle = this.StructureType.Title; break;
            }
            TreeNodeValueString = NodeTitle;
            TreeNode Result = base.GetTreeNode();
            if (IniSettingsManager.ShowValueMemo)
                ((TreeTag)Result.Tag).Obj = this;
            if (!(IniSettingsManager.ShowValueMemo))
                Result.Nodes.AddRange(CachedChildTreeNodes.ToArray());
            //                UniversalLogViewer.Program.MainForm.IncreaseProgressLevel(1);
            return Result;
        }
    

        void ProcessString()
        {
            bool bStringProcessed = false;
            string sProcessedString = Source;

            ChildElements = new List<Value>();
            ChildElements.Clear();
            ChildElements.TrimExcess();
          
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
                    Value NewValue = new Value(UsedType, SeparatedStrings[i]);
                    if (NewValue.Value.Length != 0)
                        ChildElements.Add(NewValue);
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
                        Value NewValue = new Value(VType, sProcessedString);
                        if (NewValue.Value.Length != 0)
                        {
                            ChildElements.Add(NewValue);
                            sProcessedString = NewValue.CutSource(sProcessedString);
                        }
                    }
                }
            FCache(ChildElements);
        }
        public override void Parse()
        {
            ProcessString();
        }
        public StringValue(StringType StringType, string Source)
            : base(StringType, Source)
        {
            this.Value = Source;
            this.StructureType = StringType;
        }
        private string[] CachedValues;
        private void FCacheValues(List<Value> LocalChildren)
        {
            List<string> Result = new List<string>();
            Result.Add("Contents of " + this.StructureType.Title);
            foreach (Value ChildValue in LocalChildren)
                Result.Add(ChildValue.StructureType.Name + ": " + ChildValue.Value);
            CachedValues = Result.ToArray();
        }
        public string[] GetValues()
        {
            return CachedValues;
        }
        private void FCache(List<Value> LocalChildren)
        {
            if (IniSettingsManager.ShowValueMemo)
                FCacheValues(LocalChildren);
            else
                FCacheTreeNodes(LocalChildren);                    
        }
        private List<TreeNode> CachedChildTreeNodes;
        private void FCacheTreeNodes(List<Value> LocalChildren)
        {
            CachedChildTreeNodes = new List<TreeNode>();
            foreach (Value ResultValue in LocalChildren)
                if (ResultValue.StructureType.Style.Visible)
                CachedChildTreeNodes.Add(ResultValue.GetTreeNode());                 

        }
        protected override void Dispose(bool disposing)
        {
            CachedChildTreeNodes.Clear();
            CachedChildTreeNodes.TrimExcess();
            base.Dispose(disposing);
        }
    }
}
