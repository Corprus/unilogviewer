using System.IO;


namespace IniFiles
{
    public class AutoSavedIniFile : IniFile
    {
        public AutoSavedIniFile(string INIPath)
            : base(INIPath)
        {
            if (!(File.Exists(FilePath)))
                File.Create(FilePath);
        }
        public override string ReadValue(string Section, string Key)
        {
            string sValue = base.ReadValue(Section, Key);
            if (sValue.Length == 0)
                WriteValue(Section, Key, sValue);
            return sValue;
        }
    }
}
