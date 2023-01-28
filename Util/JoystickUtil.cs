using System;
using System.Threading;
using AKI.Win32;
using AKI.Win32.API;
using System.Runtime.InteropServices;

namespace AKI.Util
{
    public class JoystickUtil
    {
        public enum eJoystic
        {
            None,
            JoystickId1,
            JoystickId2,
            JoystickId12,
        }

        public delegate void EventJoystick(uint nJoystickId, Winmm.joyinfoexStruct joyinfoex);

        public event EventJoystick EventJoystickInfo;

        private AutoResetEvent m_autoEvent = new AutoResetEvent(false);
        private bool m_isThread = false;
        private Winmm.joyinfoexStruct m_joyinfoex1 = new Winmm.joyinfoexStruct();
        private Winmm.joyinfoexStruct m_joyinfoex2 = new Winmm.joyinfoexStruct();

        public static eJoystic CheckJoystic()
        {
            uint nCount = Winmm.joyGetNumDevs();
            if (0 < nCount)
            {
                Winmm.joyinfoexStruct joyinfoex = new Winmm.joyinfoexStruct();
                joyinfoex.dwSize = (UInt32)Marshal.SizeOf(joyinfoex);
                joyinfoex.dwFlags = JOY.RETURNALL;
                int joy1 = Winmm.joyGetPosEx(JOY.JOYSTICKID1, ref joyinfoex);
                int joy2 = Winmm.joyGetPosEx(JOY.JOYSTICKID2, ref joyinfoex);
                if (JOYERR.NOERROR == joy1 && JOYERR.NOERROR == joy2)
                    return eJoystic.JoystickId12;
                else if (JOYERR.NOERROR == joy1)
                    return eJoystic.JoystickId1;
                else if (JOYERR.NOERROR == joy2)
                    return eJoystic.JoystickId2;
                else
                    return eJoystic.None;
            }
            else
                return eJoystic.None;
        }

        public bool Start()
        {
            try
            {
                if (!m_isThread)
                {
                    m_isThread = true;
                    m_autoEvent.Reset();
                    Thread thread = new Thread(new ThreadStart(PlayThread));
                    thread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        
        public void Stop()
        {
            m_isThread = false;
            m_autoEvent.Set();
        }

        private void PlayThread()
        {
            Winmm.joyinfoexStruct joyinfoex = new Winmm.joyinfoexStruct();
            joyinfoex.dwSize = (UInt32)Marshal.SizeOf(joyinfoex);
            joyinfoex.dwFlags = JOY.RETURNALL;

            while (m_isThread)
            {
                if (m_autoEvent.WaitOne(10))
                    break;

                if (null == EventJoystickInfo)
                    break;

                if (JOYERR.NOERROR == Winmm.joyGetPosEx(JOY.JOYSTICKID1, ref joyinfoex))
                    EventJoystickInfo(JOY.JOYSTICKID1, joyinfoex);

                if (JOYERR.NOERROR == Winmm.joyGetPosEx(JOY.JOYSTICKID2, ref joyinfoex))
                    EventJoystickInfo(JOY.JOYSTICKID2, joyinfoex);
            }
        }
    }
}
