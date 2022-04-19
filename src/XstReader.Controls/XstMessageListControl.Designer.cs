namespace XstReader.App.Controls
{
    partial class XstMessageListControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.DataGridView = new XstReader.App.Controls.XstDataGridView();
            this.subjectDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.From = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.To = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xstMessageBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xstMessageBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // DataGridView
            // 
            this.DataGridView.AllowUserToAddRows = false;
            this.DataGridView.AllowUserToDeleteRows = false;
            this.DataGridView.AllowUserToOrderColumns = true;
            this.DataGridView.AutoGenerateColumns = false;
            this.DataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader;
            this.DataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.subjectDataGridViewTextBoxColumn,
            this.From,
            this.To,
            this.dateDataGridViewTextBoxColumn});
            this.DataGridView.DataSource = this.xstMessageBindingSource;
            this.DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView.Location = new System.Drawing.Point(0, 0);
            this.DataGridView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DataGridView.MultiSelect = false;
            this.DataGridView.Name = "DataGridView";
            this.DataGridView.ReadOnly = true;
            this.DataGridView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DataGridView.RowHeadersVisible = false;
            this.DataGridView.RowHeadersWidth = 50;
            this.DataGridView.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridView.Size = new System.Drawing.Size(484, 558);
            this.DataGridView.TabIndex = 1;
            // 
            // subjectDataGridViewTextBoxColumn
            // 
            this.subjectDataGridViewTextBoxColumn.DataPropertyName = "Subject";
            this.subjectDataGridViewTextBoxColumn.HeaderText = "Subject";
            this.subjectDataGridViewTextBoxColumn.MinimumWidth = 250;
            this.subjectDataGridViewTextBoxColumn.Name = "subjectDataGridViewTextBoxColumn";
            this.subjectDataGridViewTextBoxColumn.ReadOnly = true;
            this.subjectDataGridViewTextBoxColumn.Width = 250;
            // 
            // From
            // 
            this.From.DataPropertyName = "From";
            this.From.HeaderText = "From";
            this.From.MinimumWidth = 50;
            this.From.Name = "From";
            this.From.ReadOnly = true;
            this.From.Width = 50;
            // 
            // To
            // 
            this.To.DataPropertyName = "To";
            this.To.HeaderText = "To";
            this.To.MinimumWidth = 50;
            this.To.Name = "To";
            this.To.ReadOnly = true;
            this.To.Width = 50;
            // 
            // dateDataGridViewTextBoxColumn
            // 
            this.dateDataGridViewTextBoxColumn.DataPropertyName = "Date";
            this.dateDataGridViewTextBoxColumn.HeaderText = "Date";
            this.dateDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.dateDataGridViewTextBoxColumn.Name = "dateDataGridViewTextBoxColumn";
            this.dateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // xstMessageBindingSource
            // 
            this.xstMessageBindingSource.DataSource = typeof(XstReader.XstMessage);
            // 
            // XstMessageListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DataGridView);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "XstMessageListControl";
            this.Size = new System.Drawing.Size(484, 558);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xstMessageBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private XstDataGridView DataGridView;
        private BindingSource xstMessageBindingSource;
        private DataGridViewTextBoxColumn subjectDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn From;
        private DataGridViewTextBoxColumn To;
        private DataGridViewTextBoxColumn dateDataGridViewTextBoxColumn;
    }
}
