using Faceless.Domain;
using Faceless.Domain.Entities;
using Faceless.Repositories.Abstractions;

namespace Faceless.Repositories;

public class ProductRepository : FacelessBaseRepository<Product>, IProductRepository
{
    public ProductRepository(FacelessDbContext db) : base(db)
    {
    }
}