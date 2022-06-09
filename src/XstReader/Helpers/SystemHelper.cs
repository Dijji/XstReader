using System.Diagnostics;
using System.Runtime.InteropServices;

namespace XstReader.App.Helpers
{
    public static class SystemHelper
    {
        public static void OpenWith(string? fileFullname)
        {
            if (string.IsNullOrEmpty(fileFullname) || !File.Exists(fileFullname))
                return;

            if (Environment.OSVersion.Version.Major > 5)
            {
                IntPtr hwndParent = Process.GetCurrentProcess().MainWindowHandle;
                tagOPENASINFO oOAI = new tagOPENASINFO
                {
                    cszFile = fileFullname,
                    cszClass = String.Empty,
                    oaifInFlags = tagOPEN_AS_INFO_FLAGS.OAIF_ALLOW_REGISTRATION | tagOPEN_AS_INFO_FLAGS.OAIF_EXEC
                };
                SHOpenWithDialog(hwndParent, ref oOAI);
            }
            else
            {
                using (Process.Start("rundll32", "shell32.dll,OpenAs_RunDLL " + fileFullname)) { }
            }
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
