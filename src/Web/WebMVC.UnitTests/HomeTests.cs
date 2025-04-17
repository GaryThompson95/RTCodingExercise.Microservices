using Catalog.Domain;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using RTCodingExercise.Microservices.Controllers;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebMVC.Interfaces;
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
            var mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
            var reserveRequestClientMock = new Mock<IRequestClient<ReservePlateMessage>>();
            reserveRequestClientMock.Setup(x => x.GetResponse<ConsumerResponse>(It.IsAny<ReservePlateMessage>(), It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
                                                .Returns(Task.FromResult((Response<ConsumerResponse>)new ResponseFake(new ConsumerResponse() { Success = true })));

            var buyRequestClientMock = new Mock<IRequestClient<BuyPlateMessage>>();
            buyRequestClientMock.Setup(x => x.GetResponse<ConsumerResponse>(It.IsAny<BuyPlateMessage>(), It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
                                                .Returns(Task.FromResult((Response<ConsumerResponse>)new ResponseFake(new ConsumerResponse() { Success = true })));

            var plateServiceMock = new Mock<PlateService>(mockHttpClientWrapper.Object, reserveRequestClientMock.Object, buyRequestClientMock.Object);
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
            var mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
            var reserveRequestClientMock = new Mock<IRequestClient<ReservePlateMessage>>();
            reserveRequestClientMock.Setup(x => x.GetResponse<ConsumerResponse>(It.IsAny<ReservePlateMessage>(), It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
                                                .Returns(Task.FromResult((Response<ConsumerResponse>)new ResponseFake(new ConsumerResponse() { Success = true })));

            var buyRequestClientMock = new Mock<IRequestClient<BuyPlateMessage>>();
            buyRequestClientMock.Setup(x => x.GetResponse<ConsumerResponse>(It.IsAny<BuyPlateMessage>(), It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
                                                .Returns(Task.FromResult((Response<ConsumerResponse>)new ResponseFake(new ConsumerResponse() { Success = true })));

            var plateServiceMock = new Mock<PlateService>(mockHttpClientWrapper.Object, reserveRequestClientMock.Object, buyRequestClientMock.Object);
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
            var mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
            var reserveRequestClientMock = new Mock<IRequestClient<ReservePlateMessage>>();
            reserveRequestClientMock.Setup(x => x.GetResponse<ConsumerResponse>(It.IsAny<ReservePlateMessage>(), It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
                                                .Returns(Task.FromResult((Response<ConsumerResponse>)new ResponseFake(new ConsumerResponse() { Success = true })));

            var buyRequestClientMock = new Mock<IRequestClient<BuyPlateMessage>>();
            buyRequestClientMock.Setup(x => x.GetResponse<ConsumerResponse>(It.IsAny<BuyPlateMessage>(), It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
                                                .Returns(Task.FromResult((Response<ConsumerResponse>)new ResponseFake(new ConsumerResponse() { Success = true })));

            var plateServiceMock = new Mock<PlateService>(mockHttpClientWrapper.Object, reserveRequestClientMock.Object, buyRequestClientMock.Object);
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
            var mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
            var reserveRequestClientMock = new Mock<IRequestClient<ReservePlateMessage>>();
            reserveRequestClientMock.Setup(x => x.GetResponse<ConsumerResponse>(It.IsAny<ReservePlateMessage>(), It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
                                                .Returns(Task.FromResult((Response<ConsumerResponse>)new ResponseFake(new ConsumerResponse() { Success = true })));

            var buyRequestClientMock = new Mock<IRequestClient<BuyPlateMessage>>();
            buyRequestClientMock.Setup(x => x.GetResponse<ConsumerResponse>(It.IsAny<BuyPlateMessage>(), It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
                                                .Returns(Task.FromResult((Response<ConsumerResponse>)new ResponseFake(new ConsumerResponse() { Success = true })));

            var plateServiceMock = new Mock<PlateService>(mockHttpClientWrapper.Object, reserveRequestClientMock.Object, buyRequestClientMock.Object);
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
            var mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
            var reserveRequestClientMock = new Mock<IRequestClient<ReservePlateMessage>>();
            reserveRequestClientMock.Setup(x => x.GetResponse<ConsumerResponse>(It.IsAny<ReservePlateMessage>(), It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
                                                .Returns(Task.FromResult((Response<ConsumerResponse>)new ResponseFake(new ConsumerResponse() { Success = true })));

            var buyRequestClientMock = new Mock<IRequestClient<BuyPlateMessage>>();
            buyRequestClientMock.Setup(x => x.GetResponse<ConsumerResponse>(It.IsAny<BuyPlateMessage>(), It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
                                                .Returns(Task.FromResult((Response<ConsumerResponse>)new ResponseFake(new ConsumerResponse() { Success = true })));

            var plateServiceMock = new Mock<PlateService>(mockHttpClientWrapper.Object, reserveRequestClientMock.Object, buyRequestClientMock.Object);
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

        [Fact]
        public async Task ReservePlate_SuccessfulResponse_ShouldShowSuccessMessage()
        {
            //Arrange
            var mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
            var reserveRequestClientMock = new Mock<IRequestClient<ReservePlateMessage>>();
            reserveRequestClientMock.Setup(x => x.GetResponse<ConsumerResponse>(It.IsAny<ReservePlateMessage>(), It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
                                                .Returns(Task.FromResult((Response<ConsumerResponse>)new ResponseFake(new ConsumerResponse() { Success = true })));

            var buyRequestClientMock = new Mock<IRequestClient<BuyPlateMessage>>();
            buyRequestClientMock.Setup(x => x.GetResponse<ConsumerResponse>(It.IsAny<BuyPlateMessage>(), It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
                                                .Returns(Task.FromResult((Response<ConsumerResponse>)new ResponseFake(new ConsumerResponse() { Success = true })));

            var plateServiceMock = new Mock<PlateService>(mockHttpClientWrapper.Object, reserveRequestClientMock.Object, buyRequestClientMock.Object);
            plateServiceMock.Setup(x => x.CreatePlateAsync(It.IsAny<Plate>()))
                .Returns(Task.CompletedTask);
            var fakeLogger = new FakeLogger();
            var controller = new HomeController(fakeLogger, plateServiceMock.Object);
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            controller.TempData = tempData;

            //Act
            await controller.Reserve(Guid.NewGuid(), "testuser");

            //Assert
            Assert.True(tempData["Message"] == "Plate reserved successfully!");
        }

        [Fact]
        public async Task ReservePlate_UnsuccessfulResponse_ShouldShowErrorMessage()
        {
            //Arrange
            var errorMessage = "Didn't work";
            var mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
            var reserveRequestClientMock = new Mock<IRequestClient<ReservePlateMessage>>();
            reserveRequestClientMock.Setup(x => x.GetResponse<ConsumerResponse>(It.IsAny<ReservePlateMessage>(), It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
                                                .Returns(Task.FromResult((Response<ConsumerResponse>)new ResponseFake(new ConsumerResponse() { Success = false, Message = errorMessage })));

            var buyRequestClientMock = new Mock<IRequestClient<BuyPlateMessage>>();
            buyRequestClientMock.Setup(x => x.GetResponse<ConsumerResponse>(It.IsAny<BuyPlateMessage>(), It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
                                                .Returns(Task.FromResult((Response<ConsumerResponse>)new ResponseFake(new ConsumerResponse() { Success = true })));

            var plateServiceMock = new Mock<PlateService>(mockHttpClientWrapper.Object, reserveRequestClientMock.Object, buyRequestClientMock.Object);
            plateServiceMock.Setup(x => x.CreatePlateAsync(It.IsAny<Plate>()))
                .Returns(Task.CompletedTask);
            var fakeLogger = new FakeLogger();
            var controller = new HomeController(fakeLogger, plateServiceMock.Object);
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            controller.TempData = tempData;

            //Act
            await controller.Reserve(Guid.NewGuid(), "testuser");

            //Assert
            Assert.True(tempData["Message"] == errorMessage);
        }

        [Fact]
        public async Task BuyPlate_SuccessfulResponse_ShouldShowSuccessMessage()
        {
            //Arrange
            var mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
            var reserveRequestClientMock = new Mock<IRequestClient<ReservePlateMessage>>();
            reserveRequestClientMock.Setup(x => x.GetResponse<ConsumerResponse>(It.IsAny<ReservePlateMessage>(), It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
                                                .Returns(Task.FromResult((Response<ConsumerResponse>)new ResponseFake(new ConsumerResponse() { Success = true })));

            var buyRequestClientMock = new Mock<IRequestClient<BuyPlateMessage>>();
            buyRequestClientMock.Setup(x => x.GetResponse<ConsumerResponse>(It.IsAny<BuyPlateMessage>(), It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
                                                .Returns(Task.FromResult((Response<ConsumerResponse>)new ResponseFake(new ConsumerResponse() { Success = true })));

            var plateServiceMock = new Mock<PlateService>(mockHttpClientWrapper.Object, reserveRequestClientMock.Object, buyRequestClientMock.Object);
            plateServiceMock.Setup(x => x.CreatePlateAsync(It.IsAny<Plate>()))
                .Returns(Task.CompletedTask);
            var fakeLogger = new FakeLogger();
            var controller = new HomeController(fakeLogger, plateServiceMock.Object);
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            controller.TempData = tempData;

            //Act
            await controller.Buy(Guid.NewGuid(), "testuser");

            //Assert
            Assert.True(tempData["Message"] == "Plate bought successfully!");
        }

        [Fact]
        public async Task BuyPlate_UnsuccessfulResponse_ShouldShowErrorMessage()
        {
            //Arrange
            var errorMessage = "Didn't work";
            var mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
            var reserveRequestClientMock = new Mock<IRequestClient<ReservePlateMessage>>();
            reserveRequestClientMock.Setup(x => x.GetResponse<ConsumerResponse>(It.IsAny<ReservePlateMessage>(), It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
                                                .Returns(Task.FromResult((Response<ConsumerResponse>)new ResponseFake(new ConsumerResponse() { Success = true })));

            var buyRequestClientMock = new Mock<IRequestClient<BuyPlateMessage>>();
            buyRequestClientMock.Setup(x => x.GetResponse<ConsumerResponse>(It.IsAny<BuyPlateMessage>(), It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
                                                .Returns(Task.FromResult((Response<ConsumerResponse>)new ResponseFake(new ConsumerResponse() { Success = false, Message = errorMessage })));

            var plateServiceMock = new Mock<PlateService>(mockHttpClientWrapper.Object, reserveRequestClientMock.Object, buyRequestClientMock.Object);
            plateServiceMock.Setup(x => x.CreatePlateAsync(It.IsAny<Plate>()))
                .Returns(Task.CompletedTask);
            var fakeLogger = new FakeLogger();
            var controller = new HomeController(fakeLogger, plateServiceMock.Object);
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            controller.TempData = tempData;

            //Act
            await controller.Buy(Guid.NewGuid(), "testuser");

            //Assert
            Assert.True(tempData["Message"] == errorMessage);
        }
    }
}