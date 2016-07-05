// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace XstReader
{
    // These classes are the view of the xst (.ost and .pst) file rendered by XAML
    // The data layer is effectively provided by the xst file itself

    class View : INotifyPropertyChanged
    {
        private Folder selectedFolder = null;
        private Message selectedMessage = null;
        private bool attachmentSelected = false;

        public ObservableCollection<Folder> RootFolders { get; private set; } = new ObservableCollection<Folder>();
        public Folder SelectedFolder { get { return selectedFolder; } set { selectedFolder = value; OnPropertyChanged("SelectedFolder"); } }
        public Message SelectedMessage { get { return selectedMessage; } set { selectedMessage = value; OnPropertyChanged("SelectedMessage"); OnPropertyChanged("AttachmentPresent"); } }
        public bool AttachmentPresent { get { return (SelectedMessage != null && SelectedMessage.HasAttachment); } }
        public bool AttachmentSelected { get { return attachmentSelected; } set { attachmentSelected = value; OnPropertyChanged("AttachmentSelected"); } }

        public void  Clear()
        {
            SelectedFolder = null;
            SelectedMessage = null;
            RootFolders.Clear();
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

    public class Folder
    {
        public string Name { get; set; }
        public uint ContentCount { get; set; } = 0;
        public bool HasSubFolders { get; set; } = false;
        public string Description { get { return String.Format("{0} ({1})", Name, ContentCount); } }
        public NID Nid { get; set; }  // Where folder data is held
        public ObservableCollection<Folder> Folders { get; private set; } = new ObservableCollection<Folder>();
        public ObservableCollection<Message> Messages { get; private set; } = new ObservableCollection<Message>();
    }

    public class Message 
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
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
    
        // The following properties are used in XAML bindings to control the UI
        public bool HasAttachment { get { return (Flags & MessageFlags.mfHasAttach) == MessageFlags.mfHasAttach; } }
        public bool ShowText { get { return NativeBody == BodyType.PlainText || (NativeBody == BodyType.Undefined && Body != null && Body.Length > 0); } }
        public bool ShowHtml { get { return NativeBody == BodyType.HTML || (NativeBody == BodyType.Undefined && 
                                            ((BodyHtml != null && BodyHtml.Length > 0) || (Html != null && Html.Length > 0))); } }
        public bool ShowRtf { get { return NativeBody == BodyType.RTF || (NativeBody == BodyType.Undefined && RtfCompressed != null && RtfCompressed.Length > 0); } }
    }

    public class Attachment
    {
        public Message Parent { get; set; }
        public string FileNameW { get; set; }
        public string LongFileName { get; set; }
        public string FileName { get { return LongFileName ?? FileNameW; } }
        public int Size { get; set; }
        public NID Nid { get; set; }  
        public AttachMethods AttachMethod { get; set; }
        public dynamic Content { get; set; }  
    }
}
