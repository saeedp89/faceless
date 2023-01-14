using Faceless.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Faceless.Repositories.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureRepositories(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IPictureRepository, PictureRepository>();
        services.AddScoped<IProductPicturesRepository, ProductPicturesRepository>();
        services.AddDbContext<FacelessDbContext>(options => options
            .UseSqlServer(configuration.GetConnectionString("FacelessLocal")));
        return services;
    }
}