using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.RequisitoPuesto
{
    public class EliminarModel : PageModel
    {
        private readonly IRequisitoPuestoService _service;
        private readonly BitacoraService _bitacoraService;

        public EliminarModel(IRequisitoPuestoService service, BitacoraService bitacoraService)
        {
            _service = service;
            _bitacoraService = bitacoraService;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var requisito = _service.ObtenerPorId(id);
            _service.Eliminar(id);
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "Eliminación: " + JsonSerializer.Serialize(requisito));
            TempData["Mensaje"] = "Requisito eliminado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}