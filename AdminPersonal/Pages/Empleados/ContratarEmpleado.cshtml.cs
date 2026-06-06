using AdminPersonal.Entities;
using AdminPersonal.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Empleados
{
    public class ContratarEmpleadoModel : PageModel
    {
        private readonly EmpleadoRepository _empleadoRepo;
        private readonly string _connStr;

        public ContratarEmpleadoModel(IConfiguration configuration)
        {
            _connStr = configuration.GetConnectionString("DefaultConnection")!;
            _empleadoRepo = new EmpleadoRepository(_connStr);
        }

        public List<Oferente> Oferentes { get; set; } = new();
        public List<Puesto> Puestos { get; set; } = new();

        [BindProperty]
        public int IdOferenteSeleccionado { get; set; }

        [BindProperty]
        public int IdPuestoSeleccionado { get; set; }

        public string? MensajeExito { get; set; }
        public string? MensajeError { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Verificar sesión
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null) return RedirectToPage("/Login");

            Oferentes = await _empleadoRepo.ObtenerOferentesDisponiblesAsync();
            Puestos = await _empleadoRepo.ObtenerPuestosAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null) return RedirectToPage("/Login");

            Oferentes = await _empleadoRepo.ObtenerOferentesDisponiblesAsync();
            Puestos = await _empleadoRepo.ObtenerPuestosAsync();

            if (IdOferenteSeleccionado == 0 || IdPuestoSeleccionado == 0)
            {
                MensajeError = "Debe seleccionar un oferente y un puesto.";
                return Page();
            }

            try
            {
                var numeroEmpleado = await _empleadoRepo.GenerarNumeroEmpleadoAsync();
                await _empleadoRepo.ContratarEmpleadoAsync(IdOferenteSeleccionado, IdPuestoSeleccionado, numeroEmpleado);

                // Bitácora
                var oferente = Oferentes.FirstOrDefault(o => o.id_oferente == IdOferenteSeleccionado);
                var puesto = Puestos.FirstOrDefault(p => p.IdPuesto == IdPuestoSeleccionado);
                var detalle = new
                {
                    accion = "Contratación de empleado",
                    numeroEmpleado,
                    oferente = oferente?.NombreCompleto,
                    identificacion = oferente?.Identificacion,
                    puesto = puesto?.Nombre
                };
                await _empleadoRepo.RegistrarBitacoraAsync(
                    idUsuario.Value,
                    JsonSerializer.Serialize(detalle)
                );

                MensajeExito = "Empleado creado con éxito";
              
                Oferentes = await _empleadoRepo.ObtenerOferentesDisponiblesAsync();
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al contratar el empleado: {ex.Message}";
            }

            return Page();
        }
    }
}