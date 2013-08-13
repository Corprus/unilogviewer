using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UniversalLogViewer.Common
{
    public class FileReader
    {

        public string FileName { get; private set; }            
        public FileReader(string Name)
        {
            FileName = Name;
        }
        public string[] ReadFile()
        {
            var s = new List<string>();

            StreamReader reader = File.OpenText(FileName);
            while (!reader.EndOfStream)
                s.Add(reader.ReadLine());
            reader.Close();
            return s.ToArray();
        }
    }
}
