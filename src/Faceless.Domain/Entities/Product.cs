namespace Faceless.Domain.Entities;

public record Product(string Title, string Price, string Size,
    string Weight, string TechnoLifeProductId) : BaseEntity
{
}