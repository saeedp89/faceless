using Microsoft.Extensions.DependencyInjection;

namespace Faceless.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCrawlerService(this IServiceCollection services)
    {
        services.AddScoped<IProductCrawler, ProductCrawler>();
        return services;
    }
}