using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdminPersonal.Entities;
using AdminPersonal.Repository;

namespace AdminPersonal.Pages.PreparacionAcademica
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public AdminPersonal.Entities.PreparacionAcademica Preparacion { get; set; } = new AdminPersonal.Entities.PreparacionAcademica();

        public int OferenteId { get; set; }

        public void OnGet(int id, int oferenteId)
        {
            OferenteId = oferenteId;
            var preparacion = MemoryPreparacionAcademicaRepository.GetById(id);
            if (preparacion != null)
            {
                Preparacion = preparacion;
            }
            else
            {
                TempData["ErrorMessage"] = "Registro no encontrado";
            }
        }

        public IActionResult OnPost()
        {
            int oferenteId = Preparacion.OferenteId;
            MemoryPreparacionAcademicaRepository.Delete(Preparacion.Id);
            TempData["SuccessMessage"] = "Preparación académica eliminada correctamente";
            return RedirectToPage("Index", new { oferenteId = oferenteId });
        }
    }
}
