using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKI.Logger.Console
{
    public enum eLogResult
    {
        Unknown = 0,
        OK = 1,
        NotFilePath = 3,
        NotFoundDirectory = 4,
        NotAccessFile = 5,
    }

    public class LogInfo
    {
        private string logText = "";
        private DateTime logTime = DateTime.MinValue;

        public string LogText
        {
            get { return logText; }
            set { logText = value; }
        }
        public DateTime LogTime
        {
            get { return logTime; }
            set { logTime = value; }
        }

        public LogInfo()
        {
            this.logText = "";
            this.logTime = DateTime.Now;
        }
        public LogInfo(string logText)
        {
            this.logText = logText;
            this.logTime = DateTime.Now;
        }
        public LogInfo(string logText, DateTime logTime)
        {
            this.logText = logText;
            this.logTime = logTime;
        }
    }

    public class LogEventArgs : EventArgs
    {
        private LogInfo logInfo = null;

        public string LogText
        {
            get { return logInfo.LogText; }
            set { logInfo.LogText = value; }
        }
        public DateTime LogTime
        {
            get { return logInfo.LogTime; }
            set { logInfo.LogTime = value; }
        }

        public LogEventArgs()
        {
            this.logInfo = new LogInfo();
        }
        public LogEventArgs(LogInfo logInfo)
        {
            this.logInfo = new LogInfo(logInfo.LogText, logInfo.LogTime);
        }
    }
}
