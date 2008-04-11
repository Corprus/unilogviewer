using System;
using System.Collections;
using System.Collections.Generic;
using UniversalLogViewer.Types.Structures;
using UniversalLogViewer.IniFiles;


namespace UniversalLogViewer.Common
{
    public class LogTypeCollection<T> : IEnumerable<T>
        where T : BaseType, new()

    {
        List<T> _TypeList;

        LogType LogType;
        public LogTypeCollection(LogType LogType)
        {
            _TypeList = new List<T>();
            this.LogType = LogType;
        }
        public T this[string Name]
        {
            get
            {
                foreach (T ClassDef in _TypeList)
                    if (ClassDef.SectionName == Name)
                        return ClassDef;
                
                //Если в списке не найден элемент - пытаемся прочитать...                               
                if (LogType.LogTypeFile.Sections[Name] != null)
                {
                    T NewElement = new T();
                    NewElement.ReInit(LogType, LogType.LogTypeFile.Sections[Name]);
                    _TypeList.Add(NewElement);
                    return NewElement;
                }
                return null;
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            foreach (T ClassDef in _TypeList)
                yield return ClassDef;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public bool AddType(string SectionName)
        {
            T Type = this[SectionName];
            return (Type != null);
        }
        public bool AddType(LogIniSection Section)
        {
            T Type = this[Section.SectionName];
            return (Type != null);
        }
        public bool AddType(T TypeDef)
        {
            if (this[TypeDef.Name] == null)
                _TypeList.Add(TypeDef);
            return (this[TypeDef.Name] != null);
        }
        public List<T> GetList(string[] Names)
        {
            List<T> Result = new List<T>();
            foreach (string Name in Names)
            {
                Result.Add(this[Name]);
            }
            return Result;
        }
        
    }

}