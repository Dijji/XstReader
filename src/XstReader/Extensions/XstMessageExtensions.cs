using Razor.Templating.Core;
using System.Text;
using XstReader.App.Common;
using XstReader.ElementProperties;
using XstReader.Razor.Templates;

namespace XstReader.App
{
    public static class XstMessageExtensions
    {
        public static string RenderAsHtml(this XstMessage? message, bool isInApp)
        {
            if(message == null)
                return string.Empty;

            RenderOptions.ExportOptions = XstReaderEnvironment.Options.ExportOptions.Clone();
            if (isInApp)
            {
                RenderOptions.ExportOptions.EmbedAttachmentsInFile = false;
                RenderOptions.ExportOptions.ShowDetails = false;
            }

            RenderOptions.Initialize();

            return Task.Run(() => RazorTemplateEngine.RenderAsync("~/XstMessageView.cshtml", message))
                       .GetAwaiter()
                       .GetResult();
        }

    }
}
