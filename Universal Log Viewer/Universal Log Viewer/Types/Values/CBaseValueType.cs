using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Universal_Log_Viewer.Types.Structures;

namespace Universal_Log_Viewer.Types.Values
{
    public abstract class CBaseStringValueType<T> : IEnumerable<T>
        where T : CBaseStringValueType<T>
    {
        protected CBaseType Type { get; set; }
        public string Value { get; protected set; }
        public TreeNode TreeNode { get { return GetTreeNode(); } }
        protected abstract TreeNode GetTreeNode();
        protected string Source;
        public abstract void Parse();
        protected List<T> ChildElements { get; set; }
        public CBaseStringValueType(CBaseType Type, string Source)
        {
            this.Type = Type;
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
        public abstract class CBaseStringsValueType<T> : IEnumerable<T>
        where T : CBaseStringsValueType<T>
    {
        protected CBaseType Type { get; set; }
        public string[] Value { get; protected set; }
        public TreeNode TreeNode { get { return GetTreeNode(); } }
        protected abstract TreeNode GetTreeNode();
        protected string[] Source;
        public abstract void Parse();
        protected List<T> ChildElements { get; set; }
        public CBaseStringsValueType(CBaseType Type, string[] Source)
        {
            this.Type = Type;
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
