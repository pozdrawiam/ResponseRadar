namespace Rr.Core.Services;

public interface INotificationService
{
    Task NotifyAsync(string message, params object[] args);
}
