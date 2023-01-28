using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AKI.Communication.FTP
{
    public class FTPFileInfo
    {
        private string m_strFilename;
        private string m_strPath;
        private eDirectoryEntryTypes m_typeFile;
        private long m_nSize;
        private DateTime m_dtFile;
        private string m_strPermission;
        private static string[] m_aStrParseFormats = new string[] {
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\w+\\s+\\w+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{4})\\s+(?<name>.+)",
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\d+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{4})\\s+(?<name>.+)",
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\d+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{1,2}:\\d{2})\\s+(?<name>.+)",
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})\\s+\\d+\\s+\\w+\\s+\\w+\\s+(?<size>\\d+)\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{1,2}:\\d{2})\\s+(?<name>.+)",
            "(?<dir>[\\-d])(?<permission>([\\-r][\\-w][\\-xs]){3})(\\s+)(?<size>(\\d+))(\\s+)(?<ctbit>(\\w+\\s\\w+))(\\s+)(?<size2>(\\d+))\\s+(?<timestamp>\\w+\\s+\\d+\\s+\\d{2}:\\d{2})\\s+(?<name>.+)",
            "(?<timestamp>\\d{2}\\-\\d{2}\\-\\d{2}\\s+\\d{2}:\\d{2}[Aa|Pp][mM])\\s+(?<dir>\\<\\w+\\>){0,1}(?<size>\\d+){0,1}\\s+(?<name>.+)" };

        public string FullName
        {
            get
            {
                return Path.TrimEnd('/') + "/" + Filename;
                
            }
        }
        public string Filename
        {
            get
            {
                return m_strFilename;
            }
        }
        public string Path
        {
            get
            {
                return m_strPath;
            }
        }
        public eDirectoryEntryTypes FileType
        {
            get
            {
                return m_typeFile;
            }
        }
        public long Size
        {
            get
            {
                return m_nSize;
            }
        }
        public DateTime FileDateTime
        {
            get
            {
                return m_dtFile;
            }
        }
        public string Permission
        {
            get
            {
                return m_strPermission;
            }
        }
        public string Extension
        {
            get
            {
                int i = this.Filename.LastIndexOf(".");
                if (i >= 0 && i < (this.Filename.Length - 1))
                {
                    return this.Filename.Substring(i + 1);
                }
                else
                {
                    return "";
                }
            }
        }
        public string NameOnly
        {
            get
            {
                int i = this.Filename.LastIndexOf(".");
                if (i > 0)
                {
                    return this.Filename.Substring(0, i);
                }
                else
                {
                    return this.Filename;
                }
            }
        }

        public FTPFileInfo(string strLine, string strPath)
        {
            Match m = GetMatchingRegex(strLine);
            if (m == null)
            {
                throw (new ApplicationException("Unable to parse line: " + strLine));
            }
            else
            {
                m_strFilename = m.Groups["name"].Value;
                m_strPath = strPath;

                Int64.TryParse(m.Groups["size"].Value, out m_nSize);

                m_strPermission = m.Groups["permission"].Value;
                string _dir = m.Groups["dir"].Value;
                if (_dir != "" && _dir != "-")
                    m_typeFile = eDirectoryEntryTypes.Directory;
                else
                    m_typeFile = eDirectoryEntryTypes.File;

                try
                {
                    m_dtFile = DateTime.Parse(m.Groups["timestamp"].Value);
                }
                catch (Exception)
                {
                    m_dtFile = Convert.ToDateTime(null);
                }
            }
        }

        private Match GetMatchingRegex(string strLine)
        {
            Regex rx;
            Match m;
            for (int i = 0; i <= m_aStrParseFormats.Length - 1; i++)
            {
                rx = new Regex(m_aStrParseFormats[i]);
                m = rx.Match(strLine);
                if (m.Success)
                {
                    return m;
                }
            }
            return null;
        }
    }
}
