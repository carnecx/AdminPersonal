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
            var error = await _rolService.ValidarYActualizarAsync(Rol);

            if (error != null)
            {
                ViewData["Error"] = error;
                return Page();
            }


            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "Actualización: Anterior=" + JsonSerializer.Serialize(anterior) + " Nuevo=" + JsonSerializer.Serialize(Rol));
            TempData["Mensaje"] = "Rol actualizado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}