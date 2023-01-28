using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKI.Communication.FTP
{
    public class FTPDirectory : List<FTPFileInfo>
    {
        private const char C_SLASH = '/';

        public FTPDirectory()
        {

        }

        public FTPDirectory(string strDirectory, string strPath)
        {
            foreach (string line in strDirectory.Replace("\n", "").Split(System.Convert.ToChar('\r')))
            {
                if (line != "")
                {
                    this.Add(new FTPFileInfo(line, strPath));
                }
            }
        }

        public FTPDirectory GetFiles(string strExtension)
        {
            return this.GetFileOrDirectory(eDirectoryEntryTypes.File, strExtension);
        }

        public FTPDirectory GetDirectories()
        {
            return this.GetFileOrDirectory(eDirectoryEntryTypes.Directory, "");
        }

        private FTPDirectory GetFileOrDirectory(eDirectoryEntryTypes type, string strExtension)
        {
            FTPDirectory result = new FTPDirectory();
            foreach (FTPFileInfo fi in this)
            {
                if (fi.FileType == type)
                {
                    if ("" == strExtension)
                    {
                        result.Add(fi);
                    }
                    else if (strExtension == fi.Extension)
                    {
                        result.Add(fi);
                    }
                }
            }
            return result;

        }

        public bool FileExists(string strFileName)
        {
            foreach (FTPFileInfo ftpfile in this)
            {
                if (ftpfile.Filename == strFileName)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetParentDirectory(string strDirectory)
        {
            string strTemp = strDirectory.TrimEnd(C_SLASH);
            int i = strTemp.LastIndexOf(C_SLASH);
            if (i > 0)
                return strTemp.Substring(0, i - 1);
            else
                throw (new ApplicationException("No parent for root"));
        }
    }
}
