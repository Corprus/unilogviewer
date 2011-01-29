using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using UniversalLogViewer.LogIniFiles;

namespace UniversalLogViewer.Types.Structures
{
    public class StyleType : BaseType
    {
        public const string INI_TYPE_NAME = "Style";
        const string KEY_BOLD = "Bold";
        const string KEY_ITALIC = "Italic";
        const string KEY_UNDERLINE = "Underline";
        const string KEY_STRIKE = "Strike";
        const string KEY_VISIBLE = "Visible";
        const string KEY_COLOR = "Color";
        const string KEY_BACKGROUND = "Background";
        const string KEY_TRIM = "Trim";

        public bool Visible { get; private set; }
        public bool Bold { get; private set; }
        public bool Trim { get; private set; }
        public bool Italic { get; private set; }
        public bool Underline { get; private set; }
        public bool Strike { get; private set; }
        public Color Color { get; private set; }
        public Color Background { get; private set; }
        public StyleType(LogType LogType, LogIniSection Section)
            : base(LogType, Section)
        {
            ReInit(LogType, Section);
        }

        public StyleType()
        {
        }
        private Font _font;
        private void ReInitFont()
        {
            if (_font != null)
                _font.Dispose();
            _font = new System.Drawing.Font(FontFamily.GenericSansSerif, 8);
            if (this.Bold)
                _font = new System.Drawing.Font(_font, FontStyle.Bold);
            if (this.Italic)
                _font = new System.Drawing.Font(_font, FontStyle.Italic);
            if (this.Underline)
                _font = new System.Drawing.Font(_font, FontStyle.Underline);
            if (this.Strike)
                _font = new System.Drawing.Font(_font, FontStyle.Strikeout);

        }
        public Font Font
        {
            get { return _font; }
        }

        public override void ReInit(LogType LogType, LogIniSection Section)
        {
            base.ReInit(LogType, Section);
            Bold = Section.BoolValues[KEY_BOLD];
            Italic = Section.BoolValues[KEY_ITALIC];
            Underline = Section.BoolValues[KEY_UNDERLINE];
            Strike = Section.BoolValues[KEY_STRIKE];

            Trim = Section.BoolValues[KEY_TRIM, true];
            Visible = Section.BoolValues[KEY_VISIBLE, true];
            try
            {                
                Color = Color.FromName(Section.Values[KEY_COLOR]);
            }
            catch (Exception)
            {
                Color = Color.Black;
            }
            try
            {
                Background = Color.FromName(Section.Values[KEY_BACKGROUND]);
                if (Section.Values[KEY_BACKGROUND].Length == 0)
                    Background = Color.White;
            }
            catch (Exception)
            {
                Background = Color.White;
            }
            ReInitFont();
        }
        public StyleType(bool Bold, bool Italic, bool Underline, bool Strike, Color Color, Color Background, bool Visible, bool Trim)
        {
            this.Bold = Bold;
            this.Color = Color;
            this.Background = Background;
            this.Visible = Visible;
            this.Trim = Trim;
            this.Underline = Underline;
            this.Italic = Italic;
            this.Strike = Strike;
            ReInitFont();
        }
    }
}
