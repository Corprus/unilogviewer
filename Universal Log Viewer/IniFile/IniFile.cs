using System;
using System.Text;

[assembly: CLSCompliant(true)]
namespace IniFiles
{

    public class IniFile
    {

        protected string FilePath { get; private set; }
        public string FileName { get { return FilePath; } }




        public IniFile(string INIPath)
        {
            FilePath = INIPath;
        }
        public void WriteValue(string Section, string Key, string Value)
        {
            NativeMethods.WritePrivateProfileString(Section, Key, Value, this.FilePath);
        }
        public virtual string ReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = NativeMethods.GetPrivateProfileString(Section, Key, "", temp,
                                            255, this.FilePath);
            return temp.ToString();

        }

        string[] FSectionNames()
        {

            uint MAX_BUFFER = 32767;
            byte[] pReturnedString = new byte[MAX_BUFFER];
            uint bytesReturned = NativeMethods.GetPrivateProfileSectionNamesA(pReturnedString, MAX_BUFFER, this.FilePath);
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
