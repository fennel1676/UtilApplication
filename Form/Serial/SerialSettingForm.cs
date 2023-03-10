using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AKI.Form.Serial
{
    public partial class SerialSettingForm : System.Windows.Forms.Form
    {
        public int Port { get; set; }
        public int Baudrate { get; set; }
        public int DataBits { get; set; }
        public int Parity { get; set; }
        public int StopBits { get; set; }

        public SerialSettingForm()
        {
            InitializeComponent();
        }

        public SerialSettingForm(int nPort)
        {
            InitializeComponent();

            m_numericUpDownComPort.Value = nPort;
        }

        private void m_buttonApply_Click(object sender, EventArgs e)
        {
            Port = (int)m_numericUpDownComPort.Value;
            Baudrate = int.Parse(m_comboBoxBaudrate.Text);
            DataBits = int.Parse(m_comboBoxDataBits.Text);
            Parity = m_comboBoxParity.SelectedIndex;
            StopBits = m_comboBoxStopBits.SelectedIndex;
            this.DialogResult = DialogResult.Yes;
        }

        private void m_buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
