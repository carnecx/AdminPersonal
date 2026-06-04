using AdminPersonal.Entities;
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
            if (string.IsNullOrWhiteSpace(Pantalla.nombre_pantalla))
            {
                ViewData["Error"] = "El nombre de la pantalla es obligatorio.";
                return Page();
            }

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
