using Measurement.Domain;
using Measurement.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Measurement.Infrastructure.Tests.Integration")]

namespace Measurement.Infrastructure
{
    public static class ServicesConfiguration
    {
        private const string DbConnectionStringKey = "DbConnectionString";
        public static void Configure(IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<MeasurementsDbContext>(
                options => options.UseNpgsql(configuration[DbConnectionStringKey]));
            services.AddScoped<IMeasurementRepository, MeasurementEFRepository>();
        }
    }
}
