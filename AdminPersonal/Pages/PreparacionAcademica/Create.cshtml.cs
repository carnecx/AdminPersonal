using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdminPersonal.Repository;
using AdminPersonal.Entities;
using System.Collections.Generic;
using System.Linq;
using PreparacionAcademicaEntity = AdminPersonal.Entities.PreparacionAcademica;

namespace AdminPersonal.Pages.PreparacionAcademica
{
    public class CreateModel : PageModel
    {
        [Microsoft.AspNetCore.Mvc.BindProperty]
        public PreparacionAcademicaEntity Preparacion { get; set; } = new PreparacionAcademicaEntity();

        public List<InstitucionSimple> InstitucionesList { get; set; } = new List<InstitucionSimple>();

        public int OferenteId { get; set; }

        public void OnGet(int oferenteId)
        {
            OferenteId = oferenteId;
            Preparacion.OferenteId = oferenteId;
            Preparacion.FechaInicio = System.DateTime.Today;
            Preparacion.FechaFin = System.DateTime.Today;
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

            MemoryPreparacionAcademicaRepository.Add(Preparacion);

            TempData["SuccessMessage"] = "Preparación académica registrada correctamente";
            return RedirectToPage("Index", new { oferenteId = Preparacion.OferenteId });
        }
    }
}
