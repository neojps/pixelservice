using Application.Service.Track;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Api.Controllers;
using Xunit;
using Microsoft.AspNetCore.Http;

namespace Unit.Tests.Presentation.Api
{
    public class TrackControllerTests
    {
        private readonly TrackController trackController;
        private readonly Mock<ITrackService> trackServiceMock;
        private readonly Mock<HttpContext> httpContext;

        public TrackControllerTests()
        {
            httpContext = new Mock<HttpContext>();
            httpContext.Setup(httpc => httpc.Request.Headers.Referer).Returns("referer");
            httpContext.Setup(httpc => httpc.Request.Headers.UserAgent).Returns("user-agent");

            this.trackServiceMock = new Mock<ITrackService>();
            this.trackServiceMock.Setup(ie => ie.GetTrackAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.Run(() => Array.Empty<byte>()));

            this.trackController = new TrackController(this.trackServiceMock.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
                {
                    HttpContext = httpContext.Object
                }
            };
        }

        [Fact]
        public async Task PostEventAsync_Ok()
        {
            httpContext.Setup(httpc => httpc.Connection.RemoteIpAddress).Returns(System.Net.IPAddress.Parse("198.127.5.1"));

            var response = await this.trackController.GetTrackAsync();

            Assert.NotNull(response as FileContentResult);
        }

        [Fact]
        public async Task PostEventAsync_NotOk()
        {
            var response = await trackController.GetTrackAsync();

            Assert.NotNull(response as BadRequestObjectResult);
        }
    }
}
