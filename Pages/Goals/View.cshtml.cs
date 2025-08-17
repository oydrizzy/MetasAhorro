using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MetasAhorro.Data;
using MetasAhorro.Models;
using System.Security.Cryptography;

namespace MetasAhorro.Pages.Goals
{
    public class ViewModel : PageModel
    {
        private readonly AppDbContext _db;
        public ViewModel(AppDbContext db) => _db = db;

        public Goal? Goal { get; set; }
        public List<Deposit> Timeline { get; set; } = new();

        [TempData] public string? Flash { get; set; }

        private const string OwnerCookieName = "MA.OwnerKey";
        private string GetOwnerKey() => Request.Cookies.TryGetValue(OwnerCookieName, out var k) ? k : "";

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var ownerKey = GetOwnerKey();

            Goal = await _db.Goals
                .AsNoTracking()
                .Include(g => g.Deposits)
                .FirstOrDefaultAsync(g => g.Id == id && g.OwnerKey == ownerKey);

            if (Goal is null)
            {
                Flash = "La meta no existe o no te pertenece.";
                return RedirectToPage("Index");
            }

            Timeline = Goal.Deposits
                .OrderBy(d => d.Date)
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var ownerKey = GetOwnerKey();

            var goal = await _db.Goals
                .Include(g => g.Deposits)
                .FirstOrDefaultAsync(g => g.Id == id && g.OwnerKey == ownerKey);

            if (goal is null)
            {
                Flash = "La meta no existe o no te pertenece.";
                return RedirectToPage("Index");
            }

            if (goal.Deposits?.Count > 0)
                _db.Deposits.RemoveRange(goal.Deposits);

            _db.Goals.Remove(goal);
            await _db.SaveChangesAsync();

            Flash = "Meta eliminada correctamente.";
            return RedirectToPage("Index");
        }
    }
}
