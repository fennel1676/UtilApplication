using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AKI.Control.PictureBox
{
    public class StatusPictureBox : System.Windows.Forms.PictureBox
    {
        private int m_nStatus = 0;
        private Dictionary<int, Image> m_dicStatusImage = new Dictionary<int, Image>();
        private Image m_imgEnable = null;
        public int Status { get { return m_nStatus; } }
        public int ImageCount { get { return m_dicStatusImage.Count; } }

        public void AddEnableImage(Image img)
        {
            m_imgEnable = img;
        }

        public void AddStatusImage(int nStatus, Image img)
        {
            m_dicStatusImage[nStatus] = img;
        }

        public void RemoveStatusImage(int nStatus)
        {
            if (m_dicStatusImage.ContainsKey(nStatus))
                m_dicStatusImage.Remove(nStatus);
        }

        public void SetStatus(int nStatus)
        {
            if (m_nStatus == nStatus || !m_dicStatusImage.ContainsKey(nStatus))
                return;

            m_nStatus = nStatus;
            this.Image = Enabled ? m_dicStatusImage[nStatus] : (null == m_imgEnable) ? m_dicStatusImage[m_nStatus] : m_imgEnable;
            this.Update();
        }

        public void SetStatus(bool isStatus)
        {
            int nStatus = isStatus ? 1 : 0;
            if (m_nStatus == nStatus || !m_dicStatusImage.ContainsKey(nStatus))
                return;

            m_nStatus = nStatus;
            this.Image = Enabled ? m_dicStatusImage[nStatus] : (null == m_imgEnable) ? m_dicStatusImage[m_nStatus] : m_imgEnable;
            this.Update();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (Enabled)
                this.Image = m_dicStatusImage[m_nStatus];
            else
                this.Image = (null == m_imgEnable) ? m_dicStatusImage[m_nStatus] : m_imgEnable;

            base.OnEnabledChanged(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
        }
    }
}
