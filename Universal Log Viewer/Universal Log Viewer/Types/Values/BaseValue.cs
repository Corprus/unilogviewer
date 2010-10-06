using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniversalLogViewer.Types.Structures;

namespace UniversalLogViewer.Types.Values
{
    public abstract class BaseValue
    {
        protected string TreeNodeValueString { get; set; }
        public BaseType StructureType { get; protected set; }
        public virtual TreeNode TreeNode 
        { 
            get
            {
                TreeNode Result = null;
                if (this.StructureType.Style.Visible)
                {
                    string RealValue = TreeNodeValueString;
                    if (this.StructureType.Style.Trim)
                        RealValue = RealValue.Trim();
                    Result = new TreeNode(TreeNodeValueString);
                    Result.BeginEdit();
                    Result.Tag = this;
                    Result.ForeColor = this.StructureType.Style.Color;
                    Result.BackColor = this.StructureType.Style.Background;
                    if (Result.NodeFont == null)
                        Result.NodeFont = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 8);
                    if (this.StructureType.Style.Bold)
                        Result.NodeFont = new System.Drawing.Font(Result.NodeFont, System.Drawing.FontStyle.Bold);
                    if (this.StructureType.Style.Italic)
                        Result.NodeFont = new System.Drawing.Font(Result.NodeFont, System.Drawing.FontStyle.Italic);
                    if (this.StructureType.Style.Underline)
                        Result.NodeFont = new System.Drawing.Font(Result.NodeFont, System.Drawing.FontStyle.Underline);
                    if (this.StructureType.Style.Strike)
                        Result.NodeFont = new System.Drawing.Font(Result.NodeFont, System.Drawing.FontStyle.Strikeout);
                    Result.EndEdit(false);

                }
                return Result;

            } 
        }

        public abstract void Parse();
        protected BaseValue(ref BaseType Type)
        {
            this.StructureType = Type;
        }
    }
    public abstract class BaseStringValueCollection<T> : BaseValue, IEnumerable<T>
        where T : BaseStringValueCollection<T>
    {
        public string Value { get; protected set; }
        protected string Source { get; private set; }
        protected List<T> ChildElements { get; private set; }
        protected BaseStringValueCollection(BaseType Type,ref string Source)
            : base(ref Type)
        {
            this.Source = Source;
            Value = "";
            ChildElements = new List<T>();
            Parse();
        }
        public IEnumerator<T> GetEnumerator()
        {
            foreach (T Child in ChildElements)
                yield return Child;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
        public abstract class BaseStringsValueCollection<T> : BaseValue,  IEnumerable<T>
        where T : BaseStringsValueCollection<T>
    {
        public string[] Value { get; protected set; }
        protected string[] Source { get; private set; }
        protected List<T> ChildElements { get; private set; }
        protected BaseStringsValueCollection(BaseType Type,ref string[] Source)
            :base(ref Type)
        {
            this.Source = Source;
            ChildElements = new List<T>();
            Parse();
        }
        public IEnumerator<T> GetEnumerator()
        {
            foreach (T Child in ChildElements)
                yield return Child;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }        
        }


}
