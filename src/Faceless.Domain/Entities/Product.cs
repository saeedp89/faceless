namespace Faceless.Domain;

public record Product
    (string Title, decimal Price, decimal Weight, decimal Length, decimal Width, string Type) : BaseEntity
{
}