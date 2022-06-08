using XstReader.App.Common;

namespace XstReader.App.Controls
{
    public partial class OldXstMessageViewControl : UserControl,
                                                 IXstDataSourcedControl<XstMessage>
    {
        public OldXstMessageViewControl()
        {
            InitializeComponent();
            Initialize();
        }
        private void Initialize()
        {
            if (DesignMode) return;

            WebView2.EnsureCoreWebView2Async();
        }

        private XstMessage? _DataSource;
        public XstMessage? GetDataSource()
            => _DataSource;

        public void SetDataSource(XstMessage? dataSource)
        {
            CleanTempFile();
            _DataSource = dataSource;
            try
            {
                FormatLabel.Text = _DataSource?.Body?.Format.ToString();
                MessageLabel.Text = _DataSource?.Subject;
                var text = _DataSource?.Body?.Text ?? "";
                if (text.Length < 1500000)
                    WebView2.NavigateToString(text);
                else
                    WebView2.Source = new Uri(SetTempFileContent(text));
            }
            catch (Exception ex)
            {
                WebView2?.NavigateToString(ex.Message);
                MessageBox.Show(ex.Message, "Error showing message");
            }
        }

        private string? _TempFileName = null;
        private void CleanTempFile()
        {
            if (_TempFileName != null && File.Exists(_TempFileName))
                try { File.Delete(_TempFileName); }
                catch { }
            _TempFileName = null;
        }
        private string SetTempFileContent(string text)
        {
            _TempFileName = Path.GetTempFileName() + ".html";
            File.WriteAllText(_TempFileName, text);

            return _TempFileName;
        }
        public void ClearContents()
        {
            GetDataSource()?.ClearContents();
            SetDataSource(null);
        }

    }
}
