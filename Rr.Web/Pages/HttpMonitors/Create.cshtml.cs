using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rr.Core;
using Rr.Core.HttpMonitors;

namespace Rr.Web.Pages.HttpMonitors;

public class CreateModel : PageModel
{
    private readonly IDb _db;

    public CreateModel(IDb db)
    {
        _db = db;
    }

    [BindProperty]
    public HttpMonitor HttpMonitor { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        _db.HttpMonitors.Add(HttpMonitor);
        await _db.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
