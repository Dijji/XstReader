using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using XstExport;
using XstReader;

namespace XstExporter
{
    enum Command
    {
        Help,
        Email,
        Properties,
        Attachments,
        Subfolders,
        Only
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Command command = Command.Help;
        bool only =  false;
        bool subfolders = false;
        bool emails = false;
        bool properties = false;
        bool attachments = false;

        string outlookFolder, exportDir, outlookFile = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CommonCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void tBarOpen_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    List<string> files = Directory.GetFiles(fbd.SelectedPath, "*.pst").ToList();

                    if (files != null && files.Count > 0)
                    {
                        var file = files.FirstOrDefault();
                        
                        outlookFile = file;
                        outlookFolder = Path.GetDirectoryName(file);
                        txtOutlookFile.Text = Path.GetDirectoryName(file);
                        txtOutputOutlook.Text = Path.GetFileNameWithoutExtension(Path.GetFileName(file));
                    }   
                }
            }
        }

        private void tBarHelp_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.Show();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnBrowseTargetDir_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    txtTargetDirectory.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            OutputBlock.Text = string.Empty;

            only = (bool)ckOnly.IsChecked ? true : false;
            subfolders = (bool)ckSubfolders.IsChecked ? true : false;
            emails = (bool)ckEmail.IsChecked ? true : false;
            properties = (bool)ckProperties.IsChecked ? true : false;
            attachments = (bool)ckAttachments.IsChecked ? true : false;
            
            exportDir = txtTargetDirectory.Text;

            try
            {
                if (!File.Exists(outlookFile))
                    MessageBox.Show("Cannot find Outlook file '{outlookFile}'", "Error file not found");

                if (exportDir != null)
                {
                    // Handle relative target directory
                    if (!Path.IsPathRooted(exportDir)) // IsPathFullyQualified would be better, but not in 4
                        exportDir = Path.Combine(Path.GetDirectoryName(outlookFile), exportDir);

                    if (!Directory.Exists(exportDir))
                        Directory.CreateDirectory(exportDir);
                }

                var xstFile = new XstFile(outlookFile);
                var root = xstFile.ReadFolderTree();

                Folder sourceFolder = null;
                if (outlookFolder != null)
                {
                    sourceFolder = FindOutlookFolder(root, outlookFolder);
                    if (sourceFolder == null)
                        OutputBlock.Text += "Cannot find folder '{outlookFolder}' in '{outlookFile}'";
                }

                // The arguments look good, so prepare to actually export
                if (exportDir == null)
                {
                    exportDir = CreateDirectoryIfNeeded(Path.GetDirectoryName(outlookFile),
                                Path.GetFileNameWithoutExtension(outlookFile) + ".Export." +
                                Enum.GetName(typeof(Command), command));
                }

                // Work out which folders to export
                List<Folder> sources = new List<Folder>();
                if (only)
                {
                    sources.Add(sourceFolder ?? root.Folders[0]);
                }
                else
                {
                    if (sourceFolder != null)
                    {
                        sources.Add(sourceFolder);
                        sources.AddRange(sourceFolder.Folders.Flatten(f => f.Folders));
                    }
                    else
                    {
                        sources.AddRange(root.Folders.Flatten(f => f.Folders));
                    }
                }

                foreach (var f in sources)
                {
                    string targetDir;
                    if (subfolders)
                    {
                        targetDir = Path.Combine(exportDir, ValidFolderPath(f));
                    }
                    else
                    {
                        targetDir = exportDir;
                    }

                    ExportFolder(xstFile, f, command, targetDir);
                }
            }
            catch (XstExportException xe)
            {
                OutputBlock.Text += xe.DisplayString + Environment.NewLine;                
            }
            catch (XstException xe)
            {
                OutputBlock.Text += xe.Message + Environment.NewLine;
            }
            catch (Exception ex)
            {
                OutputBlock.Text += "Unexpected Exception:" + Environment.NewLine;
                OutputBlock.Text += ex.Message + Environment.NewLine;
            }

            OutputBlock.Text += "Done!" + Environment.NewLine;
        }

        #region Methods

        private void ExportFolder(XstFile xstFile, Folder folder, Command command, string exportDir)
        {
            if (folder.ContentCount == 0)
            {
                OutputBlock.Text += $"Skipping folder '{folder.Name}', which is empty" + Environment.NewLine;
                return;
            }

            bool createdByUs = false;
            if (!Directory.Exists(exportDir))
            {
                Directory.CreateDirectory(exportDir);
                createdByUs = true;
            }

            if (emails)
            {
                ExtractEmailsInFolder(xstFile, folder, exportDir);
            }
            else if (properties)
            {
                ExtractPropertiesInFolder(xstFile, folder, exportDir);
            }
            else if(attachments)
            {
                ExtractAttachmentsInFolder(xstFile, folder, exportDir);
            }
            
            // If we create the directory, clean it up if nothing is put in it
            if (createdByUs)
            {
                var di = new DirectoryInfo(exportDir);
                if (!di.EnumerateFiles().Any() && !di.EnumerateDirectories().Any())
                    di.Delete();
            }
        }

        private Folder FindOutlookFolder(Folder root, string outlookFolder)
        {
            string[] folders = outlookFolder.Split(new char[] { '\\', '/' }); // Accept backward or forward slash

            // We do a breadth first search of the folder tree
            Queue<Folder> q = new Queue<Folder>();
            q.Enqueue(root);

            while (q.Count > 0)
            {
                var f = q.Dequeue();
                var match = FolderMatch(f, folders);
                if (match != null)
                    return match;

                foreach (var child in f.Folders)
                    q.Enqueue(child);
            }

            return null;
        }

        private Folder FolderMatch(Folder folder, string[] folderNames)
        {
            // First name segment must match
            if (String.Compare(folder.Name, folderNames[0], true) != 0)
                return null;

            // And subsequent segments must be matched by some child
            for (int i = 1; i < folderNames.Length; i++)
            {
                // This exploits the fact that there can be only one child of a given folder with a given name
                folder = folder.Folders.Find(f => String.Compare(f.Name, folderNames[i], true) == 0);
                if (folder == null)
                    return null;
            }

            // All segments of the name have been matched - return the innermost
            return folder;
        }

        private string CreateDirectoryIfNeeded(string rootDirName, string dirName)
        {
            string exportDirectory = Path.Combine(rootDirName,
                RemoveInvalidChars(Path.GetFileName(dirName)));
            if (!Directory.Exists(exportDirectory))
                Directory.CreateDirectory(exportDirectory);
            return exportDirectory;
        }

        private string ValidFolderPath(Folder f)
        {
            if (string.IsNullOrEmpty(f.ParentFolder?.Name))
                return RemoveInvalidChars(f.Name);
            else
                return $"{ValidFolderPath(f.ParentFolder)}\\{RemoveInvalidChars(f.Name)}";
        }

        private string RemoveInvalidChars(string filename)
        {
            return filename.ReplaceInvalidFileNameChars("");
        }

        private void ExtractEmailsInFolder(XstFile xstFile, XstReader.Folder folder, string exportDirectory)
        {
            Message current = null;
            int good = 0, bad = 0;
            // If files already exist, we overwrite them.
            // But if emails within this batch generate the same filename,
            // use a numeric suffix to distinguish them
            HashSet<string> usedNames = new HashSet<string>();

            xstFile.ReadMessages(folder);
            foreach (Message m in folder.Messages)
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

                    OutputBlock.Text += "Exporting " + m.ExportFileName + Environment.NewLine;                    

                    // Ensure that we have the message contents
                    xstFile.ReadMessageDetails(m);
                    var fullFileName = String.Format(@"{0}\{1}.{2}",
                                exportDirectory, fileName, m.ExportFileExtension);
                    m.ExportToFile(fullFileName, xstFile);
                    xstFile.SaveVisibleAttachmentsToAssociatedFolder(fullFileName, m);
                    good++;
                }
                catch (System.Exception ex)
                {
                    OutputBlock.Text += String.Format("Error '{0}' exporting email '{1}'", ex.Message, current.Subject) + Environment.NewLine;
                    bad++;
                }
            }
            OutputBlock.Text += $"Folder '{folder.Name}' completed with {good} successes and {bad} failures" + Environment.NewLine;
        }

        private void ckEmail_Checked(object sender, RoutedEventArgs e)
        {
            CheckboxExportOptions(Command.Email);
        }

        private void ckProperties_Checked(object sender, RoutedEventArgs e)
        {
            CheckboxExportOptions(Command.Properties);
        }

        private void ckAttachments_Checked(object sender, RoutedEventArgs e)
        {
            CheckboxExportOptions(Command.Attachments);
        }

        private void ckSubfolders_Checked(object sender, RoutedEventArgs e)
        {
            CheckboxStructureOptions(Command.Subfolders);
        }

        private void ckOnly_Checked(object sender, RoutedEventArgs e)
        {
            CheckboxStructureOptions(Command.Only);
        }

        private void CheckboxExportOptions(Command cmd)
        {
            switch (cmd)
            {
                case Command.Email:
                    ckEmail.IsChecked = true;
                    ckProperties.IsChecked = false;
                    ckAttachments.IsChecked = false;
                    break;
                case Command.Properties:
                    ckEmail.IsChecked = false;
                    ckProperties.IsChecked = true;
                    ckAttachments.IsChecked = false;
                    break;
                case Command.Attachments:
                    ckEmail.IsChecked = false;
                    ckProperties.IsChecked = false;
                    ckAttachments.IsChecked = true;
                    break;
            }

            emails = (bool)ckEmail.IsChecked;
            properties = (bool)ckProperties.IsChecked;
            attachments = (bool)ckAttachments.IsChecked;
        }

        private void CheckboxStructureOptions(Command cmd)
        {
            switch (cmd)
            {
                case Command.Subfolders:
                    ckSubfolders.IsChecked = true;
                    ckOnly.IsChecked = false;
                    break;
                case Command.Only:
                    ckSubfolders.IsChecked = false;
                    ckOnly.IsChecked = true;
                    break;
            }

            subfolders = (bool)ckSubfolders.IsChecked;
            only = (bool)ckOnly.IsChecked;
        }

        private void ExtractPropertiesInFolder(XstFile xstFile, XstReader.Folder folder, string exportDirectory)
        {
            var fileName = Path.Combine(exportDirectory, RemoveInvalidChars(folder.Name)) + ".csv";
            OutputBlock.Text += "Exporting " + fileName + Environment.NewLine;
            xstFile.ReadMessages(folder);
            xstFile.ExportMessageProperties(folder.Messages, fileName);
        }

        private void ExtractAttachmentsInFolder(XstFile xstFile, XstReader.Folder folder, string exportDirectory)
        {
            int good = 0, bad = 0;

            xstFile.ReadMessages(folder);
            foreach (var message in folder.Messages)
            {
                try
                {
                    xstFile.ReadMessageDetails(message);
                    foreach (var att in message.Attachments)
                    {
                        if (att.IsFile)
                        {
                            var attachmentExpectedName = Path.Combine(exportDirectory, att.FileName);
                            var fi = new FileInfo(attachmentExpectedName);
                            var actionName = string.Empty;

                            if (!fi.Exists)
                            {
                                actionName = "Create";
                            }
                            else
                            {
                                if (fi.CreationTime < message.Received)
                                {
                                    actionName = "CreateNewer";
                                }
                                else
                                {
                                    actionName = "Skip";
                                }
                            }
                            OutputBlock.Text += $"{actionName} : {attachmentExpectedName}" + Environment.NewLine;                            
                            switch (actionName)
                            {
                                case "Create":
                                case "CreateNewer":
                                    xstFile.SaveAttachment(attachmentExpectedName, message.Received, att);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    good++;
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(String.Format("Error '{0}' exporting email '{1}'", ex.Message, message.Subject));
                    bad++;

                }
            }
            
            OutputBlock.Text += $"Folder '{folder.Name}' completed with {good} successes and {bad} failures" + Environment.NewLine;
        }

        #endregion
    }
}
