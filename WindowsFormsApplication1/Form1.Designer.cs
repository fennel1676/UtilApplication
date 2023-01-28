namespace WindowsFormsApplication1
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new AKI.Control.Button.ImageButton();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox1 = new AKI.Control.TextBox.TextBoxExEndKey();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.m_textBoxFolder = new System.Windows.Forms.TextBox();
            this.m_folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.m_buttonFolder = new System.Windows.Forms.Button();
            this.m_textBoxTop = new System.Windows.Forms.TextBox();
            this.m_textBoxBottom = new System.Windows.Forms.TextBox();
            this.m_buttonRun = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::WindowsFormsApplication1.Properties.Resources.캡처;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(106, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(166, 110);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.TextColor = System.Drawing.Color.Empty;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(13, 43);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(13, 73);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(13, 103);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(13, 132);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 4;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // textBox1
            // 
            this.textBox1.EndKey = System.Windows.Forms.Keys.Tab;
            this.textBox1.Location = new System.Drawing.Point(106, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(155, 21);
            this.textBox1.TabIndex = 5;
            this.textBox1.Event_EndKey += new AKI.Control.TextBox.TextBoxExEndKey.delegateEndKey(this.textBox1_Event_EndKey);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(106, 156);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 21);
            this.textBox2.TabIndex = 6;
            // 
            // m_textBoxFolder
            // 
            this.m_textBoxFolder.Location = new System.Drawing.Point(13, 254);
            this.m_textBoxFolder.Name = "m_textBoxFolder";
            this.m_textBoxFolder.Size = new System.Drawing.Size(178, 21);
            this.m_textBoxFolder.TabIndex = 7;
            // 
            // m_buttonFolder
            // 
            this.m_buttonFolder.Location = new System.Drawing.Point(197, 253);
            this.m_buttonFolder.Name = "m_buttonFolder";
            this.m_buttonFolder.Size = new System.Drawing.Size(75, 23);
            this.m_buttonFolder.TabIndex = 8;
            this.m_buttonFolder.Text = "폴더 선택";
            this.m_buttonFolder.UseVisualStyleBackColor = true;
            this.m_buttonFolder.Click += new System.EventHandler(this.m_buttonFolder_Click);
            // 
            // m_textBoxTop
            // 
            this.m_textBoxTop.Location = new System.Drawing.Point(12, 281);
            this.m_textBoxTop.Name = "m_textBoxTop";
            this.m_textBoxTop.Size = new System.Drawing.Size(76, 21);
            this.m_textBoxTop.TabIndex = 7;
            // 
            // m_textBoxBottom
            // 
            this.m_textBoxBottom.Location = new System.Drawing.Point(115, 281);
            this.m_textBoxBottom.Name = "m_textBoxBottom";
            this.m_textBoxBottom.Size = new System.Drawing.Size(76, 21);
            this.m_textBoxBottom.TabIndex = 7;
            // 
            // m_buttonRun
            // 
            this.m_buttonRun.Location = new System.Drawing.Point(198, 281);
            this.m_buttonRun.Name = "m_buttonRun";
            this.m_buttonRun.Size = new System.Drawing.Size(75, 23);
            this.m_buttonRun.TabIndex = 9;
            this.m_buttonRun.Text = "진행";
            this.m_buttonRun.UseVisualStyleBackColor = true;
            this.m_buttonRun.Click += new System.EventHandler(this.m_buttonRun_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(12, 161);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 4;
            this.button6.Text = "button6";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 363);
            this.Controls.Add(this.m_buttonRun);
            this.Controls.Add(this.m_buttonFolder);
            this.Controls.Add(this.m_textBoxBottom);
            this.Controls.Add(this.m_textBoxTop);
            this.Controls.Add(this.m_textBoxFolder);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AKI.Control.Button.ImageButton button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private AKI.Control.TextBox.TextBoxExEndKey textBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox m_textBoxFolder;
        private System.Windows.Forms.FolderBrowserDialog m_folderBrowserDialog;
        private System.Windows.Forms.Button m_buttonFolder;
        private System.Windows.Forms.TextBox m_textBoxTop;
        private System.Windows.Forms.TextBox m_textBoxBottom;
        private System.Windows.Forms.Button m_buttonRun;
        private System.Windows.Forms.Button button6;
    }
}

