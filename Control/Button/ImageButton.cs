using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AKI.Control.Button
{
    public class ImageButton : System.Windows.Forms.Button
    {
        public class ImageButtonEventArgs : EventArgs
        {
            public int Status { get; set; }
        }

        private int m_nStatus;
        private int m_nStatusIndex;
        private Dictionary<eMouseEvent, Image> m_dicEventImage = new Dictionary<eMouseEvent, Image>();
        private Dictionary<int, Dictionary<eMouseEvent, Image>> m_dicDicEventImage = new Dictionary<int, Dictionary<eMouseEvent, Image>>();
        private bool m_isUsedEnableImage = false;
        private bool m_isChangeClickImage = true;

        public int Status { get { return m_nStatus; } }

        public int StatusIndex { get { return m_nStatusIndex; } }

        public bool ChangeClickImage { set { m_isChangeClickImage = value; } }

        public Color TextColor { get; set; }
               
        public ImageButton()
            : base()
        {
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
        }

        public void AddMouseEventImage(eMouseEvent eEvent, Image img)
        {
            m_dicEventImage[eEvent] = img;

            m_isUsedEnableImage = m_dicEventImage.ContainsKey(eMouseEvent.Enable);
        }

        public void AddMouseEventImage(int nStatus, eMouseEvent eEvent, Image img)
        {
            if (!m_dicDicEventImage.ContainsKey(nStatus))
                m_dicDicEventImage[nStatus] = new Dictionary<eMouseEvent, Image>();

            m_dicDicEventImage[nStatus][eEvent] = img;

            m_dicEventImage = m_dicDicEventImage.ElementAt(0).Value;
            m_nStatus = m_dicDicEventImage.ElementAt(0).Key;
            m_nStatusIndex = 0;
            m_isUsedEnableImage = m_dicEventImage.ContainsKey(eMouseEvent.Enable);
        }

        public void AddMouseEventImage(bool isStatus, eMouseEvent eEvent, Image img)
        {
            int nStatus = isStatus ? 1 : 0;
            if (!m_dicDicEventImage.ContainsKey(nStatus))
                m_dicDicEventImage[nStatus] = new Dictionary<eMouseEvent, Image>();

            m_dicDicEventImage[nStatus][eEvent] = img;

            m_dicEventImage = m_dicDicEventImage.ElementAt(0).Value;
            m_nStatus = m_dicDicEventImage.ElementAt(0).Key;
            m_nStatusIndex = 0;
            m_isUsedEnableImage = m_dicEventImage.ContainsKey(eMouseEvent.Enable);
        }

        public void RemoveMouseEventImage(eMouseEvent eEvent)
        {
            if (m_dicEventImage.ContainsKey(eEvent))
                m_dicEventImage.Remove(eEvent);

            m_isUsedEnableImage = m_dicEventImage.ContainsKey(eMouseEvent.Enable);
        }

        public void SetStatus(bool isStatus)
        {
            int nStatus = isStatus ? 1 : 0;
            if (m_nStatus == nStatus)
                return;

            m_dicEventImage = m_dicDicEventImage.ElementAt(nStatus).Value;
            m_nStatus = m_dicDicEventImage.ElementAt(nStatus).Key;
            this.Image = m_dicEventImage[eMouseEvent.MouseLeave];
            this.Update();
        }

        public void SetStatus(int nStatus)
        {
            if (m_nStatus == nStatus)
                return;

            m_dicEventImage = m_dicDicEventImage.ElementAt(nStatus).Value;
            m_nStatus = m_dicDicEventImage.ElementAt(nStatus).Key;
            this.Image = m_dicEventImage[eMouseEvent.MouseLeave];
            this.Update();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            if (null == this.Image)
                return;

            Graphics grpAll = pevent.Graphics;
            int nWidth = this.Height; // this.Image.Width * this.Height / this.Image.Height;
            int nHeight = this.Image.Height * nWidth / this.Image.Width;

            grpAll.DrawImage(this.Image, 0, 0, nWidth, nHeight);

            base.OnPaint(pevent);
        }

        //protected override void OnEnabledChanged(EventArgs e)
        //{
        //    eMouseEvent eEvent = this.Enabled ? eMouseEvent.MouseLeave : eMouseEvent.Enable;
        //    if (m_dicEventImage.ContainsKey(eMouseEvent.Click))
        //    {
        //        if (!m_dicEventImage.ContainsKey(eEvent))
        //            return;
        //        this.Image = m_dicEventImage[eEvent];
        //    }

        //    base.OnEnabledChanged(e);
        //}

        protected override void OnClick(EventArgs e)
        {
            if (m_dicEventImage.ContainsKey(eMouseEvent.Click))
            {
                if (m_isUsedEnableImage)
                {
                    if (this.Enabled)
                        this.Image = m_dicEventImage[eMouseEvent.Click];
                    else
                        this.Image = m_dicEventImage[eMouseEvent.Enable];
                }
                else
                    this.Image = m_dicEventImage[eMouseEvent.Click];
            }
                        
            if (0 != m_dicDicEventImage.Count && m_isChangeClickImage)
            {
                ImageButtonEventArgs ibea = new ImageButtonEventArgs();
                ibea.Status = m_nStatus;

                m_nStatusIndex++;
                if (m_dicDicEventImage.Count <= m_nStatusIndex)
                    m_nStatusIndex = 0;
                m_dicEventImage = m_dicDicEventImage.ElementAt(m_nStatusIndex).Value;
                m_nStatus = m_dicDicEventImage.ElementAt(m_nStatusIndex).Key;
                this.Image = m_dicEventImage.ElementAt(0).Value;

                base.OnClick(ibea);
            }
            else
                base.OnClick(e);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            if (m_dicEventImage.ContainsKey(eMouseEvent.DoubleClick))
            {
                if (m_isUsedEnableImage)
                {
                    if (this.Enabled)
                        this.Image = m_dicEventImage[eMouseEvent.DoubleClick];
                    else
                        this.Image = m_dicEventImage[eMouseEvent.Enable];
                }
                else
                    this.Image = m_dicEventImage[eMouseEvent.DoubleClick];
            }

            base.OnDoubleClick(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (m_dicEventImage.ContainsKey(eMouseEvent.MouseClick))
            {
                if (m_isUsedEnableImage)
                {
                    if (this.Enabled)
                        this.Image = m_dicEventImage[eMouseEvent.MouseClick];
                    else
                        this.Image = m_dicEventImage[eMouseEvent.Enable];
                }
                else
                    this.Image = m_dicEventImage[eMouseEvent.MouseClick];
            }

            base.OnMouseClick(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (m_dicEventImage.ContainsKey(eMouseEvent.MouseDoubleClick))
            {
                if (m_isUsedEnableImage)
                {
                    if (this.Enabled)
                        this.Image = m_dicEventImage[eMouseEvent.MouseDoubleClick];
                    else
                        this.Image = m_dicEventImage[eMouseEvent.Enable];
                }
                else
                    this.Image = m_dicEventImage[eMouseEvent.MouseDoubleClick];
            }

            base.OnMouseDoubleClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (m_dicEventImage.ContainsKey(eMouseEvent.MouseDown))
            {
                if (m_isUsedEnableImage)
                {
                    if (this.Enabled)
                        this.Image = m_dicEventImage[eMouseEvent.MouseDown];
                    else
                        this.Image = m_dicEventImage[eMouseEvent.Enable];
                }
                else
                    this.Image = m_dicEventImage[eMouseEvent.MouseDown];
            }

            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (m_dicEventImage.ContainsKey(eMouseEvent.MouseUp))
            {
                if (m_isUsedEnableImage)
                {
                    if (this.Enabled)
                        this.Image = m_dicEventImage[eMouseEvent.MouseUp];
                    else
                        this.Image = m_dicEventImage[eMouseEvent.Enable];
                }
                else
                    this.Image = m_dicEventImage[eMouseEvent.MouseUp];
            }

            base.OnMouseUp(mevent);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (m_dicEventImage.ContainsKey(eMouseEvent.MouseEnter))
            {
                if (m_isUsedEnableImage)
                {
                    if (this.Enabled)
                        this.Image = m_dicEventImage[eMouseEvent.MouseEnter];
                    else
                        this.Image = m_dicEventImage[eMouseEvent.Enable];
                }
                else
                    this.Image = m_dicEventImage[eMouseEvent.MouseEnter];
            }

            base.OnMouseEnter(e);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            if (m_dicEventImage.ContainsKey(eMouseEvent.MouseHover))
            {
                if (m_isUsedEnableImage)
                {
                    if (this.Enabled)
                        this.Image = m_dicEventImage[eMouseEvent.MouseHover];
                    else
                        this.Image = m_dicEventImage[eMouseEvent.Enable];
                }
                else
                    this.Image = m_dicEventImage[eMouseEvent.MouseHover];
            }

            base.OnMouseHover(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (m_dicEventImage.ContainsKey(eMouseEvent.MouseLeave))
            {
                if (m_isUsedEnableImage)
                {
                    if (this.Enabled)
                        this.Image = m_dicEventImage[eMouseEvent.MouseLeave];
                    else
                        this.Image = m_dicEventImage[eMouseEvent.Enable];
                }
                else
                    this.Image = m_dicEventImage[eMouseEvent.MouseLeave];
            }

            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            if (m_dicEventImage.ContainsKey(eMouseEvent.MouseMove))
            {
                if (m_isUsedEnableImage)
                {
                    if (this.Enabled)
                        this.Image = m_dicEventImage[eMouseEvent.MouseMove];
                    else
                        this.Image = m_dicEventImage[eMouseEvent.Enable];
                }
                else
                    this.Image = m_dicEventImage[eMouseEvent.MouseMove];
            }

            base.OnMouseMove(mevent);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (m_dicEventImage.ContainsKey(eMouseEvent.MouseWheel))
            {
                if (m_isUsedEnableImage)
                {
                    if (this.Enabled)
                        this.Image = m_dicEventImage[eMouseEvent.MouseWheel];
                    else
                        this.Image = m_dicEventImage[eMouseEvent.Enable];
                }
                else
                    this.Image = m_dicEventImage[eMouseEvent.MouseWheel];
            }

            base.OnMouseWheel(e);
        }
    }
}
