using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MetasAhorro.Pages
{
    public class ContactModel : PageModel
    {
        [BindProperty]
        public string Nombre { get; set; } = "";

        [BindProperty]
        public string Correo { get; set; } = "";

        [BindProperty]
        public string Mensaje { get; set; } = "";

        public string? Confirmacion { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            Confirmacion = "Gracias por contactarnos, te responderemos pronto.";
        }
    }
}
