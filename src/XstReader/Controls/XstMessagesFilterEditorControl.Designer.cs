namespace XstReader.App.Controls
{
    partial class XstMessagesFilterEditorControl
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
            this.SearchCancelButton = new System.Windows.Forms.Button();
            this.SearchButton = new System.Windows.Forms.Button();
            this.SearchText = new System.Windows.Forms.TextBox();
            this.SearchLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SearchCancelButton
            // 
            this.SearchCancelButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.SearchCancelButton.Enabled = false;
            this.SearchCancelButton.Location = new System.Drawing.Point(252, 0);
            this.SearchCancelButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SearchCancelButton.Name = "SearchCancelButton";
            this.SearchCancelButton.Size = new System.Drawing.Size(82, 22);
            this.SearchCancelButton.TabIndex = 7;
            this.SearchCancelButton.Text = "Clear";
            this.SearchCancelButton.UseVisualStyleBackColor = true;
            // 
            // SearchButton
            // 
            this.SearchButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.SearchButton.Enabled = false;
            this.SearchButton.Location = new System.Drawing.Point(170, 0);
            this.SearchButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(82, 22);
            this.SearchButton.TabIndex = 6;
            this.SearchButton.Text = "Filter";
            this.SearchButton.UseVisualStyleBackColor = true;
            // 
            // SearchText
            // 
            this.SearchText.Dock = System.Windows.Forms.DockStyle.Left;
            this.SearchText.Location = new System.Drawing.Point(60, 0);
            this.SearchText.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SearchText.Name = "SearchText";
            this.SearchText.Size = new System.Drawing.Size(110, 23);
            this.SearchText.TabIndex = 5;
            // 
            // SearchLabel
            // 
            this.SearchLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.SearchLabel.Location = new System.Drawing.Point(0, 0);
            this.SearchLabel.Name = "SearchLabel";
            this.SearchLabel.Size = new System.Drawing.Size(60, 22);
            this.SearchLabel.TabIndex = 4;
            this.SearchLabel.Text = "Filter:";
            this.SearchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // XstMessagesFilterEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SearchCancelButton);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.SearchText);
            this.Controls.Add(this.SearchLabel);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(500, 22);
            this.Name = "XstMessagesFilterEditorControl";
            this.Size = new System.Drawing.Size(500, 22);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button SearchCancelButton;
        private Button SearchButton;
        private TextBox SearchText;
        private Label SearchLabel;
    }
}
