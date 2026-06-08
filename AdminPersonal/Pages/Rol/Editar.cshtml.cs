using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Rol
{
    public class EditarModel : PageModel
    {
        private readonly IRolService _rolService;
        private readonly BitacoraService _bitacoraService;

        public EditarModel(IRolService rolService, BitacoraService bitacoraService)
        {
            _rolService = rolService;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public AdminPersonal.Entities.Rol Rol { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var rol = await _rolService.ObtenerPorIdAsync(id);
            if (rol == null)
            {
                return NotFound();
            }
            Rol = rol;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var anterior = await _rolService.ObtenerPorIdAsync(Rol.id_rol);
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
            if (await _rolService.NombreExisteAsync(Rol.nombre_rol, Rol.id_rol))
            {
                ViewData["Error"] = "El nombre del rol ya existe.";
                return Page();
            }
            await _rolService.ActualizarAsync(Rol);
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "ActualizaciÛn: Anterior=" + JsonSerializer.Serialize(anterior) + " Nuevo=" + JsonSerializer.Serialize(Rol));
            TempData["Mensaje"] = "Rol actualizado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}