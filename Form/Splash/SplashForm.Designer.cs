namespace AKI.Form.Splash
{
    partial class SplashForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashForm));
            this.timerLoadding = new System.Windows.Forms.Timer(this.components);
            this.pictureBoxLoadding = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoadding)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timerLoadding.Enabled = true;
            this.timerLoadding.Tick += new System.EventHandler(this.timerLoadding_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBoxLoadding.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxLoadding.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBoxLoadding.Location = new System.Drawing.Point(33, 31);
            this.pictureBoxLoadding.Name = "pictureBox1";
            this.pictureBoxLoadding.Size = new System.Drawing.Size(131, 127);
            this.pictureBoxLoadding.TabIndex = 0;
            this.pictureBoxLoadding.TabStop = false;
            this.pictureBoxLoadding.UseWaitCursor = true;
            // 
            // SplashForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.ClientSize = new System.Drawing.Size(204, 194);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBoxLoadding);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplashForm";
            this.Opacity = 0.5D;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Loading";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.White;
            this.UseWaitCursor = true;
            this.VisibleChanged += new System.EventHandler(this.SplashForm_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoadding)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerLoadding;
        private System.Windows.Forms.PictureBox pictureBoxLoadding;
    }
}