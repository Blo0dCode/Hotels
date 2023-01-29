namespace Hotels;

public class XmlResult<T> : IResult
{
    private static readonly XmlSerializer XmlSerializer = new(typeof(T));
    private readonly T _result;
    public XmlResult(T result) => _result = result;

    public Task ExecuteAsync(HttpContext httpContext)
    {
        using var ms = new MemoryStream();
        XmlSerializer.Serialize(ms, _result);
        httpContext.Response.ContentType = "application/xml";
        return ms.CopyToAsync(httpContext.Response.Body);
    }
}

static class XmlResultExtensions
{
    public static IResult Xml<T>(this IResultExtensions _, T result) =>
        new XmlResult<T>(result);
}