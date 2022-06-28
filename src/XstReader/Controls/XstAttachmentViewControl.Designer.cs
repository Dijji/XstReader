namespace XstReader.App.Controls
{
    partial class XstAttachmentViewControl
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
            this.KryptonWebBrowser = new Krypton.Toolkit.KryptonWebBrowser();
            this.SuspendLayout();
            // 
            // KryptonWebBrowser
            // 
            this.KryptonWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KryptonWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.KryptonWebBrowser.Name = "KryptonWebBrowser";
            this.KryptonWebBrowser.Size = new System.Drawing.Size(527, 525);
            this.KryptonWebBrowser.TabIndex = 3;
            // 
            // XstAttachmentViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.KryptonWebBrowser);
            this.Name = "XstAttachmentViewControl";
            this.Size = new System.Drawing.Size(527, 525);
            this.ResumeLayout(false);

        }

        #endregion
        private Krypton.Toolkit.KryptonWebBrowser KryptonWebBrowser;
    }
}
