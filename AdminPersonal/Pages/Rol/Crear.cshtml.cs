using AdminPersonal.Services;
using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace AdminPersonal.Pages.Rol
{
    public class CrearModel : PageModel
    {
        private readonly IRolService _rolService;
        private readonly BitacoraService _bitacoraService;

        public CrearModel(IRolService rolService, BitacoraService bitacoraService)
        {
            _rolService = rolService;
            _bitacoraService = bitacoraService;
        }

        [BindProperty]
        public AdminPersonal.Entities.Rol Rol { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var error = await _rolService.ValidarYCrearAsync(Rol);

            if (error != null)
            {
                ViewData["Error"] = error;
                return Page();
            }


            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            await _bitacoraService.RegistrarAsync(idUsuario, "Creación: " + JsonSerializer.Serialize(Rol));
            TempData["Mensaje"] = "Rol creado exitosamente.";
            return RedirectToPage("Index");
        }
    }
}