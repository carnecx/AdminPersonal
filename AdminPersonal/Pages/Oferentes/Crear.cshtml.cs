using AdminPersonal.Entities;
using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Oferentes
{
    public class CrearModel : PageModel
    {
        private readonly IOferenteService _oferenteService;
        private readonly IConcursoService _concursoService;
        private readonly BitacoraService _bitacoraService;

        public CrearModel(IOferenteService oferenteService, IConcursoService concursoService,
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

        public async Task<IActionResult> OnGetAsync()
        {
            Concursos = await _concursoService.ObtenerVigentesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Concursos = await _concursoService.ObtenerVigentesAsync();
            Correos = Correos.Where(c => !string.IsNullOrWhiteSpace(c)).ToList();
            Telefonos = Telefonos.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();

            if (string.IsNullOrWhiteSpace(Oferente.Identificacion))
            { ViewData["Error"] = "La identificación es obligatoria."; return Page(); }

            if (string.IsNullOrWhiteSpace(Oferente.TipoIdentificacion))
            { ViewData["Error"] = "El tipo de identificación es obligatorio."; return Page(); }

            if (string.IsNullOrWhiteSpace(Oferente.NombreCompleto))
            { ViewData["Error"] = "El nombre completo es obligatorio."; return Page(); }

            if (Oferente.FechaNacimiento == default)
            { ViewData["Error"] = "La fecha de nacimiento es obligatoria."; return Page(); }

            if (Correos.Count == 0)
            { ViewData["Error"] = "Debe ingresar al menos un correo electrónico."; return Page(); }

            foreach (var correo in Correos)
                if (!correo.Contains("@") || !correo.Contains("."))
                { ViewData["Error"] = $"El correo '{correo}' no tiene formato válido."; return Page(); }

            if (Telefonos.Count == 0)
            { ViewData["Error"] = "Debe ingresar al menos un teléfono."; return Page(); }

            if (await _oferenteService.IdentificacionExisteAsync(Oferente.Identificacion))
            { ViewData["Error"] = "Ya existe un oferente con esa identificación."; return Page(); }

            Oferente.Correos = Correos;
            Oferente.Telefonos = Telefonos;
            Oferente.ConcursosIds = ConcursosSeleccionados;

            var nuevoId = await _oferenteService.InsertarAsync(Oferente);
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario,
                $"Crear Oferente: {JsonSerializer.Serialize(new { nuevoId, Oferente.Identificacion, Oferente.NombreCompleto })}");

            TempData["Mensaje"] = "El oferente ha sido registrado correctamente.";
            return RedirectToPage("Index");
        }
    }
}