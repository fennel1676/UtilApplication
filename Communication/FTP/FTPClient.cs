using System.Diagnostics;
using System.Data;
using System.Collections;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace AKI.Communication.FTP
{
    public class FTPClient
    {
        private string m_strHostname;

        public string Hostname
        {
            get
            {
                if (m_strHostname.StartsWith("ftp://"))
                {
                    return m_strHostname;
                }
                else
                {
                    return "ftp://" + m_strHostname;
                }
            }
            set
            {
                m_strHostname = value;
            }
        }

        private string m_strUsername;
        public string Username
        {
            get
            {
                return (m_strUsername == "" ? "anonymous" : m_strUsername);
            }
            set
            {
                m_strUsername = value;
            }
        }

        private string m_strPassword;
        public string Password
        {
            get
            {
                return m_strPassword;
            }
            set
            {
                m_strPassword = value;
            }
        }

        private string m_strCurrentDirectory = "/";
        public string CurrentDirectory
        {
            get
            {
                return m_strCurrentDirectory + ((m_strCurrentDirectory.EndsWith("/")) ? "" : "/").ToString();
            }
            set
            {
                if (!value.StartsWith("/"))
                {
                    throw (new ApplicationException("Directory should start with /"));
                }
                m_strCurrentDirectory = value;
            }
        }

        private string m_strLastDirectory = "";

        public FTPClient()
        {
        }

        public FTPClient(string strHostName)
        {
            m_strHostname = strHostName;
        }

        public FTPClient(string strHostName, string strUserName, string strPassword)
        {
            m_strHostname = strHostName;
            m_strUsername = strUserName;
            m_strPassword = strPassword;
        }

        public List<string> ListDirectory(string strDirectory)
        {
            List<string> listResult = null;
            try
            {
                System.Net.FtpWebRequest ftp = GetRequest(GetDirectory(strDirectory));
                ftp.Method = System.Net.WebRequestMethods.Ftp.ListDirectory;

                string str = GetStringResponse(ftp);
                str = str.Replace("\r\n", "\r").TrimEnd('\r');

                listResult = new List<string>();
                listResult.AddRange(str.Split('\r'));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : ListDirectory - " + ex.Message);
                return null;
            }
            return listResult;
        }

        public FTPDirectory ListDirectoryDetail(string strDirectory)
        {
            System.Net.FtpWebRequest ftp = GetRequest(GetDirectory(strDirectory));
            ftp.Method = System.Net.WebRequestMethods.Ftp.ListDirectoryDetails;

            string str = GetStringResponse(ftp);
            str = str.Replace("\r\n", "\r").TrimEnd('\r');
            return new FTPDirectory(str, m_strLastDirectory);
        }

        public bool Upload(string strLocalFilename, string strTargetFilename)
        {
            if (!File.Exists(strLocalFilename))
            {
                throw (new ApplicationException("File " + strLocalFilename + " not found"));
            }

            FileInfo fi = new FileInfo(strLocalFilename);
            return Upload(fi, strTargetFilename);
        }

        public bool Upload(FileInfo fi, string strTargetFilename)
        {
            string strTarget;
            if (strTargetFilename.Trim() == "")
                strTarget = this.CurrentDirectory + fi.Name;
            else if (strTargetFilename.Contains("/"))
                strTarget = AdjustDir(strTargetFilename);
            else
                strTarget = CurrentDirectory + strTargetFilename;

            string strURI = Hostname + strTarget;
            System.Net.FtpWebRequest ftp = GetRequest(strURI);

            ftp.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
            ftp.UseBinary = true;

            ftp.ContentLength = fi.Length;

            const int nBufferSize = 2048;
            byte[] aContent = new byte[nBufferSize - 1 + 1];
            int nDataReadCount;

            using (FileStream fs = fi.OpenRead())
            {
                try
                {
                    using (Stream rs = ftp.GetRequestStream())
                    {
                        do
                        {
                            nDataReadCount = fs.Read(aContent, 0, nBufferSize);
                            rs.Write(aContent, 0, nDataReadCount);
                        } while (!(nDataReadCount < nBufferSize));
                        rs.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    fs.Close();
                }
            }

            ftp = null;
            return true;
        }

        public bool Download(string strSourceFilename, string strLocalFilename, bool isPermitOverwrite)
        {
            FileInfo fi = new FileInfo(strLocalFilename);
            return this.Download(strSourceFilename, fi, isPermitOverwrite);
        }

        public bool Download(FTPFileInfo file, string strLocalFilename, bool isPermitOverwrite)
        {
            return this.Download(file.FullName, strLocalFilename, isPermitOverwrite);
        }

        public bool Download(FTPFileInfo file, FileInfo fiLocal, bool isPermitOverwrite)
        {
            return this.Download(file.FullName, fiLocal, isPermitOverwrite);
        }

        public bool Download(string strSourceFilename, FileInfo fiTarget, bool isPermitOverwrite)
        {
            if (fiTarget.Exists && !(isPermitOverwrite))
            {
                throw (new ApplicationException("Target file already exists"));
            }

            string strTarget;
            if (strSourceFilename.Trim() == "")
                throw (new ApplicationException("File not specified"));
            else if (strSourceFilename.Contains("/"))
                strTarget = AdjustDir(strSourceFilename);
            else
                strTarget = CurrentDirectory + strSourceFilename;

            string strURI = Hostname + strTarget;

            System.Net.FtpWebRequest ftp = GetRequest(strURI);
            ftp.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
            ftp.UseBinary = true;

            using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (FileStream fs = fiTarget.OpenWrite())
                    {
                        try
                        {
                            byte[] aBuffer = new byte[2048];
                            int nReadCount = 0;
                            do
                            {
                                nReadCount = responseStream.Read(aBuffer, 0, aBuffer.Length);
                                fs.Write(aBuffer, 0, nReadCount);
                            } while (!(0 == nReadCount));
                            responseStream.Close();
                            fs.Flush();
                            fs.Close();
                        }
                        catch (Exception)
                        {
                            fs.Close();
                            fiTarget.Delete();
                            throw;
                        }
                    }
                    responseStream.Close();
                }
                response.Close();
            }

            return true;
        }

        public bool FtpDelete(string strFileName)
        {
            string strURI = this.Hostname + GetFullPath(strFileName);

            System.Net.FtpWebRequest ftp = GetRequest(strURI);
            ftp.Method = System.Net.WebRequestMethods.Ftp.DeleteFile;
            try
            {
                string str = GetStringResponse(ftp);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool FtpFileExists(string strFileName)
        {
            try
            {
                long size = GetFileSize(strFileName);
                return true;

            }
            catch (Exception ex)
            {
                if (ex is System.Net.WebException)
                {
                    if (ex.Message.Contains("550"))
                        return false;
                    else
                        throw;
                }
                else
                {
                    throw;
                }
            }
        }

        public long GetFileSize(string strFileName)
        {
            string strPath;
            if (strFileName.Contains("/"))
                strPath = AdjustDir(strFileName);
            else
                strPath = this.CurrentDirectory + strFileName;

            string strURI = this.Hostname + strPath;
            System.Net.FtpWebRequest ftp = GetRequest(strURI);
            ftp.Method = System.Net.WebRequestMethods.Ftp.GetFileSize;
            string strTmp = this.GetStringResponse(ftp);
            return GetSize(ftp);
        }

        public bool FtpRename(string strSourceFileName, string strNewName)
        {
            string strSource = GetFullPath(strSourceFileName);
            if (!FtpFileExists(strSource))
            {
                throw (new FileNotFoundException("File " + strSource + " not found"));
            }

            string strTarget = GetFullPath(strNewName);
            if (strTarget == strSource)
            {
                throw (new ApplicationException("Source and target are the same"));
            }
            else if (FtpFileExists(strTarget))
            {
                throw (new ApplicationException("Target file " + strTarget + " already exists"));
            }

            string strURI = this.Hostname + strSource;

            System.Net.FtpWebRequest ftp = GetRequest(strURI);
            ftp.Method = System.Net.WebRequestMethods.Ftp.Rename;
            ftp.RenameTo = strTarget;

            try
            {
                string str = GetStringResponse(ftp);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool FtpCreateDirectory(string strDirectoryPath)
        {
            if (ExistDirectory(strDirectoryPath))
            {
                return true;
            }

            string[] aDirectory = strDirectoryPath.Split('/');
            StringBuilder sbDirectory = new StringBuilder();
            foreach(string strPath in aDirectory)
            {
                if (string.Empty == strPath)
                    continue;
                sbDirectory.AppendFormat("/{0}", strPath);
                if (!ExistDirectory(sbDirectory.ToString()))
                {
                    if (!CreateDirectory(sbDirectory.ToString()))
                        return false;
                }
            }
            return true;
        }

        private bool ExistDirectory(string strDirectoryPath)
        {
            try
            {
                List<string> listDirectory = this.ListDirectory(strDirectoryPath);
                return (null != listDirectory && 0 < listDirectory.Count) ? true : false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : ExistDirectory - " + ex.Message);
                return false;
            }
        }

        private bool CreateDirectory(string strDirectoryPath)
        {
            string strURI = this.Hostname + AdjustDir(strDirectoryPath);
            System.Net.FtpWebRequest ftp = GetRequest(strURI);
            ftp.Method = System.Net.WebRequestMethods.Ftp.MakeDirectory;

            try
            {
                string str = GetStringResponse(ftp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : FtpCreateDirectory : " + strDirectoryPath + " - " + ex.Message);
                return false;
            }
            return true;
        }

        public bool FtpDeleteDirectory(string strDirectoryPath)
        {
            string strURI = this.Hostname + AdjustDir(strDirectoryPath);
            System.Net.FtpWebRequest ftp = GetRequest(strURI);
            ftp.Method = System.Net.WebRequestMethods.Ftp.RemoveDirectory;

            try
            {
                string str = GetStringResponse(ftp);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private FtpWebRequest GetRequest(string strURI)
        {
            FtpWebRequest result = (FtpWebRequest)FtpWebRequest.Create(strURI);
            result.Credentials = GetCredentials();
            result.KeepAlive = false;

            return result;
        }

        private System.Net.ICredentials GetCredentials()
        {
            return new System.Net.NetworkCredential(Username, Password);
        }

        private string GetFullPath(string strFile)
        {
            if (strFile.Contains("/"))
            {
                return AdjustDir(strFile);
            }
            else
            {
                return this.CurrentDirectory + strFile;
            }
        }

        private string AdjustDir(string strPath)
        {
            return ((strPath.StartsWith("/")) ? "" : "/").ToString() + strPath;
        }

        private string GetDirectory(string strDirectory)
        {
            string strURI;
            if (strDirectory == "")
            {
                strURI = Hostname + this.CurrentDirectory;
                m_strLastDirectory = this.CurrentDirectory;
            }
            else
            {
                if (!strDirectory.StartsWith("/"))
                {
                    throw (new ApplicationException("Directory should start with /"));
                }
                strURI = this.Hostname + strDirectory;
                m_strLastDirectory = strDirectory;
            }
            return strURI;
        }

        private string GetStringResponse(FtpWebRequest ftp)
        {
            string strResult = "";
            using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
            {
                long nSize = response.ContentLength;
                using (Stream datastream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(datastream))
                    {
                        strResult = sr.ReadToEnd();
                        sr.Close();
                    }

                    datastream.Close();
                }

                response.Close();
            }

            return strResult;
        }

        private long GetSize(FtpWebRequest ftp)
        {
            long nSize;
            using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
            {
                nSize = response.ContentLength;
                response.Close();
            }

            return nSize;
        }
    }
}

