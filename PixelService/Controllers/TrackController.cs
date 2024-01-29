namespace Presentation.Api.Controllers
{
    using Application.Service.Track;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Net;

    [ApiVersion("1.0")]
    [Route("/track")]
    [ApiController]
    [Produces("image/gif")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None)]
    public class TrackController : Controller
    {
        private readonly ITrackService trackService;

        public TrackController(ITrackService trackService)
        {
            this.trackService = trackService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTrackAsync()
        {
            HttpContext.Request.Headers.TryGetValue("Referer", out var refererHeader);
            HttpContext.Request.Headers.TryGetValue("User-Agent", out var userAgentHeader);
            
            var ipAddress = GetIp(HttpContext);

            if (string.IsNullOrEmpty(ipAddress)) 
            {
                return this.BadRequest("Ip Address is a required data");
            }

            return File(
                await this.trackService.GetTrackAsync(
                    refererHeader.ToString() ?? string.Empty,
                    userAgentHeader.ToString() ?? string.Empty,
                    ipAddress), "image/gif");
        }

        private static string GetIp(HttpContext httpContext)
        {
            IPAddress? ip = null;
            var headers = httpContext.Request.Headers.ToList();
            if (headers.Exists((kvp) => kvp.Key == "X-Forwarded-For"))
            {
                var header = headers.First((kvp) => kvp.Key == "X-Forwarded-For").Value.ToString();
                ip = IPAddress.Parse(header.Remove(header.IndexOf(':')));
            }
            else if (httpContext.Connection != null && httpContext.Connection.RemoteIpAddress != null)
            {
                ip = httpContext.Connection.RemoteIpAddress;
            }

            return ip?.ToString() ?? string.Empty;
        }
    }
}
