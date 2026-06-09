using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using AdminPersonal.Entities;
using AdminPersonal.Repository;

namespace AdminPersonal.Pages.Entrevistas
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Entrevista Entrevista { get; set; } = new Entrevista();

        public List<OferenteItem> OferentesList { get; set; } = new List<OferenteItem>();
        public List<EmpleadoItem> EmpleadosList { get; set; } = new List<EmpleadoItem>();

        public void OnGet()
        {
            Entrevista.FechaEntrevista = System.DateTime.Now.AddDays(1);
            OferentesList = MemoryEntrevistaRepository.GetOferentesList();
            EmpleadosList = MemoryEntrevistaRepository.GetEmpleadosList();
        }

        public IActionResult OnPost()
        {
            if (Entrevista.OferenteId <= 0)
            {
                ModelState.AddModelError("", "Seleccione un oferente");
                CargarListas();
                return Page();
            }

            if (Entrevista.EmpleadoEntrevistadorId <= 0)
            {
                ModelState.AddModelError("", "Seleccione un empleado");
                CargarListas();
                return Page();
            }

            var oferentes = MemoryEntrevistaRepository.GetOferentesList();
            var empleados = MemoryEntrevistaRepository.GetEmpleadosList();
            Entrevista.NombreOferente = oferentes.FirstOrDefault(o => o.Id == Entrevista.OferenteId)?.Nombre;
            Entrevista.NombreEntrevistador = empleados.FirstOrDefault(e => e.Id == Entrevista.EmpleadoEntrevistadorId)?.Nombre;

            MemoryEntrevistaRepository.Add(Entrevista);
            TempData["SuccessMessage"] = "Entrevista agendada correctamente";
            return RedirectToPage("Index");
        }

        private void CargarListas()
        {
            OferentesList = MemoryEntrevistaRepository.GetOferentesList();
            EmpleadosList = MemoryEntrevistaRepository.GetEmpleadosList();
        }
    }
}
