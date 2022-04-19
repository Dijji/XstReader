namespace XstReader
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainMenuSrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.FolderTree = new XstReader.App.Controls.XstFolderTreeControl();
            this.SplitContainer2 = new System.Windows.Forms.SplitContainer();
            this.SplitContainer3 = new System.Windows.Forms.SplitContainer();
            this.MessageList = new XstReader.App.Controls.XstMessageListControl();
            this.SplitContainer4 = new System.Windows.Forms.SplitContainer();
            this.RecipientList = new XstReader.App.Controls.XstRecipientListControl();
            this.SplitContainer5 = new System.Windows.Forms.SplitContainer();
            this.MessageView = new XstReader.App.Controls.XstMessageViewControl();
            this.AttachmentList = new XstReader.App.Controls.XstAttachmentListControl();
            this.PropertiesControl = new XstReader.App.Controls.XstPropertiesControl();
            this.OpenXstFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.MainMenuSrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).BeginInit();
            this.SplitContainer1.Panel1.SuspendLayout();
            this.SplitContainer1.Panel2.SuspendLayout();
            this.SplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer2)).BeginInit();
            this.SplitContainer2.Panel1.SuspendLayout();
            this.SplitContainer2.Panel2.SuspendLayout();
            this.SplitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer3)).BeginInit();
            this.SplitContainer3.Panel1.SuspendLayout();
            this.SplitContainer3.Panel2.SuspendLayout();
            this.SplitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer4)).BeginInit();
            this.SplitContainer4.Panel1.SuspendLayout();
            this.SplitContainer4.Panel2.SuspendLayout();
            this.SplitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer5)).BeginInit();
            this.SplitContainer5.Panel1.SuspendLayout();
            this.SplitContainer5.Panel2.SuspendLayout();
            this.SplitContainer5.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenuSrip
            // 
            this.MainMenuSrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenuSrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.MainMenuSrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuSrip.Name = "MainMenuSrip";
            this.MainMenuSrip.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.MainMenuSrip.Size = new System.Drawing.Size(1140, 24);
            this.MainMenuSrip.TabIndex = 0;
            this.MainMenuSrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.closeFileToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open .ost or .pst file";
            // 
            // closeFileToolStripMenuItem
            // 
            this.closeFileToolStripMenuItem.Enabled = false;
            this.closeFileToolStripMenuItem.Name = "closeFileToolStripMenuItem";
            this.closeFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.closeFileToolStripMenuItem.Text = "Close file";
            // 
            // SplitContainer1
            // 
            this.SplitContainer1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.SplitContainer1.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.SplitContainer1.Location = new System.Drawing.Point(0, 24);
            this.SplitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SplitContainer1.Name = "SplitContainer1";
            // 
            // SplitContainer1.Panel1
            // 
            this.SplitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.SplitContainer1.Panel1.Controls.Add(this.FolderTree);
            this.SplitContainer1.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // SplitContainer1.Panel2
            // 
            this.SplitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.SplitContainer1.Panel2.Controls.Add(this.SplitContainer2);
            this.SplitContainer1.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.SplitContainer1.Size = new System.Drawing.Size(1140, 436);
            this.SplitContainer1.SplitterDistance = 246;
            this.SplitContainer1.TabIndex = 1;
            // 
            // FolderTree
            // 
            this.FolderTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FolderTree.Location = new System.Drawing.Point(0, 0);
            this.FolderTree.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.FolderTree.Name = "FolderTree";
            this.FolderTree.Size = new System.Drawing.Size(246, 436);
            this.FolderTree.TabIndex = 0;
            // 
            // SplitContainer2
            // 
            this.SplitContainer2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.SplitContainer2.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer2.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SplitContainer2.Name = "SplitContainer2";
            // 
            // SplitContainer2.Panel1
            // 
            this.SplitContainer2.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.SplitContainer2.Panel1.Controls.Add(this.SplitContainer3);
            this.SplitContainer2.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // SplitContainer2.Panel2
            // 
            this.SplitContainer2.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.SplitContainer2.Panel2.Controls.Add(this.PropertiesControl);
            this.SplitContainer2.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.SplitContainer2.Size = new System.Drawing.Size(890, 436);
            this.SplitContainer2.SplitterDistance = 691;
            this.SplitContainer2.TabIndex = 0;
            // 
            // SplitContainer3
            // 
            this.SplitContainer3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.SplitContainer3.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.SplitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.SplitContainer3.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SplitContainer3.Name = "SplitContainer3";
            // 
            // SplitContainer3.Panel1
            // 
            this.SplitContainer3.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.SplitContainer3.Panel1.Controls.Add(this.MessageList);
            this.SplitContainer3.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // SplitContainer3.Panel2
            // 
            this.SplitContainer3.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.SplitContainer3.Panel2.Controls.Add(this.SplitContainer4);
            this.SplitContainer3.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.SplitContainer3.Size = new System.Drawing.Size(691, 436);
            this.SplitContainer3.SplitterDistance = 444;
            this.SplitContainer3.TabIndex = 0;
            // 
            // MessageList
            // 
            this.MessageList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessageList.Location = new System.Drawing.Point(0, 0);
            this.MessageList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MessageList.Name = "MessageList";
            this.MessageList.Size = new System.Drawing.Size(444, 436);
            this.MessageList.TabIndex = 0;
            // 
            // SplitContainer4
            // 
            this.SplitContainer4.BackColor = System.Drawing.SystemColors.ControlDark;
            this.SplitContainer4.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.SplitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.SplitContainer4.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SplitContainer4.Name = "SplitContainer4";
            this.SplitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer4.Panel1
            // 
            this.SplitContainer4.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.SplitContainer4.Panel1.Controls.Add(this.RecipientList);
            this.SplitContainer4.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // SplitContainer4.Panel2
            // 
            this.SplitContainer4.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.SplitContainer4.Panel2.Controls.Add(this.SplitContainer5);
            this.SplitContainer4.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.SplitContainer4.Size = new System.Drawing.Size(243, 436);
            this.SplitContainer4.SplitterDistance = 136;
            this.SplitContainer4.SplitterWidth = 3;
            this.SplitContainer4.TabIndex = 0;
            // 
            // RecipientList
            // 
            this.RecipientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RecipientList.Location = new System.Drawing.Point(0, 0);
            this.RecipientList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.RecipientList.Name = "RecipientList";
            this.RecipientList.Size = new System.Drawing.Size(243, 136);
            this.RecipientList.TabIndex = 0;
            // 
            // SplitContainer5
            // 
            this.SplitContainer5.BackColor = System.Drawing.SystemColors.ControlDark;
            this.SplitContainer5.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.SplitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer5.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.SplitContainer5.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SplitContainer5.Name = "SplitContainer5";
            this.SplitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer5.Panel1
            // 
            this.SplitContainer5.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.SplitContainer5.Panel1.Controls.Add(this.MessageView);
            this.SplitContainer5.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // SplitContainer5.Panel2
            // 
            this.SplitContainer5.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.SplitContainer5.Panel2.Controls.Add(this.AttachmentList);
            this.SplitContainer5.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.SplitContainer5.Size = new System.Drawing.Size(243, 297);
            this.SplitContainer5.SplitterDistance = 180;
            this.SplitContainer5.SplitterWidth = 3;
            this.SplitContainer5.TabIndex = 0;
            // 
            // MessageView
            // 
            this.MessageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessageView.Location = new System.Drawing.Point(0, 0);
            this.MessageView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MessageView.Name = "MessageView";
            this.MessageView.Size = new System.Drawing.Size(243, 180);
            this.MessageView.TabIndex = 0;
            // 
            // AttachmentList
            // 
            this.AttachmentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AttachmentList.Location = new System.Drawing.Point(0, 0);
            this.AttachmentList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.AttachmentList.Name = "AttachmentList";
            this.AttachmentList.Size = new System.Drawing.Size(243, 114);
            this.AttachmentList.TabIndex = 0;
            // 
            // PropertiesControl
            // 
            this.PropertiesControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertiesControl.Location = new System.Drawing.Point(0, 0);
            this.PropertiesControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PropertiesControl.Name = "PropertiesControl";
            this.PropertiesControl.Size = new System.Drawing.Size(195, 436);
            this.PropertiesControl.TabIndex = 0;
            // 
            // OpenXstFileDialog
            // 
            this.OpenXstFileDialog.Filter = "xst files (*.ost;*.pst)|*.ost;*.pst|All files (*.*)|*.*";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1140, 460);
            this.Controls.Add(this.SplitContainer1);
            this.Controls.Add(this.MainMenuSrip);
            this.MainMenuStrip = this.MainMenuSrip;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "XstReader";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.MainMenuSrip.ResumeLayout(false);
            this.MainMenuSrip.PerformLayout();
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).EndInit();
            this.SplitContainer1.ResumeLayout(false);
            this.SplitContainer2.Panel1.ResumeLayout(false);
            this.SplitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer2)).EndInit();
            this.SplitContainer2.ResumeLayout(false);
            this.SplitContainer3.Panel1.ResumeLayout(false);
            this.SplitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer3)).EndInit();
            this.SplitContainer3.ResumeLayout(false);
            this.SplitContainer4.Panel1.ResumeLayout(false);
            this.SplitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer4)).EndInit();
            this.SplitContainer4.ResumeLayout(false);
            this.SplitContainer5.Panel1.ResumeLayout(false);
            this.SplitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer5)).EndInit();
            this.SplitContainer5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip MainMenuSrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private SplitContainer SplitContainer1;
        private OpenFileDialog OpenXstFileDialog;
        private ToolStripMenuItem closeFileToolStripMenuItem;
        private SplitContainer SplitContainer2;
        private SplitContainer SplitContainer3;
        private SplitContainer SplitContainer4;
        private SplitContainer SplitContainer5;
        private App.Controls.XstFolderTreeControl FolderTree;
        private App.Controls.XstPropertiesControl PropertiesControl;
        private App.Controls.XstMessageListControl MessageList;
        private App.Controls.XstRecipientListControl RecipientList;
        private App.Controls.XstAttachmentListControl AttachmentList;
        private App.Controls.XstMessageViewControl MessageView;
    }
}