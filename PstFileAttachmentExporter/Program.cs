using System;
using XstReader;

namespace PstFileExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Export all attachments from a given pst file");
                Console.WriteLine("- Give the path of a pst file as first argument");
                Console.WriteLine("- All attachments will be extracted in the same directory, respecting the directory structure inside the pst");
                return;
            }

            var fileName = args[0];
            var exportDirectory = CreateDirectoryIfNeeded(System.IO.Path.GetDirectoryName(fileName),
                System.IO.Path.GetFileNameWithoutExtension(fileName) + "_Export");

            var xstView = new XstReader.View();
            var xstFile = new XstReader.XstFile(xstView, fileName);
            xstFile.ReadFolderTree();
            foreach (var folder in xstView.RootFolders)
            {
                ExtractAttachmentsInFolder(xstView, xstFile, folder, exportDirectory);
            }

            Console.WriteLine("Done!");

        }

        private static string CreateDirectoryIfNeeded(string rootDirName, string dirName)
        {
            string exportDirectory = System.IO.Path.Combine(rootDirName, 
                RemoveInvalidChars(System.IO.Path.GetFileName(dirName)));
            if (!System.IO.Directory.Exists(exportDirectory))
                System.IO.Directory.CreateDirectory(exportDirectory);
            return exportDirectory;
        }



        private static string RemoveInvalidChars(string filename)
        {
            return string.Concat(filename.Split(System.IO.Path.GetInvalidFileNameChars())).TrimEnd();
        }

        private static void ExtractAttachmentsInFolder(View xstView, XstFile xstFile, XstReader.Folder folder, string exportDirectoryBase)
        {
            var exportDirectory = CreateDirectoryIfNeeded(exportDirectoryBase, RemoveInvalidChars(folder.Name));

            xstFile.ReadMessages(folder);
            foreach (var message in folder.Messages)
            {
                xstFile.ReadMessageDetails(message);
                foreach(var att in message.Attachments)
                {
                    if (att.IsFile)
                    {
                        var attachmentExpectedName = System.IO.Path.Combine(exportDirectory, att.FileName);
                        var fi = new System.IO.FileInfo(attachmentExpectedName);
                        var actionName = string.Empty;

                        if (!fi.Exists) 
                        {
                            actionName = "Create";
                        } else
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

            foreach(var subFolder in folder.Folders)
            {
                ExtractAttachmentsInFolder(xstView, xstFile, subFolder, exportDirectory);
            }

        }
    }
}
