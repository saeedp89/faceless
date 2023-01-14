using Faceless.Domain;
using Faceless.Domain.Entities;

namespace Faceless.Repositories.Abstractions;

public interface IProductRepository : IFacelessBaseRepository<Product>
{
    Task<Product?> GetByTechnoIdAsync(string technoId);
}