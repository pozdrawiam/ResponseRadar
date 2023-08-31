using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rr.Core.Data;
using Rr.Core.Services;

namespace Rr.Web.Pages.HttpMonitors;

public class IndexModel : PageModel
{
    private readonly IDb _db;
    private readonly IMonitorService _monitorService;

    public IndexModel(IDb db, IMonitorService monitorService)
    {
        _db = db;
        _monitorService = monitorService;
    }

    public HttpMonitor[] HttpMonitors { get; set; } = Array.Empty<HttpMonitor>();

    public async Task OnGetAsync()
    {
        HttpMonitors = await _db.HttpMonitors
            .OrderByDescending(x => x.CheckedAt != default && (x.Status == 0 || x.Status >= 300))
            .ThenBy(x => x.Name)
            .ToArrayAsync();
    }

    public async Task<IActionResult> OnPostCheckAllUrlsAsync()
    {
        await _monitorService.CheckUrlsAsync();
        
        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostCheckUrlAsync(int id)
    {
        await _monitorService.CheckUrlAsync(id);

        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        HttpMonitor? monitor = await _db.HttpMonitors.FindAsync(id);

        if (monitor != null)
        {
            _db.HttpMonitors.Remove(monitor);
            await _db.SaveChangesAsync();
        }

        return RedirectToPage();
    }
}
