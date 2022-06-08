namespace XstReader.App.Controls
{
    partial class XstAttachmentListControl
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
            this.ObjectListView = new BrightIdeasSoftware.ObjectListView();
            this.KryptonToolStrip = new Krypton.Toolkit.KryptonToolStrip();
            this.SaveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.SaveAllToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.OpenInAppToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.OpenWithToolStripMenuItem = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectListView)).BeginInit();
            this.KryptonToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ObjectListView
            // 
            this.ObjectListView.AllowColumnReorder = true;
            this.ObjectListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ObjectListView.EmptyListMsg = "";
            this.ObjectListView.FullRowSelect = true;
            this.ObjectListView.GridLines = true;
            this.ObjectListView.IncludeColumnHeadersInCopy = true;
            this.ObjectListView.Location = new System.Drawing.Point(0, 27);
            this.ObjectListView.MultiSelect = false;
            this.ObjectListView.Name = "ObjectListView";
            this.ObjectListView.ShowGroups = false;
            this.ObjectListView.ShowItemToolTips = true;
            this.ObjectListView.Size = new System.Drawing.Size(507, 191);
            this.ObjectListView.TabIndex = 1;
            this.ObjectListView.UseFilterIndicator = true;
            this.ObjectListView.UseFiltering = true;
            this.ObjectListView.View = System.Windows.Forms.View.Details;
            // 
            // KryptonToolStrip
            // 
            this.KryptonToolStrip.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KryptonToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.KryptonToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.KryptonToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveToolStripButton,
            this.SaveAllToolStripButton,
            this.toolStripSeparator1,
            this.OpenInAppToolStripButton,
            this.OpenWithToolStripMenuItem});
            this.KryptonToolStrip.Location = new System.Drawing.Point(0, 0);
            this.KryptonToolStrip.Name = "KryptonToolStrip";
            this.KryptonToolStrip.Size = new System.Drawing.Size(507, 27);
            this.KryptonToolStrip.TabIndex = 2;
            this.KryptonToolStrip.Text = "AttachmentsToolStrip";
            // 
            // SaveToolStripButton
            // 
            this.SaveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveToolStripButton.Image = global::XstReader.App.Controls.Properties.Resources.content_save;
            this.SaveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveToolStripButton.Name = "SaveToolStripButton";
            this.SaveToolStripButton.Size = new System.Drawing.Size(24, 24);
            this.SaveToolStripButton.Text = "Save as..";
            this.SaveToolStripButton.ToolTipText = "Save selected Attachment as...";
            // 
            // SaveAllToolStripButton
            // 
            this.SaveAllToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveAllToolStripButton.Image = global::XstReader.App.Controls.Properties.Resources.content_save_all;
            this.SaveAllToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveAllToolStripButton.Name = "SaveAllToolStripButton";
            this.SaveAllToolStripButton.Size = new System.Drawing.Size(24, 24);
            this.SaveAllToolStripButton.Text = "Save all";
            this.SaveAllToolStripButton.ToolTipText = "Save all Attachments";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // OpenInAppToolStripButton
            // 
            this.OpenInAppToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.OpenInAppToolStripButton.Image = global::XstReader.App.Controls.Properties.Resources.open_in_app;
            this.OpenInAppToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenInAppToolStripButton.Name = "OpenInAppToolStripButton";
            this.OpenInAppToolStripButton.Size = new System.Drawing.Size(24, 24);
            this.OpenInAppToolStripButton.Text = "Open in XstReader";
            // 
            // OpenWithToolStripMenuItem
            // 
            this.OpenWithToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.OpenWithToolStripMenuItem.Image = global::XstReader.App.Controls.Properties.Resources.open_in_new;
            this.OpenWithToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenWithToolStripMenuItem.Name = "OpenWithToolStripMenuItem";
            this.OpenWithToolStripMenuItem.Size = new System.Drawing.Size(24, 24);
            this.OpenWithToolStripMenuItem.Text = "Open with...";
            // 
            // XstAttachmentListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ObjectListView);
            this.Controls.Add(this.KryptonToolStrip);
            this.MinimumSize = new System.Drawing.Size(300, 100);
            this.Name = "XstAttachmentListControl";
            this.Size = new System.Drawing.Size(507, 218);
            ((System.ComponentModel.ISupportInitialize)(this.ObjectListView)).EndInit();
            this.KryptonToolStrip.ResumeLayout(false);
            this.KryptonToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView ObjectListView;
        private Krypton.Toolkit.KryptonToolStrip KryptonToolStrip;
        private ToolStripButton SaveToolStripButton;
        private ToolStripButton SaveAllToolStripButton;
        private SaveFileDialog SaveFileDialog;
        private FolderBrowserDialog FolderBrowserDialog;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton OpenInAppToolStripButton;
        private ToolStripButton OpenWithToolStripMenuItem;
    }
}
