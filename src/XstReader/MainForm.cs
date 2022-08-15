// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

using Krypton.Docking;
using Krypton.Toolkit;
using System.Data;
using System.Reflection;
using XstReader.App.Controls;
using XstReader.App.Helpers;

namespace XstReader.App
{
    public partial class MainForm : KryptonForm
    {
        private XstFolderTreeControl FolderTreeControl { get; } = new XstFolderTreeControl() { Name = "Folders Tree" };
        private XstMessageListControl MessageListControl { get; } = new XstMessageListControl() { Name = "Message List" };
        private XstMessageViewControl MessageViewControl { get; } = new XstMessageViewControl() { Name = "Message View" };
        private XstPropertiesControl PropertiesControl { get; } = new XstPropertiesControl() { Name = "Properties" };
        private XstPropertiesInfoControl InfoControl { get; } = new XstPropertiesInfoControl() { Name = "Information" };


        private XstFile? _XstFile = null;
        private XstFile? XstFile
        {
            get => _XstFile;
            set => SetXstFile(value);
        }
        private void SetXstFile(XstFile? value)
        {
            _XstFile = value;
            FolderTreeControl.SetDataSource(value);
            CloseFileToolStripMenuItem.Enabled = value != null;
        }

        private XstElement? _CurrentXstElement = null;
        private XstElement? CurrentXstElement
        {
            get => _CurrentXstElement;
            set => SetCurrentXstElement(value);
        }
        private void SetCurrentXstElement(XstElement? value)
        {
            if (_CurrentXstElement == value)
                return;

            _CurrentXstElement?.ClearContents();
            if (value is XstFolder folder)
            {
                MessageListControl.SetDataSource(folder?.Messages?.OrderByDescending(m => m.Date));//, MessageFilter.GetSelectedFilter());
            }
            else if (value is XstMessage message)
            {
                MessageViewControl.SetDataSource(message);
            }

            MessageToolStripMenuItem.Enabled = value != null && value is XstMessage;
            InfoControl.SetDataSource(value);
            PropertiesControl.SetDataSource(value);

            UpdateMenu();

            _CurrentXstElement = value;
        }

        private XstFile? GetCurrentXstFile() => FolderTreeControl.GetDataSource();
        private XstFolder? GetCurrentXstFolder() => FolderTreeControl.GetSelectedItem();
        private XstMessage? GetCurrentXstMessage() => MessageViewControl.GetDataSource();
        private IEnumerable<XstAttachment>? GetCurrentXstAttachmentsToExport() => GetCurrentXstMessage()?.Attachments?.Where(a => a.IsFile && !a.IsHidden);

        private void UpdateMenu()
        {
            FileExportFoldersToolStripMenuItem.Enabled =
                FileExportAttachmentsToolStripMenuItem.Enabled =
                GetCurrentXstFile() != null;

            FolderToolStripMenuItem.Enabled = GetCurrentXstFolder() != null;
            FolderExportFoldersToolStripMenuItem.Enabled = GetCurrentXstFolder()?.Folders?.Any() ?? false;

            MessageToolStripMenuItem.Enabled = GetCurrentXstMessage() != null;
            MessageExportAttachmentsToolStripMenuItem.Enabled = GetCurrentXstAttachmentsToExport()?.Any() ?? false;
        }

        public MainForm()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            OpenToolStripMenuItem.Click += OpenXstFile;
            CloseFileToolStripMenuItem.Click += (s, e) => CloseXstFile();

            FolderTreeControl.SelectedItemChanged += (s, e) => CurrentXstElement = e.Element;
            FolderTreeControl.GotFocus += (s, e) => CurrentXstElement = FolderTreeControl.GetSelectedItem();

            MessageListControl.SelectedItemChanged += (s, e) => CurrentXstElement = e.Element;
            MessageListControl.GotFocus += (s, e) => CurrentXstElement = MessageListControl.GetSelectedItem();

            MessageViewControl.SelectedItemChanged += (s, e) => CurrentXstElement = e.Element;
            MessageViewControl.GotFocus += (s, e) => CurrentXstElement = MessageViewControl.GetSelectedItem();

            ConfigExportToolStripMenuItem.Click += (s, e) => { using var f = new SettingsForm(); f.ShowDialog(); };


            AboutToolStripMenuItem.Click += (s, e) => { using var f = new AboutForm(); f.ShowDialog(); };

            FileExportFoldersToolStripMenuItem.Click += (s, e) =>
            {
                string path = string.Empty;
                var elem = GetCurrentXstFile();
                if (elem != null && ExportHelper.AskDirectoryPath(ref path))
                    DoInWait($"Exporting all Folders and Messages from {elem.FileName}",
                             () => ExportHelper.ExportFolderToHtmlFiles(elem.RootFolder, path, true));
            };
            FileExportAttachmentsToolStripMenuItem.Click += (s, e) =>
            {
                string path = string.Empty;
                var elem = GetCurrentXstFile();
                if (elem != null && ExportHelper.AskDirectoryPath(ref path))
                    DoInWait($"Exporting all Attachments from {elem.FileName}",
                             () => ExportHelper.ExportAttachmentsToDirectory(elem.RootFolder, path, true));
            };

