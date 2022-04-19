using XstReader.App.Common;

namespace XstReader.App.Controls
{
    public partial class XstMessageListControl : UserControl,
                                                 IXstDataSourcedControl<IEnumerable<XstMessage>>,
                                                 IXstElementSelectable<XstMessage>
    {
        public XstMessageListControl()
        {
            InitializeComponent();
            Initialize();
        }

        public int GetGridWidth()
        {
            foreach (var ctrl in DataGridView.Controls)
                if (ctrl is HScrollBar scrollBar)
                    if (scrollBar.Visible) return scrollBar.Maximum; else break;

            int gridWidth = 0;
            for (int i = 0; i < DataGridView.ColumnCount; i++)
                gridWidth += DataGridView.Columns[i].Width;

            return gridWidth;
        }

        private int? _CurrentRowIndex = null;
        private void Initialize()
        {
            if (DesignMode) return;

            DataGridView.CellEnter += (s, e) =>
            {
                if (e.RowIndex != _CurrentRowIndex)
                {
                    _CurrentRowIndex = e.RowIndex;
                    RaiseSelectedItemChanged();
                }
            };
            DataGridView.Sorted += (s, e) => RaiseSelectedItemChanged();
            DataGridView.GotFocus += (s, e) => OnGotFocus(e);

            //foreach (var ctrl in DataGridView.Controls)
            //    if (ctrl is HScrollBar scrollBar)
            //        scrollBar.VisibleChanged += (s, e) => MessageBox.Show(scrollBar.Maximum.ToString());
            /*
             For Each ctrl As Control In DataGridView1.Controls
            If TypeOf (ctrl) Is VScrollBar Then
                AddHandler ctrl.VisibleChanged, AddressOf VScrollBar1_VisibleChanged
            End If
        Next
        Dim bl As New BindingList(Of BindTo)
        For x As Integer = 0 To 3
            Dim bt As New BindTo
            bt.NewProperty = x.ToString
            bl.Add(bt)
        Next
            */
        }

        public event EventHandler<XstElementEventArgs>? SelectedItemChanged;
        private void RaiseSelectedItemChanged() => SelectedItemChanged?.Invoke(this, new XstElementEventArgs(GetSelectedItem()));

        private IEnumerable<XstMessage>? _DataSource;
        public IEnumerable<XstMessage>? GetDataSource()
            => _DataSource;

        private Func<XstMessage, bool>? _CurrentFilter = null;
        public void ApplyFilter(Func<XstMessage, bool>? filter)
        {
            _CurrentFilter = filter;
            DataGridView.DataSource = (_CurrentFilter == null) ? _DataSource?.ToList() : _DataSource?.Where(_CurrentFilter).ToList();
            RaiseSelectedItemChanged();
        }
        public void SetDataSource(IEnumerable<XstMessage>? dataSource)
        {
            _DataSource = dataSource;
            DataGridView.DataSource = dataSource?.ToList();
            RaiseSelectedItemChanged();
        }
        public void SetDataSource(IEnumerable<XstMessage>? dataSource, Func<XstMessage, bool>? filter)
        {
            _DataSource = dataSource;
            _CurrentFilter = filter;
            DataGridView.DataSource = (_CurrentFilter == null) ? _DataSource?.ToList() : _DataSource?.Where(_CurrentFilter).ToList();
            RaiseSelectedItemChanged();
        }

        public XstMessage? GetSelectedItem()
        {
            if (_CurrentRowIndex == null) return null;
            if (_CurrentRowIndex.Value >= DataGridView.Rows.Count) return null;
            return DataGridView.Rows[_CurrentRowIndex.Value].DataBoundItem as XstMessage;
        }
        public void SetSelectedItem(XstMessage? item)
        { }
        //=> MainTreeView.SelectedNode = (item != null && _DicMapFoldersNodes.ContainsKey(item.GetId())) ?
        //                                   _DicMapFoldersNodes[item.GetId()]
        //                                   : null;

    }
}
