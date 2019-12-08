// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using SearchTextBox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
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
        private List<string> tempFileNames = new List<string>();
        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;
        private int searchIndex = -1;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = view;

            // For testing purposes, use these flags to control the display of print headers
            //view.DisplayPrintHeaders = true;
            //view.DisplayEmailType = true;

            // Supply the Search control with the list of sections
            searchTextBox.SectionsList = new List<string> { "Subject", "From/To", "Date", "Cc", "Bcc" };
            searchTextBox.SectionsInitiallySelected = new List<bool> { true, true, true, false, false };

            if (Properties.Settings.Default.Top != 0.0)
            {
                this.Top = Properties.Settings.Default.Top;
                this.Left = Properties.Settings.Default.Left;
                this.Height = Properties.Settings.Default.Height;
                this.Width = Properties.Settings.Default.Width;
            }
        }

        public void OpenFile(string fileName)
        {
            if (!System.IO.File.Exists(fileName))
                return;

            view.Clear();
            ShowStatus("Loading...");
            Mouse.OverrideCursor = Cursors.Wait;

            // Load on a background thread so we can keep the UI in sync
            Task.Factory.StartNew(() =>
            {
                try
                {
                    xstFile = new XstFile(view, fileName);
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
                    ShowStatus(null);
                    Mouse.OverrideCursor = null;
                    Title = "Xst Reader - " + System.IO.Path.GetFileName(fileName);
                }));
            });
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            // Ask for a .ost or .pst file to open
            string fileName = GetXstFileName();

            if (fileName != null)
                OpenFile(fileName);
        }

        private void exportAllProperties_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExportEmailProperties(view.SelectedFolder.Messages);
        }

        private void exportAllEmails_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExportEmails(view.SelectedFolder.Messages);
        }

        private void treeFolders_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Folder f = (Folder)e.NewValue;
            view.SelectedFolder = f;

            if (f != null)
            {
                view.SetMessage(null);
                ShowMessage(null);
                f.Messages.Clear();
                ShowStatus("Reading messages...");
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
                        ShowStatus(null);
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
            searchIndex = listMessages.SelectedIndex;
            searchTextBox.ShowSearch = true;
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

            searchIndex = listMessages.SelectedIndex;
            listMessages.ScrollIntoView(listMessages.SelectedItem);
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

        private void exportEmail_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (listMessages.SelectedItems.Count > 1)
            {
                ExportEmails(listMessages.SelectedItems.Cast<Message>());
            }
            else
            {
                string fullFileName = GetEmailExportFileName(view.CurrentMessage.ExportFileName,
                                            view.CurrentMessage.ExportFileExtension);

                if (fullFileName != null)
                {
                    try
                    {
                        view.CurrentMessage.ExportToFile(fullFileName, xstFile);
                        SaveVisibleAttachmentsToAssociatedFolder(fullFileName, view.CurrentMessage);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error exporting email");
                    }
                }
            }
        }

        private void ExportEmails(IEnumerable<Message>messages)
        {
            string folderName = GetEmailsExportFolderName();

            if (folderName != null)
            {
                ShowStatus("Exporting emails...");
                Mouse.OverrideCursor = Cursors.Wait;

                // Export emails on a background thread so we can keep the UI in sync
                Task.Factory.StartNew<Tuple<int, int>>(() =>
                {
                    Message current = null;
                    int good = 0, bad = 0;
                    // If files already exist, we overwrite them.
                    // But if emails within this batch generate the same filename,
                    // use a numeric suffix to distinguish them
                    HashSet<string> usedNames = new HashSet<string>();
                    foreach (Message m in messages)
                    {
                        try
                        {
                            current = m;
                            string fileName = m.ExportFileName;
                            for (int i = 1; ; i++)
                            {
                                if (!usedNames.Contains(fileName))
                                {
                                    usedNames.Add(fileName);
                                    break;
                                }
                                else
                                    fileName = String.Format("{0} ({1})", m.ExportFileName, i);
                            }
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                ShowStatus("Exporting " + m.ExportFileName);
                            }));
                            // Ensure that we have the message contents
                            xstFile.ReadMessageDetails(m);
                            var fullFileName = String.Format(@"{0}\{1}.{2}",
                                        folderName, fileName, m.ExportFileExtension);
                            m.ExportToFile(fullFileName, xstFile);
                            SaveVisibleAttachmentsToAssociatedFolder(fullFileName, m);
                            good++;
                        }
                        catch (System.Exception ex)
                        {
                            var result = MessageBox.Show(String.Format("Error '{0}' exporting email '{1}'",
                                ex.Message, current.Subject), "Error exporting emails",
                                MessageBoxButton.OKCancel);
                            bad++;
                            if (result == MessageBoxResult.Cancel)
                                break;
                        }
                    }
                    return new Tuple<int, int>(good, bad);
                })
                // When exporting completes, update the UI using the UI thread 
                .ContinueWith((task) =>
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        ShowStatus(null);
                        Mouse.OverrideCursor = null;
                        txtStatus.Text = String.Format("Completed with {0} successes and {1} failures",
                            task.Result.Item1, task.Result.Item2);
                    }));
                });
            }
        }

        private void ExportEmailProperties(IEnumerable<Message> messages)
        {
            string fileName = GetPropertiesExportFileName(view.SelectedFolder.Name);

            if (fileName != null)
            {
                ShowStatus("Exporting properties...");
                Mouse.OverrideCursor = Cursors.Wait;

                // Export properties on a background thread so we can keep the UI in sync
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        xstFile.ExportMessageProperties(messages, fileName);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error exporting properties");
                    }
                })
                // When exporting completes, update the UI using the UI thread 
                .ContinueWith((task) =>
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        ShowStatus(null);
                        Mouse.OverrideCursor = null;
                    }));
                });
            }
        }

        private void SaveAttachments(IEnumerable<Attachment> attachments)
        {
            string folderName = GetAttachmentsSaveFolderName();

            if (folderName != null)
            {
                try
                {
                    SaveAttachmentsToFolder(folderName, view.CurrentMessage.Date, attachments);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(String.Format("Error '{0}' saving attachments to '{1}'",
                        ex.Message, view.CurrentMessage.Subject), "Error saving attachments");
                }
            }
        }

        private void SaveVisibleAttachmentsToAssociatedFolder(string fullFileName, Message m)
        {
            if (m.HasVisibleFileAttachment)
            {
                var targetFolder = Path.Combine(Path.GetDirectoryName(fullFileName),
                    Path.GetFileNameWithoutExtension (fullFileName) + " Attachments");
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                    if (m.Date != null)
                        Directory.SetCreationTime(targetFolder, (DateTime)m.Date);
                }
                SaveAttachmentsToFolder(targetFolder, m.Date,  m.Attachments.Where(a => a.IsFile && !a.Hide));
            }
        }

        private void SaveAttachmentsToFolder(string fullFolderName, DateTime? creationTime, IEnumerable<Attachment> attachments)
        {
            foreach (var a in attachments)
            {
                xstFile.SaveAttachmentToFolder(fullFolderName, creationTime, a);
            }
        }

        private void exportEmailProperties_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (listMessages.SelectedItems.Count > 1)
            {
                ExportEmailProperties(listMessages.SelectedItems.Cast<Message>());
            }
            else
            {
                string fileName = GetPropertiesExportFileName(view.CurrentMessage.ExportFileName);

                if (fileName != null)
                    xstFile.ExportMessageProperties(new Message[1] { view.CurrentMessage }, fileName);
            }
        }

        private void btnSaveAllAttachments_Click(object sender, RoutedEventArgs e)
        {
            SaveAttachments(view.CurrentMessage.Attachments);
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

        private void searchTextBox_OnSearch(object sender, RoutedEventArgs e)
        {
            try
            {
                var args = e as SearchEventArgs;
                bool subject = args.Sections.Contains("Subject");
                bool fromTo = args.Sections.Contains("From/To");
                bool date = args.Sections.Contains("Date");
                bool cc = args.Sections.Contains("Cc");
                bool bcc = args.Sections.Contains("Bcc");
                bool found = false;
                switch (args.SearchEventType)
                {
                    case SearchEventType.Search:
                        for (int i = 0; i < listMessages.Items.Count; i++)
                        {
                            found = PropertyHitTest(i, args.Keyword, subject, fromTo, date, cc, bcc);
                            if (found)
                                break;
                        }

                        if (!found)
                            searchIndex = -1;
                        break;
                    case SearchEventType.Next:
                        for (int i = searchIndex + 1; i < listMessages.Items.Count; i++)
                        {
                            found = PropertyHitTest(i, args.Keyword, subject, fromTo, date, cc, bcc);
                            if (found)
                            {
                                searchTextBox.ShowSearch = true;
                                break;
                            }
                        }
                        break;
                    case SearchEventType.Previous:
                        for (int i = searchIndex - 1; i >= 0; i--)
                        {
                            found = PropertyHitTest(i, args.Keyword, subject, fromTo, date, cc, bcc);
                            if (found)
                            {
                                searchTextBox.ShowSearch = true;
                                break;
                            }
                        }
                        break;
                }

                if (!found)
                    searchTextBox.IndicateSearchFailed(args.SearchEventType);
            }
            catch (Exception ex)
            {
                // Unclear what we can do here, as we were invoked by an event from the search text box control
            }
        }

        private bool PropertyHitTest(int index, string text, bool subject, bool fromTo, bool date, bool cc, bool bcc)
        {
            Message m = listMessages.Items[index] as Message;
            if ((subject && m.Subject != null && m.Subject.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) >= 0) ||
                (fromTo && m.FromTo != null && m.FromTo.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) >= 0) ||
                (date && m.DisplayDate.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) >= 0) ||
                (cc && m.CcDisplayList.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) >= 0) ||
                (bcc && m.BccDisplayList.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) >= 0))
            {
                searchIndex = index;
                listMessages.UnselectAll();
                m.IsSelected = true;
                listMessages.ScrollIntoView(m);
                return true;
            }
            else
                return false;
        }

        private void ShowStatus(string status)
        {
            if (status != null)
            {
                view.IsBusy = true;
                txtStatus.Text = status;
            }
            else
            {
                view.IsBusy = false;
                txtStatus.Text = "";
            }
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
                        string body = m.GetBodyAsHtmlString();
                        if (m.MayHaveInlineAttachment)
                            body = m.EmbedAttachments(body, xstFile);  // Returns null if this is not appropriate

                        if (body != null)
                        {
                            // For testing purposes, can show print header in main visualisation
                            if (view.DisplayPrintHeaders)
                                body = m.EmbedHtmlPrintHeader(body, view.DisplayEmailType);

                            wbMessage.NavigateToString(body);
                            if (m.MayHaveInlineAttachment)
                            {
                                m.SortAndSaveAttachments();  // Re-sort attachments in case any new in-line rendering discovered
                            }
                        }
                    }
                    // Can't bind RTF content, so push it into the control, if the message is RTF
                    else if (m.ShowRtf)
                    {
                        var body  = m.GetBodyAsFlowDocument();

                        // For testing purposes, can show print header in main visualisation
                        if (view.DisplayPrintHeaders)
                            m.EmbedRtfPrintHeader(body, view.DisplayEmailType);

                        rtfMessage.Document = body;
                    }
                    // Could bind text content, but use push so that we can optionally add headers
                    else if (m.ShowText)
                    {
                        var body = m.Body;

                        // For testing purposes, can show print header in main visualisation
                        if (view.DisplayPrintHeaders)
                            body = m.EmbedTextPrintHeader(body, true, view.DisplayEmailType);

                        txtMessage.Text = body;
                        scrollTextMessage.ScrollToHome();
                    }
                }
                else
                {
                    // Clear the displays, in case we were showing that type before
                    wbMessage.Navigate("about:blank");
                    rtfMessage.Document.Blocks.Clear();
                    txtMessage.Text = "";
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error reading message body");
            }
        }

        private void OpenEmailAttachment (Attachment a)
        {
            Message m = xstFile.OpenAttachedMessage(a);
            ShowMessage(m);
            view.PushMessage(m);
        }

        #region File and folder dialogs

        private string GetXstFileName()
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
                Properties.Settings.Default.LastFolder = Path.GetDirectoryName(dialog.FileName);
                Properties.Settings.Default.Save();
                return dialog.FileName;
            }
            else
                return null;
        }

        private string GetAttachmentsSaveFolderName()
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
                return dialog.SelectedPath;
            }
            else
                return null;
        }
       
        private string GetSaveAttachmentFileName(string defaultFileName)
        {
            var dialog = new System.Windows.Forms.SaveFileDialog();

            dialog.Title = "Specify file to save to";
            dialog.InitialDirectory = Properties.Settings.Default.LastAttachmentFolder;
            if (dialog.InitialDirectory == "")
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Filter = "All Files (*.*)|*.*";
            dialog.FileName = defaultFileName;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.LastAttachmentFolder = Path.GetDirectoryName(dialog.FileName);
                Properties.Settings.Default.Save();
                return dialog.FileName;
            }
            else
                return null;
        }

        private string GetEmailsExportFolderName()
        {
            // Find out where to export the emails
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();

            dialog.Description = "Choose folder to export emails into";
            dialog.RootFolder = Environment.SpecialFolder.MyComputer;
            dialog.SelectedPath = Properties.Settings.Default.LastExportFolder;
            if (dialog.SelectedPath == "")
                dialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.ShowNewFolderButton = true;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.LastExportFolder = dialog.SelectedPath;
                Properties.Settings.Default.Save();
                return dialog.SelectedPath;
            }
            else
                return null;
        }

        private string GetEmailExportFileName(string defaultFileName, string extension)
        {
            var dialog = new System.Windows.Forms.SaveFileDialog();

            dialog.Title = "Specify file to save to";
            dialog.InitialDirectory = Properties.Settings.Default.LastExportFolder;
            if (dialog.InitialDirectory == "")
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Filter = String.Format("{0} Files (*.{0})|*.{0}|All Files (*.*)|*.*", extension);
            dialog.FileName = defaultFileName;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.LastExportFolder = Path.GetDirectoryName(dialog.FileName);
                Properties.Settings.Default.Save();
                return dialog.FileName;
            }
            else
                return null;
        }

        private string GetPropertiesExportFileName(string defaultName)
        {
            var dialog = new System.Windows.Forms.SaveFileDialog();

            dialog.Title = "Specify properties export file";
            dialog.InitialDirectory = Properties.Settings.Default.LastExportFolder;
            if (dialog.InitialDirectory == "")
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Filter = "csv files (*.csv)|*.csv";
            dialog.FileName = defaultName;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.LastExportFolder = Path.GetDirectoryName(dialog.FileName);
                Properties.Settings.Default.Save();
                return dialog.FileName;
            }
            else
                return null;
        }
        #endregion

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // Clean up temporary files
            foreach (var fileFullName in tempFileNames)
            {
                // Wrap in try in case the file is still open
                try
                {
                    File.Delete(fileFullName);
                }
                catch { }
            }
            
            if (WindowState == WindowState.Maximized)
            {
                // Use the RestoreBounds as the current values will be 0, 0 and the size of the screen
                Properties.Settings.Default.Top = RestoreBounds.Top;
                Properties.Settings.Default.Left = RestoreBounds.Left;
                Properties.Settings.Default.Height = RestoreBounds.Height;
                Properties.Settings.Default.Width = RestoreBounds.Width;
            }
            else
            {
                Properties.Settings.Default.Top = this.Top;
                Properties.Settings.Default.Left = this.Left;
                Properties.Settings.Default.Height = this.Height;
                Properties.Settings.Default.Width = this.Width;
            }
            Properties.Settings.Default.Save();
        }

        private string SaveAttachmentToTemporaryFile(Attachment a)
        {
            if (a == null)
                return null;

            string fileFullName = Path.ChangeExtension(
                Path.GetTempPath() + Guid.NewGuid().ToString(), Path.GetExtension(a.FileName)); ;

            try
            {
                xstFile.SaveAttachment(fileFullName, null, listAttachments.SelectedItem as Attachment);
                tempFileNames.Add(fileFullName);
                return fileFullName;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error saving attachment");
                return null;
            }
        }

        private void attachmentEmailCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var a = listAttachments.SelectedItem as Attachment;
            e.CanExecute = a != null && a.IsEmail;
        }

        //private void openEmail_Executed(object sender, ExecutedRoutedEventArgs e)
        //{
        //    var a = listAttachments.SelectedItem as Attachment;
        //    OpenEmailAttachment(a);
        //}

        private void attachmentCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var a = listAttachments.SelectedItem as Attachment;
            e.CanExecute = a != null;
        }

        private void attachmentFileCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var a = listAttachments.SelectedItem as Attachment;
            e.CanExecute = a != null && a.IsFile;
        }

        private void openAttachment_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var a = listAttachments.SelectedItem as Attachment;

            if (a.IsFile)
            {
                string fileFullname = SaveAttachmentToTemporaryFile(a);
                if (fileFullname == null)
                    return;

                using (Process.Start(fileFullname)) { }
            }
            else if (a.IsEmail)
                OpenEmailAttachment(a);

            e.Handled = true;
        }

        private void openAttachmentWith_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var a = listAttachments.SelectedItem as Attachment;
            string fileFullname = SaveAttachmentToTemporaryFile(a);
            if (fileFullname == null)
                return;
         
            if (Environment.OSVersion.Version.Major > 5)
            {
                IntPtr hwndParent = Process.GetCurrentProcess().MainWindowHandle;
                tagOPENASINFO oOAI = new tagOPENASINFO();
                oOAI.cszFile = fileFullname;
                oOAI.cszClass = String.Empty;
                oOAI.oaifInFlags = tagOPEN_AS_INFO_FLAGS.OAIF_ALLOW_REGISTRATION | tagOPEN_AS_INFO_FLAGS.OAIF_EXEC;
                SHOpenWithDialog(hwndParent, ref oOAI);
            }
            else
            {
                using (Process.Start("rundll32", "shell32.dll,OpenAs_RunDLL " + fileFullname)) { }
            }
            e.Handled = true;
        }

        private void saveAttachmentAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (listAttachments.SelectedItems.Count > 1)
            {
                SaveAttachments(listAttachments.SelectedItems.Cast<Attachment>());
            }
            else
            {
                var a = listAttachments.SelectedItem as Attachment;
                var fullFileName = GetSaveAttachmentFileName(a.LongFileName);

                if (fullFileName != null)
                {
                    try
                    {
                        xstFile.SaveAttachment(fullFileName, view.CurrentMessage.Date, a);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error saving attachment");
                    }
                }
            }
            e.Handled = true;
        }

        // Plumbing to enable access to SHOpenWithDialog
        [DllImport("shell32.dll", EntryPoint = "SHOpenWithDialog", CharSet = CharSet.Unicode)]
        private static extern int SHOpenWithDialog(IntPtr hWndParent, ref tagOPENASINFO oOAI);
        private struct tagOPENASINFO
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string cszFile;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string cszClass;

            [MarshalAs(UnmanagedType.I4)]
            public tagOPEN_AS_INFO_FLAGS oaifInFlags;
        }
        [Flags]
        private enum tagOPEN_AS_INFO_FLAGS
        {
            OAIF_ALLOW_REGISTRATION = 0x00000001,   // Show "Always" checkbox
            OAIF_REGISTER_EXT = 0x00000002,   // Perform registration when user hits OK
            OAIF_EXEC = 0x00000004,   // Exec file after registering
            OAIF_FORCE_REGISTRATION = 0x00000008,   // Force the checkbox to be registration
            OAIF_HIDE_REGISTRATION = 0x00000020,   // Vista+: Hide the "always use this file" checkbox
            OAIF_URL_PROTOCOL = 0x00000040,   // Vista+: cszFile is actually a URI scheme; show handlers for that scheme
            OAIF_FILE_IS_URI = 0x00000080    // Win8+: The location pointed to by the pcszFile parameter is given as a URI
        }
    }
}
