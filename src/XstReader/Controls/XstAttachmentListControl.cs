using BrightIdeasSoftware;
using System.Diagnostics;
using System.Runtime.InteropServices;
using XstReader.App.Common;

namespace XstReader.App.Controls
{
    public partial class XstAttachmentListControl : UserControl,
                                                    IXstDataSourcedControl<IEnumerable<XstAttachment>>,
                                                    IXstElementDoubleClickable<XstAttachment>
    {
        private static bool ShowHidden { get; set; } = true;

        public XstAttachmentListControl()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            if (DesignMode) return;

            ObjectListView.Columns.Add(new OLVColumn("File Name", nameof(XstAttachment.LongFileName)) { WordWrap = true, FillsFreeSpace = true });
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
            SaveAllToolStripButton.Click += (s, e) => SaveAllAttachments(_DataSource);
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
            var elems = _DataSource?.Where(a => ShowHidden || !a.IsHidden);
            ObjectListView.Objects = elems;
            SaveAllToolStripButton.Enabled = elems?.Any() ?? false;

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
                foreach (var attachment in attachments)
                    attachment.SaveToFolder(FolderBrowserDialog.SelectedPath);
        }

        private void OpenWith(XstAttachment? attachment)
        {
            if (attachment == null)
                return;

            string fileFullname = attachment.SaveToTempFile();
            if (string.IsNullOrEmpty(fileFullname))
                return;

            if (Environment.OSVersion.Version.Major > 5)
            {
                IntPtr hwndParent = Process.GetCurrentProcess().MainWindowHandle;
                tagOPENASINFO oOAI = new tagOPENASINFO
                {
                    cszFile = fileFullname,
                    cszClass = String.Empty,
                    oaifInFlags = tagOPEN_AS_INFO_FLAGS.OAIF_ALLOW_REGISTRATION | tagOPEN_AS_INFO_FLAGS.OAIF_EXEC
                };
                SHOpenWithDialog(hwndParent, ref oOAI);
            }
            else
            {
                using (Process.Start("rundll32", "shell32.dll,OpenAs_RunDLL " + fileFullname)) { }
            }
        }


        // Plumbing to enable access to SHOpenWithDialog
        [DllImport("shell32.dll", EntryPoint = "SHOpenWithDialog", CharSet = CharSet.Unicode)]
        private static extern int SHOpenWithDialog(IntPtr hWndParent, ref tagOPENASINFO oOAI);
        private struct tagOPENASINFO
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string cszFile;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string cszClass;

            [MarshalAs(UnmanagedType.I4)]
            public tagOPEN_AS_INFO_FLAGS oaifInFlags;
        }
        [Flags]
        private enum tagOPEN_AS_INFO_FLAGS
        {
            OAIF_ALLOW_REGISTRATION = 0x00000001,   // Show "Always" checkbox
            OAIF_REGISTER_EXT = 0x00000002,   // Perform registration when user hits OK
            OAIF_EXEC = 0x00000004,   // Exec file after registering
            OAIF_FORCE_REGISTRATION = 0x00000008,   // Force the checkbox to be registration
            OAIF_HIDE_REGISTRATION = 0x00000020,   // Vista+: Hide the "always use this file" checkbox
            OAIF_URL_PROTOCOL = 0x00000040,   // Vista+: cszFile is actually a URI scheme; show handlers for that scheme
            OAIF_FILE_IS_URI = 0x00000080    // Win8+: The location pointed to by the pcszFile parameter is given as a URI
        }

    }
}
