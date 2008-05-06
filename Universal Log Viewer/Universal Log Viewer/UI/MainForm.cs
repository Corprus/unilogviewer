using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniversalLogViewer.Types.Values;
using UniversalLogViewer.Types.Structures;
using UniversalLogViewer.Common.Types.Managers;
using UniversalLogViewer.Types.Managers;
using UniversalLogViewer.Common;

namespace UniversalLogViewer.UI
{
    public partial class MainForm : Form
    {
        public TreeView SelectedTreeView
        {
            get         
            {
                return GetSelectedTreeView();
            }
        }
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnLoadLogTypes_Click(object sender, EventArgs e)
        {
            LogTypesManagerForm fmLoadLogType = new LogTypesManagerForm();
            fmLoadLogType.ShowDialog(this);
            var CurrentLogType = cmbLogTypes.SelectedItem as LogType;
            LogTypeManager.oInstance.UpdateList(cmbLogTypes.Items);
            if (CurrentLogType != null)
                foreach (var CurrentItem in cmbLogTypes.Items)
                {
                    var NewLogType = CurrentItem as LogType;
                    if ((NewLogType != null) && (NewLogType.LogName == CurrentLogType.LogName))
                        cmbLogTypes.SelectedItem = NewLogType;
                }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout frmAbout = new FormAbout();
            frmAbout.ShowDialog();
        }

        public struct LoadLogParametersData
        {
            public LogType LoadedLogType;
            public string LogFileName;

        }
        private void LoadLog(object o)
        {

            LoadLogParametersData Data = (LoadLogParametersData)o;
            Log oLog = new Log(Data.LoadedLogType, Data.LogFileName);
            TabPage LogTab = new TabPage();
            LogTab.Text = Data.LogFileName + " (" + oLog.StructureType.LogName + ")";
            TreeView LogTreeView = new TreeView();
            LogTreeView.Dock = DockStyle.Fill;
            LogTreeView.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TreeView_KeyPress);
            LogTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LogTreeViewSelectedItemChanged);

