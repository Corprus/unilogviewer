using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UniversalLogViewer.Types.Structures;

namespace UniversalLogViewer.Types.Values
{
    

    public class BlockValue : BaseStringsValueCollection<BlockValue>
    {
        private new BlockType StructureType { get { return base.StructureType as BlockType; }}
        private bool IsEmpty { get { return (Value.Length == 0); } }
        private int StartIndex { get; set; }
        //bool[] ProcessedStrings;
        private List<StringValue> ChildStrings { get; set; }
        private string[] CutSourceString { get; set; }
        static bool IsBlock(Object obj) { return (obj is BlockValue); }
        static bool IsString(Object obj) { return (obj is StringValue); }
        static bool IsStringOrBlock(Object obj) { return ((IsString(obj)) || (IsBlock(obj))); }

        private static int CompareChilds(Object a, Object b)
        {
            if ((!IsStringOrBlock(a)) || (!IsStringOrBlock(b)))
                return 0;
            int startIndexA = IsString(a) ? ((StringValue) a).StartIndex : ((BlockValue) a).StartIndex;
            int startIndexB = IsString(b) ? ((StringValue) b).StartIndex : ((BlockValue) b).StartIndex;
            return (startIndexA - startIndexB);
        }
        public override TreeNode TreeNode
        {
            get
            {
                TreeNodeValueString = StructureType.Title;
                TreeNode result = base.TreeNode;
                var childList = ChildStrings.Cast<object>().ToList();
                childList.AddRange(ChildElements);
                childList.Sort(CompareChilds);
                for (var i = childList.Count - 1; i >= 0; i--)
                    //foreach (Object Obj in ChildList)
                {
                    var typeObj = childList[i] as BaseValue;
                    if ((typeObj != null) && ((typeObj is BlockValue) || (typeObj is StringValue)) &&
                        typeObj.StructureType.Style.Visible) result.Nodes.Insert(0, typeObj.TreeNode);
                    childList.RemoveAt(i);
                }
                childList.Clear();

                return result;
            }
        }
        string[] ParseStringList(string[] sourceList)
        {
            var bStartConditionDone = false;
            var bEndConditionDone = false;
            var result = new List<string>();
            var cutSourceList = new List<string>();
            ConditionType startCondition = StructureType.StartCondition;
            ConditionType endCondition = StructureType.EndCondition;

            for(int i = 0; i < sourceList.Length; i++)
            {
                string curString = sourceList[i];
                if (!bStartConditionDone)
                {
                    bStartConditionDone = (startCondition == null) || startCondition.IsCorrect(curString);

                    if (bStartConditionDone)
                        StartIndex = i;
                }

                if ((!bStartConditionDone) || (bEndConditionDone))
                    cutSourceList.Add(curString);
                if ((!bEndConditionDone) && (bStartConditionDone))
                {
                    result.Add(curString);
                    bEndConditionDone = (endCondition != null) && endCondition.IsCorrect(curString);
                }

            }
            CutSourceString = cutSourceList.ToArray();
            return result.ToArray();
        }
        public override void Parse()
        {
            ChildStrings = new List<StringValue>();
            Value = ParseStringList(Source);
            ProcessStringList();
        }
        int AddProcessedBlock(int startIndex, int length, ref List<int> processedStrings)
        {
            int result = 0;
            int processedCounter = 0;            
            bool bStarted = false;
            int stringsCount = processedStrings.Count;
            for (int i = 0; i < stringsCount; i++)
            {
                if (processedStrings.Contains(i)) continue;
                if (!bStarted)
                    if (processedCounter == startIndex)
                    {
                        result = i;
                        bStarted = true;
                        processedCounter = 0;
                        processedStrings.Remove(i);
                    }
                    else
                        processedCounter++;
                else
                {
                    if (processedCounter == (length - 1))
                        return result;
                    processedStrings.Remove(i);
                    processedCounter++;
                }
            }
            return stringsCount;
        }
        public BlockValue(BlockType type, ref string[] source)
            : base(type, ref source)
        {
            
        }

        public void ProcessStringList()
        {
            //bool[] ProcessedStrings = new bool[Source.Length];
            var processedStrings = new List<int>(Source.Length);
            for (int i = 0; i < processedStrings.Count; i++)
                processedStrings[i] = i;

            string[] processedList = Value;
            //Проверка блоков и генерация дочерних блоков (и их процесинг)
            foreach (BlockType blockType in StructureType.ChildBlockTypes)
            {
                var noBlocksToProcess = false;
                while (!(noBlocksToProcess))
                {

                    noBlocksToProcess = true;
                    var newBlock = new BlockValue(blockType,ref processedList);
                    if (newBlock.IsEmpty) continue;
                    processedList = newBlock.CutSourceString;
                    newBlock.StartIndex = AddProcessedBlock(newBlock.StartIndex, newBlock.Value.Length,ref processedStrings);
                    ChildElements.Add(newBlock);
                    noBlocksToProcess = false;
                }
            }
            //Проверка строк
            for (int I = 0; I < processedList.Length; I++)
            {
                foreach (var oStringType in StructureType.ChildStringTypes)
                {
                    using (var newString = new StringValue(oStringType, ref processedList[I]))
                    {
                        if (!newString.ConditionCorrect) continue;
                        newString.StartIndex = AddProcessedBlock(0, 1, ref processedStrings);
                        ChildStrings.Add(newString);
                        Program.MainForm.LogProgress.IncreaseProgressLevel(1);
                        break;
                    }
                }
            }
        }
    }
}

