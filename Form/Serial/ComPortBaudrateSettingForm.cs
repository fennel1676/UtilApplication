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
    public partial class ComPortBaudrateSettingForm : System.Windows.Forms.Form
    {
        public int Port { get; set; }
        public int Baudrate { get; set; }

        public ComPortBaudrateSettingForm()
        {
            InitializeComponent();

            m_comboBoxBaudrate.SelectedIndex = 12;
        }

        public ComPortBaudrateSettingForm(int nPort, int nBaudrate)
        {
            InitializeComponent();

            m_numericUpDownComPort.Value = nPort;
            if (m_comboBoxBaudrate.Items.Contains(nBaudrate.ToString()))
                m_comboBoxBaudrate.SelectedItem = nBaudrate.ToString();
        }

        private void m_buttonApply_Click(object sender, EventArgs e)
        {
            Port = (int)m_numericUpDownComPort.Value;
            Baudrate = int.Parse(m_comboBoxBaudrate.Text);
            this.DialogResult = DialogResult.Yes;
        }

        private void m_buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
