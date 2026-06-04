using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.RequisitoPuesto
{
    public class CrearModel : PageModel
    {
        private readonly IRequisitoPuestoService _service;

        public CrearModel(IRequisitoPuestoService service)
        {
            _service = service;
        }

        [BindProperty]
        public AdminPersonal.Entities.RequisitoPuesto Requisito { get; set; } = new();

        public IEnumerable<dynamic> Puestos { get; set; } = new List<dynamic>();

        public IActionResult OnGet()
        {
            Puestos = _service.ObtenerPuestos();
            return Page();
        }

        public IActionResult OnPost()
        {
            // Validar que se seleccionó un puesto
            if (Requisito.id_puesto <= 0)
            {
                ViewData["Error"] = "Debe seleccionar un puesto.";
                Puestos = _service.ObtenerPuestos();
                return Page();
            }

            // Validar nombre obligatorio
            if (string.IsNullOrWhiteSpace(Requisito.nombre_requisito))
            {
                ViewData["Error"] = "El nombre del requisito es obligatorio.";
                Puestos = _service.ObtenerPuestos();
                return Page();
            }

            // Validar duplicado en el mismo puesto
            if (_service.BuscarDuplicado(Requisito.nombre_requisito, Requisito.id_puesto) != null)
            {
                ViewData["Error"] = "Ya existe ese requisito para este puesto.";
                Puestos = _service.ObtenerPuestos();
                return Page();
            }

            _service.Crear(Requisito);
            TempData["Mensaje"] = "Requisito creado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}
