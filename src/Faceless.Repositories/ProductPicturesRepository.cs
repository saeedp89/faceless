using Faceless.Domain;
using Faceless.Domain.Entities;
using Faceless.Repositories.Abstractions;

namespace Faceless.Repositories;

public class ProductPicturesRepository : FacelessBaseRepository<ProductPicture>, IProductPicturesRepository
{
    public ProductPicturesRepository(FacelessDbContext db) : base(db)
    {
    }
}