using Measurement.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Measurement.Infrastructure.Tests.Integration
{
    internal class MeasurementDataFixture : IDisposable
    {
        public readonly MeasurementsDbContext DbContext;

        public MeasurementDataFixture(DateTime createDate)
        {
            var options = new DbContextOptionsBuilder<MeasurementsDbContext>()
            .UseInMemoryDatabase(databaseName: $"MeasurementsDatabase_{createDate.Ticks}")
            .Options;

            DbContext = new MeasurementsDbContext(options);

            DbContext.Measurements.Add(new EntityFramework.Measurement
            {
                Id = Guid.NewGuid(),
                PM2_5 = 1,
                PM10 = 2,
                TimestampTicks = createDate.Ticks
            });

            DbContext.Measurements.Add(new EntityFramework.Measurement
            {
                Id = Guid.NewGuid(),
                PM2_5 = 3,
                PM10 = 4,
                TimestampTicks = createDate.AddYears(-1).Ticks
            });

            DbContext.Measurements.Add(new EntityFramework.Measurement
            {
                Id = Guid.NewGuid(),
                PM2_5 = 5,
                PM10 = 6,
                TimestampTicks = createDate.AddYears(1).Ticks
            });

            DbContext.SaveChanges();
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
