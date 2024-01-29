namespace Presentation.Api
{
    using Application.Service;
    using Application.Service.Track;
    using Data.Gateway.StorageService;

    internal static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMvc();
            builder.Services.AddCors();

            builder.Services.AddScoped<ITrackService, TrackService>();
            builder.Services.AddTransient<IStorageService, StorageService>();

            var url = Environment.GetEnvironmentVariable("StorageServiceUrl");

            if (url != null)
            {
                builder.Services.AddHttpClient(nameof(StorageService), c =>
                {
                    c.BaseAddress = new Uri(url);
                    c.DefaultRequestHeaders.Add("cache-control", "no-cache");
                    c.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                });
            }
        }
    }
}
