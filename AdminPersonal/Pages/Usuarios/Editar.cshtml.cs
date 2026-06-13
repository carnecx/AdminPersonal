using AdminPersonal.Entities;
using AdminPersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Usuarios
{
    // modelo de la pagina para editar usuarios
    public class EditarModel : PageModel
    {
        // servicio que contiene la logica de usuarios
        private readonly IUsuarioService _service;

        // id del usuario que realiza la accion
        // se utiliza para registrar cambios en la bitacora
        private int IdSesion => 1;

        // objeto que almacena los datos del usuario a editar
        [BindProperty]
        public Usuario Usuario { get; set; } = new();

        // lista de roles disponibles para asignar al usuario
        public IEnumerable<AdminPersonal.Entities.Rol> Roles { get; set; }
            = new List<AdminPersonal.Entities.Rol>();

        // constructor que recibe el servicio mediante inyeccion de dependencias
        public EditarModel(IUsuarioService service)
        {
            _service = service;
        }

        // se ejecuta cuando se abre la pantalla de editar
        public async Task<IActionResult> OnGetAsync(int id)
        {
            // busca el usuario segun el id recibido
            var usuario = await _service.ObtenerPorIdAsync(id);

            // si no existe retorna error 404
            if (usuario == null)
                return NotFound();

            // carga los datos del usuario en el formulario
            Usuario = usuario;

            // obtiene la lista de roles para mostrar en pantalla
            Roles = await _service.ObtenerRolesAsync();

            return Page();
        }

        // se ejecuta cuando el usuario presiona guardar
        public async Task<IActionResult> OnPostAsync()
        {
            // valida que se haya seleccionado al menos un rol
            if (Usuario.RolesSeleccionados == null ||
                !Usuario.RolesSeleccionados.Any())

                ModelState.AddModelError(
                    "",
                    "Debe seleccionar al menos un rol.");

            // si existen errores de validacion
            if (!ModelState.IsValid)
            {
                // vuelve a cargar los roles
                Roles = await _service.ObtenerRolesAsync();

                // retorna a la misma pagina mostrando errores
                return Page();
            }

            try
            {
                // llama al servicio para actualizar el usuario
                await _service.ActualizarAsync(
                    Usuario,
                    IdSesion);

                // mensaje de exito
                TempData["Exito"] =
                    "Usuario actualizado correctamente.";

                // redirecciona al listado de usuarios
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                // muestra el error ocurrido
                ModelState.AddModelError(
                    "",
                    "Error al actualizar: " + ex.Message);

                // vuelve a cargar los roles
                Roles = await _service.ObtenerRolesAsync();

                // retorna a la pagina actual
                return Page();
            }
        }
    }
}