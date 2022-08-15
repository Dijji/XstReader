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
            this.PropDescWebView = new Krypton.Toolkit.KryptonWebBrowser();
            this.ElementNameLabel = new Krypton.Toolkit.KryptonLabel();
            this.ElementTypeLabel = new Krypton.Toolkit.KryptonLabel();
            this.KryptonToolStrip = new Krypton.Toolkit.KryptonToolStrip();
            this.SaveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesSplitContainer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesSplitContainer.Panel1)).BeginInit();
            this.PropertiesSplitContainer.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesSplitContainer.Panel2)).BeginInit();
            this.PropertiesSplitContainer.Panel2.SuspendLayout();
            this.KryptonToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // PropertiesSplitContainer
            // 
            this.PropertiesSplitContainer.BackColor = System.Drawing.SystemColors.Control;
            this.PropertiesSplitContainer.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.PropertiesSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertiesSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.PropertiesSplitContainer.Location = new System.Drawing.Point(0, 67);
            this.PropertiesSplitContainer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PropertiesSplitContainer.Name = "PropertiesSplitContainer";
            this.PropertiesSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // 
            // 
            this.PropertiesSplitContainer.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.PropertiesSplitContainer.Panel1.Controls.Add(this.PropertyGridProperties);
            this.PropertiesSplitContainer.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // 
            // 
            this.PropertiesSplitContainer.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.PropertiesSplitContainer.Panel2.Controls.Add(this.PropDescWebView);
            this.PropertiesSplitContainer.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.PropertiesSplitContainer.Size = new System.Drawing.Size(355, 418);
            this.PropertiesSplitContainer.SplitterDistance = 178;
            this.PropertiesSplitContainer.SplitterWidth = 3;
            this.PropertiesSplitContainer.TabIndex = 3;
            // 
            // PropertyGridProperties
            // 
            this.PropertyGridProperties.CategoryForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.PropertyGridProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyGridProperties.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PropertyGridProperties.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(206)))), ((int)(((byte)(230)))));
            this.PropertyGridProperties.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.PropertyGridProperties.HelpVisible = false;
            this.PropertyGridProperties.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(196)))), ((int)(((byte)(216)))));
            this.PropertyGridProperties.Location = new System.Drawing.Point(0, 0);
            this.PropertyGridProperties.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PropertyGridProperties.Name = "PropertyGridProperties";
            this.PropertyGridProperties.Size = new System.Drawing.Size(355, 178);
            this.PropertyGridProperties.TabIndex = 1;
            this.PropertyGridProperties.ToolbarVisible = false;
            // 
            // PropDescWebView
            // 
            this.PropDescWebView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropDescWebView.Location = new System.Drawing.Point(0, 0);
            this.PropDescWebView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PropDescWebView.Name = "PropDescWebView";
            this.PropDescWebView.Size = new System.Drawing.Size(355, 237);
            this.PropDescWebView.TabIndex = 2;
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
            // KryptonToolStrip
            // 
            this.KryptonToolStrip.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KryptonToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.KryptonToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.KryptonToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveToolStripButton});
            this.KryptonToolStrip.Location = new System.Drawing.Point(0, 40);
            this.KryptonToolStrip.Name = "KryptonToolStrip";
            this.KryptonToolStrip.Size = new System.Drawing.Size(355, 27);
            this.KryptonToolStrip.TabIndex = 7;
            this.KryptonToolStrip.Text = "AttachmentsToolStrip";
            // 
            // SaveToolStripButton
            // 
            this.SaveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveToolStripButton.Image = global::XstReader.App.Properties.Resources.content_save_outline;
            this.SaveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveToolStripButton.Name = "SaveToolStripButton";
            this.SaveToolStripButton.Size = new System.Drawing.Size(24, 24);
            this.SaveToolStripButton.Text = "Export to csv..";
            this.SaveToolStripButton.ToolTipText = "Export all properties to CSV file...";
            // 
            // SaveFileDialog
            // 
            this.SaveFileDialog.FileName = "Properties.csv";
            this.SaveFileDialog.Filter = "CSV files|*.csv|All files|*.*";
            // 
            // XstPropertiesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PropertiesSplitContainer);
            this.Controls.Add(this.KryptonToolStrip);
            this.Controls.Add(this.ElementNameLabel);
            this.Controls.Add(this.ElementTypeLabel);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "XstPropertiesControl";
            this.Size = new System.Drawing.Size(355, 485);
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesSplitContainer.Panel1)).EndInit();
            this.PropertiesSplitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesSplitContainer.Panel2)).EndInit();
            this.PropertiesSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesSplitContainer)).EndInit();
            this.KryptonToolStrip.ResumeLayout(false);
            this.KryptonToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Krypton.Toolkit.KryptonPropertyGrid PropertyGridProperties;
        private Krypton.Toolkit.KryptonWebBrowser PropDescWebView;
        private Krypton.Toolkit.KryptonSplitContainer PropertiesSplitContainer;
        private Krypton.Toolkit.KryptonLabel ElementNameLabel;
        private Krypton.Toolkit.KryptonLabel ElementTypeLabel;
        private Krypton.Toolkit.KryptonToolStrip KryptonToolStrip;
        private ToolStripButton SaveToolStripButton;
        private SaveFileDialog SaveFileDialog;
    }
}
