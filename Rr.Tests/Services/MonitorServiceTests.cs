using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute.ExceptionExtensions;
using Rr.Core.Data;
using Rr.Core.Services;

namespace Rr.Tests.Services;

public class MonitorServiceTests
{
    private readonly IAppConfig _appConfig = Substitute.For<IAppConfig>();
    private readonly IDb _db = Substitute.For<IDb>();
    private readonly IHttpService _httpService = Substitute.For<IHttpService>();
    private readonly ILogger<MonitorService> _logger = Substitute.For<ILogger<MonitorService>>();
    private readonly INotificationService _notificationService = Substitute.For<INotificationService>();

    private readonly IMonitorService _sut;

    public MonitorServiceTests()
    {
        _appConfig.TimeoutSeconds.Returns(1);
        
        _sut = new MonitorService(_appConfig, _db, _httpService, _logger, _notificationService);
    }

    [Fact]
    public async Task CheckUrlsAsync_WhenNoMonitors_ShouldNotDoHttpRequest()
    {
        DbSet<HttpMonitor> monitors = TestHelper.MockDbSet(Array.Empty<HttpMonitor>());
        
        _db.HttpMonitors.Returns(monitors);
        
        // Act
        await _sut.CheckUrlsAsync();

        await _httpService.DidNotReceiveWithAnyArgs().GetAsync(Arg.Any<string>());
    }

    [Fact]
    public async Task CheckUrlsAsync_WhenHttpRequestException_ShouldNotify()
    {
        var monitor = new HttpMonitor { Url = "http://example.com", Name = "TestMonitor" };
        DbSet<HttpMonitor> monitors = TestHelper.MockDbSet(new[] { monitor });
        
        _db.HttpMonitors.Returns(monitors);
        _httpService.GetAsync(monitor.Url).Throws<HttpRequestException>();

        // Act
        await _sut.CheckUrlsAsync();

        _logger.ReceivedWithAnyArgs(1).LogWarning(default);
        await _notificationService.ReceivedWithAnyArgs(1).NotifyAsync(default!);
    }
    
    [Fact]
    public async Task CheckUrlsAsync_WhenHttpStatusOk_ShouldNotNotify()
    {
        var monitor = new HttpMonitor { Url = "http://example.com", Name = "TestMonitor" };
        DbSet<HttpMonitor> monitors = TestHelper.MockDbSet(new[] { monitor });
        
        _db.HttpMonitors.Returns(monitors);
        
        _httpService.GetAsync(monitor.Url).Returns(new HttpResponseMessage(HttpStatusCode.OK));

        // Act
        await _sut.CheckUrlsAsync();

        _logger.ReceivedWithAnyArgs(1).LogInformation(default);
        await _notificationService.DidNotReceiveWithAnyArgs().NotifyAsync(default!);
    }
    
    [Fact]
    public async Task CheckUrlsAsync_WhenHttpStatusNotOk_ShouldNotify()
    {
        var monitor = new HttpMonitor { Url = "http://example.com", Name = "TestMonitor" };
        DbSet<HttpMonitor> monitors = TestHelper.MockDbSet(new[] { monitor });
        
        _db.HttpMonitors.Returns(monitors);
        
        _httpService.GetAsync(monitor.Url).Returns(new HttpResponseMessage(HttpStatusCode.InternalServerError));

        // Act
        await _sut.CheckUrlsAsync();

        _logger.ReceivedWithAnyArgs(1).LogWarning(default);
        await _notificationService.ReceivedWithAnyArgs(1).NotifyAsync(default!);
    }
}
