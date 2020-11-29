// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace XstReader
{
    // These classes are the view of the xst (.ost and .pst) file rendered by XAML
    // The data layer is effectively provided by the xst file itself

    class View : INotifyPropertyChanged
    {
        private Folder selectedFolder = null;
        private Message currentMessage = null;
        private bool isBusy = false;
        private Stack<Message> stackMessage = new Stack<Message>();
        private bool fileAttachmentSelected = false;
        private bool emailAttachmentSelected = false;
        private bool showContent = true;

        public ObservableCollection<Folder> RootFolders { get; private set; } = new ObservableCollection<Folder>();
        public Folder SelectedFolder {
            get { return selectedFolder; }
            set { selectedFolder = value; OnPropertyChanged("SelectedFolder"); OnPropertyChanged("CanExportFolder"); } }
        public bool DisplayPrintHeaders { get; set; } = false;
        public bool DisplayEmailType { get; set; } = false;
        public bool DisplayHeaderFields { get { return !DisplayPrintHeaders; } }
        public bool IsBusy { get { return isBusy; } set { isBusy = value; OnPropertyChanged(nameof(IsBusy)); OnPropertyChanged(nameof(IsNotBusy)); OnPropertyChanged("CanExportFolder"); } }
        public bool IsNotBusy { get { return !isBusy; } }
        public ObservableCollection<Property> CurrentProperties { get; private set; } = new ObservableCollection<Property>();
        public bool IsMessagePresent { get { return (CurrentMessage != null); } }
        public bool CanSaveEmail { get { return ShowContent && CurrentMessage != null; } }
        public bool CanPopMessage { get { return (stackMessage.Count > 0); } }
        public bool CanExportFolder { get { return !IsBusy && SelectedFolder != null; } }
        public bool CanExportProperties { get { return IsMessagePresent && ShowProperties; } }
        public bool IsAttachmentPresent { get { return (ShowContent && CurrentMessage != null && CurrentMessage.HasAttachment); } }
        public bool IsFileAttachmentPresent { get { return (ShowContent && CurrentMessage != null && CurrentMessage.HasFileAttachment); } }
        public bool IsFileAttachmentSelected { get { return fileAttachmentSelected; } set { fileAttachmentSelected = value; OnPropertyChanged("IsFileAttachmentSelected"); } }
        public bool IsEmailAttachmentPresent { get { return (ShowContent && CurrentMessage != null && CurrentMessage.HasEmailAttachment); } }
        public bool IsEmailAttachmentSelected { get { return emailAttachmentSelected; } set { emailAttachmentSelected = value; OnPropertyChanged("IsEmailAttachmentSelected"); } }

        public Message CurrentMessage
        {
            get { return currentMessage; }
            private set
            {
                currentMessage = value;
                OnPropertyChanged(nameof(IsMessagePresent));
                OnPropertyChanged(nameof(CanSaveEmail));
                OnPropertyChanged(nameof(IsAttachmentPresent));
                OnPropertyChanged(nameof(IsFileAttachmentPresent));
                OnPropertyChanged(nameof(IsEmailAttachmentPresent));
            }
        }

        public bool ShowProperties { get { return !showContent; } }
        public bool ShowContent {
            get { return showContent; }
            set
            {
                showContent = value;
                OnPropertyChanged("ShowContent");
                OnPropertyChanged("ShowProperties");
                OnPropertyChanged("CanSaveEmail");
                OnPropertyChanged("CanExportProperties");
                OnPropertyChanged("IsAttachmentPresent");
                OnPropertyChanged("IsFileAttachmentPresent");
                OnPropertyChanged("IsFileAttachmentSelected");
                OnPropertyChanged("IsEmailAttachmentPresent");
                OnPropertyChanged("IsEmailAttachmentSelected");
            }
        }

        public void SelectedRecipientChanged(Recipient recipient)
        {
            if (recipient != null)
            {
                CurrentProperties.PopulateWith(recipient.Properties);
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
                CurrentProperties.PopulateWith(firstAttachment.Properties);
                OnPropertyChanged("CurrentProperties");
            }
        }

        public void SetMessage(Message m)
        {
            if (CurrentMessage != null)
                CurrentMessage.ClearContents();
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
            if (m != null)
                CurrentProperties.PopulateWith(m.Properties);
            else
                CurrentProperties.Clear();

            OnPropertyChanged("CurrentMessage");
            OnPropertyChanged("CurrentProperties");
            OnPropertyChanged("IsMessagePresent");
            OnPropertyChanged("CanExportProperties");
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

    class Recipient
    {
        public RecipientType RecipientType { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public List<Property> Properties { get; private set; } = new List<Property>();
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

        public string CsvId
        {
            get
            {
                if (IsNamed)
                    // Prefix with 80 In order to ensure they collate last
                    return String.Format("80{0}{1:x8}", Guid, Lid);
                else
                    return String.Format("{0:x4}", (UInt16)Tag);
            }
        }


        public string CsvDescription
        {
            get
            {
                string description;
                if (IsNamed)
                {
                    return String.Format("{0}: {1}",
                        GuidName != null ? GuidName : Guid,
                        Name != null ? Name : String.Format("0x{0:x8}", Lid));
                }
                else if (StandardProperties.TagToDescription.TryGetValue(Tag, out description))
                {
                    if (description.StartsWith("undocumented"))
                        return "undocumented " + DisplayId;
                    else
                        return description;
                }
                else
                    return DisplayId;
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
                else if (Value is List<byte[]>)
                    return String.Join(",\r\n", ((List<byte[]>)Value).Select(v => BitConverter.ToString(v)));
                else if (Value == null)
                    return null; 
                else
                    return Value.ToString();
            }
        }
    }

    class Attachment
    {
        private List<Property> properties = null;

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
        public bool WasLoadedFromMime { get; set; } = false;

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
        public bool HasContentId { get { return (ContentId != null && ContentId.Length > 0); } }

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

        public List<Property> Properties
        {
            get
            {
                // We read the full set of attachment property values only on demand
                if (properties == null)
                { 
                    properties = new List<Property>();
                    if (!WasLoadedFromMime)
                    {
                        foreach (var p in XstFile.ReadAttachmentProperties(this))
                        {
                            properties.Add(p);
                        }
                    }
                }
                return properties;
            }
        }
        
        public Attachment()
        {

        }

        public Attachment(string fileName, byte[] content)
        {
            LongFileName = fileName;
            AttachMethod = AttachMethods.afByValue;
            Size = content.Length;
            this.Content = content;
            WasLoadedFromMime = true;
        }

        public Attachment(string fileName, string contentId, Byte[] content)
            : this(fileName, content)
        {
            ContentId = contentId;
            Flags = AttachFlags.attRenderedInBody;
        }

    }
}
