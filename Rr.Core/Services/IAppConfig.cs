namespace Rr.Core.Services;

public interface IAppConfig
{
    double IntervalMinutes { get; }
    double TimeoutSeconds { get; }
    
    string NtfyTopic { get; }
    string NtfyUrl { get; }
}
