using System.Runtime.CompilerServices;
using Monitor = Monitoring.Domain.Monitor;

[assembly: InternalsVisibleTo("Monitoring.Service.Tests.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Monitoring.Service
{
    internal sealed class Worker : BackgroundService
    {
        private readonly Monitor _monitor;
        private readonly WorkerOptions _options;
        private readonly ILogger<Worker> _logger;
        
        public Worker(WorkerOptions options,
            Monitor monitor,
            ILogger<Worker> logger)
        {
            _options = options;
            _monitor = monitor;
            _logger = logger;
            _logger.LogInformation("Starting service to collect measurement every {interval} seconds", _options.CollectMeasurementsIntervalSeconds);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _monitor.CollectMeasurementAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error collecting measurement at {time}: {ex}", DateTime.Now, ex);
                }

                await Task.Delay(_options.CollectMeasurementsIntervalSeconds * 1000, stoppingToken);
            }
        }
    }
}