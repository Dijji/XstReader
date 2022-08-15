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
    public partial class XstPropertiesControl : UserControl,
                                                IXstDataSourcedControl<XstElement>
    {
        public XstPropertiesControl()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            if (DesignMode) return;

            PropertyGridProperties.SelectedGridItemChanged += (s, e)
                => PropDescWebView.DocumentText = (e.NewSelection.PropertyDescriptor as CustomXstPropertyDescriptor)?.HtmlDescription ?? "";

            PropDescWebView.DocumentCompleted += (s,e) => PropDescWebView.Document.Body.Style = "zoom:90%";
            SaveToolStripButton.Click += (s, e) => SaveProperties();
        }

        private XstElement? _DataSource;
        public XstElement? GetDataSource()
            => _DataSource;

        public void SetDataSource(XstElement? dataSource)
        {
            _DataSource = dataSource;
            LoadProperties();
        }

        private void LoadProperties()
        {
            ElementTypeLabel.Text = _DataSource?.ElementType.ToString();
            ElementNameLabel.Text = _DataSource?.ToString();

            PropertyGridProperties.SelectedObject = _DataSource?.Properties.ToPropertyGridSelectedObject();
        }

        public void ClearContents()
        {
            GetDataSource()?.ClearContents();
            SetDataSource(null);
        }

        private void SaveProperties()
        {
            if (_DataSource?.Properties == null)
                return;

            SaveFileDialog.FileName = $"{_DataSource}.csv";

            if (SaveFileDialog.ShowDialog() == DialogResult.OK)
                _DataSource.SavePropertiesToFile(SaveFileDialog.FileName);
        }

    }
}
