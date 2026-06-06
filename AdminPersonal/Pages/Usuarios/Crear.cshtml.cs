using AdminPersonal.Entities;
using AdminPersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Usuarios
{
    public class CrearModel : PageModel
    {
        private readonly IUsuarioService _service;
        private int IdSesion => 1;

        [BindProperty]
        public Usuario Usuario { get; set; } = new();

        public IEnumerable<AdminPersonal.Entities.Rol> Roles { get; set; } = new List<AdminPersonal.Entities.Rol>();

        public CrearModel(IUsuarioService service)
        {
            _service = service;
        }

        public async Task OnGetAsync()
        {
            Roles = await _service.ObtenerRolesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Usuario.Contrasena))
                ModelState.AddModelError("Usuario.Contrasena", "La contraseña es requerida.");

            if (Usuario.RolesSeleccionados == null || !Usuario.RolesSeleccionados.Any())
                ModelState.AddModelError("", "Debe seleccionar al menos un rol.");

            if (!ModelState.IsValid)
            {
                Roles = await _service.ObtenerRolesAsync();
                return Page();
            }

            try
            {
                await _service.CrearAsync(Usuario, IdSesion);
                TempData["Exito"] = "Usuario creado correctamente.";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear usuario: " + ex.Message);
                Roles = await _service.ObtenerRolesAsync();
                return Page();
            }
        }
    }
}