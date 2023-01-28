namespace AKI.Control.ListBox
{
    partial class ImageListBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_buttonPreviously = new System.Windows.Forms.Button();
            this.m_buttonNext = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_buttonPreviously
            // 
            this.m_buttonPreviously.BackColor = System.Drawing.Color.Transparent;
            this.m_buttonPreviously.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.m_buttonPreviously.Location = new System.Drawing.Point(3, 183);
            this.m_buttonPreviously.Name = "m_buttonPreviously";
            this.m_buttonPreviously.Size = new System.Drawing.Size(75, 47);
            this.m_buttonPreviously.TabIndex = 0;
            this.m_buttonPreviously.Text = "<<";
            this.m_buttonPreviously.UseVisualStyleBackColor = false;
            this.m_buttonPreviously.Click += new System.EventHandler(this.m_buttonPreviously_Click);
            // 
            // m_buttonNext
            // 
            this.m_buttonNext.BackColor = System.Drawing.Color.Transparent;
            this.m_buttonNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.m_buttonNext.Location = new System.Drawing.Point(261, 183);
            this.m_buttonNext.Name = "m_buttonNext";
            this.m_buttonNext.Size = new System.Drawing.Size(75, 47);
            this.m_buttonNext.TabIndex = 0;
            this.m_buttonNext.Text = ">>";
            this.m_buttonNext.UseVisualStyleBackColor = false;
            // 
            // ImageListBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_buttonNext);
            this.Controls.Add(this.m_buttonPreviously);
            this.Name = "ImageListBox";
            this.Size = new System.Drawing.Size(339, 429);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button m_buttonPreviously;
        private System.Windows.Forms.Button m_buttonNext;
    }
}
