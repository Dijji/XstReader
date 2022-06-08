using BrightIdeasSoftware;
using XstReader.App.Common;

namespace XstReader.App.Controls
{
    public partial class XstRecipientListControl : UserControl,
                                                   IXstDataSourcedControl<IEnumerable<XstRecipient>>,
                                                   IXstElementDoubleClickable<XstRecipient>
    {
        public XstRecipientListControl()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            if (DesignMode) return;

            ObjectListView.Columns.Add(new OLVColumn("Type", nameof(XstRecipient.RecipientType)) { Width = 100 });
            ObjectListView.Columns.Add(new OLVColumn("Name", nameof(XstRecipient.DisplayName)) { WordWrap = true, Width = 150 });
            ObjectListView.Columns.Add(new OLVColumn("Address", nameof(XstRecipient.Address)) { Width = 150, FillsFreeSpace = true });

            ObjectListView.ItemSelectionChanged += (s, e) => RaiseSelectedItemChanged();
            ObjectListView.DoubleClick += (s, e) => RaiseDoubleClickItem(GetSelectedItem());

            SetDataSource(null);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ObjectListView.BackColor = this.BackColor;
            ObjectListView.Font = this.Font;
            ObjectListView.ForeColor = this.ForeColor;
        }

        public event EventHandler<XstElementEventArgs>? SelectedItemChanged;
        private void RaiseSelectedItemChanged() => SelectedItemChanged?.Invoke(this, new XstElementEventArgs(GetSelectedItem()));

        public event EventHandler<XstElementEventArgs>? DoubleClickItem;
        private void RaiseDoubleClickItem(XstRecipient? element) => DoubleClickItem?.Invoke(this, new XstElementEventArgs(element));


        private IEnumerable<XstRecipient>? _DataSource;
        public IEnumerable<XstRecipient>? GetDataSource()
            => _DataSource;

        public void SetDataSource(IEnumerable<XstRecipient>? dataSource)
        {
            _DataSource = dataSource;
            ObjectListView.Objects = dataSource;
            RaiseSelectedItemChanged();
        }

        public XstRecipient? GetSelectedItem()
        {
            return ObjectListView.SelectedItem?.RowObject as XstRecipient;
        }
        public void SetSelectedItem(XstRecipient? item)
        { }

        public void ClearContents()
        {
            GetSelectedItem()?.ClearContents();
            SetDataSource(null);
        }
    }
}
