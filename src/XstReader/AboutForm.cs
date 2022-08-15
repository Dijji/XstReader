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
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            AboutDescWebView.DocumentText = GetAboutText();
        }

        private string GetAboutText()
        {
            var sb = new StringBuilder();
            sb.Append("<center>");
            sb.Append($"<h2>{Application.ProductName} v.{Application.ProductVersion}</h2>");
            sb.Append("<p>Copyright (c) 2021, iluvadev, and released under Ms-PL License.</p>");
            sb.Append("<p>XstReader is an open source viewer for Microsoft Outlook’s .ost and .pst files (also those protected by unknown password), written entirely in C#, with no dependency on any Microsoft Office components.</p>");
            sb.Append("</center>");

            sb.Append("<p>");
            sb.Append("<b>Project site</b>: <a href='https://github.com/iluvadev/XstReader'>https://github.com/iluvadev/XstReader</a><br/>");
            sb.Append("<b>Issues</b>: <a href='https://github.com/iluvadev/XstReader/issues'>https://github.com/iluvadev/XstReader/issues</a><br/>");
            sb.Append("<b>License</b>: <a href='https://github.com/iluvadev/XstReader/blob/master/license.md'>Ms-PL</a><br/>");
            sb.Append("</p>");
            sb.Append("<p>");
            sb.Append("Based on the great work of <a href='https://github.com/Dijji'>Dijji</a>.<br/>");
            sb.Append("<b>Original project</b>: <a href='https://github.com/dijji/XstReader'>https://github.com/dijji/XstReader</a>");
            sb.Append("</p>");
                        
            return sb.ToString();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}
