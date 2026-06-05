using AdminPersonal.Entities;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Pantalla
{
    public class IndexModel : PageModel
    {
        private readonly IPantallaService _service;

        public IndexModel(IPantallaService service)
        {
            _service = service;
            Pantallas = new List<AdminPersonal.Entities.Pantalla>();
        }

        public IEnumerable<AdminPersonal.Entities.Pantalla> Pantallas { get; set; }

        public IActionResult OnGet()
        {
            var pantallasStr = HttpContext.Session.GetString("PantallasRol") ?? "";
            var pantallas = pantallasStr.Split('|', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim().ToLower()).ToHashSet();

            if (!pantallas.Contains("pantallas"))
            {
                TempData["Error"] = "No tiene permisos para acceder a esta sección.";
                return RedirectToPage("/Home/Bienvenida");
            }

            Pantallas = _service.ObtenerTodos();
            return Page();
        }
    }
}
