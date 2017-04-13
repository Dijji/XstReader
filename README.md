# Xst Reader
Xst Reader is an open source viewer for Microsoft Outlook’s .ost and .pst files, written entirely in C#, requiring only .Net Framework 4, and with no dependency on any Microsoft Office components.

It presents as a simple, classic, three pane mail viewer:

![](screenshot5.png)

Xst Reader goes beyond Outlook in that it will allow you to open .ost files, which are the caches created by Outlook to hold a local copy of a mailbox. Wanting to read an .ost file was the original motivation for this project: knowing that documentation of the format existed, I thought it would only take me a couple of days. In fact, it rapidly became clear that I’d bitten into something considerably more complex than that. But once I’d started…

It requires only .Net Framework 4, which is installed by default on Windows 8.1 and later, but will need to be installed on Windows 7 and earlier systems before Xst Reader can be run.  .Net Framework 4 can be downloaded from <https://www.microsoft.com/en-us/download/details.aspx?id=17851>

Xst Reader is based on Microsoft’s documentation of the Outlook file formats in [MS-PST], first published in 2010 as part of the anti-trust settlement with the DOJ and the EU: <https://msdn.microsoft.com/en-us/library/ff385210(v=office.12).aspx>

## Installation

To install a binary:
1.	Choose a release, then download the XstReader.zip file attached to it.
2.	Extract the contents of the zip file to a programs folder.
3.	Run XstReader.exe, and create shortcuts to it as required.

## Release History

* 1.1
    * Add support for showing Recipient and Attachment properties
* 1.0
    * The first release

## Meta

Distributed under the MS-PL license. See [license](license.md) for more information.



