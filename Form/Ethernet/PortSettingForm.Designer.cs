namespace AKI.Form.Ethernet
{
    partial class PortSettingForm
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
            this.m_numericUpDownPort = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.m_buttonApply = new System.Windows.Forms.Button();
            this.m_buttonCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.m_numericUpDownPort)).BeginInit();
            this.SuspendLayout();
            // 
            // m_numericUpDownPort
            // 
            this.m_numericUpDownPort.Location = new System.Drawing.Point(66, 12);
            this.m_numericUpDownPort.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.m_numericUpDownPort.Name = "m_numericUpDownPort";
            this.m_numericUpDownPort.Size = new System.Drawing.Size(102, 21);
            this.m_numericUpDownPort.TabIndex = 0;
            this.m_numericUpDownPort.Value = new decimal(new int[] {
            4444,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "Port :";
            // 
            // m_buttonApply
            // 
            this.m_buttonApply.Location = new System.Drawing.Point(12, 39);
            this.m_buttonApply.Name = "m_buttonApply";
            this.m_buttonApply.Size = new System.Drawing.Size(75, 23);
            this.m_buttonApply.TabIndex = 1;
            this.m_buttonApply.Text = "적용";
            this.m_buttonApply.UseVisualStyleBackColor = true;
            this.m_buttonApply.Click += new System.EventHandler(this.m_buttonApply_Click);
            // 
            // m_buttonCancel
            // 
            this.m_buttonCancel.Location = new System.Drawing.Point(93, 39);
            this.m_buttonCancel.Name = "m_buttonCancel";
            this.m_buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.m_buttonCancel.TabIndex = 2;
            this.m_buttonCancel.Text = "취소";
            this.m_buttonCancel.UseVisualStyleBackColor = true;
            this.m_buttonCancel.Click += new System.EventHandler(this.m_buttonCancel_Click);
            // 
            // PortSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(180, 72);
            this.Controls.Add(this.m_buttonCancel);
            this.Controls.Add(this.m_buttonApply);
            this.Controls.Add(this.m_numericUpDownPort);
            this.Controls.Add(this.label5);
            this.Name = "PortSettingForm";
            this.Text = "Port 설정";
            ((System.ComponentModel.ISupportInitialize)(this.m_numericUpDownPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown m_numericUpDownPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button m_buttonApply;
        private System.Windows.Forms.Button m_buttonCancel;
    }
}