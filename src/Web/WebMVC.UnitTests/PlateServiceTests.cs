using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebMVC.Interfaces;
using WebMVC.Models;
using WebMVC.Services;
using Xunit;

namespace WebMVC.UnitTests
{
    public class PlateServiceTests
    {
        [Fact]
        public async Task GetPlatesAsync_HittingEndpoint_ShouldReturnPlate()
        {
            // Arrange
            var httpFactoryMock = new Mock<IHttpClientFactory>();
            var mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
            mockHttpClientWrapper.Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[{\"Registration\":\"ABC1234\",\"PurchasePrice\":100}]")
                });
            var plateService = new PlateService(mockHttpClientWrapper.Object);
            // Act
            var result = await plateService.GetPlatesAsync(1);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<HomeViewModel>(result);
            Assert.True(result.Plates.Count() == 1);
            Assert.False(result.HasNext);
            Assert.True(result.CurrentPage == 1);
        }

        [Fact]
        public async Task GetPlatesAsync_ReturnFullPage_ShouldHaveNextButReturn20()
        {
            // Arrange
            var plates = GeneratePlates(21);
            var httpFactoryMock = new Mock<IHttpClientFactory>();
            var mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
            mockHttpClientWrapper.Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(plates))
            });
            var plateService = new PlateService(mockHttpClientWrapper.Object);
            // Act
            var result = await plateService.GetPlatesAsync(1);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<HomeViewModel>(result);
            Assert.True(result.Plates.Count() == 20);
            Assert.True(result.HasNext);
            Assert.True(result.CurrentPage == 1);
        }

        [Fact]
        public async Task GetPlatesAsync_GetSecondPage_ShouldUpdateCurrentPage()
        {
            // Arrange
            var plates = GeneratePlates(21);
            var httpFactoryMock = new Mock<IHttpClientFactory>();
            var mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
            mockHttpClientWrapper.Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(plates))
            });
            var plateService = new PlateService(mockHttpClientWrapper.Object);
            // Act
            var result = await plateService.GetPlatesAsync(2);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<HomeViewModel>(result);
            Assert.True(result.Plates.Count() == 20);
            Assert.True(result.HasNext);
            Assert.True(result.CurrentPage == 2);
        }

        private List<GetPlateObject> GeneratePlates(int count)
        {
            var plates = new List<GetPlateObject>();
            for (int i = 0; i < count; i++)
            {
                plates.Add(new GetPlateObject
                {
                    Registration = $"ABC1{i}",
                    PurchasePrice = 100
                });
            }
            return plates;
        }
    }

    internal class GetPlateObject
    {
        public string Registration { get; set; }
        public decimal PurchasePrice { get; set; }
    }
}
