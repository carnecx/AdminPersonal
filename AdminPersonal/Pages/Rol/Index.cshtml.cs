using AdminPersonal.Services.Abstract;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPersonal.Pages.Rol
{
    public class IndexModel : PageModel
    {
        private readonly IRolService _rolService;

        public IndexModel(IRolService rolService)
        {
            _rolService = rolService;
            Roles = new List<AdminPersonal.Entities.Rol>();
        }

        public IEnumerable<AdminPersonal.Entities.Rol> Roles { get; set; }

        public async Task OnGetAsync()
        {
            Roles = await _rolService.ObtenerTodosAsync();
        }
    }
}