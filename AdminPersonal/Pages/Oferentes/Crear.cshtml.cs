using AdminPersonal.Entities;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;

namespace AdminPersonal.Pages.Oferentes
{
    public class CrearModel : PageModel
    {
        private readonly IOferenteService _service;
        public List<Concurso> Concursos { get; set; } = new();
        public string Error { get; set; } = string.Empty;

        [BindProperty] public string Identificacion { get; set; } = string.Empty;
        [BindProperty] public string TipoIdentificacion { get; set; } = string.Empty;
        [BindProperty] public string NombreCompleto { get; set; } = string.Empty;
        [BindProperty] public DateTime FechaNacimiento { get; set; }
        [BindProperty] public List<string> Correos { get; set; } = new();
        [BindProperty] public List<string> Telefonos { get; set; } = new();
        [BindProperty] public List<int> ConcursosSeleccionados { get; set; } = new();

        public CrearModel(IOferenteService service)
        {
            _service = service;
        }

        public async Task OnGetAsync()
        {
            Concursos = (await _service.ObtenerConcursosAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Concursos = (await _service.ObtenerConcursosAsync()).ToList();

            var correosLimpios = Correos.Where(c => !string.IsNullOrWhiteSpace(c)).ToList();
            var telefonosLimpios = Telefonos.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();

            if (string.IsNullOrWhiteSpace(Identificacion) || string.IsNullOrWhiteSpace(TipoIdentificacion) ||
                string.IsNullOrWhiteSpace(NombreCompleto) || correosLimpios.Count == 0 ||
                telefonosLimpios.Count == 0 || ConcursosSeleccionados.Count == 0)
            {
                Error = "Todos los campos son obligatorios.";
                return Page();
            }

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            foreach (var correo in correosLimpios)
            {
                if (!emailRegex.IsMatch(correo))
                {
                    Error = $"El correo '{correo}' no tiene un formato válido.";
                    return Page();
                }
            }

            if (await _service.IdentificacionExisteAsync(Identificacion))
            {
                Error = "El número de identificación ya existe en el sistema.";
                return Page();
            }

            var oferente = new Oferente
            {
                Identificacion = Identificacion,
                TipoIdentificacion = TipoIdentificacion,
                NombreCompleto = NombreCompleto,
                FechaNacimiento = FechaNacimiento
            };

            await _service.InsertarAsync(oferente, correosLimpios, telefonosLimpios, ConcursosSeleccionados);
            TempData["Mensaje"] = "El oferente ha sido registrado correctamente.";
            return RedirectToPage("Index");
        }
    }
}