using System.Text;

namespace XstReader.App
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Add a reference to the NuGet package System.Text.Encoding.CodePages for .Net core only
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Krypton.Toolkit.PaletteTools.ApplyTheme(new Krypton.Toolkit.KryptonManager(), Krypton.Toolkit.PaletteModeManager.Office365Silver);
            

            Application.Run(new MainForm());
        }
    }
}