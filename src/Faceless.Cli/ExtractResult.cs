using Faceless.Domain;

public class ExtractResult
{
    public bool ShouldTerminate { get; set; }

    private ExtractResult() : this(true, Enumerable.Empty<Product>())
    {
    }

    public ExtractResult(bool terminate, IEnumerable<Product> products)
    {
        ShouldTerminate = terminate;
        Data = products;
    }

    public static ExtractResult Default => new();


    public IEnumerable<Product> Data { get; set; }
    public override string ToString()
    {
        return $"Terminate: {ShouldTerminate}, Data: {Data.Select(x=>x.ToString())}";
    }
}