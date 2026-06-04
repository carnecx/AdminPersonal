using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Compania
{
    public class CrearModel : PageModel
    {
        private readonly ICompaniaService _companiaService;

        public CrearModel(ICompaniaService companiaService)
        {
            _companiaService = companiaService;
        }

        [BindProperty]
        public AdminPersonal.Entities.Compania Compania { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validar código obligatorio
            if (string.IsNullOrWhiteSpace(Compania.Codigo))
            {
                ViewData["Error"] = "El código es obligatorio.";
                return Page();
            }

            // Validar nombre obligatorio
            if (string.IsNullOrWhiteSpace(Compania.Nombre))
            {
                ViewData["Error"] = "El nombre es obligatorio.";
                return Page();
            }

            // Validar longitud del nombre
            if (Compania.Nombre.Length > 150)
            {
                ViewData["Error"] = "El nombre no puede superar los 150 caracteres.";
                return Page();
            }

            // Validar código duplicado
            if (await _companiaService.CodigoExisteAsync(Compania.Codigo))
            {
                ViewData["Error"] = "Ya existe una compañía con ese código.";
                return Page();
            }

            await _companiaService.InsertarAsync(Compania);
            TempData["Mensaje"] = "Compañía creada exitosamente.";
            return RedirectToPage("Index");
        }
    }
}
