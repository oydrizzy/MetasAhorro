using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MetasAhorro.Data;
using MetasAhorro.Models;
using System.Security.Cryptography;

namespace MetasAhorro.Pages.Goals
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        public IndexModel(AppDbContext db) => _db = db;

        public List<Goal> Items { get; set; } = new();

        public decimal TotalTarget { get; set; }
        public decimal TotalCurrent { get; set; }
        public decimal TotalRemaining => Math.Max(0, TotalTarget - TotalCurrent);
        public int OverallProgressPct { get; set; }
        public string Motivation { get; set; } = "";

        [TempData] public string? Flash { get; set; }

        private const string OwnerCookieName = "MA.OwnerKey";

        public async Task OnGetAsync()
        {
            var ownerKey = GetOrCreateOwnerKey();

            Items = await _db.Goals
                .AsNoTracking()
                .Include(g => g.Deposits)
                .Where(g => g.OwnerKey == ownerKey)
                .ToListAsync();

            TotalTarget  = Items.Sum(g => g.TargetAmount);
            TotalCurrent = Items.Sum(g => g.InitialAmount + (g.Deposits?.Sum(d => d.Amount) ?? 0m));

            OverallProgressPct = (TotalTarget > 0)
                ? (int)Math.Round((TotalCurrent / TotalTarget) * 100m)
                : 0;

            Motivation = OverallProgressPct switch
            {
                >= 90 and < 100 => "¡Ya casi logras tu meta! 🎉",
                >= 50           => "Buen ritmo, no te detengas 🚀",
                > 0             => "Cada peso cuenta, ¡sigue así! 💪",
                _               => "Da el primer paso: crea tu primera meta ✨"
            };
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var ownerKey = GetOrCreateOwnerKey();

            if (id <= 0)
            {
                TempData["Flash"] = "Solicitud inválida.";
                return RedirectToPage();
            }

            var goal = await _db.Goals
                .Include(g => g.Deposits)
                .FirstOrDefaultAsync(g => g.Id == id && g.OwnerKey == ownerKey);

            if (goal is null)
            {
                TempData["Flash"] = "La meta no existe o no te pertenece.";
                return RedirectToPage();
            }

            if (goal.Deposits?.Count > 0)
                _db.Deposits.RemoveRange(goal.Deposits);

            _db.Goals.Remove(goal);
            await _db.SaveChangesAsync();

            TempData["Flash"] = "Meta eliminada correctamente.";
            return RedirectToPage();
        }

        private string GetOrCreateOwnerKey()
        {
            if (Request.Cookies.TryGetValue(OwnerCookieName, out var existing) &&
                !string.IsNullOrWhiteSpace(existing))
                return existing;

            var newKey = Convert.ToHexString(RandomNumberGenerator.GetBytes(16));
            Response.Cookies.Append(OwnerCookieName, newKey, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(2),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                IsEssential = true
            });
            return newKey;
        }
    }
}
