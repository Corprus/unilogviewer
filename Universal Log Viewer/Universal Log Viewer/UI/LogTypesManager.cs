using System;
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
        void InitElements(bool updateList)
        {
            if (updateList)
                LogTypeManager.Instance.UpdateList(lbxLogTypes.Items);
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
            if (dlgOpenLogType.ShowDialog() != DialogResult.OK) return;
            var sLogFileName = dlgOpenLogType.FileName;
            LogTypeManager.Instance.AddLogType(sLogFileName);
            InitElements(true);
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
                if (MessageBox.Show(Consts.TextDeleteLogType, Consts.HeaderDeleteLogType, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, Consts.DefaultMessageBoxOptions) == DialogResult.Yes)
                {
                    var deletedLogType = (lbxLogTypes.SelectedItem as LogType);
                    var deletedIndexInManager = LogTypeManager.Instance.TypesList.IndexOf(deletedLogType);
                    var deletedFileName = deletedLogType.LogTypeFile.FileName;
                    System.IO.File.Delete(deletedFileName);
                    LogTypeManager.Instance.TypesList.RemoveAt(deletedIndexInManager);
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
