# XstReader
Xst Reader is an open source viewer for Microsoft Outlook’s .ost and .pst files, written entirely in C#, requiring only .Net Framework 4.6.1, and with no dependency on any Microsoft Office components.

It presents as a simple, classic, three pane mail viewer:

![](screenshot5.png)

XstReader goes beyond Outlook in that it will allow you to open .ost files, which are the caches created by Outlook to hold a local copy of a mailbox. Wanting to read an .ost file as the original motivation for this project: now it also as the ability to export the header and body of an email in its native format (plain text, HTML, or rich text), and inspect and export all the properties of an email.

It requires only .Net Framework 4, which is installed by default on Windows 8.1 and later, but will need to be installed on Windows 7 and earlier systems before Xst Reader can be run.  .Net Framework 4 can be downloaded from <https://www.microsoft.com/en-us/download/details.aspx?id=17851>

Xst Reader is based on Microsoft’s documentation of the Outlook file formats in [MS-PST], first published in 2010 as part of the anti-trust settlement with the DOJ and the EU: <https://msdn.microsoft.com/en-us/library/ff385210(v=office.12).aspx>


## Installation

To install a binary:
1.	Choose a release, then download the XstReader.zip file attached to it.
2.	Extract the contents of the zip file to a programs folder.
3.	Run XstReader.exe, and create shortcuts to it as required.

## Notes for developers
There is no more XstReader.Base!

Now there is a nuget package: XstReader.Api. Every .Net developer can use it to build their own projects

This new library is designed to be used in any kind of project, and it is very different from the old XstReader.Base

---- 
### [Obsolete]
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



