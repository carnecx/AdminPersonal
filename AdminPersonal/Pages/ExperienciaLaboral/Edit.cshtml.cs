using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdminPersonal.Entities;
using AdminPersonal.Repository;

namespace AdminPersonal.Pages.ExperienciaLaboral
{
    public class EditModel : PageModel
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
            if (string.IsNullOrWhiteSpace(Experiencia.NombreEmpresa))
            {
                ModelState.AddModelError("", "El nombre de la empresa es requerido");
                return Page();
            }

            if (Experiencia.NombreEmpresa.Length > 100)
            {
                ModelState.AddModelError("", "El nombre de la empresa no puede superar los 100 caracteres");
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Experiencia.PuestoDesempenado))
            {
                ModelState.AddModelError("", "El puesto desempeñado es requerido");
                return Page();
            }

            if (Experiencia.PuestoDesempenado.Length > 100)
            {
                ModelState.AddModelError("", "El puesto desempeñado no puede superar los 100 caracteres");
                return Page();
            }

            if (Experiencia.FechaFin < Experiencia.FechaInicio)
            {
                ModelState.AddModelError("", "La fecha de fin debe ser mayor o igual a la fecha de inicio");
                return Page();
            }

            MemoryExperienciaLaboralRepository.Update(Experiencia);

            TempData["SuccessMessage"] = "Experiencia laboral actualizada correctamente";
            return RedirectToPage("Index", new { oferenteId = Experiencia.OferenteId });
        }
    }
}
