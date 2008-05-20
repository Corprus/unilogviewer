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
        public bool IsEmpty { get { return (Length == 0); } }
        public int StartIndex { get; private set; }
        public int EndIndex { get; private set; }
        public int Length { get; private set;}
        List<bool> ProcessedStrings;
        public List<StringValue> ChildStrings { get; private set; }
        List<TreeNode> ChildTreeNodes;
        public List<string> CutSourceString
        {
            get
            {

                if ((StartIndex <= EndIndex))
                {
                    List<string> Result = new List<string>();
                    for (int i = 0; i < StartIndex; i++)
                        Result.Add(Source[i]);

                    for (int i = EndIndex + 1; i < Source.Count; i++)
                        Result.Add(Source[i]);

                    return Result;
                }
                return null;
            }
        }

        static bool IsBlock(TreeNode Obj) { return (Obj.Tag == null); }
        static bool IsString(TreeNode Obj) { return (Obj.Tag is StringValue); }
        static bool IsStringOrBlock(TreeNode Obj) { return ((IsString(Obj)) || (IsBlock(Obj))); }

        private static int CompareChilds(Object a, Object b)
        {
            int StartIndexA;
            int StartIndexB;
            TreeNode aNode = a as TreeNode;
            TreeNode bNode = b as TreeNode;
            if ((aNode != null) && (bNode != null))
            {
                if ((!IsStringOrBlock(aNode)) || (!IsStringOrBlock(bNode)))
                    return 0;
                else
                {
                    StartIndexA = ((TreeTag)aNode.Tag).StartIndex;
                    StartIndexB = ((TreeTag)bNode.Tag).StartIndex;
                    return (StartIndexA - StartIndexB);
                }
            }
            else
                return 0;

            
            
        }
        public override TreeNode GetTreeNode()
        {
            TreeNodeValueString = this.StructureType.Title;
            TreeNode Result = base.GetTreeNode();
            if (Result != null)
            {
                ChildTreeNodes.Sort(CompareChilds);
                Result.Nodes.AddRange(ChildTreeNodes.ToArray());

                /*                foreach (Object Obj in ChildTreeNodes)
                                {
                                    var TypeObj = Obj as BaseValue;
                                    if ((TypeObj != null) && ((TypeObj is BlockValue) || (TypeObj is StringValue)))
                                        if (TypeObj.StructureType.Style.Visible)
                                            Result.Nodes.Add(TypeObj.TreeNode);
                                }
                 */
                UniversalLogViewer.Program.MainForm.IncreaseProgressLevel(ChildTreeNodes.Count);             
            }
            return Result;
        }
        string[] ParseStringList(List<string> SourceList)
        {
            bool bStartConditionDone = false;
            bool bEndConditionDone = false;
            List<string> Result = new List<string>();
            List<string> CutSourceList = new List<string>();
            for(int i = 0; i < SourceList.Count; i++)
            {
                string CurString = SourceList[i];
                if (!(bStartConditionDone))
                {
                    if (this.StructureType.StartCondition != null)
                        bStartConditionDone = this.StructureType.StartCondition.IsCorrect(CurString);
                    else
                        bStartConditionDone = true;

                    if (bStartConditionDone)
                    {
                        StartIndex = i;
                        EndIndex = SourceList.Count - 1;
                    }
                }

/*                if ((!bStartConditionDone) || (bEndConditionDone))
                    CutSourceList.Add(CurString);
 */
                if ((!(bEndConditionDone)) && (bStartConditionDone))
                {
                    Result.Add(CurString);
                    if (this.StructureType.EndCondition != null)
                        bEndConditionDone = this.StructureType.EndCondition.IsCorrect(CurString);
                    if (bEndConditionDone)
                        EndIndex = i;
                }
            }
            return Result.ToArray();
        }
        public override void Parse()
        {
            ChildStrings = new List<StringValue>();
            Value = new string[0];
            ParseStringList(Source);
            if (EndIndex == 0)
                Length = 0;
            else
                Length = EndIndex - StartIndex + 1;
            ProcessStringList();
        }
        int CurrentStartingProcessedIndex;
        int AddProcessedBlock(int StartIndex, int Length)
        {
            int Result = 0;
            int ProcessedCounter = 0;            
            bool bStarted = false;
            for (int i = 0; i < ProcessedStrings.Count; i++)
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
                        {
                            ProcessedStrings.RemoveRange(0, i);
                            ProcessedStrings.TrimExcess();
                            CurrentStartingProcessedIndex += i;
                            return Result;
                        }
                        else
                        {
                            ProcessedStrings[i] = true;
                            ProcessedCounter++;
                        }
                    }
                }
            }
            return ProcessedStrings.Count;
        }
        public BlockValue(BlockType Type, List<string> Source)
            : base(Type, Source)
        {
            CurrentStartingProcessedIndex = 0;
            
        }
        public void ProcessStringList()
        {
            ProcessedStrings = new List<bool>(Source.Count);
            for (int i = 0; i < ProcessedStrings.Capacity; i++)
                ProcessedStrings.Add(false);

            ChildTreeNodes = new List<TreeNode>();
            List<string> ProcessedList = Source;
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
                        NewBlock.StartIndex = AddProcessedBlock(NewBlock.StartIndex, NewBlock.Length);
                        TreeNode TempNode = NewBlock.GetTreeNode();
                        if (TempNode != null)
                            ChildTreeNodes.Add(TempNode);
                        //                        ChildElements.Add(NewBlock);
                        NoBlocksToProcess = false;
                    }
                }
            }
            //Проверка строк
            int LocalIndex = 0;
            for (int I = StartIndex; I < EndIndex; I++)
            {
                foreach (StringType oStringType in this.StructureType.ChildStringTypes)
                {
                    using (StringValue NewString = new StringValue(oStringType, Source[I]))
                    {
                        if (NewString.ConditionCorrect)
                        {
                            NewString.StartIndex = AddProcessedBlock(0, 1);
                            TreeNode NewTreeNode = NewString.GetTreeNode();
                            ChildTreeNodes.Add(NewTreeNode);
                            LocalIndex++;
                            if (LocalIndex >= 10000)
                            {
                                LocalIndex = 0;
                                GC.Collect();
                                
                            }
/*                            if (ChildTreeNodes.Count >= 10000)
                            {
                                var sFileName = Application.CommonAppDataPath + "\\" + this.GetHashCode() + "\\tree" + NewTreeNode.GetHashCode() + ".tmp";
                                var fDirectory = System.IO.Directory.GetParent(sFileName);
                                if (!fDirectory.Exists)
                                {
                                    fDirectory.Create();
                                }
                                Common.TreeViewSerializer.SerializeTreeNodeList(ChildTreeNodes, sFileName);
                                foreach(TreeNode ChildNode in ChildTreeNodes)
                                    ChildNode.Remove();
                                ChildTreeNodes.Clear();
                                ChildTreeNodes.TrimExcess();
                                ChildTreeNodes = new List<TreeNode>();
                                GC.Collect();
                            }
 */
                            //ChildStrings.Add(NewString);
                            UniversalLogViewer.Program.MainForm.IncreaseProgressLevel(1);
                            break;
                        }
                    }
                }
            }
            //            Source = new List<string>();
            ProcessedStrings.Clear();
            ProcessedStrings.TrimExcess();
        }
    }
}

