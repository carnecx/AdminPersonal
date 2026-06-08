using AdminPersonal.Entities;
using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Compania
{
    public class IndexModel : PageModel
    {
        private readonly ICompaniaService _companiaService;
        private readonly BitacoraService _bitacoraService;

        public IndexModel(ICompaniaService companiaService, BitacoraService bitacoraService)
        {
            _companiaService = companiaService;
            Companias = new List<AdminPersonal.Entities.Compania>();
            _bitacoraService = bitacoraService;
        }

        public IEnumerable<AdminPersonal.Entities.Compania> Companias { get; set; }

        public async Task OnGetAsync()
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "El usuario consulta compañías");
            Companias = await _companiaService.ObtenerTodosAsync();
        }
    }
}