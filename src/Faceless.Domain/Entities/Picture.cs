namespace Faceless.Domain.Entities;

public record Picture(string Path, string AltAttribute, string MimeType) : BaseEntity;

