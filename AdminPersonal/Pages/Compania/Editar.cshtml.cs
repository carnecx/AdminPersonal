using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Compania
{
    public class EditarModel : PageModel
    {
        private readonly ICompaniaService _companiaService;

        public EditarModel(ICompaniaService companiaService)
        {
            _companiaService = companiaService;
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
            if (string.IsNullOrWhiteSpace(Compania.Codigo))
            {
                ViewData["Error"] = "El código es obligatorio.";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Compania.Nombre))
            {
                ViewData["Error"] = "El nombre es obligatorio.";
                return Page();
            }

            if (Compania.Nombre.Length > 150)
            {
                ViewData["Error"] = "El nombre no puede superar los 150 caracteres.";
                return Page();
            }

            if (await _companiaService.CodigoExisteAsync(Compania.Codigo, Compania.id_compania))
            {
                ViewData["Error"] = "Ya existe una compañía con ese código.";
                return Page();
            }

            await _companiaService.ActualizarAsync(Compania);
            TempData["Mensaje"] = "Compañía actualizada exitosamente.";
            return RedirectToPage("Index");
        }
    }
}