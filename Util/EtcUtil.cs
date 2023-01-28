using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AKI.Util
{

    public class EtcUtil
    {
        public static bool HotKeyRegister(IntPtr hwnd, int nHotKeyCode, Keys key)
        {
            return AKI.Win32.API.User32.RegisterHotKey(hwnd, nHotKeyCode, 0, (int)key);
        }

        public static bool HotKeyUnregister(IntPtr hwnd, int nHotKeyCode)
        {
            return AKI.Win32.API.User32.UnregisterHotKey(hwnd, nHotKeyCode);
        }

        public static bool ExeExecute(string strWorkingPath, string strExecFileName, string strArguments, bool isSync = false)
        {
            try
            {
                string strFile = Path.Combine(strWorkingPath, strExecFileName);
                
                ProcessStartInfo stProcessStartInfo = new ProcessStartInfo(strFile);
                stProcessStartInfo.WorkingDirectory = strWorkingPath;

                stProcessStartInfo.RedirectStandardOutput = true;
                stProcessStartInfo.UseShellExecute = false;
                stProcessStartInfo.CreateNoWindow = false;
                stProcessStartInfo.FileName = strFile;
                stProcessStartInfo.Arguments = strArguments;
                Process pi = Process.Start(stProcessStartInfo);

                if (isSync)
                {
                    while (!pi.HasExited)
                    { }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : EtcUtil.Execute({0}, {1}, {2}) - {3}",
                                strWorkingPath, strExecFileName, strArguments, ex.Message);
                return false;
            }
        }

        public static string ExeExecuteOutput(string strWorkingPath, string strExecFileName, string strArguments)
        {
            try
            {
                string strFile = Path.Combine(strWorkingPath, strExecFileName);

                ProcessStartInfo stProcessStartInfo = new ProcessStartInfo(strFile);
                stProcessStartInfo.WorkingDirectory = strWorkingPath;

                stProcessStartInfo.RedirectStandardOutput = true;
                stProcessStartInfo.UseShellExecute = false;
                stProcessStartInfo.CreateNoWindow = false;
                stProcessStartInfo.FileName = strFile;
                stProcessStartInfo.Arguments = strArguments;
                Process pi = Process.Start(stProcessStartInfo);

                StringBuilder sb = new StringBuilder();
                while (true)
                {
                    string strResult = pi.StandardOutput.ReadLine();
                    if (null == strResult)
                        break;
                    else
                        sb.AppendLine(strResult);
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : EtcUtil.Execute({0}, {1}, {2}) - {3}",
                                strWorkingPath, strExecFileName, strArguments, ex.Message);
                return null;
            }
        }

        public static void CopyFolder(string sourceFolder, string destFolder, List<string> listDirectory = null, List<string> listFile = null, bool isDeleteDestFolder = true)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);
            else
            {
                if (isDeleteDestFolder)
                {
                    Directory.Delete(destFolder, true);
                    Directory.CreateDirectory(destFolder);
                }
            }

            string[] files = Directory.GetFiles(sourceFolder);
            string[] folders = Directory.GetDirectories(sourceFolder);

            foreach (string file in files)
            {
                if (null != listFile && listFile.Contains(file))
                    continue;
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest, true);
            }

            foreach (string folder in folders)
            {
                if (null != listDirectory && listDirectory.Contains(folder))
                    continue;
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyFolder(folder, dest);
            }
        }

        public static void DeleteFolder(string sourceFolder, List<string> listExceptionDirectory = null, List<string> listExceptionFile = null)
        {
            if (!Directory.Exists(sourceFolder))
                return;

            string[] files = Directory.GetFiles(sourceFolder);
            string[] folders = Directory.GetDirectories(sourceFolder);

            foreach (string file in files)
            {
                if (null != listExceptionFile && listExceptionFile.Contains(file))
                    continue;
                File.Delete(file);
            }

            foreach (string folder in folders)
            {
                if (null != listExceptionDirectory && listExceptionDirectory.Contains(folder))
                    continue;
                Directory.Delete(folder, true);
            }
        }
    }
}
