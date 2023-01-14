using Faceless.Repositories.Extensions;
using Faceless.WorkerService;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<Worker>();
        services.ConfigureRepositories(context.Configuration);
    }).Build();
host.Run();