using System.Collections.Generic;
using System.IO;

namespace UniversalLogViewer.Common
{
    public class FileReader
    {

        public string FileName { get; private set; }            
        public FileReader(string name)
        {
            FileName = name;
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
