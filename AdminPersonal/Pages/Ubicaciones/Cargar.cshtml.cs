using AdminPersonal.Entities;
using AdminPersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Ubicaciones
{
    // modelo de la pagina para cargar provincias, cantones y distritos desde un csv
    public class CargarModel : PageModel
    {
        // servicio que procesa la carga de ubicaciones
        private readonly UbicacionService _ubicacionService;

        // servicio que registra acciones en la bitacora
        private readonly BitacoraService _bitacoraService;

        // constructor que recibe los servicios mediante inyeccion de dependencias
        public CargarModel(UbicacionService ubicacionService, BitacoraService bitacoraService)
        {
            _ubicacionService = ubicacionService;
            _bitacoraService = bitacoraService;
        }

        // modelo que contiene el archivo csv seleccionado por el usuario
        [BindProperty]
        public UbicacionUploadViewModel Datos { get; set; } = new();

        // mensaje de exito mostrado en pantalla
        public string? Mensaje { get; set; }

        // mensaje de error mostrado en pantalla
        public string? Error { get; set; }

        // se ejecuta cuando se abre la pagina
        public IActionResult OnGet()
        {
            // valida que exista una sesion activa
            if (HttpContext.Session.GetInt32("IdUsuario") == null)

                // si no hay sesion redirige al login
                return RedirectToPage("/Account/Login",
                    new { mensaje = "Por favor inicie sesión para utilizar el sistema" });

            // muestra la pagina normalmente
            return Page();
        }

        // se ejecuta cuando el usuario presiona guardar
        public async Task<IActionResult> OnPostAsync()
        {
            // valida que exista una sesion activa
            if (HttpContext.Session.GetInt32("IdUsuario") == null)

                // si no hay sesion redirige al login
                return RedirectToPage("/Account/Login",
                    new { mensaje = "Por favor inicie sesión para utilizar el sistema" });

            try
            {
                // envia el archivo al servicio
                // las validaciones del archivo se realizan en UbicacionService
                int contador = await _ubicacionService.CargarCsvAsync(
                    Datos.Archivo!.OpenReadStream(),
                    Datos.Archivo.FileName);

                // obtiene el usuario que realiza la accion
                int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 1;

                // registra la accion en la bitacora del sistema
                await _bitacoraService.RegistrarAsync(
                    idUsuario,
                    "El usuario realizo la carga de informacion de ubicacion");

                // muestra la cantidad de registros procesados
                Mensaje = $"Carga realizada correctamente. Registros procesados: {contador}";
            }
            catch (Exception ex)
            {
                // muestra el error enviado por el servicio
                Error = ex.Message;
            }

            // retorna nuevamente la pagina
            return Page();
        }
    }
}