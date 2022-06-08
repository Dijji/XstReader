
using XstReader.App.Common;

namespace XstReader.App.Controls
{
    public partial class XstFolderTreeControl : UserControl,
                                                IXstDataSourcedControl<XstFile>,
                                                IXstElementSelectable<XstFolder>
    {
        public XstFolderTreeControl()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            if (DesignMode) return;

            MainTreeView.AfterSelect += (s, e) => RaiseSelectedItemChanged();
            MainTreeView.GotFocus += (s, e) => OnGotFocus(e);
            SetDataSource(null);
        }

        public event EventHandler<XstElementEventArgs>? SelectedItemChanged;
        private void RaiseSelectedItemChanged() => SelectedItemChanged?.Invoke(this, new XstElementEventArgs(GetSelectedItem()));

        private XstFile? _DataSource;
        public XstFile? GetDataSource()
            => _DataSource;

        public void SetDataSource(XstFile? dataSource)
        {
            _DataSource = dataSource;
            LoadFolders();
            if (MainTreeView.Nodes.Count == 0)
                RaiseSelectedItemChanged();
        }
        public XstFolder? GetSelectedItem()
            => MainTreeView.SelectedNode?.Tag as XstFolder;


        public void SetSelectedItem(XstFolder? item)
            => MainTreeView.SelectedNode = (item != null && _DicMapFoldersNodes.ContainsKey(item.GetId())) ?
                                           _DicMapFoldersNodes[item.GetId()]
                                           : null;



        private Dictionary<string, TreeNode> _DicMapFoldersNodes = new Dictionary<string, TreeNode>();

        private void LoadFolders()
        {
            MainTreeView.Nodes.Clear();
            _DicMapFoldersNodes = new Dictionary<string, TreeNode>();
            if (_DataSource != null)
                MainTreeView.SelectedNode = AddFolderToTree(_DataSource.RootFolder, null);
        }

        private TreeNode AddFolderToTree(XstFolder folder, TreeNode? parentNode)
        {
            string name = $"{folder.ToString() ?? "<no_name>"} ({folder.ContentCount}|{folder.ContentUnreadCount})";
            var node = new TreeNode(name) { Tag = folder };
            _DicMapFoldersNodes[folder.GetId()] = node;

            if (parentNode == null) MainTreeView.Nodes.Add(node);
            else parentNode.Nodes.Add(node);

            foreach (var childFolder in folder.Folders)
                AddFolderToTree(childFolder, node);

            node.Expand();
            return node;
        }

        public void ClearContents()
        {
            GetSelectedItem()?.ClearContents();
            SetDataSource(null);
        }

    }
}
