using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AKI.Control.TextBox
{
    public class TextBoxExEndKey : System.Windows.Forms.TextBox
    {
        public delegate void delegateEndKey(Keys key);

        [CategoryAttribute("Custom")]
        [IODescriptionAttribute("EndKey")]
        public event delegateEndKey Event_EndKey;

        private AKI.Util.HiPerfTimerUtil m_timer = new Util.HiPerfTimerUtil();
        public Keys EndKey { get; set; }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            
            if (keyData == EndKey && Win32.WM.KEYDOWN == msg.Msg)
            {
                if (!m_timer.IsRunning)
                    m_timer.Start();
                else
                {
                    m_timer.Stop();
                    if (100 > m_timer.DurationMilisecond)
                        return true;
                }

                if (null != Event_EndKey)
                    Event_EndKey(keyData);

                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
