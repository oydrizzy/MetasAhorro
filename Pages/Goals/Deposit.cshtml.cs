using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MetasAhorro.Data;
using MetasAhorro.Models;

namespace MetasAhorro.Pages.Goals
{
    public class DepositModel : PageModel
    {
        private readonly AppDbContext _db;
        public DepositModel(AppDbContext db) => _db = db;

        [BindProperty] public Deposit Input { get; set; } = new();
        public Goal? Goal { get; set; }

        private const string OwnerCookieName = "MA.OwnerKey";
        private string GetOwnerKey() => Request.Cookies.TryGetValue(OwnerCookieName, out var k) ? k : "";

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var ownerKey = GetOwnerKey();

            Goal = await _db.Goals
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id && g.OwnerKey == ownerKey);

            if (Goal is null) return NotFound();

            Input.GoalId = id;
            Input.Date = DateTime.Today;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var ownerKey = GetOwnerKey();

            Input.Note = Input.Note?.Trim();
            if (Input.Date == default) Input.Date = DateTime.Today;
            else Input.Date = Input.Date.Date;

            var goal = await _db.Goals
                .Include(g => g.Deposits)
                .FirstOrDefaultAsync(g => g.Id == Input.GoalId && g.OwnerKey == ownerKey);

            if (goal is null)
            {
                ModelState.AddModelError(string.Empty, "La meta no existe o no te pertenece.");
                return Page();
            }

            if (Input.Amount <= 0)
                ModelState.AddModelError("Input.Amount", "El depósito debe ser mayor que 0.");

            var current   = goal.InitialAmount + (goal.Deposits?.Sum(d => d.Amount) ?? 0m);
            var remaining = goal.TargetAmount - current;

            if (remaining <= 0)
                ModelState.AddModelError("Input.Amount", "Esta meta ya está completada. No puedes depositar más.");
            else if (Input.Amount > remaining)
                ModelState.AddModelError("Input.Amount", $"El depósito supera el objetivo. Máximo permitido: RD$ {remaining:N2}.");

            if (!ModelState.IsValid)
            {
                Goal = goal;
                return Page();
            }

            _db.Deposits.Add(new Deposit
            {
                GoalId = Input.GoalId,
                Amount = Input.Amount,
                Date   = Input.Date,
                Note   = Input.Note
            });

            await _db.SaveChangesAsync();
            TempData["Flash"] = "Depósito agregado correctamente. 🐷";
            return RedirectToPage("View", new { id = Input.GoalId });
        }
    }
}
