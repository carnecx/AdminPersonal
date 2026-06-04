using AdminPersonal.Entities;
using AdminPersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Ubicaciones
{
    public class CargarModel : PageModel
    {
        private readonly UbicacionService _ubicacionService;
        private readonly BitacoraService _bitacoraService;

        public CargarModel(UbicacionService ubicacionService, BitacoraService bitacoraService)
        {
            _ubicacionService = ubicacionService;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public UbicacionUploadViewModel Datos { get; set; } = new();

        public string? Mensaje { get; set; }
        public string? Error { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("IdUsuario") == null)
                return RedirectToPage("/Account/Login", new { mensaje = "Por favor inicie sesión para utilizar el sistema" });

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (HttpContext.Session.GetInt32("IdUsuario") == null)
                return RedirectToPage("/Account/Login", new { mensaje = "Por favor inicie sesión para utilizar el sistema" });

            if (Datos.Archivo == null || Datos.Archivo.Length == 0)
            {
                Error = "Debe seleccionar un archivo.";
                return Page();
            }

            int contador = await _ubicacionService.CargarCsvAsync(Datos.Archivo.OpenReadStream());

            int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 1;
            await _bitacoraService.RegistrarAsync(idUsuario, "El usuario realizo la carga de informacion de ubicacion");

            Mensaje = $"Carga realizada correctamente. Registros procesados: {contador}";
            return Page();
        }
    }
}
