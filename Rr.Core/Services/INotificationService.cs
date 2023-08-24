namespace Rr.Core.Services;

public interface INotificationService
{
    Task NotifyAsync(string message, params object[] args);
}

public class NotificationService : INotificationService
{
    public async Task NotifyAsync(string message, params object[] args)
    {
        using var client = new HttpClient();
        
        await client.PostAsync("https://ntfy.sh/response-radar-notif", new StringContent(string.Format(message, args)));
    }
}
