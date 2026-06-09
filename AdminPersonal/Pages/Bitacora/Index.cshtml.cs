using BitacoraModel = AdminPersonal.Entities.Bitacora;
using AdminPersonal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace AdminPersonal.Pages.Bitacora
{
    public class IndexModel : PageModel
    {
        private readonly BitacoraService _bitacoraService;

        public IndexModel(BitacoraService bitacoraService)
        {
            _bitacoraService = bitacoraService;
        }

        [BindProperty(SupportsGet = true)] public string? Usuario { get; set; }
        [BindProperty(SupportsGet = true)] public string? Descripcion { get; set; }
        [BindProperty(SupportsGet = true)] public string Orden { get; set; } = "fecha_desc";
        [BindProperty(SupportsGet = true)] public int Pagina { get; set; } = 1;

        public int TotalPaginas { get; set; }
        public IEnumerable<BitacoraModel> Lista { get; set; } = new List<BitacoraModel>();

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetInt32("IdUsuario") == null)
                return RedirectToPage("/Account/Login", new { mensaje = "Por favor inicie sesión para utilizar el sistema" });

            const int tamanoPagina = 10;
            if (Pagina < 1) Pagina = 1;

            int total = await _bitacoraService.ContarAsync(Usuario, Descripcion);
            TotalPaginas = Math.Max(1, (int)Math.Ceiling(total / (double)tamanoPagina));
            Lista = await _bitacoraService.ListarAsync(Usuario, Descripcion, Orden, Pagina, tamanoPagina);
            return Page();
        }
    }
}