            FolderExportFoldersToolStripMenuItem.Click += (s, e) =>
            {
                string path = string.Empty;
                var elem = GetCurrentXstFolder();
                if (elem != null && ExportHelper.AskDirectoryPath(ref path))
                    DoInWait($"Exporting all Folders and Messages from folder {elem.Path}",
                             () => ExportHelper.ExportFolderToHtmlFiles(elem, path, true));
            };
            FolderExportMessagesToolStripMenuItem.Click += (s, e) =>
            {
                string path = string.Empty;
                var elem = GetCurrentXstFolder();
                if (elem != null && ExportHelper.AskDirectoryPath(ref path))
                    DoInWait($"Exporting all Messages in folder {elem.Path}",
                             () => ExportHelper.ExportFolderToHtmlFiles(elem, path, false));
            };
            FolderExportAttachmentsToolStripMenuItem.Click += (s, e) =>
            {
                string path = string.Empty;
                var elem = GetCurrentXstFolder();
                if (elem != null && ExportHelper.AskDirectoryPath(ref path))
                    DoInWait($"Exporting all Attachments from folder {elem.Path}",
                             () => ExportHelper.ExportAttachmentsToDirectory(elem, path, true));
            };
            MessagePrintToolStripMenuItem.Click += (s, e) => MessageViewControl.Print();
            MessageExportToolStripMenuItem.Click += (s, e) =>
            {
                string fileName = GetCurrentXstMessage()?.GetFilenameForExport() ?? "none" + ".html";
                var elem = GetCurrentXstMessage();
                if (elem != null && ExportHelper.AskFileName(ref fileName))
                    DoInWait($"Exporting Message {elem.Path}",
                             () => ExportHelper.ExportMessageToHtmlFile(elem, fileName));
            };
            MessageExportAttachmentsToolStripMenuItem.Click += (s, e) =>
            {
                string path = string.Empty;
                var elem = GetCurrentXstMessage();
                var elems = GetCurrentXstAttachmentsToExport();
                if (elem != null && elems != null && ExportHelper.AskDirectoryPath(ref path))
                    DoInWait($"Exporting Attachments from message {elem.Path}",
                             () => ExportHelper.ExportAttachmentsToDirectory(elems, path));
            };

            LayoutDefaultToolStripMenuItem.Click += (s, e) =>
            {
                try
                {
                    var fileName = Path.GetTempFileName()+".xml";
                    File.WriteAllBytes(fileName, Properties.Resources.layout_default);
                    KryptonDockingManager.LoadConfigFromFile(fileName);
                    File.Delete(fileName);
                }
                catch { }
            };
            LayoutClassic3PanelToolStripMenuItem.Click += (s, e) =>
            {
                try
                {
                    var fileName = Path.GetTempFileName() + ".xml";
                    File.WriteAllBytes(fileName, Properties.Resources.layout_3panels);
                    KryptonDockingManager.LoadConfigFromFile(fileName);
                    File.Delete(fileName);
                }
                catch { }
            };

            Reset();
            UpdateMenu();
        }

        private void DoInWait(string description, Action action)
            => WaitingForm.Execute(description, action);

        private void OpenXstFile(object? sender, EventArgs e)
        {
            if (OpenXstFileDialog.ShowDialog(this) == DialogResult.OK)
                LoadXstFile(OpenXstFileDialog.FileName);
        }

        private void Reset()
        {
            if (XstFile != null)
            {
                XstFile.ClearContents();
                XstFile.Dispose();
                XstFile = null;

                FolderTreeControl.ClearContents();
                MessageListControl.ClearContents();
                MessageViewControl.ClearContents();
                //RecipientListControl.ClearContents();
                //AttachmentListControl.ClearContents();
                MessageViewControl.ClearContents();
                PropertiesControl.ClearContents();
            }
        }
        private void LoadXstFile(string filename)
        {
            CloseXstFile();
            XstFile = new XstFile(filename);
        }

        private void CloseXstFile()
        {
            Reset();
        }

        protected override void OnClosed(EventArgs e)
        {
            try { KryptonDockingManager.SaveConfigToFile(Path.Combine(Application.StartupPath, "Layout.xml")); }
            catch { }

            Reset();
            base.OnClosed(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadForm();
        }

        private void LoadForm()
        {
            KryptonMessagePanel.BeginInit();
            KryptonMessagePanel.Controls.Add(MessageViewControl);
            MessageViewControl.Dock = DockStyle.Fill;

            // Setup docking functionality
            KryptonDockingManager.ManageControl(KryptonMainPanel);

            KryptonDockingManager.AddXstDockSpaceInTabs(DockingEdge.Top, MessageListControl);
            KryptonDockingManager.AddXstDockSpaceInTabs(DockingEdge.Right, InfoControl, PropertiesControl);
            KryptonDockingManager.AddXstDockSpaceInTabs(DockingEdge.Left, FolderTreeControl);

            try { KryptonDockingManager.LoadConfigFromFile(Path.Combine(Application.StartupPath, "Layout.xml")); }
            catch { LayoutDefaultToolStripMenuItem.PerformClick(); }
            KryptonMessagePanel.EndInit();
        }

        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KryptonDockingManager.SaveConfigToFile(@"C:\dev\pst\layout.xml");
        }

        private void loadLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KryptonDockingManager.LoadConfigFromFile(@"C:\dev\pst\layout.xml");
        }
    }
}
