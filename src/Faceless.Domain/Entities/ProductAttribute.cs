namespace Faceless.Domain;

public class ProductAttribute : List<Attribute>
{
    public Guid ProductId { get; set; }
}