using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MetasAhorro.Data;
using MetasAhorro.Models;
using System.Security.Cryptography;

namespace MetasAhorro.Pages.Goals
{
    public class NewModel : PageModel
    {
        private readonly AppDbContext _db;
        public NewModel(AppDbContext db) => _db = db;

        [BindProperty]
        public Goal Input { get; set; } = new();

        [TempData]
        public string? Flash { get; set; }

        private const string OwnerCookieName = "MA.OwnerKey";

        public void OnGet()
        {
            // Garantiza que el navegador ya tenga OwnerKey
            _ = GetOrCreateOwnerKey();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Asigna dueño anónimo por cookie
            Input.OwnerKey = GetOrCreateOwnerKey();

            // IMPORTANTE: el binder ya marcó OwnerKey como requerido.
            // Lo quitamos del ModelState para que no bloquee la validación.
            ModelState.Remove("Input.OwnerKey"); // <- clave del arreglo

            // Normalizaciones
            Input.Title = Input.Title?.Trim() ?? string.Empty;
            Input.Description = string.IsNullOrWhiteSpace(Input.Description) ? null : Input.Description.Trim();

            // Fechas "solo fecha"
            if (Input.StartDate == default) Input.StartDate = DateTime.Today;
            else Input.StartDate = Input.StartDate.Date;

            Input.Deadline = Input.Deadline.Date;

            // Validaciones de negocio
            if (string.IsNullOrWhiteSpace(Input.Title))
                ModelState.AddModelError(nameof(Input.Title), "El título es obligatorio.");

            if (Input.TargetAmount <= 0)
                ModelState.AddModelError(nameof(Input.TargetAmount), "El objetivo debe ser mayor que 0.");

            if (Input.InitialAmount < 0)
                ModelState.AddModelError(nameof(Input.InitialAmount), "El monto inicial no puede ser negativo.");

            if (Input.InitialAmount > Input.TargetAmount)
                ModelState.AddModelError(nameof(Input.InitialAmount), "El monto inicial no puede superar el objetivo.");

            if (Input.Deadline < DateTime.Today)
                ModelState.AddModelError(nameof(Input.Deadline), "La fecha límite no puede estar en el pasado.");

            if (Input.StartDate > Input.Deadline)
                ModelState.AddModelError(nameof(Input.StartDate), "La fecha de inicio no puede ser posterior a la fecha límite.");

            if (!ModelState.IsValid) return Page();

            _db.Goals.Add(Input);
            await _db.SaveChangesAsync();

            TempData["Flash"] = $"Meta \"{Input.Title}\" creada correctamente. 🎯";
            return RedirectToPage("Index");
        }

        private string GetOrCreateOwnerKey()
        {
            if (Request.Cookies.TryGetValue(OwnerCookieName, out var existing) &&
                !string.IsNullOrWhiteSpace(existing))
            {
                return existing;
            }

            var newKey = Convert.ToHexString(RandomNumberGenerator.GetBytes(16)); // 32 chars hex

            Response.Cookies.Append(OwnerCookieName, newKey, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(2),
                HttpOnly = true,
                Secure = Request.IsHttps,    // en http local no bloquea
                SameSite = SameSiteMode.Lax,
                IsEssential = true
            });

            return newKey;
        }
    }
}
