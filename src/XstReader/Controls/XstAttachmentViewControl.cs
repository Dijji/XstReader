// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

using XstReader.App.Common;

namespace XstReader.App.Controls
{
    public partial class XstAttachmentViewControl : UserControl,
                                                    IXstDataSourcedControl<XstAttachment>,
                                                    IXstElementSelectable<XstAttachment>
    {
        public XstAttachmentViewControl()
        {
            InitializeComponent();
            Initialize();
        }
        private void Initialize()
        {
            if (DesignMode) return;
        }

        public event EventHandler<XstElementEventArgs>? SelectedItemChanged;
        private void RaiseSelectedItemChanged() => RaiseSelectedItemChanged(GetSelectedItem());
        private void RaiseSelectedItemChanged(XstElement? element) => SelectedItemChanged?.Invoke(this, new XstElementEventArgs(element));

        private XstAttachment? _DataSource;
        public XstAttachment? GetDataSource()
            => _DataSource;

        public void SetDataSource(XstAttachment? dataSource)
        {
            CleanTempFile();
            _DataSource = dataSource;

            try
            {
                string fileName = _DataSource?.SaveToTempFile() ?? "";
                KryptonWebBrowser.Url= new Uri(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error showing message");
            }
        }

        public XstAttachment? GetSelectedItem()
        {
            return _DataSource;
        }

        public void SetSelectedItem(XstAttachment? item)
        { }

        private string? _TempFileName = null;
        private void CleanTempFile()
        {
            if (_TempFileName != null && File.Exists(_TempFileName))
                try { File.Delete(_TempFileName); }
                catch { }
            _TempFileName = null;
        }

        public void ClearContents()
        {
            GetDataSource()?.ClearContents();
            SetDataSource(null);
        }

    }
}
