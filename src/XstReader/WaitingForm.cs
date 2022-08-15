// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

using System.ComponentModel;

namespace XstReader.App
{
    public partial class WaitingForm : Form
    {

        public WaitingForm()
        {
            InitializeComponent();
            Initialize();
        }

        private Action Action { get; set; }

        private void Initialize()
        {
            Cursor.Current = Cursors.WaitCursor;
            BackgroundWorker.RunWorkerCompleted += (s, e) => { try { Close(); } catch { } };
            BackgroundWorker.DoWork += (s, e) => Action?.Invoke();
        }

        private void Start(string description, Action action)
        {
            TitleLabel.Text = description;
            Action = action;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            BackgroundWorker.RunWorkerAsync();
        }

        public static void Execute(string description, Action action)
        {
            using (var frm = new WaitingForm())
            {
                frm.Start(description, action);
                frm.ShowDialog();
            }
        }
    }
}
