namespace XstReader.App.Common
{
    public interface IXstElementSelectable<T>
        where T : XstElement
    {
        public event EventHandler<XstElementEventArgs>? SelectedItemChanged;
        public T? SelectedItem
        {
            get => GetSelectedItem();
            set => SetSelectedItem(value);
        }

        public T? GetSelectedItem();
        public void SetSelectedItem(T? item);
    }
}
