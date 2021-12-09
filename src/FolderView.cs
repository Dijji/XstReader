// Copyright (c) 2020, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.ObjectModel;

namespace XstReader
{
    public class FolderView
    {
        public Folder Folder { get; private set; }
        public string Name { get { return Folder.Name; } }
        public uint ContentCount { get { return Folder.ContentCount; } }
        public string Description { get { return String.Format("{0} ({1})", Name, ContentCount); } }
        public ObservableCollection<FolderView> FolderViews { get; private set; } = new ObservableCollection<FolderView>();
        public ObservableCollection<MessageView> MessageViews { get; private set; } = new ObservableCollection<MessageView>();

        public FolderView(Folder folder)
        {
            if (folder == null)
                throw new XstException("FolderView requires a Folder object");

            Folder = folder;

            // Recursively add views for any subfolders
            foreach (Folder f in folder.Folders)
                FolderViews.Add(new FolderView(f));
        }

        public void AddMessage(Message m)
        {
            MessageViews.Add(new MessageView(m));
        }

        /// <summary>
        /// Updates the MessageViews List with the information from Core (inside Folder.Messages)
        /// </summary>
        public void UpdateMessageViews()
        {
            MessageViews.Clear();
            foreach (var message in Folder.Messages)
                MessageViews.Add(new MessageView(message));
        }
    }
}