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
            // Validar vacÌo
            if (string.IsNullOrWhiteSpace(Pantalla.nombre_pantalla))
            {
                ViewData["Error"] = "El nombre de la pantalla es obligatorio.";
                return Page();
            }

            // Validar longitud
            if (Pantalla.nombre_pantalla.Length > 100)
            {
                ViewData["Error"] = "El nombre de la pantalla no puede superar los 100 caracteres.";
                return Page();
            }

            // Validar solo letras y espacios
            if (!System.Text.RegularExpressions.Regex.IsMatch(Pantalla.nombre_pantalla,
                @"^[a-zA-Z·ÈÌÛ˙¡…Õ”⁄Ò— ]+$"))
            {
                ViewData["Error"] = "El nombre de la pantalla solo debe contener letras y espacios.";
                return Page();
            }

            // Validar duplicado
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
