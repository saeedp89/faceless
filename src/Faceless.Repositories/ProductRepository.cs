using Faceless.Domain;
using Faceless.Domain.Entities;
using Faceless.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Faceless.Repositories;

public class ProductRepository : FacelessBaseRepository<Product>, IProductRepository
{
    public ProductRepository(FacelessDbContext db) : base(db)
    {
    }

    public async Task<Product?> GetByTechnoIdAsync(string technoId)
    {
        return await _facelessDb.Products.FirstOrDefaultAsync(x => x.TechnoLifeProductId == technoId);
    }
}