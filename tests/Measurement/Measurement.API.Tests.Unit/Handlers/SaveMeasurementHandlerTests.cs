using Measurement.API.Handlers;
using Measurement.API.Requests;
using Measurement.Domain;

namespace Measurement.API.Tests.Unit.Handlers
{
    public class SaveMeasurementHandlerTests
    {
        [Fact]
        public async Task Handle_ProperVariablePassedToRepository()
        {
            var measurementDTO = new DTO.Measurement
            {
                PM2_5 = 100,
                PM10 = 200,
                Timestamp = DateTime.Now
            };

            var repositoryMock = new Mock<IMeasurementRepository>();

            repositoryMock
                .Setup(x => x.SaveAsync(It.IsAny<Domain.Measurement>(),
                    It.IsAny<CancellationToken>()))
                .Callback((Domain.Measurement m, CancellationToken t) =>
                {
                    m.PM2_5.Value.Should().Be(measurementDTO.PM2_5); 
                    m.PM10.Value.Should().Be(measurementDTO.PM10);
                    m.Timestamp.Should().Be(measurementDTO.Timestamp);                
                });

            var request = new SaveMeasurementRequest(measurementDTO);

            var handler = new SaveMeasurementHandler(repositoryMock.Object);

            await handler.Handle(request, default);

            repositoryMock
                .Verify(x => x.SaveAsync(It.IsAny<Domain.Measurement>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
        }
    }
}