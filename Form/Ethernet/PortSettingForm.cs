using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AKI.Form.Ethernet
{
    public partial class PortSettingForm : System.Windows.Forms.Form
    {
        public int Port { get; set; }

        public PortSettingForm()
        {
            InitializeComponent();
        }

        private void m_buttonApply_Click(object sender, EventArgs e)
        {
            Port = (int)m_numericUpDownPort.Value;
            this.DialogResult = DialogResult.Yes;
        }

        private void m_buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
