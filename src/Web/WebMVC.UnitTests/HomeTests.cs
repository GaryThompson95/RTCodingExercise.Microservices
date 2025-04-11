using Catalog.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using RTCodingExercise.Microservices.Controllers;
using System.Net.Http;
using System.Threading.Tasks;
using WebMVC.Services;
using WebMVC.UnitTests.Fakes;
using Xunit;

namespace WebMVC.UnitTests
{
    public class HomeTests
    {
        [Fact]
        public async Task CreatePlate_ValidPlate_ShouldNotError()
        {
            //Arrange
            var httpFactoryMock = new Mock<IHttpClientFactory>();
            httpFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient());
            var plateServiceMock = new Mock<PlateService>(httpFactoryMock.Object);
            plateServiceMock.Setup(x => x.CreatePlateAsync(It.IsAny<Plate>()))
                .Returns(Task.CompletedTask);
            var fakeLogger = new FakeLogger();
            var controller = new HomeController(fakeLogger, plateServiceMock.Object);
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            controller.TempData = tempData;
            var plate = new Plate
            {
                Registration = "ABC1234",
                PurchasePrice = 100
            };

            //Act
            await controller.Create(plate);

            //Assert
            Assert.True(tempData["Message"] == "Plate saved successfully!");
            Assert.True(fakeLogger.LastLog == string.Empty);
        }

        [Fact]
        public async Task CreatePlate_EmptyReg_ShouldError()
        {
            //Arrange
            var httpFactoryMock = new Mock<IHttpClientFactory>();
            httpFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient());
            var plateServiceMock = new Mock<PlateService>(httpFactoryMock.Object);
            plateServiceMock.Setup(x => x.CreatePlateAsync(It.IsAny<Plate>()))
                .Returns(Task.CompletedTask);
            var fakeLogger = new FakeLogger();
            var controller = new HomeController(fakeLogger, plateServiceMock.Object);
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            controller.TempData = tempData;
            var plate = new Plate
            {
                Registration = "",
                PurchasePrice = 100
            };

            //Act
            await controller.Create(plate);

            //Assert
            Assert.True(tempData["Message"] == "Add Plate Failed - The registration must be between 1 and 7 characters");
            Assert.True(fakeLogger.LastLog == "Add Plate Failed - The registration must be between 1 and 7 characters");
        }

        [Fact]
        public async Task CreatePlate_TooLongReg_ShouldError()
        {
            //Arrange
            var httpFactoryMock = new Mock<IHttpClientFactory>();
            httpFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient());
            var plateServiceMock = new Mock<PlateService>(httpFactoryMock.Object);
            plateServiceMock.Setup(x => x.CreatePlateAsync(It.IsAny<Plate>()))
                .Returns(Task.CompletedTask);
            var fakeLogger = new FakeLogger();
            var controller = new HomeController(fakeLogger, plateServiceMock.Object);
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            controller.TempData = tempData;
            var plate = new Plate
            {
                Registration = "11AAAAAAA",
                PurchasePrice = 100
            };

            //Act
            await controller.Create(plate);

            //Assert
            Assert.True(tempData["Message"] == "Add Plate Failed - The registration must be between 1 and 7 characters");
            Assert.True(fakeLogger.LastLog == "Add Plate Failed - The registration must be between 1 and 7 characters");
        }

        [Fact]
        public async Task CreatePlate_RegHasSymbol_ShouldError()
        {
            //Arrange
            var httpFactoryMock = new Mock<IHttpClientFactory>();
            httpFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient());
            var plateServiceMock = new Mock<PlateService>(httpFactoryMock.Object);
            plateServiceMock.Setup(x => x.CreatePlateAsync(It.IsAny<Plate>()))
                .Returns(Task.CompletedTask);
            var fakeLogger = new FakeLogger();
            var controller = new HomeController(fakeLogger, plateServiceMock.Object);
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            controller.TempData = tempData;
            var plate = new Plate
            {
                Registration = "1A?AAA",
                PurchasePrice = 100
            };

            //Act
            await controller.Create(plate);

            //Assert
            Assert.True(tempData["Message"] == "Add Plate Failed - The registration can only contain letters and digits");
            Assert.True(fakeLogger.LastLog == "Add Plate Failed - The registration can only contain letters and digits");
        }

        [Fact]
        public async Task CreatePlate_RegHasNoNumbers_ShouldError()
        {
            //Arrange
            var httpFactoryMock = new Mock<IHttpClientFactory>();
            httpFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient());
            var plateServiceMock = new Mock<PlateService>(httpFactoryMock.Object);
            plateServiceMock.Setup(x => x.CreatePlateAsync(It.IsAny<Plate>()))
                .Returns(Task.CompletedTask);
            var fakeLogger = new FakeLogger();
            var controller = new HomeController(fakeLogger, plateServiceMock.Object);
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            controller.TempData = tempData;
            var plate = new Plate
            {
                Registration = "AAAA",
                PurchasePrice = 100
            };

            //Act
            await controller.Create(plate);

            //Assert
            Assert.True(tempData["Message"] == "Add Plate Failed - The registration must contain at least one digit");
            Assert.True(fakeLogger.LastLog == "Add Plate Failed - The registration must contain at least one digit");
        }
    }
}