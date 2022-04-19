using System.ComponentModel;
using XstReader.App.Common;

namespace XstReader.App.Controls
{
    public partial class XstRecipientListControl : UserControl,
                                                   IXstDataSourcedControl<IEnumerable<XstRecipient>>,
                                                   IXstElementSelectable<XstRecipient>
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
            DataGridView.Sorted += (s, e) => RaiseSelectedItemChanged();
            DataGridView.GotFocus += (s, e) => OnGotFocus(e);
        }

        public event EventHandler<XstElementEventArgs>? SelectedItemChanged;
        private void RaiseSelectedItemChanged() => SelectedItemChanged?.Invoke(this, new XstElementEventArgs(GetSelectedItem()));

        private IEnumerable<XstRecipient>? _DataSource;
        public IEnumerable<XstRecipient>? GetDataSource()
            => _DataSource;

        public void SetDataSource(IEnumerable<XstRecipient>? dataSource)
        {
            _DataSource = dataSource;
            DataGridView.DataSource = dataSource?.ToList();
            DataGridView.StretchLastColumn();
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
        //=> MainTreeView.SelectedNode = (item != null && _DicMapFoldersNodes.ContainsKey(item.GetId())) ?
        //                                   _DicMapFoldersNodes[item.GetId()]
        //                                   : null;

    }
}
