using Rr.Core.Services;

namespace Rr.Web.Services;

public class AppConfig : IAppConfig
{
    private readonly IConfiguration _config;

    public AppConfig(IConfiguration config)
    {
        _config = config;
    }

    public double IntervalMinutes => _config.GetValue<double>(nameof(IntervalMinutes));
    public string NtfyTopic => _config[nameof(NtfyTopic)] ?? "";
    public string NtfyUrl => _config[nameof(NtfyUrl)] ?? "";
}
