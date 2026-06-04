using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Parametro
{
    public class CrearModel : PageModel
    {
        private readonly IParametroService _parametroService;

        public CrearModel(IParametroService parametroService)
        {
            _parametroService = parametroService;
        }

        [BindProperty]
        public AdminPersonal.Entities.Parametro Parametro { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
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

            // Validar código duplicado
            if (await _parametroService.CodigoExisteAsync(Parametro.Codigo))
            {
                ViewData["Error"] = "Ya existe un parámetro con ese código.";
                return Page();
            }

            await _parametroService.InsertarAsync(Parametro);
            TempData["Mensaje"] = "Parámetro creado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}
