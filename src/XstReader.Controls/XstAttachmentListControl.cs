using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XstReader.App.Common;

namespace XstReader.App.Controls
{
    public partial class XstAttachmentListControl : UserControl,
                                                    IXstDataSourcedControl<IEnumerable<XstAttachment>>,
                                                    IXstElementSelectable<XstAttachment>
    {
        public XstAttachmentListControl()
        {
            InitializeComponent();
            Initialize();
        }
        private int? _CurrentRowIndex = null;
        private void Initialize()
        {
            if (DesignMode) return;

            //AdvDataGridViewSearchToolBar.SetColumns(AdvDataGridViewSearchToolBar.getCol)
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
        }

        public event EventHandler<XstElementEventArgs>? SelectedItemChanged;
        private void RaiseSelectedItemChanged() => SelectedItemChanged?.Invoke(this, new XstElementEventArgs(GetSelectedItem()));

        private IEnumerable<XstAttachment>? _DataSource;
        public IEnumerable<XstAttachment>? GetDataSource()
            => _DataSource;

        public void SetDataSource(IEnumerable<XstAttachment>? dataSource)
        {
            _DataSource = dataSource;
            DataGridView.DataSource = dataSource?.ToList();
            DataGridView.StretchLastColumn();
            RaiseSelectedItemChanged();
        }
        public XstAttachment? GetSelectedItem()
        {
            if (_CurrentRowIndex == null) return null;
            if (_CurrentRowIndex.Value >= DataGridView.Rows.Count) return null;
            return DataGridView.Rows[_CurrentRowIndex.Value].DataBoundItem as XstAttachment;
        }
        public void SetSelectedItem(XstAttachment? item)
        { }
        //=> MainTreeView.SelectedNode = (item != null && _DicMapFoldersNodes.ContainsKey(item.GetId())) ?
        //                                   _DicMapFoldersNodes[item.GetId()]
        //                                   : null;

    }
}
