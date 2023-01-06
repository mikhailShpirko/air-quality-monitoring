using Monitoring.Infrastructure;
using Monitor = Monitoring.Domain.Monitor;
using Monitoring.Service;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        ServicesConfiguration.Configure(services, context.Configuration);

        var collectMeasurementsIntervalSeconds = 10 * 60; //set default to 10 minutes
        int.TryParse(context.Configuration[nameof(WorkerOptions.CollectMeasurementsIntervalSeconds)], 
                out collectMeasurementsIntervalSeconds);

        services.AddScoped(x => new WorkerOptions(collectMeasurementsIntervalSeconds));       
        services.AddScoped<Monitor>();
        services.AddHostedService<Worker>();
        
    })
    .Build();

await host.RunAsync();
