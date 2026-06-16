using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Pantalla
{
    public class EliminarModel : PageModel
    {
        private readonly IPantallaService _service;
        private readonly BitacoraService _bitacoraService;

        public EliminarModel(IPantallaService service, BitacoraService bitacoraService)
        {
            _service = service;
            _bitacoraService = bitacoraService;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var pantallasStr = HttpContext.Session.GetString("PantallasRol") ?? "";
            var pantallas = pantallasStr.Split('|', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim().ToLower()).ToHashSet();
            if (!pantallas.Contains("pantallas"))
            {
                TempData["Error"] = "No tiene permisos para realizar esta acción.";
                return RedirectToPage("/Home/Bienvenida");
            }

            var pantalla = _service.ObtenerPorId(id);

            var error = _service.Eliminar(id);

            if (error != null)
            {
                TempData["Error"] = error;
                return RedirectToPage("Index");
            }

            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "Eliminación: " + JsonSerializer.Serialize(pantalla));
            TempData["Mensaje"] = "Pantalla eliminada exitosamente.";
            return RedirectToPage("Index");
        }
    }
}