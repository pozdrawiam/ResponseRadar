using Rr.Core.Services;

namespace Rr.Tests.Services;

public class NotificationServiceTests
{
    private readonly IAppConfig _appConfig = Substitute.For<IAppConfig>();
    private readonly IHttpService _httpService = Substitute.For<IHttpService>();

    private readonly Lazy<NotificationService> _sut;
    
    public NotificationServiceTests()
    {
        _sut = new(() => new(_appConfig, _httpService));
    }
    
    [Fact]
    public async Task NotifyAsync_WhenUrlIsEmpty_DoesNotSendNotification()
    {
        _appConfig.NtfyUrl.Returns("");

        // Act
        await _sut.Value.NotifyAsync("Test message");
        
        await _httpService.DidNotReceiveWithAnyArgs().PostAsync(default!, default!);
    }
    
    [Fact]
    public async Task NotifyAsync_WithValidUrl_SendsNotification()
    {
        _appConfig.NtfyUrl.Returns("http://example.com");
        _appConfig.NtfyTopic.Returns("test-topic");

        // Act
        await _sut.Value.NotifyAsync("Test message");

        await _httpService.Received(1).PostAsync("http://example.com/test-topic", 
#pragma warning disable xUnit1031
            Arg.Is<StringContent>(x => x.ReadAsStringAsync().Result == "Test message"));
#pragma warning restore xUnit1031
    }
}
