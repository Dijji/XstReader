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
    public partial class XstMessagesFilterEditorControl : UserControl,
                                                          IXstDataSourcedControl<IEnumerable<XstMessage>>
    {
        public XstMessagesFilterEditorControl()
        {
            InitializeComponent();
            Initialize();
        }
        private void Initialize()
        {
            if (DesignMode) return;
            
            SearchText.TextChanged += (s, e) => SearchButton.Enabled = !string.IsNullOrWhiteSpace(SearchText.Text);
            SearchButton.Click += (s, e) =>
            {
                var text = SearchText.Text?.Trim();
                if (string.IsNullOrWhiteSpace(text))
                    CurrentFilter = null;
                else
                    CurrentFilter = m => (m.Subject ?? "").ContainsIgnoringSymbols(text) ||
                                         (m.To ?? "").ContainsIgnoringSymbols(text) ||
                                         (m.From ?? "").ContainsIgnoringSymbols(text);
            };
            SearchCancelButton.Click += (s, e) =>
            {
                SearchText.Text = "";
                CurrentFilter = null;
            };
        }

        private Func<XstMessage, bool>? _CurrentFilter = null;
        private Func<XstMessage, bool>? CurrentFilter
        {
            get => _CurrentFilter;
            set
            {
                _CurrentFilter = value;
                SearchCancelButton.Enabled = _CurrentFilter != null;
                RaiseFilterChanged();
            }
        }


        private IEnumerable<XstMessage>? _DataSource;

        public event EventHandler<XstElementFilterEventArgs<XstMessage>>? FilterChanged;
        private void RaiseFilterChanged() => FilterChanged?.Invoke(this, new XstElementFilterEventArgs<XstMessage>(GetSelectedFilter()));

        public IEnumerable<XstMessage>? GetDataSource()
            => _DataSource;
        public void SetDataSource(IEnumerable<XstMessage>? dataSource)
        {
            _DataSource = dataSource;
        }

        public Func<XstMessage, bool>? GetSelectedFilter()
            => CurrentFilter;

        public void ClearContents()
        {
            SetDataSource(null);
        }

    }
}
