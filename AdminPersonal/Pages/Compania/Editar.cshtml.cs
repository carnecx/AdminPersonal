using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Compania
{
    public class EditarModel : PageModel
    {
        private readonly ICompaniaService _companiaService;
        private readonly BitacoraService _bitacoraService;

        public EditarModel(ICompaniaService companiaService, BitacoraService bitacoraService)
        {
            _companiaService = companiaService;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public AdminPersonal.Entities.Compania Compania { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var compania = await _companiaService.ObtenerPorIdAsync(id);
            if (compania == null)
            {
                return NotFound();
            }
            Compania = compania;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var anterior = await _companiaService.ObtenerPorIdAsync(Compania.id_compania);

            var error = await _companiaService.ValidarYActualizarAsync(Compania);

            if (error != null)
            {
                ViewData["Error"] = error;
                return Page();
            }

            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario,"Actualización: Anterior=" + JsonSerializer.Serialize(anterior) + " Nuevo=" + JsonSerializer.Serialize(Compania));
            TempData["Mensaje"] = "Compañía actualizada exitosamente.";
            return RedirectToPage("Index");
        }
    }
}
