namespace XstReader.App.Controls
{
    partial class XstPropertiesInfoControl
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
            this.ElementTypeLabel = new Krypton.Toolkit.KryptonLabel();
            this.ElementNameLabel = new Krypton.Toolkit.KryptonLabel();
            this.PropertyGridInfo = new Krypton.Toolkit.KryptonPropertyGrid();
            this.SuspendLayout();
            // 
            // ElementTypeLabel
            // 
            this.ElementTypeLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ElementTypeLabel.LabelStyle = Krypton.Toolkit.LabelStyle.ItalicControl;
            this.ElementTypeLabel.Location = new System.Drawing.Point(0, 0);
            this.ElementTypeLabel.Name = "ElementTypeLabel";
            this.ElementTypeLabel.Size = new System.Drawing.Size(350, 20);
            this.ElementTypeLabel.TabIndex = 0;
            this.ElementTypeLabel.Values.Text = "Type";
            // 
            // ElementNameLabel
            // 
            this.ElementNameLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ElementNameLabel.LabelStyle = Krypton.Toolkit.LabelStyle.BoldPanel;
            this.ElementNameLabel.Location = new System.Drawing.Point(0, 20);
            this.ElementNameLabel.Name = "ElementNameLabel";
            this.ElementNameLabel.Size = new System.Drawing.Size(350, 20);
            this.ElementNameLabel.TabIndex = 1;
            this.ElementNameLabel.Values.Text = "Type";
            // 
            // PropertyGridInfo
            // 
            this.PropertyGridInfo.CategoryForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.PropertyGridInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyGridInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PropertyGridInfo.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(230)))), ((int)(((byte)(232)))));
            this.PropertyGridInfo.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.PropertyGridInfo.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(188)))), ((int)(((byte)(193)))));
            this.PropertyGridInfo.Location = new System.Drawing.Point(0, 40);
            this.PropertyGridInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PropertyGridInfo.Name = "PropertyGridInfo";
            this.PropertyGridInfo.Size = new System.Drawing.Size(350, 460);
            this.PropertyGridInfo.TabIndex = 0;
            this.PropertyGridInfo.ToolbarVisible = false;
            // 
            // XstPropertiesInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PropertyGridInfo);
            this.Controls.Add(this.ElementNameLabel);
            this.Controls.Add(this.ElementTypeLabel);
            this.MinimumSize = new System.Drawing.Size(100, 200);
            this.Name = "XstPropertiesInfoControl";
            this.Size = new System.Drawing.Size(350, 500);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Krypton.Toolkit.KryptonLabel ElementTypeLabel;
        private Krypton.Toolkit.KryptonLabel ElementNameLabel;
        private Krypton.Toolkit.KryptonPropertyGrid PropertyGridInfo;
    }
}
