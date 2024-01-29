namespace Data.Gateway.StorageService
{
    using Data.Gateway.Dto;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    public class StorageService : IStorageService
    {
        public const string HttpClientName = nameof(StorageService);
        private IConfiguration Configuration { get; }
        private readonly HttpClient httpClient;

        public StorageService(IConfiguration configuration)
        {
            this.httpClient = new HttpClient();
            this.Configuration = configuration;
        }

        public async Task StorageEventAsync(string eventInfo)
        {
            if (string.IsNullOrWhiteSpace(eventInfo))
            {
                throw new ArgumentNullException(nameof(eventInfo));
            }

            var url = this.Configuration.GetSection("StorageServiceUrl")?.Value;

            this.httpClient.BaseAddress = new Uri(url);
            this.httpClient.DefaultRequestHeaders.Add("cache-control", "no-cache");
            this.httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var request = new AddEventRequest() { EventContent = eventInfo };

            var response = await httpClient.PostAsJsonAsync(url, request);

            response.EnsureSuccessStatusCode();
        }
    }
}