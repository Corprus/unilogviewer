using System;
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
            this.LogProgress = new LogLoadProgress(this.prbProcess, this.lblProgress);
        }
        System.Threading.Thread LoadLogThread;

        public Common.LogLoadProgress LogProgress { get; private set; }

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
                    if ((cmbLogTypes.SelectedIndex != -1)&&
                        (cmbLogTypes.Items[cmbLogTypes.SelectedIndex] is LogType))
                    {
                        LoadLogParametersData LoadLogData = new LoadLogParametersData();
                        LoadLogThread = new System.Threading.Thread(LoadLog);
                        LoadLogData.LoadedLogType = 
                            (cmbLogTypes.Items[cmbLogTypes.SelectedIndex] as LogType);
                        LoadLogData.LogFileName = sLogFileName;
                        LoadLogThread.Start(LoadLogData);
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
            LogTypeManager.oInstance.UpdateList(cmbLogTypes.Items);
            this.Text = Application.ProductName + " " + Application.ProductVersion;
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


        delegate void InitProgressLevelCallback(int max, int min, string action);
        public void InitProgressLevel(int max, int min, string action)
        {
            if (InvokeRequired)
            {
                InitProgressLevelCallback d = new InitProgressLevelCallback(InitProgressLevel);
                this.Invoke(d, new object[] { max, min, action });
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
                EndProgressCallback d = new EndProgressCallback(EndProgress);
                this.Invoke(d, new object[] { });
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
            if (this.InvokeRequired)
            {
                FocusControlCallback d = new FocusControlCallback(FocusControl);
                this.Invoke(d, new object[] { c });
            }
            else
                c.Focus();
        }

        private void Search()
        {
            TreeView SelTreeView = GetSelectedTreeView();
            FocusControl(SelTreeView);
            InitProgressLevel(SelTreeView.GetNodeCount(true), 0, "Search progress...");
            bool SearchResult = UICommon.SearchWithinNodes(this.LogProgress, SelTreeView, GetSelectedTreeNode(GetSelectedTreeView()), txtFind.Text);
            if (SearchResult)
                FocusControl(SelTreeView);
            else
                MessageBox.Show("Search string \" " + txtFind.Text + "\" was not found within tree after selected node", "Nothing found", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, Consts.DefaultMessageBoxOptions);
            EndProgress();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (SelectedTreeView != null)
            {
                if (SelectedTreeView.SelectedNode == null)
                    SelectedTreeView.SelectedNode = SelectedTreeView.Nodes[0];
                else                
                    SelectedTreeView.SelectedNode = UICommon.GetNextNodeInList(SelectedTreeView.SelectedNode);                
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

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (LoadLogThread.IsAlive)
                LoadLogThread.Abort();
        }
    }

}
