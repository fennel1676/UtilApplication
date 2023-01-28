using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace AKI.Form.Splash
{
    partial class SplashForm : System.Windows.Forms.Form
    {
        private int m_nDot;
        public Rectangle ParentRect { get; set; }

        public SplashForm()
        {
            InitializeComponent();
            m_nDot = 0;
        }

        private void timerLoadding_Tick(object sender, EventArgs e)
        {
            string title = "loading....";
            Text = title.Substring(0, title.Length - 4 + (m_nDot++ % 5));
        }

        private void SplashForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == true && ParentRect != null)
            {
                this.Location = ParentRect.Location;
                this.Size = ParentRect.Size;
                pictureBoxLoadding.Location = new Point(((ParentRect.Width - pictureBoxLoadding.Width) / 2),
                                                        ((ParentRect.Height - pictureBoxLoadding.Height) / 2));
            }
        }
    }
}
