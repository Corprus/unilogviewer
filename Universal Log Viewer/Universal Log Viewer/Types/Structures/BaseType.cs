using System;
using UniversalLogViewer.Common;
using UniversalLogViewer.LogIniFiles;

namespace UniversalLogViewer.Types.Structures
{
    public abstract class BaseType
    {
        const string KeyTitle = "Title";
        const string KeyTitleType = "TitleType";
        const string KeyStyle = "Style";

        protected LogIniSection Section { get; set; }
        public string Name { get; private set; }
        public string SectionName { get { return Section.SectionName; } }
        public string Title { get; private set; }
        public StyleType Style { get; private set; }
        public TitleType TitleType { get; private set; }
        protected LogType ParentLogType { get; set; }
        protected BaseType(LogType logType, LogIniSection section)
        {
            ReInit(logType, section);
        }

        protected BaseType()
        {
        }
        public virtual void ReInit(LogType logType, LogIniSection section)
        {
            ParentLogType = logType;
            Section = section;
            try
            {
                Name = section.Values[true, Consts.KeyName, section.SectionName];
                Title = section.Values[KeyTitle];
                if (Title.Length == 0)
                    Title = Name;
                if (ParentLogType != null) //То есть это и есть лог тайп
                    Style = ParentLogType.Styles[section.Values[KeyStyle]];
                if (Style == null)
                    Style = new StyleType(false, false, false, false, System.Drawing.Color.Black, System.Drawing.Color.White, true, false);
                try
                {
                    TitleType = (TitleType)Enum.Parse(typeof(TitleType), section.Values[KeyTitleType], false);
                }
                catch (ArgumentException)
                {
                    TitleType = TitleType.Title;
                }
            }
            catch (IniFiles.Exceptions.IniFileRequiredFieldReadException e)
            {
                throw new Common.Exceptions.LogTypeLoadException(
                    string.Format("Required field missing in the log type description : {0}", e.Message), e);

            }

        }

    }
}
