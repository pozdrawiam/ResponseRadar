namespace Rr.Core.Services;

public interface IHttpService
{
    TimeSpan Timeout { get; set; }

    Task<HttpResponseMessage> GetAsync(string url);
    Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
}

public class HttpService : HttpClient, IHttpService
{
}
