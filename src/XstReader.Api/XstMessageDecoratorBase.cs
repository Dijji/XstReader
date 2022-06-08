using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using XstReader.ElementProperties;

namespace XstReader
{
    /// <summary>
    /// Base class for Message Decorators
    /// </summary>
    public abstract class XstMessageDecoratorBase : XstMessage
    {
        /// <summary>
        /// The Message
        /// </summary>
        protected XstMessage Message { get; private set; }

        #region Structure Class properties
        /// <summary>
        /// The Folder of this Message
        /// </summary>
        [DisplayName("Parent Folder")]
        [Category("General")]
        [Description("The Folder of this Message")]
        public override XstFolder ParentFolder => Message.ParentFolder;

        /// <summary>
        /// If is an Attached Message, contains the Attachement
        /// </summary>
        [DisplayName("Parent Attachment")]
        [Category("General")]
        [Description("If is an Attached Message, contains the Attachement")]
        public override XstAttachment ParentAttachment => Message.ParentAttachment;

        /// <summary>
        /// The Container File
        /// </summary>
        [DisplayName("File")]
        [Category("General")]
        [Description("The Container File")]
        public override XstFile XstFile => Message.XstFile;
        #endregion Structure Class properties

        #region Sending Class Properties
        /// <summary>
        /// The Recipients involved in the Message
        /// </summary>
        [Browsable(false)]
        public override XstRecipientSet Recipients => Message.Recipients;

        /// <summary>
        /// The Subject of the Message
        /// </summary>
        [DisplayName("Subject")]
        [Category(@"General Message Properties")]
        [Description(@"Contains the subject of the email message.")]
        public override string Subject => Message.Subject;

        /// <summary>
        /// The Cc Summary of the Message
        /// </summary>
        [DisplayName("Display Cc")]
        [Category(@"Message Properties")]
        [Description(@"Contains a list of carbon copy (Cc) recipient display names.")]
        public override string Cc => Message.Cc;

        /// <summary>
        /// The To Summary of the Message
        /// </summary>
        [DisplayName("Display To")]
        [Category(@"Message Properties")]
        [Description(@"Contains a list of the primary recipient display names, separated by semicolons, when an email message has primary recipients .")]
        public override string To => Message.To;
        /// <summary>
        /// The From Summary of the Message
        /// </summary>
        [DisplayName("Sender Name")]
        [Category(@"Address Properties")]
        [Description(@"Contains the display name of the sending mailbox owner.")]
        public override string From => Message.From;

        /// <summary>
        /// Indicates if the Message is sent in representation of other 
        /// </summary>
        [DisplayName("Is Sent Representing Other")]
        [Category("General")]
        [Description(@"Indicates if the Message is sent in representation of other.")]
        public override bool IsSentRepresentingOther => Message.IsSentRepresentingOther;

        /// <summary>
        /// Indicates if the Messages was received representing other
        /// </summary>
        [DisplayName("Is Received Representing Other")]
        [Category("General")]
        [Description(@"Indicates if the Messages was received representing other.")]
        public override bool IsReceivedRepresentingOther => Message.IsReceivedRepresentingOther;

        /// <summary>
        /// DateTime when Message was submitted
        /// </summary>
        [DisplayName("Client Submit Time")]
        [Category(@"Message Time Properties")]
        [Description(@"Contains the current time, in UTC, when the email message is submitted.")]
        public override DateTime? SubmittedTime => Message.SubmittedTime;
        /// <summary>
        /// DateTime when Message was received
        /// </summary>
        [DisplayName("Message Delivery Time")]
        [Category(@"Message Time Properties")]
        [Description(@"Specifies the time (in UTC) when the server received the message.")]
        public override DateTime? ReceivedTime => Message.ReceivedTime;
        /// <summary>
        /// DateTime of the Message (Received or Submitted)
        /// </summary>
        [DisplayName("Date")]
        [Category("General")]
        [Description(@"DateTime of the Message (Received or Submitted)")]
        public override DateTime? Date => Message.Date;
        #endregion Sending Class Properties

        #region Message State Class Properties
        /// <summary>
        /// The Flags of the Message
        /// </summary>
        [DisplayName("Message Flags")]
        [Category(@"General Message Properties")]
        [Description(@"Specifies the status of the Message object.")]
        public override MessageFlags? Flags => Message.Flags;

        /// <summary>
        /// The Status of the Message
        /// </summary>
        [DisplayName("Message Status")]
        [Category(@"General Message Properties")]
        [Description(@"Specifies the status of a message in a contents table.")]
        public override MessageStatus? Status => Message.Status;

        /// <summary>
        /// The Priority of the message
        /// </summary>
        [DisplayName("Priority")]
        [Category(@"Email")]
        [Description(@"Indicates the client's request for the priority with which the message is to be sent by the messaging system.")]
        public override MessagePriority? Priority => Message.Priority;

