using XstReader.App.Common;

namespace XstReader.App.Controls
{
    public partial class XstMessageViewControl : UserControl,
                                                 IXstDataSourcedControl<XstMessage>,
                                                 IXstElementSelectable<XstElement>
    {

        private XstMessageContentViewControl MessageContentControl { get; } = new XstMessageContentViewControl();

        public XstMessageViewControl()
        {
            InitializeComponent();
            Initialize();
        }
        private void Initialize()
        {
            if (DesignMode) return;

            MessageContentControl.DoubleClickItem += (s, e) => AddTab(e?.Element);
            MessageContentControl.SelectedItemChanged += (s, e) => RaiseSelectedItemChanged(e.Element);
        }

        public event EventHandler<XstElementEventArgs>? SelectedItemChanged;
        private void RaiseSelectedItemChanged(XstElement? element) => SelectedItemChanged?.Invoke(this, new XstElementEventArgs(element));

        private XstMessage? _DataSource;
        public XstMessage? GetDataSource()
            => _DataSource;

        public void SetDataSource(XstMessage? dataSource)
        {
            if (dataSource != null && _DataSource != null && dataSource.GetHashCode() == _DataSource.GetHashCode())
                return;

            _DataSource = dataSource;

            if (dataSource != null)
            {
                if (MainKryptonNavigator.Pages.Count == 0)
                {
                    var page = new Krypton.Navigator.KryptonPage();
                    MainKryptonNavigator.Pages.Add(page);
                    page.Controls.Add(MessageContentControl);
                    MessageContentControl.Dock = DockStyle.Fill;
                }
                MainKryptonNavigator.Pages[0].Text = $"Message: {dataSource.Subject}";
                MainKryptonNavigator.SelectedIndex = 0;
                while (MainKryptonNavigator.Pages.Count > 1)
                    MainKryptonNavigator.Pages.RemoveAt(1);
            }
            MessageContentControl.SetDataSource(dataSource);
        }

        public XstElement? GetSelectedItem()
        {
            return _DataSource;
        }

        public void SetSelectedItem(XstElement? item)
        { }

        public void ClearContents()
        {
            MainKryptonNavigator.Pages.Clear();
            MessageContentControl.ClearContents();

            GetDataSource()?.ClearContents();
            SetDataSource(null);
        }

        private void AddTab(XstElement? element)
        {
            if (element == null)
                return;

            var page = MainKryptonNavigator.Pages.FirstOrDefault(p => p.Tag != null && ((int)p.Tag) == element.GetHashCode());
            if (page == null)
            {
                page = new Krypton.Navigator.KryptonPage
                {
                    Text = $"{element.ElementType}: {element.DisplayName}",
                    Tag = element.GetHashCode(),
                };
                MainKryptonNavigator.Pages.Add(page);

                if (element is XstAttachment attachment)
                {
                    if (attachment.IsEmail)
                    {
                        var viewer = new XstMessageContentViewControl();
                        page.Controls.Add(viewer);
                        viewer.Dock = DockStyle.Fill;
                        viewer.SetDataSource(attachment.AttachedEmailMessage);
                        viewer.SelectedItemChanged += (s, e) => RaiseSelectedItemChanged(e.Element);
                        viewer.DoubleClickItem += (s, e) => AddTab(e.Element);
                    }
                    else
                    {
                        var viewer = new XstAttachmentViewControl();
                        page.Controls.Add(viewer);
                        viewer.Dock = DockStyle.Fill;
                        viewer.SetDataSource(attachment);
                        viewer.SelectedItemChanged += (s, e) => RaiseSelectedItemChanged(e.Element);
                    }
                }
            }
            MainKryptonNavigator.SelectedPage = page;
        }
    }
}
