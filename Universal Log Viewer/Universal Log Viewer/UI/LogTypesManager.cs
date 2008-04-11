using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using UniversalLogViewer.Types.Structures;
using UniversalLogViewer.Types.Managers;
using UniversalLogViewer.Common;

namespace UniversalLogViewer.UI
{
    public partial class LogTypesManagerForm : Form
    {
        public LogTypesManagerForm()
        {
            InitializeComponent();
            InitElements(true);
        }
        void InitElements(bool UpdateList)
        {
            if (UpdateList)
                LogTypeManager.oInstance.UpdateList(lbxLogTypes.Items);
            btnReInit.Enabled = (lbxLogTypes.SelectedItem is LogType);
            btnDelete.Enabled = (lbxLogTypes.SelectedItem is LogType);
            if (lbxLogTypes.SelectedItem is LogType)
                memoLogDescription.Rtf = (lbxLogTypes.SelectedItem as LogType).RTFDescription;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnImportLogType_Click(object sender, EventArgs e)
        {
            if (dlgOpenLogType.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string sLogFileName = dlgOpenLogType.FileName;
                LogTypeManager.oInstance.AddLogType(sLogFileName);
                InitElements(true);
            }

        }


        private void btnReInit_Click(object sender, EventArgs e)
        {
            if (lbxLogTypes.SelectedItem is LogType)
                (lbxLogTypes.SelectedItem as LogType).ReInit((lbxLogTypes.SelectedItem as LogType).LogTypeFile.FileName);
        }

        private void lbxLogTypes_SelectedValueChanged(object sender, EventArgs e)
        {
            InitElements(false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbxLogTypes.SelectedItem is LogType)
            {
                if (MessageBox.Show(Consts.TEXT_DELETE_LOG_TYPE, Consts.HEADER_DELETE_LOG_TYPE, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, Consts.DEFAULT_MESSAGE_BOX_OPTIONS) == DialogResult.Yes)
                {
                    LogType DeletedLogType = (lbxLogTypes.SelectedItem as LogType);
                    int DeletedIndexInManager = LogTypeManager.oInstance.TypesList.IndexOf(DeletedLogType);
                    string DeletedFileName = DeletedLogType.LogTypeFile.FileName;
                    System.IO.File.Delete(DeletedFileName);
                    LogTypeManager.oInstance.TypesList.RemoveAt(DeletedIndexInManager);
                    InitElements(true);
                }
            }


        }

        private void btnReInitAll_Click(object sender, EventArgs e)
        {
            LogTypeManager.ReInit();
            InitElements(true);
        }

        private void lbxLogTypes_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lbxLogTypes.SelectedItem is LogType)
                (lbxLogTypes.SelectedItem as LogType).ExternalOpen();

        }

    }
}
