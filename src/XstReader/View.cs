// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace XstReader
{
    // These classes are the view of the xst (.ost and .pst) file rendered by XAML
    // The data layer is effectively provided by the xst file itself

    public class View : INotifyPropertyChanged
    {
        private FolderView selectedFolder = null;
        private MessageView currentMessage = null;
        private bool isBusy = false;
        private Stack<MessageView> stackMessage = new Stack<MessageView>();
        private bool fileAttachmentSelected = false;
        private bool emailAttachmentSelected = false;
        private bool showContent = true;

        public ObservableCollection<FolderView> RootFolderViews { get; private set; } = new ObservableCollection<FolderView>();
        public FolderView SelectedFolder
        {
            get => selectedFolder;
            set
            {
                if (selectedFolder != value)
                {
                    selectedFolder = value;
                    OnPropertyChanged(nameof(SelectedFolder), nameof(CanExportFolder));
                }
            }
        }
        public bool DisplayPrintHeaders { get; set; } = true;//false;
        public bool DisplayEmailType { get; set; } = false;
        public bool DisplayHeaderFields => !DisplayPrintHeaders;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged(nameof(IsBusy), nameof(IsNotBusy), nameof(CanExportFolder));
                }
            }
        }
        public bool IsNotBusy => !isBusy;
        public ObservableCollection<XstProperty> CurrentProperties { get; private set; } = new ObservableCollection<XstProperty>();
        public bool IsMessagePresent => (CurrentMessage != null);
        public bool CanSaveEmail => ShowContent && CurrentMessage != null;
        public bool CanPopMessage => stackMessage.Count > 0;
        public bool CanExportFolder => !IsBusy && SelectedFolder != null;
        public bool CanExportProperties => IsMessagePresent && ShowProperties;
        public bool IsAttachmentPresent => ShowContent && CurrentMessage != null && CurrentMessage.HasAttachment;
        public bool IsFileAttachmentPresent => ShowContent && CurrentMessage != null && CurrentMessage.HasFileAttachment;
        public bool IsFileAttachmentSelected
        {
            get => fileAttachmentSelected;
            set
            {
                if (fileAttachmentSelected != value)
                {
                    fileAttachmentSelected = value;
                    OnPropertyChanged(nameof(IsFileAttachmentSelected));
                }
            }
        }
        public bool IsEmailAttachmentPresent => ShowContent && CurrentMessage != null && CurrentMessage.HasEmailAttachment;
        public bool IsEmailAttachmentSelected
        {
            get => emailAttachmentSelected;
            set
            {
                if (emailAttachmentSelected != value)
                {
                    emailAttachmentSelected = value;
                    OnPropertyChanged(nameof(IsEmailAttachmentSelected));
                }
            }
        }
        public MessageView CurrentMessage
        {
            get { return currentMessage; }
            private set
            {
                currentMessage = value;
                OnPropertyChanged(
                    nameof(IsMessagePresent),
                    nameof(CanSaveEmail),
                    nameof(IsAttachmentPresent),
                    nameof(IsFileAttachmentPresent),
                    nameof(IsEmailAttachmentPresent)
                    );
            }
        }

        public bool ShowProperties { get { return !showContent; } }
        public bool ShowContent
        {
            get { return showContent; }
            set
            {
                showContent = value;
                OnPropertyChanged(
                    nameof(ShowContent),
                    nameof(ShowProperties),
                    nameof(CanSaveEmail),
                    nameof(CanExportProperties),
                    nameof(IsAttachmentPresent),
                    nameof(IsFileAttachmentPresent),
                    nameof(IsFileAttachmentSelected),
                    nameof(IsEmailAttachmentPresent),
                    nameof(IsEmailAttachmentSelected)
                    );
            }
        }

        public void UpdateFolderViews(XstFolder rootFolder)
        {
            RootFolderViews.Clear();
            foreach (var f in rootFolder.Folders)
            {
                RootFolderViews.Add(new FolderView(f));
            }
        }

        public void SelectedRecipientChanged(XstRecipient recipient)
        {
            if (recipient != null)
            {
                CurrentProperties.PopulateWith(recipient.Properties);
                OnPropertyChanged(nameof(CurrentProperties));
            }
        }

        public void SelectedAttachmentsChanged(IEnumerable<XstAttachment> selection)
        {
            IsFileAttachmentSelected = selection.FirstOrDefault(a => a.IsFile) != null;
            IsEmailAttachmentSelected = selection.FirstOrDefault(a => a.IsEmail) != null;

            var firstAttachment = selection.FirstOrDefault(a => (a.IsFile || a.IsEmail));
            if (firstAttachment != null)
            {
                CurrentProperties.PopulateWith(firstAttachment.Properties);
                OnPropertyChanged(nameof(CurrentProperties));
            }
        }

        public void SetMessage(MessageView mv)
        {
            if (CurrentMessage != null)
                CurrentMessage.ClearContents();
            stackMessage.Clear();
            UpdateCurrentMessage(mv);
        }

        public void PushMessage(MessageView mv)
        {
            stackMessage.Push(CurrentMessage);
            UpdateCurrentMessage(mv);
        }

        public void PopMessage()
        {
            UpdateCurrentMessage(stackMessage.Pop());
        }

        public void Clear()
        {
            SelectedFolder?.Folder?.ClearContents();
            SelectedFolder = null;
            CurrentMessage?.Message?.ClearContents();
            CurrentMessage = null;
            RootFolderViews.Clear();
            stackMessage.Clear();
        }

        private void UpdateCurrentMessage(MessageView mv)
        {
            CurrentMessage = mv;
            if (mv != null)
                CurrentProperties.PopulateWith(mv.Properties);
            else
                CurrentProperties.Clear();

            OnPropertyChanged(
                nameof(CurrentMessage),
                nameof(CurrentProperties),
                nameof(IsMessagePresent),
                nameof(CanExportProperties),
                nameof(CanPopMessage),
                nameof(IsAttachmentPresent),
                nameof(IsFileAttachmentPresent),
                nameof(IsFileAttachmentSelected),
                nameof(IsEmailAttachmentPresent),
                nameof(IsEmailAttachmentSelected)
                );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(params string[] infos)
        {
            if (PropertyChanged != null)
            {
                foreach (string info in infos)
                    PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }


}
