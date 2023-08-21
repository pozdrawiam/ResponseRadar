using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rr.Core;

namespace Rr.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly Db _db;

    public IndexModel(ILogger<IndexModel> logger, Db db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        bool dbCreated = await _db.Database.EnsureCreatedAsync();

        if (dbCreated)
            _logger.LogInformation("Database created");

        return RedirectToPage("./HttpMonitors/Index");
    }
}
