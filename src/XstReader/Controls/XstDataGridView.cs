// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

//Adapted from https://stackoverflow.com/questions/1377568/c-sharp-datagridview-sorting-with-generic-list-as-underlying-source


using System.ComponentModel;

namespace XstReader.App.Controls
{
    internal class XstDataGridView : DataGridView
    {
        private string? _lastSortColumn;
        private ListSortDirection _lastSortDirection;

        public XstDataGridView() : base()
        {
            RowHeadersVisible = false;
            
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            MultiSelect = false;
            RowPrePaint += (s,e)=> e.PaintParts &= ~DataGridViewPaintParts.Focus;

            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AllowUserToOrderColumns = true;
            AllowUserToResizeColumns = true;
            ReadOnly = true;

            BackgroundColor = Color.FromKnownColor(KnownColor.Window);
        }

        public void Sort(DataGridViewColumn column)
        {
            // Flip sort direction, if the column chosen was the same as last time
            if (column.Name == _lastSortColumn)
                _lastSortDirection = 1 - _lastSortDirection;
            // Otherwise, reset the sort direction to its default, ascending
            else
            {
                _lastSortColumn = column.Name;
                _lastSortDirection = ListSortDirection.Ascending;
            }

            // Prep data for sorting
            var data = (IEnumerable<dynamic>)DataSource;
            var orderProperty = column.DataPropertyName;

            if (data != null)
            {
                // Sort data
                if (_lastSortDirection == ListSortDirection.Ascending)
                    DataSource = data.OrderBy(x => x.GetType().GetProperty(orderProperty).GetValue(x, null)).ToList();
                else
                    DataSource = data.OrderByDescending(x => x.GetType().GetProperty(orderProperty).GetValue(x, null)).ToList();
            }
            // Set direction of the glyph
            Columns[column.Index].HeaderCell.SortGlyphDirection = 
                _lastSortDirection == ListSortDirection.Ascending 
                ? SortOrder.Ascending 
                : SortOrder.Descending;

            OnSorted(new DataGridViewColumnEventArgs(column));
            //OnCellEnter(new DataGridViewCellEventArgs(CurrentCell.ColumnIndex, CurrentCell.RowIndex));
        }

        public void StretchLastColumn()
        {
            var lastColIndex = Columns.Count - 1;
            var lastCol = Columns[lastColIndex];
            lastCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        protected override void OnColumnHeaderMouseClick(DataGridViewCellMouseEventArgs e)
        {
            base.OnColumnHeaderMouseClick(e);

            var column = Columns[e.ColumnIndex];
            if (column.SortMode == DataGridViewColumnSortMode.Automatic || 
                column.SortMode == DataGridViewColumnSortMode.NotSortable)
                Sort(column);
        }
       
    }
}
