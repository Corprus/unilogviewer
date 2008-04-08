using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Universal_Log_Viewer.Types.Values;
using Universal_Log_Viewer.Types.Structures;
using Universal_Log_Viewer.Types.Managers;

namespace Universal_Log_Viewer.UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnLoadLogTypes_Click(object sender, EventArgs e)
        {
            frmLogTypesManager fmLoadLogType = new frmLogTypesManager();
            fmLoadLogType.ShowDialog(this);
            CLogType CurrentLogType = null;
            if (cmbLogTypes.SelectedItem is CLogType)
                CurrentLogType = (cmbLogTypes.SelectedItem as CLogType);
            CLogTypeManager.oInstance.UpdateList(cmbLogTypes.Items);
            if (CurrentLogType != null)
                foreach (var NewLogType in cmbLogTypes.Items)
                    if ((NewLogType is CLogType) && ((NewLogType as CLogType).LogName == CurrentLogType.LogName))
                        cmbLogTypes.SelectedItem = NewLogType;
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

        private void btnLoadLog_Click(object sender, EventArgs e)
        {
            if (dlgOpenLog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string sLogFileName = dlgOpenLog.FileName;
                if (CLogTypeManager.oInstance.TypesList.Count > 0)
                {
                    if ((cmbLogTypes.SelectedIndex != -1)&&(cmbLogTypes.Items[cmbLogTypes.SelectedIndex] is CLogType))
                    {
                        CLog oLog = new CLog((cmbLogTypes.Items[cmbLogTypes.SelectedIndex] as CLogType), sLogFileName);
                        TabPage LogTab = new TabPage();
                        LogTab.Text = dlgOpenLog.FileName + " (" + oLog.Type.LogName + ")";
                        TreeView LogTreeView = new TreeView();
                        LogTreeView.Dock = DockStyle.Fill;
                        LogTreeView.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TreeView_KeyPress);
                        LogTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.LogTreeViewSelectedItemChanged);
                             
                        LogTab.Controls.Add(LogTreeView);
                        LogTab.ContextMenuStrip = cntTabPopup;
                        LogTreeView.Nodes.Add(oLog.TreeNode);
                        tabLogs.TabPages.Add(LogTab);
                        tabLogs.SelectedIndex = tabLogs.TabPages.Count - 1;
                    }
                    else
                    {
                        MessageBox.Show(Consts.SELECT_CORRECT_LOG_TYPE);
                    }
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CLogTypeManager.oInstance.UpdateList(cmbLogTypes.Items);
        }

        private void closeTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabLogs.TabPages.Remove(tabLogs.SelectedTab);
        }

        private void TreeView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(sender is TreeView))
                return;

            if (tabLogs.SelectedIndex > -1)
                if (e.KeyChar == 3)
                    Clipboard.SetText(((TreeView)sender).SelectedNode.Text);         
        }
        private void LogTreeViewSelectedItemChanged(object sender, EventArgs e)
        {
            if ((CIniSettingsManager.ShowValueMemo) && (sender is TreeView))
            {
                TreeView Tree = (sender as TreeView);
                if (Tree.SelectedNode.Tag is CString)
                {
                    memoValue.Visible = true;
                    memoValue.Lines = (Tree.SelectedNode.Tag as CString).GetValues();
                }
                else
                {
                    memoValue.Visible = false;
                }

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


    }
}
