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
            CLogTypeManager.oInstance.UpdateList(cmbLogTypes.Items);
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
    }
}
