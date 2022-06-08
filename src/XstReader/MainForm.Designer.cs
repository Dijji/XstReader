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
            this.components = new System.ComponentModel.Container();
            this.KryptonDockingManager = new Krypton.Docking.KryptonDockingManager();
            this.KryptonManager = new Krypton.Toolkit.KryptonManager(this.components);
            this.KryptonMainPanel = new Krypton.Toolkit.KryptonPanel();
            this.KryptonMessagePanel = new Krypton.Toolkit.KryptonPanel();
            this.KryptonContextMenu = new Krypton.Toolkit.KryptonContextMenu();
            this.MainMenuSrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportAsmsgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenXstFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.KryptonMainPanel)).BeginInit();
            this.KryptonMainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KryptonMessagePanel)).BeginInit();
            this.MainMenuSrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // KryptonManager
            // 
            this.KryptonManager.GlobalPaletteMode = Krypton.Toolkit.PaletteModeManager.Office2010Silver;
            // 
            // KryptonMainPanel
            // 
            this.KryptonMainPanel.Controls.Add(this.KryptonMessagePanel);
            this.KryptonMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KryptonMainPanel.Location = new System.Drawing.Point(0, 24);
            this.KryptonMainPanel.Name = "KryptonMainPanel";
            this.KryptonMainPanel.PanelBackStyle = Krypton.Toolkit.PaletteBackStyle.PanelAlternate;
            this.KryptonMainPanel.Size = new System.Drawing.Size(770, 426);
            this.KryptonMainPanel.TabIndex = 2;
            // 
            // KryptonMessagePanel
            // 
            this.KryptonMessagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KryptonMessagePanel.Location = new System.Drawing.Point(0, 0);
            this.KryptonMessagePanel.Name = "KryptonMessagePanel";
            this.KryptonMessagePanel.Size = new System.Drawing.Size(770, 426);
            this.KryptonMessagePanel.TabIndex = 0;
            // 
            // MainMenuSrip
            // 
            this.MainMenuSrip.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MainMenuSrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenuSrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.MessageToolStripMenuItem});
            this.MainMenuSrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuSrip.Name = "MainMenuSrip";
            this.MainMenuSrip.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.MainMenuSrip.Size = new System.Drawing.Size(770, 24);
            this.MainMenuSrip.TabIndex = 3;
            this.MainMenuSrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenToolStripMenuItem,
            this.CloseFileToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // OpenToolStripMenuItem
            // 
            this.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            this.OpenToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.OpenToolStripMenuItem.Text = "Open .ost or .pst file";
            // 
            // CloseFileToolStripMenuItem
            // 
            this.CloseFileToolStripMenuItem.Enabled = false;
            this.CloseFileToolStripMenuItem.Name = "CloseFileToolStripMenuItem";
            this.CloseFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.CloseFileToolStripMenuItem.Text = "Close file";
            // 
            // MessageToolStripMenuItem
            // 
            this.MessageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExportAsmsgToolStripMenuItem});
            this.MessageToolStripMenuItem.Name = "MessageToolStripMenuItem";
            this.MessageToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.MessageToolStripMenuItem.Text = "Message";
            // 
            // ExportAsmsgToolStripMenuItem
            // 
            this.ExportAsmsgToolStripMenuItem.Name = "ExportAsmsgToolStripMenuItem";
            this.ExportAsmsgToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.ExportAsmsgToolStripMenuItem.Text = "Export as .msg";
            // 
            // OpenXstFileDialog
            // 
            this.OpenXstFileDialog.Filter = "xst files (*.ost;*.pst)|*.ost;*.pst|All files (*.*)|*.*";
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
        private Krypton.Toolkit.KryptonManager KryptonManager;
        private Krypton.Toolkit.KryptonPanel KryptonMainPanel;
        private Krypton.Toolkit.KryptonContextMenu KryptonContextMenu;
        private MenuStrip MainMenuSrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem OpenToolStripMenuItem;
        private ToolStripMenuItem CloseFileToolStripMenuItem;
        private ToolStripMenuItem MessageToolStripMenuItem;
        private ToolStripMenuItem ExportAsmsgToolStripMenuItem;
        private OpenFileDialog OpenXstFileDialog;
        private Krypton.Toolkit.KryptonPanel KryptonMessagePanel;
    }
}