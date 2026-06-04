using AdminPersonal.Entities;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.RequisitoPuesto
{
    public class IndexModel : PageModel
    {
        private readonly IRequisitoPuestoService _service;

        public IndexModel(IRequisitoPuestoService service)
        {
            _service = service;
            Requisitos = new List<AdminPersonal.Entities.RequisitoPuesto>();
        }

        public IEnumerable<AdminPersonal.Entities.RequisitoPuesto> Requisitos { get; set; }

        public void OnGet()
        {
            Requisitos = _service.ObtenerTodos();
        }
    }
}
