using Microsoft.EntityFrameworkCore;

namespace Measurement.Infrastructure.EntityFramework
{
    internal class MeasurementsDbContext : DbContext
    {
        public DbSet<Measurement> Measurements { get; set; }

        public MeasurementsDbContext(DbContextOptions<MeasurementsDbContext> options) 
            : base(options)
        {
        }

    }
}
