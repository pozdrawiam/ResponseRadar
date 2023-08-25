using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rr.Core.Data;

namespace Rr.Web.Pages.HttpMonitors;

public class IndexModel : PageModel
{
    private readonly IDb _db;

    public IndexModel(IDb db)
    {
        _db = db;
    }

    public HttpMonitor[] HttpMonitors { get; set; } = Array.Empty<HttpMonitor>();

    public async Task OnGetAsync()
    {
        HttpMonitors = await _db.HttpMonitors.ToArrayAsync();
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
