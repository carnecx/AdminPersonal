using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Rol
{
    public class CrearModel : PageModel
    {
        private readonly IRolService _rolService;
        private readonly BitacoraService _bitacoraService;

        public CrearModel(IRolService rolService, BitacoraService bitacoraService)
        {
            _rolService = rolService;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public AdminPersonal.Entities.Rol Rol { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Rol.nombre_rol))
            {
                ViewData["Error"] = "El nombre del rol no puede estar vacÌo.";
                return Page();
            }
            if (Rol.nombre_rol.Length > 40)
            {
                ViewData["Error"] = "El nombre del rol no puede superar los 40 caracteres.";
                return Page();
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(Rol.nombre_rol, @"^[a-zA-Z·ÈÌÛ˙¡…Õ”⁄Ò— ]+$"))
            {
                ViewData["Error"] = "El nombre del rol solo debe contener letras y espacios.";
                return Page();
            }
            if (await _rolService.NombreExisteAsync(Rol.nombre_rol))
            {
                ViewData["Error"] = "El nombre del rol ya existe.";
                return Page();
            }
            await _rolService.InsertarAsync(Rol);
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "CreaciÛn: " + JsonSerializer.Serialize(Rol));
            TempData["Mensaje"] = "Rol creado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}