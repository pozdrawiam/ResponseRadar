using Rr.Core;
using Rr.Core.HttpMonitors;
using Rr.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<IDb, Db>(options =>
    options.UseSqlite("Data Source=ResponseRadar.db"));

builder.Services.AddScoped<MonitorService>();
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

app.Logger.LogInformation("Starting app at {}", DateTime.Now);

app.Run();
