using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Parametro
{
    public class IndexModel : PageModel
    {
        private readonly IParametroService _parametroService;
        private readonly BitacoraService _bitacoraService;

        public IndexModel(IParametroService parametroService, BitacoraService bitacoraService)
        {
            _parametroService = parametroService;
            _bitacoraService = bitacoraService;
            Parametros = new List<AdminPersonal.Entities.Parametro>();
        }

        public IEnumerable<AdminPersonal.Entities.Parametro> Parametros { get; set; }

        public async Task OnGetAsync()
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "El usuario consulta parámetros");
            Parametros = await _parametroService.ObtenerTodosAsync();
        }
    }
}