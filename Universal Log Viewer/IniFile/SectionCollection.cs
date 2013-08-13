using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IniFiles
{
    public class SectionCollection<T> : IEnumerable<T>
        where T : IniSection
    {
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>) _iniSections).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        List<T> _iniSections;
        protected virtual T this[string sectionName, bool throwNonExistExceptions]
        {
            get
            {
                if (_iniSections == null)
                    _iniSections = new List<T>();
                foreach (T iniSection in _iniSections.Where(section => section.SectionName == sectionName))
                    return iniSection;
                string path = "";
                if (_iniSections.Count == 0)
                {
                    if (throwNonExistExceptions)
                        throw new Exceptions.IniFileSectionsReadException(
                            string.Format("Cannot read section with name \"{0}\" from file {1}", sectionName, path));
                }
                return null;

            }
        }

        public virtual T this[string sectionName]
        {
            get
            {
                return this[sectionName, true];

            }
        }
        public void AddSection(T section)
        {
            if (this[section.SectionName, false] == null)
                _iniSections.Add(section);
        }

    }
}
