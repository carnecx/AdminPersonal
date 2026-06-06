using AdminPersonal.Entities;
using AdminPersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Usuarios
{
    public class EditarModel : PageModel
    {
        private readonly IUsuarioService _service;
        private int IdSesion => 1;

        [BindProperty]
        public Usuario Usuario { get; set; } = new();

        public IEnumerable<AdminPersonal.Entities.Rol> Roles { get; set; } = new List<AdminPersonal.Entities.Rol>();
        public EditarModel(IUsuarioService service)
        {
            _service = service;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var usuario = await _service.ObtenerPorIdAsync(id);
            if (usuario == null) return NotFound();
            Usuario = usuario;
            Roles = await _service.ObtenerRolesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Usuario.RolesSeleccionados == null || !Usuario.RolesSeleccionados.Any())
                ModelState.AddModelError("", "Debe seleccionar al menos un rol.");

            if (!ModelState.IsValid)
            {
                Roles = await _service.ObtenerRolesAsync();
                return Page();
            }

            try
            {
                await _service.ActualizarAsync(Usuario, IdSesion);
                TempData["Exito"] = "Usuario actualizado correctamente.";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar: " + ex.Message);
                Roles = await _service.ObtenerRolesAsync();
                return Page();
            }
        }
    }
}