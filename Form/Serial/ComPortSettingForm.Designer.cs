namespace AKI.Form.Serial
{
    partial class ComPortSettingForm
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
            ((System.ComponentModel.ISupportInitialize)(this.m_numericUpDownComPort)).BeginInit();
            this.SuspendLayout();
            // 
            // m_buttonApply
            // 
            this.m_buttonApply.Location = new System.Drawing.Point(13, 47);
            this.m_buttonApply.Name = "m_buttonApply";
            this.m_buttonApply.Size = new System.Drawing.Size(75, 23);
            this.m_buttonApply.TabIndex = 1;
            this.m_buttonApply.Text = "적용";
            this.m_buttonApply.UseVisualStyleBackColor = true;
            this.m_buttonApply.Click += new System.EventHandler(this.m_buttonApply_Click);
            // 
            // m_buttonCancel
            // 
            this.m_buttonCancel.Location = new System.Drawing.Point(94, 47);
            this.m_buttonCancel.Name = "m_buttonCancel";
            this.m_buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.m_buttonCancel.TabIndex = 2;
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
            this.m_numericUpDownComPort.Location = new System.Drawing.Point(94, 13);
            this.m_numericUpDownComPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_numericUpDownComPort.Name = "m_numericUpDownComPort";
            this.m_numericUpDownComPort.Size = new System.Drawing.Size(75, 21);
            this.m_numericUpDownComPort.TabIndex = 0;
            this.m_numericUpDownComPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ComPortSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(182, 82);
            this.ControlBox = false;
            this.Controls.Add(this.m_numericUpDownComPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_buttonCancel);
            this.Controls.Add(this.m_buttonApply);
            this.Name = "ComPortSettingForm";
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
    }
}