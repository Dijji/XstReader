# XstExporter
A command line tool for exporting emails, attachments or properties from an Outlook file.

By default, all folders in the Outlook file are exported into a directory structure that mirrors the Outlook folder structure. Options are available to specify the starting Outlook folder and to collapse all output into a single directory.

The differences from the export capabilities of the UI are: the ability to export from a subtree of Outlook folders; and the ability to export attachments only, without the body of the email.

In addition to XstExport, XstPortableExport is also provided, which is a portable version based on .Net Core 2.1 that can be run on Windows, Mac, Linux etc. 

Both versions support the following options:

   XstExporter.exe {-e|-p|-a|-h} [-f=`<Outlook folder>`] [-o] [-s] [-t=`<target directory>`] `<Outlook file name>`

Where:

   -e, --email  
      Export in native body format (.html, .rtf, .txt)
      with attachments in associated folder   
   -- OR --   
   -p, --properties  
      Export properties only (in CSV file)   
   -- OR --   
   -a, --attachments  
      Export attachments only
      (Latest date wins in case of name conflict)  
   -- OR --  
   -h, --help  
      Display this help

   -f=`<Outlook folder>`, -folder=`<Outlook folder>`  
      Folder within the Outlook file from which to export.
      This may be a partial path, for example "Week1\Sent"

   -o, --only  
      If set, do not export from subfolders of the nominated folder.

   -s, --subfolders  
      If set, Outlook subfolder structure is preserved.
      Otherwise, all output goes to a single directory

   -t=`<target directory name>`, --target=`<target directory name>`  
      The directory to which output is written. This may be an
      absolute path or one relative to the location of the Outlook file.
      By default, output is written to a directory `<Outlook file name>.Export.<Command>`
      created in the same directory as the Outlook file

   `<Outlook file name>`  
      The full name of the .pst or .ost file from which to export

## XstExporter.Portable
The XstExporter.Portable is a version of XstExporter based on .Net Core 2.1. However, in order to remain portable, two areas of functionality have to be #ifdef'd out in order not to create a framework dependency and so tie the program to Windows. These are support for RTF body formats, and support for MIME decryption.

To run the portable version, open a command line and run:

dotnet XstExporter.Portable.dll `<options as above>`


## Installation
To install a binary:
1. Choose a release, then download the XstReader-[release].zip file attached to it.
2. Extract the contents of the zip file to a programs folder.
3. Run XstReader.exe, and create shortcuts to it as required.


## More information
* [Readme](./README.md)
* [XstReader](./XstReader.md)
* [XstExporter](./XstExporter.md)
* [XstReader.Api](./XstReader.Api.md)
* [Release Notes](./ReleaseNotes.md)
* [License](./license.md)

## License
Distributed under the MS-PL license. See [license](license.md) for more information.
