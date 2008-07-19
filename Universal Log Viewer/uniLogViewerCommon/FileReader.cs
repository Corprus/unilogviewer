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
            StreamReader SR;
            List<string> S = new List<string>();

            SR = File.OpenText(FileName);
            while (!SR.EndOfStream)
                S.Add(SR.ReadLine());
            SR.Close();
            return S.ToArray();
        }
    }
}
