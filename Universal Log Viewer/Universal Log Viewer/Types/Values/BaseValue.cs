using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniversalLogViewer.Types.Structures;

namespace UniversalLogViewer.Types.Values
{
    public abstract class BaseValue : IDisposable
    {
        bool disposed = false;
        ~BaseValue()
        {
            if (!disposed)
            {
                disposed = true;
                Dispose(false);
            }
        }
        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //managed
            }
            //unmanaged        
        }
        
        protected string TreeNodeValueString { get; set; }
        public BaseType StructureType { get; protected set; }
        public virtual TreeNode GetTreeNode()
        {
            TreeNode Result = null;
            if (this.StructureType.Style.Visible)
            {
                string RealValue = TreeNodeValueString;
                if (this.StructureType.Style.Trim)
                    RealValue = RealValue.Trim();
                Result = new TreeNode(TreeNodeValueString);
                Result.BeginEdit();
                Result.Tag = new TreeTag(this);
                Result.ForeColor = this.StructureType.Style.Color;
                Result.BackColor = this.StructureType.Style.Background;
                Result.NodeFont = Common.Consts.GetFontFromSettings(this.StructureType.Style.Bold, this.StructureType.Style.Italic, this.StructureType.Style.Underline, this.StructureType.Style.Strike);
                Result.EndEdit(false);
            }
            return Result;

        }

        public abstract void Parse();
        protected BaseValue(BaseType Type)
        {
            this.StructureType = Type;
        }
    }
    public abstract class BaseStringValueCollection<T> : BaseValue, IEnumerable<T>
        where T : BaseStringValueCollection<T>
    {
        public string Value { get; protected set; }
        protected string Source { get; private set; }
        protected List<T> ChildElements { get; set; }
        protected BaseStringValueCollection(BaseType Type, string Source)
            : base(Type)
        {
            this.Source = Source;
            Value = "";
            ChildElements = new List<T>();
            Parse();
        }
        protected override void Dispose(bool disposing)
        {
            foreach (T Element in ChildElements)
                Element.Dispose();
            ChildElements.Clear();
            ChildElements.TrimExcess();
            base.Dispose(disposing);
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
        protected List<string> Source { get; set; }
        protected List<T> ChildElements { get; private set; }
        protected BaseStringsValueCollection(BaseType Type, List<string> Source)
            :base(Type)
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
