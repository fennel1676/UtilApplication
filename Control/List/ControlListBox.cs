using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AKI.Control.List
{
    public partial class ControlListBox : UserControl
    {
        private int m_nStartIndex = 0;
        private int m_nHeight;
        private List<System.Windows.Forms.Control> m_listControl = new List<System.Windows.Forms.Control>();

        public int MaxItem { get; set; }

        public ControlListBox()
        {
            InitializeComponent();
        }

        public void UpdateControl()
        {
            m_tableLayoutPanel.Controls.Clear();
            m_tableLayoutPanel.RowStyles.Clear();
            for (int i = 0; i < m_listControl.Count; i++)
            {
                m_tableLayoutPanel.Controls.Add(m_listControl[i], 0, i);
                m_listControl[i].Dock = DockStyle.Fill;
                m_listControl[i].Margin = new Padding(0);
                m_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, m_nHeight));
            }
            m_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, m_nHeight));

            m_vScrollBar.Maximum = m_listControl.Count - 1;
        }

        public void AddControl(System.Windows.Forms.Control control)
        {
            m_nHeight = control.Height;
            m_listControl.Add(control);
        }

        private void m_vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (ScrollEventType.EndScroll == e.Type)
            {
                m_nStartIndex = m_vScrollBar.Value;

                for (int i = 0; i < m_nStartIndex; i++)
                {
                    m_tableLayoutPanel.RowStyles[i].Height = 0;
                }

                for (int i = m_nStartIndex; i < m_listControl.Count; i++)
                {
                    m_tableLayoutPanel.RowStyles[i].Height = m_nHeight;
                }
            }
        }
    }
}
