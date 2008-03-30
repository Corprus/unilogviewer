using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Universal_Log_Viewer.Types.Structures;

namespace Universal_Log_Viewer.Types.Values
{
    

    public class CBlock : CBaseStringsValueType<CBlock>
    {
        public new CBlockType Type { get { return base.Type as CBlockType; } private set { base.Type = value; } }
        public new string[] Value { get; private set; }
        public bool IsEmpty { get { return (Value.Length == 0); } }
        public int StartIndex;
        bool[] ProcessedStrings;
        public List<CString> ChildStrings { get; private set; }
        public string[] CutSourceString { get; private set; }
        static bool IsBlock(Object Obj) { return (Obj is CBlock); }
        static bool IsString(Object Obj) { return (Obj is CString); }
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
                    StartIndexA = (a as CString).StartIndex;
                else //Block
                    StartIndexA = (a as CBlock).StartIndex;
                if (IsString(b))
                    StartIndexB = (b as CString).StartIndex;
                else //Block
                    StartIndexB = (b as CBlock).StartIndex;
                return (StartIndexA - StartIndexB);
            }

            
            
        }
        protected override TreeNode GetTreeNode()
        {
            TreeNode Result = null;
            if (this.Type.Style.Visible)
            {
                Result = new TreeNode(this.Type.Title);
                Result.BeginEdit();
                Result.ForeColor = this.Type.Style.Color;
                if (Result.NodeFont == null)
                    Result.NodeFont = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 8);
                if (this.Type.Style.Bold)
                    Result.NodeFont = new System.Drawing.Font(Result.NodeFont, System.Drawing.FontStyle.Bold);
                List<Object> ChildList = new List<Object>();
                foreach (CString ChildString in ChildStrings)
                    ChildList.Add(ChildString);
                foreach (CBlock ChildBlock in ChildElements)
                    ChildList.Add(ChildBlock);
                ChildList.Sort(CompareChilds);
                foreach (Object Obj in ChildList)
                {
                    if ((Obj is CBlock) && ((Obj as CBlock).Type.Style.Visible))
                        Result.Nodes.Add((Obj as CBlock).TreeNode);
                    if ((Obj is CString) && ((Obj as CString).Type.Style.Visible))
                        Result.Nodes.Add((Obj as CString).TreeNode);


                }
                Result.EndEdit(false);
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
                    if (this.Type.StartCondition != null)
                        bStartConditionDone = this.Type.StartCondition.IsCorrect(CurString);
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
                    if (this.Type.EndCondition != null)
                        bEndConditionDone = this.Type.EndCondition.IsCorrect(CurString);
                }

            }
            CutSourceString = CutSourceList.ToArray();
            return Result.ToArray();
        }
        public override void Parse()
        {
            ChildStrings = new List<CString>();
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
        public CBlock(CBlockType Type, string[] Source)
            : base(Type, Source)
        {
            
        }
        public void ProcessStringList()
        {
            string[] ProcessedList = Value;
            //Проверка блоков и генерация дочерних блоков (и их процесинг)
            foreach (CBlockType oBlockType in this.Type.ChildBlockTypes)
            {
                bool NoBlocksToProcess = false;
                while (!(NoBlocksToProcess))
                {

                    NoBlocksToProcess = true;
                    CBlock NewBlock = new CBlock(oBlockType, ProcessedList);
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
                foreach (CStringType oStringType in this.Type.ChildStringTypes)
                {
                    CString NewString = new CString(oStringType, ProcessedList[I]);
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