        /// <summary>
        /// The Importance of the Message
        /// </summary>
        [DisplayName("Importance")]
        [Category(@"General Message Properties")]
        [Description(@"Indicates the level of importance assigned by the end user to the Message object.")]
        public override MessageImportance? Importance => Message.Importance;

        /// <summary>
        /// The Sensitivity of the Message
        /// </summary>
        [DisplayName("Sensitivity")]
        [Category(@"General Message Properties")]
        [Description(@"Indicates the sender's assessment of the sensitivity of the Message object.")]
        public override MessageSensitivity? Sensitivity => Message.Sensitivity;

        /// <summary>
        /// The Internet Message Id
        /// </summary>
        [DisplayName("Internet Message Id")]
        [Category(@"MIME Properties")]
        [Description(@"Corresponds to the message-id field.")]
        public override string InternetMessageId => Message.InternetMessageId;

        /// <summary>
        /// Indicates if the Message was been read
        /// </summary>
        [DisplayName("Is Read")]
        [Category("General")]
        [Description(@"Indicates if the Message was been read")]
        public override bool IsRead => Message.IsRead;

        /// <summary>
        /// Indicates if the Message is a Draft (unsent message)
        /// </summary>
        [DisplayName("Is Draft")]
        [Category("General")]
        [Description(@"Indicates if the Message is a Draft (unsent message)")]
        public override bool IsDraft => Message.IsDraft;

        /// <summary>
        /// Indicates if the Message is Encrypted or signed
        /// </summary>
        [DisplayName("Is Encrypted or Signed")]
        [Category("General")]
        [Description(@"Indicates if the Message is Encrypted or signed")]
        public override bool IsEncryptedOrSigned => Message.IsEncryptedOrSigned;

        /// <summary>
        /// Indicates if the Message is an attachment of other Message
        /// </summary>
        [DisplayName("Is Attached")]
        [Category("General")]
        [Description(@"Indicates if the Message is an attachment of other Message")]
        public override bool IsAttached => Message.IsAttached;
        #endregion Message State Class Properties

        #region Body Class Properties
        /// <summary>
        /// The Message Encoding
        /// </summary>
        [DisplayName("Encoding")]
        [Category("General")]
        [Description(@"The Message Encoding")]
        public override Encoding Encoding => Message.Encoding;
        /// <summary>
        /// The Body of the Message
        /// </summary>
        [Browsable(false)]
        public override XstMessageBody Body => Message.Body;
        #endregion Body Class Properties

        #region Attachments Class Properties
        /// <summary>
        /// The Attachments of the Message
        /// </summary>
        [Browsable(false)]
        public override IEnumerable<XstAttachment> Attachments => Message.Attachments;

        /// <summary>
        /// The Files Attached to the Message
        /// </summary>
        [Obsolete("This property is Obsolete. Use Attachments.Files() instead")]
        [Browsable(false)]
        public override IEnumerable<XstAttachment> AttachmentsFiles => Message.AttachmentsFiles;
        
        /// <summary>
        /// The Visible Files Attached to the Message
        /// </summary>
        [Obsolete("This property is Obsolete. Use Attachments.VisibleFiles() instead")]
        [Browsable(false)]
        public override IEnumerable<XstAttachment> AttachmentsVisibleFiles => Message.AttachmentsVisibleFiles;
        
        /// <summary>
        /// Indicates if the Message has Attachments
        /// </summary>
        [DisplayName("Has Attachments")]
        [Category("General")]
        [Description(@"Indicates if the Message has Attachments")]
        public override bool HasAttachments => Message.HasAttachments;
        
        /// <summary>
        /// Indicates if the Message may have Attachments inline, incrusted in the body
        /// </summary>
        [Obsolete("This property is Obsolete. Use Attachments.Inlines().Any() instead")]
        [Browsable(false)]
        public override bool MayHaveAttachmentsInline => Message.MayHaveAttachmentsInline;

        /// <summary>
        /// Indicates if the Message has any File attached
        /// </summary>
        [Obsolete("This property is Obsolete. Use Attachments.Files().Any() instead")]
        [Browsable(false)]
        public override bool HasAttachmentsFiles => Message.HasAttachmentsFiles;

        /// <summary>
        /// Indicates if the Message has any Visible File attached
        /// </summary>
        [Obsolete("This property is Obsolete. Use Attachments.VisibleFiles().Any() instead")]
        [Browsable(false)]
        public override bool HasAttachmentsVisibleFiles => Message.HasAttachmentsVisibleFiles;
        #endregion Attachments Class Properties


        #region Attachments
        /// <summary>
        /// Returns the Attachments of this Message
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<XstAttachment> GetAttachments() => Message.GetAttachments();
        #endregion Attachments

        #region Body
        /// <summary>
        /// Obtains the Body of the Message
        /// </summary>
        /// <returns></returns>
        public override XstMessageBody GetBody() => Message.GetBody();
        #endregion Body


        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message"></param>
        protected XstMessageDecoratorBase(XstMessage message)
        {
            Message = message;
        }
        #endregion Ctor

    }
}
