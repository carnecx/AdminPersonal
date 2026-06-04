using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Rol
{
    public class CrearModel : PageModel
    {
        private readonly IRolService _rolService;

        public CrearModel(IRolService rolService)
        {
            _rolService = rolService;
        }

        [BindProperty]
        public AdminPersonal.Entities.Rol Rol { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validar vacÌo primero
            if (string.IsNullOrWhiteSpace(Rol.nombre_rol))
            {
                ViewData["Error"] = "El nombre del rol no puede estar vacÌo.";
                return Page();
            }

            // Validar longitud
            if (Rol.nombre_rol.Length > 40)
            {
                ViewData["Error"] = "El nombre del rol no puede superar los 40 caracteres.";
                return Page();
            }

            // Validar solo letras y espacios
            if (!System.Text.RegularExpressions.Regex.IsMatch(Rol.nombre_rol, @"^[a-zA-Z·ÈÌÛ˙¡…Õ”⁄Ò— ]+$"))
            {
                ViewData["Error"] = "El nombre del rol solo debe contener letras y espacios.";
                return Page();
            }

            // Validar duplicado
            if (await _rolService.NombreExisteAsync(Rol.nombre_rol))
            {
                ViewData["Error"] = "El nombre del rol ya existe.";
                return Page();
            }

            await _rolService.InsertarAsync(Rol);
            TempData["Mensaje"] = "Rol creado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}
