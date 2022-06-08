namespace XstReader.App.Controls
{
    partial class XstFolderTreeControl
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
            this.MainTreeView = new Krypton.Toolkit.KryptonTreeView();
            this.SuspendLayout();
            // 
            // MainTreeView
            // 
            this.MainTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTreeView.HideSelection = false;
            this.MainTreeView.Location = new System.Drawing.Point(0, 0);
            this.MainTreeView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MainTreeView.Name = "MainTreeView";
            this.MainTreeView.Size = new System.Drawing.Size(200, 350);
            this.MainTreeView.TabIndex = 0;
            // 
            // XstFolderTreeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainTreeView);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(200, 350);
            this.Name = "XstFolderTreeControl";
            this.Size = new System.Drawing.Size(200, 350);
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonTreeView MainTreeView;
    }
}
