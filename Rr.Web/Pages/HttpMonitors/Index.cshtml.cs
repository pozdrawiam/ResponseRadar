using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rr.Core.Data;
using Rr.Core.Services;

namespace Rr.Web.Pages.HttpMonitors;

public class IndexModel(IDb db, IMonitorService monitorService) : PageModel
{
    public HttpMonitor[] HttpMonitors { get; set; } = Array.Empty<HttpMonitor>();

    public async Task OnGetAsync()
    {
        HttpMonitors = await db.HttpMonitors
            .OrderByDescending(x => x.CheckedAt != default && (x.Status == 0 || x.Status >= 300))
            .ThenBy(x => x.Name)
            .ToArrayAsync();
    }

    public async Task<IActionResult> OnPostCheckAllUrlsAsync()
    {
        await monitorService.CheckUrlsAsync();
        
        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostCheckUrlAsync(int id)
    {
        await monitorService.CheckUrlAsync(id);

        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        HttpMonitor? monitor = await db.HttpMonitors.FindAsync(id);

        if (monitor != null)
        {
            db.HttpMonitors.Remove(monitor);
            await db.SaveChangesAsync();
        }

        return RedirectToPage();
    }
}
