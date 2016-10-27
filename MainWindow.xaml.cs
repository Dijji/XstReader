// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace XstReader
{
    /// <summary>
    /// XstReader is a viewer for xst (.ost and .pst) files
    /// This file contains the interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private View view = new View();
        private XstFile xstFile = null;
        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = view;
        }
        
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            // Ask for a .ost or .pst file to open
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();

            dialog.Filter = "xst files (*.ost;*.pst)|*.ost;*.pst|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.InitialDirectory = Properties.Settings.Default.LastFolder;
            if (dialog.InitialDirectory == "")
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.LastFolder = System.IO.Path.GetDirectoryName(dialog.FileName);
                Properties.Settings.Default.Save();

                view.Clear();
                txtStatus.Text = "Loading...";
                Mouse.OverrideCursor = Cursors.Wait;

                // Load on a background thread so we can keep the UI in sync
                Task.Factory.StartNew(() => 
                {
                    try
                    {
                        xstFile = new XstFile(view, dialog.FileName);
                        xstFile.ReadFolderTree();
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error reading xst file");
                    }
                })
                // When loading completes, update the UI using the UI thread 
                .ContinueWith((task) =>
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        txtStatus.Text = "";
                        Mouse.OverrideCursor = null;
                        Title = "Xst Reader - " + System.IO.Path.GetFileName(dialog.FileName);
                    }));
                });
            }
        }

        private void treeFolders_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Folder f = (Folder)e.NewValue;
            view.SelectedFolder = f;

            if (f != null)
            {
                f.Messages.Clear();
                txtStatus.Text = "Reading messages...";
                Mouse.OverrideCursor = Cursors.Wait;

                // Read messages on a background thread so we can keep the UI in sync
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        xstFile.ReadMessages(f);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error reading messages");
                    }
                })
                // When loading completes, update the UI using the UI thread 
                .ContinueWith((task) =>
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        txtStatus.Text = "";
                        Mouse.OverrideCursor = null;
                    }));
                });

                // If there is no sort in effect, sort by date in descending order
                if (listViewSortCol == null)
                {
                    string tag = "Date";
                    listViewSortCol = ((GridView)listMessages.View).Columns.Select(c => (GridViewColumnHeader)c.Header).Where(h => h.Tag.ToString() == tag).First();
                    listViewSortAdorner = new SortAdorner(listViewSortCol, ListSortDirection.Descending);
                    AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
                    listMessages.Items.SortDescriptions.Add(new SortDescription(tag, ListSortDirection.Descending));
                }
            }
        }

        private void listMessages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Message m = (Message)listMessages.SelectedItem;

            if (m != null)
            {
                try
                {
                    xstFile.ReadMessageDetails(m);
                    ShowMessage(m);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error reading message details");
                }
            }
            view.SetMessage(m);
        }

        private void listMessagesColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            // Sort the messages by the clicked on column
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                listMessages.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            listMessages.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void listRecipients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                view.SelectedRecipientChanged((Recipient)listRecipients.SelectedItem);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error reading recipient");
            }
        }

        private void listAttachments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                view.SelectedAttachmentsChanged(listAttachments.SelectedItems.Cast<Attachment>());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error reading attachment");
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveAttachments(listAttachments.SelectedItems.Cast<Attachment>().Where(a => a.IsFile));
        }

        private void btnSaveAll_Click(object sender, RoutedEventArgs e)
        {
            SaveAttachments(listAttachments.Items.Cast<Attachment>().Where(a => a.IsFile));
        }

        private void btnOpenEmail_Click(object sender, RoutedEventArgs e)
        {
            Message m = xstFile.OpenAttachedMessage(listAttachments.Items.Cast<Attachment>().First(a => a.IsEmail));
            ShowMessage(m);
            view.PushMessage(m);
        }

        private void btnCloseEmail_Click(object sender, RoutedEventArgs e)
        {
            view.PopMessage();
            ShowMessage(view.CurrentMessage);
        }

        private void rbContent_Click(object sender, RoutedEventArgs e)
        {
            view.ShowContent = true;
        }

        private void rbProperties_Click(object sender, RoutedEventArgs e)
        {
            view.ShowContent = false;
        }

        private void ShowMessage(Message m)
        {
            try
            {
                if (m != null)
                {
                    // Can't bind HTML content, so push it into the control, if the message is HTML
                    if (m.ShowHtml)
                    {
                        if (m.BodyHtml != null)
                            wbMessage.NavigateToString(m.BodyHtml);
                        else if(m.Html != null)
                        {
                            var ms = new System.IO.MemoryStream(m.Html);
                            wbMessage.NavigateToStream(ms);
                        }
                        else if(m.Body != null)
                            wbMessage.NavigateToString(m.Body);
                    }
                    // Can't bind RTF content, so push it into the control, if the message is RTF
                    else if (m.ShowRtf)
                    {
                        var decomp = new RtfDecompressor();

                        using (System.IO.MemoryStream ms = decomp.Decompress(m.RtfCompressed, true))
                        {
                            ms.Position = 0;
                            rtfMessage.SelectAll();
                            rtfMessage.Selection.Load(ms, DataFormats.Rtf);
                        }
                    }
                }
                else
                {
                    // Clear the HTML, in case we were showing that before
                    wbMessage.Navigate("about:blank");

                    // Clear the RTF, in case we were showing that before
                    rtfMessage.Document.Blocks.Clear();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error reading message body");
            }
        }

        private void SaveAttachments(IEnumerable<Attachment> attachments)
        {
            // Find out where to save the attachments
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();

            dialog.Description = "Choose folder for saving attachments";
            dialog.RootFolder = Environment.SpecialFolder.MyComputer; 
            dialog.SelectedPath = Properties.Settings.Default.LastAttachmentFolder;
            if (dialog.SelectedPath == "")
                dialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.ShowNewFolderButton = true;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.LastAttachmentFolder = dialog.SelectedPath;
                Properties.Settings.Default.Save();
                try
                {
                    foreach (var a in attachments)
                    {
                        xstFile.SaveAttachment(dialog.SelectedPath, a);
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error saving attachments");
                }
            }
        }
    }
}
