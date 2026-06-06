using AdminPersonal.Entities;
using AdminPersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Usuarios
{
    public class IndexModel : PageModel
    {
        private readonly IUsuarioService _service;
        public IEnumerable<Usuario> Usuarios { get; set; } = new List<Usuario>();
        private int IdSesion => 1;

        public IndexModel(IUsuarioService service)
        {
            _service = service;
        }

        public async Task OnGetAsync()
        {
            Usuarios = await _service.ObtenerTodosAsync();
        }

        public async Task<IActionResult> OnPostCambiarEstadoAsync(int id, string nuevoEstado)
        {
            await _service.CambiarEstadoAsync(id, nuevoEstado, IdSesion);
            TempData["Exito"] = "Estado actualizado correctamente.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var (ok, mensaje) = await _service.EliminarAsync(id, IdSesion);
            if (ok) TempData["Exito"] = mensaje;
            else TempData["Error"] = mensaje;
            return RedirectToPage();
        }
    }
}