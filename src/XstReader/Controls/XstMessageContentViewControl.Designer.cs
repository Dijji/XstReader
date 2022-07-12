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
            this.KryptonWebBrowser = new Krypton.Toolkit.KryptonWebBrowser();
            this.KryptonDockingManager = new Krypton.Docking.KryptonDockingManager();
            this.KryptonToolStrip = new Krypton.Toolkit.KryptonToolStrip();
            this.ExportHtmlToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.PrintToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.MainKryptonPanel)).BeginInit();
            this.MainKryptonPanel.SuspendLayout();
            this.KryptonToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainKryptonPanel
            // 
            this.MainKryptonPanel.Controls.Add(this.KryptonWebBrowser);
            this.MainKryptonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainKryptonPanel.Location = new System.Drawing.Point(0, 31);
            this.MainKryptonPanel.Name = "MainKryptonPanel";
            this.MainKryptonPanel.Size = new System.Drawing.Size(435, 426);
            this.MainKryptonPanel.TabIndex = 1;
            // 
            // KryptonWebBrowser
            // 
            this.KryptonWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KryptonWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.KryptonWebBrowser.Name = "KryptonWebBrowser";
            this.KryptonWebBrowser.Size = new System.Drawing.Size(435, 426);
            this.KryptonWebBrowser.TabIndex = 3;
            // 
            // KryptonToolStrip
            // 
            this.KryptonToolStrip.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KryptonToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.KryptonToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.KryptonToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExportHtmlToolStripButton,
            this.PrintToolStripButton});
            this.KryptonToolStrip.Location = new System.Drawing.Point(0, 0);
            this.KryptonToolStrip.Name = "KryptonToolStrip";
            this.KryptonToolStrip.Size = new System.Drawing.Size(435, 31);
            this.KryptonToolStrip.TabIndex = 2;
            this.KryptonToolStrip.Text = "kryptonToolStrip1";
            // 
            // ExportHtmlToolStripButton
            // 
            this.ExportHtmlToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ExportHtmlToolStripButton.Image = global::XstReader.App.Properties.Resources.content_save;
            this.ExportHtmlToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExportHtmlToolStripButton.Name = "ExportHtmlToolStripButton";
            this.ExportHtmlToolStripButton.Size = new System.Drawing.Size(28, 28);
            this.ExportHtmlToolStripButton.Text = "Export as single Html";
            this.ExportHtmlToolStripButton.ToolTipText = "Export as single Html";
            // 
            // PrintToolStripButton
            // 
            this.PrintToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PrintToolStripButton.Image = global::XstReader.App.Properties.Resources.printer;
            this.PrintToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PrintToolStripButton.Name = "PrintToolStripButton";
            this.PrintToolStripButton.Size = new System.Drawing.Size(28, 28);
            this.PrintToolStripButton.Text = "Print...";
            // 
            // XstMessageContentViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainKryptonPanel);
            this.Controls.Add(this.KryptonToolStrip);
            this.Name = "XstMessageContentViewControl";
            this.Size = new System.Drawing.Size(435, 457);
            ((System.ComponentModel.ISupportInitialize)(this.MainKryptonPanel)).EndInit();
            this.MainKryptonPanel.ResumeLayout(false);
            this.KryptonToolStrip.ResumeLayout(false);
            this.KryptonToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel MainKryptonPanel;
        private Krypton.Docking.KryptonDockingManager KryptonDockingManager;
        private Krypton.Toolkit.KryptonToolStrip KryptonToolStrip;
        private SaveFileDialog SaveFileDialog;
        private Krypton.Toolkit.KryptonWebBrowser KryptonWebBrowser;
        private ToolStripButton ExportHtmlToolStripButton;
        private ToolStripButton PrintToolStripButton;
    }
}
