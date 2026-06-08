using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Rol
{
    public class IndexModel : PageModel
    {
        private readonly IRolService _rolService;
        private readonly BitacoraService _bitacoraService;

        public IndexModel(IRolService rolService, BitacoraService bitacoraService)
        {
            _rolService = rolService;
            _bitacoraService = bitacoraService;
            Roles = new List<AdminPersonal.Entities.Rol>();
        }

        public IEnumerable<AdminPersonal.Entities.Rol> Roles { get; set; }

        public async Task OnGetAsync()
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "El usuario consulta roles");
            Roles = await _rolService.ObtenerTodosAsync();
        }
    }
}