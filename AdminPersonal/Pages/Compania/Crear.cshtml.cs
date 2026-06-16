using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Compania
{
    public class CrearModel : PageModel
    {
        private readonly ICompaniaService _companiaService;
        private readonly BitacoraService _bitacoraService;

        public CrearModel(ICompaniaService companiaService, BitacoraService bitacoraService)
        {
            _companiaService = companiaService;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public AdminPersonal.Entities.Compania Compania { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var error = await _companiaService.ValidarYCrearAsync(Compania);

            if (error != null)
            {
                ViewData["Error"] = error;
                return Page();
            }

            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "Creación: " + JsonSerializer.Serialize(Compania));
            TempData["Mensaje"] = "Compañía creada exitosamente.";
            return RedirectToPage("Index");
        }
    }
}
