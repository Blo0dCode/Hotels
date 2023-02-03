using Microsoft.AspNetCore.WebUtilities;

namespace Hotels;

public class XmlResult<T> : IResult
{
    private static readonly XmlSerializer XmlSerializer = new(typeof(T));
    private readonly T _result;
    public XmlResult(T result) => _result = result;

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        using var fbws = new FileBufferingWriteStream();//MemoryStream
        XmlSerializer.Serialize(fbws, _result);
        httpContext.Response.ContentType = "application/xml";
        await fbws.DrainBufferAsync(httpContext.Response.Body);//CopyToAsync
        
/*
        var stream = new MemoryStream();
        XmlSerializer.Serialize(stream, _result);
        stream.Flush();
        var s = Encoding.ASCII.GetString(stream.ToArray());*/
    }
}

static class XmlResultExtensions
{
    public static IResult Xml<T>(this IResultExtensions _, T result) =>
        new XmlResult<T>(result);
}