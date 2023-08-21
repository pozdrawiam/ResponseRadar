using Microsoft.AspNetCore.Mvc.RazorPages;
using Rr.Core;
using Rr.Core.HttpMonitors;

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
}
