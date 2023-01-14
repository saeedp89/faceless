namespace Faceless.Domain.Entities;

public abstract record BaseEntity(Guid Id, DateTimeOffset CreatedAt)
{
    protected BaseEntity() : this(Guid.NewGuid(), DateTimeOffset.Now)
    {
    }

    public DateTimeOffset? UpdateAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }
    public bool? IsDeleted => DeletedAt != null;
}