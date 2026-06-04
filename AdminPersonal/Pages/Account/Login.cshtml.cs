using AdminPersonal.Entities;
using AdminPersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly UsuarioService _usuarioService;
        private readonly BitacoraService _bitacoraService;

        public LoginModel(UsuarioService usuarioService, BitacoraService bitacoraService)
        {
            _usuarioService = usuarioService;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public LoginViewModel Datos { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Mensaje { get; set; }

        public string? Error { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Error = "Usuario o contraseña incorrectos.";
                return Page();
            }

            var usuario = await _usuarioService.BuscarPorUsuarioAsync(Datos.NombreUsuario);

            if (usuario == null)
            {
                Error = "Usuario y/o contraseña incorrectos.";
                return Page();
            }

            if (usuario.estado == "Bloqueado")
            {
                Error = "El usuario se encuentra bloqueado.";
                return Page();
            }

            if (usuario.estado == "Inactivo")
            {
                Error = "El usuario se encuentra inactivo.";
                return Page();
            }

            if (!_usuarioService.ValidarPassword(Datos.Contrasena, usuario.contrasena))
            {
                await _usuarioService.RegistrarFalloAsync(usuario);
                Error = "Usuario y/o contraseña incorrectos.";
                return Page();
            }

            await _usuarioService.ReiniciarIntentosAsync(usuario.id_usuario);

            HttpContext.Session.SetInt32("IdUsuario", usuario.id_usuario);
            HttpContext.Session.SetString("NombreUsuario", usuario.nombre_usuario);
            HttpContext.Session.SetString("NombreCompleto", usuario.nombre_completo);

            var rol = await _usuarioService.ObtenerRolAsync(usuario.id_usuario);
            if (!string.IsNullOrEmpty(rol))
                HttpContext.Session.SetString("RolUsuario", rol);

            await _bitacoraService.RegistrarAsync(usuario.id_usuario, "El usuario inicio sesion");

            return RedirectToPage("/Home/Bienvenida");
        }
    }
}
