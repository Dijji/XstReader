using Krypton.Docking;
using Krypton.Toolkit;
using System.Data;
using XstReader.App.Controls;

namespace XstReader.App
{
    public partial class MainForm : KryptonForm
    {
        private XstFolderTreeControl FolderTreeControl { get; } = new XstFolderTreeControl() { Name = "Folders tree" };
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
            ExportAsmsgToolStripMenuItem.Enabled = value != null && value is XstMessage;
            InfoControl.SetDataSource(value);
            PropertiesControl.SetDataSource(value);

            _CurrentXstElement = value;
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
            ExportAsmsgToolStripMenuItem.Click += (s, e) => ExportToMsg();

            FolderTreeControl.SelectedItemChanged += (s, e) => CurrentXstElement = e.Element;
            FolderTreeControl.GotFocus += (s, e) => CurrentXstElement = FolderTreeControl.GetSelectedItem();

            MessageListControl.SelectedItemChanged += (s, e) => CurrentXstElement = e.Element;
            MessageListControl.GotFocus += (s, e) => CurrentXstElement = MessageListControl.GetSelectedItem();

            MessageViewControl.SelectedItemChanged += (s, e) => CurrentXstElement = e.Element;
            MessageViewControl.GotFocus += (s, e) => CurrentXstElement = MessageViewControl.GetSelectedItem();

            Reset();
        }
        private void ExportToMsg()
        {
            if (CurrentXstElement is XstMessage msg)
            {
                var invalidPathChars = Path.GetInvalidFileNameChars();
                string fileName = Path.Combine(@"C:\Dev\out\", new string(msg.Subject.Where(c => !invalidPathChars.Contains(c)).ToArray()) + ".msg");
                msg.SaveToMsg(fileName);
                //System.Diagnostics.Process.Start(fileName);
            }
        }
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
            KryptonMessagePanel.Controls.Add(MessageViewControl);
            MessageViewControl.Dock = DockStyle.Fill;

            // Setup docking functionality
            KryptonDockingManager.ManageControl(KryptonMainPanel);

            KryptonDockingManager.AddXstDockSpaceInTabs(DockingEdge.Top, MessageListControl);
            KryptonDockingManager.AddXstDockSpaceInTabs(DockingEdge.Right, InfoControl, PropertiesControl);
            KryptonDockingManager.AddXstDockSpaceInTabs(DockingEdge.Left, FolderTreeControl);
        }

    }
}
