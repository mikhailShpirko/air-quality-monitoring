using Castle.Core.Logging;
using Measurement.API.Controllers;
using Measurement.API.Requests;
using Measurement.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Measurement.API.Tests.Unit.Controllers
{
    public class MeasurementControllerTests
    {
        #region GetMeasurementsForPeriodAsync

        [Fact]
        public async Task GetMeasurementsForPeriodAsync_MediatorReturnsData_OkResponseWithData()
        {
            var measurements = new List<DTO.Measurement>
            {
                new DTO.Measurement
                {
                    PM2_5 = 1, 
                    PM10 = 2, 
                    Timestamp = DateTime.Now 
                },
                new DTO.Measurement
                {
                    PM2_5 = 3,
                    PM10 = 4,
                    Timestamp = DateTime.Now.AddDays(1) 
                },
                new DTO.Measurement
                {
                    PM2_5 = 5,
                    PM10 = 6,
                    Timestamp = DateTime.Now.AddDays(-10) 
                },
                new DTO.Measurement
                {
                    PM2_5 = 7,
                    PM10 = 8,
                    Timestamp = DateTime.Now.AddYears(-1) 
                },
                new DTO.Measurement
                {
                    PM2_5 = 9,
                    PM10 = 10,
                    Timestamp = DateTime.Now.AddMonths(-3) 
                },
            }.AsEnumerable();

            var from = DateTime.Now.AddDays(-7);
            var to = DateTime.Now.AddDays(-2);

            var mediatorMock = new Mock<IMediator>();

            mediatorMock
                .Setup(x => x.Send(It.IsAny<GetMeasurementsForPeriodRequest>(), 
                        It.IsAny<CancellationToken>()))
                .Callback((IRequest<IEnumerable<DTO.Measurement>> ir, CancellationToken ct) =>
                {
                    var r = ir as GetMeasurementsForPeriodRequest;
                    r.Should().NotBeNull(); 
                    r.From.Should().Be(from);
                    r.To.Should().Be(to);
                })
                .Returns(Task.FromResult(measurements));

            var loggerMock = new Mock<ILogger<MeasurementController>>();

            var controller = new MeasurementController(mediatorMock.Object, 
                loggerMock.Object);

            var response = await controller
                .GetMeasurementsForPeriodAsync(from, 
                    to, 
                    default);

            var responseObject = response as ObjectResult;

            responseObject.Should().NotBeNull();
            responseObject.StatusCode.Should().Be(200);

            responseObject.Value.Should().BeOfType<List<DTO.Measurement>>();
            responseObject.Value.Should().BeSameAs(measurements);

            mediatorMock.Verify(x => x.Send(It.IsAny<GetMeasurementsForPeriodRequest>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
        }

        [Fact]
        public async Task GetMeasurementsForPeriodAsync_MediatorThrowsException_ErrorResponseAndExceptionLogged()
        {

            var from = DateTime.Now.AddDays(-7);
            var to = DateTime.Now.AddDays(-2);
            var exception = new Exception("Exception from test");

            var mediatorMock = new Mock<IMediator>();

            mediatorMock
                .Setup(x => x.Send(It.IsAny<GetMeasurementsForPeriodRequest>(),
                        It.IsAny<CancellationToken>()))
                .Throws(exception);

            var loggerMock = new Mock<ILogger<MeasurementController>>();

            var controller = new MeasurementController(mediatorMock.Object,
                loggerMock.Object);

            var response = await controller
                .GetMeasurementsForPeriodAsync(from,
                    to,
                    default);

            var responseObject = response as ObjectResult;

            responseObject.Should().NotBeNull();
            responseObject.StatusCode.Should().Be(500);

            responseObject.Value.Should().BeOfType<string>();
            responseObject.Value.Should().BeSameAs("Exception from test");

            mediatorMock.Verify(x => x.Send(It.IsAny<GetMeasurementsForPeriodRequest>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);

            loggerMock.VerifyLog(x => x.LogError(exception,
                        "Error retrieving measurements for period {from} - {to}",
                        from,
                        to),
                    Times.Once);
        }

        #endregion

        #region SaveMeasurementAsync

        [Fact]
        public async Task SaveMeasurementAsync_MediatorExecutesWithoutException_OkResponse()
        {
            var measurement = new DTO.Measurement
            {
                PM2_5 = 1,
                PM10 = 2,
                Timestamp = DateTime.Now
            };

            var mediatorMock = new Mock<IMediator>();

            mediatorMock
                .Setup(x => x.Send(It.IsAny<SaveMeasurementRequest>(),
                        It.IsAny<CancellationToken>()))
                .Callback((IRequest<MediatR.Unit> ir, CancellationToken ct) =>
                {
                    var r = ir as SaveMeasurementRequest;
                    r.Should().NotBeNull();
                    r.Measurement.Should().NotBeNull();
                    r.Measurement.Should().BeSameAs(measurement);
                })
                .Returns(Task.FromResult(MediatR.Unit.Value));

            var loggerMock = new Mock<ILogger<MeasurementController>>();

            var controller = new MeasurementController(mediatorMock.Object,
                loggerMock.Object);

            var response = await controller
                .SaveMeasurementAsync(measurement,
                    default);

            var responseObject = response as OkResult;

            responseObject.Should().NotBeNull();
            responseObject.StatusCode.Should().Be(200);

            mediatorMock.Verify(x => x.Send(It.IsAny<SaveMeasurementRequest>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
        }

        [Fact]
        public async Task SaveMeasurementAsync_MediatorThrowsException_ErrorResponseAndExceptionLogged()
        {

            var measurement = new DTO.Measurement
            {
                PM2_5 = 1,
                PM10 = 2,
                Timestamp = DateTime.Now
            };

            var exception = new Exception("Exception from test");

            var mediatorMock = new Mock<IMediator>();

            mediatorMock
                .Setup(x => x.Send(It.IsAny<SaveMeasurementRequest>(),
                        It.IsAny<CancellationToken>()))
                .Throws(exception);

            var loggerMock = new Mock<ILogger<MeasurementController>>();

            var controller = new MeasurementController(mediatorMock.Object,
                loggerMock.Object);

            var response = await controller
                .SaveMeasurementAsync(measurement,
                    default);

            var responseObject = response as ObjectResult;

            responseObject.Should().NotBeNull();
            responseObject.StatusCode.Should().Be(500);

            responseObject.Value.Should().BeOfType<string>();
            responseObject.Value.Should().BeSameAs("Exception from test");

            mediatorMock.Verify(x => x.Send(It.IsAny<SaveMeasurementRequest>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);

            loggerMock.VerifyLog(x => x.LogError(exception,
                        "Error saving measurement {measurement}",
                        measurement),
                    Times.Once);
        }

        [Fact]
        public async Task SaveMeasurementAsync_MediatorThrowsInvalidMeasurementValueException_BadRequestResponse()
        {

            var measurement = new DTO.Measurement
            {
                PM2_5 = 1,
                PM10 = 2,
                Timestamp = DateTime.Now
            };

            var exception = new InvalidMeasurementValueException(-100);

            var mediatorMock = new Mock<IMediator>();

            mediatorMock
                .Setup(x => x.Send(It.IsAny<SaveMeasurementRequest>(),
                        It.IsAny<CancellationToken>()))
                .Throws(exception);

            var loggerMock = new Mock<ILogger<MeasurementController>>();

            var controller = new MeasurementController(mediatorMock.Object,
                loggerMock.Object);

            var response = await controller
                .SaveMeasurementAsync(measurement,
                    default);

            var responseObject = response as BadRequestObjectResult;

            responseObject.Should().NotBeNull();
            responseObject.StatusCode.Should().Be(400);

            responseObject.Value.Should().BeOfType<string>();
            responseObject.Value.Should().Be("Measurement Value should be >= 0. Current value: -100");

            mediatorMock.Verify(x => x.Send(It.IsAny<SaveMeasurementRequest>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);

        }

        #endregion

        #region DeleteMeasurementsForPeriodAsync

        [Fact]
        public async Task DeleteMeasurementsForPeriodAsync_MediatorExecutesWithoutException_OkResponse()
        {
            var from = DateTime.Now.AddDays(-7);
            var to = DateTime.Now.AddDays(-2);

            var mediatorMock = new Mock<IMediator>();

            mediatorMock
                .Setup(x => x.Send(It.IsAny<DeleteMeasurementsForPeriodRequest>(),
                        It.IsAny<CancellationToken>()))
                .Callback((IRequest<MediatR.Unit> ir, CancellationToken ct) =>
                {
                    var r = ir as DeleteMeasurementsForPeriodRequest;
                    r.Should().NotBeNull();
                    r.From.Should().Be(from);
                    r.To.Should().Be(to);
                })
                .Returns(Task.FromResult(MediatR.Unit.Value));

            var loggerMock = new Mock<ILogger<MeasurementController>>();

            var controller = new MeasurementController(mediatorMock.Object,
                loggerMock.Object);

            var response = await controller
                .DeleteMeasurementsForPeriodAsync(from,
                    to,
                    default);

            var responseObject = response as OkResult;

            responseObject.Should().NotBeNull();
            responseObject.StatusCode.Should().Be(200);

            mediatorMock.Verify(x => x.Send(It.IsAny<DeleteMeasurementsForPeriodRequest>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
        }

        [Fact]
        public async Task DeleteMeasurementsForPeriodAsync_MediatorThrowsException_ErrorResponseAndExceptionLogged()
        {

            var from = DateTime.Now.AddDays(-7);
            var to = DateTime.Now.AddDays(-2);
            var exception = new Exception("Exception from test");

            var mediatorMock = new Mock<IMediator>();

            mediatorMock
                .Setup(x => x.Send(It.IsAny<DeleteMeasurementsForPeriodRequest>(),
                        It.IsAny<CancellationToken>()))
                .Throws(exception);

            var loggerMock = new Mock<ILogger<MeasurementController>>();

            var controller = new MeasurementController(mediatorMock.Object,
                loggerMock.Object);

            var response = await controller
                .DeleteMeasurementsForPeriodAsync(from,
                    to,
                    default);

            var responseObject = response as ObjectResult;

            responseObject.Should().NotBeNull();
            responseObject.StatusCode.Should().Be(500);

            responseObject.Value.Should().BeOfType<string>();
            responseObject.Value.Should().BeSameAs("Exception from test");

            mediatorMock.Verify(x => x.Send(It.IsAny<DeleteMeasurementsForPeriodRequest>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);

            loggerMock.VerifyLog(x => x.LogError(exception,
                        "Error deleting measurements for period {from} - {to}",
                        from,
                        to),
                    Times.Once);
        }

        #endregion
    }
}
