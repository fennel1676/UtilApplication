using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace AKI.Logger.Console
{
    public class LogManager
    {
        System.Windows.Forms.Timer m_deleteLogTimer;

        private string logFilePath = Directory.GetCurrentDirectory() + @"\Log";

        private int logDeleteDay = 30;
        private string logNameType = "yyyyMMdd_HH";
        private string logFileType = "log";

        public string LogFilePath { get { return logFilePath; } set { logFilePath = value; } }
        public int LogDeleteDay { get { return logDeleteDay; } set { logDeleteDay = value; } }
        public string LogNameType { get { return logNameType; } set { logNameType = value; } }
        public string LogFileType { get { return logFileType; } set { logFileType = value; } }
        public bool IsDeleteLog { get { return m_deleteLogTimer.Enabled; } set { m_deleteLogTimer.Enabled = value; } }

        public StreamWriter m_swLog;
        private Thread m_threadLog;
        private bool m_isTheadCreated = false;
        private List<LogInfo> m_listLog = new List<LogInfo>();

        public LogManager()
        {
            m_deleteLogTimer = new System.Windows.Forms.Timer();
            m_deleteLogTimer.Tick += new System.EventHandler(m_deleteLogTimer_Tick);

            SetupDebugConsole();
        }

        [Conditional("DEBUG")]
        public static void SetupDebugConsole()
        {
            AKI.Win32.API.Kernel32.AllocConsole();
        }

        [Conditional("DEBUG")]
        public static void DebugMessage(string msg)
        {
            System.Console.WriteLine(msg);
        }

        public eLogResult WriteLog(string logText)
        {
            eLogResult result = eLogResult.OK;
            LogInfo logInfo = new LogInfo(logText);

            if (logFilePath.Trim() != "")
            {
                string pathName = logFilePath.TrimEnd('\\');

                string fileName = pathName + "\\" + logInfo.LogTime.ToString("yyyyMMdd") + "." + logFileType;

                if (!Directory.Exists(pathName)) Directory.CreateDirectory(pathName);

                if (Directory.Exists(pathName))
                {
                    try
                    {
                        using (StreamWriter sw = File.AppendText(fileName))
                        {
                            sw.WriteLine(logInfo.LogTime.ToString("[yyyy-MM-dd HH:mm:ss] ") + logInfo.LogText);
                            DebugMessage(logInfo.LogTime.ToString("[yyyy-MM-dd HH:mm:ss] ") + logInfo.LogText);
                        }
                    }
                    catch
                    {
                        result = eLogResult.NotAccessFile;
                        DebugMessage(result.ToString());
                    }
                }
                else
                {
                    result = eLogResult.NotFoundDirectory;
                    DebugMessage(result.ToString());
                }
            }
            else
            {
                result = eLogResult.NotFilePath;
                DebugMessage(result.ToString());
            }

            return result;
        }

        public void CreateLog()
        {
            m_threadLog = new Thread(new ThreadStart(RunLog));
            m_threadLog.Start();
            m_isTheadCreated = true;
        }

        public void DeleteLog()
        {
            m_isTheadCreated = false;
        }

        private void RunLog()
        {
            int nCount = 0;
            while (m_isTheadCreated)
            {
                nCount = m_listLog.Count;
                if (0 < nCount)
                {
                    OpenLog();
                    for (int i = 0; i < nCount; i++)
                    {
                        WriteLog(m_listLog[0]);
                        m_listLog.RemoveAt(0);
                    }
                    CloseLog();
                }

                Thread.Sleep(100);
            }
        }

        public eLogResult OpenLog()
        {
            eLogResult result = eLogResult.OK;

            if (logFilePath.Trim() != "")
            {
                string pathName = logFilePath.TrimEnd('\\');
                string fileName = pathName + "\\" + DateTime.Now.ToString("yyyyMMdd") + "." + logFileType;

                if (!Directory.Exists(pathName))
                    Directory.CreateDirectory(pathName);

                if (Directory.Exists(pathName))
                {
                    try
                    {
                        m_swLog = File.AppendText(fileName);
                    }
                    catch
                    {
                        result = eLogResult.NotAccessFile;
                        DebugMessage(result.ToString());
                    }
                }
                else
                {
                    result = eLogResult.NotFoundDirectory;
                    DebugMessage(result.ToString());
                }
            }
            else
            {
                result = eLogResult.NotFilePath;
                DebugMessage(result.ToString());
            }

            return result;
        }

        public eLogResult CloseLog()
        {
            eLogResult result = eLogResult.OK;
            if (null != m_swLog)
            {
                try
                {
                    m_swLog.Close();
                }
                catch
                {
                    result = eLogResult.NotFilePath;
                    DebugMessage(result.ToString());
                }
                finally
                {
                    if (m_swLog != null)
                        m_swLog.Dispose();
                }
            }
            return result;
        }

        public void SaveLog(string logText)
        {
            m_listLog.Add(new LogInfo(logText));
        }

        private eLogResult WriteLog(LogInfo logInfo)
        {
            eLogResult result = eLogResult.OK;

            try
            {
                m_swLog.WriteLine(logInfo.LogTime.ToString("[HH:mm:ss.fff] ") + logInfo.LogText);
                DebugMessage(logInfo.LogTime.ToString("[HH:mm:ss.fff] ") + logInfo.LogText);
            }
            catch
            {
                result = eLogResult.NotAccessFile;
                DebugMessage(result.ToString());
            }

            return result;
        }

        private void m_deleteLogTimer_Tick(object sender, EventArgs e)
        {
            DirectoryInfo files = new DirectoryInfo(logFilePath);
            if (files.Exists == true)
            {
                foreach (FileInfo fileInfo in files.GetFiles())
                {
                    if (fileInfo.CreationTime <= DateTime.Now.AddDays(-logDeleteDay))
                    {
                        try
                        {
                            fileInfo.Delete();
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }
    }
}
 