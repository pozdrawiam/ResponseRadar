using Rr.Core.Data;
using Rr.Core.Services;
using Rr.Web.Services;
using Serilog;
using System.Globalization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.AddLogging(x => x.ClearProviders().AddSerilog());

builder.Services.AddRazorPages();

builder.Services.AddDbContext<IDb, Db>(o => o.UseSqlite(builder.Configuration["DbConnection"]));

builder.Services.AddTransient<IHttpService, HttpService>();
builder.Services.AddSingleton<IAppConfig, AppConfig>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddScoped<IMonitorService, MonitorService>();
builder.Services.AddHostedService<MonitorWorker>();

var cultureSetting = builder.Configuration["Culture"];

if (cultureSetting != null)
{
    CultureInfo culture = cultureSetting == "invariant" ? CultureInfo.InvariantCulture : new(cultureSetting);

    CultureInfo.DefaultThreadCurrentCulture = culture;
    CultureInfo.DefaultThreadCurrentUICulture = culture;
}

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

using (IServiceScope scope = app.Services.CreateScope())
{
    Db db = scope.ServiceProvider.GetRequiredService<Db>();
    await db.Database.MigrateAsync();
}

app.Logger.LogInformation("Starting app at {Date}", DateTime.Now);

app.Run();

Log.CloseAndFlush();
