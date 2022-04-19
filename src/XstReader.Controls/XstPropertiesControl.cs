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

            PropDescWebView.EnsureCoreWebView2Async();
            PropertyGridProperties.SelectedGridItemChanged += (s, e) 
                => PropDescWebView.NavigateToString((e.NewSelection.PropertyDescriptor as CustomXstPropertyDescriptor)?.HtmlDescription??"");
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
            ElementTypeLabel.Text = _DataSource?.GetType().Name?.Replace("Xst","");
            ElementNameLabel.Text = _DataSource?.ToString();

            PropertyGridInfo.SelectedObject = _DataSource;
            PropertyGridProperties.SelectedObject = _DataSource?.Properties.ToPropertyGridSelectedObject();
        }
    }
}
