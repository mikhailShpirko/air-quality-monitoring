namespace Monitoring.Domain
{
    public interface IMeasurementProvider
    {
        Task<Measurement> GetCurrentMeasurementAsync(CancellationToken cancellationToken);
    }
}
