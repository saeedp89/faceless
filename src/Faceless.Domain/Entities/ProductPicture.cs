namespace Faceless.Domain;

public record ProductPicture : BaseEntity
{
    public Guid ProductId { get; set; }
    public Guid PictureId { get; set; }
}