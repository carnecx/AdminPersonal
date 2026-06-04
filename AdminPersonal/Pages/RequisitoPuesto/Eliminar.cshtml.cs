using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.RequisitoPuesto
{
    public class EliminarModel : PageModel
    {
        private readonly IRequisitoPuestoService _service;

        public EliminarModel(IRequisitoPuestoService service)
        {
            _service = service;
        }

        public IActionResult OnPost(int id)
        {
            _service.Eliminar(id);
            TempData["Mensaje"] = "Requisito eliminado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}
