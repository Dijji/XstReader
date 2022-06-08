using XstReader;

namespace XstReader.App.Common
{
    public interface IXstDataSourcedControl<T>
    {
        public T? DataSource
        {
            get => GetDataSource();
            set => SetDataSource(value);
        }

        public T? GetDataSource();
        public void SetDataSource(T? dataSource);

        public void ClearContents();

    }
}
