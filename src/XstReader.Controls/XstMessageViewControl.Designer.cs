namespace XstReader.App.Controls
{
    partial class XstMessageViewControl
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
            this.MainKryptonNavigator = new Krypton.Navigator.KryptonNavigator();
            ((System.ComponentModel.ISupportInitialize)(this.MainKryptonNavigator)).BeginInit();
            this.SuspendLayout();
            // 
            // MainKryptonNavigator
            // 
            this.MainKryptonNavigator.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.MainKryptonNavigator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainKryptonNavigator.Location = new System.Drawing.Point(0, 0);
            this.MainKryptonNavigator.Name = "MainKryptonNavigator";
            this.MainKryptonNavigator.Size = new System.Drawing.Size(683, 554);
            this.MainKryptonNavigator.TabIndex = 1;
            this.MainKryptonNavigator.Text = "kryptonNavigator1";
            // 
            // XstMessageViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainKryptonNavigator);
            this.Name = "XstMessageViewControl";
            this.Size = new System.Drawing.Size(683, 554);
            ((System.ComponentModel.ISupportInitialize)(this.MainKryptonNavigator)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Krypton.Navigator.KryptonNavigator MainKryptonNavigator;
    }
}
