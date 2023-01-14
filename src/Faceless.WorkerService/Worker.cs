using Faceless.Application;

namespace Faceless.WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var crawler = scope.ServiceProvider.GetRequiredService<IProductCrawler>();
                await crawler.DoCrawlAsync(stoppingToken);
            }
            
            await Task.Delay(1000, stoppingToken);
        }
    }
}