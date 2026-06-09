using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using AdminPersonal.Entities;
using AdminPersonal.Repository;

namespace AdminPersonal.Pages.Entrevistas
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Entrevista Entrevista { get; set; } = new Entrevista();

        public List<EmpleadoItem> EmpleadosList { get; set; } = new List<EmpleadoItem>();

        public void OnGet(int id)
        {
            var entrevista = MemoryEntrevistaRepository.GetById(id);
            if (entrevista != null)
            {
                Entrevista = entrevista;
            }
            EmpleadosList = MemoryEntrevistaRepository.GetEmpleadosList();
        }

        public IActionResult OnPost()
        {
            if (Entrevista.EmpleadoEntrevistadorId <= 0)
            {
                EmpleadosList = MemoryEntrevistaRepository.GetEmpleadosList();
                return Page();
            }

            var empleados = MemoryEntrevistaRepository.GetEmpleadosList();
            Entrevista.NombreEntrevistador = empleados.FirstOrDefault(e => e.Id == Entrevista.EmpleadoEntrevistadorId)?.Nombre;

            MemoryEntrevistaRepository.Update(Entrevista);
            TempData["SuccessMessage"] = "Entrevista actualizada";
            return RedirectToPage("Index");
        }
    }
}
