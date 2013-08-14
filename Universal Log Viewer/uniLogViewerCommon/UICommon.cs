using System;
using System.Windows.Forms;
[assembly: CLSCompliant(true)]

namespace UniversalLogViewer.Common
{
    public class UICommon
    {
        public static TreeNode GetNextNodeInList(TreeNode currentNode)
        {
            if (currentNode.Nodes.Count > 0)
                return currentNode.Nodes[0];
            if (currentNode.NextNode != null)
                return currentNode.NextNode;
            TreeNode parentNode = currentNode.Parent;
            TreeNode nextNode = null;
            while (nextNode == null)
                if (parentNode != null)
                {
                    nextNode = parentNode.NextNode;
                    parentNode = parentNode.Parent;
                }
                else
                    return null;
            return null;
        }


        public static bool SearchWithinNodes(LogLoadProgress logProgress, TreeView searchTreeView, TreeNode startingNode, string text)
        {
            if (logProgress.GetProgressLevel() == 100)
                return false;
            var result = startingNode.Text.Contains(text);
            if (result)
                SelectTreeNode(searchTreeView, startingNode);

            if (!result)
            {
                foreach (TreeNode node in startingNode.Nodes)
                {
                    logProgress.IncreaseProgressLevel(1);
                    if (node.Nodes.Count > 0)
                        result = SearchWithinNodes(logProgress, searchTreeView, node, text);
                    else
                    {
                        result = node.Text.Contains(text);
                        if (result)
                            SelectTreeNode(searchTreeView, node);
                    }

                    if (result)
                        return true;
                }
            }

            if (!result)
            {
                var nextNode = startingNode.NextNode;
                if (nextNode == null && startingNode.Parent != null) nextNode = startingNode.Parent.NextNode;

                return nextNode != null && SearchWithinNodes(logProgress, searchTreeView, nextNode, text);
            }
            return true;
        }
        delegate void SelectTreeNodeCallback(TreeView tree, TreeNode node);

        public static void SelectTreeNode(TreeView tree, TreeNode node)
        {
            if (tree.InvokeRequired)
            {
                var d = new SelectTreeNodeCallback(SelectTreeNode);
                tree.Invoke(d, new object[] { tree, node });
            }
            else
            {
                tree.SelectedNode = node;
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
        private readonly Label _lblProgress;
        private readonly ProgressBar _prbProgress;
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
                progressLabel.Text = !((current == min) || (current == 0))
                                         ? string.Format("{0:%}", (current)/(max - min))
                                         : "0%";
            }
            else
            {
                if ((progressBar.IsDisposed || progressLabel.IsDisposed))
                    return;
                var d = new SetFullProgressLevelCallback(SetFullProgressLevel);
                progressBar.Invoke(d, new object[] { progressBar, progressLabel, current, max, min });
            }

        }

        
        public void SetProgressLevel(int current)
        {
            SetFullProgressLevel(current, _prbProgress.Maximum, _prbProgress.Minimum);
        }

        public void IncreaseProgressLevel(int byvalue)
        {
            SetProgressLevel(_prbProgress.Value + byvalue);
        }
        delegate int GetProgressLevelCallback();
        public int GetProgressLevel()
        {
            if (!_prbProgress.InvokeRequired)
            {
                return 100 * _prbProgress.Value / (_prbProgress.Maximum - _prbProgress.Minimum);
            }
            var d = new GetProgressLevelCallback(GetProgressLevel);
            return (int)_prbProgress.Invoke(d, new object[] { });
        }
        public void InitProgressLevel(int max, int min)
        {
            SetFullProgressLevel(min, max, min);
        }
    }
}
