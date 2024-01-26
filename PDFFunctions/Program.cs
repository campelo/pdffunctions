using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PDFFunctions.Options;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((builderContext, services) =>
    {
        var configuration = builderContext.Configuration;
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.Configure<ApryseOptions>(configuration.GetSection(ApryseOptions.OPTIONS_NAME));
    })
    .Build();

host.Run();
