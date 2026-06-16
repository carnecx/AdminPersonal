using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Parametro
{
    public class CrearModel : PageModel
    {
        private readonly IParametroService _parametroService;
        private readonly BitacoraService _bitacoraService;

        public CrearModel(IParametroService parametroService, BitacoraService bitacoraService)
        {
            _parametroService = parametroService;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public AdminPersonal.Entities.Parametro Parametro { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var error = await _parametroService.ValidarYCrearAsync(Parametro);

            if (error != null)
            {
                ViewData["Error"] = error;
                return Page();
            }

            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "Creación: " + JsonSerializer.Serialize(Parametro));
            TempData["Mensaje"] = "Parámetro creado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}
