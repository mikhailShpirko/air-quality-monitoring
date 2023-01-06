namespace Measurement.Domain
{
    public interface IMeasurementRepository
    {
        Task SaveAsync(Measurement measurement, CancellationToken cancellationToken);
        Task<IEnumerable<Measurement>> GetForPeriodAsync(DateTime from, DateTime to, CancellationToken cancellationToken);
        Task DeleteForPeriodAsync(DateTime from, DateTime to, CancellationToken cancellationToken);
    }
}
