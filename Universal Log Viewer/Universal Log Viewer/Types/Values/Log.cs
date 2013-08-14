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
                Program.MainForm.InitProgressLevel(LogFile.ReadFile().Length, 0, "Generating Tree...");
                TreeNode result = RootBlock.TreeNode;
                result.Text = string.Format("{0}:{1}", StructureType.LogName, result.Text);
                Program.MainForm.EndProgress();
                return result;
            }
        }


        public Log(LogType type, string fileName)
        {
            StructureType = type;
            LogFile = new FileReader(fileName);
            string[] logFileStrings = LogFile.ReadFile();
            Program.MainForm.InitProgressLevel(logFileStrings.Length, 0, "Processing Log...");
            RootBlock = new BlockValue(StructureType.RootBlockType,ref logFileStrings);
            Program.MainForm.EndProgress();
        }
    }
}
