using AdminPersonal.Entities;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Pantalla
{
    public class CrearModel : PageModel
    {
        private readonly IPantallaService _service;

        public CrearModel(IPantallaService service)
        {
            _service = service;
        }

        [BindProperty]
        public AdminPersonal.Entities.Pantalla Pantalla { get; set; } = new();

        public IActionResult OnGet() => Page();

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Pantalla.nombre_pantalla))
            {
                ViewData["Error"] = "El nombre de la pantalla es obligatorio.";
                return Page();
            }

            if (_service.BuscarDuplicado(Pantalla.nombre_pantalla) != null)
            {
                ViewData["Error"] = "Ya existe una pantalla con ese nombre.";
                return Page();
            }

            _service.Crear(Pantalla);
            TempData["Mensaje"] = "Pantalla creada exitosamente.";
            return RedirectToPage("Index");
        }
    }
}
