// Copyright (c) 2016,2019,2020, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace XstReader
{
    // Provides a view of a Message object for the UI

    public class MessageView : INotifyPropertyChanged
    {
        private bool isSelected = false;

        public MessageView(Message message)
        {
            if (message == null)
                throw new XstException("MessageView requires a Message object");
            Message = message;
        }

        public Message Message { get; private set; }
        public string From { get { return Message.From; } }
        public string To { get { return Message.To; } }
        public string Cc { get { return Message.Cc; } }
        public string FromTo { get { return Message.Folder.Name.StartsWith("Sent") ? To : From; } }
        public string Subject { get { return Message.Subject; } }
        public DateTime? Received { get { return Message.Received; } }
        public DateTime? Submitted { get { return Message.Submitted; } }
        public DateTime? Modified { get { return Message.Modified; } }  // When any attachment was last modified
        public DateTime? Date { get { return Received ?? Submitted; } }
        public string DisplayDate { get { return Date != null ? ((DateTime)Date).ToString("g") : "<unknown>"; } }
        public BodyType NativeBody { get { return Message.NativeBody; } }
        public string Body { get { return Message.Body; } }
        public string BodyHtml { get { return Message.BodyHtml; } }
        public byte[] Html { get { return Message.Html; } }
        public byte[] RtfCompressed { get { return Message.RtfCompressed; } }
        public ObservableCollection<Attachment> Attachments { get; private set; } = new ObservableCollection<Attachment>();
        public List<Recipient> Recipients { get { return Message.Recipients; } }
        public List<Property> Properties { get { return Message.Properties; } }
        public bool MayHaveInlineAttachment { get { return Message.MayHaveInlineAttachment; } }
        public bool IsEncryptedOrSigned { get { return Message.IsEncryptedOrSigned; } }

        // The following properties are used in XAML bindings to control the UI
        public bool HasAttachment { get { return Message.HasAttachment; } }
        public bool HasFileAttachment { get { return Message.HasFileAttachment; } }
        public bool HasVisibleFileAttachment { get { return (Attachments.FirstOrDefault(a => a.IsFile && !a.Hide) != null); } }
        public bool HasEmailAttachment { get { return (Attachments.FirstOrDefault(a => a.IsEmail) != null); } }
        public bool ShowText { get { return Message.IsBodyText; } }
        public bool ShowHtml { get { return Message.IsBodyHtml; } }
        public bool ShowRtf { get { return Message.IsBodyRtf; } }
        
        public bool HasToDisplayList { get { return ToDisplayList.Length > 0; } }
        public string ToDisplayList { get { return Message.ToDisplayList; } }
        public bool HasCcDisplayList { get { return CcDisplayList.Length > 0; } }
        public string CcDisplayList { get { return Message.CcDisplayList; } }
        public bool HasBccDisplayList { get { return BccDisplayList.Length > 0; } }
        public string BccDisplayList { get { return Message.BccDisplayList; } }
        public string FileAttachmentDisplayList { get { return Message.FileAttachmentDisplayList; } }
        public string ExportFileName { get { return Message.ExportFileName; } }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }


        public void ClearContents()
        {
            Message.ClearContents();
            Attachments.Clear();
        }

        public void ReadSignedOrEncryptedMessage(XstFile xstFile)
        {
            Attachment a = Message.Attachments[0];

            //get attachment bytes
            var ms = new MemoryStream();
            xstFile.SaveAttachment(ms, a);
            byte[] attachmentBytes = ms.ToArray();

            Message.ReadSignedOrEncryptedMessage(attachmentBytes);
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

    }

}
