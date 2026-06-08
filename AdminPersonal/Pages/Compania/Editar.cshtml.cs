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

            // Validar código duplicado excluyendo el registro actual
            if (await _companiaService.CodigoExisteAsync(Compania.Codigo, Compania.id_compania))
            {
                ViewData["Error"] = "Ya existe una compañía con ese código.";
                return Page();
            }

            await _companiaService.ActualizarAsync(Compania);
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario,"Actualización: Anterior=" + JsonSerializer.Serialize(anterior) + " Nuevo=" + JsonSerializer.Serialize(Compania));
            TempData["Mensaje"] = "Compañía actualizada exitosamente.";
            return RedirectToPage("Index");
        }
    }
}
