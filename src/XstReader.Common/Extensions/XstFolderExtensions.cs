using XstReader;

namespace XstReader
{
    public static class XstFolderExtensions
    {
        public static string GetId(this XstFolder folder)
            => folder?.Path ?? string.Empty;

        public static DateTime GetDate(this XstFolder folder)
            => folder?.LastModificationTime ??
               DateTime.MinValue.ToUniversalTime();

        public static int CountTreeSubfolders(this XstFolder folder)
            => folder == null ? 0
               : folder.Folders.Count() + folder.Folders.Sum(f => f.CountTreeSubfolders());
    }
}
