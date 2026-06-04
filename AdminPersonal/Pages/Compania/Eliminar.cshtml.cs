using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Compania
{
    public class EliminarModel : PageModel
    {
        private readonly ICompaniaService _companiaService;

        public EliminarModel(ICompaniaService companiaService)
        {
            _companiaService = companiaService;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _companiaService.EliminarAsync(id);
            TempData["Mensaje"] = "Compañía eliminada exitosamente.";
            return RedirectToPage("Index");
        }
    }
}