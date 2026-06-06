using AdminPersonal.Entities;
using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Concursos
{
    public class CrearModel : PageModel
    {
        private readonly IConcursoService _concursoService;
        private readonly BitacoraService _bitacoraService;

        public CrearModel(IConcursoService concursoService, BitacoraService bitacoraService)
        {
            _concursoService = concursoService;
            _bitacoraService = bitacoraService;
        }

        [BindProperty] public Concurso Concurso { get; set; } = new();

        public IActionResult OnGet() => Page();

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Concurso.Codigo))
            { ViewData["Error"] = "El código es obligatorio."; return Page(); }

            if (string.IsNullOrWhiteSpace(Concurso.Nombre))
            { ViewData["Error"] = "El nombre es obligatorio."; return Page(); }

            if (Concurso.FechaInicio == default)
            { ViewData["Error"] = "La fecha de inicio es obligatoria."; return Page(); }

            if (Concurso.FechaFin == default)
            { ViewData["Error"] = "La fecha de fin es obligatoria."; return Page(); }

            if (Concurso.FechaFin < Concurso.FechaInicio)
            { ViewData["Error"] = "La fecha de fin debe ser mayor o igual a la fecha de inicio."; return Page(); }

            if (await _concursoService.CodigoExisteAsync(Concurso.Codigo))
            { ViewData["Error"] = "Ya existe un concurso con ese código."; return Page(); }

            Concurso.Estado = "Vigente";
            await _concursoService.InsertarAsync(Concurso);

            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario,
                $"Crear Concurso: {JsonSerializer.Serialize(Concurso)}");

            TempData["Mensaje"] = "Concurso creado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}