using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
[assembly: CLSCompliant(true)]

namespace UniversalLogViewer.Common
{
    public class UICommon
    {
        public static TreeNode GetNextNodeInList(TreeNode CurrentNode)
        {
            if (CurrentNode.Nodes.Count > 0)
                return CurrentNode.Nodes[0];
            if (CurrentNode.NextNode != null)
                return CurrentNode.NextNode;
            TreeNode ParentNode = CurrentNode.Parent;
            TreeNode NextNode = null;
            while (NextNode == null)
                if (ParentNode != null)
                {
                    NextNode = ParentNode.NextNode;
                    ParentNode = ParentNode.Parent;
                }
                else
                    return null;
            return null;
        }


        public static bool SearchWithinNodes(LogLoadProgress logProgress, TreeView SearchTreeView, TreeNode StartingNode, string Text)
        {
            if (logProgress.GetProgressLevel() == 100)
                return false;
            bool Result = StartingNode.Text.Contains(Text);
            if (Result)
                SelectTreeNode(SearchTreeView, StartingNode);

            if (!Result)
            {
                foreach (TreeNode Node in StartingNode.Nodes)
                {
                    logProgress.IncreaseProgressLevel(1);
                    if (Node.Nodes.Count > 0)
                        Result = SearchWithinNodes(logProgress, SearchTreeView, Node, Text);
                    else
                    {
                        Result = Node.Text.Contains(Text);
                        if (Result)
                            SelectTreeNode(SearchTreeView, Node);
                    }

                    if (Result)
                        return Result;
                }
            }

            if (!Result)
            {
                TreeNode NextNode = StartingNode.NextNode;
                if (NextNode == null)
                    if (StartingNode.Parent != null)
                        NextNode = StartingNode.Parent.NextNode;

                if (NextNode == null)
                    return false;
                else
                    return SearchWithinNodes(logProgress, SearchTreeView, NextNode, Text);
            }
            return Result;
        }
        delegate void SelectTreeNodeCallback(TreeView Tree, TreeNode Node);
        public static void SelectTreeNode(TreeView Tree, TreeNode Node)
        {
            if (Tree.InvokeRequired)
            {
                SelectTreeNodeCallback d = new SelectTreeNodeCallback(SelectTreeNode);
                Tree.Invoke(d, new object[] { Tree, Node });
            }
            else
            {
                Tree.SelectedNode = Node;
                //                tabLogs.SelectedIndex = tabLogs.TabPages.Count - 1;
            }
        }
    }

    [CLSCompliant(true)]
    public class LogLoadProgress
    {
        public LogLoadProgress(ProgressBar progressBar, Label progressLabel)
        {
            _lblProgress = progressLabel;
            _prbProgress = progressBar;
        }
        private Label _lblProgress;
        private ProgressBar _prbProgress;
        private void SetFullProgressLevel(int current, int max, int min)
        {
            SetFullProgressLevel(_prbProgress, _lblProgress, current, max, min);
        }
        delegate void SetFullProgressLevelCallback(ProgressBar progressBar, Label progressLabel, int current, int max, int min);
        private void SetFullProgressLevel(ProgressBar progressBar, Label progressLabel, int current, int max, int min)
        {
            if (!progressBar.InvokeRequired)
            {
            progressBar.Maximum = max;
            progressBar.Minimum = min;
            if (current > max)
                current = max;
            progressBar.Value = current;
            if (!((current == min) || (current == 0)))
                progressLabel.Text = ((int)((100 * current) / (max - min))).ToString() + "%";
            else
                progressLabel.Text = "0%";
            }
            else
            {

                if ((progressBar.IsDisposed || progressLabel.IsDisposed))
                    return;
                else
                {
                    SetFullProgressLevelCallback d = new SetFullProgressLevelCallback(SetFullProgressLevel);
                    progressBar.Invoke(d, new object[] { progressBar, progressLabel, current, max, min });
                }
            }

        }

        
        public void SetProgressLevel(int current)
        {
            SetFullProgressLevel(current, _prbProgress.Maximum, _prbProgress.Minimum);
        }
        delegate void IncreaseProgressLevelCallback(int byvalue);
        public void IncreaseProgressLevel(int byvalue)
        {
            SetProgressLevel(_prbProgress.Value + byvalue);
        }
        delegate int GetProgressLevelCallback();
        public int GetProgressLevel()
        {
            if (!_prbProgress.InvokeRequired)
            {
                return (int)(100 * _prbProgress.Value / (_prbProgress.Maximum - _prbProgress.Minimum));
            }
            else
            {
                GetProgressLevelCallback d = new GetProgressLevelCallback(GetProgressLevel);
                return (int)_prbProgress.Invoke(d, new object[] { });
            }              
        }
        public void InitProgressLevel(int max, int min)
        {
            SetFullProgressLevel(min, max, min);
        }
    }
}
