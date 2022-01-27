# XstReader
XstReader is an open source viewer for Microsoft Outlook’s .ost and .pst files, written entirely in C#, requiring only .Net Framework 4.6.1, and with no dependency on any Microsoft Office components.

It presents as a simple, classic, three pane mail viewer:

![XstReader ScreenShot](https://raw.githubusercontent.com/iluvadev/XstReader/master/docs/img/Home_screenshot0.9.jpg)

XstReader goes beyond Outlook in that it will allow you to open .ost files, which are the caches created by Outlook to hold a local copy of a mailbox. Wanting to read an .ost file as the original motivation for this project: now it also as the ability to export the header and body of an email in its native format (plain text, HTML, or rich text), and inspect and export all the properties of an email.

It requires only .Net Framework 4.6.1. It can be downloaded from <https://www.microsoft.com/es-es/download/details.aspx?id=49982>

XstReader is based on Microsoft’s documentation of the Outlook file formats in [MS-PST], first published in 2010 as part of the anti-trust settlement with the DOJ and the EU: <https://msdn.microsoft.com/en-us/library/ff385210(v=office.12).aspx>

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
