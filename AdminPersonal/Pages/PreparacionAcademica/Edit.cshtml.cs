using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdminPersonal.Entities;
using AdminPersonal.Repository;
using System.Collections.Generic;
using System.Linq;

namespace AdminPersonal.Pages.PreparacionAcademica
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public AdminPersonal.Entities.PreparacionAcademica Preparacion { get; set; } = new AdminPersonal.Entities.PreparacionAcademica();

        public List<InstitucionSimple> InstitucionesList { get; set; } = new List<InstitucionSimple>();

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
            InstitucionesList = MemoryPreparacionAcademicaRepository.GetInstitucionesList();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Preparacion.Titulo))
            {
                ModelState.AddModelError("", "El título es requerido");
                InstitucionesList = MemoryPreparacionAcademicaRepository.GetInstitucionesList();
                return Page();
            }

            if (Preparacion.Titulo.Length > 100)
            {
                ModelState.AddModelError("", "El título no puede superar los 100 caracteres");
                InstitucionesList = MemoryPreparacionAcademicaRepository.GetInstitucionesList();
                return Page();
            }

            if (Preparacion.FechaFin < Preparacion.FechaInicio)
            {
                ModelState.AddModelError("", "La fecha de fin debe ser mayor o igual a la fecha de inicio");
                InstitucionesList = MemoryPreparacionAcademicaRepository.GetInstitucionesList();
                return Page();
            }

            if (Preparacion.InstitucionEducativaId <= 0)
            {
                ModelState.AddModelError("", "Debe seleccionar una institución educativa");
                InstitucionesList = MemoryPreparacionAcademicaRepository.GetInstitucionesList();
                return Page();
            }

            var instituciones = MemoryPreparacionAcademicaRepository.GetInstitucionesList();
            Preparacion.NombreInstitucion = instituciones.FirstOrDefault(i => i.Id == Preparacion.InstitucionEducativaId)?.Nombre;

            MemoryPreparacionAcademicaRepository.Update(Preparacion);

            TempData["SuccessMessage"] = "Preparación académica actualizada correctamente";
            return RedirectToPage("Index", new { oferenteId = Preparacion.OferenteId });
        }
    }
}
