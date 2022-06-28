using XstReader.App.Common;

namespace XstReader.App.Controls
{
    public partial class XstPropertiesControl : UserControl,
                                                IXstDataSourcedControl<XstElement>
    {
        public XstPropertiesControl()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            if (DesignMode) return;

            PropertyGridProperties.SelectedGridItemChanged += (s, e)
                => PropDescWebView.DocumentText = (e.NewSelection.PropertyDescriptor as CustomXstPropertyDescriptor)?.HtmlDescription ?? "";

            PropDescWebView.DocumentCompleted += (s,e) => PropDescWebView.Document.Body.Style = "zoom:90%";
        }

        private XstElement? _DataSource;
        public XstElement? GetDataSource()
            => _DataSource;

        public void SetDataSource(XstElement? dataSource)
        {
            _DataSource = dataSource;
            LoadProperties();
        }

        private void LoadProperties()
        {
            ElementTypeLabel.Text = _DataSource?.ElementType.ToString();
            ElementNameLabel.Text = _DataSource?.ToString();

            PropertyGridProperties.SelectedObject = _DataSource?.Properties.ToPropertyGridSelectedObject();
        }

        public void ClearContents()
        {
            GetDataSource()?.ClearContents();
            SetDataSource(null);
        }

    }
}
