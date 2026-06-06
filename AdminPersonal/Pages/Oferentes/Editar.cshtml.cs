using AdminPersonal.Entities;
using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Oferentes
{
    public class EditarModel : PageModel
    {
        private readonly IOferenteService _oferenteService;
        private readonly IConcursoService _concursoService;
        private readonly BitacoraService _bitacoraService;

        public EditarModel(IOferenteService oferenteService, IConcursoService concursoService,
                           BitacoraService bitacoraService)
        {
            _oferenteService = oferenteService;
            _concursoService = concursoService;
            _bitacoraService = bitacoraService;
        }

        [BindProperty] public Oferente Oferente { get; set; } = new();
        [BindProperty] public List<string> Correos { get; set; } = new();
        [BindProperty] public List<string> Telefonos { get; set; } = new();
        [BindProperty] public List<int> ConcursosSeleccionados { get; set; } = new();
        public IEnumerable<Concurso> Concursos { get; set; } = new List<Concurso>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var oferente = await _oferenteService.ObtenerPorIdAsync(id);
            if (oferente == null) return NotFound();

            Oferente = oferente;
            Correos = (await _oferenteService.ObtenerCorreosAsync(id)).ToList();
            Telefonos = (await _oferenteService.ObtenerTelefonosAsync(id)).ToList();
            Oferente.ConcursosIds = (await _oferenteService.ObtenerConcursosIdsAsync(id)).ToList();
            Concursos = await _concursoService.ObtenerTodosAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Concursos = await _concursoService.ObtenerTodosAsync();
            Correos = Correos.Where(c => !string.IsNullOrWhiteSpace(c)).ToList();
            Telefonos = Telefonos.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();

            if (string.IsNullOrWhiteSpace(Oferente.Identificacion))
            { ViewData["Error"] = "La identificación es obligatoria."; return Page(); }

            if (string.IsNullOrWhiteSpace(Oferente.TipoIdentificacion))
            { ViewData["Error"] = "El tipo de identificación es obligatorio."; return Page(); }

            if (string.IsNullOrWhiteSpace(Oferente.NombreCompleto))
            { ViewData["Error"] = "El nombre completo es obligatorio."; return Page(); }

            if (Correos.Count == 0)
            { ViewData["Error"] = "Debe ingresar al menos un correo electrónico."; return Page(); }

            foreach (var correo in Correos)
                if (!correo.Contains("@") || !correo.Contains("."))
                { ViewData["Error"] = $"El correo '{correo}' no tiene formato válido."; return Page(); }

            if (Telefonos.Count == 0)
            { ViewData["Error"] = "Debe ingresar al menos un teléfono."; return Page(); }

            if (await _oferenteService.IdentificacionExisteAsync(Oferente.Identificacion, Oferente.id_oferente))
            { ViewData["Error"] = "Ya existe otro oferente con esa identificación."; return Page(); }

            var anterior = await _oferenteService.ObtenerPorIdAsync(Oferente.id_oferente);
            Oferente.Correos = Correos;
            Oferente.Telefonos = Telefonos;
            Oferente.ConcursosIds = ConcursosSeleccionados;

            await _oferenteService.ActualizarAsync(Oferente);
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario,
                $"Actualizar Oferente - Anterior: {JsonSerializer.Serialize(anterior)} | Nuevo: {JsonSerializer.Serialize(new { Oferente.id_oferente, Oferente.Identificacion, Oferente.NombreCompleto })}");

            TempData["Mensaje"] = "Oferente actualizado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}