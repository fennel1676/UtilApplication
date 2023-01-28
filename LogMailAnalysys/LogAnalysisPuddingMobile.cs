using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LogMailAnalysys
{
    public class LogAnalysisPuddingMobile
    {
        public static int TotalRunCount { get; set; }
        public static int TotalErrorCount { get; set; }
        public static Dictionary<int, int> SimulatorRunCount { get; set; }
        public static Dictionary<int, int> SimulatorErrorCount { get; set; }
        public static Dictionary<int, int> DeviceRunCount { get; set; }
        public static Dictionary<int, int> DeviceErrorCount { get; set; }

        private string m_strLogPath;
        private int m_nAppRunCount;
        private List<sStart> Start = new List<sStart>();
        
        public LogAnalysisPuddingMobile(string strLogPath)
        {
            string[] aLog = File.ReadAllLines(strLogPath);
            sStart start = null;
            for (int i = 0; i < aLog.Length; i++)
            {
                string line = aLog[i];
                string strLine = line.Replace("[", "");
                strLine = strLine.Replace("]", "");
                strLine = strLine.Replace(",", "");
                string[] aLine = strLine.Split(' ');

                if ("PNI-Start" == aLine[2])
                {
                    if (null == start)
                    {
                        start = new sStart();
                        Start.Add(start);
                        if (start.Analysys(ref i, aLog, aLine))
                            continue;
                        else
                            break;
                    }
                }
            }
        }

        public string Report()
        {
            StringBuilder sbTotal = new StringBuilder();
            List<StringBuilder> listStart = new List<StringBuilder>();
            if (0 < Start.Count)
            {
                sbTotal.AppendLine("운영 위치 : " + Start[0].ZoneName);
                sbTotal.AppendLine("운영 시작 시간 : " + Start[0].StartTime.ToString());
                sbTotal.AppendLine("운영 종료 시간 : " + Start[0].EndTime.ToString());
                sbTotal.AppendLine("런쳐 재시작 횟수 : " + Start.Count.ToString());
            }

            for (int i = 0; i < Start.Count; i++)
            {
                StringBuilder sbStart = new StringBuilder();
                listStart.Add(sbStart);
                sbStart.AppendLine("런쳐 순번 : " + (i + 1).ToString());
                sbStart.AppendLine("런쳐 시작 : " + Start[i].StartTime.ToString());
                sbStart.AppendLine("런쳐 종료 : " + Start[i].EndTime.ToString());

                List<StringBuilder> listContents = new List<StringBuilder>();
                for (int j = 0; j < Start[i].Contents.Count; j++)
                {

                }
                
            }

            return string.Empty;
        }
    }
}
