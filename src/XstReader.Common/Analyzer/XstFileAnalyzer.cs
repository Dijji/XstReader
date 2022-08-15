// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

using System.ComponentModel;

namespace XstReader.App.Common.Analyzer
{
    internal class XstFileAnalyzer
    {
        public event ProgressChangedEventHandler ProgressChanged;
        private void RaiseProgressChanged(long current, long max)
        {
            if (max < current) max = current;
            if (max == 0) max = 1;
            int percentage = (int)Math.Round(current * 100f / max);

            ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(percentage, null));
        }

        public XstFile XstFile { get; private set; }

        public XstFileAnalyzer()
        {

        }

        public XstFileAnalyzer(XstFile xstFile)
        {
            XstFile = xstFile;
        }

        private long _Max = 0;
        private long _Current = 0;

        public void Analyze(XstFile xstFile)
        {
            XstFile = xstFile;
            Analyze();
        }

        private void IncMax(long incMax = 1)
        {
            _Max += incMax;
            RaiseProgressChanged(_Current, _Max);
        }
        private void IncCurrent(long incCurrent = 1)
        {
            _Current += incCurrent;
            RaiseProgressChanged(_Current, _Max);
        }
        public void Analyze()
        {
            if (XstFile == null) return;

            _Current = 0;
            _Max = XstFile.RootFolder.CountTreeSubfolders() + 1;
            AnalyzeFolder(XstFile.RootFolder);
            IncCurrent();
        }

        public Dictionary<string, IEnumerable<XstProperty>> FolderProperties { get; private set; }
            = new Dictionary<string, IEnumerable<XstProperty>>();
        public Dictionary<string, IEnumerable<XstProperty>> MessageProperties { get; private set; }
            = new Dictionary<string, IEnumerable<XstProperty>>();
        public Dictionary<(DateTime, string), IEnumerable<XstProperty>> RecipientProperties { get; private set; }
            = new Dictionary<(DateTime, string), IEnumerable<XstProperty>>();

        private void AnalyzeFolder(XstFolder folder)
        {
            FolderProperties[folder.GetId()] = folder.Properties.Items.NonBinary();

            foreach (var message in folder.Messages)
                AnalyzeMessage(message);

            foreach (var childFolder in folder.Folders)
                AnalyzeFolder(childFolder);

            IncCurrent();
        }

        private void AnalyzeMessage(XstMessage message)
        {
            MessageProperties[message.GetId()] = message.Properties.Items.NonBinary();

            foreach (var recipient in message.Recipients.Items)
                AnalyzeRecipient(recipient);

            foreach (var attachment in message.Attachments)
                AnalyzeAttchment(attachment);
        }

        private void AnalyzeRecipient(XstRecipient recipient)
        {
            RecipientProperties[(recipient.GetDate(), recipient.GetId())] = recipient.Properties.Items.NonBinary();
        }

        private void AnalyzeAttchment(XstAttachment attachment)
        {

        }
    }
}
