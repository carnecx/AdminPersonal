using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Pantalla
{
    public class EliminarModel : PageModel
    {
        private readonly IPantallaService _service;

        public EliminarModel(IPantallaService service)
        {
            _service = service;
        }

        public IActionResult OnPost(int id)
        {
            if (_service.EstaAsignada(id))
            {
                TempData["Error"] = "No se puede eliminar pantalla ya que esta asignada a uno o mas roles.";
                return RedirectToPage("Index");
            }

            _service.Eliminar(id);
            TempData["Mensaje"] = "Pantalla eliminada exitosamente.";
            return RedirectToPage("Index");
        }
    }
}
