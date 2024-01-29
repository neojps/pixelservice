using Data.Gateway.StorageService;
using Moq;
using Xunit;
using Microsoft.Extensions.Configuration;

namespace Unit.Tests.Data.Gateway
{
    public class StorageServiceTests
    {
        private readonly IStorageService storageService;
        private readonly Mock<IConfiguration> mockConfiguration;

        public StorageServiceTests()
        {
            this.mockConfiguration = new Mock<IConfiguration>();

            this.storageService = new StorageService(this.mockConfiguration.Object);
        }

        [Fact]
        public async Task StorageEventAsync_Fail_1()
        {
            var result = Record.ExceptionAsync(async () => await storageService.StorageEventAsync(string.Empty));

            Assert.NotNull(result);
            Assert.NotNull(result.Result as ArgumentNullException);
        }

        [Fact]
        public async Task StorageEventAsync_Fail_2()
        {
            var eventInfo = "eventInfo";

            var result = Record.ExceptionAsync(async () => await storageService.StorageEventAsync(eventInfo));

            Assert.NotNull(result);
            Assert.NotNull(result.Result as ArgumentNullException);
        }
    }
}
