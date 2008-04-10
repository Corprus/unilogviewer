using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using UniversalLogViewer.Types.Structures;

namespace UniversalLogViewer.Types.Values
{
    

    public class BlockValue : BaseStringsValueCollection<BlockValue>
    {
        public new BlockType StructureType { get { return base.StructureType as BlockType; }}
        public bool IsEmpty { get { return (Value.Length == 0); } }
        public int StartIndex { get; private set; }
        bool[] ProcessedStrings;
        public List<StringValue> ChildStrings { get; private set; }
        public string[] CutSourceString { get; private set; }
        static bool IsBlock(Object Obj) { return (Obj is BlockValue); }
        static bool IsString(Object Obj) { return (Obj is StringValue); }
        static bool IsStringOrBlock(Object Obj) { return ((IsString(Obj)) || (IsBlock(Obj))); }

        private static int CompareChilds(Object a, Object b)
        {
            int StartIndexA;
            int StartIndexB;
            if ((!IsStringOrBlock(a)) || (!IsStringOrBlock(b)))
                return 0;
            else
            {
                if (IsString(a))
                    StartIndexA = (a as StringValue).StartIndex;
                else //Block
                    StartIndexA = (a as BlockValue).StartIndex;
                if (IsString(b))
                    StartIndexB = (b as StringValue).StartIndex;
                else //Block
                    StartIndexB = (b as BlockValue).StartIndex;
                return (StartIndexA - StartIndexB);
            }

            
            
        }
        protected override TreeNode GetTreeNode()
        {
            TreeNodeValueString = this.StructureType.Title;
            TreeNode Result = base.GetTreeNode();
            List<Object> ChildList = new List<Object>();
            foreach (StringValue ChildString in ChildStrings)
                ChildList.Add(ChildString);
            foreach (BlockValue ChildBlock in ChildElements)
                ChildList.Add(ChildBlock);
            ChildList.Sort(CompareChilds);
            foreach (Object Obj in ChildList)
            {
                var TypeObj = Obj as BaseValue;
                if ((TypeObj != null)&&((TypeObj is BlockValue)||(TypeObj is StringValue)))
                if  (TypeObj.StructureType.Style.Visible)
                    Result.Nodes.Add(TypeObj.TreeNode);
            }
            return Result;
        }
        string[] ParseStringList(string[] SourceList)
        {
            bool bStartConditionDone = false;
            bool bEndConditionDone = false;
            List<string> Result = new List<string>();
            List<string> CutSourceList = new List<string>();
            for(int i = 0; i < SourceList.Length; i++)
            {
                string CurString = SourceList[i];
                if (!(bStartConditionDone))
                {
                    if (this.StructureType.StartCondition != null)
                        bStartConditionDone = this.StructureType.StartCondition.IsCorrect(CurString);
                    else
                        bStartConditionDone = true;

                    if (bStartConditionDone)
                        StartIndex = i;
                }

                if ((!bStartConditionDone) || (bEndConditionDone))
                    CutSourceList.Add(CurString);
                if ((!(bEndConditionDone)) && (bStartConditionDone))
                {
                    Result.Add(CurString);
                    if (this.StructureType.EndCondition != null)
                        bEndConditionDone = this.StructureType.EndCondition.IsCorrect(CurString);
                }

            }
            CutSourceString = CutSourceList.ToArray();
            return Result.ToArray();
        }
        public override void Parse()
        {
            ChildStrings = new List<StringValue>();
            ProcessedStrings = new bool[Source.Length];
            for (int i = 0; i < ProcessedStrings.Length; i++)
                ProcessedStrings[i] = false;
            Value = ParseStringList(Source);
            ProcessStringList();
        }
        int AddProcessedBlock(int StartIndex, int Length)
        {
            int Result = 0;
            int ProcessedCounter = 0;            
            bool bStarted = false;
            for (int i = 0; i < ProcessedStrings.Length; i++)
            {
                if (!(ProcessedStrings[i]))
                {
                    if (!bStarted)
                        if (ProcessedCounter == StartIndex)
                        {
                            Result = i;
                            bStarted = true;
                            ProcessedCounter = 0;
                            ProcessedStrings[i] = true;
                        }
                        else
                            ProcessedCounter++;
                    else
                    {
                        if (ProcessedCounter == (Length - 1))
                            return Result;
                        else
                        {
                            ProcessedStrings[i] = true;
                            ProcessedCounter++;
                        }
                    }
                }
            }
            return ProcessedStrings.Length;
        }
        public BlockValue(BlockType Type, string[] Source)
            : base(Type, Source)
        {
            
        }
        public void ProcessStringList()
        {
            string[] ProcessedList = Value;
            //Проверка блоков и генерация дочерних блоков (и их процесинг)
            foreach (BlockType oBlockType in this.StructureType.ChildBlockTypes)
            {
                bool NoBlocksToProcess = false;
                while (!(NoBlocksToProcess))
                {

                    NoBlocksToProcess = true;
                    BlockValue NewBlock = new BlockValue(oBlockType, ProcessedList);
                    if (!(NewBlock.IsEmpty))
                    {
                        ProcessedList = NewBlock.CutSourceString;
                        NewBlock.StartIndex = AddProcessedBlock(NewBlock.StartIndex, NewBlock.Value.Length);
                        ChildElements.Add(NewBlock);
                        NoBlocksToProcess = false;
                    }
                }
            }
            //Проверка строк
            for (int I = 0; I < ProcessedList.Length; I++)
            {
                foreach (StringType oStringType in this.StructureType.ChildStringTypes)
                {
                    StringValue NewString = new StringValue(oStringType, ProcessedList[I]);
                    if (NewString.ConditionCorrect)
                    {
                        NewString.StartIndex = AddProcessedBlock(0, 1);
                        ChildStrings.Add(NewString);
                        break;
                    }
                }
            }
        }
    }
}

