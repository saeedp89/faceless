namespace Faceless.Domain;

public record Picture(string Path, string AltAttribute, string MimeType) : BaseEntity;

