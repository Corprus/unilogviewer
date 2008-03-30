using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Universal_Log_Viewer.Common.IniFile;

namespace Universal_Log_Viewer.Types.Structures
{
    public class CStyle : CBaseType
    {
        public const string INI_TYPE_NAME = "Style";
        const string KEY_BOLD = "Bold";
        const string KEY_VISIBLE = "Visible";
        const string KEY_COLOR = "Color";
        public bool Visible { get; private set; }
        public bool Bold { get; private set; }
        public Color Color { get; private set; }
        public CStyle(CLogType LogType, CLogIniSection Section)
            : base(LogType, Section)
        {
            ReInit(LogType, Section);
        }

        public CStyle()
        {
        }
        public override void ReInit(CLogType LogType, CLogIniSection Section)
        {
            base.ReInit(LogType, Section);
            Bold = (IniSection.Values[KEY_BOLD] == "1");
            Visible = (IniSection.Values[KEY_VISIBLE] != "0");
            try
            {                
                Color = Color.FromName(IniSection.Values[KEY_COLOR]);
            }
            catch (Exception)
            {
                Color = Color.Black;
            }
        }
        public CStyle(bool Bold, Color Color, bool Visible)
        {
            this.Bold = Bold;
            this.Color = Color;
            this.Visible = Visible;
        }
    }
}
