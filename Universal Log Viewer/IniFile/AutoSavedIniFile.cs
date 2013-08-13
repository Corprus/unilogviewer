using System.IO;


namespace IniFiles
{
    public class AutoSavedIniFile : IniFile
    {
        public AutoSavedIniFile(string iniPath)
            : base(iniPath)
        {
            if (!(File.Exists(FilePath)))
                File.Create(FilePath);
        }
        public override string ReadValue(string section, string key)
        {
            string sValue = base.ReadValue(section, key);
            if (sValue.Length == 0)
                WriteValue(section, key, sValue);
            return sValue;
        }
    }
}
