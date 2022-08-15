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
    public class XstElementFilterEventArgs<T> : EventArgs
        where T : XstElement
    {
        public Func<XstMessage, bool>? Filter { get; init; } = null;

        public XstElementFilterEventArgs()
        {
        }

        public XstElementFilterEventArgs(Func<XstMessage, bool>? filter)
        {
            Filter = filter;
        }

    }
}
