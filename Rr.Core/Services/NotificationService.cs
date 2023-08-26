namespace Rr.Core.Services;

public interface INotificationService
{
    Task NotifyAsync(string message, params object[] args);
}

public class NotificationService : INotificationService
{
    private readonly HttpClient _httpClient;
    private readonly string _ntfyFullUrl;
    
    public NotificationService(IAppConfig appConfig, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _ntfyFullUrl = appConfig.NtfyUrl + "/" + appConfig.NtfyTopic;
    }
    
    public async Task NotifyAsync(string message, params object[] args)
    {
        if (_ntfyFullUrl.Length <= 1)
            return;
        
        var content = new StringContent(string.Format(message, args));
        
        await _httpClient.PostAsync(_ntfyFullUrl, content);
    }
}
