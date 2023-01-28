using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace AKI.Util
{
    public class HiPerfTimerUtil
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        private long m_lStartTime, m_lStopTime;
        private long m_lFrequency;
        private bool m_isRunning;

        public bool IsRunning { get { return m_isRunning;} }

        public HiPerfTimerUtil()
        {
            m_lStartTime = 0;
            m_lStopTime = 0;

            if (QueryPerformanceFrequency(out m_lFrequency) == false)
                throw new Win32Exception();
        }
        public double DurationSecond { get { return (double)(m_lStopTime - m_lStartTime) / (double)m_lFrequency; } }
        public double DurationMilisecond { get { return (double)(m_lStopTime - m_lStartTime) / (double)m_lFrequency * 1000; } }

        public void Start()
        {
            Thread.Sleep(0);
            QueryPerformanceCounter(out m_lStartTime);
            m_isRunning = true;
        }

        public void Stop()
        {
            QueryPerformanceCounter(out m_lStopTime);
            m_isRunning = false;
        }
    }
}
