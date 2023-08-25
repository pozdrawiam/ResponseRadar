namespace Rr.Core.Services;

public interface INotificationService
{
    Task NotifyAsync(string message, params object[] args);
}

public class NotificationService : INotificationService
{
    private readonly string _ntfyFullUrl;
    
    public NotificationService(IAppConfig appConfig)
    {
        _ntfyFullUrl = appConfig.NtfyUrl + "/" + appConfig.NtfyTopic;
    }
    
    public async Task NotifyAsync(string message, params object[] args)
    {
        if (_ntfyFullUrl.Length <= 1)
            return;
        
        using var client = new HttpClient(); //todo remove new
        
        await client.PostAsync(_ntfyFullUrl, new StringContent(string.Format(message, args)));
    }
}
