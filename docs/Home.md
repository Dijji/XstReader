### An open source viewer for Microsoft Outlook’s .ost and .pst files, written entirely in C#

**New:  Version 1.1 released as a beta.**  In version 1.0, only the properties of Messages were exposed. In version 1.1, the properties pane shows the properties of the most recently clicked Message, Recipient or Attachment..
 
Xst Reader is an open source viewer for Microsoft Outlook’s .ost and .pst files, written entirely in C#, requiring only .Net Framework 4, and with no dependency on any Microsoft Office components.

It presents as a simple, classic, three pane mail viewer:

 ![](Home_screenshot0.9.jpg)
It allows viewing of emails in plain text, HTML, and RTF formats.  Attachments are listed and can be saved as separate files.  Xst Reader will read both current 64-bit Unicode .ost and .pst files and older 32-bit ANSI formats.

Xst Reader goes beyond Outlook in that it will allow you to open .ost files, which are the caches created by Outlook to hold a local copy of a mailbox. Wanting to read an .ost file was the original motivation for this project: knowing that documentation of the format existed, I thought it would only take me a couple of days. In fact, it rapidly became clear that I’d bitten into something considerably more complex than that. But once I’d started…

It requires only .Net Framework 4, which is installed by default on Windows 8.1 and later, but will need to be installed on Windows 7 and earlier systems before Xst Reader can be run.  .Net Framework 4 can be downloaded from [https://www.microsoft.com/en-us/download/details.aspx?id=17851](https://www.microsoft.com/en-us/download/details.aspx?id=17851)

Xst Reader is based on Microsoft’s documentation of the Outlook file formats in {"[MS-PST](MS-PST)"}, first published in 2010 as part of the anti-trust settlement with the DOJ and the EU: [https://msdn.microsoft.com/en-us/library/ff385210(v=office.12).aspx](https://msdn.microsoft.com/en-us/library/ff385210(v=office.12).aspx) 

Xst Reader was created because I couldn’t find an open source .ost and .pst file reader using a modern garbage collected language and framework. Here are some other open source .ost and .pst file readers that I did find:

* PST File Format SDK. An official Microsoft implementation of {"[MS-PST](MS-PST)"}, written in C++. The viewer that comes with it presents file internals rather than anything usable: [https://pstsdk.codeplex.com/](https://pstsdk.codeplex.com/)
* pstsdk.net: .NET port of PST File Format SDK, coming out of the same team. This would have been a natural basis for my efforts, but the project seems to have stalled back in 2012: [https://pstsdknet.codeplex.com/](https://pstsdknet.codeplex.com/)
* libpff. A library and tools for accessing .ost and .pst files, written in C. Built for forensic analysis purposes by a German company, and predating {"[MS-PST](MS-PST)"} but updated since, this is probably the most comprehensive program for extracting the contents of these files, but does not include any useful UI: [https://github.com/libyal/libpff](https://github.com/libyal/libpff)
Another approach to accessing an .ost file is to convert it to a .pst file, so that it can be opened in Outlook.
Xst Reader does not support this, and I know of no open source programs that do. libpff extracts the contents of emails into standard files, but there is nothing that will write emails out into another mail format. 

Going beyond open source, there are quite a few companies offering commercial software which will read .ost files and convert them to .pst files. I haven’t found any that are free and catch-free, or tried any of them, and I make no recommendations. Examples are:
* OST2 - an OST to PST converter, free if your file is connected to Outlook (probably Office dependent), purchase needed to operate standalone: [http://www.ost2.com/](http://www.ost2.com/)
* OST to PST Converter Tool will view the contents of an .ost file for free,  but exporting files requires a licence. This is more the typical pattern: [http://www.osttopstfile.co.uk/](http://www.osttopstfile.co.uk/)


 


