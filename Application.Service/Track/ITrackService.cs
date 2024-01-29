namespace Application.Service.Track
{
    public interface ITrackService
    {
        Task<byte[]> GetTrackAsync(string referrer, string userAgent, string ipAddress);
    }
}
