using XstReader.App.Common;

namespace XstReader.App.Controls
{
    public partial class XstAttachmentViewControl : UserControl,
                                                    IXstDataSourcedControl<XstAttachment>,
                                                    IXstElementSelectable<XstAttachment>
    {
        public XstAttachmentViewControl()
        {
            InitializeComponent();
            Initialize();
        }
        private void Initialize()
        {
            if (DesignMode) return;

            WebView2.CoreWebView2InitializationCompleted += (s, e) =>
            {
                if (ToDoWhenInitialized?.Any() ?? false)
                    ToDoWhenInitialized.ForEach(a => a?.Invoke());
                ToDoWhenInitialized?.Clear();

                IsCoreViewInitialized = true;
            };
            WebView2.EnsureCoreWebView2Async();
        }
        private bool IsCoreViewInitialized { get; set; } = false;
        private List<Action> ToDoWhenInitialized { get; set; } = new List<Action>();

        public event EventHandler<XstElementEventArgs>? SelectedItemChanged;

        private void RaiseSelectedItemChanged() => RaiseSelectedItemChanged(GetSelectedItem());
        private void RaiseSelectedItemChanged(XstElement? element) => SelectedItemChanged?.Invoke(this, new XstElementEventArgs(element));

        private XstAttachment? _DataSource;
        public XstAttachment? GetDataSource()
            => _DataSource;

        public void SetDataSource(XstAttachment? dataSource)
        {
            CleanTempFile();
            _DataSource = dataSource;

            try
            {
                if (!IsCoreViewInitialized)
                    ToDoWhenInitialized.Add(() => WebView2.Source = new Uri(SetTempFileContent(_DataSource)));
                else
                    WebView2.Source = new Uri(SetTempFileContent(_DataSource));
            }
            catch (Exception ex)
            {
                WebView2?.NavigateToString(ex.Message);
                //MessageBox.Show(ex.Message, "Error showing message");
            }
        }

        public XstAttachment? GetSelectedItem()
        {
            return _DataSource;
        }

        public void SetSelectedItem(XstAttachment? item)
        { }

        private string? _TempFileName = null;
        private void CleanTempFile()
        {
            if (_TempFileName != null && File.Exists(_TempFileName))
                try { File.Delete(_TempFileName); }
                catch { }
            _TempFileName = null;
        }
        private string SetTempFileContent(XstAttachment? attachment)
        {
            if (attachment == null)
                return "";

            _TempFileName = Path.GetTempFileName() + Path.GetExtension(attachment.FileName);
            attachment?.SaveToFile(_TempFileName);

            return _TempFileName;
        }
        public void ClearContents()
        {
            GetDataSource()?.ClearContents();
            SetDataSource(null);
        }
    }
}
