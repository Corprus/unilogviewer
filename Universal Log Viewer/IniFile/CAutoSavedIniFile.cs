using System.IO;


namespace IniFile
{
    public class CAutoSavedIniFile : CIniFile
    {
        public CAutoSavedIniFile(string INIPath)
            : base(INIPath)
        {
            if (!(File.Exists(path)))
                File.Create(path);
        }
        public override string IniReadValue(string Section, string Key)
        {
            string sValue = base.IniReadValue(Section, Key);
            if (sValue == "")
                IniWriteValue(Section, Key, sValue);
            return sValue;
        }
    }
}
