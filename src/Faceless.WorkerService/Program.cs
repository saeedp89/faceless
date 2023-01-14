using Faceless.Application.Extensions;
using Faceless.Repositories.Extensions;
using Faceless.WorkerService;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<Worker>();
        services.ConfigureRepositories(context.Configuration);
        services.AddCrawlerService();
        
    }).Build();
host.Run();