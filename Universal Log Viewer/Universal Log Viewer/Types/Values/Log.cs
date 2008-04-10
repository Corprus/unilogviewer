using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using UniversalLogViewer.Types.Structures;
using UniversalLogViewer.Common;

namespace UniversalLogViewer.Types.Values
{
    class Log
    {
        public LogType StructureType { get; private set; }
        FileReader LogFile { get; set; }
        BlockValue RootBlock { get; set; }
        public TreeNode TreeNode
        {
            get
            {                
                TreeNode Result = RootBlock.TreeNode;
                Result.BeginEdit();
                Result.Text = this.StructureType.LogName + ":" + Result.Text;
                Result.EndEdit(false);
                return Result;
            }
        }


        public Log(LogType Type, string FileName)
        {
            this.StructureType = Type;
            LogFile = new FileReader(FileName);
            RootBlock = new BlockValue(this.StructureType.RootBlockType, LogFile.ReadFile());
        }
    }
}
