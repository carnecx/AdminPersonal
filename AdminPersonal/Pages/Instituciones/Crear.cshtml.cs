using AdminPersonal.Entities;
using AdminPersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Instituciones
{
    public class CrearModel : PageModel
    {
        private readonly InstitucionService _institucionService;
        private readonly BitacoraService _bitacoraService;

        public CrearModel(InstitucionService institucionService, BitacoraService bitacoraService)
        {
            _institucionService = institucionService;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public InstitucionEducativa Datos { get; set; } = new();

        //seguridad
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

            if (!ModelState.IsValid)
                return Page();

            await _institucionService.InsertarAsync(Datos);
            //crea la nueva institucion

            int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 1;
            await _bitacoraService.RegistrarAsync(idUsuario, "Nueva institucion educativa: " + JsonSerializer.Serialize(Datos));

            TempData["Mensaje"] = "Institucion guardada correctamente.";
            return RedirectToPage("/Instituciones/Index");
        }
    }
}
