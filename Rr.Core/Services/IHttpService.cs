namespace Rr.Core.Services;

public interface IHttpService
{
    TimeSpan Timeout { get; set; }

    Task<HttpResponseMessage> GetAsync(string url);
    Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
}

public class HttpService : IHttpService
{
    public TimeSpan Timeout { get; set; }
    
    public async Task<HttpResponseMessage> GetAsync(string url)
    {
        using HttpClient client = GetClient();

        return await client.GetAsync(url);
    }

    public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
    {
        using HttpClient client = GetClient();

        return await client.PostAsync(url, content);
    }

    private HttpClient GetClient()
    {
        var client = new HttpClient();
        
        if (Timeout != default)
            client.Timeout = Timeout;

        return client;
    }
}
