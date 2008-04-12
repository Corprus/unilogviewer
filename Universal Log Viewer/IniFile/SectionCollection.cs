﻿using System.Collections;
using System.Collections.Generic;

namespace IniFiles
{
    public class SectionCollection<T> : IEnumerable<T>
        where T : IniSection
    {
        public IEnumerator<T> GetEnumerator()
        {
            foreach (T Section in _IniSections)
                yield return Section;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        List<T> _IniSections = null;
        public virtual T this[string SectionName]
        {
            get
            {
                if (_IniSections == null)
                    _IniSections = new List<T>();
                foreach (T IniSection in _IniSections)
                    if (IniSection.SectionName == SectionName)
                        return IniSection;
                string path = "";
                if (_IniSections.Count > 0)
                    path = _IniSections[0].IniFile.FileName;
                else
                throw new Exceptions.IniFileSectionsReadException("Cannot read section with name \"" + SectionName + "\" from file " + path);
                return null;

            }
        }
        public void AddSection(T Section)
        {
            if (this[Section.SectionName] == null)
                _IniSections.Add(Section);
        }

    }
}
