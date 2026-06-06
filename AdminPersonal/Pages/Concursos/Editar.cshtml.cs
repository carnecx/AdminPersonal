using AdminPersonal.Entities;
using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Concursos
{
    public class EditarModel : PageModel
    {
        private readonly IConcursoService _concursoService;
        private readonly BitacoraService _bitacoraService;

        public EditarModel(IConcursoService concursoService, BitacoraService bitacoraService)
        {
            _concursoService = concursoService;
            _bitacoraService = bitacoraService;
        }

        [BindProperty] public Concurso Concurso { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var concurso = await _concursoService.ObtenerPorIdAsync(id);
            if (concurso == null) return NotFound();
            Concurso = concurso;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Concurso.Codigo))
            { ViewData["Error"] = "El código es obligatorio."; return Page(); }

            if (string.IsNullOrWhiteSpace(Concurso.Nombre))
            { ViewData["Error"] = "El nombre es obligatorio."; return Page(); }

            if (Concurso.FechaFin < Concurso.FechaInicio)
            { ViewData["Error"] = "La fecha de fin debe ser mayor o igual a la fecha de inicio."; return Page(); }

            if (await _concursoService.CodigoExisteAsync(Concurso.Codigo, Concurso.id_concurso))
            { ViewData["Error"] = "Ya existe otro concurso con ese código."; return Page(); }

            var anterior = await _concursoService.ObtenerPorIdAsync(Concurso.id_concurso);
            Concurso.Estado = anterior?.Estado ?? "Vigente";
            await _concursoService.ActualizarAsync(Concurso);

            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario,
                $"Actualizar Concurso - Anterior: {JsonSerializer.Serialize(anterior)} | Nuevo: {JsonSerializer.Serialize(Concurso)}");

            TempData["Mensaje"] = "Concurso actualizado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}