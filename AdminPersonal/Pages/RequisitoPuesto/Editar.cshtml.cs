using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.RequisitoPuesto
{
    public class EditarModel : PageModel
    {
        private readonly IRequisitoPuestoService _service;
        private readonly BitacoraService _bitacoraService;

        public EditarModel(IRequisitoPuestoService service, BitacoraService bitacoraService)
        {
            _service = service;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public AdminPersonal.Entities.RequisitoPuesto Requisito { get; set; } = new();

        public IEnumerable<dynamic> Puestos { get; set; } = new List<dynamic>();

        public IActionResult OnGet(int id)
        {
            var requisito = _service.ObtenerPorId(id);
            if (requisito == null)
            {
                return NotFound();
            }
            Requisito = requisito;
            Puestos = _service.ObtenerPuestos();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var anterior = _service.ObtenerPorId(Requisito.id_requisito);
            if (Requisito.id_puesto <= 0)
            {
                ViewData["Error"] = "Debe seleccionar un puesto.";
                Puestos = _service.ObtenerPuestos();
                return Page();
            }
            if (string.IsNullOrWhiteSpace(Requisito.nombre_requisito))
            {
                ViewData["Error"] = "El nombre del requisito es obligatorio.";
                Puestos = _service.ObtenerPuestos();
                return Page();
            }
            if (_service.BuscarDuplicadoEditar(Requisito.nombre_requisito, Requisito.id_puesto, Requisito.id_requisito) != null)
            {
                ViewData["Error"] = "Ya existe ese requisito para este puesto.";
                Puestos = _service.ObtenerPuestos();
                return Page();
            }
            _service.Actualizar(Requisito);
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "Actualización: Anterior=" + JsonSerializer.Serialize(anterior) + " Nuevo=" + JsonSerializer.Serialize(Requisito));
            TempData["Mensaje"] = "Requisito actualizado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}