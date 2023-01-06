namespace Monitoring.Domain
{
    public sealed class Monitor
    {
        private readonly IMeasurementProvider _measurementProvider;
        private readonly IMeasurementRepository _measurementRepository;

        public Monitor(IMeasurementProvider measurementProvider,
            IMeasurementRepository measurementRepository)
        {
            _measurementProvider = measurementProvider;
            _measurementRepository = measurementRepository;
        }     

        public async Task CollectMeasurementAsync(CancellationToken cancellationToken)
        {
            var measurement = await _measurementProvider.GetCurrentMeasurementAsync(cancellationToken);
            await _measurementRepository.SaveAsync(measurement, cancellationToken);
        }
    }
}
