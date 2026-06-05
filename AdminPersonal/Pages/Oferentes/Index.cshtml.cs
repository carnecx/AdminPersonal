using AdminPersonal.Entities;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Oferentes
{
    public class IndexModel : PageModel
    {
        private readonly IOferenteService _service;
        public List<Oferente> Oferentes { get; set; } = new();
        public string Mensaje { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;

        public IndexModel(IOferenteService service)
        {
            _service = service;
        }

        public async Task OnGetAsync()
        {
            Mensaje = TempData["Mensaje"]?.ToString() ?? string.Empty;
            Error = TempData["Error"]?.ToString() ?? string.Empty;
            Oferentes = (await _service.ObtenerTodosAsync()).ToList();
        }
    }
}