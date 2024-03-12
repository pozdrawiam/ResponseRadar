using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rr.Core.Data;

namespace Rr.Web.Pages.HttpMonitors;

public class EditModel(IDb db) 
    : PageModel
{
    [BindProperty]
    public HttpMonitor? HttpMonitor { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
            return NotFound();

        HttpMonitor = await db.HttpMonitors.FirstOrDefaultAsync(m => m.Id == id);

        if (HttpMonitor == null)
            return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        if (HttpMonitor != null)
        {
            db.AttachModified(HttpMonitor);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!db.HttpMonitors.Any(e => e.Id == HttpMonitor.Id))
                    return NotFound();

                throw;
            }
        }

        return RedirectToPage("./Index");
    }
}
