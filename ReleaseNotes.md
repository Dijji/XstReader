# Release Notes
**XstReader.Api v.1.0.3**:
* Optimized check if mail is Signed or encrypted
* XstElements can access XstFile
* Fixed some errors in comments

**XstReader.Api v.1.0.2**:
* Fixed error with intellisense in nuget package

## 2022-01-26
**All projects**:
* New folder structure for code
* Changed name of the project XstExport to XstExporter
* Changed name of the project XstPortableExport to XstExporter.Portable
* Changed project XstReader.Base to XstReader.Api
* Adopted [Semver](https://semver.org/) in all projects

**XstReader v.1.15.0**:
* Uses .Net Framework 4.6.1
* Includes reference to Project *XstReader.Api*, not link to the code
* It's possible view the properties of selected Folder
* Properties list with much more information, with documented description
* More information in header messages
* Some more improvements in the information included in the message list

**XstExporter v.2.0.1**:
* Uses .Net Framework 4.6.1
* Includes reference to Project *XstReader.Api*, not link to the code

**XstExporter.Portable v.2.0.1**
* Includes reference to Project *XstReader.Api*, not link to the code

**XstReader.Api v.1.0.1**:
* Converted the project to .Net Standard
* Refactored the whole project in order to make a versatile library
* The .ost/.pst file is kept open while in use
* XstFile implements IDisposable
* All internal data is obtained when needed
* XstFolder has properties
* Encoded and classified all the properties defined in the MS doc, with their descriptions
* DateTime values of properties are displayed in UTC
* Automatic processing of signed and / or encrypted messages
* More information about "Recipients" involved in a message

## Old versions

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




