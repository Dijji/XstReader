namespace XstReader.App.Controls
{
    partial class XstRecipientListControl
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
            this.xstRecipientBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.recipientTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.displayNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xstRecipientBindingSource)).BeginInit();
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
            this.recipientTypeDataGridViewTextBoxColumn,
            this.addressDataGridViewTextBoxColumn,
            this.displayNameDataGridViewTextBoxColumn});
            this.DataGridView.DataSource = this.xstRecipientBindingSource;
            this.DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView.Location = new System.Drawing.Point(0, 0);
            this.DataGridView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DataGridView.Name = "DataGridView";
            this.DataGridView.ReadOnly = true;
            this.DataGridView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DataGridView.RowHeadersVisible = false;
            this.DataGridView.RowHeadersWidth = 51;
            this.DataGridView.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridView.RowTemplate.Height = 20;
            this.DataGridView.Size = new System.Drawing.Size(659, 153);
            this.DataGridView.TabIndex = 1;
            // 
            // xstRecipientBindingSource
            // 
            this.xstRecipientBindingSource.DataSource = typeof(XstReader.XstRecipient);
            // 
            // recipientTypeDataGridViewTextBoxColumn
            // 
            this.recipientTypeDataGridViewTextBoxColumn.DataPropertyName = "RecipientType";
            this.recipientTypeDataGridViewTextBoxColumn.HeaderText = "RecipientType";
            this.recipientTypeDataGridViewTextBoxColumn.MinimumWidth = 150;
            this.recipientTypeDataGridViewTextBoxColumn.Name = "recipientTypeDataGridViewTextBoxColumn";
            this.recipientTypeDataGridViewTextBoxColumn.ReadOnly = true;
            this.recipientTypeDataGridViewTextBoxColumn.Width = 150;
            // 
            // addressDataGridViewTextBoxColumn
            // 
            this.addressDataGridViewTextBoxColumn.DataPropertyName = "Address";
            this.addressDataGridViewTextBoxColumn.HeaderText = "Address";
            this.addressDataGridViewTextBoxColumn.MinimumWidth = 150;
            this.addressDataGridViewTextBoxColumn.Name = "addressDataGridViewTextBoxColumn";
            this.addressDataGridViewTextBoxColumn.ReadOnly = true;
            this.addressDataGridViewTextBoxColumn.Width = 150;
            // 
            // displayNameDataGridViewTextBoxColumn
            // 
            this.displayNameDataGridViewTextBoxColumn.DataPropertyName = "DisplayName";
            this.displayNameDataGridViewTextBoxColumn.HeaderText = "Display Name";
            this.displayNameDataGridViewTextBoxColumn.MinimumWidth = 200;
            this.displayNameDataGridViewTextBoxColumn.Name = "displayNameDataGridViewTextBoxColumn";
            this.displayNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.displayNameDataGridViewTextBoxColumn.Width = 200;
            // 
            // XstRecipientListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DataGridView);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "XstRecipientListControl";
            this.Size = new System.Drawing.Size(659, 153);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xstRecipientBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private XstDataGridView DataGridView;
        private BindingSource xstRecipientBindingSource;
        private DataGridViewTextBoxColumn recipientTypeDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn addressDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn displayNameDataGridViewTextBoxColumn;
    }
}
