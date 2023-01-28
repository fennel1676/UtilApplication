using System.Text;
using System.IO;

namespace AKI.Util
{
    public class IniUtil
    {
        private string m_strSectionName;
        private string m_strFileName;

        public IniUtil() { }

        public IniUtil(string strFile)
        {
            if (!Directory.Exists(strFile))
                Directory.CreateDirectory(strFile);
        }

        public int LoadString(string keyName, out string returnedString, int nSize, string defaultValue = "")
        {
            StringBuilder temp = new StringBuilder(255);
            int nResult = AKI.Win32.API.Kernel32.GetPrivateProfileString(m_strSectionName, keyName, defaultValue, temp, nSize, m_strFileName);
            if (0 == nResult)
                returnedString = temp.ToString();
            else
                returnedString = null;
            return nResult;
        }

        public int LoadInt(string keyName, int defaultValue = 0)
        {
            return AKI.Win32.API.Kernel32.GetPrivateProfileInt(m_strSectionName, keyName, defaultValue, m_strFileName);
        }

        public double LoadFloat(string keyName, double defaultValue = 0.0)
        {
            string defStr = defaultValue.ToString();
            string returnedString;
            int nResult = LoadString(keyName, out returnedString, 128, defStr);

            if (0 == nResult)
            {
                double dResult = 0.0;
                if (double.TryParse(returnedString, out dResult))
                    return dResult;
                else
                    return defaultValue;
            }
            else
            {
                return defaultValue;
            }
        }

        // Saves settings to ini file
        public int SaveString(string keyName, string value)
        {
            string strQuot = string.Format("\"{0}\"", value);

            return AKI.Win32.API.Kernel32.WritePrivateProfileString(m_strSectionName, keyName, strQuot, m_strFileName);
        }

        public int SaveInt(string keyName, int value)
        {
            return AKI.Win32.API.Kernel32.WritePrivateProfileString(m_strSectionName, keyName, value.ToString(), m_strFileName);
        }

        public int SaveFloat(string keyName, double value)
        {
            return AKI.Win32.API.Kernel32.WritePrivateProfileString(m_strSectionName, keyName, value.ToString(), m_strFileName);
        }

        public void Transfer(bool bSave, string keyName, ref string value, int nSize, string defaultValue = "")
        {
            if (bSave)
                SaveString(keyName, value);
            else
                LoadString(keyName, out value, nSize, defaultValue);
        }

        public void Transfer(bool bSave, string keyName, ref bool value, bool defaultValue = false)
        {
            if (bSave)
                SaveInt(keyName, value ? 1 : 0);
            else
                value = 0 != LoadInt(keyName, defaultValue ? 1 : 0) ? true : false;
        }

        public void Transfer(bool bSave, string keyName, ref int value, int defaultValue = 0)
        {
            if (bSave)
                SaveInt(keyName, value);
            else
                value = LoadInt(keyName, defaultValue);
        }

        public void Transfer(bool bSave, string keyName, ref float value, float defaultValue = 0.0f)
        {
            if (bSave)
                SaveFloat(keyName, value);
            else
                value = (float)LoadFloat(keyName, defaultValue);
        }

        public void Transfer(bool bSave, string keyName, ref double value, double defaultValue = 0.0)
        {
            if (bSave)
                SaveFloat(keyName, value);
            else
                value = LoadFloat(keyName, defaultValue);
        }
    }
}
