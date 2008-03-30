using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Universal_Log_Viewer.Common;

namespace Universal_Log_Viewer.Common
{
    public static class Utilities
    {
        public static string ProjectRevision { 
            get 
            {
                return Universal_Log_Viewer.Properties.Resources.svn_revision;
            } 
        }
    }
}
