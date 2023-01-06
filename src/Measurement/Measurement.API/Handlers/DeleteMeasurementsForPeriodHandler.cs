using Measurement.API.Requests;
using Measurement.Domain;
using MediatR;

namespace Measurement.API.Handlers
{
    public sealed class DeleteMeasurementsForPeriodHandler
        : IRequestHandler<DeleteMeasurementsForPeriodRequest>
    {
        private readonly IMeasurementRepository _measurementRepository;
        public DeleteMeasurementsForPeriodHandler(IMeasurementRepository measurementRepository)
        {
            _measurementRepository = measurementRepository;
        }

        public async Task<Unit> Handle(DeleteMeasurementsForPeriodRequest request, 
            CancellationToken cancellationToken)
        {
            await _measurementRepository
                .DeleteForPeriodAsync(request.From,
                    request.To,
                    cancellationToken);

            return Unit.Value;
        }
    }
}
