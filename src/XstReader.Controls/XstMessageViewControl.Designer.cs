namespace XstReader.App.Controls
{
    partial class XstMessageViewControl
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
            this.MessageLabel = new System.Windows.Forms.Label();
            this.FormatLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.WebView2)).BeginInit();
            this.SuspendLayout();
            // 
            // WebView2
            // 
            this.WebView2.AllowExternalDrop = true;
            this.WebView2.CreationProperties = null;
            this.WebView2.DefaultBackgroundColor = System.Drawing.Color.White;
            this.WebView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WebView2.Location = new System.Drawing.Point(0, 30);
            this.WebView2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.WebView2.Name = "WebView2";
            this.WebView2.Size = new System.Drawing.Size(588, 416);
            this.WebView2.TabIndex = 0;
            this.WebView2.ZoomFactor = 1D;
            // 
            // MessageLabel
            // 
            this.MessageLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.MessageLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.MessageLabel.Location = new System.Drawing.Point(0, 15);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(588, 15);
            this.MessageLabel.TabIndex = 1;
            // 
            // FormatLabel
            // 
            this.FormatLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.FormatLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.FormatLabel.Location = new System.Drawing.Point(0, 0);
            this.FormatLabel.Name = "FormatLabel";
            this.FormatLabel.Size = new System.Drawing.Size(588, 15);
            this.FormatLabel.TabIndex = 2;
            // 
            // XstMessageViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.WebView2);
            this.Controls.Add(this.MessageLabel);
            this.Controls.Add(this.FormatLabel);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "XstMessageViewControl";
            this.Size = new System.Drawing.Size(588, 446);
            ((System.ComponentModel.ISupportInitialize)(this.WebView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 WebView2;
        private Label MessageLabel;
        private Label FormatLabel;
    }
}
