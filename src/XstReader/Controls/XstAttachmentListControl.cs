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
using XstReader.App.Common;
using XstReader.App.Helpers;

namespace XstReader.App.Controls
{
    public partial class XstAttachmentListControl : UserControl,
                                                    IXstDataSourcedControl<IEnumerable<XstAttachment>>,
                                                    IXstElementDoubleClickable<XstAttachment>
    {
        private static bool ShowHidden
        {
            get => XstReaderEnvironment.Options.ShowHiddenAttachments;
            set => XstReaderEnvironment.Options.ShowHiddenAttachments = value;
        }

        public XstAttachmentListControl()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            if (DesignMode) return;

            ObjectListView.Columns.Add(new OLVColumn("Display Name", nameof(XstAttachment.DisplayName)) { WordWrap = true, FillsFreeSpace = true });
            ObjectListView.Columns.Add(new OLVColumn("Size", nameof(XstAttachment.Size)) { Width = 70 });
            ObjectListView.Columns.Add(new OLVColumn("Type", nameof(XstAttachment.Type)) { Width = 100, Groupable = true });
            ObjectListView.Columns.Add(new OLVColumn("Last modification", nameof(XstAttachment.LastModificationTime)) { Width = 100 });

            ShowHiddenToolStripButton.Checked = ShowHidden;
            ShowHiddenToolStripButton.CheckedChanged += (s, e) =>
            {
                ShowHidden = ShowHiddenToolStripButton.Checked;
                RefreshFilters();
            };

            ObjectListView.ItemSelectionChanged += (s, e) => RaiseSelectedItemChanged();
            ObjectListView.DoubleClick += (s, e) => RaiseDoubleClickItem(GetSelectedItem());

            SaveToolStripButton.Click += (s, e) => SaveAttachment(GetSelectedItem());
            SaveAllToolStripButton.Click += (s, e) => SaveAllAttachments(_FilterdDataSource);
            OpenInAppToolStripButton.Click += (s, e) => RaiseDoubleClickItem(GetSelectedItem());
            OpenWithToolStripMenuItem.Click += (s, e) => OpenWith(GetSelectedItem());

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
        private void RaiseDoubleClickItem(XstAttachment? element)
        {
            if (element?.CanBeOpenedInApp() ?? false)
                DoubleClickItem?.Invoke(this, new XstElementEventArgs(element));
            else
                OpenWithToolStripMenuItem.PerformClick();
        }


        private IEnumerable<XstAttachment>? _DataSource;
        private IEnumerable<XstAttachment>? _FilterdDataSource;
        public IEnumerable<XstAttachment>? GetDataSource()
            => _DataSource;

        public void SetDataSource(IEnumerable<XstAttachment>? dataSource)
        {
            SaveToolStripButton.Enabled = false;
            OpenInAppToolStripButton.Enabled = false;
            OpenWithToolStripMenuItem.Enabled = false;

            _DataSource = dataSource;
            RefreshFilters();
        }

        private void RefreshFilters()
        {
            _FilterdDataSource = _DataSource?.Where(a => ShowHidden || !a.IsHidden);
            ObjectListView.Objects = _FilterdDataSource;
            SaveAllToolStripButton.Enabled = _FilterdDataSource?.Any() ?? false;

            RaiseSelectedItemChanged();
        }

        public XstAttachment? GetSelectedItem()
        {
            var attachment = ObjectListView.SelectedItem?.RowObject as XstAttachment;
            SaveToolStripButton.Enabled = attachment != null;
            OpenInAppToolStripButton.Enabled = attachment?.CanBeOpenedInApp() ?? false;
            OpenWithToolStripMenuItem.Enabled = attachment?.IsFile ?? false;

            return attachment;
        }
        public void SetSelectedItem(XstAttachment? item)
        { }

        public void ClearContents()
        {
            GetSelectedItem()?.ClearContents();
            SetDataSource(null);
        }

        private void SaveAttachment(XstAttachment? attachment)
        {
            if (attachment == null)
                return;

            SaveFileDialog.FileName = attachment.LongFileName;

            if (SaveFileDialog.ShowDialog() == DialogResult.OK)
                attachment.SaveToFile(SaveFileDialog.FileName);
        }

        private void SaveAllAttachments(IEnumerable<XstAttachment>? attachments)
        {
            if (!(attachments?.Any() ?? false))
                return;

            if (FolderBrowserDialog.ShowDialog() == DialogResult.OK)
                foreach (var attachment in attachments.OrderBy(a => a.LastModificationTime))
                    attachment.SaveToFolder(FolderBrowserDialog.SelectedPath);
        }

        private void OpenWith(XstAttachment? attachment)
        {
            SystemHelper.OpenWith(attachment?.SaveToTempFile());
        }

    }
}
