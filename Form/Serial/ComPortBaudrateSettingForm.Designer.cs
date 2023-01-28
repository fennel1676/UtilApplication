namespace AKI.Form.Serial
{
    partial class ComPortBaudrateSettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_buttonApply = new System.Windows.Forms.Button();
            this.m_buttonCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.m_numericUpDownComPort = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.m_comboBoxBaudrate = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_numericUpDownComPort)).BeginInit();
            this.SuspendLayout();
            // 
            // m_buttonApply
            // 
            this.m_buttonApply.Location = new System.Drawing.Point(12, 66);
            this.m_buttonApply.Name = "m_buttonApply";
            this.m_buttonApply.Size = new System.Drawing.Size(75, 23);
            this.m_buttonApply.TabIndex = 2;
            this.m_buttonApply.Text = "적용";
            this.m_buttonApply.UseVisualStyleBackColor = true;
            this.m_buttonApply.Click += new System.EventHandler(this.m_buttonApply_Click);
            // 
            // m_buttonCancel
            // 
            this.m_buttonCancel.Location = new System.Drawing.Point(93, 66);
            this.m_buttonCancel.Name = "m_buttonCancel";
            this.m_buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.m_buttonCancel.TabIndex = 3;
            this.m_buttonCancel.Text = "취소";
            this.m_buttonCancel.UseVisualStyleBackColor = true;
            this.m_buttonCancel.Click += new System.EventHandler(this.m_buttonCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "COM 포트 :";
            // 
            // m_numericUpDownComPort
            // 
            this.m_numericUpDownComPort.Location = new System.Drawing.Point(93, 13);
            this.m_numericUpDownComPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_numericUpDownComPort.Name = "m_numericUpDownComPort";
            this.m_numericUpDownComPort.Size = new System.Drawing.Size(77, 21);
            this.m_numericUpDownComPort.TabIndex = 0;
            this.m_numericUpDownComPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Baudrate :";
            // 
            // m_comboBoxBaudrate
            // 
            this.m_comboBoxBaudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_comboBoxBaudrate.FormattingEnabled = true;
            this.m_comboBoxBaudrate.Items.AddRange(new object[] {
            "110",
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "38400",
            "56000",
            "57600",
            "115200",
            "128000",
            "256000"});
            this.m_comboBoxBaudrate.Location = new System.Drawing.Point(93, 40);
            this.m_comboBoxBaudrate.Name = "m_comboBoxBaudrate";
            this.m_comboBoxBaudrate.Size = new System.Drawing.Size(77, 20);
            this.m_comboBoxBaudrate.TabIndex = 1;
            // 
            // ComPortBaudrateSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(182, 98);
            this.ControlBox = false;
            this.Controls.Add(this.m_comboBoxBaudrate);
            this.Controls.Add(this.m_numericUpDownComPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_buttonCancel);
            this.Controls.Add(this.m_buttonApply);
            this.Name = "ComPortBaudrateSettingForm";
            this.Text = "시리얼 설정";
            ((System.ComponentModel.ISupportInitialize)(this.m_numericUpDownComPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_buttonApply;
        private System.Windows.Forms.Button m_buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown m_numericUpDownComPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox m_comboBoxBaudrate;
    }
}