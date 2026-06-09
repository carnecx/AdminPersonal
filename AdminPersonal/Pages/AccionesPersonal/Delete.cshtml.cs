using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdminPersonal.Entities;
using AdminPersonal.Repository;

namespace AdminPersonal.Pages.AccionesPersonal
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public AccionPersonal Accion { get; set; } = new AccionPersonal();

        public void OnGet(int id)
        {
            var accion = MemoryAccionPersonalRepository.GetById(id);
            if (accion != null)
            {
                Accion = accion;
            }
            else
            {
                TempData["ErrorMessage"] = "Acción no encontrada";
            }
        }

        public IActionResult OnPost()
        {
            MemoryAccionPersonalRepository.Delete(Accion.Id);
            TempData["SuccessMessage"] = "Acción eliminada exitosamente";
            return RedirectToPage("Index");
        }
    }
}
