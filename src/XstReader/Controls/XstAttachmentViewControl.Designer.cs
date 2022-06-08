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
            this.WebView2 = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)(this.WebView2)).BeginInit();
            this.SuspendLayout();
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
            this.WebView2.Size = new System.Drawing.Size(527, 525);
            this.WebView2.TabIndex = 2;
            this.WebView2.ZoomFactor = 1D;
            // 
            // XstAttachmentViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.WebView2);
            this.Name = "XstAttachmentViewControl";
            this.Size = new System.Drawing.Size(527, 525);
            ((System.ComponentModel.ISupportInitialize)(this.WebView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 WebView2;
    }
}
