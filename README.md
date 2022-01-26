This repository includes 3 projects ([XstReader](#xstreader), [XstExporter](#xstexporter) and [XstReader.Api](#xstreaderapi)) to read, view and export folders, messages, attachments, recipients and properties from Microsoft Outlook's .ost and .pst files (also those protected by unknown password). All these projects are written entirely in C#, with no dependency on any Microsoft Office components.

XstReader (and subprojects) is based on Microsoft’s documentation of the Outlook file formats in [MS-PST], first published in 2010 as part of the anti-trust settlement with the DOJ and the EU: <https://msdn.microsoft.com/en-us/library/ff385210(v=office.12).aspx>

## Original code
This repo is a fork and evolution of [Dijji/XstReader](https://github.com/Dijji/XstReader)... [Dijji](https://github.com/Dijji) died at the end of May 2021

All the code here is the product of his wisdom or is inspired by his work.

As his daughter [@seeemilyplay](https://github.com/seeemilyplay) said:
> If you have the spare money, and feel you would like to pay your respects in some way, then please donate to the MS Society (or the equivalent in your home country) https://www.mssociety.org.uk/

# Projects  
### [XstReader](./XstReader.md)
![XstReader ScreenShot](https://raw.githubusercontent.com/iluvadev/XstReader/master/docs/img/Home_screenshot0.9_small.jpg)

An application to view Microsoft Outlook's .ost and .pst files:
* Can open .ost and .pst files
* With no dependency on any Microsoft Office component
* You can inspect all the properties of Folders, Emails, Attachments and Recipients
* You can export email properties and email body in its native format (plain text, Html or rich text)
* Only requires .Net Framework 4.6.1 (for Windows)

More information in [XstReader.md](./XstReader.md)

### [XstExporter](./XstExporter.md)
![XstExporter ScreenShot](https://raw.githubusercontent.com/iluvadev/XstReader/master/docs/img/XstExporter-Screenshot01_small.png)

A Command Line tool for exporting emails, attachments or properties from an Microsoft Outlook's .ost and .pst file:
* With the ability to export from a subtree of Outlook folders
* Can export attachments only, without the body of the email.
* Is built over .Net Framework 4.6.1 (for Windows)
* There is a *Portable* version based on .Net Core 2.1 (cross-platform)

More information in [XstExporter.md](./XstExporter.md)

### [XstReader.Api](./XstReader.Api.md)
A library to read all content of Microsoft Outlook's .ost and .pst files:
* Versatile library, easy to use
* Provide all funcionality of **XstReader** and **XstExporter**
* Is built with .Net Standard 2.0 (cross-platform)

More information in [XstReader.Api](./XstReader.Api.md)

## Installation
To install a binary:
1. Choose a release, then download the XstReader-[release].zip file attached to it.
2. Extract the contents of the zip file to a programs folder.
3. Run XstReader.exe, and create shortcuts to it as required.


## More information
* [XstReader](./XstReader.md)
* [XstExporter](./XstExporter.md)
* [XstReader.Api](./XstReader.Api.md)
* [Release Notes](./ReleaseNotes.md)
* [License](./license.md)

## License
Distributed under the MS-PL license. See [license](license.md) for more information.



