namespace XstReader.App.Controls
{
    partial class XstMessageContentViewControl
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
            this.MainKryptonPanel = new Krypton.Toolkit.KryptonPanel();
            this.KryptonRichTextBox = new Krypton.Toolkit.KryptonRichTextBox();
            this.WebView2 = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.KryptonDockingManager = new Krypton.Docking.KryptonDockingManager();
            ((System.ComponentModel.ISupportInitialize)(this.MainKryptonPanel)).BeginInit();
            this.MainKryptonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WebView2)).BeginInit();
            this.SuspendLayout();
            // 
            // MainKryptonPanel
            // 
            this.MainKryptonPanel.Controls.Add(this.KryptonRichTextBox);
            this.MainKryptonPanel.Controls.Add(this.WebView2);
            this.MainKryptonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainKryptonPanel.Location = new System.Drawing.Point(0, 0);
            this.MainKryptonPanel.Name = "MainKryptonPanel";
            this.MainKryptonPanel.Size = new System.Drawing.Size(435, 457);
            this.MainKryptonPanel.TabIndex = 1;
            // 
            // KryptonRichTextBox
            // 
            this.KryptonRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KryptonRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.KryptonRichTextBox.Name = "KryptonRichTextBox";
            this.KryptonRichTextBox.ReadOnly = true;
            this.KryptonRichTextBox.Size = new System.Drawing.Size(435, 457);
            this.KryptonRichTextBox.TabIndex = 2;
            this.KryptonRichTextBox.Text = "";
            // 
            // WebView2
            // 
            this.WebView2.AllowExternalDrop = true;
            this.WebView2.CreationProperties = null;
            this.WebView2.DefaultBackgroundColor = System.Drawing.Color.White;
            this.WebView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WebView2.Location = new System.Drawing.Point(0, 0);
            this.WebView2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.WebView2.Name = "WebView2";
            this.WebView2.Size = new System.Drawing.Size(435, 457);
            this.WebView2.TabIndex = 1;
            this.WebView2.ZoomFactor = 1D;
            // 
            // XstMessageContentViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainKryptonPanel);
            this.Name = "XstMessageContentViewControl";
            this.Size = new System.Drawing.Size(435, 457);
            ((System.ComponentModel.ISupportInitialize)(this.MainKryptonPanel)).EndInit();
            this.MainKryptonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.WebView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel MainKryptonPanel;
        private Krypton.Toolkit.KryptonRichTextBox KryptonRichTextBox;
        private Microsoft.Web.WebView2.WinForms.WebView2 WebView2;
        private Krypton.Docking.KryptonDockingManager KryptonDockingManager;
    }
}
