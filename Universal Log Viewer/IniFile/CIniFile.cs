using System;
using System.Runtime.InteropServices;
using System.Text;

namespace IniFile
{

    public class CIniFile
    {
        
        protected string path;
        public string FileName { get { return path; } }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        static extern uint GetPrivateProfileSectionNamesA(byte[] lpszReturnBuffer,
           uint nSize, string lpFileName);


        public CIniFile(string INIPath)
        {
            path = INIPath;
        }
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }
        public virtual string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp,
                                            255, this.path);
            return temp.ToString();

        }

        string[] FSectionNames()
        {

            uint MAX_BUFFER = 32767;
            byte[] pReturnedString = new byte[MAX_BUFFER];
            uint bytesReturned = GetPrivateProfileSectionNamesA(pReturnedString, MAX_BUFFER, this.path);
            if (bytesReturned == 0)
                return null;
            /*string local = Marshal.PtrToStringAnsi(pReturnedString, (int)bytesReturned);
            Marshal.FreeCoTaskMem(pReturnedString);
             */
            String local = System.Text.Encoding.Default.GetString(pReturnedString);
            //use of Substring below removes terminating null for split
            char[] SubChars = new char[] {'\0'};
            return local.Substring(0, local.Length - 1).Split(SubChars, StringSplitOptions.RemoveEmptyEntries);

        }
        public string[] SectionNames { get { return FSectionNames(); } }
    }


}
