using Measurement.API.Handlers;
using Measurement.API.Requests;
using Measurement.Domain;

namespace Measurement.API.Tests.Unit.Handlers
{
    public class DeleteMeasurementsForPeriodHandlerTests
    {
        [Fact]
        public async Task Handle_ProperVariablePassedToRepository()
        {
            var from = DateTime.Now.AddDays(-7);
            var to = DateTime.Now.AddDays(3);

            var repositoryMock = new Mock<IMeasurementRepository>();

            repositoryMock
                .Setup(x => x.DeleteForPeriodAsync(It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
                .Callback((DateTime f, DateTime t, CancellationToken ct) =>
                {
                    f.Should().Be(from); 
                    t.Should().Be(to);                
                });

            var request = new DeleteMeasurementsForPeriodRequest(from, to);

            var handler = new DeleteMeasurementsForPeriodHandler(repositoryMock.Object);

            await handler.Handle(request, default);

            repositoryMock
                .Verify(x => x.DeleteForPeriodAsync(It.IsAny<DateTime>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
        }
    }
}