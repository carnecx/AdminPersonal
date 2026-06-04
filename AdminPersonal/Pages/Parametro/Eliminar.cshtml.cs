using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Parametro
{
    public class EliminarModel : PageModel
    {
        private readonly IParametroService _parametroService;

        public EliminarModel(IParametroService parametroService)
        {
            _parametroService = parametroService;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _parametroService.EliminarAsync(id);
            TempData["Mensaje"] = "Parámetro eliminado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}