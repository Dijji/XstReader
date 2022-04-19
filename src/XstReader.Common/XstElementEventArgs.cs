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
