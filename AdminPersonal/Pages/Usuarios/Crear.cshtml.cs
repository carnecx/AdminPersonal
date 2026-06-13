using AdminPersonal.Entities;
using AdminPersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Usuarios
{
    // modelo de la pagina para crear usuarios
    public class CrearModel : PageModel
    {
        // servicio que contiene la logica de usuarios
        private readonly IUsuarioService _service;

        // id del usuario que realiza la accion
        // se utiliza para registrar la bitacora
        private int IdSesion => 1;

        // objeto que almacena los datos del formulario
        [BindProperty]
        public Usuario Usuario { get; set; } = new();

        // lista de roles disponibles para asignar al usuario
        public IEnumerable<AdminPersonal.Entities.Rol> Roles { get; set; }
            = new List<AdminPersonal.Entities.Rol>();

        // constructor que recibe el servicio mediante inyeccion de dependencias
        public CrearModel(IUsuarioService service)
        {
            _service = service;
        }

        // se ejecuta cuando se abre la pagina
        public async Task OnGetAsync()
        {
            // carga los roles para mostrarlos en pantalla
            Roles = await _service.ObtenerRolesAsync();
        }

        // se ejecuta cuando el usuario presiona guardar
        public async Task<IActionResult> OnPostAsync()
        {
            // valida que la contraseña no venga vacia
            if (string.IsNullOrWhiteSpace(Usuario.Contrasena))
                ModelState.AddModelError(
                    "Usuario.Contrasena",
                    "La contraseña es requerida.");

            // valida que se haya seleccionado al menos un rol
            if (Usuario.RolesSeleccionados == null ||
                !Usuario.RolesSeleccionados.Any())

                ModelState.AddModelError(
                    "",
                    "Debe seleccionar al menos un rol.");

            // si existen errores de validacion
            if (!ModelState.IsValid)
            {
                // vuelve a cargar la lista de roles
                Roles = await _service.ObtenerRolesAsync();

                // retorna a la misma pagina mostrando errores
                return Page();
            }

            try
            {
                // llama al servicio para crear el usuario
                await _service.CrearAsync(
                    Usuario,
                    IdSesion);

                // mensaje de exito
                TempData["Exito"] =
                    "Usuario creado correctamente.";

                // redirecciona al listado de usuarios
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                // muestra el error ocurrido
                ModelState.AddModelError(
                    "",
                    "Error al crear usuario: " + ex.Message);

                // vuelve a cargar los roles
                Roles = await _service.ObtenerRolesAsync();

                // retorna a la pagina actual
                return Page();
            }
        }
    }
}