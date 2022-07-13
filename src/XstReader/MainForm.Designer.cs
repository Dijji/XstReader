namespace XstReader.App
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.KryptonDockingManager = new Krypton.Docking.KryptonDockingManager();
            this.KryptonMainPanel = new Krypton.Toolkit.KryptonPanel();
            this.KryptonMessagePanel = new Krypton.Toolkit.KryptonPanel();
            this.MainMenuSrip = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.FileExportFoldersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileExportAttachmentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FolderExportMessagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FolderExportAttachmentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MessageExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MessagePrintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MessageExportAttachmentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MessageToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.MessageExportAsmsgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConfigExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenXstFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.FolderExportFoldersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.KryptonMainPanel)).BeginInit();
            this.KryptonMainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KryptonMessagePanel)).BeginInit();
            this.MainMenuSrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // KryptonMainPanel
            // 
            this.KryptonMainPanel.Controls.Add(this.KryptonMessagePanel);
            this.KryptonMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KryptonMainPanel.Location = new System.Drawing.Point(0, 28);
            this.KryptonMainPanel.Name = "KryptonMainPanel";
            this.KryptonMainPanel.PanelBackStyle = Krypton.Toolkit.PaletteBackStyle.PanelAlternate;
            this.KryptonMainPanel.Size = new System.Drawing.Size(770, 422);
            this.KryptonMainPanel.TabIndex = 2;
            // 
            // KryptonMessagePanel
            // 
            this.KryptonMessagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KryptonMessagePanel.Location = new System.Drawing.Point(0, 0);
            this.KryptonMessagePanel.Name = "KryptonMessagePanel";
            this.KryptonMessagePanel.Size = new System.Drawing.Size(770, 422);
            this.KryptonMessagePanel.TabIndex = 0;
            // 
            // MainMenuSrip
            // 
            this.MainMenuSrip.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MainMenuSrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenuSrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.FolderToolStripMenuItem,
            this.MessageToolStripMenuItem,
            this.ExportToolStripMenuItem});
            this.MainMenuSrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuSrip.Name = "MainMenuSrip";
            this.MainMenuSrip.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.MainMenuSrip.Size = new System.Drawing.Size(770, 28);
            this.MainMenuSrip.TabIndex = 3;
            this.MainMenuSrip.Text = "menuStrip1";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenToolStripMenuItem,
            this.CloseFileToolStripMenuItem,
            this.FileToolStripSeparator,
            this.FileExportFoldersToolStripMenuItem,
            this.FileExportAttachmentsToolStripMenuItem});
            this.FileToolStripMenuItem.Image = global::XstReader.App.Properties.Resources.file_outline;
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(57, 24);
            this.FileToolStripMenuItem.Text = "File";
            // 
            // OpenToolStripMenuItem
            // 
            this.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            this.OpenToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.OpenToolStripMenuItem.Text = "Open .ost or .pst file";
            // 
            // CloseFileToolStripMenuItem
            // 
            this.CloseFileToolStripMenuItem.Enabled = false;
            this.CloseFileToolStripMenuItem.Name = "CloseFileToolStripMenuItem";
            this.CloseFileToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.CloseFileToolStripMenuItem.Text = "Close file";
            // 
            // FileToolStripSeparator
            // 
            this.FileToolStripSeparator.Name = "FileToolStripSeparator";
            this.FileToolStripSeparator.Size = new System.Drawing.Size(247, 6);
            // 
            // FileExportFoldersToolStripMenuItem
            // 
            this.FileExportFoldersToolStripMenuItem.Image = global::XstReader.App.Properties.Resources.folder_multiple_outline;
            this.FileExportFoldersToolStripMenuItem.Name = "FileExportFoldersToolStripMenuItem";
            this.FileExportFoldersToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.FileExportFoldersToolStripMenuItem.Text = "Export all Folders in the file";
            // 
            // FileExportAttachmentsToolStripMenuItem
            // 
            this.FileExportAttachmentsToolStripMenuItem.Image = global::XstReader.App.Properties.Resources.paperclip;
            this.FileExportAttachmentsToolStripMenuItem.Name = "FileExportAttachmentsToolStripMenuItem";
            this.FileExportAttachmentsToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.FileExportAttachmentsToolStripMenuItem.Text = "Export all Attachments in the file";
            // 
            // FolderToolStripMenuItem
            // 
            this.FolderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FolderExportFoldersToolStripMenuItem,
            this.FolderExportMessagesToolStripMenuItem,
            this.FolderExportAttachmentsToolStripMenuItem});
            this.FolderToolStripMenuItem.Image = global::XstReader.App.Properties.Resources.folder_outline;
            this.FolderToolStripMenuItem.Name = "FolderToolStripMenuItem";
            this.FolderToolStripMenuItem.Size = new System.Drawing.Size(72, 24);
            this.FolderToolStripMenuItem.Text = "Folder";
            // 
            // FolderExportMessagesToolStripMenuItem
            // 
            this.FolderExportMessagesToolStripMenuItem.Image = global::XstReader.App.Properties.Resources.email_open_multiple_outline;
            this.FolderExportMessagesToolStripMenuItem.Name = "FolderExportMessagesToolStripMenuItem";
            this.FolderExportMessagesToolStripMenuItem.Size = new System.Drawing.Size(292, 26);
            this.FolderExportMessagesToolStripMenuItem.Text = "Export all Messages in the folder";
            // 
            // FolderExportAttachmentsToolStripMenuItem
            // 
            this.FolderExportAttachmentsToolStripMenuItem.Image = global::XstReader.App.Properties.Resources.paperclip;
            this.FolderExportAttachmentsToolStripMenuItem.Name = "FolderExportAttachmentsToolStripMenuItem";
            this.FolderExportAttachmentsToolStripMenuItem.Size = new System.Drawing.Size(292, 26);
            this.FolderExportAttachmentsToolStripMenuItem.Text = "Export all Attachments in the folder";
            // 
            // MessageToolStripMenuItem
            // 
            this.MessageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MessageExportToolStripMenuItem,
            this.MessagePrintToolStripMenuItem,
            this.MessageExportAttachmentsToolStripMenuItem,
            this.MessageToolStripSeparator,
            this.MessageExportAsmsgToolStripMenuItem});
            this.MessageToolStripMenuItem.Image = global::XstReader.App.Properties.Resources.email_outline;
            this.MessageToolStripMenuItem.Name = "MessageToolStripMenuItem";
            this.MessageToolStripMenuItem.Size = new System.Drawing.Size(85, 24);
            this.MessageToolStripMenuItem.Text = "Message";
            // 
            // MessageExportToolStripMenuItem
            // 
            this.MessageExportToolStripMenuItem.Image = global::XstReader.App.Properties.Resources.email_open_outline;
            this.MessageExportToolStripMenuItem.Name = "MessageExportToolStripMenuItem";
            this.MessageExportToolStripMenuItem.Size = new System.Drawing.Size(280, 26);
            this.MessageExportToolStripMenuItem.Text = "Export message";
            // 
            // MessagePrintToolStripMenuItem
            // 
            this.MessagePrintToolStripMenuItem.Image = global::XstReader.App.Properties.Resources.printer;
            this.MessagePrintToolStripMenuItem.Name = "MessagePrintToolStripMenuItem";
            this.MessagePrintToolStripMenuItem.Size = new System.Drawing.Size(280, 26);
            this.MessagePrintToolStripMenuItem.Text = "Print message";
            // 
            // MessageExportAttachmentsToolStripMenuItem
            // 
            this.MessageExportAttachmentsToolStripMenuItem.Image = global::XstReader.App.Properties.Resources.paperclip;
            this.MessageExportAttachmentsToolStripMenuItem.Name = "MessageExportAttachmentsToolStripMenuItem";
            this.MessageExportAttachmentsToolStripMenuItem.Size = new System.Drawing.Size(280, 26);
            this.MessageExportAttachmentsToolStripMenuItem.Text = "Export all Attachments in the message";
            // 
            // MessageToolStripSeparator
            // 
            this.MessageToolStripSeparator.Name = "MessageToolStripSeparator";
            this.MessageToolStripSeparator.Size = new System.Drawing.Size(277, 6);
            // 
            // MessageExportAsmsgToolStripMenuItem
            // 
            this.MessageExportAsmsgToolStripMenuItem.Name = "MessageExportAsmsgToolStripMenuItem";
            this.MessageExportAsmsgToolStripMenuItem.Size = new System.Drawing.Size(280, 26);
            this.MessageExportAsmsgToolStripMenuItem.Text = "Export as .msg";
            // 
            // ExportToolStripMenuItem
            // 
            this.ExportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConfigExportToolStripMenuItem});
            this.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
            this.ExportToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.ExportToolStripMenuItem.Text = "Export";
            // 
            // ConfigExportToolStripMenuItem
            // 
            this.ConfigExportToolStripMenuItem.Image = global::XstReader.App.Properties.Resources.cog;
            this.ConfigExportToolStripMenuItem.Name = "ConfigExportToolStripMenuItem";
            this.ConfigExportToolStripMenuItem.Size = new System.Drawing.Size(146, 26);
            this.ConfigExportToolStripMenuItem.Text = "Options";
            this.ConfigExportToolStripMenuItem.ToolTipText = "Export Options";
            // 
            // OpenXstFileDialog
            // 
            this.OpenXstFileDialog.Filter = "xst files (*.ost;*.pst)|*.ost;*.pst|All files (*.*)|*.*";
            // 
            // FolderExportFoldersToolStripMenuItem
            // 
            this.FolderExportFoldersToolStripMenuItem.Image = global::XstReader.App.Properties.Resources.folder_multiple_outline;
            this.FolderExportFoldersToolStripMenuItem.Name = "FolderExportFoldersToolStripMenuItem";
            this.FolderExportFoldersToolStripMenuItem.Size = new System.Drawing.Size(292, 26);
            this.FolderExportFoldersToolStripMenuItem.Text = "Export all Folders and Messages in folder";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 450);
            this.Controls.Add(this.KryptonMainPanel);
            this.Controls.Add(this.MainMenuSrip);
            this.Name = "MainForm";
            this.Text = "XstReader";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.KryptonMainPanel)).EndInit();
            this.KryptonMainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.KryptonMessagePanel)).EndInit();
            this.MainMenuSrip.ResumeLayout(false);
            this.MainMenuSrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Krypton.Docking.KryptonDockingManager KryptonDockingManager;
        private Krypton.Toolkit.KryptonPanel KryptonMainPanel;
        private MenuStrip MainMenuSrip;
        private ToolStripMenuItem FileToolStripMenuItem;
        private ToolStripMenuItem OpenToolStripMenuItem;
        private ToolStripMenuItem CloseFileToolStripMenuItem;
        private ToolStripMenuItem MessageToolStripMenuItem;
        private ToolStripMenuItem MessageExportAsmsgToolStripMenuItem;
        private OpenFileDialog OpenXstFileDialog;
        private Krypton.Toolkit.KryptonPanel KryptonMessagePanel;
        private ToolStripMenuItem ExportToolStripMenuItem;
        private ToolStripMenuItem ConfigExportToolStripMenuItem;
        private ToolStripSeparator FileToolStripSeparator;
        private ToolStripMenuItem FileExportFoldersToolStripMenuItem;
        private ToolStripMenuItem FileExportAttachmentsToolStripMenuItem;
        private ToolStripMenuItem FolderToolStripMenuItem;
        private ToolStripMenuItem FolderExportMessagesToolStripMenuItem;
        private ToolStripMenuItem FolderExportAttachmentsToolStripMenuItem;
        private ToolStripMenuItem MessageExportToolStripMenuItem;
        private ToolStripMenuItem MessagePrintToolStripMenuItem;
        private ToolStripMenuItem MessageExportAttachmentsToolStripMenuItem;
        private ToolStripSeparator MessageToolStripSeparator;
        private ToolStripMenuItem FolderExportFoldersToolStripMenuItem;
    }
}