using System;
using System.Linq;
using System.Windows.Forms;
using UniversalLogViewer.Properties;
using UniversalLogViewer.Types.Values;
using UniversalLogViewer.Types.Structures;
using UniversalLogViewer.Common.Types.Managers;
using UniversalLogViewer.Types.Managers;
using UniversalLogViewer.Common;

namespace UniversalLogViewer.UI
{
    public partial class MainForm : Form
    {
        private TreeView SelectedTreeView
        {
            get         
            {
                return GetSelectedTreeView();
            }
        }
        
        public MainForm()
        {
            InitializeComponent();
            LogProgress = new LogLoadProgress(prbProcess, lblProgress);
        }
        System.Threading.Thread _loadLogThread;

        public LogLoadProgress LogProgress { get; private set; }

        private void btnLoadLogTypes_Click(object sender, EventArgs e)
        {
            var fmLoadLogType = new LogTypesManagerForm();
            fmLoadLogType.ShowDialog(this);
            var currentLogType = cmbLogTypes.SelectedItem as LogType;
            LogTypeManager.Instance.UpdateList(cmbLogTypes.Items);
            if (currentLogType != null)
                foreach (var newLogType in cmbLogTypes.Items.Cast<object>().OfType<LogType>().Where(newLogType => newLogType.LogName == currentLogType.LogName))
                {
                    cmbLogTypes.SelectedItem = newLogType;
                }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmAbout = new FormAbout();
            frmAbout.ShowDialog();
        }

        private struct LoadLogParametersData
        {
            public LogType LoadedLogType;
            public string LogFileName;

        }
        private void LoadLog(object o)
        {

            var data = (LoadLogParametersData)o;
            var oLog = new Log(data.LoadedLogType, data.LogFileName);
            var logTab = new TabPage {Text = string.Format("{0} ({1})", data.LogFileName, oLog.StructureType.LogName)};
            var logTreeView = new TreeView {Dock = DockStyle.Fill};
            logTreeView.KeyPress += TreeView_KeyPress;
            logTreeView.AfterSelect += LogTreeViewSelectedItemChanged;

            logTab.Controls.Add(logTreeView);
            logTab.ContextMenuStrip = cntTabPopup;
            
            logTreeView.Nodes.Add(oLog.TreeNode);
            AddLogTabAndSelect(logTab);
        }

        delegate void AddLogTabAndSelectCallback(TabPage logTab);
        private void AddLogTabAndSelect(TabPage logTab)
        {
            if (tabLogs.InvokeRequired)
            {
                var d = new AddLogTabAndSelectCallback(AddLogTabAndSelect);
                Invoke(d, new object[] { logTab });
            }
            else
            {
                tabLogs.TabPages.Add(logTab);
                tabLogs.SelectedIndex = tabLogs.TabPages.Count - 1;
                HideShowSearch();
            }
        }

