using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace AKI.Form.Splash
{
    public class SplashThread
    {
        private Thread m_threadSplash;
        private SplashForm m_formSplash;
        private bool m_isTheadCreated = false;
        private int nParentId;

        private static Dictionary<int, SplashThread> g_dicThreads = new Dictionary<int, SplashThread>();

        private SplashThread(int id)
        {
            m_threadSplash = new Thread(new ThreadStart(RunSplash));
            m_threadSplash.Start();
            m_isTheadCreated = true;
            nParentId = id;
        }

        public static SplashThread Instance()
        {
            int id = Process.GetCurrentProcess().Id;
            if (!g_dicThreads.ContainsKey(id))
            {
                g_dicThreads.Add(id, new SplashThread(id));
            }

            return g_dicThreads[id];
        }

        bool isShow = false;

        public void Show(Control ctrl)
        {
            Rectangle rt = new Rectangle(ctrl.Location, ctrl.Size);
            if (ctrl as System.Windows.Forms.Form == null)
                rt = ctrl.RectangleToScreen(rt);
            while (m_formSplash == null) ;
            m_formSplash.ParentRect = rt;
            isShow = true;
        }

        public void Hide()
        {
            isShow = false;
        }

        public void Close()
        {
            m_isTheadCreated = false;

            if (g_dicThreads.ContainsKey(nParentId))
                g_dicThreads.Remove(nParentId);
        }

        private void RunSplash()
        {
            m_formSplash = new SplashForm();
            while (m_isTheadCreated)
            {
                if (isShow)
                    m_formSplash.Show();
                else
                    m_formSplash.Hide();
                Application.DoEvents();
                Thread.Sleep(100);
            }

            if (m_formSplash.Visible == true)
                m_formSplash.Hide();
        }
    }
}
