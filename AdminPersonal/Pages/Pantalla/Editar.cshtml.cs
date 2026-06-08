using AdminPersonal.Entities;
using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Pantalla
{
    public class EditarModel : PageModel
    {
        private readonly IPantallaService _service;
        private readonly BitacoraService _bitacoraService;

        public EditarModel(IPantallaService service, BitacoraService bitacoraService)
        {
            _service = service;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public AdminPersonal.Entities.Pantalla Pantalla { get; set; } = new();

        public IEnumerable<RolPantalla> Roles { get; set; } = new List<RolPantalla>();

        [BindProperty]
        public List<int> RolesSeleccionados { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var pantallasStr = HttpContext.Session.GetString("PantallasRol") ?? "";
            var pantallas = pantallasStr.Split('|', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim().ToLower()).ToHashSet();
            if (!pantallas.Contains("pantallas"))
            {
                TempData["Error"] = "No tiene permisos para acceder a esta sección.";
                return RedirectToPage("/Home/Bienvenida");
            }
            var pantalla = _service.ObtenerPorId(id);
            if (pantalla == null) return NotFound();
            Pantalla = pantalla;
            Roles = _service.ObtenerRolesConAsignacion(id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var pantallasStr = HttpContext.Session.GetString("PantallasRol") ?? "";
            var pantallas = pantallasStr.Split('|', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim().ToLower()).ToHashSet();
            if (!pantallas.Contains("pantallas"))
            {
                TempData["Error"] = "No tiene permisos para acceder a esta sección.";
                return RedirectToPage("/Home/Bienvenida");
            }
            var anterior = _service.ObtenerPorId(Pantalla.id_pantalla);
            if (string.IsNullOrWhiteSpace(Pantalla.nombre_pantalla))
            {
                ViewData["Error"] = "El nombre de la pantalla es obligatorio.";
                Roles = _service.ObtenerRolesConAsignacion(Pantalla.id_pantalla);
                return Page();
            }
            if (Pantalla.nombre_pantalla.Length > 100)
            {
                ViewData["Error"] = "El nombre de la pantalla no puede superar los 100 caracteres.";
                Roles = _service.ObtenerRolesConAsignacion(Pantalla.id_pantalla);
                return Page();
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(Pantalla.nombre_pantalla, @"^[a-zA-Z ]+$"))
            {
                ViewData["Error"] = "El nombre de la pantalla solo debe contener letras y espacios.";
                Roles = _service.ObtenerRolesConAsignacion(Pantalla.id_pantalla);
                return Page();
            }
            if (_service.BuscarDuplicadoEditar(Pantalla.nombre_pantalla, Pantalla.id_pantalla) != null)
            {
                ViewData["Error"] = "Ya existe una pantalla con ese nombre.";
                Roles = _service.ObtenerRolesConAsignacion(Pantalla.id_pantalla);
                return Page();
            }
            _service.Actualizar(Pantalla);
            _service.EliminarAsignaciones(Pantalla.id_pantalla);
            if (RolesSeleccionados.Any())
            {
                _service.AsignarRoles(Pantalla.id_pantalla, RolesSeleccionados);
            }
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "Actualización: Anterior=" + JsonSerializer.Serialize(anterior) + " Nuevo=" + JsonSerializer.Serialize(Pantalla));
            TempData["Mensaje"] = "Pantalla actualizada exitosamente.";
            return RedirectToPage("Index");
        }
    }
}