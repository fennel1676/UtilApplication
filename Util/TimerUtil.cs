using System;
using System.Threading;

namespace AKI.Util
{
    public class TimerUtil
    {
        private long m_lStartTime, m_lStopTime;
        private long m_lFrequency;
        private bool m_isRunning;

        public bool IsRunning { get { return m_isRunning;} }

        public TimerUtil()
        {
            m_lStartTime = 0;
            m_lStopTime = 0;
        }
        public double DurationSecond { get { return (double)(m_lStopTime - m_lStartTime) / (double)1000.0 ; } }
        public double DurationMilisecond { get { return (double)(m_lStopTime - m_lStartTime); } }

        public void Start()
        {
            Thread.Sleep(0);
            m_lStartTime = Environment.TickCount;
            m_isRunning = true;
        }

        public void Stop()
        {
            m_lStopTime = Environment.TickCount;
            m_isRunning = false;
        }
    }
}
