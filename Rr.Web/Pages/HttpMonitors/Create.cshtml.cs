using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rr.Core.Data;

namespace Rr.Web.Pages.HttpMonitors;

public class CreateModel(IDb db) : PageModel
{
    [BindProperty]
    public HttpMonitor HttpMonitor { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        db.HttpMonitors.Add(HttpMonitor);
        await db.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
