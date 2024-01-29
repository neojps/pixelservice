namespace Data.Gateway.StorageService
{
    public interface IStorageService
    {
        Task StorageEventAsync(string eventInfo);
    }
}
