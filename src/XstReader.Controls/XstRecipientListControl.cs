using System.ComponentModel;
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
        private int? _CurrentRowIndex = null;
        private void Initialize()
        {
            if (DesignMode) return;

            //AdvDataGridViewSearchToolBar.SetColumns(AdvDataGridViewSearchToolBar.getCol)
            DataGridView.RowEnter += (s, e) =>
            {
                if (e.RowIndex != _CurrentRowIndex)
                {
                    _CurrentRowIndex = e.RowIndex;
                    RaiseSelectedItemChanged();
                }
            };
            DataGridView.CellMouseDoubleClick += (s, e) => RaiseDoubleClickItem(GetSelectedItem());
            DataGridView.Sorted += (s, e) => RaiseSelectedItemChanged();
            DataGridView.GotFocus += (s, e) => OnGotFocus(e);
            DataGridView.StretchLastColumn();
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
            DataGridView.DataSource = dataSource?.ToList();
            RaiseSelectedItemChanged();
        }
        public XstRecipient? GetSelectedItem()
        {
            if (_CurrentRowIndex == null) return null;
            if (_CurrentRowIndex.Value >= DataGridView.Rows.Count) return null;
            return DataGridView.Rows[_CurrentRowIndex.Value].DataBoundItem as XstRecipient;
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
