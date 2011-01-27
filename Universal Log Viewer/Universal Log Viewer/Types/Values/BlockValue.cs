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
        //bool[] ProcessedStrings;
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
        public override TreeNode TreeNode
        {
            get
            {
                TreeNodeValueString = this.StructureType.Title;
                TreeNode Result = base.TreeNode;
                List<Object> ChildList = new List<Object>();
                foreach (StringValue ChildString in ChildStrings)
                    ChildList.Add(ChildString);
                foreach (BlockValue ChildBlock in ChildElements)
                    ChildList.Add(ChildBlock);
                ChildList.Sort(CompareChilds);
                for (int i= ChildList.Count -1; i>=0; i--)
                //foreach (Object Obj in ChildList)
                {
                    var TypeObj = ChildList[i] as BaseValue;
                    if ((TypeObj != null) && ((TypeObj is BlockValue) || (TypeObj is StringValue)))
                        if (TypeObj.StructureType.Style.Visible)
                            Result.Nodes.Insert(0, TypeObj.TreeNode);
                    ChildList.RemoveAt(i);
                }
                ChildList.Clear();

                return Result;
            }
        }
        string[] ParseStringList(string[] SourceList)
        {
            bool bStartConditionDone = false;
            bool bEndConditionDone = false;
            List<string> Result = new List<string>();
            List<string> CutSourceList = new List<string>();
            ConditionType StartCondition = this.StructureType.StartCondition;
            ConditionType EndCondition = this.StructureType.EndCondition;

            for(int i = 0; i < SourceList.Length; i++)
            {
                string CurString = SourceList[i];
                if (!bStartConditionDone)
                {
                    bStartConditionDone = (StartCondition != null)?
                        StartCondition.IsCorrect(CurString):true;

                    if (bStartConditionDone)
                        StartIndex = i;
                }

                if ((!bStartConditionDone) || (bEndConditionDone))
                    CutSourceList.Add(CurString);
                if ((!bEndConditionDone) && (bStartConditionDone))
                {
                    Result.Add(CurString);
                    bEndConditionDone=(EndCondition != null)?EndCondition.IsCorrect(CurString):false;
                }

            }
            CutSourceString = CutSourceList.ToArray();
            return Result.ToArray();
        }
        public override void Parse()
        {
            ChildStrings = new List<StringValue>();
            Value = ParseStringList(Source);
            ProcessStringList();
        }
        int AddProcessedBlock(int StartIndex, int Length, ref List<int> ProcessedStrings)
        {
            int Result = 0;
            int ProcessedCounter = 0;            
            bool bStarted = false;
            int stringsCount = ProcessedStrings.Count;
            for (int i = 0; i < stringsCount; i++)
            {
                if (!(ProcessedStrings.Contains(i)))
                {
                    if (!bStarted)
                        if (ProcessedCounter == StartIndex)
                        {
                            Result = i;
                            bStarted = true;
                            ProcessedCounter = 0;
                            ProcessedStrings.Remove(i);
                        }
                        else
                            ProcessedCounter++;
                    else
                    {
                        if (ProcessedCounter == (Length - 1))
                            return Result;
                        else
                        {
                            ProcessedStrings.Remove(i);
                            ProcessedCounter++;
                        }
                    }
                }
            }
            return stringsCount;
        }
        public BlockValue(BlockType Type, ref string[] Source)
            : base(Type, ref Source)
        {
            
        }
        ~BlockValue()
        {
//            ProcessedStrings = null;
        }
            
        public void ProcessStringList()
        {
            //bool[] ProcessedStrings = new bool[Source.Length];
            List<int> ProcessedStrings = new List<int>(Source.Length);
            for (int i = 0; i < ProcessedStrings.Count; i++)
                ProcessedStrings[i] = i;

            string[] ProcessedList = Value;
            //Проверка блоков и генерация дочерних блоков (и их процесинг)
            foreach (BlockType oBlockType in this.StructureType.ChildBlockTypes)
            {
                bool NoBlocksToProcess = false;
                while (!(NoBlocksToProcess))
                {

                    NoBlocksToProcess = true;
                    BlockValue NewBlock = new BlockValue(oBlockType,ref ProcessedList);
                    if (!(NewBlock.IsEmpty))
                    {
                        ProcessedList = NewBlock.CutSourceString;
                        NewBlock.StartIndex = AddProcessedBlock(NewBlock.StartIndex, NewBlock.Value.Length,ref ProcessedStrings);
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
                    using (StringValue NewString = new StringValue(oStringType, ref ProcessedList[I]))
                    {
                        if (NewString.ConditionCorrect)
                        {
                            NewString.StartIndex = AddProcessedBlock(0, 1, ref ProcessedStrings);
                            ChildStrings.Add(NewString);
                            UniversalLogViewer.Program.MainForm.LogProgress.IncreaseProgressLevel(1);
                            break;
                        }
                    }
                }
            }
        }
    }
}

