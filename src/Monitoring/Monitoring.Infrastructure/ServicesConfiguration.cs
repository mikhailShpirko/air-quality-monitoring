using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Monitoring.Domain;
using Polly.Extensions.Http;
using Polly;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Monitoring.Infrastructure.Tests.Integration")]
namespace Monitoring.Infrastructure
{
    public static class ServicesConfiguration
    {
        private const string SensorApiBaseAddressKey = "SensorApiBaseAddress";
        private const string MeasurementApiBaseAddressKey = "MeasurementApiBaseAddress";
        public static void Configure(IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IMeasurementProvider, SensorMeasurementProvider>();
            services.AddScoped<IMeasurementRepository, MeasurementApiRepository>();

            services
                .AddHttpClient<IMeasurementProvider, SensorMeasurementProvider>(c => 
                    c.BaseAddress = new Uri(configuration[SensorApiBaseAddressKey]))
                .AddPolicyHandler(GetRetryPolicy());

            services
                .AddHttpClient<IMeasurementRepository, MeasurementApiRepository>(c => 
                    c.BaseAddress = new Uri(configuration[MeasurementApiBaseAddressKey]))
                .AddPolicyHandler(GetRetryPolicy());    
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode != System.Net.HttpStatusCode.OK)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                            retryAttempt)));
        }
    }
}