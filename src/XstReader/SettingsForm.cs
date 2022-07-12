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
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            PropertyGridSettings.SelectedObject = XstReaderEnvironment.Options.ExportOptions;
        }
    }
}
