using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AKI.Form.MessageBox
{
    public class AutoClosingMessageBox
    {
        private Timer m_timeoutTimer;
        private string m_strCaption;

        public AutoClosingMessageBox(string strText, string strCaption, int nTimeout)
        {
            m_strCaption = strCaption;
            m_timeoutTimer = new Timer(m_timeoutTimer_OnTimerElapsed, null, nTimeout, Timeout.Infinite);
            System.Windows.Forms.MessageBox.Show(strText, strCaption);
        }

        public static void Show(string text, string caption, int timeout)
        {
            new AutoClosingMessageBox(text, caption, timeout);
        }

        private void m_timeoutTimer_OnTimerElapsed(object state)
        {
            IntPtr mbWnd = AKI.Win32.API.User32.FindWindow(null, m_strCaption);
            if (mbWnd != IntPtr.Zero)
                AKI.Win32.API.User32.SendMessage(mbWnd, AKI.Win32.WM.CLOSE, IntPtr.Zero, IntPtr.Zero);
            m_timeoutTimer.Dispose();
        }

    }
}
