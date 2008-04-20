using System;
using System.Collections.Generic;
using System.Text;

[assembly: CLSCompliant(true)]
namespace IniFiles
{
    public class IniFile
    {

        protected string FilePath { get; private set; }
        public string FileName { get { return FilePath; } }
        public string ShortFileName { get { return FileName.Substring(FileName.LastIndexOf('\\')); } }




        public IniFile(string INIPath)
        {
            FilePath = INIPath;
        }
        public virtual int WriteValue(string Section, string Key, string Value)
        {
            int Result = NativeMethods.WritePrivateProfileString(Section, Key, Value, this.FilePath);
            return Result;
        }
        public virtual string ReadValue(string Section, string Key)
        {
            return ReadValue(Section, Key, "");
        }
        public virtual string ReadValue(string Section, string Key, string Default)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = NativeMethods.GetPrivateProfileString(Section, Key, Default, temp,
                                            255, this.FilePath);
            if (i != 0)
            { }
            return temp.ToString();

        }
        public virtual bool ReadBoolValue(string Section, string Key, bool Default)
        {
            string Val;
            string UnVal;
            if (Default == true)
            {
                Val = "0";
                UnVal = "1";
            }
            else
            {
                Val = "1";
                UnVal = "0";
            }
            bool CompareResult = (this.ReadValue(Section, Key, UnVal) == Val);
            return (CompareResult != Default);

        }

        string[] FSectionNames()
        {

            uint MAX_BUFFER = 32767;
            byte[] pReturnedString = new byte[MAX_BUFFER];
            uint bytesReturned = NativeMethods.GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, this.FilePath);
            if (bytesReturned == 0)
                throw new Exceptions.IniFileSectionsReadException("Cannot read sections list from Ini file");
            String local = System.Text.Encoding.Default.GetString(pReturnedString);
            return FConvertUnicodeSeparatedStringsToNormalStrings(local);

        }
        string[] FConvertUnicodeSeparatedStringsToNormalStrings(string UnicodeText)
        {
            char SeparateChar = '\0';
            char[] SubChars = new char[] { SeparateChar };
            List<string> Result = new List<string>();
            Result.Add("");
            int j = 0;
            for (int i = 0; i < UnicodeText.Length; i++)
            {
                if (UnicodeText[i] != (SeparateChar))
                {
                    if (Result.Count <= j)
                        Result.Add("");
                    Result[j] += UnicodeText[i].ToString();
                }
                else if ((i < UnicodeText.Length - 1) && (UnicodeText[i + 1] == (SeparateChar)))
                {
                    j++; i++;
                }

            }
            return Result.ToArray();


        }
        public string[] SectionNames { get { return FSectionNames(); } }
    }


}
