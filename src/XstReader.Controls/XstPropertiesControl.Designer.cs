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
            this.PropertyGridInfo = new System.Windows.Forms.PropertyGrid();
            this.ElementTypeLabel = new System.Windows.Forms.Label();
            this.ElementNameLabel = new System.Windows.Forms.Label();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.TabPageInfo = new System.Windows.Forms.TabPage();
            this.TabPageProperties = new System.Windows.Forms.TabPage();
            this.PropertiesSplitContainer = new System.Windows.Forms.SplitContainer();
            this.PropertyGridProperties = new System.Windows.Forms.PropertyGrid();
            this.PropDescWebView = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.TabControl.SuspendLayout();
            this.TabPageInfo.SuspendLayout();
            this.TabPageProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesSplitContainer)).BeginInit();
            this.PropertiesSplitContainer.Panel1.SuspendLayout();
            this.PropertiesSplitContainer.Panel2.SuspendLayout();
            this.PropertiesSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PropDescWebView)).BeginInit();
            this.SuspendLayout();
            // 
            // PropertyGridInfo
            // 
            this.PropertyGridInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyGridInfo.Location = new System.Drawing.Point(3, 3);
            this.PropertyGridInfo.Name = "PropertyGridInfo";
            this.PropertyGridInfo.Size = new System.Drawing.Size(392, 564);
            this.PropertyGridInfo.TabIndex = 0;
            this.PropertyGridInfo.ToolbarVisible = false;
            // 
            // ElementTypeLabel
            // 
            this.ElementTypeLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ElementTypeLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.ElementTypeLabel.Location = new System.Drawing.Point(0, 0);
            this.ElementTypeLabel.Name = "ElementTypeLabel";
            this.ElementTypeLabel.Size = new System.Drawing.Size(406, 22);
            this.ElementTypeLabel.TabIndex = 2;
            this.ElementTypeLabel.Text = "Type";
            // 
            // ElementNameLabel
            // 
            this.ElementNameLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ElementNameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ElementNameLabel.Location = new System.Drawing.Point(0, 22);
            this.ElementNameLabel.Name = "ElementNameLabel";
            this.ElementNameLabel.Size = new System.Drawing.Size(406, 22);
            this.ElementNameLabel.TabIndex = 3;
            this.ElementNameLabel.Text = "Name";
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.TabPageInfo);
            this.TabControl.Controls.Add(this.TabPageProperties);
            this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl.Location = new System.Drawing.Point(0, 44);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(406, 603);
            this.TabControl.TabIndex = 4;
            // 
            // TabPageInfo
            // 
            this.TabPageInfo.Controls.Add(this.PropertyGridInfo);
            this.TabPageInfo.Location = new System.Drawing.Point(4, 29);
            this.TabPageInfo.Name = "TabPageInfo";
            this.TabPageInfo.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageInfo.Size = new System.Drawing.Size(398, 570);
            this.TabPageInfo.TabIndex = 0;
            this.TabPageInfo.Text = "Info";
            this.TabPageInfo.UseVisualStyleBackColor = true;
            // 
            // TabPageProperties
            // 
            this.TabPageProperties.Controls.Add(this.PropertiesSplitContainer);
            this.TabPageProperties.Location = new System.Drawing.Point(4, 29);
            this.TabPageProperties.Name = "TabPageProperties";
            this.TabPageProperties.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageProperties.Size = new System.Drawing.Size(398, 570);
            this.TabPageProperties.TabIndex = 1;
            this.TabPageProperties.Text = "Properties";
            this.TabPageProperties.UseVisualStyleBackColor = true;
            // 
            // PropertiesSplitContainer
            // 
            this.PropertiesSplitContainer.BackColor = System.Drawing.SystemColors.Control;
            this.PropertiesSplitContainer.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.PropertiesSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertiesSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.PropertiesSplitContainer.Location = new System.Drawing.Point(3, 3);
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
            this.PropertiesSplitContainer.Size = new System.Drawing.Size(392, 564);
            this.PropertiesSplitContainer.SplitterDistance = 318;
            this.PropertiesSplitContainer.TabIndex = 3;
            // 
            // PropertyGridProperties
            // 
            this.PropertyGridProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyGridProperties.HelpVisible = false;
            this.PropertyGridProperties.Location = new System.Drawing.Point(0, 0);
            this.PropertyGridProperties.Name = "PropertyGridProperties";
            this.PropertyGridProperties.Size = new System.Drawing.Size(392, 318);
            this.PropertyGridProperties.TabIndex = 1;
            this.PropertyGridProperties.ToolbarVisible = false;
            // 
            // PropDescWebView
            // 
            this.PropDescWebView.CreationProperties = null;
            this.PropDescWebView.DefaultBackgroundColor = System.Drawing.SystemColors.Control;
            this.PropDescWebView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropDescWebView.Location = new System.Drawing.Point(0, 0);
            this.PropDescWebView.Name = "PropDescWebView";
            this.PropDescWebView.Size = new System.Drawing.Size(392, 242);
            this.PropDescWebView.TabIndex = 2;
            this.PropDescWebView.ZoomFactor = 0.8D;
            // 
            // XstPropertiesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TabControl);
            this.Controls.Add(this.ElementNameLabel);
            this.Controls.Add(this.ElementTypeLabel);
            this.Name = "XstPropertiesControl";
            this.Size = new System.Drawing.Size(406, 647);
            this.TabControl.ResumeLayout(false);
            this.TabPageInfo.ResumeLayout(false);
            this.TabPageProperties.ResumeLayout(false);
            this.PropertiesSplitContainer.Panel1.ResumeLayout(false);
            this.PropertiesSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesSplitContainer)).EndInit();
            this.PropertiesSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PropDescWebView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PropertyGrid PropertyGridInfo;
        private Label ElementTypeLabel;
        private Label ElementNameLabel;
        private TabControl TabControl;
        private TabPage TabPageInfo;
        private TabPage TabPageProperties;
        private PropertyGrid PropertyGridProperties;
        private Microsoft.Web.WebView2.WinForms.WebView2 PropDescWebView;
        private SplitContainer PropertiesSplitContainer;
    }
}
