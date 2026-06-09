using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdminPersonal.Entities;
using AdminPersonal.Repository;

namespace AdminPersonal.Pages.Puestos
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Puesto Puesto { get; set; } = new Puesto();

        public void OnGet(int id)
        {
            var puesto = MemoryPuestoRepository.GetById(id);
            if (puesto != null) Puesto = puesto;
            else TempData["ErrorMessage"] = "Puesto no encontrado";
        }

        public IActionResult OnPost()
        {
            if (Puesto.JefaturaPuestoId == 0) Puesto.JefaturaPuestoId = null;

            if (Puesto.JefaturaPuestoId.HasValue)
            {
                var jefeExiste = MemoryPuestoRepository.GetById(Puesto.JefaturaPuestoId.Value) != null;
                if (!jefeExiste)
                {
                    ModelState.AddModelError("", "El ID de Jefatura ingresado no existe.");
                    return Page();
                }
            }

            if (string.IsNullOrWhiteSpace(Puesto.Codigo))
            {
                ModelState.AddModelError("", "El código es requerido");
                return Page();
            }

            // Validar que el código no exista previamente en otro puesto
            var exist = MemoryPuestoRepository.GetAll().Any(p => p.Id != Puesto.Id && p.Codigo.Equals(Puesto.Codigo, System.StringComparison.OrdinalIgnoreCase));
            if (exist)
            {
                ModelState.AddModelError("", "El código del puesto ya está registrado.");
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Puesto.Nombre))
            {
                ModelState.AddModelError("", "El nombre es requerido");
                return Page();
            }

            if (Puesto.Salario <= 0)
            {
                ModelState.AddModelError("", "El salario debe ser mayor a 0");
                return Page();
            }

            MemoryPuestoRepository.Update(Puesto);
            TempData["SuccessMessage"] = "Puesto actualizado exitosamente";
            return RedirectToPage("Index");
        }
    }
}
