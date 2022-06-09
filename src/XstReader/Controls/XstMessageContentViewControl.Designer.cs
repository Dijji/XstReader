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
            this.WebView2 = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.KryptonDockingManager = new Krypton.Docking.KryptonDockingManager();
            this.KryptonToolStrip = new Krypton.Toolkit.KryptonToolStrip();
            this.ExportPdfToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.MainKryptonPanel)).BeginInit();
            this.MainKryptonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WebView2)).BeginInit();
            this.KryptonToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainKryptonPanel
            // 
            this.MainKryptonPanel.Controls.Add(this.WebView2);
            this.MainKryptonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainKryptonPanel.Location = new System.Drawing.Point(0, 31);
            this.MainKryptonPanel.Name = "MainKryptonPanel";
            this.MainKryptonPanel.Size = new System.Drawing.Size(435, 426);
            this.MainKryptonPanel.TabIndex = 1;
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
            this.WebView2.Size = new System.Drawing.Size(435, 426);
            this.WebView2.TabIndex = 1;
            this.WebView2.ZoomFactor = 1D;
            // 
            // KryptonToolStrip
            // 
            this.KryptonToolStrip.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KryptonToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.KryptonToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.KryptonToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExportPdfToolStripButton});
            this.KryptonToolStrip.Location = new System.Drawing.Point(0, 0);
            this.KryptonToolStrip.Name = "KryptonToolStrip";
            this.KryptonToolStrip.Size = new System.Drawing.Size(435, 31);
            this.KryptonToolStrip.TabIndex = 2;
            this.KryptonToolStrip.Text = "kryptonToolStrip1";
            // 
            // ExportPdfToolStripButton
            // 
            this.ExportPdfToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ExportPdfToolStripButton.Image = global::XstReader.App.Properties.Resources.file_pdf_box;
            this.ExportPdfToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExportPdfToolStripButton.Name = "ExportPdfToolStripButton";
            this.ExportPdfToolStripButton.Size = new System.Drawing.Size(28, 28);
            this.ExportPdfToolStripButton.Text = "Export to PDF";
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
            ((System.ComponentModel.ISupportInitialize)(this.WebView2)).EndInit();
            this.KryptonToolStrip.ResumeLayout(false);
            this.KryptonToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel MainKryptonPanel;
        private Microsoft.Web.WebView2.WinForms.WebView2 WebView2;
        private Krypton.Docking.KryptonDockingManager KryptonDockingManager;
        private Krypton.Toolkit.KryptonToolStrip KryptonToolStrip;
        private ToolStripButton ExportPdfToolStripButton;
        private SaveFileDialog SaveFileDialog;
    }
}
