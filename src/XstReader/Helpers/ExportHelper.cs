namespace XstReader.App.Helpers
{
    public static class ExportHelper
    {
        private static SaveFileDialog SaveFileDialog = new();
        private static FolderBrowserDialog FolderBrowserDialog = new() { ShowNewFolderButton = true };

        public static bool AskDirectoryPath(ref string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
                FolderBrowserDialog.SelectedPath = path;

            if (FolderBrowserDialog.ShowDialog() != DialogResult.OK)
                return false;
            path = FolderBrowserDialog.SelectedPath;
            return true;
        }

        public static bool AskFileName(ref string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
                SaveFileDialog.FileName = fileName;

            if (SaveFileDialog.ShowDialog() != DialogResult.OK)
                return false;
            fileName = SaveFileDialog.FileName;
            return true;
        }


        public static bool ExportAttachmentsToDirectory(XstFolder? folder, bool includeSubfolders)
        {
            if (folder == null)
                return false;

            if (FolderBrowserDialog.ShowDialog() != DialogResult.OK)
                return false;

            return ExportAttachmentsToDirectory(folder, FolderBrowserDialog.SelectedPath, includeSubfolders);
        }

        public static bool ExportAttachmentsToDirectory(XstFolder? folder, string path, bool includeSubfolders)
        {
            if (folder == null)
                return false;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            bool ret = true;
            if (!ExportAttachmentsToDirectory(folder.Messages.SelectMany(m => m.Attachments.Where(a => a.IsFile && !a.IsHidden)), path))
                ret = false;

            if (includeSubfolders)
            {
                foreach (var subFolder in folder.Folders)
                {
                    var subfolderPathBase = Path.Combine(path, subFolder.DisplayName.ReplaceInvalidFileNameChars("_"));
                    var subfolderPath = subfolderPathBase;
                    int i = 1;
                    while (Directory.Exists(subfolderPath))
                        subfolderPath = $"{subfolderPathBase}({i++})";
                    if (!ExportAttachmentsToDirectory(subFolder, subfolderPath, includeSubfolders))
                        ret = false;
                }
            }
            return ret;
        }

        public static bool ExportAttachmentsToDirectory(IEnumerable<XstAttachment>? attachments)
        {
            if (!(attachments?.Any() ?? false))
                return false;

            if (FolderBrowserDialog.ShowDialog() != DialogResult.OK)
                return false;

            return ExportAttachmentsToDirectory(attachments, FolderBrowserDialog.SelectedPath);
        }

        public static bool ExportAttachmentsToDirectory(IEnumerable<XstAttachment>? attachments, string path)
        {
            if (!(attachments?.Any() ?? false))
                return false;
            bool ret = true;
            foreach (var attachment in attachments)
            {
                string extension = Path.GetExtension(attachment.FileNameForSaving);
                string fileNameBase = Path.Combine(path, Path.GetFileNameWithoutExtension(attachment.FileNameForSaving));
                string fileName = fileNameBase + extension;
                int i = 1;
                while (File.Exists(fileName))
                    fileName = $"{fileNameBase}({i++}){extension}";
                try { attachment.SaveToFile(fileName); }
                catch { ret = false; }
            }
            return ret;
        }


        public static bool ExportFolderToHtmlFiles(XstFolder? folder, bool includeSubfolders)
        {
            if (folder == null)
                return false;

            if (FolderBrowserDialog.ShowDialog() != DialogResult.OK)
                return false;

            return ExportFolderToHtmlFiles(folder, FolderBrowserDialog.SelectedPath, includeSubfolders);
        }
        public static bool ExportFolderToHtmlFiles(XstFolder? folder, string path, bool includeSubfolders)
        {
            if (folder == null)
                return false;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            bool ret = true;
            foreach (var message in folder.Messages.OrderBy(m => m.Date ?? DateTime.MinValue))
            {
                string fileNameBase = Path.Combine(path, message.GetFilenameForExport());
                string fileName = fileNameBase + ".html";
                int i = 1;
                while (File.Exists(fileName))
                    fileName = $"{fileNameBase}({i++}).html";
                if (!ExportMessageToHtmlFile(message, fileName))
                    ret = false;
            }
            if (includeSubfolders)
            {
                foreach (var subFolder in folder.Folders)
                {
                    var subfolderPathBase = Path.Combine(path, subFolder.DisplayName.ReplaceInvalidFileNameChars("_"));
                    var subfolderPath = subfolderPathBase;
                    int i = 1;
                    while (Directory.Exists(subfolderPath))
                        subfolderPath = $"{subfolderPathBase}({i++})";
                    if (!ExportFolderToHtmlFiles(subFolder, subfolderPath, includeSubfolders))
                        ret = false;
                }
            }
            return ret;
        }

        public static bool ExportMessageToHtmlFile(XstMessage? message, bool openFile)
        {
            if (message == null)
                return false;

            SaveFileDialog.FileName = message.GetFilenameForExport() + ".html";


            if (SaveFileDialog.ShowDialog() == DialogResult.OK &&
                ExportMessageToHtmlFile(message, SaveFileDialog.FileName))
            {
                if (openFile)
                    SystemHelper.OpenWith(SaveFileDialog.FileName);
                return true;
            }
            return false;
        }

        public static bool ExportMessageToHtmlFile(XstMessage? message, string fileName)
        {
            if (message == null)
                return false;

            try
            {
                File.WriteAllText(fileName, message.RenderAsHtml(false));
                if (message.Date.HasValue)
                    File.SetCreationTime(fileName, message.Date.Value);
            }
            catch { return false; }
            return true;
        }
    }
}
