using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            using(var frm = new WaitingForm())
            {
                frm.Start(description, action);
                frm.ShowDialog();
            }
        }
    }
}
