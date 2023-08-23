using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rr.Core;
using Rr.Core.HttpMonitors;

namespace Rr.Web.Pages.HttpMonitors;

public class EditModel : PageModel
{
    private readonly IDb _db;

    public EditModel(IDb db)
    {
        _db = db;
    }

    [BindProperty]
    public HttpMonitor? HttpMonitor { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
            return NotFound();

        HttpMonitor = await _db.HttpMonitors.FirstOrDefaultAsync(m => m.Id == id);

        if (HttpMonitor == null)
            return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (HttpMonitor != null)
        {
            _db.AttachModified(HttpMonitor);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(HttpMonitor.Id))
                {
                    return NotFound();
                }

                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool CustomerExists(int id)
    {
        return _db.HttpMonitors.Any(e => e.Id == id);
    }
}
