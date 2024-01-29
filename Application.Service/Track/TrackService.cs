namespace Application.Service
{
    using Application.Service.Track;
    using Data.Gateway.StorageService;

    public class TrackService : ITrackService
    {
        private readonly IStorageService storageService;

        public TrackService(IStorageService storageService)
        {
            this.storageService = storageService;
        }

        public async Task<byte[]> GetTrackAsync(string referrer, string userAgent, string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                throw new ArgumentNullException(nameof(ipAddress));
            }

            var referrerToUse = string.IsNullOrEmpty(referrer) ? "null" : referrer;
            var userAgentToUse = string.IsNullOrEmpty(userAgent) ? "null" : userAgent;

            var content = $"{DateTime.UtcNow:o}|{referrerToUse}|{userAgentToUse}|{ipAddress}";

            await this.storageService.StorageEventAsync(content);

            var cleanContent = GetStringFromBase64String(content);

            return System.Convert.FromBase64String(cleanContent);
        }

        static string GetStringFromBase64String(string toEncode)
        {
            var toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);

            var returnValue = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }
    }
}