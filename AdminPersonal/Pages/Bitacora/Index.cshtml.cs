using BitacoraModel = AdminPersonal.Entities.Bitacora;
using AdminPersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Bitacora
{
    // modelo de la pagina de bitacoras
    public class IndexModel : PageModel
    {
        // servicio que contiene la logica de bitacoras
        private readonly BitacoraService _bitacoraService;

        // constructor que recibe el servicio mediante inyeccion de dependencias
        public IndexModel(BitacoraService bitacoraService)
        {
            _bitacoraService = bitacoraService;
        }

        // filtro por usuario
        [BindProperty(SupportsGet = true)]
        public string? Usuario { get; set; }

        // filtro por descripcion
        [BindProperty(SupportsGet = true)]
        public string? Descripcion { get; set; }

        // tipo de ordenamiento seleccionado
        [BindProperty(SupportsGet = true)]
        public string Orden { get; set; } = "fecha_desc";

        // numero de pagina actual
        [BindProperty(SupportsGet = true)]
        public int Pagina { get; set; } = 1;

        // total de paginas calculadas
        public int TotalPaginas { get; set; }

        // lista de bitacoras que se mostraran en pantalla
        public IEnumerable<BitacoraModel> Lista { get; set; }
            = new List<BitacoraModel>();

        // se ejecuta cuando se abre la pagina
        public async Task<IActionResult> OnGetAsync()
        {
            // valida que exista una sesion activa
            if (HttpContext.Session.GetInt32("IdUsuario") == null)

                // si no hay sesion redirige al login
                return RedirectToPage(
                    "/Account/Login",
                    new
                    {
                        mensaje = "Por favor inicie sesión para utilizar el sistema"
                    });

            // cantidad de registros que se mostraran por pagina
            const int tamanoPagina = 10;

            // evita que el numero de pagina sea menor a 1
            if (Pagina < 1)
                Pagina = 1;

            // obtiene la cantidad total de registros segun filtros
            int total = await _bitacoraService.ContarAsync(
                Usuario,
                Descripcion);

            // calcula el total de paginas necesarias
            TotalPaginas = Math.Max(
                1,
                (int)Math.Ceiling(total / (double)tamanoPagina));

            // obtiene la lista paginada de bitacoras
            Lista = await _bitacoraService.ListarAsync(
                Usuario,
                Descripcion,
                Orden,
                Pagina,
                tamanoPagina);

            // retorna la pagina con los datos cargados
            return Page();
        }
    }
}