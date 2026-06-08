using AdminPersonal.Entities;
using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.RequisitoPuesto
{
    public class IndexModel : PageModel
    {
        private readonly IRequisitoPuestoService _service;
        private readonly BitacoraService _bitacoraService;

        public IndexModel(IRequisitoPuestoService service, BitacoraService bitacoraService)
        {
            _service = service;
            _bitacoraService = bitacoraService;
            Requisitos = new List<AdminPersonal.Entities.RequisitoPuesto>();
        }

        public IEnumerable<AdminPersonal.Entities.RequisitoPuesto> Requisitos { get; set; }

        public async Task OnGetAsync()
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "El usuario consulta requisitos de puestos");
            Requisitos = _service.ObtenerTodos();
        }
    }
}