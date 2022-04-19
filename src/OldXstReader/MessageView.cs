// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2016,2019,2020, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using XstReader.ElementProperties;

namespace XstReader
{
    // Provides a view of a Message object for the UI

    public class MessageView : INotifyPropertyChanged
    {
        private bool isSelected = false;

        public MessageView(XstMessage message)
        {
            if (message == null)
                throw new XstException("MessageView requires a Message object");
            Message = message;
            MessageFormatter = new XstMessageFormatter(Message);
        }

        internal XstMessageFormatter MessageFormatter { get; private set; }
        public XstMessage Message { get; private set; }
        public string From => Message.From;
        public string To => Message.To;
        public string Cc => Message.Cc;
        public string FromTo => Message.ParentFolder.DisplayName.StartsWith("Sent") ? To : From;
        public string Subject => Message.Subject ?? Message.DisplayName;
        public DateTime? Received => Message.ReceivedTime;
        public DateTime? Submitted => Message.SubmittedTime;
        public DateTime? Modified => Message.LastModificationTime; // When any attachment was last modified
        public DateTime? Date => Received ?? Submitted;
        public string DisplayDate => Date != null ? ((DateTime)Date).ToString("g") : "<unknown>";
        public XstMessageBody Body => Message.Body;
        public ObservableCollection<XstAttachment> Attachments { get; private set; } = new ObservableCollection<XstAttachment>();
        public IEnumerable<XstRecipient> Recipients => Message.Recipients.Items;
        public IEnumerable<XstProperty> Properties => Message.Properties.Items.NonBinary();
        public bool MayHaveInlineAttachment => Message.Attachments.Inlines().Any();
        public bool IsEncryptedOrSigned => Message.IsEncryptedOrSigned;

        // The following properties are used in XAML bindings to control the UI
        public bool HasAttachment => Message.Attachments.Any();
        public bool HasFileAttachment => Message.Attachments.Files().Any();
        public bool HasVisibleFileAttachment => Message.Attachments.VisibleFiles().Any();
        public bool HasEmailAttachment => Attachments.Any(a => a.IsEmail);
        public bool ShowText => Message.Body.Format == XstMessageBodyFormat.PlainText;
        public bool ShowHtml => Message.Body.Format == XstMessageBodyFormat.Html;
        public bool ShowRtf => Message.Body.Format == XstMessageBodyFormat.Rtf;

        public bool HasToDisplayList => Message.Recipients.To.Any();
        public string ToDisplayList => XstFormatter.Format(Message.Recipients.To);
        public bool HasCcDisplayList => Message.Recipients.Cc.Any();
        public string CcDisplayList => XstFormatter.Format(Message.Recipients.Cc);
        public bool HasBccDisplayList => Message.Recipients.Bcc.Any();
        public string BccDisplayList => XstFormatter.Format(Message.Recipients.Bcc);

        public string ExportFileName => MessageFormatter.ExportFileName;

        public bool IsSelected
        {
            get => isSelected;
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

        public void SortAndSaveAttachments(List<XstAttachment> atts = null)
        {
            // If no attachments are supplied, sort the list we already have
            if (atts == null)
                atts = new List<XstAttachment>(Attachments);

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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

    }

}