        private void BtnLoadLogClick(object sender, EventArgs e)
        {
            if (dlgOpenLog.ShowDialog() == DialogResult.OK)
            {
                string sLogFileName = dlgOpenLog.FileName;
                if (LogTypeManager.Instance.TypesList.Count > 0)
                {
                    if ((cmbLogTypes.SelectedIndex != -1)&&
                        (cmbLogTypes.Items[cmbLogTypes.SelectedIndex] is LogType))
                    {
                        var loadLogData = new LoadLogParametersData();
                        _loadLogThread = new System.Threading.Thread(LoadLog);
                        loadLogData.LoadedLogType = 
                            (cmbLogTypes.Items[cmbLogTypes.SelectedIndex] as LogType);
                        loadLogData.LogFileName = sLogFileName;
                        _loadLogThread.Start(loadLogData);
                    }
                    else
                    {
                        MessageBox.Show(Consts.SelectCorrectLogType, 
                                        Consts.SelectCorrectLogType, 
                                        MessageBoxButtons.OK, 
                                        MessageBoxIcon.Error, 
                                        MessageBoxDefaultButton.Button1, 
                                        Consts.DefaultMessageBoxOptions);
                    }
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LogTypeManager.Instance.UpdateList(cmbLogTypes.Items);
            Text = string.Format("{0} {1}", Application.ProductName, Application.ProductVersion);
            HideShowSearch();
        }

        private void CloseTabToolStripMenuItemClick(object sender, EventArgs e)
        {
            tabLogs.TabPages.Remove(tabLogs.SelectedTab);
            HideShowSearch();
        }

        private void TreeView_KeyPress(object sender, KeyPressEventArgs e)
        {
            var tree = sender as TreeView;
            if (sender == null || tree == null)
                return;

            if (tabLogs.SelectedIndex > -1 && e.KeyChar == 3) Clipboard.SetText(tree.SelectedNode.Text);
        }
        private void LogTreeViewSelectedItemChanged(object sender, EventArgs e)
        {
            var tree = (sender as TreeView);
            if ((!IniSettingsManager.ShowValueMemo) || (tree == null)) return;
            var stringTag = tree.SelectedNode.Tag as StringValue;
            if (stringTag != null)
            {
                memoValue.Visible = true;
                memoValue.Lines = stringTag.GetValues();
            }
            else
                memoValue.Visible = false;
        }

        private void MnuHelpFileClick(object sender, EventArgs e)
        {
            Help.ShowHelp(this, hlpUniLogViewer.HelpNamespace);
        }

        private void MnuHelpIndexClick(object sender, EventArgs e)
        {
            Help.ShowHelpIndex(this, hlpUniLogViewer.HelpNamespace);
        }
        delegate TreeView GetSelectedTreeViewCallback();
        private TreeView GetSelectedTreeView()
        {
            if (tabLogs.InvokeRequired)
            {
                var d = new GetSelectedTreeViewCallback(GetSelectedTreeView);
                return (TreeView) Invoke(d, new object[] {});
            }
            var selTab = tabLogs.SelectedTab;
            return selTab.Controls.OfType<TreeView>().FirstOrDefault();
        }

        delegate TreeNode GetSelectedTreeNodeCallback(TreeView currentTreeView);
        private TreeNode GetSelectedTreeNode(TreeView currentTreeView)
        {
            if (currentTreeView.InvokeRequired)
            {
                var d = new GetSelectedTreeNodeCallback(GetSelectedTreeNode);
                return (TreeNode)Invoke(d, new object[] { currentTreeView });
            }
            return currentTreeView.SelectedNode;
        }


        delegate void InitProgressLevelCallback(int max, int min, string action);
        public void InitProgressLevel(int max, int min, string action)
        {
            if (InvokeRequired)
            {
                var d = new InitProgressLevelCallback(InitProgressLevel);
                Invoke(d, new object[] { max, min, action });
            }
            else
            {
                LogProgress.InitProgressLevel(max, min);
                lblAction.Text = action;
                lblAction.Visible = true;
                prbProcess.Visible = true;
                lblProgress.Visible = true;
            }
        }
        delegate void EndProgressCallback();
        public void EndProgress()
        {
            if (InvokeRequired)
            {
                var d = new EndProgressCallback(EndProgress);
                Invoke(d, new object[] { });
            }
            else
            {
                lblAction.Visible = false;
                prbProcess.Visible = false;
                lblProgress.Visible = false;
            }
        }


        delegate void FocusControlCallback(Control c);
        private void FocusControl(Control c)
        {
            if (InvokeRequired)
            {
                var d = new FocusControlCallback(FocusControl);
                Invoke(d, new object[] { c });
            }
            else
                c.Focus();
        }

        private void Search()
        {
            TreeView selTreeView = GetSelectedTreeView();
            FocusControl(selTreeView);
            InitProgressLevel(selTreeView.GetNodeCount(true), 0, "Search progress...");
            var searchResult = UICommon.SearchWithinNodes(LogProgress, selTreeView, GetSelectedTreeNode(GetSelectedTreeView()), txtFind.Text);
            if (searchResult)
                FocusControl(selTreeView);
            else
                MessageBox.Show(
                    string.Format("Search string \" {0}\" was not found within tree after selected node", txtFind.Text),
                    Resources.NothingFoundMessage, MessageBoxButtons.OK, MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1, Consts.DefaultMessageBoxOptions);
            EndProgress();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (SelectedTreeView == null) return;
            SelectedTreeView.SelectedNode = SelectedTreeView.SelectedNode == null
                                                ? SelectedTreeView.Nodes[0]
                                                : UICommon.GetNextNodeInList(SelectedTreeView.SelectedNode);
            (new System.Threading.Thread(Search)).Start();
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

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_loadLogThread.IsAlive)
                _loadLogThread.Abort();
        }
    }

}
