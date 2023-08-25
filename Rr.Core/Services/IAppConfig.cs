namespace Rr.Core.Services;

public interface IAppConfig
{
    double IntervalMinutes { get; }
    
    string NtfyTopic { get; }
    string NtfyUrl { get; }
}
