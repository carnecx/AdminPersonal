using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdminPersonal.Entities;
using AdminPersonal.Repository;

namespace AdminPersonal.Pages.Puestos
{
    public class DeleteModel : PageModel
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
            try
            {
                MemoryPuestoRepository.Delete(Puesto.Id);
                TempData["SuccessMessage"] = "Puesto eliminado exitosamente";
                return RedirectToPage("Index");
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
