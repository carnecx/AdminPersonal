using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Concursos
{
    public class CambiarEstadoModel : PageModel
    {
        private readonly IConcursoService _concursoService;
        private readonly BitacoraService _bitacoraService;

        public CambiarEstadoModel(IConcursoService concursoService, BitacoraService bitacoraService)
        {
            _concursoService = concursoService;
            _bitacoraService = bitacoraService;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var concurso = await _concursoService.ObtenerPorIdAsync(id);
            if (concurso == null) return NotFound();

            concurso.Estado = concurso.Estado == "Vigente" ? "Vencido" : "Vigente";
            await _concursoService.ActualizarAsync(concurso);

            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario,
                $"Cambiar estado Concurso id={id} a '{concurso.Estado}'");

            TempData["Mensaje"] = $"El concurso fue marcado como {concurso.Estado}.";
            return RedirectToPage("Index");
        }
    }
}