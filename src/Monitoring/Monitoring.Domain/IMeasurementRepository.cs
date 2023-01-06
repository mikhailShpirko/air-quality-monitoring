namespace Monitoring.Domain
{
    public interface IMeasurementRepository
    {
        Task SaveAsync(Measurement measurement, CancellationToken cancellationToken);
    }
}
