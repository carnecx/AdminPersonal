using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdminPersonal.Entities;
using AdminPersonal.Repository;

namespace AdminPersonal.Pages.ExperienciaLaboral
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public AdminPersonal.Entities.ExperienciaLaboral Experiencia { get; set; } = new AdminPersonal.Entities.ExperienciaLaboral();

        public int OferenteId { get; set; }

        public void OnGet(int id, int oferenteId)
        {
            OferenteId = oferenteId;
            var experiencia = MemoryExperienciaLaboralRepository.GetById(id);
            if (experiencia != null)
            {
                Experiencia = experiencia;
            }
            else
            {
                TempData["ErrorMessage"] = "Registro no encontrado";
            }
        }

        public IActionResult OnPost()
        {
            int oferenteId = Experiencia.OferenteId;
            MemoryExperienciaLaboralRepository.Delete(Experiencia.Id);
            TempData["SuccessMessage"] = "Experiencia laboral eliminada correctamente";
            return RedirectToPage("Index", new { oferenteId = oferenteId });
        }
    }
}
