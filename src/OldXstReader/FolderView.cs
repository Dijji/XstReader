// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

using System;
using System.Collections.ObjectModel;

namespace XstReader
{
    public class FolderView
    {
        public XstFolder Folder { get; private set; }
        public string Name => Folder.DisplayName;
        public int ContentCount => Folder.ContentCount;
        public string Description => $"{Name} ({ContentCount})";
        public ObservableCollection<FolderView> FolderViews { get; private set; } = new ObservableCollection<FolderView>();
        public ObservableCollection<MessageView> MessageViews { get; private set; } = new ObservableCollection<MessageView>();

        public FolderView(XstFolder folder)
        {
            Folder = folder ?? throw new XstException("FolderView requires a Folder object");

            // Recursively add views for any subfolders
            foreach (XstFolder f in folder.Folders)
                FolderViews.Add(new FolderView(f));
        }

        public void AddMessage(XstMessage m)
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