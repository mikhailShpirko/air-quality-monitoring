using Measurement.API.Requests;
using Measurement.Domain;
using MediatR;

namespace Measurement.API.Handlers
{
    public sealed class GetMeasurementsForPeriodHandler
        : IRequestHandler<GetMeasurementsForPeriodRequest, IEnumerable<DTO.Measurement>>
    {
        private readonly IMeasurementRepository _measurementRepository;
        public GetMeasurementsForPeriodHandler(IMeasurementRepository measurementRepository)
        {
            _measurementRepository = measurementRepository;
        }
        public async Task<IEnumerable<DTO.Measurement>> Handle(GetMeasurementsForPeriodRequest request, 
            CancellationToken cancellationToken)
        {
            var measurements = await _measurementRepository
                .GetForPeriodAsync(request.From,
                    request.To,
                    cancellationToken);

            return measurements
                .Select(m => new DTO.Measurement
                {
                    PM2_5 = m.PM2_5.Value,
                    PM10 = m.PM10.Value,
                    Timestamp = m.Timestamp
                });
        }
    }
}
