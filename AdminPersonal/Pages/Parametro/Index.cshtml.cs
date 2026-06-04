using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Parametro
{
    public class IndexModel : PageModel
    {
        private readonly IParametroService _parametroService;

        public IndexModel(IParametroService parametroService)
        {
            _parametroService = parametroService;
            Parametros = new List<AdminPersonal.Entities.Parametro>();
        }

        public IEnumerable<AdminPersonal.Entities.Parametro> Parametros { get; set; }

        public async Task OnGetAsync()
        {
            Parametros = await _parametroService.ObtenerTodosAsync();
        }
    }
}