using System;
using System.Collections.Generic;
using System.IO;
using NDesk.Options;
using XstReader;

namespace XstExport
{
    class Program
    {

        static string[] helpText = new string[] {
            "",
            "Usage:",
            "",
            "   XstExport.exe {-n|-p|-a|-h} [-f=<Outlook folder>] [-o] [-s]",
            "                 [-t=<target directory>] <Outlook file name>",
            "",
            "Where:",
            "",
            "   -n, --native",
            "      Export in native body format (.html, .rtf, .txt)",
            "      with attachments in associated folder",
            "   -- OR --",
            "   -p, --properties",
            "      Export properties only (in CSV file)",
            "   -- OR --",
            "   -a, --attachments",
            "      Export attachments only",
            "      (Latest date wins in case of name conflict)",
            "   -- OR --",
            "   -h, --help",
            "      Display this help",
            "",
            "   -f=<Outlook folder>, -folder=<Outlook folder>",
            "      Folder within the Outlook file from which to export.",
            "      This may be a partial path, for example \"Week1\\Sent\"",
            "",
            "   -o, --only",
            "      If set, do not export from subfolders of the nominated folder.",
            "",
            "   -s, --subfolders",
            "      If set, Outlook subfolder structure is preserved.",
            "      Otherwise, all output goes to a single directory",
            "",
            "   -t=<target directory name>, --target=<target directory name>",
            "      The directory to which output is written. This may be an",
            "      absolute path or one relative to the location of the Outlook file.",
            "      By default, output is written to a directory <Outlook file name>.Export.<Command>",
            "      created in the same directory as the Outlook file",
            "",
            "   <Outlook file name>",
            "      The full name of the .pst or .ost file from which to export",
            "",
        };

        enum Command
        {
            Help,
            Native,
            Properties,
            Attachments,
        }

        static int Main(string[] args)
        {
            int commands = 0;
            Command command = Command.Help;
            string outlookFolder = null;
            bool only = false;
            bool subfolders = false;
            string exportDir = null;

            try
            {
                var argParser = new OptionSet() {
                    { "n|native", v => {command = Command.Native; commands++;} },
                    { "p|properties",  v => {command = Command.Properties; commands++;} },
                    { "a|attachments", v => {command = Command.Attachments; commands++;} },
                    { "h|?|help", v => {command = Command.Help; commands++;} },
                    { "f|folder=", v => outlookFolder = v },
                    { "o|only", v => only = true },
                    { "s|subfolders", v => subfolders = true },
                    { "t|target=", v => exportDir = v },
                };
                List<string> outlookFiles = argParser.Parse(args);

                if (commands != 1)
                {
                    throw new XstExportException
                    {
                        Description = "You must specify exactly one of --native, --properties, --attachments or --help.",
                        ErrorCode = WindowsErrorCodes.ERROR_INVALID_PARAMETER
                    };
                }

                if (command == Command.Help)
                {
                    foreach (var line in helpText)
                        Console.WriteLine(line);
                    return 0;
                }

                if (outlookFiles.Count != 1)
                {
                    throw new XstExportException
                    {
                        Description = "You must specify exactly one Outlook file to export from.",
                        ErrorCode = WindowsErrorCodes.ERROR_INVALID_PARAMETER
                    };
                }

                string outlookFile = outlookFiles[0];

                if (!File.Exists(outlookFile))
                {
                    throw new XstExportException
                    {
                        Description = @"Cannot find Outlook file '{outlookFile}'",
                        ErrorCode = WindowsErrorCodes.ERROR_FILE_NOT_FOUND
                    };
                }

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
                    {
                        throw new XstExportException
                        {
                            Description = @"Cannot find folder '{outlookFolder}' in '{outlookFile}'",
                            ErrorCode = WindowsErrorCodes.ERROR_INVALID_PARAMETER
                        };
                    }
                }

                // The arguments look good, so prepare to actually export
                if (exportDir == null)
                    exportDir = CreateDirectoryIfNeeded(Path.GetDirectoryName(outlookFile),
                                Path.GetFileNameWithoutExtension(outlookFile) + ".Export." +
                                Enum.GetName(typeof(Command), command));

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
                        sources.AddRange(root.Folders.Flatten(f => f.Folders));
                }

