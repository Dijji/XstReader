namespace XstReader.App.Controls
{
    partial class XstPropertiesControl
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
            this.PropertiesSplitContainer = new Krypton.Toolkit.KryptonSplitContainer();
            this.PropertyGridProperties = new Krypton.Toolkit.KryptonPropertyGrid();
            this.PropDescWebView = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.ElementNameLabel = new Krypton.Toolkit.KryptonLabel();
            this.ElementTypeLabel = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesSplitContainer)).BeginInit();
            this.PropertiesSplitContainer.Panel1.SuspendLayout();
            this.PropertiesSplitContainer.Panel2.SuspendLayout();
            this.PropertiesSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PropDescWebView)).BeginInit();
            this.SuspendLayout();
            // 
            // PropertiesSplitContainer
            // 
            this.PropertiesSplitContainer.BackColor = System.Drawing.SystemColors.Control;
            this.PropertiesSplitContainer.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.PropertiesSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertiesSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.PropertiesSplitContainer.Location = new System.Drawing.Point(0, 40);
            this.PropertiesSplitContainer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PropertiesSplitContainer.Name = "PropertiesSplitContainer";
            this.PropertiesSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // PropertiesSplitContainer.Panel1
            // 
            this.PropertiesSplitContainer.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.PropertiesSplitContainer.Panel1.Controls.Add(this.PropertyGridProperties);
            this.PropertiesSplitContainer.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // PropertiesSplitContainer.Panel2
            // 
            this.PropertiesSplitContainer.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.PropertiesSplitContainer.Panel2.Controls.Add(this.PropDescWebView);
            this.PropertiesSplitContainer.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.PropertiesSplitContainer.Size = new System.Drawing.Size(355, 445);
            this.PropertiesSplitContainer.SplitterDistance = 201;
            this.PropertiesSplitContainer.SplitterWidth = 3;
            this.PropertiesSplitContainer.TabIndex = 3;
            // 
            // PropertyGridProperties
            // 
            this.PropertyGridProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyGridProperties.HelpVisible = false;
            this.PropertyGridProperties.Location = new System.Drawing.Point(0, 0);
            this.PropertyGridProperties.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PropertyGridProperties.Name = "PropertyGridProperties";
            this.PropertyGridProperties.Size = new System.Drawing.Size(355, 201);
            this.PropertyGridProperties.TabIndex = 1;
            this.PropertyGridProperties.ToolbarVisible = false;
            // 
            // PropDescWebView
            // 
            this.PropDescWebView.AllowExternalDrop = true;
            this.PropDescWebView.CreationProperties = null;
            this.PropDescWebView.DefaultBackgroundColor = System.Drawing.SystemColors.Control;
            this.PropDescWebView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropDescWebView.Location = new System.Drawing.Point(0, 0);
            this.PropDescWebView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PropDescWebView.Name = "PropDescWebView";
            this.PropDescWebView.Size = new System.Drawing.Size(355, 241);
            this.PropDescWebView.TabIndex = 2;
            this.PropDescWebView.ZoomFactor = 0.8D;
            // 
            // ElementNameLabel
            // 
            this.ElementNameLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ElementNameLabel.LabelStyle = Krypton.Toolkit.LabelStyle.BoldPanel;
            this.ElementNameLabel.Location = new System.Drawing.Point(0, 20);
            this.ElementNameLabel.Name = "ElementNameLabel";
            this.ElementNameLabel.Size = new System.Drawing.Size(355, 20);
            this.ElementNameLabel.TabIndex = 6;
            this.ElementNameLabel.Values.Text = "Type";
            // 
            // ElementTypeLabel
            // 
            this.ElementTypeLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ElementTypeLabel.LabelStyle = Krypton.Toolkit.LabelStyle.ItalicControl;
            this.ElementTypeLabel.Location = new System.Drawing.Point(0, 0);
            this.ElementTypeLabel.Name = "ElementTypeLabel";
            this.ElementTypeLabel.Size = new System.Drawing.Size(355, 20);
            this.ElementTypeLabel.TabIndex = 5;
            this.ElementTypeLabel.Values.Text = "Type";
            // 
            // XstPropertiesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PropertiesSplitContainer);
            this.Controls.Add(this.ElementNameLabel);
            this.Controls.Add(this.ElementTypeLabel);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "XstPropertiesControl";
            this.Size = new System.Drawing.Size(355, 485);
            this.PropertiesSplitContainer.Panel1.ResumeLayout(false);
            this.PropertiesSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesSplitContainer)).EndInit();
            this.PropertiesSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PropDescWebView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Krypton.Toolkit.KryptonPropertyGrid PropertyGridProperties;
        private Microsoft.Web.WebView2.WinForms.WebView2 PropDescWebView;
        private Krypton.Toolkit.KryptonSplitContainer PropertiesSplitContainer;
        private Krypton.Toolkit.KryptonLabel ElementNameLabel;
        private Krypton.Toolkit.KryptonLabel ElementTypeLabel;
    }
}
