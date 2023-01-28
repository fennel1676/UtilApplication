namespace AKI.Form.Ethernet
{
    partial class IpPortSettingForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.m_numericUpDownIP1 = new System.Windows.Forms.NumericUpDown();
            this.m_numericUpDownIP2 = new System.Windows.Forms.NumericUpDown();
            this.m_numericUpDownIP3 = new System.Windows.Forms.NumericUpDown();
            this.m_numericUpDownIP4 = new System.Windows.Forms.NumericUpDown();
            this.m_numericUpDownPort = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.m_buttonApply = new System.Windows.Forms.Button();
            this.m_buttonCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.m_numericUpDownIP1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numericUpDownIP2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numericUpDownIP3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numericUpDownIP4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numericUpDownPort)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP :";
            // 
            // m_numericUpDownIP1
            // 
            this.m_numericUpDownIP1.Location = new System.Drawing.Point(78, 11);
            this.m_numericUpDownIP1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.m_numericUpDownIP1.Name = "m_numericUpDownIP1";
            this.m_numericUpDownIP1.Size = new System.Drawing.Size(58, 21);
            this.m_numericUpDownIP1.TabIndex = 0;
            this.m_numericUpDownIP1.Value = new decimal(new int[] {
            127,
            0,
            0,
            0});
            // 
            // m_numericUpDownIP2
            // 
            this.m_numericUpDownIP2.Location = new System.Drawing.Point(149, 11);
            this.m_numericUpDownIP2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.m_numericUpDownIP2.Name = "m_numericUpDownIP2";
            this.m_numericUpDownIP2.Size = new System.Drawing.Size(58, 21);
            this.m_numericUpDownIP2.TabIndex = 1;
            // 
            // m_numericUpDownIP3
            // 
            this.m_numericUpDownIP3.Location = new System.Drawing.Point(220, 11);
            this.m_numericUpDownIP3.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.m_numericUpDownIP3.Name = "m_numericUpDownIP3";
            this.m_numericUpDownIP3.Size = new System.Drawing.Size(58, 21);
            this.m_numericUpDownIP3.TabIndex = 2;
            // 
            // m_numericUpDownIP4
            // 
            this.m_numericUpDownIP4.Location = new System.Drawing.Point(291, 11);
            this.m_numericUpDownIP4.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.m_numericUpDownIP4.Name = "m_numericUpDownIP4";
            this.m_numericUpDownIP4.Size = new System.Drawing.Size(58, 21);
            this.m_numericUpDownIP4.TabIndex = 3;
            this.m_numericUpDownIP4.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // m_numericUpDownPort
            // 
            this.m_numericUpDownPort.Location = new System.Drawing.Point(78, 38);
            this.m_numericUpDownPort.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.m_numericUpDownPort.Name = "m_numericUpDownPort";
            this.m_numericUpDownPort.Size = new System.Drawing.Size(129, 21);
            this.m_numericUpDownPort.TabIndex = 4;
            this.m_numericUpDownPort.Value = new decimal(new int[] {
            4444,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(138, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(9, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = ".";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(209, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(9, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = ".";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(280, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(9, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = ".";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "Port :";
            // 
            // m_buttonApply
            // 
            this.m_buttonApply.Location = new System.Drawing.Point(193, 65);
            this.m_buttonApply.Name = "m_buttonApply";
            this.m_buttonApply.Size = new System.Drawing.Size(75, 23);
            this.m_buttonApply.TabIndex = 5;
            this.m_buttonApply.Text = "적용";
            this.m_buttonApply.UseVisualStyleBackColor = true;
            this.m_buttonApply.Click += new System.EventHandler(this.m_buttonApply_Click);
            // 
            // m_buttonCancel
            // 
            this.m_buttonCancel.Location = new System.Drawing.Point(274, 66);
            this.m_buttonCancel.Name = "m_buttonCancel";
            this.m_buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.m_buttonCancel.TabIndex = 6;
            this.m_buttonCancel.Text = "취소";
            this.m_buttonCancel.UseVisualStyleBackColor = true;
            this.m_buttonCancel.Click += new System.EventHandler(this.m_buttonCancel_Click);
            // 
            // IpPortSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 96);
            this.Controls.Add(this.m_buttonCancel);
            this.Controls.Add(this.m_buttonApply);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_numericUpDownIP4);
            this.Controls.Add(this.m_numericUpDownIP3);
            this.Controls.Add(this.m_numericUpDownIP2);
            this.Controls.Add(this.m_numericUpDownPort);
            this.Controls.Add(this.m_numericUpDownIP1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Name = "IpPortSettingForm";
            this.Text = "IP/Port 설정";
            ((System.ComponentModel.ISupportInitialize)(this.m_numericUpDownIP1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numericUpDownIP2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numericUpDownIP3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numericUpDownIP4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numericUpDownPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown m_numericUpDownIP1;
        private System.Windows.Forms.NumericUpDown m_numericUpDownIP2;
        private System.Windows.Forms.NumericUpDown m_numericUpDownIP3;
        private System.Windows.Forms.NumericUpDown m_numericUpDownIP4;
        private System.Windows.Forms.NumericUpDown m_numericUpDownPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button m_buttonApply;
        private System.Windows.Forms.Button m_buttonCancel;
    }
}