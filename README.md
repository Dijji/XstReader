There are 3 projects here to view Microsoft Outlook's .ost and .pst files, all of them written entirely in C#, with no dependency on any Microsoft Office components.

# Dijji
This repo is a fork and evolution of [Dijji/XstReader](https://github.com/Dijji/XstReader), who died at the end of May 2021.

As his daughter [@seeemilyplay](https://github.com/seeemilyplay) said:
> If you have the spare money, and feel you would like to pay your respects in some way, then please donate to the MS Society (or the equivalent in your home country) https://www.mssociety.org.uk/

# Projects  
* [XstReader](#xstreader): An application to view Microsoft Outlook's .ost and .pst files (Windows)
* [XstExport](#xstexport): A Command Line tool for exporting emails, attachments or properties from an Microsoft Outlook's .ost and .pst file (cross-platform)
* [XstReader.Api](#xstreader-api): A library to read all content of Microsoft Outlook's .ost and .pst files (cross-platform)

# XstReader
Xst Reader is an open source viewer for Microsoft Outlook’s .ost and .pst files, written entirely in C#, requiring only .Net Framework 4.6.1, and with no dependency on any Microsoft Office components.

It presents as a simple, classic, three pane mail viewer:

![](screenshot5.png)

Xst Reader goes beyond Outlook in that it will allow you to open .ost files, which are the caches created by Outlook to hold a local copy of a mailbox. Wanting to read an .ost file as the original motivation for this project: now it also as the ability to export the header and body of an email in its native format (plain text, HTML, or rich text), and inspect and export all the properties of an email.

It requires only .Net Framework 4, which is installed by default on Windows 8.1 and later, but will need to be installed on Windows 7 and earlier systems before Xst Reader can be run.  .Net Framework 4 can be downloaded from <https://www.microsoft.com/en-us/download/details.aspx?id=17851>

Xst Reader is based on Microsoft’s documentation of the Outlook file formats in [MS-PST], first published in 2010 as part of the anti-trust settlement with the DOJ and the EU: <https://msdn.microsoft.com/en-us/library/ff385210(v=office.12).aspx>

## XstExport

A command line tool for exporting emails, attachments or properties from an Outlook file.

By default, all folders in the Outlook file are exported into a directory structure that mirrors the Outlook folder structure. Options are available to specify the starting Outlook folder and to collapse all output into a single directory.

The differences from the export capabilities of the UI are: the ability to export from a subtree of Outlook folders; and the ability to export attachments only, without the body of the email.

In addition to XstExport, XstPortableExport is also provided, which is a portable version based on .Net Core 2.1 that can be run on Windows, Mac, Linux etc. 

Both versions support the following options:

   XstExport.exe {-e|-p|-a|-h} [-f=`<Outlook folder>`] [-o] [-s] [-t=`<target directory>`] `<Outlook file name>`

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

To run the portable version, open a command line and run:

dotnet XstPortableExport.dll `<options as above>`

## Installation

To install a binary:
1.	Choose a release, then download the XstReader.zip file attached to it.
2.	Extract the contents of the zip file to a programs folder.
3.	Run XstReader.exe, and create shortcuts to it as required.

## Notes for developers
* The provided Visual Studio solution includes a XstReader.Base project, which contains all the basic common functionality for reading Outlook files used by XstReader and XstExport. The project builds a DLL, which you can use to add the same capability to your own projects. XstReader and XstExport do not themselves use the DLL, instead, they simply include the code from the XstReader.Base directory, in order to create executables with minimum dependencies.
* The XstPortableExport project builds a portable version of XstExport based on .Net Core 2.1. However, in order to remain portable, two areas of functionality have to be #ifdef'd out in order not to create a framework dependency and so tie the program to Windows. These are support for RTF body formats, and support for MIME decryption.
## Release History

* 1.14
    * Added command line tool for exporting email contents, attachments, or properties. Both Windows and cross-platform versions are provided.
* 1.12
    * Added support for viewing encrypted or signed messages and their attachments if matching certificate is in user certificate store. (Does not perform signature verification)
* 1.8
    * Allow searching within Cc and Bcc names
    * Show email time as well as date in list of emails in folder
    * Add an Info button in the bottom right-hand corner of the main window
* 1.7
    * Allow the contents of emails to be exported. Individual emails, multiple selected emails, or whole folders of emails may be exported. The results have the same format as the email, i.e. either HTML, Rich text format, or plain text.  
* 1.6
    * Allow searching through the listed message headers, looking for a given text
* 1.4
    * Can export message, contact et cetera properties to a CSV file 
* 1.2
    * Show inline attachments in the HTML body 
* 1.1
    * Add support for showing Recipient and Attachment properties
* 1.0
    * The first release

## Meta

Distributed under the MS-PL license. See [license](license.md) for more information.



