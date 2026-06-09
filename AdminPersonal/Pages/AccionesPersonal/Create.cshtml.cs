using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using AdminPersonal.Entities;
using AdminPersonal.Repository;

namespace AdminPersonal.Pages.AccionesPersonal
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public AccionPersonal Accion { get; set; } = new AccionPersonal();

        public List<EmpleadoSimple> EmpleadosList { get; set; } = new List<EmpleadoSimple>();

        public void OnGet()
        {
            Accion.Fecha = System.DateTime.Today;
            EmpleadosList = MemoryAccionPersonalRepository.GetEmpleadosList();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Accion.Descripcion))
            {
                ModelState.AddModelError("", "La descripción es requerida");
                EmpleadosList = MemoryAccionPersonalRepository.GetEmpleadosList();
                return Page();
            }

            if (Accion.Descripcion.Length > 500)
            {
                ModelState.AddModelError("", "La descripción no puede superar los 500 caracteres");
                EmpleadosList = MemoryAccionPersonalRepository.GetEmpleadosList();
                return Page();
            }

            if (Accion.EmpleadoId <= 0)
            {
                ModelState.AddModelError("", "Debe seleccionar un empleado");
                EmpleadosList = MemoryAccionPersonalRepository.GetEmpleadosList();
                return Page();
            }

            if (Accion.JefaturaApruebaId <= 0)
            {
                ModelState.AddModelError("", "Debe seleccionar la jefatura que aprueba");
                EmpleadosList = MemoryAccionPersonalRepository.GetEmpleadosList();
                return Page();
            }

            var empleados = MemoryAccionPersonalRepository.GetEmpleadosList();
            Accion.NombreEmpleado = empleados.FirstOrDefault(e => e.Id == Accion.EmpleadoId)?.Nombre;
            Accion.NombreJefatura = empleados.FirstOrDefault(e => e.Id == Accion.JefaturaApruebaId)?.Nombre;

            MemoryAccionPersonalRepository.Add(Accion);

            TempData["SuccessMessage"] = "Acción de personal creada exitosamente";
            return RedirectToPage("Index");
        }
    }
}
