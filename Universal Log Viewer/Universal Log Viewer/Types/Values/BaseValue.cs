using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UniversalLogViewer.Types.Structures;

namespace UniversalLogViewer.Types.Values
{
    public abstract class BaseValue : IDisposable
    {
        protected string TreeNodeValueString { get; set; }
        public BaseType StructureType { get; protected set; }
        public virtual TreeNode TreeNode 
        { 
            get
            {
                TreeNode result = null;
                if (StructureType.Style.Visible)
                {
                    result = new TreeNode(StructureType.Style.Trim ? TreeNodeValueString.Trim() : TreeNodeValueString)
                        {
                            Tag = this,
                            ForeColor = StructureType.Style.Color,
                            BackColor = StructureType.Style.Background,
                            NodeFont = StructureType.Style.Font,
                        };
                }
                return result;

            } 
        }

        public abstract void Parse();
        protected BaseValue(ref BaseType type)
        {
            StructureType = type;
        }

        void IDisposable.Dispose()
        {
            
        }
    }

    public abstract class BaseStringValueCollection<T> : BaseValue, IEnumerable<T>
        where T : BaseStringValueCollection<T>
    {
        public string Value { get; protected set; }
        protected string Source { get; private set; }
        protected List<T> ChildElements { get; private set; }
        protected BaseStringValueCollection(BaseType type, ref string source)
            : base(ref type)
        {
            Source = source;
            Value = "";
            ChildElements = new List<T>();
            Parse();
        }
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>) ChildElements).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
        public abstract class BaseStringsValueCollection<T> : BaseValue,  IEnumerable<T>
        where T : BaseStringsValueCollection<T>
    {
        public string[] Value { get; protected set; }
        protected string[] Source { get; private set; }
        protected List<T> ChildElements { get; private set; }
        protected BaseStringsValueCollection(BaseType type,ref string[] source)
            :base(ref type)
        {
            Source = source;
            ChildElements = new List<T>();
            Parse();
        }
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>) ChildElements).GetEnumerator();
        }

            IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }        
        }


}