            LogTab.Controls.Add(LogTreeView);
            LogTab.ContextMenuStrip = cntTabPopup;
            LogTreeView.Nodes.Add(oLog.TreeNode);
            AddLogTabAndSelect(LogTab);
        }
        delegate void AddLogTabAndSelectCallback(TabPage LogTab);
        private void AddLogTabAndSelect(TabPage LogTab)
        {
            if (this.tabLogs.InvokeRequired)
            {
                AddLogTabAndSelectCallback d = new AddLogTabAndSelectCallback(AddLogTabAndSelect);
                this.Invoke(d, new object[] { LogTab });
            }
            else
            {
                tabLogs.TabPages.Add(LogTab);
                tabLogs.SelectedIndex = tabLogs.TabPages.Count - 1;
                HideShowSearch();

            }
        }

        private void btnLoadLog_Click(object sender, EventArgs e)
        {
            if (dlgOpenLog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string sLogFileName = dlgOpenLog.FileName;
                if (LogTypeManager.oInstance.TypesList.Count > 0)
                {
                    if ((cmbLogTypes.SelectedIndex != -1)&&(cmbLogTypes.Items[cmbLogTypes.SelectedIndex] is LogType))
                    {
                        LoadLogParametersData LoadLogData = new LoadLogParametersData();
                        System.Threading.Thread LoadLogThread = new System.Threading.Thread(LoadLog);
                        LoadLogData.LoadedLogType = (cmbLogTypes.Items[cmbLogTypes.SelectedIndex] as LogType);
                        LoadLogData.LogFileName = sLogFileName;
                        LoadLogThread.Start(LoadLogData);
                    }
                    else
                    {
                        MessageBox.Show(Consts.SELECT_CORRECT_LOG_TYPE, Consts.SELECT_CORRECT_LOG_TYPE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, Consts.DEFAULT_MESSAGE_BOX_OPTIONS);
                    }
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LogTypeManager.oInstance.UpdateList(cmbLogTypes.Items);
            HideShowSearch();
        }

        private void closeTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabLogs.TabPages.Remove(tabLogs.SelectedTab);
            HideShowSearch();
        }

        private void TreeView_KeyPress(object sender, KeyPressEventArgs e)
        {
            var Tree = sender as TreeView;
            if (sender == null)
                return;

            if (tabLogs.SelectedIndex > -1)
                if (e.KeyChar == 3)
                    Clipboard.SetText(Tree.SelectedNode.Text);         
        }
        private void LogTreeViewSelectedItemChanged(object sender, EventArgs e)
        {
            TreeView Tree = (sender as TreeView);
            if ((IniSettingsManager.ShowValueMemo) && (Tree != null))
            {
                var StringTag = Tree.SelectedNode.Tag as StringValue;
                if (StringTag != null)
                {
                    memoValue.Visible = true;
                    memoValue.Lines = StringTag.GetValues();
                }
                else
                    memoValue.Visible = false;
            }
        }

        private void mnuHelpFile_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, hlpUniLogViewer.HelpNamespace);
        }

        private void mnuHelpIndex_Click(object sender, EventArgs e)
        {
            Help.ShowHelpIndex(this, hlpUniLogViewer.HelpNamespace);
        }
        delegate TreeView GetSelectedTreeViewCallback();
        private TreeView GetSelectedTreeView()
        {
            if (tabLogs.InvokeRequired)
            {
                GetSelectedTreeViewCallback d = new GetSelectedTreeViewCallback(GetSelectedTreeView);
                return (TreeView) this.Invoke(d, new object[] {});
            }
            else
            {
                var SelTab = tabLogs.SelectedTab;
                foreach (Control SelectedControl in SelTab.Controls)
                {
                    TreeView SelectedTreeView = SelectedControl as TreeView;
                    if (SelectedTreeView != null)
                        return SelectedTreeView;
                }
                return null;
            }
        }
        delegate void SelectTreeNodeCallback(TreeView Tree, TreeNode Node);
        private void SelectTreeNode(TreeView Tree, TreeNode Node)
        {
            if (Tree.InvokeRequired)
            {
                SelectTreeNodeCallback d = new SelectTreeNodeCallback(SelectTreeNode);
                this.Invoke(d, new object[] { Tree, Node });
            }
            else
            {
                Tree.SelectedNode = Node;
                tabLogs.SelectedIndex = tabLogs.TabPages.Count - 1;
            }
        }
        delegate TreeNode GetSelectedTreeNodeCallback(TreeView CurrentTreeView);
        private TreeNode GetSelectedTreeNode(TreeView CurrentTreeView)
        {
            if (CurrentTreeView.InvokeRequired)
            {
                GetSelectedTreeNodeCallback d = new GetSelectedTreeNodeCallback(GetSelectedTreeNode);
                return (TreeNode)this.Invoke(d, new object[] { CurrentTreeView });
            }
            else
            {
                return CurrentTreeView.SelectedNode;
            }

        }

        
        private bool SearchWithinNodes(TreeView SearchTreeView, TreeNode StartingNode, string Text)
        {
            bool Result = StartingNode.Text.Contains(Text);
            if (Result)
                SelectTreeNode(SearchTreeView, StartingNode);

            if (!Result)
            {
                foreach (TreeNode Node in StartingNode.Nodes)
                {
                    if (Node.Nodes.Count > 0)
                        Result = SearchWithinNodes(SearchTreeView, Node, Text);
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
                    return SearchWithinNodes(SearchTreeView, NextNode, Text);
            }
            return Result;
        }
        delegate void FocusControlCallback(Control c);
        private void FocusControl(Control c)
        {
            if (tabLogs.InvokeRequired)
            {
                FocusControlCallback d = new FocusControlCallback(FocusControl);
                this.Invoke(d, new object[] { c });
            }
            else
                c.Focus();
        }
        private TreeNode GetNextNodeInList(TreeNode CurrentNode)
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
        private void Search()
        {
            bool SearchResult = SearchWithinNodes(GetSelectedTreeView(), GetSelectedTreeNode(GetSelectedTreeView()), txtFind.Text);
            if (SearchResult)
                FocusControl(GetSelectedTreeView());
            else
                MessageBox.Show("Search string \" " + txtFind.Text + "\" was not found within tree after selected node", "Nothing found", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, Consts.DEFAULT_MESSAGE_BOX_OPTIONS);

        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (SelectedTreeView != null)
            {
                if (SelectedTreeView.SelectedNode == null)
                    SelectedTreeView.SelectedNode = SelectedTreeView.Nodes[0];
                else                
                    SelectedTreeView.SelectedNode = GetNextNodeInList(SelectedTreeView.SelectedNode);                
                (new System.Threading.Thread(Search)).Start();

            }

        }
        private void HideShowSearch()
        {
            txtFind.Visible = (tabLogs.TabCount > 0);
            btnSearch.Visible = (tabLogs.TabCount > 0);
        }

        private void txtFind_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                btnSearch_Click(this, null);

        }
    }
}
