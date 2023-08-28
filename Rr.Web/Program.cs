using Rr.Core.Data;
using Rr.Core.Services;
using Rr.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

builder.Services.AddDbContext<IDb, Db>(o => o.UseSqlite(builder.Configuration["DbConnection"]));

builder.Services.AddTransient<IHttpService, HttpService>();
builder.Services.AddSingleton<IAppConfig, AppConfig>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddScoped<IMonitorService, MonitorService>();
builder.Services.AddHostedService<MonitorWorker>();

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
    bool dbCreated = await db.Database.EnsureCreatedAsync();
    
    if (dbCreated)
        app.Logger.LogInformation("Database created");
}

app.Logger.LogInformation("Starting app at {}", DateTime.Now);

app.Run();
