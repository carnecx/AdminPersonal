using AdminPersonal.Entities;
using AdminPersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Usuarios
{
    // modelo de la pagina principal de usuarios
    public class IndexModel : PageModel
    {
        // servicio que contiene la logica de usuarios
        private readonly IUsuarioService _service;

        // lista que almacena los usuarios obtenidos de la base de datos
        public IEnumerable<Usuario> Usuarios { get; set; }
            = new List<Usuario>();

        // id del usuario que realiza la accion
        // se utiliza para registrar eventos en la bitacora
        private int IdSesion => 1;

        // constructor que recibe el servicio mediante inyeccion de dependencias
        public IndexModel(IUsuarioService service)
        {
            _service = service;
        }

        // se ejecuta cuando se abre la pantalla
        public async Task OnGetAsync()
        {
            // obtiene todos los usuarios registrados
            Usuarios = await _service.ObtenerTodosAsync();
        }

        // se ejecuta cuando se solicita cambiar el estado de un usuario
        public async Task<IActionResult> OnPostCambiarEstadoAsync(
            int id,
            string nuevoEstado)
        {
            // llama al servicio para actualizar el estado
            await _service.CambiarEstadoAsync(
                id,
                nuevoEstado,
                IdSesion);

            // mensaje de exito
            TempData["Exito"] =
                "Estado actualizado correctamente.";

            // recarga la pagina
            return RedirectToPage();
        }

        // se ejecuta cuando se solicita eliminar un usuario
        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            // intenta eliminar el usuario
            var (ok, mensaje) =
                await _service.EliminarAsync(
                    id,
                    IdSesion);

            // si se elimino correctamente
            if (ok)
                TempData["Exito"] = mensaje;

            // si existe alguna relacion o error
            else
                TempData["Error"] = mensaje;

            // recarga la pagina
            return RedirectToPage();
        }
    }
}