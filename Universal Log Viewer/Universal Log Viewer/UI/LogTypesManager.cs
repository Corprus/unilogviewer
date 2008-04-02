using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Universal_Log_Viewer.Types.Structures;
using Universal_Log_Viewer.Types.Managers;

namespace Universal_Log_Viewer.UI
{
    public partial class frmLogTypesManager : Form
    {
        public frmLogTypesManager()
        {
            InitializeComponent();
            InitElements(true);
        }
        void InitElements(bool UpdateList)
        {
            if (UpdateList)
                CLogTypeManager.oInstance.UpdateList(lbxLogTypes.Items);
            btnReInit.Enabled = (lbxLogTypes.SelectedItem is CLogType);
            btnDelete.Enabled = (lbxLogTypes.SelectedItem is CLogType);
            if (lbxLogTypes.SelectedItem is CLogType)
                memoLogDescription.Rtf = (lbxLogTypes.SelectedItem as CLogType).GetRTFDescription();

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
                CLogTypeManager.oInstance.AddLogType(sLogFileName);
                InitElements(true);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void btnReInit_Click(object sender, EventArgs e)
        {
            if (lbxLogTypes.SelectedItem is CLogType)
                (lbxLogTypes.SelectedItem as CLogType).ReInit((lbxLogTypes.SelectedItem as CLogType).LogIniFile.FileName);
        }

        private void lbxLogTypes_SelectedValueChanged(object sender, EventArgs e)
        {
            InitElements(false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbxLogTypes.SelectedItem is CLogType)
            {
                if (MessageBox.Show( Consts.TEXT_DELETE_LOG_TYPE, Consts.HEADER_DELETE_LOG_TYPE, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    CLogType DeletedLogType = (lbxLogTypes.SelectedItem as CLogType);
                    int DeletedIndexInManager = CLogTypeManager.oInstance.TypesList.IndexOf(DeletedLogType);
                    string DeletedFileName = DeletedLogType.LogIniFile.FileName;
                    System.IO.File.Delete(DeletedFileName);
                    CLogTypeManager.oInstance.TypesList.RemoveAt(DeletedIndexInManager);
                    InitElements(true);
                }
            }


        }

        private void btnReInitAll_Click(object sender, EventArgs e)
        {
            CLogTypeManager.ReInit();
            InitElements(true);
        }

        private void lbxLogTypes_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lbxLogTypes.SelectedItem is CLogType)
                (lbxLogTypes.SelectedItem as CLogType).ExternalOpen();

        }

    }
}
