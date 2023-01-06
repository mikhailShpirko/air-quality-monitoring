using Measurement.API.Requests;
using Measurement.Domain;
using MediatR;

namespace Measurement.API.Handlers
{
    public sealed class SaveMeasurementHandler : IRequestHandler<SaveMeasurementRequest>
    {
        private readonly IMeasurementRepository _measurementRepository;
        public SaveMeasurementHandler(IMeasurementRepository measurementRepository)
        {
            _measurementRepository = measurementRepository;
        }

        public async Task<Unit> Handle(SaveMeasurementRequest request, 
            CancellationToken cancellationToken)
        {
            var measurement = new Domain.Measurement(request.Measurement.PM2_5, 
                request.Measurement.PM10, 
                request.Measurement.Timestamp);

            await _measurementRepository.SaveAsync(measurement, cancellationToken);

            return Unit.Value;
        }
    }
}
