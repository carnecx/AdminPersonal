using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Oferentes
{
    public class EliminarModel : PageModel
    {
        private readonly IOferenteService _oferenteService;
        private readonly BitacoraService _bitacoraService;

        public EliminarModel(IOferenteService oferenteService, BitacoraService bitacoraService)
        {
            _oferenteService = oferenteService;
            _bitacoraService = bitacoraService;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (await _oferenteService.TieneRelacionesAsync(id))
            {
                TempData["Error"] = "No se puede eliminar un registro con datos relacionados.";
                return RedirectToPage("Index");
            }

            var oferente = await _oferenteService.ObtenerPorIdAsync(id);
            await _oferenteService.EliminarAsync(id);

            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario,
                $"Eliminar Oferente: {JsonSerializer.Serialize(oferente)}");

            TempData["Mensaje"] = "Oferente eliminado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}