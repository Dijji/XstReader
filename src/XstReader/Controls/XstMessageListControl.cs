// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

using BrightIdeasSoftware;
using Krypton.Toolkit;
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

        private void Initialize()
        {
            if (DesignMode) return;

            ObjectListView.Columns.Add(new OLVColumn("Subject", nameof(XstMessage.Subject)) { WordWrap = true, FillsFreeSpace = true });
            ObjectListView.Columns.Add(new OLVColumn("From", nameof(XstMessage.From)) { Width = 150 });
            ObjectListView.Columns.Add(new OLVColumn("To", nameof(XstMessage.To)) { Width = 150 });
            ObjectListView.Columns.Add(new OLVColumn("Date", nameof(XstMessage.Date)) { Width = 150 });

            ObjectListView.FormatRow += (s, e) =>
            {
                if (e.Item.RowObject is XstMessage message)
                {
                    if (!message.IsRead)
                    {
                        e.Item.Font = new Font(Font, FontStyle.Bold);
                        e.Item.ForeColor = Color.Blue;
                    }
                }
            };

            ObjectListView.ItemSelectionChanged += (s, e) => RaiseSelectedItemChanged();

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

        private IEnumerable<XstMessage>? _DataSource;
        public IEnumerable<XstMessage>? GetDataSource()
            => _DataSource;

        public void SetDataSource(IEnumerable<XstMessage>? dataSource)
        {
            _DataSource = dataSource;
            ObjectListView.Objects = dataSource;
            RaiseSelectedItemChanged();
        }

        public XstMessage? GetSelectedItem()
        {
            return ObjectListView.SelectedItem?.RowObject as XstMessage;
        }
        public void SetSelectedItem(XstMessage? item)
        { }

        public void ClearContents()
        {
            GetSelectedItem()?.ClearContents();
            SetDataSource(null);
        }
    }
}
