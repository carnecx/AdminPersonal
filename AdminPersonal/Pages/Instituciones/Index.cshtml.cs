using AdminPersonal.Entities;
using AdminPersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Instituciones
{
    public class IndexModel : PageModel
    {
        private readonly InstitucionService _institucionService;
        private readonly BitacoraService _bitacoraService;

        public IndexModel(InstitucionService institucionService, BitacoraService bitacoraService)
        {
            _institucionService = institucionService;
            _bitacoraService = bitacoraService;
        }

        public IEnumerable<InstitucionEducativa> Lista { get; set; } = new List<InstitucionEducativa>();

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetInt32("IdUsuario") == null)
                return RedirectToPage("/Account/Login", new { mensaje = "Por favor inicie sesión para utilizar el sistema" });

            Lista = await _institucionService.ObtenerTodosAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            if (HttpContext.Session.GetInt32("IdUsuario") == null)
                return RedirectToPage("/Account/Login", new { mensaje = "Por favor inicie sesión para utilizar el sistema" });

            var item = await _institucionService.ObtenerPorIdAsync(id);
            try
            {
                await _institucionService.EliminarAsync(id);
                int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 1;
                await _bitacoraService.RegistrarAsync(idUsuario, "Eliminacion institucion: " + JsonSerializer.Serialize(item));
                TempData["Mensaje"] = "Institucion eliminada correctamente.";
            }
            catch
            {
                TempData["Error"] = "No se puede eliminar un registro con datos relacionados.";
            }

            return RedirectToPage("/Instituciones/Index");
        }
    }
}
