namespace Monitoring.Domain.Tests.Unit
{
    public class MonitorTests
    {
        [Fact]
        public async Task CollectMeasurementAsync_MeasurementReturnedFromProvider_SameMeasurementSendToRepository()
        {
            var measurement = new Measurement
            {
                PM2_5 = 123,
                PM10 = 456,
                Timestamp = DateTime.Now
            };

            var measurementProviderMock = new Mock<IMeasurementProvider>();

            measurementProviderMock
                .Setup(x => x.GetCurrentMeasurementAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(measurement));

            var measurementRepositoryMock = new Mock<IMeasurementRepository>();

            measurementRepositoryMock
                .Setup(x => x.SaveAsync(It.IsAny<Measurement>(), It.IsAny<CancellationToken>()))
                .Callback((Measurement m, CancellationToken t) =>
                {
                    m.Should().BeSameAs(measurement);
                });

            var monitor = new Monitor(measurementProviderMock.Object, measurementRepositoryMock.Object);

            await monitor.CollectMeasurementAsync(default);

            measurementProviderMock
                .Verify(x => x.GetCurrentMeasurementAsync(It.IsAny<CancellationToken>()), 
                    Times.Once);

            measurementRepositoryMock
                .Verify(x => x.SaveAsync(It.IsAny<Measurement>(), 
                        It.IsAny<CancellationToken>()), 
                    Times.Once);
        }
    }
}