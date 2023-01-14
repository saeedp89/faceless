using Faceless.Domain;
using Faceless.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Faceless.Repositories;

public class FacelessDbContext : DbContext
{
    public FacelessDbContext(DbContextOptions<FacelessDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Picture> Pictures { get; set; }
    public DbSet<ProductPicture> Gallery { get; set; }
}