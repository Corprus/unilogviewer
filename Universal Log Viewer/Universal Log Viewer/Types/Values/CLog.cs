using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Universal_Log_Viewer.Types.Structures;
using Universal_Log_Viewer.Common;

namespace Universal_Log_Viewer.Types.Values
{
    class CLog
    {
        public CLogType Type { get; private set; }
        FileReader LogFile { get; set; }
        CBlock RootBlock { get; set; }
        public TreeNode TreeNode
        {
            get
            {                
                TreeNode Result = RootBlock.TreeNode;
                Result.BeginEdit();
                Result.Text = this.Type.LogName + ":" + Result.Text;
                Result.EndEdit(false);
                return Result;
            }
        }


        public CLog(CLogType Type, string FileName)
        {
            this.Type = Type;
            LogFile = new FileReader(FileName);
            RootBlock = new CBlock(this.Type.RootBlockType, LogFile.ReadFile());
        }
    }
}
