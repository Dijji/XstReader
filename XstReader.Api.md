# XstReader.Api
XstReader.Api is an open source library for read Microsoft Outlook’s .ost and .pst files, written entirely in C#, over .NET Standard 2.1, and with no dependency on any Microsoft Office components. Is an open source, cross-platform library that can be used in any .NET project (.NET Standard, .NET Core, .NET Framework and .NET 5+)

This library is the core of [XstReader](./XstReader) and [XstExporter](./XstExporter), included in this repository.

## Usage
The use of this library is simple. There is a little summary: 
* The .ost or .pst file is represented with `XstFile` class
* An `XstFile` has a `RootFolder` property of type `XstFolder`, with the Root folder of message structure in the file
* Each `XstFolder` has:
  * `Messages` with all messages included in the Folder (type: `XstMessage`)
  * `Folders` with all Folders under that Folder (type: `XstFolder`)
* Each `XstMessage` has:
  * `Recipients` with all the Recipients involved in the Message (type: `XstRecipient`)
  * `Body` with the body of the Message (type: `XstMessageBody`)
  * `Attachments` with all the Attachments of the Message (type: `XstAttachment`)
* Each Folder (`XstForlder`), Message (`XstMessage`), Recipient (`XstRecipient`) and Attachment (`XstAttachment`) have its own `Properties`
* You can call `ClearContents()` of an element to release internal data used by the object: it will find the released data  again when needed.

### Examples
Usage of XstReader.Api is simple. Here are some basic examples to show how easy it is to use the library. Obviously, this is just a simplification: objects have many more methods and properties than those shown.

#### Open .ost or .pst file
```csharp
public void OpenOstOrPstFile(string fileName)
{
  // Note the "using": XstFile implements IDisposable
  // The file remains opened until dispose of XstFile
  using(var xstFile = new XstFile(fileName))
  {
    ProcessFolder(xstFile.RootFolder);

    // We don't need more the data inside the folder
    childFolder.ClearContents(); 
  }
}
```

#### Processing a Folder
```csharp
public void ProcessFolder(XstFolder folder)
{
  // We can process the properties of the Folder
  var properties = folder.Properties; 

  // Messages in the folder
  foreach(var message in folder.Messages)
  {
    ProcessMessage(message);
  }

  // Folders inside the folder
  foreach(var childFolder in folder.Folders)
  {
    ProcessFolder(childFolder);
  }
}
```

#### Processing a Message
```csharp
public void ProcessMessage(XstMessage message)
{
  // We can process the properties of the Message
  var properties = message.Properties;
  
  // Recipients of the Message
  ProcessRecipients(message.Recipients);
  
  // Body of the Message
  ProcessBody(message.Body);
  
  // Attachments in the message
  foreach (var attachment in message.Attachments)
  {
    ProcessAttachment(attachment);
  }
}
```

#### Processing Recipients
```csharp
public void ProcessRecipients(XstRecipientSet recipients)
{
  // We have info about recipients involved in a Message:
  XstRecipient originator = recipients.Originator;
  XstRecipient originalSentRepresenting = recipients.OriginalSentRepresenting;
  XstRecipient sentRepresenting = recipients.SentRepresenting;
  XstRecipient sender = recipients.Sender;
  IEnumerable<XstRecipient> to = recipients.To;
  IEnumerable<XstRecipient> cc = recipients.Cc;
  IEnumerable<XstRecipient> bcc = recipients.Bcc;
  XstRecipient receivedBy = recipients.ReceivedBy;
  XstRecipient receivedRepresenting = recipients.ReceivedRepresenting;

  // All Recipients with its own properties:
  var senderProperties = sender.Properties;
}
```

#### Processing Body
```csharp
public void ProcessBody(XstMessageBody body)
{
  switch (body.Format)
  {
    case XstMessageBodyFormat.Html:
	  Console.Write("body in html"); break;
    case XstMessageBodyFormat.Rtf:
      Console.Write("body in rtf"); break;
    case XstMessageBodyFormat.PlainText:
      Console.Write("body in txt"); break;
  }

  // The Body in the format can be accessed by text or bytearray
  var text = body.Text;
  var bytes = body.Bytes;
}
```

#### Processing Attachment
```csharp
public void ProcessAttachment(XstAttachment attachment)
{
  string fileName = "myAttachment";

  // We can process the properties of the Attachment
  var properties = attachment.Properties;

  // We can open attached Messages
  if (attachment.IsEmail)
    ProcessMessage(attachment.AttachedEmailMessage);
  // We can save attachments
  else if (attachment.IsFile)
    attachment.SaveToFile(fileName, attachment.LastModificationTime);
}
```

## Installation
Use the Nuget package in your projects: 
[![Nuget](https://img.shields.io/nuget/v/XstReader.Api?style=plastic)](https://www.nuget.org/packages/XstReader.Api/)

Go to [Nuget project page](https://www.nuget.org/packages/XstReader.Api/) to see options


## More information
* [Readme](./README.md)
* [XstReader](./XstReader.md)
* [XstExporter](./XstExporter.md)
* [XstReader.Api](./XstReader.Api.md)
* [Release Notes](./ReleaseNotes.md)
* [License](./license.md)

## License
Distributed under the MS-PL license. See [license](license.md) for more information.