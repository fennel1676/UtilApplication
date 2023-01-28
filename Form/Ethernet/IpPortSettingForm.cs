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
    public partial class IpPortSettingForm : System.Windows.Forms.Form
    {
        public string IP { get; set; }
        public int Port { get; set; }

        public IpPortSettingForm()
        {
            InitializeComponent();
        }

        private void m_buttonApply_Click(object sender, EventArgs e)
        {
            IP = string.Format("{0}.{1}.{2}.{3}", m_numericUpDownIP1.Value, m_numericUpDownIP2.Value, m_numericUpDownIP3.Value, m_numericUpDownIP4.Value);
            Port = (int)m_numericUpDownPort.Value;
            this.DialogResult = DialogResult.Yes;
        }

        private void m_buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
