using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniversalLogViewer.LogIniFiles;
using UniversalLogViewer.Types.Structures;

namespace UniversalLogViewer.Types.Managers
{
    public class LogTypeCollection<T> : IEnumerable<T>
        where T : BaseType, new()

    {
        readonly List<T> _typeList;

        readonly LogType _logType;

        public LogTypeCollection(LogType logType)
        {
            _typeList = new List<T>();
            _logType = logType;
        }
        public T this[string name]
        {
            get
            {
                var classDef = _typeList.FirstOrDefault(arg => arg.SectionName == name);
                if (classDef != null)
                    return classDef;

                    //Если в списке не найден элемент - пытаемся прочитать...                               
                    if (_logType.LogTypeFile.Sections[name] != null)
                    {
                        var newElement = new T();
                        newElement.ReInit(_logType, _logType.LogTypeFile.Sections[name]);
                        _typeList.Add(newElement);
                        return newElement;                     
                    }
                return null;
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>) _typeList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool AddType(string sectionName)
        {
            T type = this[sectionName];
            return (type != null);
        }
        public bool AddType(LogIniSection section)
        {
            T type = this[section.SectionName];
            return (type != null);
        }
        public bool AddType(T typeDef)
        {
            if (this[typeDef.Name] == null)
                _typeList.Add(typeDef);
            return (this[typeDef.Name] != null);
        }
        public List<T> GetList(IEnumerable<string> names)
        {
            return names.Select(name => this[name]).ToList();
        }
    }

}