using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.RequisitoPuesto
{
    public class CrearModel : PageModel
    {
        private readonly IRequisitoPuestoService _service;
        private readonly BitacoraService _bitacoraService;

        public CrearModel(IRequisitoPuestoService service, BitacoraService bitacoraService)
        {
            _service = service;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public AdminPersonal.Entities.RequisitoPuesto Requisito { get; set; } = new();

        public IEnumerable<dynamic> Puestos { get; set; } = new List<dynamic>();

        public IActionResult OnGet()
        {
            Puestos = _service.ObtenerPuestos();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var error = _service.ValidarYCrear(Requisito);

            if (error != null)
            {
                ViewData["Error"] = error;
                Puestos = _service.ObtenerPuestos();
                return Page();
            }

            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "Creación: " + JsonSerializer.Serialize(Requisito));
            TempData["Mensaje"] = "Requisito creado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}