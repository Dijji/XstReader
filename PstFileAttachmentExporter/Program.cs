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
            var exportDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(fileName),
                System.IO.Path.GetFileNameWithoutExtension(fileName) + "_Export");
            if (!System.IO.Directory.Exists(exportDirectory))
                System.IO.Directory.CreateDirectory(exportDirectory);

            var xstView = new XstReader.View();
            var xstFile = new XstReader.XstFile(xstView, fileName);
            xstFile.ReadFolderTree();
            foreach (var folder in xstView.RootFolders)
            {
                ExtractAttachmentsInFolder(xstView, xstFile, folder, exportDirectory);
            }

            Console.WriteLine("Done!");

        }

        private static void ExtractAttachmentsInFolder(View xstView, XstFile xstFile, XstReader.Folder folder, string exportDirectoryBase)
        {
            //Console.WriteLine("Folder : " +folder.Name);
            string exportDirectory = System.IO.Path.Combine(exportDirectoryBase, folder.Name);
            if (!System.IO.Directory.Exists(exportDirectory))
                System.IO.Directory.CreateDirectory(exportDirectory);

            xstFile.ReadMessages(folder);
            foreach (var message in folder.Messages)
            {
                xstFile.ReadMessageDetails(message);
                //Console.WriteLine("Message : " + message.Subject);
                foreach(var att in message.Attachments)
                {
                    if (att.IsFile)
                    {
                        Console.WriteLine("Extract : " + System.IO.Path.Combine(exportDirectory , att.FileName));
                        xstFile.SaveAttachmentToFolder(exportDirectory, null, att);
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
