using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniversalLogViewer.Common;

namespace UniversalLogViewer.Common
{
    public static class Utilities
    {
        public static string ProjectRevision { 
            get 
            {
                return UniversalLogViewer.Properties.Resources.svn_revision;
            } 
        }
    }
}
