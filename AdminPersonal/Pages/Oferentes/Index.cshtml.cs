using AdminPersonal.Entities;
using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Oferentes
{
    public class IndexModel : PageModel
    {
        private readonly IOferenteService _oferenteService;
        private readonly BitacoraService _bitacoraService;

        public IndexModel(IOferenteService oferenteService, BitacoraService bitacoraService)
        {
            _oferenteService = oferenteService;
            _bitacoraService = bitacoraService;
            Oferentes = new List<Oferente>();
        }

        public IEnumerable<Oferente> Oferentes { get; set; }

        public async Task OnGetAsync()
        {
            Oferentes = await _oferenteService.ObtenerTodosAsync();
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "El usuario consulta Oferentes");
        }
    }
}