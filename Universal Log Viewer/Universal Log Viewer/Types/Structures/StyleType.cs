using System;
using System.Drawing;
using UniversalLogViewer.LogIniFiles;

namespace UniversalLogViewer.Types.Structures
{
    public class StyleType : BaseType
    {
        public const string IniTypeName = "Style";
        const string KeyBold = "Bold";
        const string KeyItalic = "Italic";
        const string KeyUnderline = "Underline";
        const string KeyStrike = "Strike";
        const string KeyVisible = "Visible";
        const string KeyColor = "Color";
        const string KeyBackground = "Background";
        const string KeyTrim = "Trim";

        public bool Visible { get; private set; }
        public bool Bold { get; private set; }
        public bool Trim { get; private set; }
        public bool Italic { get; private set; }
        public bool Underline { get; private set; }
        public bool Strike { get; private set; }
        public Color Color { get; private set; }
        public Color Background { get; private set; }

        public StyleType()
        {
        }

        private Font _font;
        private void ReInitFont()
        {
            if (_font != null)
                _font.Dispose();
            _font = new Font(FontFamily.GenericSansSerif, 8);
            if (Bold)
                _font = new Font(_font, FontStyle.Bold);
            if (Italic)
                _font = new Font(_font, FontStyle.Italic);
            if (Underline)
                _font = new Font(_font, FontStyle.Underline);
            if (Strike)
                _font = new Font(_font, FontStyle.Strikeout);

        }
        public Font Font
        {
            get { return _font; }
        }

        public override void ReInit(LogType logType, LogIniSection section)
        {
            base.ReInit(logType, section);
            Bold = section.BoolValues[KeyBold];
            Italic = section.BoolValues[KeyItalic];
            Underline = section.BoolValues[KeyUnderline];
            Strike = section.BoolValues[KeyStrike];

            Trim = section.BoolValues[KeyTrim, true];
            Visible = section.BoolValues[KeyVisible, true];
            try
            {                
                Color = Color.FromName(section.Values[KeyColor]);
            }
            catch (Exception)
            {
                Color = Color.Black;
            }
            try
            {
                Background = Color.FromName(section.Values[KeyBackground]);
                if (section.Values[KeyBackground].Length == 0)
                    Background = Color.White;
            }
            catch (Exception)
            {
                Background = Color.White;
            }
            ReInitFont();
        }
        public StyleType(bool bold, bool italic, bool underline, bool strike, Color color, Color background, bool visible, bool trim)
        {
            Bold = bold;
            Color = color;
            Background = background;
            Visible = visible;
            Trim = trim;
            Underline = underline;
            Italic = italic;
            Strike = strike;
            ReInitFont();
        }
    }
}
