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
        public List<string> File { get; private set; }
        BlockValue RootBlock { get; set; }
        public TreeNode TreeNode
        {
            get
            {
                UniversalLogViewer.Program.MainForm.InitProgressLevel(File.Count, 0, "Generating Tree...");
                TreeNode Result = RootBlock.GetTreeNode();
                Result.BeginEdit();
                Result.Text = this.StructureType.LogName + ":" + Result.Text;
                Result.EndEdit(false);
                UniversalLogViewer.Program.MainForm.EndProgress();
                return Result;
            }
        }


        public Log(LogType Type, string FileName)
        {
            this.StructureType = Type;
            LogFile = new FileReader(FileName);
            File = LogFile.ReadFile();
            UniversalLogViewer.Program.MainForm.InitProgressLevel(File.Count, 0, "Processing Log...");
            RootBlock = new BlockValue(this.StructureType.RootBlockType, File);
            UniversalLogViewer.Program.MainForm.EndProgress();
        }
    }
}
