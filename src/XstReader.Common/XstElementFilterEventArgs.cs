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
