using System.Net;
using Rr.Core.Services;

namespace Rr.Tests.Services;

public class NotificationServiceTests
{
    [Fact]
    public async Task NotifyAsync_WhenUrlIsEmpty_DoesNotSendNotification()
    {
        var appConfig = Substitute.For<IAppConfig>();
        appConfig.NtfyUrl.Returns("");
        
        var httpClient = Substitute.ForPartsOf<HttpClient>();
        
        var notificationService = new NotificationService(appConfig, httpClient);

        // Act
        await notificationService.NotifyAsync("Test message");

        // Assert
        await httpClient.DidNotReceive().SendAsync(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task NotifyAsync_WithValidUrl_SendsNotification()
    {
        var appConfig = Substitute.For<IAppConfig>();
        appConfig.NtfyUrl.Returns("http://example.com");
        appConfig.NtfyTopic.Returns("test-topic");

        var httpClient = Substitute.For<HttpClient>();
        httpClient.BaseAddress = new Uri("http://example.com");
        httpClient.SendAsync(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

        var notificationService = new NotificationService(appConfig, httpClient);

        // Act
        await notificationService.NotifyAsync("Test message");

        // Assert
        await httpClient.Received(1).SendAsync(
            Arg.Is<HttpRequestMessage>(request => 
                request.Method == HttpMethod.Post &&
                request.RequestUri!.ToString().Contains("http://example.com/test-topic") &&
                request.Content!.ReadAsStringAsync().Result == new StringContent("Test message").ReadAsStringAsync().Result
            ),
            Arg.Any<CancellationToken>()
        );
    }
}
