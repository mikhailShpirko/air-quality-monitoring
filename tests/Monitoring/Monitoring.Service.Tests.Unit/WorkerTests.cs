using Microsoft.Extensions.Logging;
using Monitoring.Domain;
using Monitor = Monitoring.Domain.Monitor;

namespace Monitoring.Service.Tests.Unit
{
    public class WorkerTests
    {
        [Fact]
        public void Ctor_LogInformationCalledWithOptions()
        {
            var options = new WorkerOptions(16);

            var measurementProviderMock = new Mock<IMeasurementProvider>();
            var measurementRepositoryMock = new Mock<IMeasurementRepository>();

            var loggerMock = new Mock<ILogger<Worker>>();

            var monitor = new Monitor(measurementProviderMock.Object,
                measurementRepositoryMock.Object);

            var worker = new Worker(options,
                monitor,
                loggerMock.Object);

            loggerMock
                .VerifyLog(x => x.LogInformation("Starting service to collect measurement every {interval} seconds",
                        options.CollectMeasurementsIntervalSeconds),
                    Times.Once);
        }

        [Fact]
        public async Task StartAsync_NoExceptionsThrownByMonitor_NoErrorLogging()
        {
            var options = new WorkerOptions(2);

            var measurementProviderMock = new Mock<IMeasurementProvider>();
            var measurementRepositoryMock = new Mock<IMeasurementRepository>();

            var loggerMock = new Mock<ILogger<Worker>>();

            var monitor = new Monitor(measurementProviderMock.Object,
                measurementRepositoryMock.Object);

            var worker = new Worker(options,
                monitor,
                loggerMock.Object);

            worker.StartAsync(default);

            await Task.Delay(1000);

            measurementProviderMock.
                Verify(x => x.GetCurrentMeasurementAsync(It.IsAny<CancellationToken>()),
                    Times.Once);

            measurementRepositoryMock.
               Verify(x => x.SaveAsync(It.IsAny<Measurement>(),
                        It.IsAny<CancellationToken>()),
                   Times.Once);

            loggerMock
                .VerifyLog(x => x.LogError("Error collecting measurement at {time}: {ex}", 
                        It.IsAny<DateTime>(), 
                        It.IsAny<Exception>()),
                    Times.Never);

            await worker.StopAsync(default);

            worker.Dispose();
        }

        [Fact]
        public async Task StartAsync_SettingsWithInterval_ProperNumberOfExecutions()
        {
            var options = new WorkerOptions(1);

            var measurementProviderMock = new Mock<IMeasurementProvider>();
            var measurementRepositoryMock = new Mock<IMeasurementRepository>();

            var loggerMock = new Mock<ILogger<Worker>>();

            var monitor = new Monitor(measurementProviderMock.Object,
                measurementRepositoryMock.Object);

            var worker = new Worker(options,
                monitor,
                loggerMock.Object);

            worker.StartAsync(default);

            await Task.Delay(3000);

            measurementProviderMock.
                Verify(x => x.GetCurrentMeasurementAsync(It.IsAny<CancellationToken>()),
                    Times.Exactly(3));

            measurementRepositoryMock.
               Verify(x => x.SaveAsync(It.IsAny<Measurement>(),
                        It.IsAny<CancellationToken>()),
                   Times.Exactly(3));

            loggerMock
                .VerifyLog(x => x.LogError("Error collecting measurement at {time}: {ex}",
                        It.IsAny<DateTime>(),
                        It.IsAny<Exception>()),
                    Times.Never);

            await worker.StopAsync(default);

            worker.Dispose();
        }

        [Fact]
        public async Task StartAsync_ExceptionThrownByMonitor_ErrorLogged()
        {
            var options = new WorkerOptions(2);

            var measurementProviderMock = new Mock<IMeasurementProvider>();
            var measurementRepositoryMock = new Mock<IMeasurementRepository>();

            measurementProviderMock
                .Setup(x => x.GetCurrentMeasurementAsync(It.IsAny<CancellationToken>()))
                .Throws(new ArgumentOutOfRangeException());

            var loggerMock = new Mock<ILogger<Worker>>();

            var monitor = new Monitor(measurementProviderMock.Object,
                measurementRepositoryMock.Object);

            var worker = new Worker(options,
                monitor,
                loggerMock.Object);

            worker.StartAsync(default);

            await Task.Delay(1000);

            loggerMock
                .VerifyLog(x => x.LogError("Error collecting measurement at {time}: {ex}",
                        It.IsAny<DateTime>(),
                        It.IsAny<ArgumentOutOfRangeException>()),
                    Times.Once);

            await worker.StopAsync(default);

            worker.Dispose();
        }
    }
}