using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UniversalLogViewer.Types.Structures;
using UniversalLogViewer.Common.Types.Managers;


namespace UniversalLogViewer.Types.Values
{
    
    public class StringValue : BaseStringValueCollection<Value>
    {
        private new StringType StructureType
        {
            get { return (base.StructureType as StringType); }
            set { base.StructureType = value; }
        }

        public bool ConditionCorrect { get { return StructureType.Condition.IsCorrect(Value); } }
        public int StartIndex { get; set; }
        public override TreeNode TreeNode
        {
            get
            {

                string nodeTitle;
                switch (StructureType.TitleType)
                {
                    case Common.TitleType.Title: nodeTitle = StructureType.Title; break;
                    case Common.TitleType.Source: nodeTitle = Source; break;
                    case Common.TitleType.Value:
                        {
                            Value titleElement = null;
                            var titleValueIndex = StructureType.TitleValueIndex;
                            var titleValueType = StructureType.TitleValueType;
                            if (ChildElements.Count > titleValueIndex)
                            {
                                var childElementValue = ChildElements[titleValueIndex];
                                if (!((titleValueType.Length > 0) && 
                                   (childElementValue.StructureType.Name != titleValueType)))
                                    titleElement = childElementValue;
                            }
                            nodeTitle = (titleElement == null) ? StructureType.Title : titleElement.Value;
                            break;
                        }
                    default: nodeTitle = StructureType.Title; break;
                }
                TreeNodeValueString = nodeTitle;
                var result = base.TreeNode;
                if ((!IniSettingsManager.ShowValueMemo))
                {
                    for (var i = ChildElements.Count - 1; i >= 0; i--)
                    {
                        var resultValue = ChildElements[i];
                        if (resultValue.StructureType.Style.Visible)
                        {
                            result.Nodes.Insert(0, resultValue.TreeNode);
                        }
                        ChildElements.RemoveAt(i);
                    }
                }
                ChildElements.Clear();
                Program.MainForm.LogProgress.IncreaseProgressLevel(1);
                return result;
            }
        }
    

        void ProcessString()
        {
            var bStringProcessed = false;
            string[] processedString = {Source};
            var values = ChildElements ?? new List<Value>();
            values.Clear();
            
            if (StructureType.UseSeparator)
            {
                var separatedStrings = processedString[0].Split(StructureType.Separator);
                var types = StructureType.ChildTypes.ToArray();
                /* “ут не до конца пон€тно как минимум брать и что делать с отсе€нными пол€ми,
                 * ну да и хрен с ними */
                for (var i = 0; i < separatedStrings.Length; i++)
                {
                    var typesLength = types.Length;
                    ValuesType usedType = (i >= typesLength) ? types[typesLength - 1] : types[i];
                    var newValue = new Value(usedType,ref separatedStrings[i]);
                    if (newValue.Value.Length != 0)
                        values.Add(newValue);
                }
            }
            else
            /* ≈сли опци€ Ќ≈ использовать сепаратор, тогда начинаем веселитьс€,
             * провер€€ на соответствие и приставива€ соотв. пол€. “о есть автоматическое 
             * распарсивание по услови€м   */
                while (!(bStringProcessed))
                {
                    bStringProcessed = true;
                    foreach (
                        var newValue in
                            StructureType.ChildTypes.Select(type => new Value(type, ref processedString[0]))
                                         .Where(newValue => newValue.Value.Length != 0))
                    {
                        values.Add(newValue);
                        processedString[0] = newValue.CutSource(processedString[0]);
                    }
                }
        }
        public override void Parse()
        {
            ProcessString();
        }

        public StringValue(StringType stringType, ref string source)
            : base(stringType, ref source)
        {
            Value = source;
            StructureType = stringType;
        }

        public string[] GetValues()
        {
            var result = new List<string> {string.Format("Contents of {0}", StructureType.Title)};
            result.AddRange(
                ChildElements.Select(
                    childValue => string.Format("{0}: {1}", childValue.StructureType.Name, childValue.Value)));
            return result.ToArray();
        }
    }
}
