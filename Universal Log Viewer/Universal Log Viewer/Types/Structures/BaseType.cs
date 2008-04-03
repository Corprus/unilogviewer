using System;
using System.Collections.Generic;
using System.Text;
using Universal_Log_Viewer.Common.IniFile;

namespace Universal_Log_Viewer.Types.Structures
{
    public abstract class CBaseType
    {
        public enum TitleTypes
        {
            Title = 0,
            Value = 1,
            Source = 2
        }
        const string KEY_TITLE = "Title";
        const string KEY_TITLE_TYPE = "TitleType";
        const string KEY_STYLE = "Style";
        const string INI_TYPE_NAME = "";

        protected CLogIniSection IniSection { get; set; }
        public string Name { get; private set; }
        public string SectionName { get { return IniSection.SectionName; } }
        public string Title { get; private set; }
        public CStyle Style { get; private set; }
        public TitleTypes TitleType { get; private set; }
        protected CLogType ParentLogType { get; set; }
        public CBaseType(CLogType LogType, CLogIniSection Section)
        {
            ReInit(LogType, Section);
        }

        public CBaseType()
        {
        }
        public virtual void ReInit(CLogType LogType, CLogIniSection Section)
        {
            ParentLogType = LogType;
            IniSection = Section;
            Name = IniSection.Values[Consts.KEY_NAME];
            Title = IniSection.Values[KEY_TITLE];
            if (ParentLogType != null) //То есть это и есть лог тайп
                Style = ParentLogType.Styles[IniSection.Values[KEY_STYLE]];
            if (Style == null)
                Style = new CStyle(false, System.Drawing.Color.Black, true, false);
            try
            {
                TitleType = (TitleTypes)Enum.Parse(typeof(TitleTypes), IniSection.Values[KEY_TITLE_TYPE], false);
            }
            catch (ArgumentException)
            {
                TitleType = TitleTypes.Title;
            }

        }

    }
}
