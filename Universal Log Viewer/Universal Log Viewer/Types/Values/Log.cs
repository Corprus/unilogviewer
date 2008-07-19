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
                UniversalLogViewer.Program.MainForm.InitProgressLevel(LogFile.ReadFile().Length, 0, "Generating Tree...");
                TreeNode Result = RootBlock.TreeNode;
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
            string[] LogFileStrings = LogFile.ReadFile();
            UniversalLogViewer.Program.MainForm.InitProgressLevel(LogFileStrings.Length, 0, "Processing Log...");
            RootBlock = new BlockValue(this.StructureType.RootBlockType, LogFileStrings);
            UniversalLogViewer.Program.MainForm.EndProgress();
        }
    }
}
