namespace Faceless.Domain.Entities;

public record ProductPicture : BaseEntity
{
    public Guid ProductId { get; set; }
    public Guid PictureId { get; set; }
}