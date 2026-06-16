using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Rol
{
    public class EliminarModel : PageModel
    {
        private readonly IRolService _rolService;
        private readonly BitacoraService _bitacoraService;

        public EliminarModel(IRolService rolService, BitacoraService bitacoraService)
        {
            _rolService = rolService;
            _bitacoraService = bitacoraService;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {

            var rol = await _rolService.ObtenerPorIdAsync(id);

            var error = await _rolService.EliminarAsync(id);

            if (error != null)
            {
                TempData["Error"] = error;
                return RedirectToPage("Index");
            }

            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "Eliminación: " + JsonSerializer.Serialize(rol));
            TempData["Mensaje"] = "Rol eliminado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}