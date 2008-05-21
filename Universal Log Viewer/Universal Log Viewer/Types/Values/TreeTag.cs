using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversalLogViewer.Types.Values
{
    class TreeTag
    {
        public string TypeName { get; private set; }
        public BaseValue Obj { get; set; }
        public int StartIndex { get; set; }
        public TreeTag(BaseValue Obj)
        {
            TypeName = Obj.GetType().Name;
            if (Obj is StringValue)
                StartIndex = ((StringValue)Obj).StartIndex;
            else if (Obj is BlockValue)
                StartIndex = ((BlockValue)Obj).StartIndex;
            else
                StartIndex = -1;
        }
    }
}
