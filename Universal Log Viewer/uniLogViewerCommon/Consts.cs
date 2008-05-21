using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalLogViewer.Common
{
    public static class Consts
    {
        public const string ERROR_LOG_FILENAME = "unilogviewer.log";
        public const string INCONSISTENCIES_LOG_FILENAME = "inconsistencies.log";
        public const string EMPTY_SYMBOL = "";
        public const char ARRAY_SEPARATOR = ',';
        public const string KEY_NAME = "Name";
        public const string SETTINGS_INI_FILE_NAME = "logviewer.ini";
        public const string LOG_TYPE_EXTENSION = "ltp";
        public const string ASK_ADD_LOG_TYPE_WITH_SAME_NAME = "The Log type with the same name is already in manager. Do you want to add new one?";
        public const string HEADER_SAME_LOG_TYPE_PRESENT = "Same log type is present";
        public const string SELECT_CORRECT_LOG_TYPE = "Please select correct log type from the list";
        public const string TEXT_DELETE_LOG_TYPE = "Do you really want to delete log type (file will be deleted permanently)?";
        public const string HEADER_DELETE_LOG_TYPE = "Do you really want to delete?";
        public const System.Windows.Forms.MessageBoxOptions DEFAULT_MESSAGE_BOX_OPTIONS = (System.Windows.Forms.MessageBoxOptions)0;
        public static System.Drawing.Font DEFAULT_FONT = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 8);
        public static System.Drawing.Font DEFAULT_BOLD_FONT = new System.Drawing.Font(DEFAULT_FONT, System.Drawing.FontStyle.Bold);
        public static System.Drawing.Font DEFAULT_ITALIC_FONT = new System.Drawing.Font(DEFAULT_FONT, System.Drawing.FontStyle.Italic);
        public static System.Drawing.Font DEFAULT_STIKED_FONT = new System.Drawing.Font(DEFAULT_FONT, System.Drawing.FontStyle.Strikeout);
        public static System.Drawing.Font DEFAULT_UNDERLINED_FONT = new System.Drawing.Font(DEFAULT_FONT, System.Drawing.FontStyle.Underline);
        public static System.Drawing.Font DEFAULT_BOLD_ITALIC_FONT = new System.Drawing.Font(DEFAULT_BOLD_FONT, System.Drawing.FontStyle.Italic);
        public static System.Drawing.Font DEFAULT_BOLD_STRIKED_FONT = new System.Drawing.Font(DEFAULT_BOLD_FONT, System.Drawing.FontStyle.Strikeout);
        public static System.Drawing.Font DEFAULT_BOLD_UNDERLINED_FONT = new System.Drawing.Font(DEFAULT_BOLD_FONT, System.Drawing.FontStyle.Underline);
        public static System.Drawing.Font DEFAULT_ITALIC_STRIKED_FONT = new System.Drawing.Font(DEFAULT_ITALIC_FONT, System.Drawing.FontStyle.Strikeout);
        public static System.Drawing.Font DEFAULT_ITALIC_UNDERLINED_FONT = new System.Drawing.Font(DEFAULT_ITALIC_FONT, System.Drawing.FontStyle.Underline);
        public static System.Drawing.Font DEFAULT_BOLD_ITALIC_STRIKED_FONT = new System.Drawing.Font(DEFAULT_BOLD_ITALIC_FONT, System.Drawing.FontStyle.Strikeout);
        public static System.Drawing.Font DEFAULT_BOLD_ITALIC_UNDERLINED_FONT = new System.Drawing.Font(DEFAULT_BOLD_ITALIC_FONT, System.Drawing.FontStyle.Underline);
        public static System.Drawing.Font GetFontFromSettings(bool Bold, bool Italic, bool Underlined, bool Striked)
        {
            if (Bold)
            {
                if (Italic)
                {//bold italic
                    if (Underlined)
                    {//bold italic underlined
                        return DEFAULT_BOLD_ITALIC_UNDERLINED_FONT;
                    }
                    else
                    {
                        if (Striked)
                        {//bold italic striked
                            return DEFAULT_BOLD_ITALIC_STRIKED_FONT;
                        }
                        else
                        {
                            return DEFAULT_BOLD_ITALIC_FONT;
                        }
                    }
                }
                else
                {
                    if (Underlined)
                    {//bold underlined
                        return DEFAULT_BOLD_UNDERLINED_FONT;
                    }
                    else
                    {
                        if (Striked)
                        {//bold striked
                            return DEFAULT_BOLD_STRIKED_FONT;
                        }
                        else
                        {
                            return DEFAULT_BOLD_FONT;
                        }
                    }
                }
            }
            else//not bold
            {
                if (Italic)
                {//italic
                    if (Underlined)
                    {//italic underlined
                        return DEFAULT_ITALIC_UNDERLINED_FONT;
                    }
                    else
                    {
                        if (Striked)
                        {//italic striked
                            return DEFAULT_ITALIC_STRIKED_FONT;
                        }
                        else
                        {
                            return DEFAULT_ITALIC_FONT;
                        }
                    }
                }
                else
                {
                    if (Underlined)
                    {//underlined
                        return DEFAULT_UNDERLINED_FONT;
                    }
                    else
                    {
                        if (Striked)
                        {//striked
                            return DEFAULT_STIKED_FONT;
                        }
                        else
                        {
                            return DEFAULT_FONT;
                        }
                    }
                }
            }
        }

    }
}
