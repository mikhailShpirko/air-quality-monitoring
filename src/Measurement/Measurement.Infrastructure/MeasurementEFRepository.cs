using Measurement.Domain;
using Measurement.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Measurement.Infrastructure
{
    internal class MeasurementEFRepository : IMeasurementRepository
    {
        private readonly MeasurementsDbContext _dbContext;

        public MeasurementEFRepository(MeasurementsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteForPeriodAsync(DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            var measurements = await GetMeasurementsForPeriodAsync(from, to, cancellationToken);
            _dbContext.RemoveRange(measurements);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Domain.Measurement>> GetForPeriodAsync(DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            var measurements = await GetMeasurementsForPeriodAsync(from, to, cancellationToken);

            return measurements
                .Select(m => new Domain.Measurement(m.PM2_5, m.PM10, new DateTime(m.TimestampTicks)));
        }

        public async Task SaveAsync(Domain.Measurement measurement, CancellationToken cancellationToken)
        {
            var measurementEntity = new EntityFramework.Measurement
            {
                Id = Guid.NewGuid(),
                PM2_5 = measurement.PM2_5.Value,
                PM10 = measurement.PM10.Value,
                TimestampTicks = measurement.Timestamp.Ticks
            };

            await _dbContext.Measurements.AddAsync(measurementEntity, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }


        private async Task<IEnumerable<EntityFramework.Measurement>> GetMeasurementsForPeriodAsync(DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            return await _dbContext
                .Measurements
                .Where(m => m.TimestampTicks >= from.Ticks && m.TimestampTicks <= to.Ticks)
                .ToListAsync(cancellationToken);
        }
    }
}