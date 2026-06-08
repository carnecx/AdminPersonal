using AdminPersonal.Entities;
using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Compania
{
    public class EliminarModel : PageModel
    {
        private readonly ICompaniaService _companiaService;
        private readonly BitacoraService _bitacoraService;

        public EliminarModel(ICompaniaService companiaService, BitacoraService bitacoraService)
        {
            _companiaService = companiaService;
            _bitacoraService = bitacoraService;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var compania = await _companiaService.ObtenerPorIdAsync(id);
            await _companiaService.EliminarAsync(id);
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "Eliminación: " + JsonSerializer.Serialize(compania));
            TempData["Mensaje"] = "Compañía eliminada exitosamente.";
            return RedirectToPage("Index");
        }
    }
}