                foreach (var f in sources)
                {
                    string targetDir;
                    if (subfolders)
                        targetDir = Path.Combine(exportDir, ValidFolderPath(f));
                    else
                        targetDir = exportDir;

                    ExportFolder(xstFile, f, command, targetDir);
                }
            }
            catch (XstExportException xe)
            {
                Console.Error.WriteLine(xe.DisplayString);
                return (int)xe.ErrorCode;
            }
            catch (XstException xe)
            {
                Console.Error.WriteLine(xe.Message);
                return -1;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected Exception:");
                Console.Error.WriteLine(ex.Message);
                return -1;
            }

            Console.WriteLine("Done!");

            return 0;
        }

        private static void ExportFolder(XstFile xstFile, Folder folder, Command command, string exportDir)
        {
            if (folder.ContentCount == 0)
            {
                Console.WriteLine($"Skipping folder '{folder.Name}', which is empty");
                return;
            }

            if (!Directory.Exists(exportDir))
                Directory.CreateDirectory(exportDir);

            switch (command)
            {
                case Command.Native:
                    ExtractEmailsInFolder(xstFile, folder, exportDir);
                    break;
                case Command.Properties:
                    ExtractPropertiesInFolder(xstFile, folder, exportDir);
                    break;
                case Command.Attachments:
                    ExtractAttachmentsInFolder(xstFile, folder, exportDir);
                    break;
                case Command.Help:
                default:
                    break;
            }
        }

        private static Folder FindOutlookFolder(Folder root, string outlookFolder)
        {
            string[] folders = outlookFolder.Split(new char[] { '\\', '/' }); // Accept backward or forward slash

            // We do a breadth first search of the folder tree
            Queue<Folder> q = new Queue<Folder>();
            q.Enqueue(root);

            while (q.Count > 0)
            {
                var f = q.Dequeue();
                if (FolderMatches(f, folders))
                    return f;

                foreach (var child in f.Folders)
                    q.Enqueue(child);
            }

            return null;
        }

        private static bool FolderMatches(Folder folder, string[] folderNames)
        {
            // First name segment must match
            if (String.Compare(folder.Name, folderNames[0], true) != 0)
                return false;

            // And subsequent segments must be matched by some child
            for (int i = 1; i < folderNames.Length; i++)
            {
                // This exploits the fact that there can be only one child of a given folder with a given name
                folder = folder.Folders.Find(f => String.Compare(f.Name, folderNames[i], true) != 0);
                if (folder == null)
                    return false;
            }

            // All segments of the name have been matched
            return true;
        }

        private static string CreateDirectoryIfNeeded(string rootDirName, string dirName)
        {
            string exportDirectory = Path.Combine(rootDirName,
                RemoveInvalidChars(Path.GetFileName(dirName)));
            if (!Directory.Exists(exportDirectory))
                Directory.CreateDirectory(exportDirectory);
            return exportDirectory;
        }

        private static string ValidFolderPath(Folder f)
        {
            if (string.IsNullOrEmpty(f.ParentFolder?.Name))
                return RemoveInvalidChars(f.Name);
            else
                return $"{ValidFolderPath(f.ParentFolder)}\\{RemoveInvalidChars(f.Name)}";
        }

        private static string RemoveInvalidChars(string filename)
        {
            return filename.ReplaceInvalidFileNameChars("");
        }

        private static void ExtractEmailsInFolder(XstFile xstFile, XstReader.Folder folder, string exportDirectory)
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

                    Console.WriteLine("Exporting " + m.ExportFileName);

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
                    Console.WriteLine(String.Format("Error '{0}' exporting email '{1}'",
                        ex.Message, current.Subject));
                    bad++;

                }
            }
            Console.WriteLine($"Folder '{folder.Name}' completed with {good} successes and {bad} failures");
        }

        private static void ExtractPropertiesInFolder(XstFile xstFile, XstReader.Folder folder, string exportDirectory)
        {
            var fileName = Path.Combine(exportDirectory, RemoveInvalidChars(folder.Name)) + ".csv";
            Console.WriteLine("Exporting " + fileName);
            xstFile.ReadMessages(folder);
            xstFile.ExportMessageProperties(folder.Messages, fileName);
        }

        private static void ExtractAttachmentsInFolder(XstFile xstFile, XstReader.Folder folder, string exportDirectory)
        {
            xstFile.ReadMessages(folder);
            foreach (var message in folder.Messages)
            {
                xstFile.ReadMessageDetails(message);
                foreach(var att in message.Attachments)
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
                            else { 
                                actionName = "Skip";
                            }
                        }
                        Console.WriteLine(String.Format("{0} : {1}" , actionName, attachmentExpectedName));
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
            }
            Console.WriteLine($"Folder '{folder.Name}' completed");
        }
    }
}
