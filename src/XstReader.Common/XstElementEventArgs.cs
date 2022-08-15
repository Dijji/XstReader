// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

namespace XstReader.App.Common
{
    public class XstElementEventArgs : EventArgs
    {
        public XstElement? Element { get; init; } = null;

        public XstElementEventArgs()
        {
        }

        public XstElementEventArgs(XstElement? element)
        {
            Element = element;
        }
    }
}
