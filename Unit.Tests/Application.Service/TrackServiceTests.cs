using Application.Service;
using Application.Service.Track;
using Data.Gateway.StorageService;
using Moq;
using Xunit;

namespace Unit.Tests.Application.Service
{
    public class TrackServiceTests
    {
        private readonly ITrackService trackService;
        private readonly Mock<IStorageService> mockStorageService;

        public TrackServiceTests()
        {
            this.mockStorageService = new Mock<IStorageService>();
            this.mockStorageService.Setup(ic => ic.StorageEventAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            this.trackService = new TrackService(this.mockStorageService.Object);
        }

        [Fact]
        public async Task GetTrackAsync_Ok()
        {
            var referer = "referer";
            var userAgent = "userAgent";
            var ipAddress = "198.0.0.1";

            var result = Record.ExceptionAsync(async () => await trackService.GetTrackAsync(referer, userAgent, ipAddress));

            Assert.NotNull(result);
            Assert.Null(result.Result);
        }

        [Fact]
        public async Task GetTrackAsync_Fail()
        {
            var referer = "referer";
            var userAgent = "userAgent";
            var ipAddress = string.Empty;

            var result = Record.ExceptionAsync(async () => await trackService.GetTrackAsync(referer, userAgent, ipAddress));

            Assert.NotNull(result);
            Assert.NotNull(result.Result as ArgumentNullException);
        }
    }
}
