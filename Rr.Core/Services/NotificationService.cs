namespace Rr.Core.Services;

public class NotificationService : INotificationService
{
    private readonly IHttpService _httpService;
    private readonly string _ntfyFullUrl;
    
    public NotificationService(IAppConfig appConfig, IHttpService httpService)
    {
        _httpService = httpService;
        _ntfyFullUrl = appConfig.NtfyUrl + "/" + appConfig.NtfyTopic;
    }
    
    public async Task NotifyAsync(string message, params object[] args)
    {
        if (_ntfyFullUrl.Length <= 1)
            return;
        
        var content = new StringContent(string.Format(message, args));
        
        await _httpService.PostAsync(_ntfyFullUrl, content);
    }
}
