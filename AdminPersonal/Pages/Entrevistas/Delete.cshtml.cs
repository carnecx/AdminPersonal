using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdminPersonal.Entities;
using AdminPersonal.Repository;

namespace AdminPersonal.Pages.Entrevistas
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public Entrevista Entrevista { get; set; } = new Entrevista();

        public void OnGet(int id)
        {
            var entrevista = MemoryEntrevistaRepository.GetById(id);
            if (entrevista != null)
            {
                Entrevista = entrevista;
            }
        }

        public IActionResult OnPost()
        {
            MemoryEntrevistaRepository.Delete(Entrevista.Id);
            TempData["SuccessMessage"] = "Entrevista eliminada";
            return RedirectToPage("Index");
        }
    }
}
