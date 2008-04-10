using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using UniversalLogViewer.Common.IniFiles;

namespace UniversalLogViewer.Types.Structures
{
    public class StyleType : BaseType
    {
        public const string INI_TYPE_NAME = "Style";
        const string KEY_BOLD = "Bold";
        const string KEY_VISIBLE = "Visible";
        const string KEY_COLOR = "Color";
        const string KEY_TRIM = "Trim";
        public bool Visible { get; private set; }
        public bool Bold { get; private set; }
        public bool Trim { get; private set; }
        public Color Color { get; private set; }
        public StyleType(LogType LogType, LogIniSection Section)
            : base(LogType, Section)
        {
            ReInit(LogType, Section);
        }

        public StyleType()
        {
        }
        public override void ReInit(LogType LogType, LogIniSection Section)
        {
            base.ReInit(LogType, Section);
            Bold = (Section.Values[KEY_BOLD] == "1");
            Trim = (Section.Values[KEY_TRIM] != "0");
            Visible = (Section.Values[KEY_VISIBLE] != "0");
            try
            {                
                Color = Color.FromName(Section.Values[KEY_COLOR]);
            }
            catch (Exception)
            {
                Color = Color.Black;
            }
        }
        public StyleType(bool Bold, Color Color, bool Visible, bool Trim)
        {
            this.Bold = Bold;
            this.Color = Color;
            this.Visible = Visible;
            this.Trim = Trim;
        }
    }
}
