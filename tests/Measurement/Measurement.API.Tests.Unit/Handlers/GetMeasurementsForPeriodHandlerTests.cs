using Measurement.API.Handlers;
using Measurement.API.Requests;
using Measurement.Domain;

namespace Measurement.API.Tests.Unit.Handlers
{
    public class GetMeasurementsForPeriodHandlerTests
    {
        [Fact]
        public async Task Handle_ProperVariablePassedToRepositoryAndProperMappingToDTO()
        {
            var measurements = new List<Domain.Measurement>
            {
                new Domain.Measurement(1, 2, DateTime.Now),
                new Domain.Measurement(3, 4, DateTime.Now.AddDays(1)),
                new Domain.Measurement(5, 6, DateTime.Now.AddDays(-10)),
                new Domain.Measurement(7, 8, DateTime.Now.AddYears(-1)),
                new Domain.Measurement(9, 10, DateTime.Now.AddMonths(-3)),
            }.AsEnumerable();

            var from = DateTime.Now.AddDays(-7);
            var to = DateTime.Now.AddDays(3);

            var repositoryMock = new Mock<IMeasurementRepository>();

            repositoryMock
                .Setup(x => x.GetForPeriodAsync(It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
                .Callback((DateTime f, DateTime t, CancellationToken ct) =>
                {
                    f.Should().Be(from); 
                    t.Should().Be(to);                
                })
                .Returns(Task.FromResult(measurements));

            var request = new GetMeasurementsForPeriodRequest(from, to);

            var handler = new GetMeasurementsForPeriodHandler(repositoryMock.Object);

            var response = await handler.Handle(request, default);

            repositoryMock
                .Verify(x => x.GetForPeriodAsync(It.IsAny<DateTime>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);

            for (int i = 0; i < measurements.Count(); i++)
            {
                measurements.ElementAt(i).PM2_5.Value.Should().Be(
                    response.ElementAt(i).PM2_5);

                measurements.ElementAt(i).PM10.Value.Should().Be(
                    response.ElementAt(i).PM10);

                measurements.ElementAt(i).Timestamp.Should().Be(
                    response.ElementAt(i).Timestamp);
            }
        }
    }
}