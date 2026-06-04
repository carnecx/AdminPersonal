using AdminPersonal.Entities;
using AdminPersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Instituciones
{
    public class EditarModel : PageModel
    {
        private readonly InstitucionService _institucionService;
        private readonly BitacoraService _bitacoraService;

        public EditarModel(InstitucionService institucionService, BitacoraService bitacoraService)
        {
            _institucionService = institucionService;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public InstitucionEducativa Datos { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (HttpContext.Session.GetInt32("IdUsuario") == null)
                return RedirectToPage("/Account/Login", new { mensaje = "Por favor inicie sesión para utilizar el sistema" });

            var item = await _institucionService.ObtenerPorIdAsync(id);
            if (item == null) return NotFound();

            Datos = item;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (HttpContext.Session.GetInt32("IdUsuario") == null)
                return RedirectToPage("/Account/Login", new { mensaje = "Por favor inicie sesión para utilizar el sistema" });

            if (!ModelState.IsValid)
                return Page();

            var anterior = await _institucionService.ObtenerPorIdAsync(Datos.id_institucion);
            await _institucionService.ActualizarAsync(Datos);

            int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 1;
            await _bitacoraService.RegistrarAsync(idUsuario,
                "Actualizacion institucion. Antes: " + JsonSerializer.Serialize(anterior) +
                " Ahora: " + JsonSerializer.Serialize(Datos));

            TempData["Mensaje"] = "Institucion actualizada correctamente.";
            return RedirectToPage("/Instituciones/Index");
        }
    }
}
