using System;
using System.Collections.Generic;
using System.Text;
using UniversalLogViewer;
using UniversalLogViewer.Common;
using UniversalLogViewer.IniFiles;

namespace UniversalLogViewer.Types.Structures
{
    public abstract class BaseType
    {
        const string KEY_TITLE = "Title";
        const string KEY_TITLE_TYPE = "TitleType";
        const string KEY_STYLE = "Style";
        const string INI_TYPE_NAME = "";

        protected LogIniSection Section { get; set; }
        public string Name { get; private set; }
        public string SectionName { get { return Section.SectionName; } }
        public string Title { get; private set; }
        public StyleType Style { get; private set; }
        public Common.TitleType TitleType { get; private set; }
        protected LogType ParentLogType { get; set; }
        protected BaseType(LogType LogType, LogIniSection Section)
        {
            ReInit(LogType, Section);
        }

        protected BaseType()
        {
        }
        public virtual void ReInit(LogType LogType, LogIniSection Section)
        {
            ParentLogType = LogType;
            this.Section = Section;
            Name = Section.Values[Consts.KEY_NAME];
            Title = Section.Values[KEY_TITLE];
            if (Title.Length == 0)
                Title = Name;
            if (ParentLogType != null) //То есть это и есть лог тайп
                Style = ParentLogType.Styles[Section.Values[KEY_STYLE]];
            if (Style == null)
                Style = new StyleType(false, System.Drawing.Color.Black, true, false);
            try
            {
                TitleType = (Common.TitleType)Enum.Parse(typeof(Common.TitleType), Section.Values[KEY_TITLE_TYPE], false);
            }
            catch (ArgumentException)
            {
                TitleType = Common.TitleType.Title;
            }

        }

    }
}
