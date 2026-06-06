using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Concursos
{
    public class EliminarModel : PageModel
    {
        private readonly IConcursoService _concursoService;
        private readonly BitacoraService _bitacoraService;

        public EliminarModel(IConcursoService concursoService, BitacoraService bitacoraService)
        {
            _concursoService = concursoService;
            _bitacoraService = bitacoraService;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (await _concursoService.TieneRelacionesAsync(id))
            {
                TempData["Error"] = "No se puede eliminar un registro con datos relacionados.";
                return RedirectToPage("Index");
            }

            var concurso = await _concursoService.ObtenerPorIdAsync(id);
            await _concursoService.EliminarAsync(id);

            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario,
                $"Eliminar Concurso: {JsonSerializer.Serialize(concurso)}");

            TempData["Mensaje"] = "Concurso eliminado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}