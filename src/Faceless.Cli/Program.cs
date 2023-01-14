using Faceless.Application;
using Faceless.Application.Extensions;
using Faceless.Repositories.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

var services = new ServiceCollection();
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", true, true)
    .Build();
services.ConfigureRepositories(configuration);
services.AddCrawlerService();
services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Trace);
    
});
var provider = services.BuildServiceProvider();
var crawler = provider.GetRequiredService<IProductCrawler>();
crawler.DoCrawlAsync(new CancellationToken(false));