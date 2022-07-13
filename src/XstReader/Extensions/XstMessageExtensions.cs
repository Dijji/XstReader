using XstReader.Exporter;

namespace XstReader.App
{
    public static class XstMessageExtensions
    {
        public static string RenderAsHtml(this XstMessage? message, bool isInApp)
        {
            if (message == null)
                return string.Empty;

            var exporter = new ExporterHtml();
            exporter.ExportOptions.EmbedAttachmentsInFile = !isInApp;
            exporter.ExportOptions.ShowDetails = !isInApp;

            return exporter.Render(message);
        }

        public static string GetFilenameForExport(this XstMessage? message)
            => message?.DisplayName.ReplaceInvalidFileNameChars("_") ?? "No_Name";

    }
}
