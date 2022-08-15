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
            this.ObjectListView = new BrightIdeasSoftware.ObjectListView();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectListView)).BeginInit();
            this.SuspendLayout();
            // 
            // ObjectListView
            // 
            this.ObjectListView.AllowColumnReorder = true;
            this.ObjectListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ObjectListView.EmptyListMsg = "";
            this.ObjectListView.FullRowSelect = true;
            this.ObjectListView.GridLines = true;
            this.ObjectListView.IncludeColumnHeadersInCopy = true;
            this.ObjectListView.Location = new System.Drawing.Point(0, 0);
            this.ObjectListView.MultiSelect = false;
            this.ObjectListView.Name = "ObjectListView";
            this.ObjectListView.ShowGroups = false;
            this.ObjectListView.ShowItemToolTips = true;
            this.ObjectListView.Size = new System.Drawing.Size(300, 150);
            this.ObjectListView.TabIndex = 2;
            this.ObjectListView.UseFilterIndicator = true;
            this.ObjectListView.UseFiltering = true;
            this.ObjectListView.View = System.Windows.Forms.View.Details;
            // 
            // XstRecipientListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ObjectListView);
            this.MinimumSize = new System.Drawing.Size(200, 100);
            this.Name = "XstRecipientListControl";
            this.Size = new System.Drawing.Size(300, 150);
            ((System.ComponentModel.ISupportInitialize)(this.ObjectListView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView ObjectListView;
    }
}
