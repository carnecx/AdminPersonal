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
            if (await _rolService.EstaAsignadoAUsuarioAsync(id))
            {
                TempData["Error"] = "No se puede eliminar un registro con datos relacionados.";
                return RedirectToPage("Index");
            }

            var rol = await _rolService.ObtenerPorIdAsync(id);
            await _rolService.EliminarAsync(id);
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "Eliminación: " + JsonSerializer.Serialize(rol));
            TempData["Mensaje"] = "Rol eliminado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}