using Rr.Core.Services;

namespace Rr.Tests.Services;

public class NotificationServiceTests
{
    private readonly IAppConfig _appConfig = Substitute.For<IAppConfig>();
    private readonly IHttpService _httpService = Substitute.For<IHttpService>();
    
    [Fact]
    public async Task NotifyAsync_WhenUrlIsEmpty_DoesNotSendNotification()
    {
        _appConfig.NtfyUrl.Returns("");
        
        var notificationService = new NotificationService(Substitute.For<IAppConfig>(), _httpService);

        // Act
        await notificationService.NotifyAsync("Test message");
        
        await _httpService.DidNotReceiveWithAnyArgs().PostAsync(default!, default!);
    }
    
    [Fact]
    public async Task NotifyAsync_WithValidUrl_SendsNotification()
    {
        _appConfig.NtfyUrl.Returns("http://example.com");
        _appConfig.NtfyTopic.Returns("test-topic");

        var notificationService = new NotificationService(_appConfig, _httpService);

        // Act
        await notificationService.NotifyAsync("Test message");

        await _httpService.Received(1).PostAsync("http://example.com/test-topic", Arg.Any<HttpContent>());
    }
}
