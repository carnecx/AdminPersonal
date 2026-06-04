using AdminPersonal.Entities;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Pantalla
{
    public class IndexModel : PageModel
    {
        private readonly IPantallaService _service;

        public IndexModel(IPantallaService service)
        {
            _service = service;
            Pantallas = new List<AdminPersonal.Entities.Pantalla>();
        }

        public IEnumerable<AdminPersonal.Entities.Pantalla> Pantallas { get; set; }

        public void OnGet()
        {
            Pantallas = _service.ObtenerTodos();
        }
    }
}
