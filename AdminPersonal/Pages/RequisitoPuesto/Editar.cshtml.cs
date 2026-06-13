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
            var error = _service.ValidarYActualizar(Requisito);

            if (error != null)
            {
                ViewData["Error"] = error;
                Puestos = _service.ObtenerPuestos();
                return Page();
            }


            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "Actualización: Anterior=" + JsonSerializer.Serialize(anterior) + " Nuevo=" + JsonSerializer.Serialize(Requisito));
            TempData["Mensaje"] = "Requisito actualizado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}