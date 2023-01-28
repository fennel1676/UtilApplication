namespace AKI.Control.List
{
    partial class ControlListBox
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
            this.m_vScrollBar = new System.Windows.Forms.VScrollBar();
            this.m_tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // m_vScrollBar
            // 
            this.m_vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.m_vScrollBar.LargeChange = 1;
            this.m_vScrollBar.Location = new System.Drawing.Point(437, 0);
            this.m_vScrollBar.Maximum = 1;
            this.m_vScrollBar.Name = "m_vScrollBar";
            this.m_vScrollBar.Size = new System.Drawing.Size(30, 396);
            this.m_vScrollBar.TabIndex = 1;
            this.m_vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.m_vScrollBar_Scroll);
            // 
            // m_tableLayoutPanel
            // 
            this.m_tableLayoutPanel.ColumnCount = 1;
            this.m_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.m_tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.m_tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.m_tableLayoutPanel.Name = "m_tableLayoutPanel";
            this.m_tableLayoutPanel.RowCount = 1;
            this.m_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.m_tableLayoutPanel.Size = new System.Drawing.Size(437, 396);
            this.m_tableLayoutPanel.TabIndex = 2;
            // 
            // ControlListBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_tableLayoutPanel);
            this.Controls.Add(this.m_vScrollBar);
            this.Name = "ControlListBox";
            this.Size = new System.Drawing.Size(467, 396);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar m_vScrollBar;
        private System.Windows.Forms.TableLayoutPanel m_tableLayoutPanel;
    }
}
