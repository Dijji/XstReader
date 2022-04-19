namespace XstReader
{
    public partial class MainForm : Form
    {
        private XstFile? _XstFile = null;
        private XstFile? XstFile
        {
            get => _XstFile;
            set => SetXstFile(value);
        }
        private void SetXstFile(XstFile? value)
        {
            _XstFile = value;
            FolderTree.SetDataSource(value);
            closeFileToolStripMenuItem.Enabled = value != null;
        }

        private XstElement? _CurrentXstElement = null;
        private XstElement? CurrentXstElement
        {
            get => _CurrentXstElement;
            set => SetCurrentXstElement(value);
        }
        private void SetCurrentXstElement(XstElement? value)
        {
            _CurrentXstElement = value;
            PropertiesControl.SetDataSource(value);
        }

        public MainForm()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            openToolStripMenuItem.Click += OpenXstFile;
            closeFileToolStripMenuItem.Click += (s, e) => CloseXstFile();

            XstFolder? lastFolder = null;
            FolderTree.SelectedItemChanged += (s, e) =>
            {
                if (lastFolder != null) lastFolder.ClearContents();
                lastFolder = e.Element as XstFolder;
                //MessageFilter.SetDataSource(lastFolder?.Messages);
                MessageList.SetDataSource(lastFolder?.Messages?.OrderByDescending(m => m.Date));//, MessageFilter.GetSelectedFilter());
                SplitContainer3.SplitterDistance = MessageList.GetGridWidth();
                CurrentXstElement = e.Element;
            };
            FolderTree.GotFocus += (s, e) => CurrentXstElement = FolderTree.GetSelectedItem();
            //MessageFilter.FilterChanged += (s, e) => MessageList.ApplyFilter(e.Filter);
            MessageList.SelectedItemChanged += (s, e) =>
            {
                var message = e.Element as XstMessage;
                RecipientList.SetDataSource(message?.Recipients.Items);
                AttachmentList.SetDataSource(message?.Attachments);
                MessageView.SetDataSource(message);
                CurrentXstElement = e.Element;
            };
            MessageList.GotFocus += (s, e) => CurrentXstElement = MessageList.GetSelectedItem();
            RecipientList.SelectedItemChanged += (s, e) =>
            {
                CurrentXstElement = e.Element;
            };
            RecipientList.GotFocus += (s, e) => CurrentXstElement = RecipientList.GetSelectedItem();
            AttachmentList.SelectedItemChanged += (s, e) =>
            {
                CurrentXstElement = e.Element;
            };
            AttachmentList.GotFocus += (s, e) => CurrentXstElement = AttachmentList.GetSelectedItem();

            Reset();
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
            }
            CurrentXstElement = null;
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
    }
}