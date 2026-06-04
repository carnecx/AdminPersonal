using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Pantalla
{
    public class EditarModel : PageModel
    {
        private readonly IPantallaService _service;

        public EditarModel(IPantallaService service)
        {
            _service = service;
        }

        [BindProperty]
        public AdminPersonal.Entities.Pantalla Pantalla { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var pantalla = _service.ObtenerPorId(id);
            if (pantalla == null) return NotFound();
            Pantalla = pantalla;
            return Page();
        }

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

            // Validar duplicado excluyendo el registro actual
            if (_service.BuscarDuplicadoEditar(Pantalla.nombre_pantalla, Pantalla.id_pantalla) != null)
            {
                ViewData["Error"] = "Ya existe una pantalla con ese nombre.";
                return Page();
            }

            _service.Actualizar(Pantalla);
            TempData["Mensaje"] = "Pantalla actualizada exitosamente.";
            return RedirectToPage("Index");
        }
    }
}
