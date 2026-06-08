using AdminPersonal.Entities;
using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Pantalla
{
    public class IndexModel : PageModel
    {
        private readonly IPantallaService _service;
        private readonly BitacoraService _bitacoraService;

        public IndexModel(IPantallaService service, BitacoraService bitacoraService)
        {
            _service = service;
            _bitacoraService = bitacoraService;
            Pantallas = new List<AdminPersonal.Entities.Pantalla>();
        }

        public IEnumerable<AdminPersonal.Entities.Pantalla> Pantallas { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var pantallasStr = HttpContext.Session.GetString("PantallasRol") ?? "";
            var pantallas = pantallasStr.Split('|', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim().ToLower()).ToHashSet();

            if (!pantallas.Contains("pantallas"))
            {
                TempData["Error"] = "No tiene permisos para acceder a esta sección.";
                return RedirectToPage("/Home/Bienvenida");
            }
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "El usuario consulta pantallas");
            Pantallas = _service.ObtenerTodos();
            return Page();
        }
    }
}