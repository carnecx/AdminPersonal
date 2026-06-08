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

            // Validar código obligatorio
            if (string.IsNullOrWhiteSpace(Parametro.Codigo))
            {
                ViewData["Error"] = "El código es obligatorio.";
                return Page();
            }

            // Validar valor obligatorio
            if (string.IsNullOrWhiteSpace(Parametro.Valor))
            {
                ViewData["Error"] = "El valor es obligatorio.";
                return Page();
            }

            // Validar longitud del valor
            if (Parametro.Valor.Length > 500)
            {
                ViewData["Error"] = "El valor no puede superar los 500 caracteres.";
                return Page();
            }

            // Validar código duplicado excluyendo el registro actual
            if (await _parametroService.CodigoExisteAsync(Parametro.Codigo, Parametro.id_parametro))
            {
                ViewData["Error"] = "Ya existe un parámetro con ese código.";
                return Page();
            }

            await _parametroService.ActualizarAsync(Parametro);
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "Actualización: Anterior=" + JsonSerializer.Serialize(anterior) + " Nuevo=" + JsonSerializer.Serialize(Parametro));
            TempData["Mensaje"] = "Parámetro actualizado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}
