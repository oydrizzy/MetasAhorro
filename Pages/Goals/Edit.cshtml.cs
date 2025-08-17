using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MetasAhorro.Data;
using MetasAhorro.Models;

namespace MetasAhorro.Pages.Goals
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _db;
        public EditModel(AppDbContext db) => _db = db;

        [BindProperty]
        public GoalInput Input { get; set; } = new();

        [TempData]
        public string? Flash { get; set; }

        private const string OwnerCookieName = "MA.OwnerKey";
        private string GetOwnerKey() =>
            Request.Cookies.TryGetValue(OwnerCookieName, out var k) && !string.IsNullOrWhiteSpace(k) ? k : string.Empty;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var ownerKey = GetOwnerKey();

            var g = await _db.Goals
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.OwnerKey == ownerKey);

            if (g is null)
            {
                TempData["Flash"] = "La meta no existe o no te pertenece.";
                return RedirectToPage("Index");
            }

            Input = new GoalInput
            {
                Id = g.Id,
                Title = g.Title,
                Description = g.Description,
                TargetAmount = g.TargetAmount,
                InitialAmount = g.InitialAmount,
                Deadline = g.Deadline
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Normaliza
            Input.Title = Input.Title?.Trim() ?? string.Empty;
            Input.Deadline = Input.Deadline.Date;

            // Validaciones de DataAnnotations primero
            if (!ModelState.IsValid) return Page();

            var ownerKey = GetOwnerKey();

            // Trae con depósitos para verificar coherencias reales
            var g = await _db.Goals
                .Include(x => x.Deposits)
                .FirstOrDefaultAsync(x => x.Id == Input.Id && x.OwnerKey == ownerKey);

            if (g is null)
            {
                TempData["Flash"] = "La meta no existe o no te pertenece.";
                return RedirectToPage("Index");
            }

            // --- Validaciones de negocio ---
            // 1) Inicial no puede superar el objetivo
            if (Input.InitialAmount > Input.TargetAmount)
                ModelState.AddModelError(nameof(Input.InitialAmount), "El monto inicial no puede superar el objetivo.");

            // 2) Objetivo no puede ser menor a (nuevo inicial + depósitos)
            var depositsSum = g.Deposits?.Sum(d => d.Amount) ?? 0m;
            var currentWithNewInitial = Input.InitialAmount + depositsSum;
            if (Input.TargetAmount < currentWithNewInitial)
                ModelState.AddModelError(nameof(Input.TargetAmount),
                    $"El objetivo no puede ser menor a lo ya ahorrado con el inicial nuevo (RD$ {currentWithNewInitial:N2}).");

            // 3) Fecha límite no en el pasado
            if (Input.Deadline < DateTime.Today)
                ModelState.AddModelError(nameof(Input.Deadline), "La fecha límite no puede estar en el pasado.");

            if (!ModelState.IsValid) return Page();
            // --- Fin validaciones ---

            // Mapear campos permitidos
            g.Title = Input.Title;
            g.Description = Input.Description;
            g.TargetAmount = Input.TargetAmount;
            g.InitialAmount = Input.InitialAmount;
            g.Deadline = Input.Deadline;

            await _db.SaveChangesAsync();

            TempData["Flash"] = $"Meta \"{g.Title}\" actualizada correctamente. ✅";
            return RedirectToPage("View", new { id = g.Id });
        }

        public class GoalInput
        {
            [Required]
            public int Id { get; set; }

            [Required(ErrorMessage = "El título es obligatorio.")]
            [StringLength(80, ErrorMessage = "El título no debe exceder los 80 caracteres.")]
            public string Title { get; set; } = string.Empty;

            [StringLength(500, ErrorMessage = "La descripción no debe exceder los 500 caracteres.")]
            public string? Description { get; set; }

            [Range(0.01, 999_999_999, ErrorMessage = "El objetivo debe ser mayor que 0.")]
            public decimal TargetAmount { get; set; }

            [Range(0, 999_999_999, ErrorMessage = "El monto inicial no puede ser negativo.")]
            public decimal InitialAmount { get; set; }

            [DataType(DataType.Date)]
            public DateTime Deadline { get; set; }
        }
    }
}
