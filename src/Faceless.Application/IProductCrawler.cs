namespace Faceless.Application;

public interface IProductCrawler
{
    Task DoCrawlAsync(CancellationToken token);
}