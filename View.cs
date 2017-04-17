// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace XstReader
{
    // These classes are the view of the xst (.ost and .pst) file rendered by XAML
    // The data layer is effectively provided by the xst file itself

    class View : INotifyPropertyChanged
    {
        private Folder selectedFolder = null;
        private Stack<Message> stackMessage = new Stack<Message>();
        private bool fileAttachmentSelected = false;
        private bool emailAttachmentSelected = false;
        private bool showContent = true;

        public ObservableCollection<Folder> RootFolders { get; private set; } = new ObservableCollection<Folder>();
        public Folder SelectedFolder { get { return selectedFolder; } set { selectedFolder = value; OnPropertyChanged("SelectedFolder"); } }
        public Message CurrentMessage { get; private set; } = null;
        public ObservableCollection<Property> CurrentProperties { get; private set; } = null;
        public bool IsMessagePresent { get { return (CurrentMessage != null); } }
        public bool CanPopMessage { get { return (stackMessage.Count > 0); } }
        public bool IsAttachmentPresent { get { return (ShowContent && CurrentMessage != null && CurrentMessage.HasAttachment); } }
        public bool IsFileAttachmentPresent { get { return (ShowContent && CurrentMessage != null && CurrentMessage.HasFileAttachment); } }
        public bool IsFileAttachmentSelected { get { return fileAttachmentSelected; } set { fileAttachmentSelected = value; OnPropertyChanged("IsFileAttachmentSelected"); } }
        public bool IsEmailAttachmentPresent { get { return (ShowContent && CurrentMessage != null && CurrentMessage.HasEmailAttachment); } }
        public bool IsEmailAttachmentSelected { get { return emailAttachmentSelected; } set { emailAttachmentSelected = value; OnPropertyChanged("IsEmailAttachmentSelected"); } }
        public bool ShowContent {
            get { return showContent; }
            set
            {
                showContent = value;
                OnPropertyChanged("ShowContent");
                OnPropertyChanged("ShowProperties");
                OnPropertyChanged("IsAttachmentPresent");
                OnPropertyChanged("IsFileAttachmentPresent");
                OnPropertyChanged("IsFileAttachmentSelected");
                OnPropertyChanged("IsEmailAttachmentPresent");
                OnPropertyChanged("IsEmailAttachmentSelected");
            }
        }
        public bool ShowProperties { get { return !showContent; } }

        public void SelectedRecipientChanged(Recipient recipient)
        {
            if (recipient != null)
            {
                CurrentProperties = recipient.Properties;
                OnPropertyChanged("CurrentProperties");
            }
        }

        public void SelectedAttachmentsChanged(IEnumerable<Attachment> selection)
        {
            IsFileAttachmentSelected = selection.FirstOrDefault(a => a.IsFile) != null;
            IsEmailAttachmentSelected = selection.FirstOrDefault(a => a.IsEmail) != null;

            var firstAttachment = selection.FirstOrDefault(a => (a.IsFile || a.IsEmail));
            if (firstAttachment != null)
            {
                CurrentProperties = firstAttachment.Properties;
                OnPropertyChanged("CurrentProperties");
            }
        }

        public void SetMessage(Message m)
        {
            stackMessage.Clear();
            UpdateCurrentMessage(m);
        }

        public void PushMessage(Message m)
        {
            stackMessage.Push(CurrentMessage);
            UpdateCurrentMessage(m);
        }

        public void PopMessage()
        {
            UpdateCurrentMessage(stackMessage.Pop());
        }

        public void Clear()
        {
            SelectedFolder = null;
            CurrentMessage = null;
            RootFolders.Clear();
            stackMessage.Clear();
        }

        private void UpdateCurrentMessage(Message m)
        {
            CurrentMessage = m;
            CurrentProperties = m != null ? m.Properties : null;
            
            OnPropertyChanged("CurrentMessage");
            OnPropertyChanged("CurrentProperties");
            OnPropertyChanged("IsMessagePresent");
            OnPropertyChanged("CanPopMessage");
            OnPropertyChanged("IsAttachmentPresent");
            OnPropertyChanged("IsFileAttachmentPresent");
            OnPropertyChanged("IsFileAttachmentSelected");
            OnPropertyChanged("IsEmailAttachmentPresent");
            OnPropertyChanged("IsEmailAttachmentSelected");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }

    class Folder
    {
        public string Name { get; set; }
        public uint ContentCount { get; set; } = 0;
        public bool HasSubFolders { get; set; } = false;
        public string Description { get { return String.Format("{0} ({1})", Name, ContentCount); } }
        public NID Nid { get; set; }  // Where folder data is held
        public ObservableCollection<Folder> Folders { get; private set; } = new ObservableCollection<Folder>();
        public ObservableCollection<Message> Messages { get; private set; } = new ObservableCollection<Message>();

        public void AddMessage(Message m)
        {
            m.Folder = this;
            Messages.Add(m);
        }
    }

    class Message 
    {
        public Folder Folder { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string FromTo { get { return Folder.Name.StartsWith("Sent") ? To : From; } }
        public string Subject { get; set; }
        public MessageFlags Flags { get; set; }
        public DateTime? Received { get; set; }
        public DateTime? Submitted { get; set; }
        public DateTime? Modified { get; set; }  // When any attachment was last modified
        public DateTime? Date { get { return Received ?? Submitted; } }
        public string DisplayDate { get { return Date != null ? ((DateTime)Date).ToShortDateString() : "<unknown>"; } }
        public NID Nid { get; set; }
        public BodyType NativeBody { get; set; }
        public string Body { get; set; }
        public string BodyHtml { get; set; }
        public byte[] Html { get; set; }
        public byte[] RtfCompressed { get; set; }
        public ObservableCollection<Attachment> Attachments { get; private set; } = new ObservableCollection<Attachment>();
        public ObservableCollection<Recipient> Recipients { get; private set; } = new ObservableCollection<Recipient>();
        public ObservableCollection<Property> Properties { get; private set; } = new ObservableCollection<Property>();
        public bool MayHaveInlineAttachment { get { return (Attachments.FirstOrDefault(a => a.HasContentId) != null); } }

        // The following properties are used in XAML bindings to control the UI
        public bool HasAttachment { get { return (Flags & MessageFlags.mfHasAttach) == MessageFlags.mfHasAttach; } }
        public bool HasFileAttachment { get { return (Attachments.FirstOrDefault(a => a.IsFile) != null); } }
        public bool HasEmailAttachment { get { return (Attachments.FirstOrDefault(a => a.IsEmail) != null); } }
        public bool ShowText { get { return NativeBody == BodyType.PlainText || (NativeBody == BodyType.Undefined && Body != null && Body.Length > 0); } }
        public bool ShowHtml { get { return NativeBody == BodyType.HTML || (NativeBody == BodyType.Undefined && 
                                            ((BodyHtml != null && BodyHtml.Length > 0) || (Html != null && Html.Length > 0))); } }
        public bool ShowRtf { get { return NativeBody == BodyType.RTF || (NativeBody == BodyType.Undefined && RtfCompressed != null && RtfCompressed.Length > 0); } }

        public string EmbedAttachments(XstFile xst)
        {
            string raw = null;

            if (BodyHtml != null)
                raw = BodyHtml;
            else if (Html != null)
            {
                var e = GetEncoding();
                if (e != null)
                {
                    raw = new String(e.GetChars(Html));
                }
            }
            else if (Body != null)
                raw = Body;

            if (raw == null)
                return null;

            var dict = new Dictionary<string, Attachment>();
            foreach (var a in Attachments.Where(x => x.HasContentId))
                dict.Add(a.ContentId, a);

            return Regex.Replace(raw, @"(="")cid:(.*?)("")", match =>
            {
                Attachment a;
                
                if (dict.TryGetValue(match.Groups[2].Value, out a))
                {
                    // There are limits to what we can push into an inline data image, 
                    // but we don't know exactly what
                    // Todo handle limit when known
                    a.WasRenderedInline = true;
                    var s = new MemoryStream();
                    xst.SaveAttachment(s, a);
                    s.Seek(0, SeekOrigin.Begin);
                    var cooked = match.Groups[1] + @"data:image/jpg;base64," + EscapeString(Convert.ToBase64String(s.ToArray())) + match.Groups[3];
                    return cooked;
                }

                return match.Value;
            }, RegexOptions.Singleline | RegexOptions.IgnoreCase);
        }

        public void SortAndSaveAttachments(List<Attachment> atts = null)
        {
            // If no attachments are supplied, sort the list we already have
            if (atts == null)
                atts = new List<Attachment>(Attachments);

            atts.Sort((a, b) =>
            {
                if (a == null)
                    return -1;
                else if (b == null)
                    return 1;
                else if (a.Hide != b.Hide)
                    return a.Hide ? 1 : -1;
                else
                    return 0;
            });

            Attachments.Clear();
            foreach (var a in atts)
                Attachments.Add(a);
        }

        private string EscapeString(string s)
        {
            var sb = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length;)
            {
                int len = Math.Min(s.Length - i, 32766);
                sb.Append(Uri.EscapeDataString(s.Substring(i, len)));
                i += len;
            }
            return sb.ToString();
        }

        private Encoding GetEncoding()
        {
            var p = Properties.FirstOrDefault(x => x.Guid == "00020386-0000-0000-c000-000000000046" && x.Name == "content-type");
            if (p != null)
            {

                Match m = Regex.Match((string)p.Value, @".*charset=""(.*?)""");
                if (m.Success)
                     return Encoding.GetEncoding(m.Groups[1].Value);
            }

            p = Properties.FirstOrDefault(x => x.Tag == EpropertyTag.PidTagInternetCodepage);
            if (p != null)
            {
                return Encoding.GetEncoding((int)p.Value);
            }

            return null;
        }
    }

    class Recipient
    {
        public RecipientType RecipientType { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public ObservableCollection<Property> Properties { get; private set; } = new ObservableCollection<Property>();
    }

    class Property
    {
 
        public EpropertyTag Tag { get; set; }
        public dynamic Value { get; set; }

        // Standard properties have a Tag value less than 0x8000,
        // and identify a particular property
        //
        // Tag values from 0x8000 to 0x8fff are named properties,
        // where the Tag Is the key into a per .ost or .pst dictionary of properties
        // identified by a GUID (identifying a Property Set) and a name (identifying a property Within that set), 
        // which can be a string or a 32-bit value
        //
        public string Guid { get; set; }        // String representation of hex GUID
        public string GuidName { get; set; }    // Equivalent name, where known e.g. PSETID_Common 
        public UInt32? Lid { get; set; }        // Property identifier, when we know it
        public string Name { get; set; }        // String name of property, when we know it

        public bool IsNamed { get { return (UInt16)Tag >= 0x8000 && (UInt16)Tag <= 0x8fff; } }

        public string DisplayId
        {
            get
            {
                return String.Format("0x{0:x4}", (UInt16)Tag);
            }
        }
        
        public string Description
        {
            get
            {
                string description;
                if (IsNamed)
                {
                    return String.Format("Guid: {0}\r\nName: {1}",
                        GuidName != null ? GuidName : Guid,
                        Name != null ? Name : String.Format("0x{0:x8}", Lid));
                }
                else if (StandardProperties.TagToDescription.TryGetValue(Tag, out description))
                    return description;
                else
                    return null;
            }
        }

        public string DisplayValue
        {
            get
            {
                if (Value is byte[])
                    return BitConverter.ToString(Value);
                else if (Value is Int32[])
                    return String.Join(", ", Value);
                else if (Value is string[])
                    return String.Join(",\r\n", Value);
                else if (Value == null)
                    return null; 
                else
                    return Value.ToString();
            }
        }
    }

    class Attachment
    {
        private ObservableCollection<Property> properties = null;

        public XstFile XstFile { get; set; }
        public Message Parent { get; set; }
        public BTree<Node> subNodeTreeProperties { get; set; } = null; // Used when handling attachments which are themselves messages
        public string DisplayName { get; set; }
        public string FileNameW { get; set; }
        public string LongFileName { get; set; }
        public AttachFlags Flags { get; set; }
        public string MimeTag { get; set; }
        public string ContentId { get; set; }
        public bool Hidden { get; set; }
        public string FileName { get { return LongFileName ?? FileNameW; } }
        public int Size { get; set; }
        public NID Nid { get; set; }  
        public AttachMethods AttachMethod { get; set; }
        public dynamic Content { get; set; }  
        public bool IsFile { get { return AttachMethod == AttachMethods.afByValue; } }
        public bool IsEmail { get { return /*AttachMethod == AttachMethods.afStorage ||*/ AttachMethod == AttachMethods.afEmbeddedMessage; } }
        public bool WasRenderedInline { get; set; } = false;

        public string Type
        {
            get
            {
                if (IsFile)
                    return "File";
                else if (IsEmail)
                    return "Email";
                else
                    return "Other";
            }
        }

        public string Description
        {
            get
            {
                if (IsFile)
                    return FileName;
                else
                    return DisplayName;
            }
        }

        public bool Hide { get { return (Hidden || IsInlineAttachment); } }
        public FontWeight Weight { get { return Hide ? FontWeights.ExtraLight: FontWeights.SemiBold; } }
        public bool HasContentId { get { return (ContentId != null); } }

        // To do: case where ContentLocation property is used instead of ContentId
        public bool IsInlineAttachment
        {
            get
            {
                // It is an in-line attachment either if the flags say it is, or the content ID
                // matched a reference in the body and it was rendered inline
                return ((Flags & AttachFlags.attRenderedInBody) == AttachFlags.attRenderedInBody ||
                        WasRenderedInline) &&
                       HasContentId;
            }
        }

        public ObservableCollection<Property> Properties
        {
            get
            {
                // We read the full set of attachment property values only on demand
                if (properties == null)
                {
                    properties = new ObservableCollection<Property>();
                    foreach (var p in XstFile.ReadAttachmentProperties(this))
                    {
                        properties.Add(p);
                    }
                }
                return properties;
            }
        }
    }
}
