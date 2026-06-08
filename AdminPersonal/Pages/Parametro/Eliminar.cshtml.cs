using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Parametro
{
    public class EliminarModel : PageModel
    {
        private readonly IParametroService _parametroService;
        private readonly BitacoraService _bitacoraService;

        public EliminarModel(IParametroService parametroService, BitacoraService bitacoraService)
        {
            _parametroService = parametroService;
            _bitacoraService = bitacoraService;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var parametro = await _parametroService.ObtenerPorIdAsync(id);
            await _parametroService.EliminarAsync(id);
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "Eliminación: " + JsonSerializer.Serialize(parametro));
            TempData["Mensaje"] = "Parámetro eliminado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}