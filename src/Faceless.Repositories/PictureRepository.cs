using Faceless.Domain;
using Faceless.Domain.Entities;
using Faceless.Repositories.Abstractions;

namespace Faceless.Repositories;

public class PictureRepository : FacelessBaseRepository<Picture>, IPictureRepository
{
    public PictureRepository(FacelessDbContext db) : base(db)
    {
    }
}