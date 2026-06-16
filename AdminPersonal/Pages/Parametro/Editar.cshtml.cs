using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Parametro
{
    public class EditarModel : PageModel
    {
        private readonly IParametroService _parametroService;
        private readonly BitacoraService _bitacoraService;

        public EditarModel(IParametroService parametroService, BitacoraService bitacoraService)
        {
            _parametroService = parametroService;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public AdminPersonal.Entities.Parametro Parametro { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var parametro = await _parametroService.ObtenerPorIdAsync(id);
            if (parametro == null)
            {
                return NotFound();
            }
            Parametro = parametro;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var anterior = await _parametroService.ObtenerPorIdAsync(Parametro.id_parametro);

            var error = await _parametroService.ValidarYActualizarAsync(Parametro);

            if (error != null)
            {
                ViewData["Error"] = error;
                return Page();
            }

            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "Actualización: Anterior=" + JsonSerializer.Serialize(anterior) + " Nuevo=" + JsonSerializer.Serialize(Parametro));
            TempData["Mensaje"] = "Parámetro actualizado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}
