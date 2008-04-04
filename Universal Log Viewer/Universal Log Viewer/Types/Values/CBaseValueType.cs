using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Universal_Log_Viewer.Types.Structures;

namespace Universal_Log_Viewer.Types.Values
{
    public abstract class CBaseValueType
    {
        protected string _TreeNodeValueString;
        protected CBaseType Type { get; set; }
        public TreeNode TreeNode { get { return GetTreeNode(); } }
        protected virtual TreeNode GetTreeNode()
        {
            TreeNode Result = null;
            if (this.Type.Style.Visible)
            {
                string RealValue = _TreeNodeValueString;
                if (this.Type.Style.Trim)
                    RealValue = RealValue.Trim();
                Result = new TreeNode(_TreeNodeValueString);
                Result.BeginEdit();
                Result.Tag = this;
                Result.ForeColor = this.Type.Style.Color;
                if (Result.NodeFont == null)
                    Result.NodeFont = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 8);
                if (this.Type.Style.Bold)
                    Result.NodeFont = new System.Drawing.Font(Result.NodeFont, System.Drawing.FontStyle.Bold);
                Result.EndEdit(false);

            }
            return Result;
        }
        public abstract void Parse();
        protected CBaseValueType(CBaseType Type)
        {
            this.Type = Type;
        }
    }
    public abstract class CBaseStringValueType<T> : CBaseValueType, IEnumerable<T>
        where T : CBaseStringValueType<T>
    {
        public string Value { get; protected set; }
        protected string Source;
        protected List<T> ChildElements { get; set; }
        public CBaseStringValueType(CBaseType Type, string Source)
            : base(Type)
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
        public abstract class CBaseStringsValueType<T> : CBaseValueType,  IEnumerable<T>
        where T : CBaseStringsValueType<T>
    {
        public string[] Value { get; protected set; }
        protected string[] Source;
        protected List<T> ChildElements { get; set; }
        public CBaseStringsValueType(CBaseType Type, string[] Source)
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
